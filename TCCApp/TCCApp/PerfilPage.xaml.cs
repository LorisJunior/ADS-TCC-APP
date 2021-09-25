using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilPage : ContentPage
    {
        public PerfilPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var media = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Pega uma foto"
                });
                var stream = await media.OpenReadAsync();
                image.Source = ImageSource.FromStream(() => stream);
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}