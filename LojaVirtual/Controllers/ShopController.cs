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
            // Declare a list of ProductVM
            List<ProdutoVM> productVMList;

            using (Db db = new Db())
            {
                // Get category id
                CategoriaDTO categoryDTO = db.Categoria.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;

                // Init the list
                productVMList = db.Produto.ToArray().Where(x => x.CategoriaId == catId).Select(x => new ProdutoVM(x)).ToList();

            }

            // Return view with list
            return View(productVMList);
        }

        [ActionName("produtos-detalhes")]
        public ActionResult ProdutoDetalhes(string name)
        {
            // Declare the VM and DTO
            ProdutoVM model;
            ProdutoDTO dto;

            // Init product id
            int id = 0;

            using (Db db = new Db())
            {
                // Check if product exists
                if (!db.Produto.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                // Init productDTO
                dto = db.Produto.Where(x => x.Slug == name).FirstOrDefault();

                // Get id
                id = dto.Id;

                // Init model
                model = new ProdutoVM(dto);
            }

            // Get gallery images
            model.Galeria = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Produto/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));

            // Return view with model
            return View("ProdutoDetalhes", model);
        }

    }
}