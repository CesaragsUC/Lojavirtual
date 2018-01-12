using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LojaVirtual.Areas.Admin.Models.ViewModel.Shop
{
    public class PedidoParaAdminVM
    {

        public int NumeroPedido { get; set; }
        public string Login { get; set; }
        public decimal Total { get; set; }
        public Dictionary <string, int> ProdutoQtd{ get; set; }
        public DateTime DataCriacao { get; set; }

    }
}