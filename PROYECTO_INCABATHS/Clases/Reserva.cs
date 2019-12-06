using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.Clases
{
    public class Reserva
    {
        public Reserva()
        {
            DetalleReservas = new List<DetalleReserva>();
        }
        public int IdReserva { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public int IdModoPago { get; set; }
        public decimal Total { get; set; }
        public List<DetalleReserva> DetalleReservas { get; set; }
        public Usuario Usuario { get; set; }
    }
}