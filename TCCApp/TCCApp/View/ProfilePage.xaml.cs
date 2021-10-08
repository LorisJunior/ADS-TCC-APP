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
            SetProfileImage();
        }
        public async void SetProfileImage()
        {
            App.user = await DatabaseService.GetUser(App.user.Id);
            image.Source = ImageSource.FromStream(() => new MemoryStream(App.user.Buffer));
        }
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var media = await MediaPicker.PickPhotoAsync();
                var stream = await media.OpenReadAsync();
                var buffer = ImageService.ConvertToByte(stream);
                image.Source = ImageSource.FromStream(() => new MemoryStream(buffer));

                App.user.Buffer = buffer;
                await DatabaseService.UpdateUser(App.user);

            }
            catch (NullReferenceException)
            {
            }
        }
    }
}