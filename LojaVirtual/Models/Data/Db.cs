using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LojaVirtual.Models.Data
{
    public class Db:DbContext
    {

        public DbSet<PaginaDTO> Paginas { get; set; }
        public DbSet<SidebarDTO> Sidebar { get; set; }
        public DbSet<CategoriaDTO> Categoria { get; set; }
        public DbSet<ProdutoDTO> Produto { get; set; }
        public DbSet<UsuarioDTO> Usuario { get; set; }
        public DbSet<RegrasDTO> Regras { get; set; }
        public DbSet<RegraUsuarioDTO> RegraUsuario { get; set; }
        public DbSet<PedidoDTO> Pedido { get; set; }
        public DbSet<DetalhePedidoDTO> PedidoDetalhes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) // metodo que retira tipo plural  no nomes das tabelas
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<LojaVirtual.Models.ViewModel.Pages.PaginaVM> PaginaVMs { get; set; }

        public System.Data.Entity.DbSet<LojaVirtual.Models.ViewModel.Pages.SidebarVM> SidebarVMs { get; set; }

        public System.Data.Entity.DbSet<LojaVirtual.Models.ViewModel.Shop.CategoriaVM> CategoriaVMs { get; set; }

        public System.Data.Entity.DbSet<LojaVirtual.Models.ViewModel.Shop.ProdutoVM> ProdutoVMs { get; set; }

        public System.Data.Entity.DbSet<LojaVirtual.Models.ViewModel.Carrinho.CarrinhoVM> CarrinhoVMs { get; set; }

        public System.Data.Entity.DbSet<LojaVirtual.Models.ViewModel.Account.UsuarioVM> UsuarioVMs { get; set; }
    }
}