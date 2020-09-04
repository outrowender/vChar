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
using static vCharShared.PermissionsManager;

namespace vCharClient
{
    public class About
    {
        // Variables
        private Menu menu;

        private void CreateMenu()
        {
            // Create the menu.
            menu = new Menu("vChar", "About vChar");

            // Create menu items.
            MenuItem version = new MenuItem("vChar Version", $"This server is using vChar ~b~~h~{MainMenu.Version}~h~~s~.")
            {
                Label = $"~h~{MainMenu.Version}~h~"
            };
            MenuItem credits = new MenuItem("About vChar / Credits", "vChar is made by ~b~Vespura~s~. For more info, checkout ~b~www.vespura.com/vmenu~s~. Thank you to: Deltanic, Brigliar, IllusiveTea, Shayan Doust and zr0iq for your contributions.");

            string serverInfoMessage = vCharShared.ConfigManager.GetSettingsString(vCharShared.ConfigManager.Setting.vmenu_server_info_message);
            if (!string.IsNullOrEmpty(serverInfoMessage))
            {
                MenuItem serverInfo = new MenuItem("Server Info", serverInfoMessage);
                string siteUrl = vCharShared.ConfigManager.GetSettingsString(vCharShared.ConfigManager.Setting.vmenu_server_info_website_url);
                if (!string.IsNullOrEmpty(siteUrl))
                {
                    serverInfo.Label = $"{siteUrl}";
                }
                menu.AddMenuItem(serverInfo);
            }
            menu.AddMenuItem(version);
            menu.AddMenuItem(credits);
        }

        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }
    }
}
