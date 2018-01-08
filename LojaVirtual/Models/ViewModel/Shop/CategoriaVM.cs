using LojaVirtual.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.ViewModel.Shop
{
    public class CategoriaVM
    {
        public CategoriaVM()
        {

        }
        public CategoriaVM(CategoriaDTO row)
        {
            Id = row.Id;
            Nome = row.Nome;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}