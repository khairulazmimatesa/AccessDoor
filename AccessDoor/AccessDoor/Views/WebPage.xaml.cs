using AccessDoor.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccessDoor.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage : ContentPage
    {
        string Id = "";
        string Url = "";

        public WebPage(string url, string id) {
            InitializeComponent();

            Id = id;
            Url = url;
            webView.Source = url;
            webView.IsVisible = false;
            NavigationPage.SetHasNavigationBar(this, false);


            BackgroundColor = Constants.BackgroungColor;
        }

        void Logout_Clicked(object sender, EventArgs e) {

            Navigation.PushModalAsync(new NavigationPage(new LogInPage(Id)));
        }

        void Retry_Clicked(object sender, EventArgs e) {

            webView.Source = (webView.Source as UrlWebViewSource).Url;
        }


        void webOnEndNavigating(object sender, WebNavigatedEventArgs e) {
            bool Allow = false;
            string Msg = "";

            switch (e.Result) {
                case WebNavigationResult.Failure:
                    Msg = "Please check your internet connection or contact system Admin";
                    webView.IsVisible = false;
                    ErrorMsg.Text = Msg;
                    Error.IsVisible = true;
                    btnRetry.IsVisible = true;
                    progress.IsVisible = false;

                    return;

                case WebNavigationResult.Success:
                    var ReturnUrl = e.Url;
                    int indexmsg = ReturnUrl.IndexOf('?');
                    if (ReturnUrl.Contains("msg=")) {
                        Msg = ReturnUrl.Substring(indexmsg + 1)
                         .Split('&')
                         .SingleOrDefault(s => s.StartsWith("msg="))
                         .Replace(@"msg=", "")
                         .Replace(@"%20", " ");


                        var Isallow = ReturnUrl.Substring(indexmsg + 1)
                                     .Split('&')
                                     .SingleOrDefault(s => s.StartsWith("IsAllow="))
                                     .Replace(@"IsAllow=", "");

                        Allow = bool.Parse(Isallow);


                    } 
                    break;
                case WebNavigationResult.Timeout:
                    Msg = "Please contact system Admin";
                    break;
            }

            if (!Allow) {
                webView.IsVisible = false;
                ErrorMsg.Text = Msg;
                Error.IsVisible = true;
                btnRetry.IsVisible = false;
                btnBack.IsVisible = true;

            } else {
                webView.IsVisible = true;
                Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                Application.Current.Properties["Url"] = Url;
                NavigationPage.SetHasNavigationBar(this, true);

            }

            progress.IsVisible = false;

        }

    }
}