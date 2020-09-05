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

namespace vCharClient
{
    public class EventManager : BaseScript
    {
        public static int GetServerMinutes => MathUtil.Clamp(GetSettingsInt(Setting.vmenu_current_minute), 0, 59);
        public static int GetServerHours => MathUtil.Clamp(GetSettingsInt(Setting.vmenu_current_hour), 0, 23);
        public static int GetServerMinuteDuration => GetSettingsInt(Setting.vmenu_ingame_minute_duration);
        public static bool IsServerTimeFrozen => GetSettingsBool(Setting.vmenu_freeze_time);
        public static bool IsServerTimeSyncedWithMachineTime => GetSettingsBool(Setting.vmenu_sync_to_machine_time);
        public static string GetServerWeather => GetSettingsString(Setting.vmenu_current_weather, "CLEAR");
        public static bool DynamicWeatherEnabled => GetSettingsBool(Setting.vmenu_enable_dynamic_weather);
        public static bool IsBlackoutEnabled => GetSettingsBool(Setting.vmenu_blackout_enabled);
        public static int WeatherChangeTime => MathUtil.Clamp(GetSettingsInt(Setting.vmenu_weather_change_duration), 0, 45);

        /// <summary>
        /// Constructor.
        /// </summary>
        public EventManager()
        {
            // Add event handlers.
            //EventHandlers.Add("vChar:SetAddons", new Action(SetAddons));
        
            if (GetSettingsBool(Setting.vmenu_enable_weather_sync))
                Tick += WeatherSync;
            if (GetSettingsBool(Setting.vmenu_enable_time_sync))
                Tick += TimeSync;

        }
        
        private bool firstSpawn = true;

        /// <summary>
        /// Loads/unloads the snow fx particles if needed.
        /// </summary>
        private async void UpdateWeatherParticles()
        {
            if (GetServerWeather.ToUpper() == "XMAS")
            {
                if (!HasNamedPtfxAssetLoaded("core_snow"))
                {
                    RequestNamedPtfxAsset("core_snow");
                    while (!HasNamedPtfxAssetLoaded("core_snow"))
                    {
                        await Delay(0);
                    }
                }
                UseParticleFxAssetNextCall("core_snow");
                SetForceVehicleTrails(true);
                SetForcePedFootstepsTracks(true);
            }
            else
            {
                SetForceVehicleTrails(false);
                SetForcePedFootstepsTracks(false);
                RemoveNamedPtfxAsset("core_snow");
            }
        }

        /// <summary>
        /// OnTick loop to keep the weather synced.
        /// </summary>
        /// <returns></returns>
        private async Task WeatherSync()
        {
            UpdateWeatherParticles();
            SetArtificialLightsState(IsBlackoutEnabled);
            if (GetNextWeatherType() != GetHashKey(GetServerWeather))
            {
                // Dbg logging
                Log($"Start changing weather type. New weather: {GetServerWeather} Blackout? {IsBlackoutEnabled}");

                /*SetWeatherTypeOverTime(GetServerWeather, (float)WeatherChangeTime);*/
                SetWeatherTypeOvertimePersist(GetServerWeather, (float)WeatherChangeTime);
                await Delay(WeatherChangeTime * 1000 + 2000);

                // Dbg logging
                Log("done changing weather type");

                TriggerEvent("vChar:WeatherChangeComplete", GetServerWeather);
            }
            await Delay(1000);
        }

        /// <summary>
        /// This function will take care of time sync. It'll be called once, and never stop.
        /// </summary>
        /// <returns></returns>
        private async Task TimeSync()
        {
            NetworkOverrideClockTime(GetServerHours, GetServerMinutes, 0);
            if (IsServerTimeFrozen || IsServerTimeSyncedWithMachineTime)
            {
                await Delay(5);
            }
            else
            {
                await Delay(MathUtil.Clamp(GetServerMinuteDuration, 100, 2000));
            }
        }       
    }
}
