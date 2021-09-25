using System;
using TCCApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            DependencyService.Get<ISetStatusBarColor>().SetStatusBarColor(Color.White, false);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
