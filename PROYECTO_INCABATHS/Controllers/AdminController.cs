using PROYECTO_INCABATHS.Clases;
using PROYECTO_INCABATHS.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PROYECTO_INCABATHS.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private AppConexionDB conexion = new AppConexionDB();
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            var fecha = DateTime.Now.Date;
            decimal suma = 0;
            var ContGanancias = conexion.Reservas.Count(a => a.Fecha == fecha);
            if (ContGanancias > 0)
            {
                var Ganancias = conexion.Reservas.Where(a => a.Fecha == fecha).ToList();
                for (int i = 0; i < ContGanancias; i++)
                {
                    suma = suma + Ganancias[i].Total;
                }
                ViewBag.GanaciasDelDia = suma;
            }
            else
            {
                ViewBag.GanaciasDelDia = 0;
            }
            return View();
        }
        [Authorize]
        public decimal GananciasPorFecha()
        {
            decimal suma = 0;
            var ContGanancias = conexion.Reservas.Count(a => a.Fecha == DateTime.Now.Date);
            if (ContGanancias > 0)
            {
                var Ganancias = conexion.Reservas.Where(a => a.Fecha == DateTime.Now.Date).ToList();
                for (int i = 0; i < ContGanancias; i++)
                {
                    suma = suma + Ganancias[i].Total;
                }
                ViewBag.GanaciasDeFechaAfecha = suma;
            }
            return suma;

        }
        public ActionResult Prueba()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.TipoUsuarios = conexion.TipoUsuarios.ToList();
            //ViewBag.Usuario = con.Usuarios.ToList();
            return View(new Usuario());
        }
        [HttpGet]

        public ActionResult Servicio()
        {
            ViewBag.Servicios = conexion.Servicios.ToList();
            return View();
        }
        [Authorize]
        public ActionResult Servicio1()
        {
            ViewBag.Servicios = conexion.Servicios.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult BuscarServicio(int id)
        {
            var datos = conexion.Turnos.Where(s => s.IdServicio == id).ToList();

            return View(datos);
        }

        public decimal ObtenerPrecioServicio(int id)
        {
            var Dbprecio = conexion.Servicios.Where(p => p.IdServicio == id).First();
            var precio = Dbprecio.Precio;
            return precio;
        }
        public string ObtenerNombreServicio(int id)
        {
            var Dbprecio = conexion.Servicios.Where(p => p.IdServicio == id).First();
            var nombre = Dbprecio.Nombre;
            return nombre;
        }

        public int ObtenerStock(int id)
        {
            var DbStock = conexion.Turnos.Where(p => p.IdTurno == id).First();
            return DbStock.Stock;
        }

        [HttpPost]
        public ActionResult Crear(Usuario usuario, string RepitaPassword, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string ruta = Path.Combine(Server.MapPath("~/Perfiles"), Path.GetFileName(file.FileName));
                file.SaveAs(ruta);
                usuario.Perfil = "/Perfiles/" + Path.GetFileName(file.FileName);
            }
            usuario.IdTipoUsuario = 3;
            validar(usuario, RepitaPassword);
            if (ModelState.IsValid == true)
            {
                conexion.Usuarios.Add(usuario);
                conexion.SaveChanges();
                return RedirectToAction("Login","Auth");
            }
            ViewBag.TipoUsuarios = conexion.TipoUsuarios.ToList();
            return View(usuario);
        }

        public void validar(Usuario usuario, string RepitaPassword)
        {


            if (usuario.Nombre == null || usuario.Nombre == "")
                ModelState.AddModelError("Nombre", "El Nombre es obligatorio");
            if (usuario.Nombre != null)
            {
                if (Regex.IsMatch(usuario.Nombre, @"^[a-zA-Z ]*$"))
                {
                    if (usuario.Nombre.Length <= 3)
                        ModelState.AddModelError("Nombre", "Debe de tener 3 letras a más");
                }
            }

            if (usuario.Nombre != null)
            {
                if (!Regex.IsMatch(usuario.Nombre, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Nombre", "El Nombre solo acepta letras");
            }


            if (usuario.Apellido == null || usuario.Apellido == "")
                ModelState.AddModelError("Apellido", "El Apellido es obligatorio");
            if (usuario.Apellido != null)
            {
                if (!Regex.IsMatch(usuario.Apellido, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Apellido", "El Apellido solo acepta letras");
            }
            if (usuario.Apellido != null)
            {
                if (Regex.IsMatch(usuario.Apellido, @"^[a-zA-Z ]*$"))
                {
                    if (usuario.Apellido.Length <= 3)
                        ModelState.AddModelError("Apellido", "Debe de tener 3 letras a más");
                }
            }



            if (usuario.DNI == null || usuario.DNI == "")
                ModelState.AddModelError("DNI", "El DNI es obligatorio");

            if (usuario.DNI != null)
            {
                if (!Regex.IsMatch(usuario.DNI, "^\\d+$"))
                    ModelState.AddModelError("DNI", "El Dni solo acepta numeros");
            }

            if (usuario.DNI != null)
            {
                if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
                {
                    if (usuario.DNI.Length != 8)
                        ModelState.AddModelError("DNI", "El Dni debe de tener 8 numeros");
                }
            }
            if (usuario.DNI != null)
            {
                if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
                {
                    if (usuario.DNI.Length == 8)
                    {
                        var usuarioDB = conexion.Usuarios.Any(t => t.DNI == usuario.DNI);
                        if (usuarioDB)
                        {
                            ModelState.AddModelError("DNI", "Este DNI ya existe");
                        }
                    }
                }
            }


            if (usuario.Celular == null || usuario.Celular == "")
                ModelState.AddModelError("Celular", "El celular es obligatorio");

            if (usuario.Celular != null)
            {
                if (!Regex.IsMatch(usuario.Celular, "^\\d+$"))
                    ModelState.AddModelError("Celular", "El campo celular solo acepta numeros");
            }

            if (usuario.Celular != null)
            {
                if (Regex.IsMatch(usuario.Celular, "^\\d+$"))
                {
                    if (usuario.Celular.Length != 9)
                        ModelState.AddModelError("Celular", "El campo celular debe de tener 9 numeros");
                }
            }


            if (usuario.Correo == null || usuario.Correo == "")
                ModelState.AddModelError("Correo", "El correo es obligatorio");

            if (usuario.Correo != null)
            {
                if (!Regex.IsMatch(usuario.Correo, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
                    ModelState.AddModelError("Correo", "El formato debe ser de correo");
            }

            if (usuario.Correo != null)
            {
                if (Regex.IsMatch(usuario.Correo, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
                {
                    var usuarioCorreoDB = conexion.Usuarios.Any(t => t.Correo == usuario.Correo);
                    if (usuarioCorreoDB)
                    {
                        ModelState.AddModelError("Correo", "Este correo ya existe");
                    }
                }
            }


            if (usuario.Direccion == null || usuario.Direccion == "")
                ModelState.AddModelError("Direccion", "La direccion es obligatorio");
            //if (usuario.Direccion != null)
            //{
            //    if (Regex.IsMatch(usuario.Direccion, @"^[a-zA-Z0-9""'\s.#]*$"))
            //        ModelState.AddModelError("Direccion", "Ejemplo Jr. La paz #121");

            //}
            if (usuario.Password == null || usuario.Password == "")
                ModelState.AddModelError("Password", "El pasword es obligatorio");

            if (RepitaPassword == null || RepitaPassword == "")
                ModelState.AddModelError("RepitaPassword", "Este campo es obligatorio");

            if (usuario.Password != null || RepitaPassword != null)
            {
                if (usuario.Password != RepitaPassword)
                {
                    ModelState.AddModelError("RepitaPassword", "Los passwords no coinciden");
                }
            }
            if (usuario.Perfil == null)
            {
                ModelState.AddModelError("Perfil", "El campo perfil no pude ser vacio");
            }

        }


    }
}