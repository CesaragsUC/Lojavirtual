using LojaVirtual.Models.Data;
using LojaVirtual.Models.ViewModel.Carrinho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LojaVirtual.Controllers
{
    public class CarrinhoController : Controller
    {
        // GET: Carrinho
        public ActionResult Index()
        {

            //inicia uma lista de carrinho
            var cart = Session["carrinho"] as List<CarrinhoVM> ?? new List<CarrinhoVM>();

            //verifica se o carrinho está vazio
            if(cart.Count() == 0 || Session["carrinho"] == null)
            {
                ViewBag.Mensagem = "Seu Carrinho está vazio.";
                return View();
            }


            //calcula o total e salva na viewbag
            decimal total = 0m;
            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.TotalGeral = total;

            //retorna uma view com a lista
            return View(cart);
        }


        public ActionResult CarrinhoPartial()
        {
            CarrinhoVM model = new CarrinhoVM();

            int qtd = 0;

            decimal preco = 0m;

            if(Session["carrinho"] != null)
            {
                var list = (List<CarrinhoVM >) Session["carrinho"];

                foreach (var item in list)
                {
                    qtd += item.Quantidade;
                    preco += item.Quantidade * item.Preco;
                }
                model.Quantidade = qtd;
                model.Preco = preco;
            }
            else
            {
                model.Quantidade = 0;
                model.Preco = 0m;
            }

            return PartialView(model);
        }

        public ActionResult AddAoCarrinhoPartial(int id)
        {
            List<CarrinhoVM> cart = Session["carrinho"] as List<CarrinhoVM> ?? new List<CarrinhoVM>();

            CarrinhoVM model = new CarrinhoVM();

            using (Db db = new Db())
            {
                ProdutoDTO produto = db.Produto.Find(id);

                var produtoNoCarrinho = cart.FirstOrDefault(x => x.ProdutoId == id);

                if(produtoNoCarrinho == null)
                {
                    cart.Add(new CarrinhoVM()
                    {
                        ProdutoId = produto.Id,
                        ProdutoNome =produto.Nome,
                        Quantidade = 1,
                        Preco = produto.Preco,
                        Imagem = produto.ImagemNome
                    });
                }
                else
                {
                    produtoNoCarrinho.Quantidade++;
                }
            }

            int qtd = 0;
            decimal preco = 0m;

            foreach (var item in cart)
            {
                qtd += item.Quantidade;
                preco += item.Quantidade * item.Preco;
            }

            model.Quantidade = qtd;
            model.Preco = preco;

            Session["carrinho"] = cart;

            return PartialView(model);
                
        }

        public JsonResult IncrementaProduto(int produtoId)
        {

            List<CarrinhoVM> cart = Session["carrinho"] as List<CarrinhoVM>;

            using (Db db= new Db())
            {
                CarrinhoVM model = cart.FirstOrDefault(x => x.ProdutoId == produtoId);

                model.Quantidade++;

                var resultado = new { qtd = model.Quantidade, preco = model.Preco };

                return Json(resultado,JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DecrementaProduto(int produtoId)
        {
            // Init cart
            List<CarrinhoVM> cart = Session["carrinho"] as List<CarrinhoVM>;

            using (Db db = new Db())
            {
                // Get model from list
                CarrinhoVM model = cart.FirstOrDefault(x => x.ProdutoId == produtoId);

                // Decrement qty
                if (model.Quantidade > 1)
                {
                    model.Quantidade--;
                }
                else
                {
                    model.Quantidade = 0;
                    cart.Remove(model);
                }

                // Store needed data
                var resultado = new { qtd = model.Quantidade, preco = model.Preco };

                // Return json
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }
        public void RemoveProduto(int produtoId)
        {
            // Init cart list
            List<CarrinhoVM> cart = Session["carrinho"] as List<CarrinhoVM>;

            using (Db db = new Db())
            {
                // Get model from list
                CarrinhoVM model = cart.FirstOrDefault(x => x.ProdutoId == produtoId);

                // Remove model from list
                cart.Remove(model);
            }

        }

        public ActionResult PaypalPartial()
        {
            List<CarrinhoVM> cart = Session["carrinho"] as List<CarrinhoVM>;

            return PartialView(cart);
        }

    }
}