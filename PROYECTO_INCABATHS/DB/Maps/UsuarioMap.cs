using PROYECTO_INCABATHS.Clases;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.DB.Maps
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("Usuario");
            HasKey(o => o.IdUsuario);

            HasRequired(o => o.TipoUsuario)
                .WithMany(o => o.Usuarios)
                .HasForeignKey(o => o.IdTipoUsuario);
        }
    }
}