using PROYECTO_INCABATHS.Clases;
using PROYECTO_INCABATHS.DB.Maps;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PROYECTO_INCABATHS.DB
{
    public class AppConexionDB: DbContext
    {
        public IDbSet<Usuario> Usuarios { get; set; }
        public IDbSet<TipoUsuario> TipoUsuarios { get; set; }
        public IDbSet<Reserva> Reservas { get; set; }
        public IDbSet<DetalleReserva> DetalleReservas { get; set; }
        public IDbSet<Turno> Turnos { get; set; }
        public IDbSet<Servicio> Servicios { get; set; }
        public IDbSet<ModoPago> ModoPagos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new TipoUsuarioMap());
            modelBuilder.Configurations.Add(new ReservaMap());
            modelBuilder.Configurations.Add(new DetalleReservaMap());
            modelBuilder.Configurations.Add(new TurnoMap());
            modelBuilder.Configurations.Add(new ServicioMap());
            modelBuilder.Configurations.Add(new ModoPagoMap());
        }
    }
}