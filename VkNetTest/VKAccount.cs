using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VkNetTest
{
    [JsonObject]
    class VKAccount
    {
        /// <summary>
        /// Пул уникальных аккаунтов.
        /// </summary>
        public static List<VKAccount> Accounts { get; private set; } = new List<VKAccount>();
        [JsonProperty("Login")]
        public string Login { get; }
        [JsonProperty("Password")]
        public string Password { get; }
        [JsonProperty("AccessToken")]
        public string AccessToken { get; private set; }

        public VkApi Api;

        private static readonly ILogger Logger = new FileLogger();

        [JsonConstructor]
        public VKAccount(string login, string password)
        {
            Login = login;
            Password = password;
        }

        [JsonConstructor]
        public VKAccount(string login, string password, string token) : this(login, password) 
        { 
            AccessToken = token; 
        }

        /// <summary>
        /// Данная группа методов используется для добавления новых аккаунтов в общий пул.
        /// </summary>
        /// <returns>
        /// TRUE - Аккаунт успешно добавлен
        /// FALSE - Аккаунт с таким логином уже есть а пуле.
        /// </returns>
        public static bool AddAccount(VKAccount acc)
        {
            foreach(var a in Accounts)
            {
                if (a.Login.Equals(acc.Login))
                {
                    Logger.Print($"VKAccout.AddAccount() | Неудалось добавить аккаунт {acc.Login} в общий пул. Аккаунт с таким логином уже лежит в пуле.");
                    return false;
                }
            }
            Logger.Print($"VKAccout.AddAccount() | Аккаунт {acc.Login} успешно добавлен в общий пул.");
            Accounts.Add(acc);
            return true;
        }
        public static bool AddAccount(string login, string password)
            => AddAccount(new VKAccount(login, password));

        public static bool AddAccount(string login, string password, string token)
            => AddAccount(new VKAccount(login, password, token));

        /// <summary>
        /// Данный метод необходимо вызвать у каждого аккаунта, дабы к нему был привязан объект VkApi, 
        /// с помощью которогу будет производиться дальнейшая работа
        /// </summary>
        /// <returns>
        /// TRUE - Успешная авторизация 
        /// FALSE - Авторизация неудалась. 
        /// </returns>
        public bool Authorize()
        {
            Api = new VkApi();
            var settings = Settings.AddLinkToLeftMenu | Settings.Ads | Settings.AppWidget | Settings.Audio | Settings.Documents |
                            Settings.Email | Settings.Friends | Settings.Groups | Settings.Market | Settings.Notes | Settings.Notifications |
                            Settings.Notify | Settings.Offline | Settings.Pages | Settings.Photos | Settings.Statistic | Settings.Stats | Settings.Status |
                            Settings.Video | Settings.Wall;

            if (String.IsNullOrEmpty(AccessToken))
            {
                try
                {
                    Api.Authorize(new ApiAuthParams
                    {
                        ApplicationId = 7916841,
                        Login = this.Login,
                        Password = this.Password,
                        Settings = settings
                    });
                    AccessToken = Api.Token;
                    Logger.Print($"VKAccount.Authorize() | Авторизация успешно проведена для аккаунта {Login}");
                    return true;
                }
                catch(Exception ex) 
                when (ex is VkNet.Exception.UserAuthorizationFailException || ex is VkNet.Exception.VkAuthorizationException)
                {
                    Logger.Print($"VKAccount.Authorize() | Не удалось авторизовать аккаунт {Login}. Причина: Неверный логин/пароль");
                    return false;
                }
            }

            try
            {
                Api.Authorize(new ApiAuthParams { 
                    ApplicationId = 7916841,
                    AccessToken = this.AccessToken
                });
                var res = Api.Groups.Get(new VkNet.Model.RequestParams.GroupsGetParams());
                Logger.Print($"VKAccount.Authorize() | Авторизация успешно проведена для вккаунта {Login}");
                return true;
            }
            catch(VkNet.Exception.UserAuthorizationFailException)
            {
                Logger.Print($"VKAccount.Authorize() | Не удалось авторизовать аккаунт {Login}. Причина: Неверный токен. Пытаемся провести авторизацию по логин/пароль");
                this.AccessToken = null;
                return Authorize();
            }
        }

        /// <summary>
        /// Данный метод используется для выполнения метода Authorize для всех объектов типа VkAcoount
        /// </summary>
        public static void AuthorizeAllAccounts()
        {
            foreach (var acc in VKAccount.Accounts)
                acc.Authorize();
        }
    }
}
