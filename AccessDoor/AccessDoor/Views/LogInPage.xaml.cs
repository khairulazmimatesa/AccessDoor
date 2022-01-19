using AccessDoor.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccessDoor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        private new readonly string Id = "";
        protected override void OnAppearing() {
            base.OnAppearing();

            //listView.ItemsSource = await App.Database.GetNotesAsync();
        }
        public LogInPage(string id) {
            Id = id;

            Application.Current.Properties.Clear();

            InitializeComponent();

            Init();
        }

        void Init() {
            BackgroundColor = Constants.BackgroungColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Key.TextColor = Constants.MainTextColor;
            Lbl_Url.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;

            NavigationPage.SetHasNavigationBar(this, false);

            LoginIcon.HeightRequest = Constants.LoginIconHeight;

            Entry_Url.Completed += (s, e) => Entry_Username.Focus();
            Entry_Username.Completed += (s, e) => Entry_Key.Focus();
            Entry_Key.Completed += (s, e) => SigninProcedure(s, e);
        }
        private void SigninProcedure(object sender, EventArgs e) {

            if (Entry_Username.Text != null && Entry_Key.Text != null) {

                string url = Entry_Url.Text + "?username=" + Entry_Username.Text + "&key=" + Entry_Key.Text + "&token=" + Id;

                Navigation.PushAsync(new WebPage(url, Id));
            } else {
                DisplayAlert("Login", "Empty Username or Key", "Okay");
            }
        }

    }
}