using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
            /* var img = new CachedImage
             {
                 Aspect = Aspect.AspectFill,
                 CacheDuration = new TimeSpan(0, 5, 0),
                 Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(user.Buffer)),
                 Transformations = new List<ITransformation>()
                 {
                     new CircleTransformation(borderSize:40,"#68ce37"),
                 },
             };*/

            var i = new Image();
            i.Source = ImageSource.FromStream(() => new MemoryStream(user.Buffer));
            AbsoluteLayout.SetLayoutBounds(i, new Rectangle(x: 0, y: 0, width: width, height: height));

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
                            i
                        }
                    }
                }
            });
        }
    }
}
