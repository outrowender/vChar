using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;
using static vCharClient.CommonFunctions;
using static vCharShared.ConfigManager;
using static vCharShared.PermissionsManager;

namespace vCharClient
{
    public class MainMenu : BaseScript
    {
        #region Variables
        //public static MenuPool Mp { get; } = new MenuPool();

        private bool firstTick = true;
        public static bool PermissionsSetupComplete => ArePermissionsSetup;
        public static bool ConfigOptionsSetupComplete = false;

        public static Control MenuToggleKey { get { return MenuController.MenuToggleKey; } private set { MenuController.MenuToggleKey = value; } } // M by default (InteractionMenu)
        public static int NoClipKey { get; private set; } = 289; // F2 by default (ReplayStartStopRecordingSecondary)
        public static Menu Menu { get; private set; }
        public static Menu PlayerSubmenu { get; private set; }
        public static Menu VehicleSubmenu { get; private set; }
        public static Menu WorldSubmenu { get; private set; }

        public static PlayerOptions PlayerOptionsMenu { get; private set; }
        public static OnlinePlayers OnlinePlayersMenu { get; private set; }
        public static BannedPlayers BannedPlayersMenu { get; private set; }
        public static SavedVehicles SavedVehiclesMenu { get; private set; }
        public static PersonalVehicle PersonalVehicleMenu { get; private set; }
        public static VehicleOptions VehicleOptionsMenu { get; private set; }
        public static VehicleSpawner VehicleSpawnerMenu { get; private set; }
        public static PlayerAppearance PlayerAppearanceMenu { get; private set; }
        public static MpPedCustomization MpPedCustomizationMenu { get; private set; }
        public static TimeOptions TimeOptionsMenu { get; private set; }
        public static WeatherOptions WeatherOptionsMenu { get; private set; }
        public static WeaponOptions WeaponOptionsMenu { get; private set; }
        public static WeaponLoadouts WeaponLoadoutsMenu { get; private set; }
        public static Recording RecordingMenu { get; private set; }
        public static MiscSettings MiscSettingsMenu { get; private set; }
        public static VoiceChat VoiceChatSettingsMenu { get; private set; }
        public static About AboutMenu { get; private set; }
        public static bool NoClipEnabled { get { return NoClip.IsNoclipActive(); } set { NoClip.SetNoclipActive(value); } }
        public static PlayerList PlayersList;

        // Only used when debugging is enabled:
        //private BarTimerBar bt = new BarTimerBar("Opening Menu");

        public static bool DebugMode = GetResourceMetadata(GetCurrentResourceName(), "client_debug_mode", 0) == "true" ? true : false;
        public static bool EnableExperimentalFeatures = /*true;*/ (GetResourceMetadata(GetCurrentResourceName(), "experimental_features_enabled", 0) ?? "0") == "1";
        public static string Version { get { return GetResourceMetadata(GetCurrentResourceName(), "version", 0); } }

        public static bool DontOpenMenus { get { return MenuController.DontOpenAnyMenu; } set { MenuController.DontOpenAnyMenu = value; } }
        public static bool DisableControls { get { return MenuController.DisableMenuButtons; } set { MenuController.DisableMenuButtons = value; } }

        private const int currentCleanupVersion = 2;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainMenu()
        {
            PlayersList = Players;

            #region cleanup unused kvps
            int tmp_kvp_handle = StartFindKvp("");
            bool cleanupVersionChecked = false;
            List<string> tmp_kvp_names = new List<string>();
            while (true)
            {
                string k = FindKvp(tmp_kvp_handle);
                if (string.IsNullOrEmpty(k))
                {
                    break;
                }
                if (k == "vmenu_cleanup_version")
                {
                    if (GetResourceKvpInt("vmenu_cleanup_version") >= currentCleanupVersion)
                    {
                        cleanupVersionChecked = true;
                    }
                }
                tmp_kvp_names.Add(k);
            }
            EndFindKvp(tmp_kvp_handle);

            if (!cleanupVersionChecked)
            {
                SetResourceKvpInt("vmenu_cleanup_version", currentCleanupVersion);
                foreach (string kvp in tmp_kvp_names)
                {
                    if (currentCleanupVersion == 1 || currentCleanupVersion == 2)
                    {
                        if (!kvp.StartsWith("settings_") && !kvp.StartsWith("vmenu") && !kvp.StartsWith("veh_") && !kvp.StartsWith("ped_") && !kvp.StartsWith("mp_ped_"))
                        {
                            DeleteResourceKvp(kvp);
                            Debug.WriteLine($"[vChar] [cleanup id: 1] Removed unused (old) KVP: {kvp}.");
                        }
                    }
                    if (currentCleanupVersion == 2)
                    {
                        if (kvp.StartsWith("mp_char"))
                        {
                            DeleteResourceKvp(kvp);
                            Debug.WriteLine($"[vChar] [cleanup id: 2] Removed unused (old) KVP: {kvp}.");
                        }
                    }
                }
                Debug.WriteLine("[vChar] Cleanup of old unused KVP items completed.");
            }
            #endregion

            if (EnableExperimentalFeatures)
            {
                RegisterCommand("testped", new Action<dynamic, List<dynamic>, string>((dynamic source, List<dynamic> args, string rawCommand) =>
                {
                    PedHeadBlendData data = Game.PlayerPed.GetHeadBlendData();
                    Debug.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                }), false);

                RegisterCommand("tattoo", new Action<dynamic, List<dynamic>, string>((dynamic source, List<dynamic> args, string rawCommand) =>
                {
                    if (args != null && args[0] != null && args[1] != null)
                    {
                        Debug.WriteLine(args[0].ToString() + " " + args[1].ToString());
                        TattooCollectionData d = Game.GetTattooCollectionData(int.Parse(args[0].ToString()), int.Parse(args[1].ToString()));
                        Debug.WriteLine("check");
                        Debug.Write(JsonConvert.SerializeObject(d, Formatting.Indented) + "\n");
                    }
                }), false);

                RegisterCommand("clearfocus", new Action<dynamic, List<dynamic>, string>((dynamic source, List<dynamic> args, string rawCommand) =>
                {
                    SetNuiFocus(false, false);
                }), false);
            }

            RegisterCommand("vmenuclient", new Action<dynamic, List<dynamic>, string>((dynamic source, List<dynamic> args, string rawCommand) =>
            {
                if (args != null)
                {
                    if (args.Count > 0)
                    {
                        if (args[0].ToString().ToLower() == "debug")
                        {
                            DebugMode = !DebugMode;
                            Notify.Custom($"Debug mode is now set to: {DebugMode}.");
                            // Set discord rich precense once, allowing it to be overruled by other resources once those load.
                            if (DebugMode)
                            {
                                SetRichPresence($"Debugging vChar {Version}!");
                            }
                            else
                            {
                                SetRichPresence($"Enjoying FiveM!");
                            }
                        }
                        else if (args[0].ToString().ToLower() == "gc")
                        {
                            GC.Collect();
                            Debug.Write("Cleared memory.\n");
                        }
                        else if (args[0].ToString().ToLower() == "dump")
                        {
                            Notify.Info("A full config dump will be made to the console. Check the log file. This can cause lag!");
                            Debug.WriteLine("\n\n\n########################### vChar ###########################");
                            Debug.WriteLine($"Running vChar Version: {Version}, Experimental features: {EnableExperimentalFeatures}, Debug mode: {DebugMode}.");
                            Debug.WriteLine("\nDumping a list of all KVPs:");
                            int handle = StartFindKvp("");
                            List<string> names = new List<string>();
                            while (true)
                            {
                                string k = FindKvp(handle);
                                if (string.IsNullOrEmpty(k))
                                {
                                    break;
                                }
                                //if (!k.StartsWith("settings_") && !k.StartsWith("vmenu") && !k.StartsWith("veh_") && !k.StartsWith("ped_") && !k.StartsWith("mp_ped_"))
                                //{
                                //    DeleteResourceKvp(k);
                                //}
                                names.Add(k);
                            }
                            EndFindKvp(handle);

                            Dictionary<string, dynamic> kvps = new Dictionary<string, dynamic>();
                            foreach (var kvp in names)
                            {
                                int type = 0; // 0 = string, 1 = float, 2 = int.
                                if (kvp.StartsWith("settings_"))
                                {
                                    if (kvp == "settings_voiceChatProximity") // float
                                    {
                                        type = 1;
                                    }
                                    else if (kvp == "settings_clothingAnimationType") // int
                                    {
                                        type = 2;
                                    }
                                    else if (kvp == "settings_miscLastTimeCycleModifierIndex") // int
                                    {
                                        type = 2;
                                    }
                                    else if (kvp == "settings_miscLastTimeCycleModifierStrength") // int
                                    {
                                        type = 2;
                                    }
                                }
                                else if (kvp == "vmenu_cleanup_version") // int
                                {
                                    type = 2;
                                }
                                switch (type)
                                {
                                    case 0:
                                        var s = GetResourceKvpString(kvp);
                                        if (s.StartsWith("{") || s.StartsWith("["))
                                        {
                                            kvps.Add(kvp, JsonConvert.DeserializeObject(s));
                                        }
                                        else
                                        {
                                            kvps.Add(kvp, GetResourceKvpString(kvp));
                                        }
                                        break;
                                    case 1:
                                        kvps.Add(kvp, GetResourceKvpFloat(kvp));
                                        break;
                                    case 2:
                                        kvps.Add(kvp, GetResourceKvpInt(kvp));
                                        break;
                                }
                            }
                            Debug.WriteLine(@JsonConvert.SerializeObject(kvps, Formatting.None) + "\n");

                            Debug.WriteLine("\n\nDumping a list of allowed permissions:");
                            Debug.WriteLine(@JsonConvert.SerializeObject(Permissions, Formatting.None));

                            Debug.WriteLine("\n\nDumping vmenu server configuration settings:");
                            var settings = new Dictionary<string, string>();
                            foreach (var a in Enum.GetValues(typeof(Setting)))
                            {
                                settings.Add(a.ToString(), GetSettingsString((Setting)a));
                            }
                            Debug.WriteLine(@JsonConvert.SerializeObject(settings, Formatting.None));
                            Debug.WriteLine("\nEnd of vChar dump!");
                            Debug.WriteLine("\n########################### vChar ###########################");
                        }
                    }
                    else
                    {
                        Notify.Custom($"vChar is currently running version: {Version}.");
                    }
                }
            }), false);

            // Set discord rich precense once, allowing it to be overruled by other resources once those load.
            if (DebugMode)
            {
                SetRichPresence($"Debugging vChar {Version}!");
            }

            if (GetCurrentResourceName() != "vChar")
            {
                Exception InvalidNameException = new Exception("\r\n\r\n[vChar] INSTALLATION ERROR!\r\nThe name of the resource is not valid. Please change the folder name from '" + GetCurrentResourceName() + "' to 'vChar' (case sensitive) instead!\r\n\r\n\r\n");
                try
                {
                    throw InvalidNameException;
                }
                catch (Exception e)
                {
                    Log(e.Message);
                }
                TriggerEvent("chatMessage", "^3IMPORTANT: vChar IS NOT SETUP CORRECTLY. PLEASE CHECK THE SERVER LOG FOR MORE INFO.");
                MenuController.MainMenu = null;
                MenuController.DontOpenAnyMenu = true;
                MenuController.DisableMenuButtons = true;
            }
            else
            {
                Tick += OnTick;
            }
            try
            {
                SetClockDate(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            }
            catch (InvalidTimeZoneException timeEx)
            {
                Debug.WriteLine($"[vChar] [Error] Could not set the in-game day, month and year because of an invalid timezone(?).");
                Debug.WriteLine($"[vChar] [Error] InvalidTimeZoneException: {timeEx.Message}");
                Debug.WriteLine($"[vChar] [Error] vChar will continue to work normally.");
            }
        }

        #region Set Permissions function
        /// <summary>
        /// Set the permissions for this client.
        /// </summary>
        /// <param name="dict"></param>
        public static void SetPermissions(string permissionsList)
        {
            vCharShared.PermissionsManager.SetPermissions(permissionsList);

            VehicleSpawner.allowedCategories = new List<bool>()
            {
                IsAllowed(Permission.VSCompacts, checkAnyway: true),
                IsAllowed(Permission.VSSedans, checkAnyway: true),
                IsAllowed(Permission.VSSUVs, checkAnyway: true),
                IsAllowed(Permission.VSCoupes, checkAnyway: true),
                IsAllowed(Permission.VSMuscle, checkAnyway: true),
                IsAllowed(Permission.VSSportsClassic, checkAnyway: true),
                IsAllowed(Permission.VSSports, checkAnyway: true),
                IsAllowed(Permission.VSSuper, checkAnyway: true),
                IsAllowed(Permission.VSMotorcycles, checkAnyway: true),
                IsAllowed(Permission.VSOffRoad, checkAnyway: true),
                IsAllowed(Permission.VSIndustrial, checkAnyway: true),
                IsAllowed(Permission.VSUtility, checkAnyway: true),
                IsAllowed(Permission.VSVans, checkAnyway: true),
                IsAllowed(Permission.VSCycles, checkAnyway: true),
                IsAllowed(Permission.VSBoats, checkAnyway: true),
                IsAllowed(Permission.VSHelicopters, checkAnyway: true),
                IsAllowed(Permission.VSPlanes, checkAnyway: true),
                IsAllowed(Permission.VSService, checkAnyway: true),
                IsAllowed(Permission.VSEmergency, checkAnyway: true),
                IsAllowed(Permission.VSMilitary, checkAnyway: true),
                IsAllowed(Permission.VSCommercial, checkAnyway: true),
                IsAllowed(Permission.VSTrains, checkAnyway: true),
                IsAllowed(Permission.VSOpenWheel, checkAnyway: true)
            };
            ArePermissionsSetup = true;

            TriggerServerEvent("vChar:IsResourceUpToDate");
        }
        #endregion


        /// <summary>
        /// Main OnTick task runs every game tick and handles all the menu stuff.
        /// </summary>
        /// <returns></returns>
        private async Task OnTick()
        {
            #region FirstTick
            // Only run this the first tick.
            if (firstTick)
            {
                firstTick = false;
                switch (GetSettingsInt(Setting.vmenu_pvp_mode))
                {
                    case 1:
                        NetworkSetFriendlyFireOption(true);
                        SetCanAttackFriendly(Game.PlayerPed.Handle, true, false);
                        break;
                    case 2:
                        NetworkSetFriendlyFireOption(false);
                        SetCanAttackFriendly(Game.PlayerPed.Handle, false, false);
                        break;
                    case 0:
                    default:
                        break;
                }
                // Clear all previous pause menu info/brief messages on resource start.
                ClearBrief();

                // Request the permissions data from the server.
                TriggerServerEvent("vChar:RequestPermissions");

                // Wait until the data is received and the player's name is loaded correctly.
                while (!ConfigOptionsSetupComplete || !PermissionsSetupComplete || Game.Player.Name == "**Invalid**" || Game.Player.Name == "** Invalid **")
                {
                    await Delay(0);
                }
                if ((IsAllowed(Permission.Staff) && GetSettingsBool(Setting.vmenu_menu_staff_only)) || GetSettingsBool(Setting.vmenu_menu_staff_only) == false)
                {
                    if (GetSettingsInt(Setting.vmenu_menu_toggle_key) != -1)
                    {
                        MenuToggleKey = (Control)GetSettingsInt(Setting.vmenu_menu_toggle_key);
                        //MenuToggleKey = GetSettingsInt(Setting.vmenu_menu_toggle_key);
                    }
                    if (GetSettingsInt(Setting.vmenu_noclip_toggle_key) != -1)
                    {
                        NoClipKey = GetSettingsInt(Setting.vmenu_noclip_toggle_key);
                    }

                    // Create the main menu.
                    Menu = new Menu(Game.Player.Name, "Main Menu");
                    PlayerSubmenu = new Menu(Game.Player.Name, "Player Related Options");
                    VehicleSubmenu = new Menu(Game.Player.Name, "Vehicle Related Options");
                    WorldSubmenu = new Menu(Game.Player.Name, "World Options");

                    // Add the main menu to the menu pool.
                    MenuController.AddMenu(Menu);
                    MenuController.MainMenu = Menu;

                    MenuController.AddSubmenu(Menu, PlayerSubmenu);
                    MenuController.AddSubmenu(Menu, VehicleSubmenu);
                    MenuController.AddSubmenu(Menu, WorldSubmenu);

                    // Create all (sub)menus.
                    CreateSubmenus();
                }
                else
                {
                    MenuController.MainMenu = null;
                    MenuController.DisableMenuButtons = true;
                    MenuController.DontOpenAnyMenu = true;
                    MenuController.MenuToggleKey = (Control)(-1); // disables the menu toggle key
                }

                // Manage Stamina
                if (PlayerOptionsMenu != null && PlayerOptionsMenu.PlayerStamina && IsAllowed(Permission.POUnlimitedStamina))
                    StatSetInt((uint)GetHashKey("MP0_STAMINA"), 100, true);
                else
                    StatSetInt((uint)GetHashKey("MP0_STAMINA"), 0, true);

                // Manage other stats, in order of appearance in the pause menu (stats) page.
                StatSetInt((uint)GetHashKey("MP0_SHOOTING_ABILITY"), 100, true);        // Shooting
                StatSetInt((uint)GetHashKey("MP0_STRENGTH"), 100, true);                // Strength
                StatSetInt((uint)GetHashKey("MP0_STEALTH_ABILITY"), 100, true);         // Stealth
                StatSetInt((uint)GetHashKey("MP0_FLYING_ABILITY"), 100, true);          // Flying
                StatSetInt((uint)GetHashKey("MP0_WHEELIE_ABILITY"), 100, true);         // Driving
                StatSetInt((uint)GetHashKey("MP0_LUNG_CAPACITY"), 100, true);           // Lung Capacity
                StatSetFloat((uint)GetHashKey("MP0_PLAYER_MENTAL_STATE"), 0f, true);    // Mental State

            }
            #endregion


            // If the setup (permissions) is done, and it's not the first tick, then do this:
            if (ConfigOptionsSetupComplete && !firstTick)
            {
                #region Handle Opening/Closing of the menu.


                var tmpMenu = GetOpenMenu();
                if (MpPedCustomizationMenu != null)
                {
                    bool IsOpen()
                    {
                        return
                            MpPedCustomizationMenu.appearanceMenu.Visible ||
                            MpPedCustomizationMenu.faceShapeMenu.Visible ||
                            MpPedCustomizationMenu.createCharacterMenu.Visible ||
                            MpPedCustomizationMenu.inheritanceMenu.Visible ||
                            MpPedCustomizationMenu.propsMenu.Visible ||
                            MpPedCustomizationMenu.clothesMenu.Visible ||
                            MpPedCustomizationMenu.tattoosMenu.Visible;
                    }

                    if (IsOpen())
                    {
                        if (tmpMenu == MpPedCustomizationMenu.createCharacterMenu)
                        {
                            MpPedCustomization.DisableBackButton = true;
                        }
                        else
                        {
                            MpPedCustomization.DisableBackButton = false;
                        }
                        MpPedCustomization.DontCloseMenus = true;
                    }
                    else
                    {
                        MpPedCustomization.DisableBackButton = false;
                        MpPedCustomization.DontCloseMenus = false;
                    }
                }

                if (Game.IsDisabledControlJustReleased(0, Control.PhoneCancel) && MpPedCustomization.DisableBackButton)
                {
                    await Delay(0);
                    Notify.Alert("You must save your ped first before exiting, or click the ~r~Exit Without Saving~s~ button.");
                }

                if (Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                {
                    if (Game.IsControlJustPressed(0, (Control)NoClipKey) && IsAllowed(Permission.NoClip) && UpdateOnscreenKeyboard() != 0)
                    {
                        if (Game.PlayerPed.IsInVehicle())
                        {
                            Vehicle veh = GetVehicle();
                            if (veh != null && veh.Exists() && veh.Driver == Game.PlayerPed)
                            {
                                NoClipEnabled = !NoClipEnabled;
                            }
                            else
                            {
                                NoClipEnabled = false;
                                Notify.Error("This vehicle does not exist (somehow) or you need to be the driver of this vehicle to enable noclip!");
                            }
                        }
                        else
                        {
                            NoClipEnabled = !NoClipEnabled;
                        }
                    }
                }

                #endregion

                // Menu toggle button.
                Game.DisableControlThisFrame(0, MenuToggleKey);
            }
        }

        #region Add Menu Function
        /// <summary>
        /// Add the menu to the menu pool and set it up correctly.
        /// Also add and bind the menu buttons.
        /// </summary>
        /// <param name="submenu"></param>
        /// <param name="menuButton"></param>
        private void AddMenu(Menu parentMenu, Menu submenu, MenuItem menuButton)
        {
            parentMenu.AddMenuItem(menuButton);
            MenuController.AddSubmenu(parentMenu, submenu);
            MenuController.BindMenuItem(parentMenu, submenu, menuButton);
            submenu.RefreshIndex();
        }
        #endregion

        #region Create Submenus
        /// <summary>
        /// Creates all the submenus depending on the permissions of the user.
        /// </summary>
        private void CreateSubmenus()
        {
            MpPedCustomizationMenu = new MpPedCustomization();
            Menu menu3 = MpPedCustomizationMenu.GetMenu();
            MenuItem button3 = new MenuItem("MP Ped Customizatiaon", "Crie seu personagem")
            {
                RightIcon = MenuItem.Icon.BARBER
            };

            Menu.AddMenuItem(button3);
            MenuController.BindMenuItem(Menu, menu3, button3);


            // Refresh everything.
            MenuController.Menus.ForEach((m) => m.RefreshIndex());
        }
        #endregion

    }
}
