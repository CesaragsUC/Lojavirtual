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
            string usuarionome = User.Identity.Name;

            if (!string.IsNullOrEmpty(usuarionome))
            {
                return RedirectToAction("usuario-perfil");
            }
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


        [HttpGet]
        [ActionName("usuario-perfil")]
        public ActionResult UsuarioProfile()
        {


            string username = User.Identity.Name;

            UsuarioProfileVM model;

            using (Db db = new Db())
            {
                UsuarioDTO dto = db.Usuario.FirstOrDefault(x => x.Login == username);

                model = new UsuarioProfileVM(dto);

            }

            return View("UsuarioProfile", model);
        }


        [HttpPost]
        [ActionName("usuario-perfil")]
        public ActionResult UsuarioProfile(UsuarioProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("UsuarioProfile", model);
            }

            if (!string.IsNullOrEmpty(model.Senha))
            {
                if (!model.Senha.Equals(model.ConfirmarSenha))
                {
                    ModelState.AddModelError("","Senhas não são iguais.");
                    return View("UsuarioProfile", model);
                }
            }

            using (Db db = new Db())
            {
                string username = User.Identity.Name;

                if(db.Usuario.Where(x => x.Id !=  model.Id).Any(x => x.Login == username))
                {
                    ModelState.AddModelError("","Login " + model.Login + " já existe.");
                    model.Login = "";

                    return View("UsuarioProfile",model);
                }

                UsuarioDTO dto = db.Usuario.Find(model.Id);
                dto.PrimeiroNome = model.PrimeiroNome;
                dto.SegundoNome = model.SegundoNome;
                dto.Email = model.Email;
                dto.Login = model.Login;

                if (!string .IsNullOrEmpty(model.Senha))
                {
                    dto.Senha = model.Senha;
                }

                db.SaveChanges();

            }

            TempData["SM"] = "Perfil editado com sucesso!";

            return Redirect("~/conta/usuario-perfil");
        }
    }
}