using PROYECTO_INCABATHS.Clases;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.DB.Maps
{
    public class TurnoMap : EntityTypeConfiguration<Turno>
    {
        public TurnoMap()
        {
            ToTable("Turno");
            HasKey(o => o.IdTurno);

            //Relaciones

            HasRequired(o => o.Servicio)
            .WithMany(o => o.Turnos)
            .HasForeignKey(o => o.IdServicio);
        }
    }
}