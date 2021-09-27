using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TCCApp.Helpers
{
    class ItemColor : INotifyPropertyChanged
    {
        private Color hslColor;
        private double hue, saturation, luminosity;
        public event PropertyChangedEventHandler PropertyChanged;
        public ItemColor()
        {
            Luminosity = 0.85;
            Saturation = 0.73;
        }

        public Color HslColor
        {
            get { return hslColor; }
            set
            {
                if (hslColor != value)
                {
                    hslColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Hue
        {
            get { return hue; }
            set
            {
                if (hue != value)
                {
                    hue = value;
                    OnPropertyChanged();
                    UpdateColor();
                }
            }
        }

        public double Saturation
        {
            get { return saturation; }
            set
            {
                if (saturation != value)
                {
                    saturation = value;
                    OnPropertyChanged();
                    UpdateColor();
                }
            }

        }

        public double Luminosity
        {
            get { return luminosity; }
            set
            {
                if (luminosity != value)
                {
                    luminosity = value;
                    OnPropertyChanged();
                    UpdateColor();
                }
            }
        }

        public void UpdateColor()
        {
            HslColor = Color.FromHsla(Hue, Saturation, Luminosity, 1);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
