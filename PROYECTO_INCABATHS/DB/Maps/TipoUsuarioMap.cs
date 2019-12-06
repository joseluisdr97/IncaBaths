using PROYECTO_INCABATHS.Clases;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.DB.Maps
{
    public class TipoUsuarioMap : EntityTypeConfiguration<TipoUsuario>
    {
        public TipoUsuarioMap()
        {
            ToTable("TipoUsuario");
            HasKey(o => o.IdTipoUsuario);

        }
    }
}