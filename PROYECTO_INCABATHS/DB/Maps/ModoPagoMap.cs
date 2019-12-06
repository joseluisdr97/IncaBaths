using PROYECTO_INCABATHS.Clases;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.DB.Maps
{
    public class ModoPagoMap : EntityTypeConfiguration<ModoPago>
    {
        public ModoPagoMap()
        {
            ToTable("ModoPago");
            HasKey(o => o.IdModoPago);

        }
    }
}