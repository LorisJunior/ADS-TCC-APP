using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Services;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        LoginViewModel loginViewModel;
        public MainPage(IOAuth2Service oAuth2Service)
        {
            loginViewModel = new LoginViewModel(this, oAuth2Service);
            InitializeComponent();
            BindingContext = loginViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        

        private async void EsqSenhaButton_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new EsqSenhaPage());
        }

        //private async void EntrarButton_Clicked(object sender, EventArgs e)
       // {
         //   await Navigation.PushAsync(new HistoryPage());
        //}

        private async void CadastroButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroPage());
        }

    }
}
