using System;
using System.Collections.Generic;
using TCCApp.Helpers;
using TCCApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp
{
    public partial class App : Application
    {
        public IList<Item> Items { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            var statusBarColor = DependencyService.Get<ISetStatusBarColor>();

            statusBarColor.SetStatusBarColor(Color.White, false);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
