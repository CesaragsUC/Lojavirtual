using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LojaVirtual
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Carrinho", "Carrinho/{action}/{id}", new { controller = "Carrinho", action = "Index",id = UrlParameter.Optional }, new[] { "LojaVirtual.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name= UrlParameter.Optional }, new[] { "LojaVirtual.Controllers" });
            routes.MapRoute("Sidebarpartial", "Paginas/Sidebarpartial", new { controller = "Paginas", action = "Sidebarpartial" }, new[] { "LojaVirtual.Controllers" });
            routes.MapRoute("PaginaMenupartial", "Paginas/PaginaMenupartial", new { controller = "Paginas", action = "PaginaMenupartial" }, new[] { "LojaVirtual.Controllers" });
            routes.MapRoute("Paginas", "{pagina}", new { controller = "Paginas", action = "Index" }, new[] { "LojaVirtual.Controllers" });
            routes.MapRoute("Default", "", new { controller = "Paginas", action = "Index" }, new[] { "LojaVirtual.Controllers" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
