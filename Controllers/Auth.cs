using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_Otelim.Controllers
{
  
        public class UserAuthorize : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                if (Roles == "admin")
                {  
                    if (httpContext.Session["aid"] != null)
                    {
                        return true;
                    }
                   
                }else if (Roles == "otel_yönetici")
                {
                    if (httpContext.Session["yid"] != null)
                    {
                        return true;
                    }

                }
                else
                {
                    if (httpContext.Session["id"] != null)
                    {
                        return true;
                    }



            }

            httpContext.Response.Redirect("/Login/Index");
                    return false;
                

        }
        }
    
}