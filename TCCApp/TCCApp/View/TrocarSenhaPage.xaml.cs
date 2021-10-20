using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrocarSenhaPage : ContentPage
    {
        TrocarSenhaViewModel trocarSenhaViewModel;
        public TrocarSenhaPage(User puser)
        {
            trocarSenhaViewModel = new TrocarSenhaViewModel(this, puser);
            InitializeComponent();
            BindingContext = trocarSenhaViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}