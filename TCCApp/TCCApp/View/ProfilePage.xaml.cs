using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Helpers;
using TCCApp.Services;
using TCCApp.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        ProfileViewModel profileViewModel;
        public ProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            profileViewModel = DependencyService.Get<ProfileViewModel>();
            BindingContext = profileViewModel;
            profileViewModel.SetProfile();
        }
        
        private async void Sobre_Completed(object sender, EventArgs e)
        {
            App.user.Sobre = sobre.Text;
            await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            profileViewModel.InitSubscription();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            profileViewModel.Notifications.SafeClear();
            profileViewModel.Subscription.Dispose();
            profileViewModel.NotificationSelectionMode = SelectionMode.Single;
            profileViewModel.DeleteButtonOpacity = 0.3;
            profileViewModel.IsDeleting = false;
        }
    }
}