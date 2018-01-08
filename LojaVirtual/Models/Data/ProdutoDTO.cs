using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.Data
{
    //DTO = Data Table Object

    [Table("Produto")]
    public class ProdutoDTO
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string Descricao { get; set; }
        public string CategoriaNome { get; set; }
        public int CategoriaId { get; set; }
        public string ImagemNome { get; set; }
        public decimal Preco { get; set; }


        [ForeignKey("CategoriaId")]
        public virtual CategoriaDTO Categoria { get; set; }
    }
}