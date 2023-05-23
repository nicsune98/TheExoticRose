using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheExoticRose.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public Menu(string nombreUsuario)
        {
            InitializeComponent();
            lblUsuario.Text = nombreUsuario;
        }

        private void btUsuarios_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Usuarios());
        }
    }
}