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
        }
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var media = await MediaPicker.PickPhotoAsync();
                var stream = await media.OpenReadAsync();
                image.Source = ImageSource.FromStream(() => new MemoryStream(ImageService.ConvertToByte(stream)));
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}