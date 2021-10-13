using System;
using System.Collections.Generic;
using System.Reflection;
using TCCApp.Helpers;
using TCCApp.Model;
using TCCApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp
{
    public partial class App : Application
    {
        public static Assembly assembly = null;
        public static User user = new User();
        public static string DatabasePath = string.Empty;

        public App(IOAuth2Service oAuth2Service, string databasePath)
        {
            //TODO
            //Chave temporária
            user.Key = "-MlqaI0xVuNArDGhXH-m";

            assembly = GetType().GetTypeInfo().Assembly;

            InitializeComponent();

            DatabasePath = databasePath;

            MainPage = new NavigationPage(new MainPage(oAuth2Service));

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
