using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                        latitude.Text =  Convert.ToString(position.Latitude);
                        longitude.Text = Convert.ToString(position.Longitude);

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

        private async void btguardar_Clicked(object sender, EventArgs e)
        {
            string nombreInvernadero = txtNombre.Text;
            string ciudad = txtCiudad.Text;
            string tecnico = txtTecnico.Text;
            string latitud = latitude.Text;
            string longitud = longitude.Text;

            // Validar que no haya campos de texto vacíos
            if (string.IsNullOrEmpty(nombreInvernadero) || string.IsNullOrEmpty(ciudad) || string.IsNullOrEmpty(tecnico) || string.IsNullOrEmpty(latitud) || string.IsNullOrEmpty(longitud))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios", "Aceptar");
                return;
            }

            // Extraer los tres primeros caracteres y el último del nombreInvernadero
            string codigoInvernadero = "";
            if (nombreInvernadero.Length >= 3)
            {
                codigoInvernadero += nombreInvernadero.Substring(0, 3);
            }
            if (nombreInvernadero.Length >= 1)
            {
                codigoInvernadero += nombreInvernadero.Substring(nombreInvernadero.Length - 1);
            }

            // Resto del código para enviar los datos al servicio web y guardar en la tabla "invernadero"
            try
            {
                HttpClient client = new HttpClient();
                var values = new Dictionary<string, string>
        {
            { "nombreInvernadero", nombreInvernadero },
            { "codigoInvernadero", codigoInvernadero },
            { "direccion", ciudad },
            { "tecnico", tecnico },
            { "longitud", longitud },
            { "latitud", latitud },
            { "estadoInvernadero", "1" } // Estado por defecto (puedes ajustarlo según tus necesidades
        };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("http://192.168.1.109/roses/postInvernadero.php", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Alerta", "Dato Ingresado", "Aceptar");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo insertar el dato", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            await Navigation.PushAsync(new Invernaderos());
        }
    }
}