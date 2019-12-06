using PROYECTO_INCABATHS.Clases;
using PROYECTO_INCABATHS.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PROYECTO_INCABATHS.Controllers
{
    public class ServicioController : Controller
    {
        private AppConexionDB conexion = new AppConexionDB();
        // GET: Servicio
        [Authorize]
        [HttpGet]
        public ActionResult Index(string query)
        {
            var datos = new List<Servicio>();
            if (query == null || query == "")
            {
                datos = conexion.Servicios.ToList();
            }
            else
            {
                datos = conexion.Servicios.Where(o => o.Nombre.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            return View(datos);
        }
        [Authorize]
        public ActionResult BuscarServicio(string query)
        {
            var datos = new List<Servicio>();
            if (query == null || query == "")
            {
                datos = conexion.Servicios.ToList();
            }
            else
            {
                datos = conexion.Servicios.Where(o => o.Nombre.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            return View(datos);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Crear()
        {
            return View(new Servicio());
        }
        [Authorize]
        [HttpPost]
        public ActionResult Crear(Servicio servicio)
        {
            int id = 0;
            validar(servicio, id);
            if (ModelState.IsValid == true)
            {
                conexion.Servicios.Add(servicio);
                conexion.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(servicio);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Editar(int id)
        {
            var servicioDb = conexion.Servicios.Find(id);
            ViewBag.IdServicio = id;
            return View(servicioDb);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Editar(Servicio servicio, int id)
        {
            var servicioDb = conexion.Servicios.Find(id);
            validar(servicio, id);
            if (ModelState.IsValid == true)
            {
                servicioDb.Nombre = servicio.Nombre;
                servicioDb.Precio = servicio.Precio;
                servicioDb.Aforo = servicio.Aforo;
                conexion.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdServicio = id;
            return View(servicio);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var servicioDb = conexion.Servicios.Where(o => o.IdServicio == id).First();
            conexion.Servicios.Remove(servicioDb);
            conexion.SaveChanges();

            var CountTurnoDb = conexion.Turnos.Count(o => o.IdServicio == id);
            if (CountTurnoDb != 0)
            {
                for (int i = 0; i < CountTurnoDb; i++)
                {
                    var turnoDb = conexion.Turnos.Where(o => o.IdServicio == id).First();
                    conexion.Turnos.Remove(turnoDb);
                    conexion.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }


        public void validar(Servicio servicio, int id)
        {


            if (servicio.Nombre == null || servicio.Nombre == "")
                ModelState.AddModelError("Nombre", "El campo nombre es obligatorio");

            if (servicio.Nombre != null)
            {
                if (!Regex.IsMatch(servicio.Nombre, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Nombre", "El campo nombre solo acepta letras");
            }

            if (servicio.Precio == 0 || Convert.ToString(servicio.Precio) == "")
                ModelState.AddModelError("Precio", "El campo precio es obligatorio");

            if (servicio.Precio != 0 && Convert.ToString(servicio.Precio) == null)
            {
                if (!Regex.IsMatch(Convert.ToString(servicio.Precio), @"^\d{1,2}([.][0-9]{1,2})?$"))
                    ModelState.AddModelError("Precio", "El campo precio solo acepta decimales [.]");
            }
            if (Regex.IsMatch(Convert.ToString(servicio.Precio), @"^[a-zA-Z ]*$"))
                ModelState.AddModelError("Precio", "El campo precio no acepta letras");


            if (servicio.Aforo == 0 || Convert.ToString(servicio.Aforo) == "")
                ModelState.AddModelError("Aforo", "El campo aforo es obligatorio");

        }
    }
}