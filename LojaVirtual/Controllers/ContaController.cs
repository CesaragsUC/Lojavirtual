using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LojaVirtual.Controllers
{
    public class ContaController : Controller
    {
        // GET: Conta
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("cria-conta")]
        public ActionResult CriaConta()
        {
            return View("CriaConta");
        }
    }
}