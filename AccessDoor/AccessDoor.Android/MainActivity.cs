
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AccessDoor.Droid
{
    [Activity(Label = "AccessDoor", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState) {
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //Generate Token For Android
            var id = Preferences.Get("my_id", string.Empty);
            if (string.IsNullOrWhiteSpace(id)) {
                id = Android.OS.Build.Serial;
                if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0") {
                    try {
                        var context = Android.App.Application.Context;
                        id = Android.Provider.Settings.Secure.GetString(context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                    } catch (Exception ex) {
                        Android.Util.Log.Warn("DeviceInfo", "Unable to get id: " + ex.ToString());
                    }
                }
            }

            Preferences.Set("my_id", id);

            //Load App
            LoadApplication(new App(id));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [System.Obsolete]
        public override void OnBackPressed() {
            RunOnUiThread(
                () => {
                    var activity = (Activity)Forms.Context;
                    activity.FinishAffinity();
                });
        }

    }
}