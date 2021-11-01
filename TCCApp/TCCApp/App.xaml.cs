using System.Reflection;
using TCCApp.Helpers;
using TCCApp.Model;
using TCCApp.Services;
using Xamarin.Forms;

namespace TCCApp
{
    public partial class App : Application
    {
        public static Assembly assembly = null;
        public static User user = new User();
        public static string DatabasePath = string.Empty;
        public static string computerVisionKey = "c2f8943ffdb442958bff7ac826cdec3f";
        public static string computerVisionEndPoint = "https://adultcheck.cognitiveservices.azure.com/";

        public App(IOAuth2Service oAuth2Service, string databasePath)
        {
            //TODO
            //Chave temporária
            //user.Key = "-MlpToSLno7tg7r5w8KA";

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
