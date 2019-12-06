using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.Clases
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public int IdServicio { get; set; }
        public string Dia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int Stock { get; set; }

        //Relaciones
        public Servicio Servicio { get; set; }
        public List<DetalleReserva> DetalleReservas { get; set; }
    }
}