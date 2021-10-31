using System.Collections.Generic;
using TCCApp.Services;
using Xamarin.Forms;

namespace TCCApp.Model
{
    class IconeData
    {
        public IList<Icone> Icones { get; set; }
        public IconeData()
        {
            Icones = new List<Icone>
            {
                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.shorts.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.shorts.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.clothesIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.clothesIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.jeansIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.jeansIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.camisaIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.camisaIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.sneakersIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.sneakersIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.garrafaIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.garrafaIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.beerIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.beerIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.meatIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.meatIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.trumpetIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.trumpetIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.violinIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.violinIcon.png",App.assembly)
                },

                new Icone
                {
                    IconImage = ImageSource.FromStream(()=>ImageService
                    .GetImageFromStream("TCCApp.Images.phoneIcon.png",App.assembly)),
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.phoneIcon.png",App.assembly)
                }
            };

        }
    }
}
