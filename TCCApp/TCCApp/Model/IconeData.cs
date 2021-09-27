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
            Icones = new List<Icone>();

            Icones.Add(new Icone
            {
                IconImage = "shorts.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "clothesIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "JeansIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "camisaIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "sneakersIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "garrafaIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "beerIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "meatIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "trumpetIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "violinIcon.png"
            });

            Icones.Add(new Icone
            {
                IconImage = "phoneIcon.png"
            });

        }
    }
}
