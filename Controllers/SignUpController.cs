using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Otelim.Models;
namespace Mvc_Otelim.Controllers
{
    public class SignUpController : Controller
    {
        //
        // GET: /SignUp/
        OtelModel1 m = new OtelModel1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Kayıt(string ad,string soyad,string email,string password)
        {
            Kullanıcı k = new Kullanıcı();
            k.Ad = ad;
            k.Soyad = soyad;
            k.Mail = email;
            k.Sifre = password;
            m.Kullanıcı.Add(k);
            m.SaveChanges();
            return RedirectToAction("../Login/Index");
        }

    }
}
