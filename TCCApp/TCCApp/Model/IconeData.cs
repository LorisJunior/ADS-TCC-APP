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

            for (int i = 0; i < 10; i++)
            {
                Icones.Add(new Icone
                {
                    IconImage = "shorts.png"
                });
            }

        }
    }
}
