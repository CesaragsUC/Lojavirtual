using LojaVirtual.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LojaVirtual.Models.ViewModel.Shop
{
    public class ProdutoVM
    {
        public ProdutoVM()
        {

        }
        public ProdutoVM(ProdutoDTO row)
        {
            Id = row.Id;
            Nome = row.Nome;
            Slug = row.Slug;
            Descricao = row.Descricao;
            CategoriaNome = row.CategoriaNome;
            ImagemNome = row.ImagemNome;
            Preco = row.Preco;
            CategoriaId = row.CategoriaId;
        }
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Descricao { get; set; }
        public string CategoriaNome { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        public string ImagemNome { get; set; }
        public decimal Preco { get; set; }

        public IEnumerable<SelectListItem> Categoria { get; set; }
        public IEnumerable<string> Galeria { get; set; }
    }
}