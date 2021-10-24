using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TCCApp.Model
{
    public class Notification
    {
        public string Author { get; set; }
        public string GroupKey { get; set; }
        public string Key { get; set; }
        public byte[] ByteImage { get; set; }
        public ImageSource Image { get; set; }
    }
}
