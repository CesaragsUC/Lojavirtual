using LojaVirtual.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.ViewModel.Account
{
    public class UsuarioVM
    {
        public UsuarioVM()
        {

        }
        public UsuarioVM(UsuarioDTO row)
        {
            Id = row.Id;
            PrimeiroNome = row.PrimeiroNome;
            SegundoNome = row.SegundoNome;
            Email = row.Email;
            Login = row.Login;
            Senha = row.Senha;
        }

        public int Id { get; set; }

        [Required]
        public string PrimeiroNome { get; set; }

        [Required]
        public string SegundoNome { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string ConfirmarSenha { get; set; }
    }

}