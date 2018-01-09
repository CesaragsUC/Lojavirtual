using LojaVirtual.Models.Data;
using LojaVirtual.Models.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LojaVirtual.Controllers
{
    public class ContaController : Controller
    {
        // GET: Conta
        public ActionResult Index()
        {
            return Redirect("~/conta/login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUsuarioVM model)
        {
            string usuarionome = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isValido = false;

            using (Db db = new Db())
            {
                if (db.Usuario.Any(x => x.Login.Equals(model.Username) && x.Senha.Equals(model.Senha)))
                {
                    isValido = true;
                }
            }

            if (!isValido)
            {

                ModelState.AddModelError("", "Login ou senha está incorreto.");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, model.LembreMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.LembreMe));
            }

        }


        [ActionName("cria-conta")]
        [HttpGet]
        public ActionResult CriaConta()
        {
            return View("CriaConta");
        }


        [ActionName("cria-conta")]
        [HttpPost]
        public ActionResult CriaConta(UsuarioVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("CriaConta",model);
            }


            if (!model.Senha.Equals(model.ConfirmarSenha))
            {
                ModelState.AddModelError("", "Senhas não são iaguais.");
                return View("CriaConta", model);
            }

            using (Db db = new Db())
            {
                if (db.Usuario.Any(x => x.Login.Equals(model.Login)))
                {
                    ModelState.AddModelError("", "Login " + model.Login + "  está sendo usado");

                    model.Login = "";
                    return View("CriaConta", model);
                }


                UsuarioDTO userDTO = new UsuarioDTO
                {
                    PrimeiroNome = model.PrimeiroNome,
                    SegundoNome = model.SegundoNome,
                    Email = model.Email,
                    Login = model.Login,
                    Senha = model.Senha
                };

                db.Usuario.Add(userDTO);

                db.SaveChanges();

                int id = userDTO.Id;

                RegraUsuarioDTO regrausuartioDTO = new RegraUsuarioDTO
                {
                    UsuarioId = id,
                    RegraId = 2
                
                };

                db.RegraUsuario.Add(regrausuartioDTO);
                db.SaveChanges();
            }

            TempData["SM"] = "Você está registrado e pode fazer login";
           

            return Redirect("~/conta/login");
        }

        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/conta/login");
        }

        public ActionResult UsuarionavPartial()
        {
            // Get username
            string username = User.Identity.Name;

            // Declare model
            UsuarioNavPartial model;

            using (Db db = new Db())
            {
                // Get the user
                UsuarioDTO dto = db.Usuario.FirstOrDefault(x => x.Login == username);

                // Build the model
                model = new UsuarioNavPartial()
                {
                    PrimeiroNome = dto.PrimeiroNome,
                    Segundonome = dto.SegundoNome
                };
            }

            // Return partial view with model
            return PartialView(model);
        }
    }
}