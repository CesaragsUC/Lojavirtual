using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.ViewModel.Carrinho
{
    public class CarrinhoVM
    {

        [Key]
        public int ProdutoId { get; set; }
        public string  ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public decimal Total { get { return Quantidade * Preco; } }
        public string Imagem { get; set; }

    }
}