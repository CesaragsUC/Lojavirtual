using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.Data
{   
    [Table("tblPages")]
    public class PaginaDTO
    {
        [Key]
        public int Id { get; set; }
        public string Titulo  { get; set; }
        public string Corpo { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }
    }
}