using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Usuarios : ContentPage
    {
        string url = $"http://192.168.1.109/roses/post.php?tabla=usuario";
        private readonly HttpClient client = new HttpClient();
        private ObservableCollection<TheExoticRose.WS.Usuario> _post;
        public Usuarios()
        {
            InitializeComponent();
            obtener();
        }
        public async void obtener()
        {
            
            var content = await client.GetStringAsync(url);
            List<TheExoticRose.WS.Usuario> posts = JsonConvert.DeserializeObject<List<TheExoticRose.WS.Usuario>>(content);
            _post = new ObservableCollection<TheExoticRose.WS.Usuario>(posts);
            Lista.ItemsSource = _post;
        }

        private void BtEditar_Clicked(object sender, EventArgs e)
        {
            string id = ((MenuItem)sender).CommandParameter.ToString();
            Navigation.PushAsync(new EditarUsuario(id));
        }

        private async void BtEliminar_Clicked(object sender, EventArgs e)
        {
            string id = ((MenuItem)sender).CommandParameter.ToString();
            if (await DisplayAlert("Confirmación", "¿Está seguro que desea eliminar el elementos. Esta acción no se puede revertir", "SI", "NO"))
            {
                //WebClient cliente = new WebClient();
                var eliminar = await client.DeleteAsync($"http://192.168.1.109/roses/post.php?idUsuario={id}");
                if (eliminar.IsSuccessStatusCode)
                {
                    obtener();
                    await DisplayAlert("Mensaje", "Usuario eliminado", "Aceptar");
                }
                else
                {
                    await DisplayAlert("Mensaje", "No se pudo eliminar al usuario", "Aceptar");
                }
            }
        }

        private void btRegistro_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrarUsuaio());

        }
    }
}