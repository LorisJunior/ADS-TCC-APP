using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EsqSenhaPage : ContentPage
    {
        public EsqSenhaPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async void OnEnviarEmail(object sender, EventArgs e)
        {
            await DisplayAlert("Recuperar Senha", "Email enviado com sucesso!", "Ok");

            await Navigation.PopAsync();
        }
    }
}