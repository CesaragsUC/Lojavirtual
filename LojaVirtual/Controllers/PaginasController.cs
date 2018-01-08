using LojaVirtual.Models.Data;
using LojaVirtual.Models.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LojaVirtual.Controllers
{
    public class PaginasController : Controller
    {
        // GET: Paginas
        public ActionResult Index(string pagina = "")// poderia ser direto public ActionResult Index(string pagina = "home") sem if abaixo
        {
            //Get/Set pagina slug

            if (pagina == "")
            {
                pagina = "home";
            }

            //Declara model DTO
            PaginaVM model;
            PaginaDTO dto;

            //Verifica se pagina existe
            using (Db db = new Db())
            {
                if (!db.Paginas.Any(x => x.Slug.Equals(pagina)))// caso não tenha nehuma pagina com nome home
                {
                    return RedirectToAction("Index", new { pagina = "" });//retorna minah index com paramaentro pagina  vazio
                }
            }

            //Pega pagina DTO
            using (Db db = new Db())
            {
                dto = db.Paginas.Where(x => x.Slug == pagina).FirstOrDefault();//procura na tabela pagina algum slug com nome home
            }

            //Set titulo da pagina
            ViewBag.TituloPagina = dto.Titulo; // view bag recebe nome da slug, nesse caso será nome home


            //Verifica por sidebar
            if (dto.HasSidebar == true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }

            //inicia o modelo
            model = new PaginaVM(dto);// retorna a view da pagina que contem nome "home" com conteudo do seu corpo(body)

            //retorn view com modelo
            return View(model);
        }


        public ActionResult PaginaMenupartial()
        {
            //Declara a lista de paginasVM

            List<PaginaVM> ListadepaginasVM;


            //Pego todas as paginas execto a home
            using (Db db= new Db())
            {
                ListadepaginasVM = db.Paginas.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PaginaVM(x)).ToList();
            }



            //Retorno a view com a lista de paginas
            return PartialView(ListadepaginasVM);
        }


        public ActionResult Sidebarpartial()
        {
            SidebarVM model;
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                model = new SidebarVM(dto);
            }


            return PartialView(model);
        }
    }
}