using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.Clases
{
    public class Servicio
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Aforo { get; set; }

        public List<Turno> Turnos { get; set; }
        public List<DetalleReserva> DetallesReservas { get; set; }
    }
}