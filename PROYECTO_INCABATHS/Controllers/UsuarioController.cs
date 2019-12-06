using PROYECTO_INCABATHS.Clases;
using PROYECTO_INCABATHS.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PROYECTO_INCABATHS.Controllers
{
    
    public class UsuarioController : Controller
    {
        private AppConexionDB conexion = new AppConexionDB();
        // GET: Usuario
        [Authorize]
        [HttpGet]
        public ActionResult Index(string query)
        {
            var datos = new List<Usuario>();
            if (query == null || query == "")
            {
                datos = conexion.Usuarios.Include(u => u.TipoUsuario).ToList();
            }
            else
            {
                datos = conexion.Usuarios.Include(o => o.TipoUsuario).Where(o => o.Nombre.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            return View(datos);
        }
        [Authorize]
        [HttpGet]
        public ActionResult BuscarUsuario(string query)
        {
            var datos = new List<Usuario>();
            if (query == null || query == "")
            {
                datos = conexion.Usuarios.Include(u => u.TipoUsuario).ToList();
            }
            else
            {
                datos = conexion.Usuarios.Include(o => o.TipoUsuario).Where(o => o.Nombre.Contains(query)).ToList();
            }
            ViewBag.datos = query;
            return View(datos);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.TipoUsuarios = conexion.TipoUsuarios.ToList();
            //ViewBag.Usuario = con.Usuarios.ToList();
            return View(new Usuario());
        }
        [Authorize]
        [HttpPost]
        public ActionResult Crear(Usuario usuario, string RepitaPassword, HttpPostedFileBase file)
        {
            //validar(usuario, RepitaPassword);
            if (file != null && file.ContentLength > 0)
            {
                string ruta = Path.Combine(Server.MapPath("~/Perfiles"), Path.GetFileName(file.FileName));
                file.SaveAs(ruta);
                usuario.Perfil = "/Perfiles/" + Path.GetFileName(file.FileName);
            }
            validar(usuario,RepitaPassword);
            if (ModelState.IsValid == true)
            {
                conexion.Usuarios.Add(usuario);
                conexion.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoUsuarios = conexion.TipoUsuarios.ToList();
            return View(usuario);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Editar(int id)
        {
            ViewBag.TipoUsuarios = conexion.TipoUsuarios.ToList();
            var UsuarioDb = conexion.Usuarios.Find(id);
            ViewBag.IdUsuario = id;
            return View(UsuarioDb);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Editar(Usuario usuario, int id, HttpPostedFileBase file)
        {
            var UsuarioDb = conexion.Usuarios.Find(id);
            if (file != null && file.ContentLength > 0)
            {
                string ruta = Path.Combine(Server.MapPath("~/Perfiles"), Path.GetFileName(file.FileName));
                file.SaveAs(ruta);
                usuario.Perfil = "/Perfiles/" + Path.GetFileName(file.FileName);
                UsuarioDb.Perfil = usuario.Perfil;
            }
            validarEditarUsuario(usuario, id);
            if (ModelState.IsValid == true)
            {
                UsuarioDb.Nombre = usuario.Nombre;
                UsuarioDb.IdTipoUsuario = usuario.IdTipoUsuario;
                UsuarioDb.Password = usuario.Password;
                UsuarioDb.Celular = usuario.Celular;
                UsuarioDb.Correo = usuario.Correo;
                UsuarioDb.DNI = usuario.DNI;
                UsuarioDb.Direccion = usuario.Direccion;
                conexion.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoUsuarios = conexion.TipoUsuarios.ToList();
            ViewBag.IdUsuario = id;
            return View(usuario);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == id).First();
            conexion.Usuarios.Remove(UsuarioDb);
            conexion.SaveChanges();
            return RedirectToAction("Index");

        }
        [Authorize]
        [HttpGet]
        public ActionResult VerMiCuenta()
        {
            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();
            return View(UsuarioDb);
        }
        [Authorize]
        [HttpGet]
        public ActionResult ActualizarDatosUCliente()
        {

            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();

            return View(UsuarioDb);
        }
        [Authorize]
        [HttpPost]
        public ActionResult ActualizarDatosUCliente(Usuario usuario, HttpPostedFileBase file)
        {
            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();

            if (file != null && file.ContentLength > 0)
            {
                string ruta = Path.Combine(Server.MapPath("~/Perfiles"), Path.GetFileName(file.FileName));
                file.SaveAs(ruta);
                usuario.Perfil = "/Perfiles/" + Path.GetFileName(file.FileName);
                UsuarioDb.Perfil = usuario.Perfil;
            }

            validarActualizarDatos(usuario);
            if (ModelState.IsValid)
            {
                UsuarioDb.Nombre = usuario.Nombre;
                UsuarioDb.Apellido = usuario.Apellido;
                UsuarioDb.DNI = usuario.DNI;
                UsuarioDb.Direccion = usuario.Direccion;
                conexion.SaveChanges();

                Session["UsuarioNombre"] = usuario.Nombre;
                Session["UsuarioDNI"] = usuario.DNI;
                return RedirectToAction("VerMiCuenta");
            }
            return View(usuario);
        }
        [Authorize]
        [HttpGet]
        public ActionResult CambiarContraUsuario()
        {
            return View(new Usuario());
        }
        [Authorize]
        [HttpPost]
        public ActionResult CambiarContraUsuario(Usuario usuario, string NuevaPassword, string RepitaPassword)
        {
            ValidarCambiarContra(usuario, NuevaPassword, RepitaPassword);
            if (ModelState.IsValid)
            {
                var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
                var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();
                UsuarioDb.Password = NuevaPassword;
                conexion.SaveChanges();
                return RedirectToAction("Logout", "Auth");
            }
            return View(usuario);
        }

        [Authorize]
        [HttpGet]
        public ActionResult VerMiCuentaAdmin()
        {
            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();
            return View(UsuarioDb);
        }
        [Authorize]
        [HttpGet]
        public ActionResult ActualizarDatosUAdmin()
        {

            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();

            return View(UsuarioDb);
        }
        [Authorize]
        [HttpPost]
        public ActionResult ActualizarDatosUAdmin(Usuario usuario, HttpPostedFileBase file)
        {

            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();

            if (file != null && file.ContentLength > 0)
            {
                string ruta = Path.Combine(Server.MapPath("~/Perfiles"), Path.GetFileName(file.FileName));
                file.SaveAs(ruta);
                usuario.Perfil = "/Perfiles/" + Path.GetFileName(file.FileName);
                UsuarioDb.Perfil = usuario.Perfil;
            }
            validarActualizarDatos(usuario);
            if (ModelState.IsValid)
            {
                UsuarioDb.Nombre = usuario.Nombre;
                UsuarioDb.Apellido = usuario.Apellido;
                UsuarioDb.DNI = usuario.DNI;
                UsuarioDb.Direccion = usuario.Direccion;
                conexion.SaveChanges();

                Session["UsuarioNombre"] = usuario.Nombre;
                Session["UsuarioDNI"] = usuario.DNI;
                return RedirectToAction("VerMiCuentaAdmin");
            }
            return View(usuario);
        }
        [Authorize]
        [HttpGet]
        public ActionResult CambiarContraUsuarioAdmin()
        {
            return View(new Usuario());
        }
        [Authorize]
        [HttpPost]
        public ActionResult CambiarContraUsuarioAdmin(Usuario usuario, string NuevaPassword, string RepitaPassword)
        {
            ValidarCambiarContra(usuario, NuevaPassword, RepitaPassword);
            if (ModelState.IsValid)
            {
                var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
                var UsuarioDb = conexion.Usuarios.Where(o => o.IdUsuario == usuarioIdDB).First();
                UsuarioDb.Password = NuevaPassword;
                conexion.SaveChanges();
                return RedirectToAction("Logout", "Auth");
            }
            return View(usuario);
        }
        public void ValidarCambiarContra(Usuario usuario, string NuevaPassword, string RepitaPassword)
        {
            var usuarioIdDB = Convert.ToInt32(Session["UsuarioId"]);
            var UExiste = conexion.Usuarios.Count(u => u.IdUsuario == usuarioIdDB && u.Password == usuario.Password);
            if (usuario.Password != null && usuario.Password != "")
            {
                if (UExiste == 0)
                {
                    ModelState.AddModelError("Password", "Contraseña no encontrada");
                }
            }
            if (usuario.Password == null || usuario.Password == "")
                ModelState.AddModelError("Password", "Este campo es obligatorio");
            if (usuario.Password == null || usuario.Password == "")
            {
                if (NuevaPassword == null || NuevaPassword == "")
                    ModelState.AddModelError("NuevaPassword", "Este campo es obligatorio");
                if (RepitaPassword == null || RepitaPassword == "")
                    ModelState.AddModelError("RepitaPassword", "Este campo es obligatorio");
            }
            if (usuario.Password != null && usuario.Password != "")
            {
                if (UExiste != 0)
                {
                    if (NuevaPassword == null || NuevaPassword == "")
                        ModelState.AddModelError("NuevaPassword", "Este campo es obligatorio");
                    if (RepitaPassword == null || RepitaPassword == "")
                        ModelState.AddModelError("RepitaPassword", "Este campo es obligatorio");
                    if (NuevaPassword != null && NuevaPassword != "" && RepitaPassword != null && RepitaPassword != "")
                    {
                        if (NuevaPassword != RepitaPassword)
                            ModelState.AddModelError("RepitaPassword", "Las contraseñas no coinciden");
                    }
                }
            }
        }
        public void validarActualizarDatos(Usuario usuario)
        {
            if (usuario.Nombre == null || usuario.Nombre == "")
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
            if (usuario.Nombre != null)
            {
                if (!Regex.IsMatch(usuario.Nombre, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Nombre", "El campo nombre solo acepta letras");
            }

            if (usuario.Apellido == null || usuario.Apellido == "")
                ModelState.AddModelError("Apellido", "El apellido es obligatorio");
            if (usuario.Apellido != null)
            {
                if (!Regex.IsMatch(usuario.Apellido, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Apellido", "El campo apellido solo acepta letras");
            }

            if (usuario.DNI == null || usuario.DNI == "")
                ModelState.AddModelError("DNI", "El DNI es obligatorio");

            if (usuario.DNI != null)
            {
                if (!Regex.IsMatch(usuario.DNI, "^\\d+$"))
                    ModelState.AddModelError("DNI", "El campo dni solo acepta numeros");
            }

            if (usuario.DNI != null)
            {
                if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
                {
                    if (usuario.DNI.Length != 8)
                        ModelState.AddModelError("DNI", "El campo dni debe de tener 8 numeros");
                }
            }
            if (usuario.DNI != null && usuario.DNI != "")
            {
                if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
                {
                    if (usuario.DNI.Length == 8)
                    {
                        var DNIUsuarioDB = Convert.ToString(Session["UsuarioDNI"]);
                        var UsuDNI = conexion.Usuarios.Where(a => a.DNI == DNIUsuarioDB).First();
                        if (UsuDNI.DNI != usuario.DNI)
                        {
                            var usuarioDB = conexion.Usuarios.Any(t => t.DNI == usuario.DNI);
                            if (usuarioDB)
                            {
                                ModelState.AddModelError("DNI", "Este DNI ya existe");
                            }
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
                if (!Regex.IsMatch(usuario.Celular, "^\\d+$"))
                {
                    if (usuario.Celular.Length != 9)
                        ModelState.AddModelError("Celular", "El campo celular debe de tener 9 numeros");
                }
            }

            if (usuario.Direccion == null || usuario.Direccion == "")
                ModelState.AddModelError("Direccion", "La direccion es obligatorio");
            //if (usuario.Direccion != null)
            //{
            //    if (Regex.IsMatch(usuario.Direccion, @"^[a-zA-Z0-9""'\s.#]*$"))
            //        ModelState.AddModelError("Direccion", "Ejemplo Jr. La paz #121");
            //}
        }
        public void validar(Usuario usuario, string RepitaPassword)
        {


            if (usuario.Nombre == null || usuario.Nombre == "")
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
            if (usuario.Nombre != null)
            {
                if (!Regex.IsMatch(usuario.Nombre, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Nombre", "El campo nombre solo acepta letras");
            }

            if (usuario.Apellido == null || usuario.Apellido == "")
                ModelState.AddModelError("Apellido", "El apellido es obligatorio");
            if (usuario.Apellido != null)
            {
                if (!Regex.IsMatch(usuario.Apellido, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Apellido", "El campo apellido solo acepta letras");
            }

            if (usuario.DNI == null || usuario.DNI == "")
                ModelState.AddModelError("DNI", "El DNI es obligatorio");

            if (usuario.DNI != null)
            {
                if (!Regex.IsMatch(usuario.DNI, "^\\d+$"))
                    ModelState.AddModelError("DNI", "El campo dni solo acepta numeros");
            }

            if (usuario.DNI != null)
            {
                if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
                {
                    if (usuario.DNI.Length != 8)
                        ModelState.AddModelError("DNI", "El campo dni debe de tener 8 numeros");
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
                if (!Regex.IsMatch(usuario.Celular, "^\\d+$"))
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
                ModelState.AddModelError("Perfil", "El campo perfil no puede ser vacio");
            }
            if (usuario.IdTipoUsuario == 0)
            {
                ModelState.AddModelError("TipoUsuario", "Seleccione un campo valido");
            }
        }
        public void validarEditarUsuario(Usuario usuario, int id)
        {


            if (usuario.Nombre == null || usuario.Nombre == "")
                ModelState.AddModelError("Nombre", "El nombre es obligatorio");
            if (usuario.Nombre != null)
            {
                if (!Regex.IsMatch(usuario.Nombre, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Nombre", "El campo nombre solo acepta letras");
            }

            if (usuario.Apellido == null || usuario.Apellido == "")
                ModelState.AddModelError("Apellido", "El apellido es obligatorio");
            if (usuario.Apellido != null)
            {
                if (!Regex.IsMatch(usuario.Apellido, @"^[a-zA-Z ]*$"))
                    ModelState.AddModelError("Apellido", "El campo apellido solo acepta letras");
            }

            if (usuario.DNI == null || usuario.DNI == "")
                ModelState.AddModelError("DNI", "El DNI es obligatorio");

            if (usuario.DNI != null)
            {
                if (!Regex.IsMatch(usuario.DNI, "^\\d+$"))
                    ModelState.AddModelError("DNI", "El campo dni solo acepta numeros");
            }

            if (usuario.DNI != null)
            {
                if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
                {
                    if (usuario.DNI.Length != 8)
                        ModelState.AddModelError("DNI", "El campo dni debe de tener 8 numeros");
                }
            }
            //if (usuario.DNI != null)
            //{
            //    if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
            //    {
            //        if (usuario.DNI.Length == 8)
            //        {
            //            var usuarioDB = conexion.Usuarios.Any(t => t.DNI == usuario.DNI);
            //            if (usuarioDB)
            //            {
            //                ModelState.AddModelError("DNI", "Este DNI ya existe");
            //            }
            //        }
            //    }
            //}


            if (usuario.Celular == null || usuario.Celular == "")
                ModelState.AddModelError("Celular", "El celular es obligatorio");

            if (usuario.Celular != null)
            {
                if (!Regex.IsMatch(usuario.Celular, "^\\d+$"))
                    ModelState.AddModelError("Celular", "El campo celular solo acepta numeros");
            }

            if (usuario.Celular != null)
            {
                if (!Regex.IsMatch(usuario.Celular, "^\\d+$"))
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

            //if (usuario.Correo != null)
            //{
            //    if (Regex.IsMatch(usuario.Correo, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
            //    {
            //        var usuarioCorreoDB = conexion.Usuarios.Any(t => t.Correo == usuario.Correo);
            //        if (usuarioCorreoDB)
            //        {
            //            ModelState.AddModelError("Correo", "Este correo ya existe");
            //        }
            //    }
            //}


            if (usuario.Direccion == null || usuario.Direccion == "")
                ModelState.AddModelError("Direccion", "La direccion es obligatorio");
            //if (usuario.Direccion != null)
            //{
            //    if (Regex.IsMatch(usuario.Direccion, @"^[a-zA-Z0-9""'\s.#]*$"))
            //        ModelState.AddModelError("Direccion", "Ejemplo Jr. La paz #121");
            //}
            if (usuario.Password == null || usuario.Password == "")
                ModelState.AddModelError("Password", "El pasword es obligatorio");


            //if (usuario.Perfil == null)
            //{
            //    ModelState.AddModelError("Perfil", "El campo perfil no pude ser vacio");
            //}
            //if (usuario.TipoUsuario == null)
            //{
            //    ModelState.AddModelError("TipoUsuario", "Seleccione un campo valido");
            //}
            if (usuario.DNI != null && usuario.DNI != "")
            {
                if (Regex.IsMatch(usuario.DNI, "^\\d+$"))
                {
                    if (usuario.DNI.Length == 8)
                    {
                        var IdUsuarioDB = conexion.Usuarios.Where(a => a.IdUsuario == id).First();
                        var UsuDNI = conexion.Usuarios.Where(a => a.DNI == IdUsuarioDB.DNI).First();
                        if (UsuDNI.DNI != usuario.DNI)
                        {
                            var usuarioDB = conexion.Usuarios.Any(t => t.DNI == usuario.DNI);
                            if (usuarioDB)
                            {
                                ModelState.AddModelError("DNI", "Este DNI ya existe");
                            }
                        }
                    }
                }
            }
            if (usuario.Correo != null)
            {
                if (Regex.IsMatch(usuario.Correo, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
                {
                    var UsuarioDBC = conexion.Usuarios.Where(a => a.IdUsuario == id).First();
                    var UsuCorreo = conexion.Usuarios.Where(a => a.Correo == UsuarioDBC.Correo).First();
                    if (UsuCorreo.Correo != usuario.Correo)
                    {
                        var usuarioDB = conexion.Usuarios.Any(t => t.Correo == usuario.Correo);
                        if (usuarioDB)
                        {
                            ModelState.AddModelError("Correo", "Este Correo ya existe");
                        }
                    }
                }
            }
        }
    }
}