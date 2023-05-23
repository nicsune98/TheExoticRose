using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheExoticRose.WS;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net;
using System.Collections.Specialized;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Invernaderos : ContentPage
    {
        string url = $"http://192.168.1.109/roses/post.php?tabla=invernadero";
        private readonly HttpClient client = new HttpClient();
        private ObservableCollection<TheExoticRose.WS.Invernadero> _post;
        public Invernaderos()
        {
            InitializeComponent();
            obtener();
        }
        public async void obtener()
        {

            var content = await client.GetStringAsync(url);
            List<TheExoticRose.WS.Invernadero> posts = JsonConvert.DeserializeObject<List<TheExoticRose.WS.Invernadero>>(content);
            _post = new ObservableCollection<TheExoticRose.WS.Invernadero>(posts);
            Lista.ItemsSource = _post;
        }

        private void Lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Invernadero invernaderoSeleccionado = (Invernadero)e.SelectedItem;
            int idInvernadero = invernaderoSeleccionado.idInvernadero;
            string latitud = invernaderoSeleccionado.latitud;
            string longitud = invernaderoSeleccionado.longitud;
            string nombre = invernaderoSeleccionado.nombreInvernadero;
            Navegar(idInvernadero,latitud, longitud, nombre);
        }
        public async void Navegar(int idInvernadero,string latitud,string longitud, string nombre)
        {
            if (!double.TryParse(latitud, out double lat)) //captura latitud
                return;
            if (!double.TryParse(longitud, out double lng)) //captura longitud
                return;
            await Map.OpenAsync(lat, lng, new MapLaunchOptions
            {
                Name = nombre,
                NavigationMode = NavigationMode.None
            });
        }

        private void btRegistro_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new IngresoMapa());
        }

        private void BtEditar_Clicked(object sender, EventArgs e)
        {
            string id = ((MenuItem)sender).CommandParameter.ToString();
            Navigation.PushAsync(new EditarMapa(id));
        }

        private async void BtEliminar_Clicked(object sender, EventArgs e)
        {
            string id = ((MenuItem)sender).CommandParameter.ToString();
            await DisplayAlert("id", "id " + id, "cerrar");
            if (await DisplayAlert("Confirmación", "¿Está seguro que desea eliminar el elementos. Esta acción no se puede revertir", "SI", "NO"))
            {
                //WebClient cliente = new WebClient();
                var eliminar = await client.DeleteAsync($"http://192.168.1.109/roses/postInvernadero.php?idInvernadero={id}");
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
    }
}