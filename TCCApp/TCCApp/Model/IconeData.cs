using System;
using System.Collections.Generic;
using System.Text;

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
                    IconImage = "shorts.png"
                },

                new Icone
                {
                    IconImage = "clothesIcon.png"
                },

                new Icone
                {
                    IconImage = "JeansIcon.png"
                },

                new Icone
                {
                    IconImage = "camisaIcon.png"
                },

                new Icone
                {
                    IconImage = "sneakersIcon.png"
                },

                new Icone
                {
                    IconImage = "garrafaIcon.png"
                },

                new Icone
                {
                    IconImage = "beerIcon.png"
                },

                new Icone
                {
                    IconImage = "meatIcon.png"
                },

                new Icone
                {
                    IconImage = "trumpetIcon.png"
                },

                new Icone
                {
                    IconImage = "violinIcon.png"
                },

                new Icone
                {
                    IconImage = "phoneIcon.png"
                }
            };

        }
    }
}
