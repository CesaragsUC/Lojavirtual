using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.Data
{
    [Table("RegraUsuario")]
    public class RegraUsuarioDTO
    {
        [Key, Column(Order =0)]
        public int UsuarioId { get; set; }
        [Key, Column(Order =1)]
        public int RegraId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual UsuarioDTO Usuario { get; set; }

        [ForeignKey("RegraId")]
        public virtual RegrasDTO Regras { get; set; }
    }
}