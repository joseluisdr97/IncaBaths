using PROYECTO_INCABATHS.Clases;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.DB.Maps
{
    public class DetalleReservaMap : EntityTypeConfiguration<DetalleReserva>
    {
        public DetalleReservaMap()
        {
            ToTable("DetalleReserva");
            HasKey(o => o.IdDetalleReserva);


            HasRequired(o => o.Reserva)
            .WithMany(o => o.DetalleReservas)
            .HasForeignKey(o => o.IdReserva);

            HasRequired(o => o.Servicio)
           .WithMany(o => o.DetallesReservas)
           .HasForeignKey(o => o.IdServicio);

            HasRequired(o => o.Turno)
           .WithMany(o => o.DetalleReservas)
           .HasForeignKey(o => o.IdTurno);
        }
    }
}