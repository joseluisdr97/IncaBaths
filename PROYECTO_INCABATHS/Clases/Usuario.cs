using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.Clases
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdTipoUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string Perfil { get; set; }

        public TipoUsuario TipoUsuario { get; set; }
        public List<Reserva> Reservas { get; set; }
    }
}