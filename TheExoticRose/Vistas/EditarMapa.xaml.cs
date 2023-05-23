using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarMapa : ContentPage
    {
        public EditarMapa(string idInvernadero)
        {
            InitializeComponent();
            lblIdInvernadero.Text = idInvernadero;
            Obtener();
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
        public async void Obtener()
        {

            try
            {
                WebClient cliente = new WebClient();
                string tabla = "invernadero"; // Nombre de la tabla
                string idInvernadero =lblIdInvernadero.Text;
                string url = $"http://192.168.1.109/roses/postInvernadero.php?idInvernadero={idInvernadero}&tabla={tabla}";
                string content = cliente.DownloadString(url);
                var data = JsonConvert.DeserializeObject<TheExoticRose.WS.Invernadero>(content);

                // Asignar los datos a los controles
                txtNombre.Text = data.nombreInvernadero;
                txtTecnico.Text = data.tecnico;
                txtCiudad.Text = data.direccion;
                latitude.Text = data.latitud;
                longitude.Text = data.longitud;
                // Asignar otros campos si es necesario
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cerrar");
            }
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
                        latitude.Text = Convert.ToString(position.Latitude);
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

        private void btguardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                WebClient cliente = new WebClient();
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("idInvernadero", lblIdInvernadero.Text);
                parametros.Add("nombreInvernadero", txtNombre.Text);
                parametros.Add("codigoInvernadero", ""); // Agrega el valor correspondiente para códigoInvernadero
                parametros.Add("direccion", txtCiudad.Text);
                parametros.Add("tecnico", txtTecnico.Text);
                parametros.Add("longitud", longitude.Text);
                parametros.Add("latitud", latitude.Text);
                // También puedes agregar el campo "foto" si es necesario

                byte[] responseBytes = cliente.UploadValues("http://192.168.1.109/roses/postInvernadero.php", "PUT", parametros);
                string responseString = Encoding.UTF8.GetString(responseBytes);

                if (responseString != "OK")
                {
                    DisplayAlert("Mensaje", "Dato Actualizado", "Aceptar");
                    Navigation.PushAsync(new Invernaderos());
                }
                else
                {
                    DisplayAlert("Alerta", "No se pudo actualizar el dato", "Cerrar");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alerta", ex.Message, "Cerrar");
            }
        }
    }
}