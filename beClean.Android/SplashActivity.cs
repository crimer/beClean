using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.App;

namespace beClean.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Label = "beClean")]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistantState)
        {
            base.OnCreate(savedInstanceState, persistantState);

            // Create your application here
        }
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
