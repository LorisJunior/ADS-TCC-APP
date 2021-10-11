using System;
using System.Collections.Generic;
using System.Text;
using TCCApp.Services;

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
                    IconImage = "shorts.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.shorts.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "clothesIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.clothesIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "jeansIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.jeansIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "camisaIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.camisaIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "sneakersIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.sneakersIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "garrafaIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.garrafaIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "beerIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.beerIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "meatIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.meatIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "trumpetIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.trumpetIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "violinIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.violinIcon.png",App.assembly),
                },

                new Icone
                {
                    IconImage = "phoneIcon.png",
                    ByteIcon = ImageService.ConvertToByte("TCCApp.Images.phoneIcon.png",App.assembly),
                }
            };

        }
    }
}
