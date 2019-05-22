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
    public partial class IntegratedApp : Form
    {
        public IntegratedApp()
        {
            InitializeComponent();
        }

#region callback StripMenuItems
        private void LoadStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                StreamReader sr = new StreamReader(openFileDialog.FileName);
                InputTextBox.Text = sr.ReadToEnd();
                sr.Close();
                InputTextBox.SelectionStart = InputTextBox.Text.Length;
            }
        }

        private void SaveStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.WriteLine(InputTextBox.Text);
                sw.Close();
            }
        }

        private void InfoStripMenuItem_Click(object sender, EventArgs e)
        {
            AdditionalWindow tacWindow = new AdditionalWindow("Справка");
            tacWindow.Show();
        }
        #endregion

        #region callback Buttons
        private void RunButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TO DO");
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            InputTextBox.Clear();
        }
        #endregion
        
        #region callback Windows
        private void TacItem_Click(object sender, EventArgs e)
        {
            AdditionalWindow tacWindow = new AdditionalWindow("TAC Window");
            tacWindow.Show();
        }

        private void CfgItem_Click(object sender, EventArgs e)
        {
            AdditionalWindow cfgWindow = new AdditionalWindow("CFG Window");
            cfgWindow.Show();
        }
        #endregion

    }
}
