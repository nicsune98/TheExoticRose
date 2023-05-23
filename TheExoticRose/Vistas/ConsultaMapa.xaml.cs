using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultaMapa : ContentPage
    {
        public ConsultaMapa()
        {
            InitializeComponent();
        }

        private async void btNavegar_Clicked(object sender, EventArgs e)
        {
            if (!double.TryParse(txtLatitud.Text, out double lat)) //captura latitud
                return;
            if (!double.TryParse(txtLongitud.Text, out double lng)) //captura longitud
                return;
            await Map.OpenAsync(lat, lng, new MapLaunchOptions
            {
                Name = txtNombreUbicacion.Text,
                NavigationMode = NavigationMode.None
            });
        }
    }
}