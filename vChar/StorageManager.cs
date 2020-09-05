using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;
using static vCharClient.CommonFunctions;

namespace vCharClient
{

    public static class StorageManager
    {
        /// <summary>
        /// Save Dictionary(string, string) to local storage.
        /// </summary>
        /// <param name="saveName">Name (including prefix) to save.</param>
        /// <param name="data">Data (dictionary) to save.</param>
        /// <param name="overrideExistingData">When true, will override existing save data with the same name. 
        /// If false, it will cancel the save if existing data is found and return false.</param>
        /// <returns>A boolean value indicating if the save was successful.</returns>
        public static bool SaveDictionary(string saveName, Dictionary<string, string> data, bool overrideExistingData)
        {
            // If the savename doesn't exist yet or we're allowed to override it.
            if (GetResourceKvpString(saveName) == null || overrideExistingData)
            {
                // Get the json string from the dictionary.
                //string jsonString = CommonFunctions.DictionaryToJson(data);
                string jsonString = JsonConvert.SerializeObject(data);
                Log($"Saving: [name: {saveName}, json:{jsonString}]");

                // Save the kvp.
                SetResourceKvp(saveName, jsonString);

                // Return true if the kvp was set successfully, false if it wasn't set successfully.
                return GetResourceKvpString(saveName) == jsonString;
            }
            // If the data already exists and we are not allowed to override it.
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a collection of saved peds.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, PedInfo> GetSavedPeds()
        {
            Dictionary<string, PedInfo> savedPeds = new Dictionary<string, PedInfo>();

            int handle = StartFindKvp("ped_");
            while (true)
            {
                string kvp = FindKvp(handle);
                if (string.IsNullOrEmpty(kvp))
                {
                    break;
                }
                savedPeds.Add(kvp, JsonConvert.DeserializeObject<PedInfo>(GetResourceKvpString(kvp)));
            }
            return savedPeds;
        }

        /// <summary>
        /// Returns a <see cref="PedInfo"/> struct containing the data of the saved ped.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PedInfo GetSavedPedInfo(string name)
        {
            return JsonToPedInfo(GetResourceKvpString(name));
        }

        /// <summary>
        /// Saves an (old/nomral) ped data to storage.
        /// </summary>
        /// <param name="saveName"></param>
        /// <param name="pedData"></param>
        /// <param name="overrideExisting"></param>
        /// <returns></returns>
        public static bool SavePedInfo(string saveName, PedInfo pedData, bool overrideExisting)
        {
            if (overrideExisting || string.IsNullOrEmpty(GetResourceKvpString(saveName)))
            {
                SetResourceKvp(saveName, JsonConvert.SerializeObject(pedData));
                return GetResourceKvpString(saveName) == JsonConvert.SerializeObject(pedData);
            }
            return false;

        }

        public static List<MpPedDataManager.MultiplayerPedData> GetSavedMpPeds()
        {
            List<MpPedDataManager.MultiplayerPedData> peds = new List<MpPedDataManager.MultiplayerPedData>();
            var handle = StartFindKvp("mp_ped_");
            while (true)
            {
                string foundName = FindKvp(handle);
                if (string.IsNullOrEmpty(foundName))
                {
                    break;
                }
                else
                {
                    peds.Add(GetSavedMpCharacterData(foundName));
                }
            }
            EndFindKvp(handle);
            peds.Sort((a, b) => a.SaveName.ToLower().CompareTo(b.SaveName.ToLower()));
            return peds;
        }

        /// <summary>
        /// Delete the specified saved item from local storage.
        /// </summary>
        /// <param name="saveName">The full name of the item to remove.</param>
        public static void DeleteSavedStorageItem(string saveName)
        {
            DeleteResourceKvp(saveName);
        }

        /// <summary>
        /// Save json data. Returns true if save was successfull.
        /// </summary>
        /// <param name="saveName">Name to store the data under.</param>
        /// <param name="jsonData">The data to store.</param>
        /// <param name="overrideExistingData">If the saveName is already in use, can we override it?</param>
        /// <returns>Whether or not the data was saved successfully.</returns>
        public static bool SaveJsonData(string saveName, string jsonData, bool overrideExistingData)
        {
            if (!string.IsNullOrEmpty(saveName) && !string.IsNullOrEmpty(jsonData))
            {
                string existingData = GetResourceKvpString(saveName); // check for existing data.

                if (!string.IsNullOrEmpty(existingData)) // data already exists for this save name.
                {
                    if (!overrideExistingData)
                    {
                        return false; // data already exists, and we are not allowed to override it.
                    }
                }

                // write data.
                SetResourceKvp(saveName, jsonData);

                // return true if the data is successfully written, otherwise return false.
                return (GetResourceKvpString(saveName) ?? "") == jsonData;
            }
            return false; // input parameters are invalid.
        }

        /// <summary>
        /// Returns the saved json data for the provided save name. Returns null if no data exists.
        /// </summary>
        /// <param name="saveName"></param>
        /// <returns></returns>
        public static string GetJsonData(string saveName)
        {
            if (!string.IsNullOrEmpty(saveName))
            {
                //Debug.WriteLine("not null");
                string data = GetResourceKvpString(saveName);
                //Debug.Write(data + "\n");
                if (!string.IsNullOrEmpty(data))
                {
                    return data;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a <see cref="MpPedDataManager.MultiplayerPedData"/> struct containing the data of the saved MP Character.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MpPedDataManager.MultiplayerPedData GetSavedMpCharacterData(string name)
        {
            var output = new MpPedDataManager.MultiplayerPedData();
            if (string.IsNullOrEmpty(name))
            {
                return output;
            }
            string jsonString = GetResourceKvpString(name.StartsWith("mp_ped_") ? name : "mp_ped_" + name);
            if (string.IsNullOrEmpty(jsonString))
            {
                return output;
            }
            try
            {
                output = JsonConvert.DeserializeObject<MpPedDataManager.MultiplayerPedData>(jsonString);
            }
            catch (JsonException e)
            {
                Debug.WriteLine(e.Message);
            }
            Log(jsonString);
            return output;
        }
    }
}
