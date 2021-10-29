using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TCCApp.Helpers;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        MapViewModel mapViewModel;

        public MapPage()
        {
            InitializeComponent();

            map.UiSettings.MyLocationButtonEnabled = true;

            mapViewModel = DependencyService.Get<MapViewModel>();

            BindingContext = mapViewModel;

            mapViewModel.InitUser();

            map.PinClicked += Map_PinClicked;

            if (mapViewModel.CurrentPosition != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(mapViewModel.CurrentPosition, Distance.FromMeters(5000)), false);
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            
            mapViewModel.LocatorStartListening();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(mapViewModel.CurrentPosition, Distance.FromMeters(5000)), false);

            map.Circles.Add(mapViewModel.UserCircle);

            mapViewModel.UpdatePosition();
           
            mapViewModel.SetPins(App.user, true);

            mapViewModel.locator.PositionChanged += Locator_PositionChanged;

        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    mapViewModel.Pins.Clear();
                });

                map.Circles.Clear();
            }
            catch (Exception)
            {
            }

            await mapViewModel.locator.StopListeningAsync();
        }
        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            try
            {
                mapViewModel.CurrentPosition = new Position(e.Position.Latitude, e.Position.Longitude);
                mapViewModel.UpdatePosition();
                map.MoveToRegion(MapSpan.FromCenterAndRadius(mapViewModel.CurrentPosition, Distance.FromMeters(5000)), true);
            }
            catch (Exception)
            {
            }
        }
        private async void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            Pin pin = e.Pin;

            var userKey = (string)pin.Address;

            if (userKey != App.user.Key)
            {
                var user = await DatabaseService.GetUserAsync(userKey);
                await Navigation.PushAsync(new ClickedUserPage(user));
            }
        }
    }
}