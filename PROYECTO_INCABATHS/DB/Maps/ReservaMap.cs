using PROYECTO_INCABATHS.Clases;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.DB.Maps
{
    public class ReservaMap : EntityTypeConfiguration<Reserva>
    {
        public ReservaMap()
        {
            ToTable("Reserva");
            HasKey(o => o.IdReserva);

            HasRequired(o => o.Usuario)
            .WithMany(o => o.Reservas)
            .HasForeignKey(o => o.IdUsuario);
        }
    }
}