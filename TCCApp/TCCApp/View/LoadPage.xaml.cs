using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadPage : ContentPage
    {
        public static bool isLoading = false; 
        public LoadPage()
        {
            this.Title = "LoadPage";
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (isLoading)
            {
                isLoading = false;
                Navigation.RemovePage(this);
            }            
        }

        public async static void CallLoadingScreen()
        {
            isLoading = true;
            await App.Current.MainPage.Navigation.PushAsync(new LoadPage());
        }

        public static void CloseLoadingScreen()
        {            
            var page = App.Current.MainPage.Navigation.NavigationStack.Last();
            if (page.Title == "LoadPage" && isLoading) {
                isLoading = false;
                App.Current.MainPage.Navigation.RemovePage(page);
            }
        }
    }
}