using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btIniciar_Clicked(object sender, EventArgs e)
        {
            string usuarioIngresado = txtUsuario.Text;
            string contrasenaIngresado = txtContrasena.Text;
            string nombreUsuario = string.Empty; // Variable para almacenar el nombre de usuario

            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(usuarioIngresado) || string.IsNullOrWhiteSpace(contrasenaIngresado))
            {
                await DisplayAlert("Mensaje", "Ingrese un Usuario y Contraseña", "Aceptar");
            }
            else
            {
                // Realizar la consulta al servicio PHP
                string tabla = "usuario";
                string url = $"http://192.168.1.109/roses/post.php?tabla={tabla}";
                HttpClient client = new HttpClient();
                string response = await client.GetStringAsync(url);

                // Deserializar la respuesta JSON como una lista de objetos TheExoticRose.WS.Usuario
                List<TheExoticRose.WS.Usuario> datos = JsonConvert.DeserializeObject<List<TheExoticRose.WS.Usuario>>(response);

                // Verificar si existe una coincidencia en los datos obtenidos
                TheExoticRose.WS.Usuario usuarioEncontrado = datos.FirstOrDefault(dato => dato.usuario == usuarioIngresado && dato.contrasena == contrasenaIngresado);

                // Verificar si se encontró el usuario y contraseña correctos
                if (usuarioEncontrado != null)
                {
                    nombreUsuario = usuarioEncontrado.nombreUsuario; // Almacenar el nombre de usuario en la variable

                    await DisplayAlert("Éxito", "Inicio de sesión exitoso", "Aceptar");
                    await Navigation.PushAsync(new Vistas.Menu(nombreUsuario)); // Pasar el nombre de usuario a la ventana Menu
                }
                else
                {
                    await DisplayAlert("Error", "Usuario y/o contraseña incorrectos", "Aceptar");
                }
            }

        }
    }
}