using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.Data
{
    [Table("PedidoDetalhe")]
    public class DetalhePedidoDTO
    {
        [Key]
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        public int Quantidade { get; set; }
        public int ProdutoId { get; set; }


        [ForeignKey("PedidoId")]
        public virtual UsuarioDTO Pedido { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual UsuarioDTO Usuario { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual UsuarioDTO Produto { get; set; }

    }
}