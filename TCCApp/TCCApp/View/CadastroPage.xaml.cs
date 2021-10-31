using System;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroPage : ContentPage
    {
        CadastroViewModel cadastroViewModel;
        public CadastroPage()
        {
            cadastroViewModel = new CadastroViewModel(this);
            InitializeComponent();
            BindingContext = cadastroViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async void OnEntrarConta(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}