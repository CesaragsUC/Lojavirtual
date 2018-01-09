using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.ViewModel.Account
{
    public class LoginUsuarioVM
    {
        [Required]
        public string  Username { get; set; }
        [Required]
        public string Senha { get; set; }
        public bool LembreMe { get; set; }
    }
}