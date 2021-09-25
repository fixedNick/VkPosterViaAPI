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

        public AddItemForm()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (AddingType == typeof(VKAccount))
            {
                //if ((textBox3.Text.Trim().Length == 0 && (textBox2.Text.Trim().Length == 0 && textBox1.Text.Trim().Length == 0)) ||
                //    ())
                //{
                //    MessageBox.Show("Необходимо заполнить поля логин/пароль или хотя бы поле токен.");
                //    return;
                //}
            }
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
            Form1.MainForm.Enabled = true;
            Form1.MainForm.Focus();
        }
    }
}
