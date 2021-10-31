using Xamarin.Forms;

namespace TCCApp.Model
{
    public class Item
    {
        public string Tipo { get; set; }
        public double Quantidade { get; set; }
        public string Descricao { get; set; }
        public Color Cor { get; set; }
        public double Hue { get; set; }
        public ImageSource ImageUrl { get; set; }
        public byte[] ByteImage { get; set; }
        public string UserKey { get; set; }
        public string Key { get; set; }
    }
}
