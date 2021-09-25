using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VkNetTest
{
    public partial class Form1 : Form
    {
        public static readonly string ServiceKey = "6d460e536d460e536d460e53876d3ec37a66d466d460e530c41393b372091d45a68989d";
        public static readonly string SecureKey = "MgbmshapJLzQmCNKbNLm";
        public static readonly string ApiVersion = "5.131";
        public static readonly string AppID = "7916841";
        public static readonly long ScopeForUserToken = 140491999 - 4096; // 4096 - Messages API

        public static Form1 MainForm { get; private set; }

        public Form1()
        {
            InitializeComponent();
            IOController.LoadItems<VKAccount>();
            IOController.LoadItems<VkCommunity>();
            IOController.LoadItems<Product>();
            IOController.LoadConfig();
            MainForm = this;
        }
        /// Рабочая версия постинга в сообщество
        public void MakePost()
        {
            VKAccount.Accounts.Add(new VKAccount("+79992092376", "ghjcnjnfR1997pRo"));
            var acc = VKAccount.Accounts[0];
            acc.Authorize();

            MessageBox.Show(acc.AccessToken);
            var res = acc.Api.Groups.Get(new GroupsGetParams());
            MessageBox.Show(res.TotalCount.ToString());

            // Получить адрес сервера для загрузки.
            var albumid = acc.Api.Photo.CreateAlbum(new PhotoCreateAlbumParams() { Title = "GAGARIN" }).Id;
            var uploadServer = acc.Api.Photo.GetUploadServer(albumid);
            // Загрузить файл.
            var wc = new WebClient();
            var uploadedFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "vaptio02.jpg"));
            // Сохранить загруженный файл
            var photos = acc.Api.Photo.Save(new PhotoSaveParams
            {
                SaveFileResponse = uploadedFile,
                AlbumId = albumid
            });

            acc.Api.Wall.Post(new WallPostParams()
            {
                OwnerId = -163370732,
                Message = "Продам вейп\nОчень красивый\nДешего, пишите!",
                Attachments = photos
            });
        }

        private void button1_Click(object sender, EventArgs e)
        //=> AddFromFile<VKAccount>();
        {
            MainForm.Enabled = false;

            var form = new AddItemForm();
            (form.Controls["label4"] as Label).Visible = false;
            (form.Controls["button2"] as System.Windows.Forms.Button).Visible = false;

            (form.Controls["label1"] as Label).Text = "Логин";
            (form.Controls["label2"] as Label).Text = "Пароль";
            (form.Controls["label3"] as Label).Text = "Токен";

            form.Text = "Add VkAccount";

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        //=> AddFromFile<Product>();
        {
            MainForm.Enabled = false;

            var form = new AddItemForm();
            (form.Controls["label1"] as Label).Text = "Название";
            (form.Controls["label2"] as Label).Text = "Цена";
            (form.Controls["label3"] as Label).Text = "Описание";
            (form.Controls["label4"] as Label).Text = "Фото";

            (form.Controls["textBox3"] as TextBox).Multiline = true;
            (form.Controls["textBox3"] as TextBox).Height = 40;

            form.Text = "Add Product";

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        //=> AddFromFile<VkCommunity>();
        {
            MainForm.Enabled = false;
            var form = new AddItemForm();
            
            (form.Controls["label4"] as Label).Visible = false;
            (form.Controls["button2"] as System.Windows.Forms.Button).Visible = false;
            (form.Controls["label3"] as Label).Visible = false;
            (form.Controls["textBox3"] as TextBox).Visible = false;

            (form.Controls["label2"] as Label).Text = "ID";

            form.Text = "Add VkCommunity";

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }

        // Вариант конвертации JSON файлов в объекты наших типов 
        //private void AddFromFile<T>()
        //{
        //    OpenFileDialog dialog = new OpenFileDialog();
        //    dialog.Filter = "Text files (*.txt)|*.txt";
        //    dialog.Multiselect = true;

        //    if (dialog.ShowDialog() == DialogResult.OK)
        //    {
        //        //if (dialog?.FileNames?.Length == 0) return;

        //        foreach (var file in dialog.FileNames)
        //        {
        //            try
        //            {
        //                var json = System.IO.File.ReadAllText(file);
        //                var deserealizedObject = JObject.Parse(json).ToObject<T>();
        //                if (typeof(T) == typeof(VKAccount))
        //                    VKAccount.AddAccount(deserealizedObject as VKAccount);
        //                else if (typeof(T) == typeof(Product))
        //                    Product.AddProduct(deserealizedObject as Product);
        //                else if (typeof(T) == typeof(VkCommunity))
        //                    VkCommunity.AddCommunity(deserealizedObject as VkCommunity);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Form1.51_str" + ex.Message);
        //            }
        //        }
        //    }
        //}
    }
}
