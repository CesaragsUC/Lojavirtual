using LojaVirtual.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.ViewModel.Shop
{
    public class PedidoVM
    {
        public PedidoVM()
        {

        }

        public PedidoVM(PedidoDTO row)
        {
            PedidoId = row.PedidoId;
            UsuarioId = row.UsuarioId;
            DataCriacao = row.DataCriacao;
        }

        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}