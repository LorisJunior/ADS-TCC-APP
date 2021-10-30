using FFImageLoading.Work;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using TCCApp.Model;
using Xamarin.Forms;

namespace TCCApp.Services
{
    public class ImageService
    {
        const double TAM = 75;
        
        public static ImageButton placeholder = new ImageButton
        {
            Source = "user.png",
            CornerRadius = 38,
        };
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
            var img = new ImageButton
            {
                Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(user.Buffer)),
                Aspect = Aspect.AspectFill,
                CornerRadius = 38,
                BackgroundColor = Color.Transparent
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
                    placeholder,
                    img
                }
            };
        }        

        public static Task<byte[]> DownloadImage(string imageUrl)
        {
            HttpClient _client = new HttpClient();

            if (!imageUrl.Trim().StartsWith("https", StringComparison.OrdinalIgnoreCase))
                throw new Exception("iOS and Android Require Https");

            return _client.GetByteArrayAsync(imageUrl);
        }

    }
}
