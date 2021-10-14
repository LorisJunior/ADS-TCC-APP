using FFImageLoading.Work;
using System.IO;
using System.Reflection;
using TCCApp.Model;
using Xamarin.Forms;

namespace TCCApp.Services
{
    public class ImageService
    {
        public static byte[] ConvertToByte(string path, Assembly assembly)
        {
            var stream = GetImageFromStream(path, assembly);
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var buffer = memoryStream.ToArray();
            return buffer;
        }
        public static byte[] ConvertToByte(Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var buffer = memoryStream.ToArray();
            return buffer;
        }
        public static Stream GetImageFromStream(string path, Assembly assembly)
        {
            Stream stream = assembly.GetManifestResourceStream(path);
            return stream;
        }

        public static Xamarin.Forms.View GetIcon(User user)
        {
            const double TAM = 75;
            var stream = new MemoryStream(user.Buffer);
            Xamarin.Forms.ImageSource temp = Xamarin.Forms.ImageSource.FromStream(() => stream);


            var img = new ImageButton
            {
                Source = temp,
                Aspect = Aspect.AspectFill,
                CornerRadius = 38,
                BackgroundColor = Color.Transparent
            };

            var placeholder = new ImageButton
            {
                Source = "user.png",
                CornerRadius = 38,
            };

            AbsoluteLayout.SetLayoutBounds(img, new Rectangle(x: 0, y: 0, width: TAM, height: TAM));
            AbsoluteLayout.SetLayoutFlags(img, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(placeholder, new Rectangle(x: 0, y: 0, width: TAM, height: TAM));
            AbsoluteLayout.SetLayoutFlags(placeholder, AbsoluteLayoutFlags.None);
            return new AbsoluteLayout
            {
                WidthRequest = TAM,
                HeightRequest = TAM,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AnchorX = 0.5,
                AnchorY = 1,
                Children =
                {
                   // new StackLayout
                   // {
                       // Children =
                       // {
                            placeholder,
                            img
                       // }
                   // }
                }
            };
        }
    }
}
