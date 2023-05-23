using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresoMapa : ContentPage
    {
        public IngresoMapa()
        {
            InitializeComponent();
        }
        private async void Localizar(double latitud, double longitud, string ubicacion)
        {
            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = ubicacion,
                Position = new Position(latitud, longitud)
            };
            Mapa.Pins.Add(pin);
            Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(300)));

        }
        private async void btCapturar_Clicked(object sender, EventArgs e)
        {
            if (CrossGeolocator.Current.IsGeolocationAvailable)
            {
                if (CrossGeolocator.Current.IsGeolocationEnabled)
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                    Device.BeginInvokeOnMainThread(() => {
                        latitude.Text = $"Latitud : {position.Latitude}";
                        longitude.Text = $"Longitud : {position.Longitude}";

                    });
                    Localizar(position.Latitude, position.Longitude, "Ubicacion Actual");
                }
                else
                {
                    await DisplayAlert("Message", "GPS Not Enabled", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Message", "GPS Not Available", "Ok");
            }
        }
    }
}