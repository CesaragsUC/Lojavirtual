using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.Data
{
    [Table("Regras")]
    public class RegrasDTO
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}