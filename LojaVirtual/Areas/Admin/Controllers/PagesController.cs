using LojaVirtual.Models.Data;
using LojaVirtual.Models.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LojaVirtual.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //declara lista de paginaVM

            List<PaginaVM> paginaList;

            using (Db db = new Db())
            {
                //Inicia a lista
                paginaList = db.Paginas.ToArray().OrderBy(x => x.Sorting).Select(x => new PaginaVM(x)).ToList();
            }
            // retorna uma view com lista    
            return View(paginaList);
        }

        //Admin/ view adicionar paginas
        [HttpGet]
        public ActionResult AddPagina()
        {
            return View();
        }

        //Admin/Confirma Adiciona nova paginas
        [HttpPost]
        public ActionResult AddPagina(PaginaVM model)
        {
            //verifica estado do modelo
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //declara slug
                string slug;
                //Inicial paginaDTO
                PaginaDTO dto = new PaginaDTO();
                //DTO titulo
                dto.Titulo = model.Titulo;
                //verifica e configura slug se necessario
                if (string.IsNullOrEmpty(model.Slug))
                {
                    slug = model.Titulo.Replace(" ", "-").ToLower();
                }
                else
                {
                    
                    slug = model.Slug.Replace(" ", "-").ToLower();
                    
                }
                //Confirma se titulo e slug são unique
                if (db.Paginas.Any(x => x.Titulo == model.Titulo) || db.Paginas.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Ese titulo ou slug já existe");
                    return View(model);
                        
                        
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Corpo = model.Corpo;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100 ;

                //Salva DTO
                db.Paginas.Add(dto);
                db.SaveChanges();
            }


            //Configura mensagem temporaria(TempData)
            TempData["SM"] = "Você adicionou uma nova pagina";

            //redirect
            return RedirectToAction("AddPagina");
        }

        //Admin/ View pra Editar paginas
        [HttpGet]
        public ActionResult EditarPagina(int? id)
        {
            PaginaVM model;
            using (Db db = new Db())
            {

                PaginaDTO dto = db.Paginas.Find(id);
                if (dto == null)
                {
                    return Content("Essa pagina não existe");
                }
                model = new PaginaVM(dto);
            }

            return View(model);
        }

        //Admin/ Confirma Editar paginas
        [HttpPost]
        public ActionResult EditarPagina(PaginaVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            using (Db db = new Db())
            {
                  int id = model.Id;
                  string slug = "home";
                  PaginaDTO dto = db.Paginas.Find(id);

                  dto.Titulo = model.Titulo;
                  if (model.Slug != "home")
                  {
                      if (string.IsNullOrEmpty(model.Slug))
                      {
                          slug = model.Titulo.Replace(" ", "-").ToLower();
                      }
                      else
                      {
                          slug = model.Slug.Replace(" ", "-").ToLower();
                      }
                  }
                   if(db.Paginas.Where(x=> x.Id != id).Any(x=> x.Titulo == model.Titulo) ||
                      db.Paginas.Where(x => x.Id != id).Any(x => x.Slug == slug))
                    {
                        ModelState.AddModelError("", "Esse Slug já existe");
                        return View();
                    }

                    dto.Slug = slug;
                    dto.Corpo = model.Corpo;
                    dto.HasSidebar = model.HasSidebar;

                    db.SaveChanges();
                }

            //Configura mensagem temporaria(TempData)
            TempData["ED"] = "Página alterada com sucesso.";
            return RedirectToAction("EditarPagina");
        }

        //Admin/ Detalhes da paginas
        public ActionResult DetalhesPagina(int id)
        {
            PaginaVM model;
            using (Db db = new Db())
            {
                PaginaDTO dto = db.Paginas.Find(id);
                if(dto == null)
                {
                    return Content("Essa pagina não existe");
                }
                model = new PaginaVM(dto);
            }
            return View(model);
        }

        //Admin/ Deletar paginas
        public ActionResult DeletarPagina(int id)
        {

            using (Db db = new Db())
            {
                PaginaDTO dto = db.Paginas.Find(id);

                db.Paginas.Remove(dto);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        //Admin/ Reordenar paginas
        [HttpPost]
        public ActionResult ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                PaginaDTO dto;
                foreach (var paginaId in id)
                {
                    dto = db.Paginas.Find(id);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult EditSidebar()
        {
            //declara modelo
            SidebarVM model;

            using (Db db = new Db())
            {
                //pega o DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //inicia o modelo
                model = new SidebarVM(dto);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {

            using (Db db = new Db())
            {
                //pega o DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                //inicia o corpo
                dto.Body = model.Body;

                //salva
                db.SaveChanges();
            }
            // mensagem de confirmação
            TempData["ES"] = "Sidebar Editado Com sucesso!";

            return RedirectToAction("EditSidebar");
        }

    }
}