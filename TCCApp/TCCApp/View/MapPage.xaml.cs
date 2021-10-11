using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using TCCApp.Services;
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
            map.PinClicked += Map_PinClicked;
            //NavigationPage.SetHasNavigationBar(this, false);
        }
        

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            locator = CrossGeolocator.Current;
            await locator.StartListeningAsync(new TimeSpan(0, 0, 0), 100);
            var position = await locator.GetPositionAsync();
            var center = new Position(position.Latitude, position.Longitude);
            CreateCircleShapeAt(center);

            CreatePin(App.user, true);
            locator.PositionChanged += Locator_PositionChanged;

            map.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMeters(5000)), true);
            //TODO
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            CleanMap(map.Pins);
            //Para de receber as posições
            await locator.StopListeningAsync();
        }
        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            //TODO
            Pin pin = e.Pin;
            //int userId = (int)pin.Tag;
            //var user = User.GetById(userId);

            //Envio uma mensagem para a TabPage ir para a ChatPage
            //MessagingCenter.Send<object, int>(user, "click", 2);
            MessagingCenter.Send<object, int>(this, "click", 3);
        }
        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var center = new Position(e.Position.Latitude, e.Position.Longitude);
            //TODO
            //App.user = User.UpdatePosition(App.user, center);
            try
            {
                userPin.Position = center;

            }
            catch (Exception)
            {
            }
            CreateCircleShapeAt(center);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMeters(5000)), true);
        }
        public void CreatePin(User user, bool isMyPin)
        {
            //TODO
            Pin pin = new Pin()
            {
                //Icon = BitmapDescriptorFactory.FromStream(new MemoryStream(ImageService.ConvertToByte("TCCApp.Images.imageIcon.png", App.assembly))),
                Icon = BitmapDescriptorFactory.FromView(ImageService.GetIcon(user, 75, 75)),
                Type = PinType.Place,
                Label = "Olá, vms comprar juntos!",
                ZIndex = 5,
                //Tag = user.Id
            };
            if (user.Latitude != 0 || user.Longitude != 0)
            {
                pin.Position = new Position(user.Latitude, user.Longitude);
            }

            if (isMyPin == true)
            {
                userPin = pin;
                map.Pins.Add(userPin);
            }
            else
            {
                map.Pins.Add(pin);
            }
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