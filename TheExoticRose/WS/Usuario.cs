using System;
using System.Collections.Generic;
using System.Text;

namespace TheExoticRose.WS
{
    internal class Usuario
    {
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string apellido { get; set; }
        public string identificacion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }

        public string usuario { get; set; }
        public string contrasena { get; set; }
        public int idRol { get; set; }
        public int estadoUsuario { get; set; }
    }
}
