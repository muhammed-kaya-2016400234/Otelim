using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Otelim.Models;
namespace Mvc_Otelim.Controllers
{
    public class HomeController : Controller
    {
        OtelModel1 m = new OtelModel1();
        //
        // GET: /Home/

        public ActionResult Index()
        {


            List<string> list = m.Otel.Select(x => x.Şehir).Distinct().ToList();
            return View(list);
        }

       

    }
}
