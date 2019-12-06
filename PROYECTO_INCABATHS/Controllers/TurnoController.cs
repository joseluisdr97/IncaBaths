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
    public class TurnoController : Controller
    {
        private AppConexionDB conexion = new AppConexionDB();
        // GET: Turno
        [Authorize]
        [HttpGet]
        public ActionResult Index(string query, int id)
        {
            var datos = new List<Turno>();
            if (query == null || query == "")
            {
                datos = conexion.Turnos.Include(t => t.Servicio).Where(t => t.Servicio.IdServicio == id).ToList();
            }
            else
            {
                datos = conexion.Turnos.Include(t => t.Servicio).Where(t => t.Servicio.IdServicio == id && t.Dia.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            ViewBag.IdServicio = id;
            return View(datos);
        }
        public ActionResult BuscarTurno(string query, int id)
        {
            var datos = new List<Turno>();
            if (query == null || query == "")
            {
                datos = conexion.Turnos.Include(t => t.Servicio).Where(t => t.Servicio.IdServicio == id).ToList();
            }
            else
            {
                datos = conexion.Turnos.Include(t => t.Servicio).Where(t => t.Servicio.IdServicio == id && t.Dia.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            ViewBag.IdServicio = id;
            return View(datos);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Crear(int id)
        {
            ViewBag.IdServicio = id;
            return View(new Turno());
        }
        [HttpPost]
        [Authorize]
        public ActionResult Crear(Turno turno, int id)
        {
            validar(turno, id);
            if (ModelState.IsValid == true)
            {
                turno.IdServicio = id;
                var stockServicio = conexion.Servicios.Where(a => a.IdServicio == id).First();
                turno.Stock = stockServicio.Aforo;
                conexion.Turnos.Add(turno);
                conexion.SaveChanges();
                return RedirectToAction("Index", new { id = turno.IdServicio });
            }
            ViewBag.IdServicio = id;
            return View(turno);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Editar(int id)
        {
            var DbTurno = conexion.Turnos.Find(id);
            ViewBag.IdServicio = conexion.Servicios.Find(DbTurno.IdServicio);
            return View(DbTurno);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Editar(Turno turno, int id)
        {
            var DbTurno = conexion.Turnos.Find(id);
            validarEditar(turno, id);
            if (ModelState.IsValid == true)
            {
                DbTurno.Dia = turno.Dia;
                DbTurno.HoraInicio = turno.HoraInicio;
                DbTurno.HoraFin = turno.HoraFin;
                conexion.SaveChanges();
                return RedirectToAction("Index", new { id = DbTurno.IdServicio });
            }
            ViewBag.IdServicio = conexion.Servicios.Find(DbTurno.IdServicio);
            return View(turno);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var DbTurno = conexion.Turnos.Find(id);
            conexion.Turnos.Remove(DbTurno);
            conexion.SaveChanges();
            return RedirectToAction("Index", new { id = DbTurno.IdServicio });
        }



        public void validar(Turno turno, int id)
        {
            if (Convert.ToString(turno.HoraInicio) == "00:00:00")
                ModelState.AddModelError("HoraInicio", "El campo hora de ingreso no debe de ser nulo");
            if (Convert.ToString(turno.HoraFin) == "00:00:00")
                ModelState.AddModelError("HoraFin", "El campo hora de salida no debe de ser nulo");



            if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            {
                if (turno.HoraInicio == turno.HoraFin)
                {
                    ModelState.AddModelError("HoraFin", "La hora de salida no debe de ser igual a la hora de inicio");
                }
            }

            if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            {
                if (turno.HoraInicio != turno.HoraFin)
                {
                    if (turno.HoraInicio > turno.HoraFin)
                    {
                        ModelState.AddModelError("HoraInicio", "La hora de ingreso no debe de ser mayor a la hora de salida");
                    }
                }
            }
            if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            {
                if (turno.HoraInicio != turno.HoraFin)
                {
                    if (turno.HoraInicio < turno.HoraFin)
                    {
                        var Existe = conexion.Turnos.Any(a => a.IdServicio == id && a.HoraInicio == turno.HoraInicio && a.HoraFin == turno.HoraFin && a.Dia == turno.Dia);
                        if (Existe)
                            ModelState.AddModelError("HoraFin", "Este turno ya existe");
                    }
                }
            }
            //VALIDACION DE CRUCE DE HORARIOS

            if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            {
                if (turno.HoraInicio != turno.HoraFin)
                {
                    if (turno.HoraInicio < turno.HoraFin)
                    {
                        var Existe = conexion.Turnos.Any(a => a.IdServicio == id && a.HoraInicio == turno.HoraInicio && a.HoraFin == turno.HoraFin && a.Dia == turno.Dia);
                        if (!Existe)
                        {
                            var ListaTurnos = conexion.Turnos.Where(a => a.IdServicio == id).ToList();
                            for (int i = 0; i < ListaTurnos.Count; i++)
                            {

                                if (turno.HoraInicio >= ListaTurnos[i].HoraInicio && turno.HoraInicio <= ListaTurnos[i].HoraFin && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                                if (turno.HoraFin <= ListaTurnos[i].HoraFin && turno.HoraFin >= ListaTurnos[i].HoraInicio && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                                if (turno.HoraFin >= ListaTurnos[i].HoraFin && turno.HoraInicio <= ListaTurnos[i].HoraFin && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                                if (turno.HoraInicio <= ListaTurnos[i].HoraInicio && turno.HoraFin >= ListaTurnos[i].HoraInicio && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                            }
                        }

                    }
                }
            }
        }
        public void validarEditar(Turno turno, int id)
        {
            if (Convert.ToString(turno.HoraInicio) == "00:00:00")
                ModelState.AddModelError("HoraInicio", "El campo hora de ingreso no debe de ser nulo");
            if (Convert.ToString(turno.HoraFin) == "00:00:00")
                ModelState.AddModelError("HoraFin", "El campo hora de salida no debe de ser nulo");



            if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            {
                if (turno.HoraInicio == turno.HoraFin)
                {
                    ModelState.AddModelError("HoraFin", "La hora de salida no debe de ser igual a la hora de inicio");
                }
            }

            if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            {
                if (turno.HoraInicio != turno.HoraFin)
                {
                    if (turno.HoraInicio > turno.HoraFin)
                    {
                        ModelState.AddModelError("HoraInicio", "La hora de ingreso no debe de ser mayor a la hora de salida");
                    }
                }
            }
            //if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            //{
            //    if (turno.HoraInicio != turno.HoraFin)
            //    {
            //        if (turno.HoraInicio < turno.HoraFin)
            //        {
            //            var Existe = conexion.Turnos.Any(a => a.IdServicio == id && a.HoraInicio == turno.HoraInicio && a.HoraFin == turno.HoraFin && a.Dia == turno.Dia);
            //            var UsuarioDBC = conexion.Usuarios.Where(a => a.IdUsuario == id).First();
            //            var UsuCorreo = conexion.Usuarios.Where(a => a.Correo == UsuarioDBC.Correo).First();
            //            if (UsuCorreo.Correo != usuario.Correo)
            //            {
            //                var usuarioDB = conexion.Usuarios.Any(t => t.Correo == usuario.Correo);
            //                if (usuarioDB)
            //                {
            //                    ModelState.AddModelError("Correo", "Este Correo ya existe");
            //                }
            //            }
            //        }
            //    }
            //}

            //VALIDACION DE CRUCE DE HORARIOS

            if (Convert.ToString(turno.HoraInicio) != "00:00:00" && Convert.ToString(turno.HoraInicio) != "00:00:00")
            {
                if (turno.HoraInicio != turno.HoraFin)
                {
                    if (turno.HoraInicio < turno.HoraFin)
                    {
                        var Existe = conexion.Turnos.Any(a => a.IdServicio == id && a.HoraInicio == turno.HoraInicio && a.HoraFin == turno.HoraFin && a.Dia == turno.Dia);
                        if (!Existe)
                        {
                            var ListaTurnos = conexion.Turnos.Where(a => a.IdServicio == id).ToList();
                            for (int i = 0; i < ListaTurnos.Count; i++)
                            {

                                if (turno.HoraInicio >= ListaTurnos[i].HoraInicio && turno.HoraInicio <= ListaTurnos[i].HoraFin && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                                if (turno.HoraFin <= ListaTurnos[i].HoraFin && turno.HoraFin >= ListaTurnos[i].HoraInicio && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                                if (turno.HoraFin >= ListaTurnos[i].HoraFin && turno.HoraInicio <= ListaTurnos[i].HoraFin && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                                if (turno.HoraInicio <= ListaTurnos[i].HoraInicio && turno.HoraFin >= ListaTurnos[i].HoraInicio && ListaTurnos[i].Dia == turno.Dia)
                                {
                                    ModelState.AddModelError("HoraFin", "Este horario se esta cruzando con otro");
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}