using Android.App;
using Android.Content;
using Android.OS;
using System.Threading.Tasks;

namespace AccessDoor.Droid
{
    [Activity(Theme = "@style/SplashScreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            Finish();
        }

    }
}