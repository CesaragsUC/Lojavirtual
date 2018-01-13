using LojaVirtual.Models.Data;
using LojaVirtual.Models.ViewModel.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LojaVirtual.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index","Paginas");
        }

        public ActionResult MenuCategoriapartial()
        {

            List<CategoriaVM> ListaCategoriaVM;

            using (Db db = new Db())
            {
                ListaCategoriaVM = db.Categoria.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoriaVM(x)).ToList();
            }

            return PartialView(ListaCategoriaVM);
        }

        // GET: /shop/category/name
        public ActionResult Categoria(string name)
        {
            // Declara a lista de ProdutoVM
            List<ProdutoVM> productVMList;

            using (Db db = new Db())
            {
                // Get categoria id
                CategoriaDTO categoryDTO = db.Categoria.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;

                // Inicia a lista
                productVMList = db.Produto.ToArray().Where(x => x.CategoriaId == catId).Select(x => new ProdutoVM(x)).ToList();

            }

            // Return view com lista
            return View(productVMList);
        }

        [ActionName("produtos-detalhes")]
        public ActionResult ProdutoDetalhes(string name)
        {
            // Declara a VM e DTO
            ProdutoVM model;
            ProdutoDTO dto;

            // Inicia produto id
            int id = 0;

            using (Db db = new Db())
            {
                // verifica se produto existe
                if (!db.Produto.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                // Inicia produtoDTO
                dto = db.Produto.Where(x => x.Slug == name).FirstOrDefault();

                // Get id
                id = dto.Id;

                // Inicia modelo
                model = new ProdutoVM(dto);
            }

            // Get galeria de images
            model.Galeria = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Produto/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));

            // Return view com modelo
            return View("ProdutoDetalhes", model);
        }

    }
}