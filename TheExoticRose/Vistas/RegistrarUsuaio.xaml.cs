using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrarUsuaio : ContentPage
    {
        public RegistrarUsuaio()
        {
            InitializeComponent();
        }

        private void btInsertar_Clicked(object sender, EventArgs e)
        {
            try
            {
                WebClient cliente = new WebClient();
                var parametros = new System.Collections.Specialized.NameValueCollection();

                // Validar que los campos no estén vacíos
                if (!string.IsNullOrEmpty(txtNombre.Text) &&
                    !string.IsNullOrEmpty(txtApellido.Text) &&
                    !string.IsNullOrEmpty(txtIdentificacion.Text) &&
                    !string.IsNullOrEmpty(txtTelefono.Text) &&
                    !string.IsNullOrEmpty(txtCorreo.Text) &&
                    !string.IsNullOrEmpty(txtUsuario.Text) &&
                    !string.IsNullOrEmpty(txtContrasena.Text))
                {
                    parametros.Add("nombreUsuario", txtNombre.Text);
                    parametros.Add("apellido", txtApellido.Text);
                    parametros.Add("identificacion", txtIdentificacion.Text);
                    parametros.Add("telefono", txtTelefono.Text);
                    parametros.Add("correo", txtCorreo.Text);
                    parametros.Add("usuario", txtUsuario.Text);
                    parametros.Add("contrasena", txtContrasena.Text);
                    parametros.Add("idRol", 1.ToString());
                    parametros.Add("estadoUsuario", 1.ToString());

                    cliente.UploadValues("http://192.168.1.109/roses/post.php", "POST", parametros);
                    DisplayAlert("Alerta", "Dato Ingresado", "Aceptar");
                }
                else
                {
                    DisplayAlert("Alerta", "Por favor, complete todos los campos", "Aceptar");
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