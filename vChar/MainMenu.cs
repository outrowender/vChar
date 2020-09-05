using System.Threading.Tasks;
using MenuAPI;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace vCharClient
{
    public class MainMenu : BaseScript
    {
        private bool firstTick = true;
        public static bool ConfigOptionsSetupComplete = false;

        public static Control MenuToggleKey { get { return MenuController.MenuToggleKey; } private set { MenuController.MenuToggleKey = value; } } // M by default (InteractionMenu)
        public static Menu Menu { get; private set; }
        public static PlayerAppearance PlayerAppearanceMenu { get; private set; }
        public static MpPedCustomization MpPedCustomizationMenu { get; private set; }
        public static PlayerList PlayersList;

        // Only used when debugging is enabled:
        //private BarTimerBar bt = new BarTimerBar("Opening Menu");

        public static bool DebugMode = GetResourceMetadata(GetCurrentResourceName(), "client_debug_mode", 0) == "true" ? true : false;
        public static bool EnableExperimentalFeatures = /*true;*/ (GetResourceMetadata(GetCurrentResourceName(), "experimental_features_enabled", 0) ?? "0") == "1";
        public static string Version { get { return GetResourceMetadata(GetCurrentResourceName(), "version", 0); } }

        public static bool DontOpenMenus { get { return MenuController.DontOpenAnyMenu; } set { MenuController.DontOpenAnyMenu = value; } }
        public static bool DisableControls { get { return MenuController.DisableMenuButtons; } set { MenuController.DisableMenuButtons = value; } }
            
        public MainMenu()
        {
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            if (firstTick)
            {
                firstTick = false;               
                // Clear all previous pause menu info/brief messages on resource start.
                ClearBrief();

                // Create the main menu.
                Menu = new Menu(Game.Player.Name, "Bem vindo a cidade!");

                // Add the main menu to the menu pool.
                MenuController.AddMenu(Menu);
                MenuController.MainMenu = Menu;

                // Create all (sub)menus.
                CreateSubmenus();
            }

            // If the setup (permissions) is done, and it's not the first tick, then do this:
            if (ConfigOptionsSetupComplete && !firstTick)
            {
                if (MpPedCustomizationMenu != null)
                {
                    MpPedCustomization.DisableBackButton = false;
                    MpPedCustomization.DontCloseMenus = true;
                }

                // Menu toggle button.
                Game.DisableControlThisFrame(0, MenuToggleKey);
            }
        }

        private void CreateSubmenus()
        {
            MpPedCustomizationMenu = new MpPedCustomization();
            Menu menu3 = MpPedCustomizationMenu.GetMenu();
            MenuItem button3 = new MenuItem("Crie o seu personagem")
            {
                RightIcon = MenuItem.Icon.CLOTHING
            };

            Menu.AddMenuItem(button3);
            MenuController.BindMenuItem(Menu, menu3, button3);

            // Refresh everything.
            MenuController.Menus.ForEach((m) => m.RefreshIndex());
        }

    }
}
