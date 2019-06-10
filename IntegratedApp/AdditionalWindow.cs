using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
    }
}
