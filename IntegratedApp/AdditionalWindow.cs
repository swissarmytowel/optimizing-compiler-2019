using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegratedApp
{
    public partial class AdditionalWindow : Form
    {
        public AdditionalWindow(string WindowName = "AdditionalWindow", string fileName = "")
        {
            InitializeComponent();
            this.Text = WindowName;

            if (!fileName.Equals("") && System.IO.File.Exists(fileName)) {
                using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default)) {
                     var text = sr.ReadToEnd();
                    ConsoleTextBox.Text = text;
                }
            }
        }
        
        private void ConsoleTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            LaunchWeblink(e.LinkText);
        }

        /// <summary>
        /// Выполняет запуск браузера по ссылке
        /// </summary>
        /// <param name="url"> Ссылка, которая должна открыться в браузере </param>
        private void LaunchWeblink(string url)
        {
            if (IsHttpURL(url)) {
                Process.Start(url);
            }
        }

        /// <summary>
        /// Проверка, что ссылка действительна
        /// </summary>
        /// <param name="url"> Ссылка, которая должна открыться в браузере </param>
        /// <returns> true - ссылка действительна </returns>
        private bool IsHttpURL(string url)
        {
            return !string.IsNullOrWhiteSpace(url) 
                && (url.ToLower().StartsWith("http") || url.ToLower().StartsWith("https"));
        }
    }
}
