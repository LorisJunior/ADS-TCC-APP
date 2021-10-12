using System.IO;
using System.Reflection;


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
        
    }
}
