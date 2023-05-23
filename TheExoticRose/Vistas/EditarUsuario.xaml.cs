using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheExoticRose.WS;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarUsuario : ContentPage
    {
        public EditarUsuario(string id)
        {
            InitializeComponent();
            lblCodigo.Text = id;
            Obtener();
        }

        public async void Obtener()
        {
            try
            {
                WebClient cliente = new WebClient();
                string codigo = lblCodigo.Text;
                string tabla = "usuario"; // Nombre de la tabla
                string url = $"http://192.168.1.109/roses/post.php?idUsuario={codigo}&tabla={tabla}";
                string content = cliente.DownloadString(url);
                var data = JsonConvert.DeserializeObject<TheExoticRose.WS.Usuario>(content);
                txtNombre.Text = data.nombreUsuario;
                txtApellido.Text = data.apellido;
                txttelefono.Text = data.telefono;
                txtCorreo.Text = data.correo;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", ex.Message, "Cerrar");
            }
        }

        private void btGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                WebClient cliente = new WebClient();
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("idUsuario", lblCodigo.Text);
                parametros.Add("nombreUsuario", txtNombre.Text);
                parametros.Add("apellido", txtApellido.Text);
                parametros.Add("telefono", txttelefono.Text);
                parametros.Add("correo", txtCorreo.Text);
                byte[] responseBytes = cliente.UploadValues("http://192.168.1.109/roses/post.php", "PUT", parametros);
                string responseString = Encoding.UTF8.GetString(responseBytes);
                if (responseString != "OK")
                {
                    DisplayAlert("Mensaje", "Dato Actualizado", "Aceptar");
                    Navigation.PushAsync(new Usuarios());
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

        private void btRegresar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Usuarios());
        }
    }
}