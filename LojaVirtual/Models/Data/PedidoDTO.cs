using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.Data
{
    [Table("Pedido")]
    public class PedidoDTO
    {
        [Key]
        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual UsuarioDTO Usuario { get; set; }

    }
}