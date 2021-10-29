using Plugin.Geolocator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TCCApp.Helpers;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

[assembly:Dependency(typeof(MapViewModel))]
namespace TCCApp.ViewModel
{
    public class MapViewModel : BaseViewModel
    {
        public Pin userPin;
        public ObservableCollection<Pin> Pins { get; private set; }

        public Plugin.Geolocator.Abstractions.IGeolocator locator = null;
        public IEnumerable PinCollection => Pins;
        public Circle UserCircle { get; set; }
        public Position CurrentPosition { get; set; }

        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public double radius = 3000;
        public double Radius
        {
            get { return radius; }
            set
            {
                Set(ref radius, value);
                UserCircle.Radius = Distance.FromMeters(radius);
            }
        }


        public MapViewModel()
        {
            UserCircle = new Circle();
            Pins = new ObservableCollection<Pin>();

            if (App.user.Latitude != 0 && App.user.Longitude != 0)
            {
                CurrentPosition = new Position(App.user.Latitude, App.user.Longitude);
            }

        }


        public ICommand GetNearUsers => new Command(async(s) =>
        {
            var btn = s as Button;

            btn.IsEnabled = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Pins.Count > 0)
                {
                    Pins.Clear();
                }
                SetPins(App.user, true);
            });
            var users = await FindNearUsers(radius);
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var user in users)
                {
                    SetPins(user, false);
                }
            });
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            btn.IsEnabled = true;

        });
        public void InitUser()
        {
            SetPins(App.user, true);
            SetCircle(CurrentPosition);
        }
        public async void LocatorStartListening()
        {
            try
            {
                locator = CrossGeolocator.Current;
                await locator.StartListeningAsync(new TimeSpan(0, 0, 0), 100);
                var position = await locator.GetPositionAsync();
                CurrentPosition = new Position(position.Latitude, position.Longitude);
            }
            catch (Exception)
            {
            }
        }

        public async void UpdatePosition()
        {
            App.user.Latitude = CurrentPosition.Latitude;
            App.user.Longitude = CurrentPosition.Longitude;
            await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
            userPin.Position = CurrentPosition;
            UserCircle.Center = CurrentPosition;
        }
        public async void SetPins(User user, bool isMyPin)
        {
            await semaphoreSlim.WaitAsync();

            BitmapDescriptor icon = null;

            if (user.Buffer != null)
            {
                icon = BitmapDescriptorFactory.FromView(ImageService.GetIcon(user));
            }
            else
            {
                icon = BitmapDescriptorFactory.DefaultMarker(Color.Red);
            }

            Pin pin = new Pin()
            {
                Icon = icon,
                Type = PinType.Place,
                Label = "Olá, vms comprar juntos!",
                ZIndex = 5,
                Address = user.Key
            };

            try
            {
                if (user.Latitude != 0 || user.Longitude != 0)
                {
                    pin.Position = new Position(user.Latitude, user.Longitude);
                }
            }
            catch (Exception)
            {
            }

            if (isMyPin == true)
            {
                userPin = pin;
                Pins.Add(userPin);
            }
            else
            {
                Pins.Add(pin);
            }

            semaphoreSlim.Release();
        }
        public async Task<IList<User>> FindNearUsers(double radius)
        {
            await semaphoreSlim.WaitAsync();

            var nearUsers = await DatabaseService.GetNearUsers(radius);

            var myItems = await DatabaseService.GetItems(App.user.Key);

            if (myItems.Count > 0 && nearUsers.Count > 0)
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

                semaphoreSlim.Release();
            }
            if (nearUsers.Count > 0)
            {
                return nearUsers;
                /*foreach (var user in nearUsers)
                {
                    SetPins(user, false);
                }*/
            }
            else
            {
                return null;
            }
        }
        public void SetCircle(Position position)
        {
            UserCircle.Center = position;
            UserCircle.Radius = Distance.FromMeters(radius);
            UserCircle.StrokeColor = Color.DodgerBlue;
            UserCircle.StrokeWidth = 6f;
            UserCircle.FillColor = Color.FromRgba(0, 0, 255, 32);
            UserCircle.Tag = "CIRCLE";
        }
    }
}