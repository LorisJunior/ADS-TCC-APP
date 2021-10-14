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

        public static Xamarin.Forms.View GetIcon(User user, double width, double height)
        {
            var img = new ImageButton
            {
                Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(user.Buffer)),
                Aspect = Aspect.AspectFill,
                CornerRadius = 38,
                BackgroundColor = Color.SkyBlue
            };

            var placeholder = new ImageButton
            {
                Source = Xamarin.Forms.ImageSource.FromStream(() =>new MemoryStream(ImageService.ConvertToByte("TCCApp.Images.user.png", App.assembly))),
                Aspect = Aspect.AspectFill,
                CornerRadius = 38,
                BackgroundColor = Color.SkyBlue
            };

            AbsoluteLayout.SetLayoutBounds(img, new Rectangle(x: 0, y: 0, width: width, height: height));

            return (new StackLayout
            {
                WidthRequest = width,
                HeightRequest = height,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AnchorX = 0.5,
                AnchorY = 1,
                Children =
                {
                    new AbsoluteLayout
                    {
                        Children =
                        {
                            placeholder,
                            img
                        }
                    }
                }
            });
        }
    }
}
