using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            SetProfile();

            /*var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => PickPhoto();
            profileImage.GestureRecognizers.Add(tapGestureRecognizer);*/
        }
        public async void PickPhoto()
        {
            try
            {
                var media = await MediaPicker.PickPhotoAsync();
                var stream = await media.OpenReadAsync();
                var buffer = ImageService.ConvertToByte(stream);
                profileImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));

                App.user.Buffer = buffer;
                await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
            }
            catch (Exception)
            {
            }
        }
        public async void SetProfile()
        {

            App.user = await DatabaseService.GetUserAsync(App.user.Key);

            try
            {
                email.Text = App.user.Email;

                if (App.user.Sobre != null)
                {
                    sobre.Text = App.user.Sobre;
                }
                if (App.user.Buffer != null)
                {
                    profileImage.Source = ImageSource.FromStream(() => new MemoryStream(App.user.Buffer));
                }
                if (App.user.Nome != null)
                {
                    nome.Text = App.user.Nome;
                }
            }
            catch (Exception)
            {
            }
            
        }
        private async void Nome_Completed(object sender, EventArgs e)
        {
            App.user.Nome = nome.Text;
            await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
        }
        private async void DeleteSobre_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("Apagar sobre", "Tem certeza que deseja apagar o conteúdo sobre você?", "não", "sim");
            if (!action)
            {
                sobre.Text = string.Empty;
            }
            return;
        }

        private async void Sobre_Completed(object sender, EventArgs e)
        {
            App.user.Sobre = sobre.Text;
            await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
        }

        private void profileImage_Clicked(object sender, EventArgs e)
        {
            PickPhoto();
        }

        private async void DisplayUser_Clicked(object sender, EventArgs e)
        {
            if (App.user.DisplayUserInMap)
            {
                var opt = await DisplayAlert("Mudar visibilidade", "Você deseja que os outros usuários não possam te ver no mapa?", "não", "sim");
                if (!opt)
                {
                    App.user.DisplayUserInMap = false;
                    DisplayUser.BackgroundColor = Color.FromHex("#FFA8BD");
                    await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
                }
            }
            else
            {
                var opt = await DisplayAlert("Mudar visibilidade", "Você deseja que os outros usuários possam te ver no mapa?", "não", "sim");
                if (!opt)
                {
                    App.user.DisplayUserInMap = true;
                    DisplayUser.BackgroundColor = Color.FromHex("#F5BDEF");
                    await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
                }
            }
        }
    }
}