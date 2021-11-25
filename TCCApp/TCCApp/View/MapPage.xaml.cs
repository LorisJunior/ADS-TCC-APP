using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        Circle userCircle = new Circle();

        Plugin.Geolocator.Abstractions.IGeolocator locator = null;
        public Position CurrentPosition { get; set; }

        Pin userPin;

        private double radius = 3000;
        public double Radius
        {
            get => radius;
            set
            {
                if (radius != value)
                {
                    radius = value;
                    OnPropertyChanged();
                    userCircle.Radius = Distance.FromMeters(Radius);
                }
            }
        }

        public MapPage()
        {
            InitializeComponent();

            BindingContext = this;

            map.UiSettings.MyLocationButtonEnabled = true;

            map.PinClicked += Map_PinClicked;

            LocatorStartListening();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!locator.IsListening)
            {
                LocatorStartListening();
            }

            SetPins(App.user, true);

            SetCircle(CurrentPosition);

            map.Circles.Add(userCircle);

            UpdatePosition();

            locator.PositionChanged += Locator_PositionChanged;
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            //Este delay evita unmanaged descriptor
            await Task.Delay(TimeSpan.FromMilliseconds(800));
            map.Pins.Clear();
            map.Circles.Clear();
            await locator.StopListeningAsync();
        }
        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            try
            {
                CurrentPosition = new Position(e.Position.Latitude, e.Position.Longitude);
                UpdatePosition();
            }
            catch (Exception)
            {
            }
        }
        private async void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            Pin pin = e.Pin;

            var userKey = (string)pin.Tag;

            if (userKey != App.user.Key)
            {
                var user = await DatabaseService.GetUserAsync(userKey);
                await Navigation.PushAsync(new ClickedUserPage(user));
            }
        }
        private void Search_Clicked(object sender, EventArgs e)
        {
            CleanMap(map.Pins);
            SetPins(App.user, true);
            FindNearUsers();
        }
        public async void LocatorStartListening()
        {
            try
            {
                await semaphoreSlim.WaitAsync();

                locator = CrossGeolocator.Current;
                if (locator.IsListening)
                    await locator.StopListeningAsync();
                await locator.StartListeningAsync(new TimeSpan(0, 0, 0), 100);
                var position = await locator.GetPositionAsync();
                CurrentPosition = new Position(position.Latitude, position.Longitude);

                semaphoreSlim.Release();
            }
            catch (Exception)
            {
                await DisplayAlert("Erro", "A localização não pode ser alcançada, por favor verifique a conexão com a internet e as permissões do aplicativo", "ok");
                await Navigation.PopAsync();
            }
        }
        public async void UpdatePosition()
        {
            App.user.Latitude = CurrentPosition.Latitude;
            App.user.Longitude = CurrentPosition.Longitude;
            await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
            userPin.Position = CurrentPosition;
            userCircle.Center = CurrentPosition;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(CurrentPosition, Distance.FromMeters(5000)), true);
        }
        public async void SetPins(User user, bool isMyPin)
        {
            await semaphoreSlim.WaitAsync();

            BitmapDescriptor icon = BitmapDescriptorFactory.DefaultMarker(Color.Red);
            Stream stream = null;

            try
            {
                stream = new MemoryStream(user.Buffer);
            }
            catch (Exception)
            {
            }

            try
            {
                if (user.Buffer != null)
                {
                    icon = BitmapDescriptorFactory.FromView(new BindingPinView(stream));
                }
            }
            catch (Exception)
            {
            }
            
            
            Pin pin = new Pin()
            {
                Icon = icon,
                Type = PinType.Place,
                Label = "Olá, vms comprar juntos!",
                ZIndex = 5,
                Tag = user.Key,
                Position = new Position(user.Latitude, user.Longitude)
            };
            try
            {
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
            catch (Exception)
            {
            }

            semaphoreSlim.Release();
        }
        public async void FindNearUsers()
        {

            var nearUsers = await DatabaseService.GetNearUsers(radius);

            if (nearUsers.Count > 0)
            {
                var myItems = await DatabaseService.GetItems(App.user.Key);

                if (myItems.Count > 0)
                {
                    int count = 0;

                    while (count < nearUsers.Count)
                    {
                        var items = await DatabaseService.GetItems(nearUsers[count].Key);
                        if (items.Count > 0)
                        {
                            var result = from m in myItems
                                         join i in items
                                         on m.Tipo.ToLowerInvariant() equals i.Tipo.ToLowerInvariant()
                                         select m.Key;

                            if (result == null)
                            {
                                nearUsers.RemoveAt(count);
                                count--;
                            }
                        }
                        else
                        {
                            nearUsers.RemoveAt(count);
                            count--;
                        }

                        count++;
                    }
                    if (nearUsers.Count > 0)
                    {
                        try
                        {
                            foreach (var user in nearUsers)
                            {
                                SetPins(user, false);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
        public void SetCircle(Position position)
        {
            userCircle.Center = position;
            userCircle.Radius = Distance.FromMeters(Radius);
            userCircle.StrokeColor = Color.DodgerBlue;
            userCircle.StrokeWidth = 6f;
            userCircle.FillColor = Color.FromRgba(0, 0, 255, 32);
            userCircle.Tag = "CIRCLE";
        }
        public void CleanMap(IEnumerable<object> o)
        {
            if (o is IList<Circle> && o != null)
            {
                try
                {
                    map.Circles.Clear();
                }
                catch (Exception)
                {
                }
            }
            else if (o is IList<Pin> && o != null)
            {
                try
                {
                    map.Pins.Clear();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}