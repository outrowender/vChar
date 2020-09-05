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

namespace vCharClient
{
    public static class CommonFunctions
    {

        internal static bool DriveToWpTaskActive = false;
        internal static bool DriveWanderTaskActive = false;
    
        public static void TriggerServerEvent(string eventName, params object[] args)
        {
            BaseScript.TriggerServerEvent(eventName, args);
        }

        public static void TriggerEvent(string eventName, params object[] args)
        {
            BaseScript.TriggerEvent(eventName, args);
        }

        public static async Task Delay(int time)
        {
            await BaseScript.Delay(time);
        }

        #region menu position
        public static bool RightAlignMenus() => UserDefaults.MiscRightAlignMenu;
        #endregion

        #region Toggle vehicle alarm
        public static void ToggleVehicleAlarm(Vehicle vehicle)
        {
            if (vehicle != null && vehicle.Exists())
            {
                if (vehicle.IsAlarmSounding)
                {
                    // Set the duration to 0;
                    vehicle.AlarmTimeLeft = 0;
                    vehicle.IsAlarmSet = false;
                }
                else
                {
                    // Randomize duration of the alarm and start the alarm.
                    vehicle.IsAlarmSet = true;
                    vehicle.AlarmTimeLeft = new Random().Next(8000, 45000);
                    vehicle.StartAlarm();
                }
            }
        }
        #endregion


        /// <summary>
        /// Gets the finger pointing camera pitch.
        /// </summary>
        /// <returns></returns>
        public static float GetPointingPitch()
        {
            float pitch = GetGameplayCamRelativePitch();
            if (pitch < -70f)
            {
                pitch = -70f;
            }
            if (pitch > 42f)
            {
                pitch = 42f;
            }
            pitch += 70f;
            pitch /= 112f;

            return pitch;
        }
        /// <summary>
        /// Gets the finger pointing camera heading.
        /// </summary>
        /// <returns></returns>
        public static float GetPointingHeading()
        {
            float heading = GetGameplayCamRelativeHeading();
            if (heading < -180f)
            {
                heading = -180f;
            }
            if (heading > 180f)
            {
                heading = 180f;
            }
            heading += 180f;
            heading /= 360f;
            heading *= -1f;
            heading += 1f;

            return heading;
        }

        #region GetUserInput
        /// <summary>
        /// Get a user input text string.
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetUserInput() => await GetUserInput(null, null, 30);
        /// <summary>
        /// Get a user input text string.
        /// </summary>
        /// <param name="maxInputLength"></param>
        /// <returns></returns>
        public static async Task<string> GetUserInput(int maxInputLength) => await GetUserInput(null, null, maxInputLength);
        /// <summary>
        /// Get a user input text string.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static async Task<string> GetUserInput(string windowTitle) => await GetUserInput(windowTitle, null, 30);
        /// <summary>
        /// Get a user input text string.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="maxInputLength"></param>
        /// <returns></returns>
        public static async Task<string> GetUserInput(string windowTitle, int maxInputLength) => await GetUserInput(windowTitle, null, maxInputLength);
        /// <summary>
        /// Get a user input text string.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static async Task<string> GetUserInput(string windowTitle, string defaultText) => await GetUserInput(windowTitle, defaultText, 30);
        /// <summary>
        /// Get a user input text string.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="defaultText"></param>
        /// <param name="maxInputLength"></param>
        /// <returns></returns>
        public static async Task<string> GetUserInput(string windowTitle, string defaultText, int maxInputLength)
        {
            // Create the window title string.
            var spacer = "\t";
            AddTextEntry($"{GetCurrentResourceName().ToUpper()}_WINDOW_TITLE", $"{windowTitle ?? "Enter"}:{spacer}(MAX {maxInputLength} Characters)");

            // Display the input box.
            DisplayOnscreenKeyboard(1, $"{GetCurrentResourceName().ToUpper()}_WINDOW_TITLE", "", defaultText ?? "", "", "", "", maxInputLength);
            await Delay(0);
            // Wait for a result.
            while (true)
            {
                int keyboardStatus = UpdateOnscreenKeyboard();

                switch (keyboardStatus)
                {
                    case 3: // not displaying input field anymore somehow
                    case 2: // cancelled
                        return null;
                    case 1: // finished editing
                        return GetOnscreenKeyboardResult();
                    default:
                        await Delay(0);
                        break;
                }
            }
        }
        #endregion

        #region ToProperString()
        /// <summary>
        /// Converts a PascalCaseString to a Propper Case String.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>Input string converted to a normal sentence.</returns>
        public static string ToProperString(string inputString)
        {
            var outputString = "";
            var prevUpper = true;
            foreach (char c in inputString)
            {
                if (char.IsLetter(c) && c != ' ' && c == char.Parse(c.ToString().ToUpper()))
                {
                    if (prevUpper)
                    {
                        outputString += $"{c}";
                    }
                    else
                    {
                        outputString += $" {c}";
                    }
                    prevUpper = true;
                }
                else
                {
                    prevUpper = false;
                    outputString += c.ToString();
                }
            }
            while (outputString.IndexOf("  ") != -1)
            {
                outputString = outputString.Replace("  ", " ");
            }
            return outputString;
        }
        #endregion

        #region Data parsing functions
        /// <summary>
        /// Converts a simple json string (only containing (string) key : (string) value).
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, string> JsonToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        #endregion

        #region StringToStringArray
        /// <summary>
        /// Converts the inputString into a string[] (array).
        /// Each string in the array is up to 99 characters long at max.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string[] StringToArray(string inputString)
        {
            return CitizenFX.Core.UI.Screen.StringToArray(inputString);
        }
        #endregion

        #region ped info struct
        public struct PedInfo
        {
            public int version;
            public uint model;
            public bool isMpPed;
            public Dictionary<int, int> props;
            public Dictionary<int, int> propTextures;
            public Dictionary<int, int> drawableVariations;
            public Dictionary<int, int> drawableVariationTextures;
        };
        #endregion

        #region Set Player Skin
        /// <summary>
        /// Sets the player's model to the provided modelName.
        /// </summary>
        /// <param name="modelName">The model name.</param>
        public static async Task SetPlayerSkin(string modelName, PedInfo pedCustomizationOptions, bool keepWeapons = true) => await SetPlayerSkin((uint)GetHashKey(modelName), pedCustomizationOptions, keepWeapons);

        /// <summary>
        /// Sets the player's model to the provided modelHash.
        /// </summary>
        /// <param name="modelHash">The model hash.</param>
        public static async Task SetPlayerSkin(uint modelHash, PedInfo pedCustomizationOptions, bool keepWeapons = true)
        {
            if (IsModelInCdimage(modelHash))
            {
                if (keepWeapons)
                {
                    Log("saved from SetPlayerSkin()");
                }
                RequestModel(modelHash);
                while (!HasModelLoaded(modelHash))
                {
                    await Delay(0);
                }

                if ((uint)GetEntityModel(Game.PlayerPed.Handle) != modelHash) // only change skins if the player is not yet using the new skin.
                {
                    // check if the ped is in a vehicle.
                    bool wasInVehicle = Game.PlayerPed.IsInVehicle();
                    Vehicle veh = Game.PlayerPed.CurrentVehicle;
                    VehicleSeat seat = Game.PlayerPed.SeatIndex;

                    int maxHealth = Game.PlayerPed.MaxHealth;
                    int maxArmour = Game.Player.MaxArmor;
                    int health = Game.PlayerPed.Health;
                    int armour = Game.PlayerPed.Armor;

                    // set the model
                    SetPlayerModel(Game.Player.Handle, modelHash);

                    Game.Player.MaxArmor = maxArmour;
                    Game.PlayerPed.MaxHealth = maxHealth;
                    Game.PlayerPed.Health = health;
                    Game.PlayerPed.Armor = armour;

                    // warp ped into vehicle if the player was in a vehicle.
                    if (wasInVehicle && veh != null && seat != VehicleSeat.None)
                    {
                        FreezeEntityPosition(Game.PlayerPed.Handle, true);
                        int tmpTimer = GetGameTimer();
                        while (!Game.PlayerPed.IsInVehicle(veh))
                        {
                            // if it takes too long, stop trying to teleport.
                            if (GetGameTimer() - tmpTimer > 1000)
                            {
                                break;
                            }
                            ClearPedTasks(Game.PlayerPed.Handle);
                            await Delay(0);
                            TaskWarpPedIntoVehicle(Game.PlayerPed.Handle, veh.Handle, (int)seat);
                        }
                        FreezeEntityPosition(Game.PlayerPed.Handle, false);
                    }
                }

                // Reset some stuff.
                SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                ClearAllPedProps(Game.PlayerPed.Handle);
                ClearPedDecorations(Game.PlayerPed.Handle);
                ClearPedFacialDecorations(Game.PlayerPed.Handle);

                if (pedCustomizationOptions.version == 1)
                {
                    var ped = Game.PlayerPed.Handle;
                    for (var drawable = 0; drawable < 21; drawable++)
                    {
                        SetPedComponentVariation(ped, drawable, pedCustomizationOptions.drawableVariations[drawable],
                            pedCustomizationOptions.drawableVariationTextures[drawable], 1);
                    }

                    for (var i = 0; i < 21; i++)
                    {
                        int prop = pedCustomizationOptions.props[i];
                        int propTexture = pedCustomizationOptions.propTextures[i];
                        if (prop == -1 || propTexture == -1)
                        {
                            ClearPedProp(ped, i);
                        }
                        else
                        {
                            SetPedPropIndex(ped, i, prop, propTexture, true);
                        }
                    }
                }
                else if (pedCustomizationOptions.version == -1)
                {
                    // do nothing.
                }
                else
                {
                    // notify user of unsupported version
                    //Notify.Error("This is an unsupported saved ped version. Cannot restore appearance. :(");
                }
                if (modelHash == (uint)GetHashKey("mp_f_freemode_01") || modelHash == (uint)GetHashKey("mp_m_freemode_01"))
                {
                    //var headBlendData = Game.PlayerPed.GetHeadBlendData();
                    if (pedCustomizationOptions.version == -1)
                    {
                        SetPedHeadBlendData(Game.PlayerPed.Handle, 0, 0, 0, 0, 0, 0, 0.5f, 0.5f, 0f, false);
                        while (!HasPedHeadBlendFinished(Game.PlayerPed.Handle))
                        {
                            await Delay(0);
                        }
                    }
                }
                SetModelAsNoLongerNeeded(modelHash);
            }
            else
            {
                //Notify.Error(CommonErrors.InvalidModel);
            }
        }

        /// <summary>
        /// Set the player model by asking for user input.
        /// </summary>
        public static async void SpawnPedByName()
        {
            string input = await GetUserInput(windowTitle: "Enter Ped Model Name", maxInputLength: 30);
            if (!string.IsNullOrEmpty(input))
            {
                await SetPlayerSkin((uint)GetHashKey(input), new PedInfo() { version = -1 });
            }
            else
            {
                //Notify.Error(CommonErrors.InvalidModel);
            }
        }
        #endregion

        #region Save Ped Model + Customizations
        /// <summary>
        /// Saves the current player ped.
        /// </summary>
        public static async Task<bool> SavePed(string forceName = null, bool overrideExistingPed = false)
        {
            string name = forceName;
            if (string.IsNullOrEmpty(name))
            {
                // Get the save name.
                name = await GetUserInput(windowTitle: "Enter a ped save name", maxInputLength: 30);
            }

            // If the save name is not invalid.
            if (!string.IsNullOrEmpty(name))
            {
                // Create a dictionary to store all data in.
                PedInfo data = new PedInfo();

                // Get the ped.
                int ped = Game.PlayerPed.Handle;

                data.version = 1;
                // Get the ped model hash & add it to the dictionary.
                uint model = (uint)GetEntityModel(ped);
                data.model = model;

                // Loop through all drawable variations.
                var drawables = new Dictionary<int, int>();
                var drawableTextures = new Dictionary<int, int>();
                for (var i = 0; i < 21; i++)
                {
                    int drawable = GetPedDrawableVariation(ped, i);
                    int textureVariation = GetPedTextureVariation(ped, i);
                    drawables.Add(i, drawable);
                    drawableTextures.Add(i, textureVariation);
                }
                data.drawableVariations = drawables;
                data.drawableVariationTextures = drawableTextures;

                var props = new Dictionary<int, int>();
                var propTextures = new Dictionary<int, int>();
                // Loop through all prop variations.
                for (var i = 0; i < 21; i++)
                {
                    int prop = GetPedPropIndex(ped, i);
                    int propTexture = GetPedPropTextureIndex(ped, i);
                    props.Add(i, prop);
                    propTextures.Add(i, propTexture);
                }
                data.props = props;
                data.propTextures = propTextures;

                data.isMpPed = (model == (uint)GetHashKey("mp_f_freemode_01") || model == (uint)GetHashKey("mp_m_freemode_01"));
                if (data.isMpPed)
                {
                    //Notify.Alert("Note, you should probably use the MP Character creator if you want more advanced features. Saving Multiplayer characters with this function does NOT save a lot of the online peds customization.");
                }

                // Try to save the data, and save the result in a variable.
                bool saveSuccessful;
                if (name == "vChar_tmp_saved_ped")
                {
                    saveSuccessful = StorageManager.SavePedInfo(name, data, true);
                }
                else
                {
                    saveSuccessful = StorageManager.SavePedInfo("ped_" + name, data, overrideExistingPed);
                }

                //if (name != "vChar_tmp_saved_ped") // only send a notification if the save wasn't triggered because the player died.
                //{
                //    // If the save was successfull.
                //    if (saveSuccessful)
                //    {
                //        //Notify.Success("Ped saved.");
                //    }
                //    // Save was not successfull.
                //    else
                //    {
                //        Notify.Error(CommonErrors.SaveNameAlreadyExists, placeholderValue: name);
                //    }
                //}

                return saveSuccessful;
            }
            // User cancelled the saving or they did not enter a valid name.
            else
            {
                //Notify.Error(CommonErrors.InvalidSaveName);
            }
            return false;
        }
        #endregion

        #region Load Saved Ped
        /// <summary>
        /// Load the saved ped and spawn it.
        /// </summary>
        /// <param name="savedName">The ped saved name</param>
        public static async void LoadSavedPed(string savedName, bool restoreWeapons)
        {
            if (savedName != "vChar_tmp_saved_ped")
            {
                PedInfo pi = StorageManager.GetSavedPedInfo("ped_" + savedName);
                Log(JsonConvert.SerializeObject(pi));
                await SetPlayerSkin(pi.model, pi, restoreWeapons);
            }
            else
            {
                PedInfo pi = StorageManager.GetSavedPedInfo(savedName);
                Log(JsonConvert.SerializeObject(pi));
                await SetPlayerSkin(pi.model, pi, restoreWeapons);
                DeleteResourceKvp("vChar_tmp_saved_ped");
            }

        }

        /// <summary>
        /// Checks if the ped is saved from before the player died.
        /// </summary>
        /// <returns></returns>
        public static bool IsTempPedSaved()
        {
            if (!string.IsNullOrEmpty(GetResourceKvpString("vChar_tmp_saved_ped")))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region saved ped json string to ped info
        /// <summary>
        /// Load and convert json ped info into PedInfo struct.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static PedInfo JsonToPedInfo(string json)
        {
            return JsonConvert.DeserializeObject<PedInfo>(json);
        }
        #endregion

        #region Get "Header" Menu Item
        /// <summary>
        /// Get a header menu item (text-centered, disabled MenuItem)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static MenuItem GetSpacerMenuItem(string title, string description = null)
        {
            string output = "~h~";
            int length = title.Length;
            int totalSize = 80 - length;

            for (var i = 0; i < totalSize / 2 - (length / 2); i++)
            {
                output += " ";
            }
            output += title;
            MenuItem item = new MenuItem(output, description ?? "")
            {
                Enabled = false
            };
            return item;
        }
        #endregion

        #region Log Function
        /// <summary>
        /// Print data to the console and save it to the CitizenFX.log file. Only when vChar debugging mode is enabled.
        /// </summary>
        /// <param name="data"></param>
        public static void Log(string data)
        {
            Debug.WriteLine(@data);
        }
        #endregion

        #region Get Currently Opened Menu
        /// <summary>
        /// Returns the currently opened menu, if no menu is open, it'll return null.
        /// </summary>
        /// <returns></returns>
        public static Menu GetOpenMenu()
        {
            return MenuController.GetCurrentMenu();
        }
        #endregion
               
        #region Set Player Walking Style
        /// <summary>
        /// Sets the walking style for this player.
        /// </summary>
        /// <param name="walkingStyle"></param>
        public static async void SetWalkingStyle(string walkingStyle)
        {
            if (IsPedModel(Game.PlayerPed.Handle, (uint)GetHashKey("mp_f_freemode_01")) || IsPedModel(Game.PlayerPed.Handle, (uint)GetHashKey("mp_m_freemode_01")))
            {
                bool isPedMale = IsPedModel(Game.PlayerPed.Handle, (uint)GetHashKey("mp_m_freemode_01"));
                ClearPedAlternateMovementAnim(Game.PlayerPed.Handle, 0, 1f);
                ClearPedAlternateMovementAnim(Game.PlayerPed.Handle, 1, 1f);
                ClearPedAlternateMovementAnim(Game.PlayerPed.Handle, 2, 1f);
                ClearPedAlternateWalkAnim(Game.PlayerPed.Handle, 1f);
                string animDict = null;
                if (walkingStyle == "Injured")
                {
                    animDict = isPedMale ? "move_m@injured" : "move_f@injured";
                }
                else if (walkingStyle == "Tough Guy")
                {
                    animDict = isPedMale ? "move_m@tough_guy@" : "move_f@tough_guy@";
                }
                else if (walkingStyle == "Femme")
                {
                    animDict = isPedMale ? "move_m@femme@" : "move_f@femme@";
                }
                else if (walkingStyle == "Gangster")
                {
                    animDict = isPedMale ? "move_m@gangster@a" : "move_f@gangster@ng";
                }
                else if (walkingStyle == "Posh")
                {
                    animDict = isPedMale ? "move_m@posh@" : "move_f@posh@";
                }
                else if (walkingStyle == "Sexy")
                {
                    animDict = isPedMale ? null : "move_f@sexy@a";
                }
                else if (walkingStyle == "Business")
                {
                    animDict = isPedMale ? null : "move_f@business@a";
                }
                else if (walkingStyle == "Drunk")
                {
                    animDict = isPedMale ? "move_m@drunk@a" : "move_f@drunk@a";
                }
                else if (walkingStyle == "Hipster")
                {
                    animDict = isPedMale ? "move_m@hipster@a" : null;
                }
                if (animDict != null)
                {
                    if (!HasAnimDictLoaded(animDict))
                    {
                        RequestAnimDict(animDict);
                        while (!HasAnimDictLoaded(animDict))
                        {
                            await Delay(0);
                        }
                    }
                    SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 0, animDict, "idle", 1f, true);
                    SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 1, animDict, "walk", 1f, true);
                    SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 2, animDict, "run", 1f, true);
                }
                else if (walkingStyle != "Normal")
                {
                    if (isPedMale)
                    {
                        //Notify.Error(CommonErrors.WalkingStyleNotForMale);
                    }
                    else
                    {
                        //Notify.Error(CommonErrors.WalkingStyleNotForFemale);
                    }
                }
            }
            else
            {
                //Notify.Error("This feature only supports the multiplayer freemode male/female ped models.");
            }
        }
        #endregion

        #region Disable Movement Controls
        /// <summary>
        /// Disables all movement and camera related controls this frame.
        /// </summary>
        /// <param name="disableMovement"></param>
        /// <param name="disableCameraMovement"></param>
        public static void DisableMovementControlsThisFrame(bool disableMovement, bool disableCameraMovement)
        {
            if (disableMovement)
            {
                Game.DisableControlThisFrame(0, Control.MoveDown);
                Game.DisableControlThisFrame(0, Control.MoveDownOnly);
                Game.DisableControlThisFrame(0, Control.MoveLeft);
                Game.DisableControlThisFrame(0, Control.MoveLeftOnly);
                Game.DisableControlThisFrame(0, Control.MoveLeftRight);
                Game.DisableControlThisFrame(0, Control.MoveRight);
                Game.DisableControlThisFrame(0, Control.MoveRightOnly);
                Game.DisableControlThisFrame(0, Control.MoveUp);
                Game.DisableControlThisFrame(0, Control.MoveUpDown);
                Game.DisableControlThisFrame(0, Control.MoveUpOnly);
                Game.DisableControlThisFrame(0, Control.VehicleFlyMouseControlOverride);
                Game.DisableControlThisFrame(0, Control.VehicleMouseControlOverride);
                Game.DisableControlThisFrame(0, Control.VehicleMoveDown);
                Game.DisableControlThisFrame(0, Control.VehicleMoveDownOnly);
                Game.DisableControlThisFrame(0, Control.VehicleMoveLeft);
                Game.DisableControlThisFrame(0, Control.VehicleMoveLeftRight);
                Game.DisableControlThisFrame(0, Control.VehicleMoveRight);
                Game.DisableControlThisFrame(0, Control.VehicleMoveRightOnly);
                Game.DisableControlThisFrame(0, Control.VehicleMoveUp);
                Game.DisableControlThisFrame(0, Control.VehicleMoveUpDown);
                Game.DisableControlThisFrame(0, Control.VehicleSubMouseControlOverride);
                Game.DisableControlThisFrame(0, Control.Duck);
                Game.DisableControlThisFrame(0, Control.SelectWeapon);
            }
            if (disableCameraMovement)
            {
                Game.DisableControlThisFrame(0, Control.LookBehind);
                Game.DisableControlThisFrame(0, Control.LookDown);
                Game.DisableControlThisFrame(0, Control.LookDownOnly);
                Game.DisableControlThisFrame(0, Control.LookLeft);
                Game.DisableControlThisFrame(0, Control.LookLeftOnly);
                Game.DisableControlThisFrame(0, Control.LookLeftRight);
                Game.DisableControlThisFrame(0, Control.LookRight);
                Game.DisableControlThisFrame(0, Control.LookRightOnly);
                Game.DisableControlThisFrame(0, Control.LookUp);
                Game.DisableControlThisFrame(0, Control.LookUpDown);
                Game.DisableControlThisFrame(0, Control.LookUpOnly);
                Game.DisableControlThisFrame(0, Control.ScaledLookDownOnly);
                Game.DisableControlThisFrame(0, Control.ScaledLookLeftOnly);
                Game.DisableControlThisFrame(0, Control.ScaledLookLeftRight);
                Game.DisableControlThisFrame(0, Control.ScaledLookUpDown);
                Game.DisableControlThisFrame(0, Control.ScaledLookUpOnly);
                Game.DisableControlThisFrame(0, Control.VehicleDriveLook);
                Game.DisableControlThisFrame(0, Control.VehicleDriveLook2);
                Game.DisableControlThisFrame(0, Control.VehicleLookBehind);
                Game.DisableControlThisFrame(0, Control.VehicleLookLeft);
                Game.DisableControlThisFrame(0, Control.VehicleLookRight);
                Game.DisableControlThisFrame(0, Control.NextCamera);
                Game.DisableControlThisFrame(0, Control.VehicleFlyAttackCamera);
                Game.DisableControlThisFrame(0, Control.VehicleCinCam);
            }
        }
        #endregion

        #region Get safe player name
        /// <summary>
        /// Returns a properly formatted and escaped player name for notifications.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSafePlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }
            return name.Replace("^", @"\^").Replace("~", @"\~").Replace("<", "«").Replace(">", "»");
        }
        #endregion

        #region Keyfob personal vehicle func
        public static async void PressKeyFob(Vehicle veh)
        {
            Player player = Game.Player;
            if (player != null && !player.IsDead && !player.Character.IsInVehicle())
            {
                uint KeyFobHashKey = (uint)GetHashKey("p_car_keys_01");
                RequestModel(KeyFobHashKey);
                while (!HasModelLoaded(KeyFobHashKey))
                {
                    await Delay(0);
                }

                int KeyFobObject = CreateObject((int)KeyFobHashKey, 0, 0, 0, true, true, true);
                AttachEntityToEntity(KeyFobObject, player.Character.Handle, GetPedBoneIndex(player.Character.Handle, 57005), 0.09f, 0.03f, -0.02f, -76f, 13f, 28f, false, true, true, true, 0, true);
                SetModelAsNoLongerNeeded(KeyFobHashKey); // cleanup model from memory

                ClearPedTasks(player.Character.Handle);
                SetCurrentPedWeapon(Game.PlayerPed.Handle, (uint)GetHashKey("WEAPON_UNARMED"), true);
                //if (player.Character.Weapons.Current.Hash != WeaponHash.Unarmed)
                //{
                //    player.Character.Weapons.Give(WeaponHash.Unarmed, 1, true, true);
                //}

                // if (!HasEntityClearLosToEntityInFront(player.Character.Handle, veh.Handle))
                {
                    /*
                    TODO: Work out how to get proper heading between entities.
                    */


                    //SetPedDesiredHeading(player.Character.Handle, )
                    //float heading = GetHeadingFromVector_2d(player.Character.Position.X - veh.Position.Y, player.Character.Position.Y - veh.Position.X);
                    //double x = Math.Cos(player.Character.Position.X) * Math.Sin(player.Character.Position.Y - (double)veh.Position.Y);
                    //double y = Math.Cos(player.Character.Position.X) * Math.Sin(veh.Position.X) - Math.Sin(player.Character.Position.X) * Math.Cos(veh.Position.X) * Math.Cos(player.Character.Position.Y - (double)veh.Position.Y);
                    //float heading = (float)Math.Atan2(x, y);
                    //Debug.WriteLine(heading.ToString());
                    //SetPedDesiredHeading(player.Character.Handle, heading);

                    ClearPedTasks(Game.PlayerPed.Handle);
                    TaskTurnPedToFaceEntity(player.Character.Handle, veh.Handle, 500);
                }

                string animDict = "anim@mp_player_intmenu@key_fob@";
                RequestAnimDict(animDict);
                while (!HasAnimDictLoaded(animDict))
                {
                    await Delay(0);
                }
                player.Character.Task.PlayAnimation(animDict, "fob_click", 3f, 1000, AnimationFlags.UpperBodyOnly);
                PlaySoundFromEntity(-1, "Remote_Control_Fob", player.Character.Handle, "PI_Menu_Sounds", true, 0);


                await Delay(1250);
                DetachEntity(KeyFobObject, false, false);
                DeleteObject(ref KeyFobObject);
                RemoveAnimDict(animDict); // cleanup anim dict from memory
            }

            await Delay(0);
        }
        #endregion

        #region Encoded float to normal float
        ///// <summary>
        ///// Converts an IEEE 754 (int encoded) floating-point to a real float value.
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static float IntToFloat(int input)
        //{
        //    // This function is based on the 'hex2float' snippet found here for Lua:
        //    // https://stackoverflow.com/a/19996852

        //    //string d = input.ToString("X8");

        //    var s1 = (char)int.Parse(d.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        //    var s2 = (char)int.Parse(d.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        //    var s3 = (char)int.Parse(d.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //    var s4 = (char)int.Parse(d.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

        //    var b1 = BitConverter.GetBytes(s1)[0];
        //    var b2 = BitConverter.GetBytes(s2)[0];
        //    var b3 = BitConverter.GetBytes(s3)[0];
        //    var b4 = BitConverter.GetBytes(s4)[0];

        //    int sign = b1 > 0x7F ? -1 : 1;
        //    int expo = ((b1 % 0x80) * 0x2) + (b2 / 0x80);
        //    float mant = ((b2 % 0x80) * 0x100 + b3) * 0x100 + b4;

        //    float n;
        //    if (mant == 0 && expo == 0)
        //    {
        //        n = sign * 0.0f;
        //    }
        //    else if (expo == 0xFF)
        //    {
        //        if (mant == 0)
        //        {
        //            n = (float)((double)sign * Math.E);
        //        }
        //        else
        //        {
        //            n = 0.0f;
        //        }
        //    }
        //    else
        //    {
        //        double left = 1.0 + mant / 0x800000;
        //        int right = expo - 0x7F;
        //        float other = (float)left * ((float)right * (float)right);
        //        n = (float)sign * (float)other;
        //    }
        //    return n;
        //}
        #endregion

        #region save player location to the server locations.json file
        /// <summary>
        /// Saves the player's location as a new teleport location in the teleport options menu.
        /// </summary>
        public static async void SavePlayerLocationToLocationsFile()
        {
            var pos = Game.PlayerPed.Position;
            var heading = Game.PlayerPed.Heading;
            string locationName = await GetUserInput("Enter location save name", 30);
            if (string.IsNullOrEmpty(locationName))
            {
                //Notify.Error(CommonErrors.InvalidInput);
                return;
            }
            if (vCharShared.ConfigManager.GetTeleportLocationsData().Any(loc => loc.name == locationName))
            {
                //Notify.Error("This location name is already used, please use a different name.");
                return;
            }
            TriggerServerEvent("vChar:SaveTeleportLocation", JsonConvert.SerializeObject(new vCharShared.ConfigManager.TeleportLocation(locationName, pos, heading)));
            //Notify.Success("The location was successfully saved.");
        }
        #endregion
    }
}
