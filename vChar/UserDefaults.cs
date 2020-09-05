using static CitizenFX.Core.Native.API;

namespace vCharClient
{
    public static class UserDefaults
    {

        // Constants.
        private const string SETTINGS_PREFIX = "settings_";

        #region Public variables.
        #region PlayerOptions
        public static bool PlayerGodMode
        {
            get { return GetSettingsBool("playerGodMode"); }
            set { SetSavedSettingsBool("playerGodMode", value); }
        }

        public static bool PlayerStayInVehicle
        {
            get { return GetSettingsBool("playerStayInVehicle"); }
            set { SetSavedSettingsBool("playerStayInVehicle", value); }
        }

        public static bool UnlimitedStamina
        {
            get { return GetSettingsBool("unlimitedStamina"); }
            set { SetSavedSettingsBool("unlimitedStamina", value); }
        }

        public static bool FastRun
        {
            get { return GetSettingsBool("fastRun"); }
            set { SetSavedSettingsBool("fastRun", value); }
        }

        public static bool FastSwim
        {
            get { return GetSettingsBool("fastSwim"); }
            set { SetSavedSettingsBool("fastSwim", value); }
        }

        public static bool SuperJump
        {
            get { return GetSettingsBool("superJump"); }
            set { SetSavedSettingsBool("superJump", value); }
        }

        public static bool NoRagdoll
        {
            get { return GetSettingsBool("noRagdoll"); }
            set { SetSavedSettingsBool("noRagdoll", value); }
        }

        public static bool NeverWanted
        {
            get { return GetSettingsBool("neverWanted"); }
            set { SetSavedSettingsBool("neverWanted", value); }
        }

        public static bool EveryoneIgnorePlayer
        {
            get { return GetSettingsBool("everyoneIgnorePlayer"); }
            set { SetSavedSettingsBool("everyoneIgnorePlayer", value); }
        }
        #endregion

        #region Vehicle Options
        public static bool VehicleGodMode
        {
            get { return GetSettingsBool("vehicleGodMode"); }
            set { SetSavedSettingsBool("vehicleGodMode", value); }
        }
        public static bool VehicleGodInvincible
        {
            get { return GetSettingsBool("vehicleGodInvincible"); }
            set { SetSavedSettingsBool("vehicleGodInvincible", value); }
        }
        public static bool VehicleGodEngine
        {
            get { return GetSettingsBool("vehicleGodEngine"); }
            set { SetSavedSettingsBool("vehicleGodEngine", value); }
        }
        public static bool VehicleGodVisual
        {
            get { return GetSettingsBool("vehicleGodVisual"); }
            set { SetSavedSettingsBool("vehicleGodVisual", value); }
        }
        public static bool VehicleGodDamage
        {
            get { return GetSettingsBool("vehicleGodDamage"); }
            set { SetSavedSettingsBool("vehicleGodDamage", value); }
        }
        public static bool VehicleGodStrongWheels
        {
            get { return GetSettingsBool("vehicleGodStrongWheels"); }
            set { SetSavedSettingsBool("vehicleGodStrongWheels", value); }
        }
        public static bool VehicleGodRamp
        {
            get { return GetSettingsBool("vehicleGodRamp"); }
            set { SetSavedSettingsBool("vehicleGodRamp", value); }
        }
        public static bool VehicleGodAutoRepair
        {
            get { return GetSettingsBool("vehicleGodAutoRepair"); }
            set { SetSavedSettingsBool("vehicleGodAutoRepair", value); }
        }

        public static bool VehicleNeverDirty
        {
            get { return GetSettingsBool("vehicleNeverDirty"); }
            set { SetSavedSettingsBool("vehicleNeverDirty", value); }
        }

        public static bool VehicleEngineAlwaysOn
        {
            get { return GetSettingsBool("vehicleEngineAlwaysOn"); }
            set { SetSavedSettingsBool("vehicleEngineAlwaysOn", value); }
        }

        public static bool VehicleNoSiren
        {
            get { return GetSettingsBool("vehicleNoSiren"); }
            set { SetSavedSettingsBool("vehicleNoSiren", value); }
        }

        public static bool VehicleNoBikeHelmet
        {
            get { return GetSettingsBool("vehicleNoBikeHelmet"); }
            set { SetSavedSettingsBool("vehicleNoBikeHelmet", value); }
        }

        public static bool VehicleHighbeamsOnHonk
        {
            get { return GetSettingsBool("vehicleHighbeamsOnHonk"); }
            set { SetSavedSettingsBool("vehicleHighbeamsOnHonk", value); }
        }

        public static bool VehicleDisablePlaneTurbulence
        {
            get { return GetSettingsBool("vehicleDisablePlaneTurbulence"); }
            set { SetSavedSettingsBool("vehicleDisablePlaneTurbulence", value); }
        }

        public static bool VehicleBikeSeatbelt
        {
            get { return GetSettingsBool("vehicleBikeSeatbelt"); }
            set { SetSavedSettingsBool("vehicleBikeSeatbelt", value); }
        }
        #endregion

        #region Vehicle Spawner Options
        public static bool VehicleSpawnerSpawnInside
        {
            get { return GetSettingsBool("vehicleSpawnerSpawnInside"); }
            set { SetSavedSettingsBool("vehicleSpawnerSpawnInside", value); }
        }

        public static bool VehicleSpawnerReplacePrevious
        {
            get { return GetSettingsBool("vehicleSpawnerReplacePrevious"); }
            set { SetSavedSettingsBool("vehicleSpawnerReplacePrevious", value); }
        }
        #endregion

        #region Weapon Options
        public static bool WeaponsNoReload
        {
            get { return GetSettingsBool("weaponsNoReload"); }
            set { SetSavedSettingsBool("weaponsNoReload", value); }
        }

        public static bool WeaponsUnlimitedAmmo
        {
            get { return GetSettingsBool("weaponsUnlimitedAmmo"); }
            set { SetSavedSettingsBool("weaponsUnlimitedAmmo", value); }
        }

        public static bool WeaponsUnlimitedParachutes
        {
            get { return GetSettingsBool("weaponsUnlimitedParachutes"); }
            set { SetSavedSettingsBool("weaponsUnlimitedParachutes", value); }
        }

        public static bool AutoEquipChute
        {
            get { return GetSettingsBool("autoEquipParachuteWhenInPlane"); }
            set { SetSavedSettingsBool("autoEquipParachuteWhenInPlane", value); }
        }
        #endregion

        #region Misc Settings
        public static bool MiscJoinQuitNotifications
        {
            get { return GetSettingsBool("miscJoinQuitNotifications"); }
            set { SetSavedSettingsBool("miscJoinQuitNotifications", value); }
        }

        public static bool MiscDeathNotifications
        {
            get { return GetSettingsBool("miscDeathNotifications"); }
            set { SetSavedSettingsBool("miscDeathNotifications", value); }
        }

        public static bool MiscSpeedKmh
        {
            get { return GetSettingsBool("miscSpeedoKmh"); }
            set { SetSavedSettingsBool("miscSpeedoKmh", value); }
        }

        public static bool MiscSpeedMph
        {
            get { return GetSettingsBool("miscSpeedoMph"); }
            set { SetSavedSettingsBool("miscSpeedoMph", value); }
        }

        public static bool MiscShowLocation
        {
            get { return GetSettingsBool("miscShowLocation"); }
            set { SetSavedSettingsBool("miscShowLocation", value); }
        }

        public static bool MiscLocationBlips
        {
            get { return GetSettingsBool("miscLocationBlips"); }
            set { SetSavedSettingsBool("miscLocationBlips", value); }
        }

        public static bool MiscShowPlayerBlips
        {
            get { return GetSettingsBool("miscShowPlayerBlips"); }
            set { SetSavedSettingsBool("miscShowPlayerBlips", value); }
        }

        public static bool MiscShowOverheadNames
        {
            get { return GetSettingsBool("miscShowOverheadNames"); }
            set { SetSavedSettingsBool("miscShowOverheadNames", value); }
        }

        public static bool MiscRestorePlayerAppearance
        {
            get { return GetSettingsBool("miscRestorePlayerAppearance"); }
            set { SetSavedSettingsBool("miscRestorePlayerAppearance", value); }
        }

        public static bool MiscRestorePlayerWeapons
        {
            get { return GetSettingsBool("miscRestorePlayerWeapons"); }
            set { SetSavedSettingsBool("miscRestorePlayerWeapons", value); }
        }

        public static bool MiscRespawnDefaultCharacter
        {
            get { return GetSettingsBool("miscRespawnDefaultCharacter"); }
            set { SetSavedSettingsBool("miscRespawnDefaultCharacter", value); }
        }

        public static bool MiscShowTime
        {
            get { return GetSettingsBool("miscShowTime"); }
            set { SetSavedSettingsBool("miscShowTime", value); }
        }

        public static bool MiscRightAlignMenu
        {
            get { return GetSettingsBool("miscRightAlignMenu"); }
            set { SetSavedSettingsBool("miscRightAlignMenu", value); }
        }

        public static bool MiscDisablePrivateMessages
        {
            get { return GetSettingsBool("miscDisablePrivateMessages"); }
            set { SetSavedSettingsBool("miscDisablePrivateMessages", value); }
        }

        public static bool MiscDisableControllerSupport
        {
            get { return GetSettingsBool("miscDisableControllerSupport"); }
            set { SetSavedSettingsBool("miscDisableControllerSupport", value); }
        }

        public static int MiscLastTimeCycleModifierIndex
        {
            get { return GetSettingsInt("miscLastTimeCycleModifierIndex"); }
            set { SetSavedSettingsInt("miscLastTimeCycleModifierIndex", value); }
        }

        public static int MiscLastTimeCycleModifierStrength
        {
            get { return GetSettingsInt("miscLastTimeCycleModifierStrength"); }
            set { SetSavedSettingsInt("miscLastTimeCycleModifierStrength", value); }
        }

        #region keybind menu
        public static bool KbTpToWaypoint
        {
            get { return GetSettingsBool("kbTpToWaypoint"); }
            set { SetSavedSettingsBool("kbTpToWaypoint", value); }
        }
        public static bool KbDriftMode
        {
            get { return GetSettingsBool("kbDriftMode"); }
            set { SetSavedSettingsBool("kbDriftMode", value); }
        }
        public static bool KbRecordKeys
        {
            get { return GetSettingsBool("kbRecordKeys"); }
            set { SetSavedSettingsBool("kbRecordKeys", value); }
        }
        public static bool KbRadarKeys
        {
            get { return GetSettingsBool("kbRadarKeys"); }
            set { SetSavedSettingsBool("kbRadarKeys", value); }
        }
        public static bool KbPointKeys
        {
            get { return GetSettingsBool("kbPointKeys"); }
            set { SetSavedSettingsBool("kbPointKeys", value); }
        }
        #endregion
        #endregion

        #region Voice Chat Settings
        public static bool VoiceChatEnabled
        {
            get { return GetSettingsBool("voiceChatEnabled"); }
            set { SetSavedSettingsBool("voiceChatEnabled", value); }
        }

        public static float VoiceChatProximity
        {
            get { return GetSettingsFloat("voiceChatProximity"); }
            set { SetSavedSettingsFloat("voiceChatProximity", value); }
        }

        public static bool ShowCurrentSpeaker
        {
            get { return GetSettingsBool("voiceChatShowSpeaker"); }
            set { SetSavedSettingsBool("voiceChatShowSpeaker", value); }
        }

        public static bool ShowVoiceStatus
        {
            get { return GetSettingsBool("voiceChatShowVoiceStatus"); }
            set { SetSavedSettingsBool("voiceChatShowVoiceStatus", value); }
        }
        #endregion

        #region Player Appearance
        public static int PAClothingAnimationType
        {
            get { return GetSettingsInt("clothingAnimationType"); }
            set { SetSavedSettingsInt("clothingAnimationType", value >= 0 ? value : 0); }
        }
        #endregion

        #region Weapon Loadouts
        public static bool WeaponLoadoutsSetLoadoutOnRespawn
        {
            get { return GetSettingsBool("weaponLoadoutsSetLoadoutOnRespawn"); }
            set { SetSavedSettingsBool("weaponLoadoutsSetLoadoutOnRespawn", value); }
        }
        #endregion

        #region Personal Vehicle
        public static bool PVEnableVehicleBlip
        {
            get { return GetSettingsBool("pvEnableVehicleBlip"); }
            set { SetSavedSettingsBool("pvEnableVehicleBlip", value); }
        }
        #endregion
        #endregion

        #region Private functions
        /// <summary>
        /// Gets whether or not the specified setting is enabled or disabled in the saved user settings.
        /// Always returns false by default if the setting does not exist.
        /// </summary>
        /// <param name="kvpString">The setting to get.</param>
        /// <returns></returns>
        private static bool GetSettingsBool(string kvpString)
        {
            // Get the current value.
            string savedValue = GetResourceKvpString($"{SETTINGS_PREFIX}{kvpString}");
            // Check if it exists.
            bool exists = !string.IsNullOrEmpty(savedValue);
            // If not, create it and save the new default value of false.
            if (!exists)
            {
                // Some options should be enabled by default:
                if (
                    kvpString == "unlimitedStamina" ||
                    kvpString == "miscDeathNotifications" ||
                    kvpString == "miscJoinQuitNotifications" ||
                    kvpString == "vehicleSpawnerSpawnInside" ||
                    kvpString == "vehicleSpawnerReplacePrevious" ||
                    kvpString == "neverWanted" ||
                    kvpString == "voiceChatShowSpeaker" ||
                    kvpString == "voiceChatEnabled" ||
                    kvpString == "autoEquipParachuteWhenInPlane" ||
                    kvpString == "miscRestorePlayerAppearance" ||
                    kvpString == "miscRestorePlayerWeapons" ||
                    kvpString == "miscRightAlignMenu" ||
                    kvpString == "miscRespawnDefaultCharacter" ||
                    kvpString == "vehicleGodInvincible" ||
                    kvpString == "vehicleGodEngine" ||
                    kvpString == "vehicleGodVisual" ||
                    kvpString == "vehicleGodStrongWheels" ||
                    kvpString == "vehicleGodRamp"
                    )
                {
                    SetSavedSettingsBool(kvpString, true);
                    return true;
                }
                // All other options should be disabled by default:
                else
                {
                    SetSavedSettingsBool(kvpString, false);
                    return false;
                }
            }
            else
            {
                // Return the (new) value.
                return (GetResourceKvpString($"{SETTINGS_PREFIX}{kvpString}").ToLower() == "true");
            }
        }

        /// <summary>
        /// Sets the new saved value for the specified setting.
        /// </summary>
        /// <param name="kvpString">The setting to save.</param>
        /// <param name="newValue">The new value for this setting.</param>
        private static void SetSavedSettingsBool(string kvpString, bool newValue)
        {
            SetResourceKvp(SETTINGS_PREFIX + kvpString, newValue.ToString());
        }

        private static float GetSettingsFloat(string kvpString)
        {
            float savedValue = GetResourceKvpFloat(SETTINGS_PREFIX + kvpString);
            if (savedValue.ToString() != null) // this can still become null for some reason, so that's why we check it.
            {
                if (savedValue.GetType() == typeof(float))
                {
                    return savedValue;
                }
                else
                {
                    return -1f;
                }
            }
            else
            {
                SetSavedSettingsFloat(SETTINGS_PREFIX + kvpString, -1f);
                return -1f;
            }
        }

        private static void SetSavedSettingsFloat(string kvpString, float newValue)
        {
            SetResourceKvpFloat(SETTINGS_PREFIX + kvpString, newValue);
        }


        private static int GetSettingsInt(string kvpString)
        {
            // Get the current value.
            int savedValue = GetResourceKvpInt($"{SETTINGS_PREFIX}{kvpString}");
            return savedValue;
        }

        private static void SetSavedSettingsInt(string kvpString, int newValue)
        {
            SetResourceKvpInt(SETTINGS_PREFIX + kvpString, newValue);
        }
        #endregion
    }

}
