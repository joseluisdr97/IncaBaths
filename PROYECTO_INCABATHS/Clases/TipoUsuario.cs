using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.Clases
{
    public class TipoUsuario
    {
        public int IdTipoUsuario { get; set; }
        public string Nombre { get; set; }

        public List<Usuario> Usuarios { get; set; }
    }
}