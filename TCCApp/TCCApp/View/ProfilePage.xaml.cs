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

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => PickPhoto();
            profileImage.GestureRecognizers.Add(tapGestureRecognizer);
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
                await DatabaseService.UpdateUser(App.user);
            }
            catch (Exception)
            {
            }
        }
        public async void SetProfile()
        {
            App.user = await DatabaseService.GetUser(App.user.Id);

            if (App.user.Buffer == null)
            {
                profileImage.Source = "user.png";
            }
            else
            {
                profileImage.Source = ImageSource.FromStream(() => new MemoryStream(App.user.Buffer));
            }

            if (App.user.Nome != null)
            {
                nome.Text = App.user.Nome;
            }
        }
        private async void Nome_Completed(object sender, EventArgs e)
        {
            App.user.Nome = nome.Text;
            await DatabaseService.UpdateUser(App.user);
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
    }
}