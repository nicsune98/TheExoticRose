using System;
using System.Collections.Generic;
using System.Text;

namespace TheExoticRose.WS
{
    class Invernadero
    {
        public string nombreInvernadero { get; set; }
        public string codigoInvernadero { get; set; }
        public string direccion { get; set; }
        public string tecnico { get; set; }
        public string longitud { get; set; }
        public string latitud { get; set; }
        public byte[] foto { get; set; }
        public int estadoInvernadero { get; set; }

    }
}
