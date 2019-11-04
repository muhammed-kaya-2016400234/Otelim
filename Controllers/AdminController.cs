using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Mvc_Otelim.Models;
namespace Mvc_Otelim.Controllers
{
    
    [UserAuthorize(Roles="admin")]
    public class AdminController : Controller
    {
       
        OtelModel1 m = new OtelModel1();


    

        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["aid"]);
            ViewBag.admin = m.Admin.Where(x => x.ID == id).FirstOrDefault();
            return View();
        }
        public ActionResult Profil()
        {
            int kid = Convert.ToInt32(Session["aid"]);
            ViewBag.k = m.Admin.Where(x => x.ID == kid).FirstOrDefault();

            return View();
        }
        public ActionResult ProfilGüncelle(string Ad, string Soyad, string email, string password)
        {
            int kid = Convert.ToInt32(Session["aid"]);
            Admin o = m.Admin.Where(x => x.ID == kid).FirstOrDefault();
            o.Ad = Ad;
            o.Soyad = Soyad;
            o.Mail = email;
            o.Şifre = password;
            m.SaveChanges();
            Session["name"] = Ad;

            return RedirectToAction("Profil", "Admin");
        }

       
        public ActionResult KullanıcılarıGörüntüle()
        {
            List<Kullanıcı> list = m.Kullanıcı.ToList();

            return View(list);
        }
        public int KullanıcıSil(int id)
        {
            int res=m.Database.ExecuteSqlCommand("delete from Kullanıcı where ID=@id",new SqlParameter("id",id));
            return res;

        }

        public ActionResult OtelleriGörüntüle()
        {
            List<Otel> list = m.Otel.ToList();

            return View(list);
        }
        public int OtelSil(int id)
        {
            int res = m.Database.ExecuteSqlCommand("delete from Otel where ID=@id", new SqlParameter("id", id));
            return res;

        }

        public ActionResult KullanıcıEkle()
        {


            return RedirectToAction("../Admin/KullanıcılarıGörüntüle");
        }
        public ActionResult KullanıcıEkleForm()
        {

            return View();
        }


        public ActionResult OtelEkle(string name,int htype,int stars,string city,string YMail,string ad,string soyad)
        {
            Otel o = new Otel();
            o.OtelAdı = name;
            o.OtelTürüID = htype;
            o.YıldızSayısı = stars;
            o.Şehir = city;
            m.Otel.Add(o);
            m.SaveChanges();
            OtelYöneticisi y = new OtelYöneticisi();
            y.Mail = YMail;
            y.Ad = ad;
            y.Soyad= soyad;
            y.OtelID = o.ID;
            m.OtelYöneticisi.Add(y);
            m.SaveChanges();
            o.YöneticiID = y.ID;
            m.SaveChanges();
            return RedirectToAction("../Admin/OtelleriGörüntüle");
        }
        public ActionResult OtelEkleForm()
        {

            return View();
        }

        public ActionResult OtelGüncelle(int otelid,int? yid,string name, int htype, int stars, string city, string YMail, string ad, string soyad)
        {
            Otel o = m.Otel.Where(x => x.ID == otelid).FirstOrDefault();
            
            o.OtelAdı = name;
            o.OtelTürüID = htype;
            o.YıldızSayısı = stars;
            o.Şehir = city;
            OtelYöneticisi y = new OtelYöneticisi();
            if (yid != null)
            {
                y = m.OtelYöneticisi.Where(x => x.ID == yid).FirstOrDefault();
                if (y != null)
                {
                    y.Mail = YMail;
                    y.Ad = ad;
                    y.Soyad = soyad;
                }
                
            }
            m.SaveChanges();

            return RedirectToAction("../Admin/OtelleriGörüntüle");
        }
        public ActionResult OtelGüncelleForm(int otelID)
        {
           
                Otel otel = m.Otel.Where(x => x.ID == otelID).FirstOrDefault();
                int? yid = otel.YöneticiID;

                OtelYöneticisi y = m.OtelYöneticisi.Where(x => x.ID == yid).FirstOrDefault();
                ViewBag.yonetici = y;
                ViewBag.otel = otel;
                return View();
           
        }

    }
}
