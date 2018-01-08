using LojaVirtual.Models.Data;
using LojaVirtual.Models.ViewModel.Shop;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace LojaVirtual.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Categoria
        public ActionResult Categoria()
        {
            //cria uma lista de categoria do tipo Categoria
            List<CategoriaVM> categoriaVMList;

            using (Db db = new Db())
            {
                categoriaVMList = db.Categoria.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoriaVM(x)).ToList();
            }
            return View(categoriaVMList);
        }

        [HttpPost]
        public string AdicionarCategoria(string catName)
        {
            //declaracao do ID
            string id;

            //Verifica se o nome da categoria é unica, nao pode ter repeticao de nome
            using (Db db = new Db())
            {
                if (db.Categoria.Any(x => x.Nome == catName))
                    return "titletaken";

                //Inicia DTO
                CategoriaDTO dto = new CategoriaDTO();
                dto.Nome = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                //Salva DTO
                db.Categoria.Add(dto);
                db.SaveChanges();

                //Pega ID
                id = dto.Id.ToString();
                //redirect
                
            }
            TempData["CT"] = "Você adicionou uma nova Categoria";

            //Retorna ID
            return id;
  
        }
        public ActionResult DeletaCategoria(int id)
        {
            using (Db db = new Db())
            {
                CategoriaDTO dto = db.Categoria.Find(id);
                db.Categoria.Remove(dto);
                db.SaveChanges();
            }
            return RedirectToAction("Categoria");
        }

        [HttpPost]
        public string RenomearCategoria(string novoNome, int id)
        {
            using (Db db = new Db())
            {
                if (db.Categoria.Any(x => x.Nome == novoNome))
                    return "titletaken";

                CategoriaDTO dto = db.Categoria.Find(id);

                dto.Nome = novoNome;
                dto.Slug = novoNome.Replace(" ", "-").ToLower();

                db.SaveChanges();
            }
            return "ok";
        }

        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduto()
        {
            // Init model
            ProdutoVM model = new ProdutoVM();

            // Add select list of Categoria to model
            using (Db db = new Db())
            {
                model.Categoria = new SelectList(db.Categoria.ToList(), "Id", "Nome");
            }

            // Return view with model
            return View(model);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]

        public ActionResult AddProduto(ProdutoVM model, HttpPostedFileBase file)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categoria = new SelectList(db.Categoria.ToList(), "Id", "Nome");
                    return View(model);
                }
            }

            // Make sure product name is unique
            using (Db db = new Db())
            {
                if (db.Produto.Any(x => x.Nome == model.Nome))
                {
                    model.Categoria = new SelectList(db.Categoria.ToList(), "Id", "Nome");
                    ModelState.AddModelError("", "Esse nome de produto já está sendo usado");
                    return View(model);
                }
            }

            // Declare product id
            int id;

            // Init and save ProdutoDTO
            using (Db db = new Db())
            {
                ProdutoDTO product = new ProdutoDTO();

                product.Nome = model.Nome;
                product.Slug = model.Nome.Replace(" ", "-").ToLower();
                product.Descricao = model.Descricao;
                product.Preco = model.Preco;
                product.CategoriaId = model.CategoriaId;

                CategoriaDTO catDTO = db.Categoria.FirstOrDefault(x => x.Id == model.CategoriaId);
                product.CategoriaNome = catDTO.Nome;

                db.Produto.Add(product);
                db.SaveChanges();

                // Get the id
                id = product.Id;
            }

            // Set TempData message
            TempData["SM"] = "Você adicionou um produto!";

            #region Upload Image

            // Create necessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Produto");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            // Check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categoria = new SelectList(db.Categoria.ToList(), "Id", "Nome");
                        ModelState.AddModelError("", "Erro ao enviar imagem - formato incorreto");
                        return View(model);
                    }
                }

                // Init image name
                string imageName = file.FileName;

                // Save image name to DTO
                using (Db db = new Db())
                {
                    ProdutoDTO dto = db.Produto.Find(id);
                    dto.ImagemNome = imageName;

                    db.SaveChanges();
                }

                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // Save original
                file.SaveAs(path);

                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            // Redirect
            return RedirectToAction("AddProduto");
        }

        // GET: Admin/Shop/Produto
        public ActionResult Produtos(int? pagina, int? catId)
        {
            // Declare a list of ProdutoVM
            List<ProdutoVM> listOfProdutoVM;

            // Set page number
            var pageNumber = pagina ?? 1;

            using (Db db = new Db())
            {
                // Init the list
                listOfProdutoVM = db.Produto.ToArray()
                                  .Where(x => catId == null || catId == 0 || x.CategoriaId == catId)
                                  .Select(x => new ProdutoVM(x))
                                  .ToList();

                // Populate Categoria select list
                ViewBag.Categoria = new SelectList(db.Categoria.ToList(), "Id", "Nome");

                // Set selected category
                ViewBag.SelectedCategoria = catId.ToString();
            }

            // Set pagination
            var onePageOfProduto = listOfProdutoVM.ToPagedList(pageNumber, 3);
            ViewBag.onPaginadeProdutos = onePageOfProduto;

            // Return view with list
            return View(listOfProdutoVM);
        }

        // GET: Admin/Shop/EditProduct/id
        [HttpGet]
        public ActionResult Editarproduto(int id)
        {
            // Declare ProdutoVM
            ProdutoVM model;

            using (Db db = new Db())
            {
                // Get the product
                ProdutoDTO dto = db.Produto.Find(id);

                // Make sure product exists
                if (dto == null)
                {
                    return Content("Esse produtonão existe.");
                }

                // init model
                model = new ProdutoVM(dto);

                // Make a select list
                model.Categoria = new SelectList(db.Categoria.ToList(), "Id", "Nome");

                // Get all gallery images
                model.Galeria = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Produto/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));
            }

            // Return view with model
            return View(model);
        }

        // POST: Admin/Shop/EditProduct/id
        [HttpPost]
        public ActionResult Editarproduto(ProdutoVM model, HttpPostedFileBase file)
        {
            // Get product id
            int id = model.Id;

            // Populate Categoria select list and gallery images
            using (Db db = new Db())
            {
                model.Categoria = new SelectList(db.Categoria.ToList(), "Id", "Nome");
            }
            model.Galeria = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Produto/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));

            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Make sure product name is unique
            using (Db db = new Db())
            {
                if (db.Produto.Where(x => x.Id != id).Any(x => x.Nome == model.Nome))
                {
                    ModelState.AddModelError("", "Nome do produto já em uso");
                    return View(model);
                }
            }

            // Update product
            using (Db db = new Db())
            {
                ProdutoDTO dto = db.Produto.Find(id);

                dto.Nome = model.Nome;
                dto.Slug = model.Nome.Replace(" ", "-").ToLower();
                dto.Descricao = model.Descricao;
                dto.Preco = model.Preco;
                dto.CategoriaId = model.CategoriaId;
                dto.ImagemNome = model.ImagemNome;

                CategoriaDTO catDTO = db.Categoria.FirstOrDefault(x => x.Id == model.CategoriaId);
                dto.CategoriaNome = catDTO.Nome;

                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "Produto editado com sucesso";

            #region Image Upload

            // Check for file upload
            if (file != null && file.ContentLength > 0)
            {

                // Get extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        ModelState.AddModelError("", "Erro ao enviar imagem - formato incorreto");
                        return View(model);
                    }
                }

                // Set uplpad directory paths
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString() + "\\Thumbs");

                // Delete files from directories

                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();

                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();

                // Save image name

                string imageName = file.FileName;

                using (Db db = new Db())
                {
                    ProdutoDTO dto = db.Produto.Find(id);
                    dto.ImagemNome = imageName;

                    db.SaveChanges();
                }

                // Save original and thumb images

                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            // Redirect
            return RedirectToAction("Editarproduto");
        }

        // GET: Admin/Shop/DeleteProduct/id
        public ActionResult DeletarProduto(int id)
        {
            // Delete product from DB
            using (Db db = new Db())
            {
                ProdutoDTO dto = db.Produto.Find(id);
                db.Produto.Remove(dto);

                db.SaveChanges();
            }

            // Delete product folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathString = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            // Redirect
            return RedirectToAction("Produtos");
        }

        // POST: Admin/Shop/SaveGalleryImages
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            // Loop through files
            foreach (string fileName in Request.Files)
            {
                // Init the file
                HttpPostedFileBase file = Request.Files[fileName];

                // Check it's not null
                if (file != null && file.ContentLength > 0)
                {
                    // Set directory paths
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Produto\\" + id.ToString() + "\\Gallery\\Thumbs");

                    // Set image paths
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    // Save original and thumb

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }

            }

        }

        [HttpPost]
        public void DeletarImagem(int id, string nomeimagem)
        {
            string caminhocompleto1 = Request.MapPath("~Images/Uploads/Produto/" +id.ToString()+"/Gallery/"+nomeimagem);
            string caminhocompleto2 = Request.MapPath("~Images/Uploads/Produto/" + id.ToString() + "/Gallery/Thumbs" + nomeimagem);


            if (System.IO.File.Exists(caminhocompleto1))
                System.IO.File.Delete(caminhocompleto1);


            if (System.IO.File.Exists(caminhocompleto2))
                System.IO.File.Delete(caminhocompleto2);
        }
    }
}