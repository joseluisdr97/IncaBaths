using PROYECTO_INCABATHS.Clases;
using PROYECTO_INCABATHS.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROYECTO_INCABATHS.Controllers
{
    public class ReservaController : Controller
    {
        private AppConexionDB conexion = new AppConexionDB();
        // GET: Reserva
        public ActionResult Index()
        {
            var datos = conexion.Reservas.Include(u=>u.Usuario).ToList();
            return View(datos);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.Servicios = conexion.Servicios.ToList();
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Crear(Reserva reserva)
        {
            int valor = 0;
            if (reserva != null && reserva.DetalleReservas != null && reserva.DetalleReservas.Count > 0)
            {
                int idUsuario = Convert.ToInt32(Session["UsuarioId"]);
                reserva.IdUsuario = idUsuario;
                reserva.IdModoPago = 1;
                reserva.Fecha = DateTime.Now.Date;
                conexion.Reservas.Add(reserva);
                conexion.SaveChanges();
                valor = 1;

                for (int i = 0; i < reserva.DetalleReservas.Count; i++)
                {
                    var cantidad = reserva.DetalleReservas[i].Cantidad;
                    var turno = reserva.DetalleReservas[i].IdTurno;
                    var dbTurno = conexion.Turnos.Where(a => a.IdTurno == turno).First();
                    dbTurno.Stock = dbTurno.Stock - cantidad;
                    conexion.SaveChanges();
                }
            }
            return RedirectToAction("Servicio", "Admin", new { msg = valor });
        }
        [Authorize]
        [HttpPost]
        public ActionResult CrearAdmin(Reserva reserva,string DniUsuario)
        {
            var UsuarioDB = conexion.Usuarios.Where(u => u.DNI == DniUsuario).First();
            reserva.IdUsuario = UsuarioDB.IdUsuario;
            int valor = 0;
            if (reserva != null && reserva.DetalleReservas != null && reserva.DetalleReservas.Count > 0)
            {
                reserva.IdUsuario = 1;
                reserva.IdModoPago = 1;
                reserva.Fecha = DateTime.Now.Date;
                conexion.Reservas.Add(reserva);
                conexion.SaveChanges();
                valor = 1;

                for (int i = 0; i < reserva.DetalleReservas.Count; i++)
                {
                    var cantidad = reserva.DetalleReservas[i].Cantidad;
                    var turno = reserva.DetalleReservas[i].IdTurno;
                    var dbTurno = conexion.Turnos.Where(a => a.IdTurno == turno).First();
                    dbTurno.Stock = dbTurno.Stock - cantidad;
                    conexion.SaveChanges();
                }
                return RedirectToAction("Index", "Reserva", new { msg = valor });
            }
            return RedirectToAction("Crear", "Reserva", new { msg = valor });
        }
        [Authorize]
        public ActionResult MisReservas()
        {
            //var fecha = DateTime.Now.Date;
            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var misReservas = conexion.Reservas.Where(a => a.IdUsuario == usuarioIdDB/*&& a.Fecha==fecha*/).ToList();

            return View(misReservas);
        }
        [Authorize]
        public ActionResult BuscarMisReservas(DateTime fecha)
        {

            var fechas = fecha.Date;
            var misReservas = new List<Reserva>();
            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            if (fecha != null)
            {

                misReservas = conexion.Reservas.Where(a => a.IdUsuario == usuarioIdDB && a.Fecha == fechas).ToList();
            }
            else
            {
                misReservas = conexion.Reservas.Where(a => a.IdUsuario == usuarioIdDB).ToList();

            }

            return View(misReservas);
        }
        [Authorize]
        public ActionResult BuscarMisReservasAdmin(string dni)
        {
            if(dni!=""&& dni != null)
            {
                var contUDB = conexion.Usuarios.Count(u => u.DNI == dni);
                if (contUDB > 0)
                {
                    var UsuDB = conexion.Usuarios.Where(u => u.DNI == dni).First();
                    var reservas = conexion.Reservas.Where(r => r.IdUsuario == UsuDB.IdUsuario).Include(u=>u.Usuario).ToList();
                    return View(reservas);

                }
                else
                {
                    var rese = conexion.Reservas.Where(r => r.IdUsuario == -1).Include(u => u.Usuario).ToList();
                    return View(rese);
                }
            }else
            {
                return View(conexion.Reservas.Include(u => u.Usuario).ToList());
            }
           
           
        }
        [Authorize]
        public ActionResult MiDetalleReserva(int id)
        {
            var misReservas = conexion.DetalleReservas.Where(a => a.IdReserva == id).Include(o => o.Servicio).Include(t=>t.Turno).ToList();

            return View(misReservas);
        }
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var DbReserva = conexion.Reservas.Where(o => o.IdReserva == id).First();
            conexion.Reservas.Remove(DbReserva);
            conexion.SaveChanges();


            var CountReservaDb = conexion.DetalleReservas.Count(o => o.IdReserva == id);
            if (CountReservaDb != 0)
            {
                for (int i = 0; i < CountReservaDb; i++)
                {
                    var ReservaDb = conexion.DetalleReservas.Where(o => o.IdReserva == id).First();
                    conexion.DetalleReservas.Remove(ReservaDb);
                    conexion.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ObtenerTurnos(int id)
        {
            var turnos = conexion.Turnos.Where(a => a.IdServicio == id).ToList();
            return View(turnos);
        }
        public string ObtenerDiaTurno(int id)
        {
            var TurnoDb = conexion.Turnos.Where(a => a.IdTurno == id).First();
            return TurnoDb.Dia;
        }
        public string ObtenerHoraInicioTurno(int id)
        {
            var TurnoDb = conexion.Turnos.Where(a => a.IdTurno == id).First();
            return TurnoDb.HoraInicio.ToString();
        }
        public string ObtenerHoraFinTurno(int id)
        {
            var TurnoDb = conexion.Turnos.Where(a => a.IdTurno == id).First();
            return TurnoDb.HoraFin.ToString();
        }
        public string BuscarUsuario(string dni)
        {
            var ExisteU = conexion.Usuarios.Count(a => a.DNI == dni);

            if (ExisteU > 0)
            {
                var usuarioDB = conexion.Usuarios.Where(u => u.DNI == dni).First();

                return usuarioDB.Nombre +" "+ usuarioDB.Apellido;
            }
            else
            {
                return "noexiste";
            }
             
        }
        [Authorize]
        public decimal CalcularGanancia(DateTime desde, DateTime hasta)
        {
            var fechaInicio = desde.Date; var fechaFin = hasta.Date;
            decimal suma = 0;
            var ContGanancias = conexion.Reservas.Count(a => a.Fecha >= fechaInicio && a.Fecha <= fechaFin);
            if (ContGanancias > 0)
            {
                var Ganancias = conexion.Reservas.Where(a => a.Fecha >= fechaInicio && a.Fecha <= fechaFin).ToList();
                for (int i = 0; i < ContGanancias; i++)
                {
                    suma = suma + Ganancias[i].Total;
                }
                return suma;
            }
            else
            {
                ViewBag.GanaciasDelDia = 0;
            }
            return 0;
        }
    }
}