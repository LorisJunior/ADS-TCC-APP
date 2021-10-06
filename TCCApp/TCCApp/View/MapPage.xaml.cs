using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        Circle circle;
        Plugin.Geolocator.Abstractions.IGeolocator locator = null;
        Pin userPin;
        private double raio = 3000;

        public double Raio
        {
            get => raio;
            set
            {
                if (raio != value)
                {
                    raio = value;
                    OnPropertyChanged();
                    UpdateCircleRadius();
                }
            }
        }
        public MapPage()
        {
            circle = new Circle();
            InitializeComponent();
            BindingContext = this;

            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            //NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            locator = CrossGeolocator.Current;
            locator.PositionChanged += Locator_PositionChanged;
            await locator.StartListeningAsync(new TimeSpan(0, 0, 0), 100);
            var position = await locator.GetPositionAsync();
            var center = new Position(position.Latitude, position.Longitude);
            CreateCircleShapeAt(center);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMeters(5000)), true);
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            CleanMap(map.Pins);
            //Para de receber as posições
            await locator.StopListeningAsync();
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var center = new Position(e.Position.Latitude, e.Position.Longitude);
            //userPin.Position = center;
            CreateCircleShapeAt(center);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMeters(5000)), true);
        }

        public void CreateCircleShapeAt(Position position)
        {
            CleanMap(map.Circles);

            circle.Center = position;
            circle.Radius = Distance.FromMeters(Raio);
            circle.StrokeColor = Color.DodgerBlue;
            circle.StrokeWidth = 6f;
            circle.FillColor = Color.FromRgba(0, 0, 255, 32);
            circle.Tag = "CIRCLE";

            map.Circles.Add(circle);
        }
        public void UpdateCircleRadius()
        {
            circle.Radius = Distance.FromMeters(Raio);
        }
        public void CleanMap(IEnumerable<object> o)
        {
            if (o is IList<Circle> && o != null)
            {
                map.Circles.Clear();
            }
            else if (o is IList<Pin> && o != null)
            {
                map.Pins.Clear();
            }
            else
            {
                return;
            }
        }
    }
}