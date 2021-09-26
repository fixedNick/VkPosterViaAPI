using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VkNetTest
{
    class IOController
    {
        private const string VK_ACCOUNT_FOLDER_PATH = "vkAccounts";
        private const string VK_COMMUNITY_FOLDER_PATH = "vkCommunities";
        private const string VK_PRODUCT_FOLDER_PATH = "products";
        private const string CONFIG_PATH = "config.txt";

        private static readonly ILogger Logger = new FileLogger();

        /// <summary>
        /// Load config at start of program
        /// </summary>
        public static void LoadConfig()
        {
            if (System.IO.File.Exists(CONFIG_PATH) == false)
                return;

            using (var file = new System.IO.StreamReader(CONFIG_PATH))
            {
                var fhalf = file.ReadLine().Split('=');
                if (fhalf?.Length > 1)
                {
                    switch(fhalf[0])
                    {
                        case "ProductLastID":
                            Product.LastUsedPID = Convert.ToInt32(fhalf[1]);
                            break;
                    }
                }
            }
            Logger.Print("IOController | Конфиг успешно загружен.");
        }

        /// <summary>
        /// Save current config vars data
        /// </summary>
        public static void UpadteConfig()
        {
            var file = new List<string>();
            file.Add($"ProductLastID={Product.LastUsedPID}");
            System.IO.File.WriteAllLines(CONFIG_PATH, file);

            Logger.Print("IOController | Конфиг успешно обновлен.");
        }

        public static void SaveItems<T>()
        {
            dynamic collection;
            if (typeof(T) == typeof(VKAccount))
                collection = VKAccount.Accounts;
            else if (typeof(T) == typeof(VkCommunity))
                collection = VkCommunity.Communities;
            else if (typeof(T) == typeof(Product))
                collection = Product.Products;
            else throw new Exception($"Операция невозможна для данного типа данных. [{typeof(T)}]");

            if( (collection as List<T>).Count == 0 )
            {
                Logger.Print($"IOCOntroller.SaveItems<{typeof(T)}> | Коллекция пуста.");
                return;
            }

            foreach (var collectionItem in collection)
            {
                var json = JsonConvert.SerializeObject(collectionItem)();
                string name = string.Empty;

                string folderName = string.Empty;
                string fileName = string.Empty;
                if (typeof(T) == typeof(VKAccount))
                {
                    folderName = VK_ACCOUNT_FOLDER_PATH;
                    fileName = (collectionItem as VKAccount).Login + ".txt";
                }
                else if (typeof(T) == typeof(VkCommunity))
                {
                    folderName = VK_COMMUNITY_FOLDER_PATH;
                    fileName = (collectionItem as VkCommunity).Domain + ".txt";
                }
                else if (typeof(T) == typeof(Product))
                {
                    folderName = VK_PRODUCT_FOLDER_PATH;
                    fileName = (collectionItem as Product).PID + ".txt";
                }
                else throw new Exception("IOController Exception. Unknown object type to save.");

                if(System.IO.Directory.Exists(folderName) == false)
                    System.IO.Directory.CreateDirectory(folderName);
                System.IO.File.WriteAllText($"{folderName}/{fileName}", json);
                Logger.Print($"IOController.SaveItems<{typeof(T)}> | File successfully saved: {folderName}/{fileName}");
            }
        }

        public static void LoadItems<T>()
        {
            string folderName;

            if (typeof(T) == typeof(VKAccount))
                folderName = VK_ACCOUNT_FOLDER_PATH;
            else if (typeof(T) == typeof(VkCommunity))
                folderName = VK_COMMUNITY_FOLDER_PATH;
            else if (typeof(T) == typeof(Product))
                folderName = VK_PRODUCT_FOLDER_PATH;
            else throw new Exception("IOContoller Exception. Unknown type to load.");

            if (System.IO.Directory.Exists(folderName) == false)
            {
                System.IO.Directory.CreateDirectory(folderName);
                Logger.Print($"IOController.LoadItems<{typeof(T)}> | Папка {folderName} не существует. Создаем.");
                return;
            }

            var files = System.IO.Directory.GetFiles(folderName);
            foreach(var file in files)
            {
                var json = System.IO.File.ReadAllText(file);
                var deserealizedObject = JObject.Parse(json).ToObject<T>();

                if (typeof(T) == typeof(VKAccount))
                    VKAccount.AddAccount(deserealizedObject as VKAccount);
                else if (typeof(T) == typeof(VkCommunity))
                    VkCommunity.AddCommunity(deserealizedObject as VkCommunity);
                else if (typeof(T) == typeof(Product))
                    Product.AddSavedProduct(deserealizedObject as Product);

                Logger.Print($"IOContoller.LoadItems<{typeof(T)}> | File successfully loaded: {file}");
            }

        }
    }
}
