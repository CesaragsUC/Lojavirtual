using LojaVirtual.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LojaVirtual.Models.ViewModel.Pages
{
    public class PaginaVM
    {
        public PaginaVM()
        {

        }
        public PaginaVM(PaginaDTO row)
        {
            Id = row.Id;
            Titulo = row.Titulo;
            Slug = row.Slug;
            Corpo = row.Corpo;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]

        [AllowHtml]
        public string Corpo { get; set; }
        public int Sorting { get; set; }
        public string Slug { get; set; }
        public bool HasSidebar { get; set; }
    }
}