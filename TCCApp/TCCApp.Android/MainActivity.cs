using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using TCCApp.Helpers;
using System.Drawing;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.GoogleClient;
using TCCApp.Droid.Serviços;

[assembly: Dependency(typeof(TCCApp.Droid.SetStatusBar))]
namespace TCCApp.Droid
{
    [Activity(Label = "TCCApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            GoogleClientManager.Initialize(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App(new OAuth2Service()));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, data);
        }
    }

    public class SetStatusBar : ISetStatusBarColor
    {
        [Obsolete]
        public void SetStatusBarColor(System.Drawing.Color color, bool darkTheme)
        {
            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
                return;

            var activity = Platform.CurrentActivity;
            var window = activity.Window;
            window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(color.ToPlatformColor());

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                var flag = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
                window.DecorView.SystemUiVisibility = darkTheme ? 0 : flag;
            }
        }
    }
}