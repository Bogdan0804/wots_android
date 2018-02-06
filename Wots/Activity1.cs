using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Wots
{
    [Activity(Label = "Wots"
        , ScreenOrientation = ScreenOrientation.Landscape
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        public static Android.Content.Res.AssetManager ASSETS;
        protected override void OnCreate(Bundle bundle)
        {
            ASSETS = Assets;
            base.OnCreate(bundle);
            var g = new Game1();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

