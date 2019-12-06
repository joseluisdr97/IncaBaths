using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.Clases
{
    public class DetalleReserva
    {
        public int IdDetalleReserva { get; set; }
        public int IdReserva { get; set; }
        public int IdServicio { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public int IdTurno { get; set; }
        public string Fecha { get; set; }
        public Reserva Reserva { get; set; }
        public Servicio Servicio { get; set; }
        public Turno Turno { get; set; }
    }
}