using AccessDoorWinforms.Model;
using AccessDoorWinforms.UserControls;
using CefSharp;
using CefSharp.WinForms;
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
using System.Xml.Serialization;

namespace AccessDoorWinforms.Views
{
    public partial class MainPage : Form
    {
        private ChromiumWebBrowser Browser;

        public string Url { get; set; }

        public void InitBrowser(string url) {
                Browser = new ChromiumWebBrowser(url);
                this.Controls.Add(Browser);
                Browser.Dock = DockStyle.Fill;

                 Browser.LoadingStateChanged += OnLoadingStateChanged;

    }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e) {
            if (!e.IsLoading) {

                    this.Invoke(new MethodInvoker(delegate {
                        btnLogout.Visible = true;
                        LoadingImage.Visible = false;
                    }));
            }

        }

        public MainPage() {
            InitializeComponent();

            btnLogout.Visible = false;
            LoadingImage.Visible = false;
        }

        private void BtnLogout_Click(object sender, EventArgs e) {
            this.Controls.Remove(Browser);
            Browser.Dispose();
            btnLogout.Visible = false;
            logInPageUC1.Show();
            btnLogIn.Visible = true;
            LoadingImage.Visible = true;

        }

        private void BtnLogIn_Click(object sender, EventArgs e) {
            
            logInPageUC1.BtnLogIn_Click(sender, e);
            Url = logInPageUC1.userInput.FullUrl;
            logInPageUC1.Visible = false;
            btnLogIn.Visible = false;
            LoadingImage.Visible = true;

            InitBrowser(Url);
        }

        private void MainPage_Load(object sender, EventArgs e) {
            Url =  Properties.Settings.Default.Url;

            if (Url != "") {
                InitBrowser(Url);
                logInPageUC1.Visible = false;
                btnLogIn.Visible = false;


            } else {
                logInPageUC1.Visible = true;
                btnLogIn.Visible = true;

            }
        }
        private void MainPage_FormClosing(object sender, EventArgs e) {
            if (logInPageUC1.Visible == false) {

                Properties.Settings.Default.Url = Url;
                Properties.Settings.Default.Save();
            } else {
                Properties.Settings.Default.Url = "";
                Properties.Settings.Default.Save();
            }
        }
    }
}
