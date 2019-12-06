using PROYECTO_INCABATHS.Clases;
using PROYECTO_INCABATHS.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PROYECTO_INCABATHS.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        private AppConexionDB conexion = new AppConexionDB();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario usuario, string RepitaPassword)//*string Correo, string Password*/)
        {
            var UExiste = conexion.Usuarios.Count(u => u.Correo == usuario.Correo && u.Password == usuario.Password);

            if (UExiste != 0)
            {
                var UsuarioDB = conexion.Usuarios.Where(u => u.Correo == usuario.Correo && u.Password == usuario.Password).First();
                FormsAuthentication.SetAuthCookie(UsuarioDB.Correo, false);

                if (UsuarioDB.IdTipoUsuario == 1)
                {
                    Session["UsuarioId"] = UsuarioDB.IdUsuario;
                    Session["UsuarioNombre"] = UsuarioDB.Nombre;
                    Session["UsuarioPerfil"] = UsuarioDB.Perfil;
                    Session["UsuarioDNI"] = UsuarioDB.DNI;
                    return RedirectToAction("Index", "Admin");
                }
                else if (UsuarioDB.IdTipoUsuario == 3)
                {
                    Session["UsuarioId"] = UsuarioDB.IdUsuario;
                    Session["UsuarioNombre"] = UsuarioDB.Nombre;
                    Session["UsuarioPerfil"] = UsuarioDB.Perfil;
                    Session["UsuarioDNI"] = UsuarioDB.DNI;
                    return RedirectToAction("Servicio", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Validation = "Usuario y/o contraseña incorrecta";
            return View();
        }
        public ActionResult Logout()
        {
            //Cuando cerramos sesion lo eliminamos la cookie
            FormsAuthentication.SignOut();

            Session.Clear();
            return RedirectToAction("login");
        }
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
        public string ObtenerUsuario()
        {
            string nombre;
            if (Session["UsuarioNombre"] == null)
            {
                nombre = "null";
                return nombre;
            }
            else
            {
                nombre = "1";
                return nombre;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public string RecuperarContrasenha(string correo)
        {

            var existe = conexion.Usuarios.Count(u => u.Correo == correo);

            if (existe > 0)
            {
                var UsuDB = conexion.Usuarios.Where(u => u.Correo == correo).First();

                string resultado = "Contraseña enviada al correo: " + correo;
                string password = conexion.Usuarios.Where(u => correo.Equals(u.Correo)).Select(q => q.Password).FirstOrDefault();
                if (!String.IsNullOrEmpty(password))
                {
                    MailMessage Correo = new MailMessage();
                    Correo.From = new MailAddress(UsuDB.Correo);
                    Correo.To.Add(correo);
                    Correo.Subject = ("Recuperar Contraseña INCABATHS");
                    Correo.Body = "Hola, tu contraseña actual es: " + UsuDB.Password;
                    Correo.Priority = MailPriority.High;

                    SmtpClient ServerEmail = new SmtpClient();
                    ServerEmail.Credentials = new NetworkCredential(UsuDB.Correo, UsuDB.Password);
                    ServerEmail.Host = "smtp.gmail.com";
                    ServerEmail.Port = 587;
                    ServerEmail.EnableSsl = true;
                    try
                    {
                        ServerEmail.Send(Correo);
                    }
                    catch (Exception e)
                    {
                        resultado = e.Message;
                    }
                    Correo.Dispose();
                    return resultado;
                }
            }else
            {
                return "Esta cuenta no existe";
            }
            return "Error";
        }
    }
}