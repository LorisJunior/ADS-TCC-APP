using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BindingPinView : StackLayout
    {
        public BindingPinView(Stream stream)
        {
            InitializeComponent();
            pinImage.Source = ImageSource.FromStream(() => stream);
        }
    }
}