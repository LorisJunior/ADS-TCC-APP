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
    public partial class ClickedUserPage : ContentPage
    {
        ClickedUserViewModel clickedUserViewModel;
        public ClickedUserPage(User user)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            clickedUserViewModel = DependencyService.Get<ClickedUserViewModel>();
            BindingContext = clickedUserViewModel;
            clickedUserViewModel.ClickedUser = user;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            clickedUserViewModel.InitClickedView();
        }
    }
}