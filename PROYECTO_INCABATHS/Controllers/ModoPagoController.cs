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
    [Authorize]
    public class ModoPagoController : Controller
    {
        private AppConexionDB conexion = new AppConexionDB();
        // GET: ModoPago
        [HttpGet]
        public ActionResult Index(string query)
        {
            var datos = new List<ModoPago>();
            if (query == null || query == "")
            {
                datos = conexion.ModoPagos.ToList();
            }
            else
            {
                datos = conexion.ModoPagos.Where(o => o.Nombre.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            return View(datos);
        }
        [HttpGet]
        public ActionResult BuscarModoPago(string query)
        {
            var datos = new List<ModoPago>();
            if (query == null || query == "")
            {
                datos = conexion.ModoPagos.ToList();
            }
            else
            {
                datos = conexion.ModoPagos.Where(o => o.Nombre.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            return View(datos);
        }
        [HttpGet]
        public ActionResult Crear()
        {
            //ViewBag.Usuario = con.Usuarios.ToList();
            return View(new ModoPago());
        }
        [HttpPost]
        public ActionResult Crear(ModoPago modoPago)
        {
            validar(modoPago);
            if (ModelState.IsValid == true)
            {
                conexion.ModoPagos.Add(modoPago);
                conexion.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modoPago);
        }
        [HttpGet]
        public ActionResult Editar(int id)
        {
            var ModoPagoDb = conexion.ModoPagos.Find(id);
            return View(ModoPagoDb);
        }
        [HttpPost]
        public ActionResult Editar(ModoPago modoPago, int id)
        {
            var ModoPagoDb = conexion.ModoPagos.Find(id);
            validar(modoPago);
            if (ModelState.IsValid == true)
            {
                ModoPagoDb.Nombre = modoPago.Nombre;
                conexion.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modoPago);
        }
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var ModoPagoDb = conexion.ModoPagos.Where(o => o.IdModoPago == id).First();
            conexion.ModoPagos.Remove(ModoPagoDb);
            conexion.SaveChanges();
            return RedirectToAction("Index");

        }
        public void validar(ModoPago modoPago)
        {


            if (modoPago.Nombre == null || modoPago.Nombre == "")
                ModelState.AddModelError("Nombre", "El campo nombre es obligatorio");

            if (modoPago.Nombre != null)
            {
                if (!Regex.IsMatch(modoPago.Nombre, @"^[a-zA-Z ]*$"))
                {
                    ModelState.AddModelError("Nombre", "El campo nombre solo acepta letras");
                }
            }



            //if (usuario.Apellido == null || usuario.Apellido == "")
            //    ModelState.AddModelError("Apellido", "El apellido es obligatorio");
            //else if (!Regex.IsMatch(usuario.Apellido, @"^[a-zA-Z ]*$"))
            //    ModelState.AddModelError("Apellido", "El campo apellido solo acepta letras");

            //if (usuario.DNI == null || usuario.DNI == "")
            //    ModelState.AddModelError("DNI", "El nombre es obligatorio");
            //else if (!Regex.IsMatch(usuario.DNI, @"^[0-9]"))
            //    ModelState.AddModelError("DNI", "El campo dni solo acepta numeros");
            //else if (usuario.DNI.Length > 8)
            //    ModelState.AddModelError("DNI", "El campo dni debe de tener 8 numeros");

            //if (usuario.Celular == null || usuario.Celular == "")
            //    ModelState.AddModelError("Celular", "El celular es obligatorio");
            //else if (!Regex.IsMatch(usuario.Celular, @"^[0-9]"))
            //    ModelState.AddModelError("Celular", "El campo celular solo acepta numeros");
            //else if (usuario.Celular.Length > 9)
            //    ModelState.AddModelError("Celular", "El campo celular debe de tener 9 numeros");


            //if (usuario.Correo == null || usuario.Correo == "")
            //    ModelState.AddModelError("Correo", "El correo es obligatorio");
            //if (usuario.Correo != null && !Regex.IsMatch(usuario.Correo, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
            //    ModelState.AddModelError("Correo", "El formato debe ser de correo");

            //if (usuario.Direccion == null || usuario.Direccion == "")
            //    ModelState.AddModelError("Direccion", "La direccion es obligatorio");

            //if (usuario.Password == null || usuario.Password == "")
            //    ModelState.AddModelError("Password", "El pasword es obligatorio");

        }
    }
}