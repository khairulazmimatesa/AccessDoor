using System;
using System.Management;
using System.Windows;

namespace AccessDoor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() {
            InitializeComponent();


        }
        private void Login_Click(object sender, RoutedEventArgs e) {
            string id = GetCPUID();

            if (txtUsername.Text.Length == 0 && txtKey.Text.Length == 0) {
                errormessage.Text = "Enter an email and key.";
                txtUsername.Focus();
                txtKey.Focus();
            } else if (txtUsername.Text.Length == 0) {

                errormessage.Text = "Enter an email.";
                txtUsername.Focus();

            } else if (txtKey.Text.Length == 0) {

                errormessage.Text = "Enter teh Key.";
                txtKey.Focus();
            } else {
                string Url = txtUrl.Text + "?username=" + txtUsername.Text + "&key=" + txtKey.Text + "&token=" + id.ToLower();

                LogInGrid.Visibility = Visibility.Hidden;

                Browser.Url = Url;

                Browser.LoadCompleted += UnhideGrid;

            }
        }

        void UnhideGrid(object sender, LoadCompletedEventArgs e) {
            WebBrowserGrid.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Visible;
        }

        public static string GetCPUID() {

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
        private void LogoutClick(object sender, RoutedEventArgs e) {
            WebBrowserGrid.Visibility = Visibility.Hidden;
            LogoutButton.Visibility = Visibility.Visible;


            txtUsername.Text = "";
            txtKey.Text = "";

            LogInGrid.Visibility = Visibility.Visible;


        }

    }
}
