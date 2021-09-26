using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkNetTest
{
    public partial class AddItemForm : Form
    {
        // Тип с которым работает форма. VkAccount/VKCommunity/Product
        private Type AddingType;
        private readonly List<string> ReceivedPhotos = new List<string>();

        public AddItemForm()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        }
        /// :VKCOM
        /// ID
        /// :PROD
        /// NAME & DESC & PRICE & PHOTO
        private void Button1_Click(object sender, EventArgs e)
        {
            if (AddingType == typeof(VKAccount) &&
                ((textBox1.Text.Trim().Length == 0 && textBox2.Text.Trim().Length == 0) || textBox3.Text.Trim().Length == 0))
            {
                MessageBox.Show("Необходимо заполнить поля логин/пароль или хотя бы поле токен.");
                return;
            }
            else if (AddingType == typeof(VkCommunity) && textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Необходимо ввести ID группы.");
                return;
            }
            else if (AddingType == typeof(Product) && 
                (textBox1.Text.Trim().Length == 0 ||
                textBox2.Text.Trim().Length == 0 ||
                textBox3.Text.Trim().Length == 0))
            {
                MessageBox.Show("Все поля обязательны к заполнению.");
                return;
            }

            // TODO: Добавление новых объектов акк/ком/прод
            if(AddingType == typeof(Product))
            {
                var name = textBox1.Text.Trim();
                var desc = textBox3.Text.Trim();
                if(double.TryParse(textBox2.Text.Trim(), out double price) == false)
                {
                    MessageBox.Show("Поле цены заполнено некорректно.");
                    return;
                }

                var photos = new List<string>();
                if (ReceivedPhotos?.Count > 0)
                    photos = ReceivedPhotos;

                var prod = new Product(name, desc, price, photos, addToCollection: true);
            }


            label5.Text = "Выбранные файлы:";
            this.Close();
        }

        private void AddItemForm_Load(object sender, EventArgs e)
        {
        }

        private void AddItemForm_Shown(object sender, EventArgs e)
        {
            var stringType = this.Text.Split(' ')[1];
            switch(stringType)
            {
                case "VkAccount":
                    AddingType = typeof(VKAccount);
                    break;
                case "VkCommunity":
                    AddingType = typeof(VkCommunity);
                    break;
                case "Product":
                    AddingType = typeof(Product);
                    break;
                default:
                    throw new Exception("Unexpected type.");
            }
        }

        private void AddItemForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            label5.Visible = false;
            Form1.MainForm.Enabled = true;
            Form1.MainForm.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ReceivedPhotos.Clear();
                ReceivedPhotos.AddRange(dialog.FileNames);
                label5.Text = "";
                foreach(var file in ReceivedPhotos)
                    label5.Text += file + "\n\r";
            }

        }
    }
}
