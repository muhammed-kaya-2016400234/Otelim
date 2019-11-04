using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Otelim.Models;
namespace Mvc_Otelim.Controllers
{


    public class LoginController : Controller
    {

        OtelModel1 m = new OtelModel1();
        //
        // GET: /Login/

        public ActionResult Index(string url="")
        {
            ViewBag.url = url;
            if (Session["loginfail"] == null)
            {
                Session["loginfail"] = false;
            }
            return View();
        }
        public ActionResult Giris(string mail,string password,string url)
        {
            Session.RemoveAll();
            Kullanıcı k = m.Kullanıcı.Where(x => x.Mail == mail).FirstOrDefault();
            OtelYöneticisi y = m.OtelYöneticisi.Where(x => x.Mail == mail).FirstOrDefault();
            Admin a = m.Admin.Where(x => x.Mail == mail).FirstOrDefault();
            if (k!=null&&k.Sifre == password)
            {
                
                Session["name"] = k.Ad;
                Session["id"] = k.ID;
                if (url == "")
                {
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    return Redirect(url);
                }
            }else if (y!=null&&y.Şifre==password)
            {
                Session["name"] = y.Ad;
                Session["yid"] = y.ID;
                return RedirectToAction("../OtelYöneticisi/Index");

            }
            else if (a != null && a.Şifre == password)
            {
                Session["name"] = a.Ad;
                Session["aid"] = a.ID;
                return RedirectToAction("../Admin/Index");

            }
            else
            {
                Session["loginfail"] = true;
                return RedirectToAction("../Login/Index");
                
            }


        }
        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("../Home/Index");
        }
    }
}
