using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc_Otelim
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(

               name: "OtelBilgileri",

               url: "Otel/Görüntüle/{id}/{kisiSayı}/{giriş}/{çıkış}",

               defaults: new { controller = "Otel", action = "Görüntüle", id = UrlParameter.Optional, kisiSayı = UrlParameter.Optional, giriş = UrlParameter.Optional, çıkış = UrlParameter.Optional }

);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
           
        }
    }
}