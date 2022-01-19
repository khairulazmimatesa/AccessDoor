using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using AccessDoorWinforms.Model;

namespace AccessDoorWinforms.UserControls
{
    public partial class LogInPageUC : UserControl
    {
        public LogInPageUC() {
            InitializeComponent();
        }

        private string GetCPUID() {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string cpuInfo = String.Empty;

            foreach (ManagementObject mo in moc) {
                if (cpuInfo == String.Empty) {
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                }
                mo.Dispose();
            }

            cpuInfo = cpuInfo.Replace(":", "");
            return cpuInfo;
        }
        public string Username { get; set; }
        public string Urltxt { get; set; }
        public string Key { get; set; }
        public string Token { get; set; }
        public string FullUrl { get; set; }

        public UserInputModel userInput { get {
                UserInputModel model = new UserInputModel();
                model.Username = Username;
                model.Key = Key;
                model.Url = Urltxt;
                model.FullUrl = FullUrl;
                model.Token = Token;
                return model;
            } }

        public void BtnLogIn_Click(object sender, EventArgs e) {
            string id = GetCPUID();

            // Checks the value of the text.
            if (txtUsername.Text.Length == 0 && txtKey.Text.Length == 0) {
                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "Please Insert Username and UserKey";
                string caption = "Error Detected in Input";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);
            } else if (txtUsername.Text.Length == 0) {

                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "Please Insert Username";
                string caption = "Error Detected in Input";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);

            } else if (txtKey.Text.Length == 0) {

                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "Please Insert UserKey";
                string caption = "Error Detected in Input";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
            } else {
                string Url = txtURL.Text + "?username=" + txtUsername.Text + "&key=" + txtKey.Text + "&token=" + id.ToLower();

                
                FullUrl = Url;
                Urltxt = txtURL.Text;
                Username = txtUsername.Text;
                Key = txtKey.Text;
                Token = id.ToLower();

                txtUsername.Text = "";
                txtKey.Text = "";

                //WebViewPageUC Webpage = new WebViewPageUC();
                ////WebviewUC Webpage = new WebviewUC();
                //Webpage.Show();
            }

        }
    }
}
