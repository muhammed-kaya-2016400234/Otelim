using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Otelim.Models;
using System.IO;
namespace Mvc_Otelim.Controllers
{
    [UserAuthorize(Roles ="otel_yönetici")]
    public class OtelYöneticisiController : Controller
    {
        //
        // GET: /Kullanıcı/
        OtelModel1 m = new OtelModel1();
       
        public ActionResult Index()
        {
            /*
            int id = Convert.ToInt32(Session["yid"]);
            List<Oda> list = m.Database.SqlQuery<Oda>("select * from Oda where otelID in (select ID from Otel where YöneticiID=@yid )", new SqlParameter("yid", id)).ToList();

            Otel ot = m.Otel.Where(x => x.YöneticiID == id).FirstOrDefault();
            int otelid = ot.ID;
            ot.OdaSayısı = m.Oda.Where(x => x.OtelID == otelid).ToList().Count();
            ViewBag.otel = ot;
            m.SaveChanges();
            */
            int yid = Convert.ToInt32(Session["yid"]);
            Otel otel = m.Otel.Where(x => x.YöneticiID == yid).FirstOrDefault();
            ViewBag.otel = otel;
            return View();

        }
        public ActionResult Odalar()
        {
            int id = Convert.ToInt32(Session["yid"]);
            List<Oda> list = m.Database.SqlQuery<Oda>("select * from Oda where otelID in (select ID from Otel where YöneticiID=@yid )", new SqlParameter("yid", id)).ToList();
                

            return View(list);

        }
        public ActionResult Fotolar()
        {
            int id = Convert.ToInt32(Session["yid"]);
            Otel o = m.Otel.Where(x => x.YöneticiID == id).FirstOrDefault();
            int oid = o.ID;
            ViewBag.otel = o;
            List<OtelFoto> list = m.OtelFoto.Where(x => x.OtelID == oid).ToList();
            return View(list);
        }
        public ActionResult Rezervasyonlar()
        {
            int id = Convert.ToInt32(Session["yid"]);
            OtelYöneticisi y=m.OtelYöneticisi.Where(x=>x.ID==id).FirstOrDefault();
            string q = "select * from Rezervasyon where OdaID in (select ID from Oda where OtelID=(select OtelID from OtelYöneticisi where ID=@yid)) and ÇıkışTarih< GETDATE()";
            List<Rezervasyon> l1 = m.Database.SqlQuery<Rezervasyon>(q,new SqlParameter("yid",id)).ToList();
            string q2 = "select * from Rezervasyon where OdaID in (select ID from Oda where OtelID=(select OtelID from OtelYöneticisi where ID=@yid)) and ÇıkışTarih> GETDATE()";
            List<Rezervasyon> l2 = m.Database.SqlQuery<Rezervasyon>(q2, new SqlParameter("yid", id)).ToList();
            RezervasyonViewModel model = new RezervasyonViewModel();
            model.past = l1;
            model.future = l2;
            
            return View(model);
        }
        
        public void OtelFotoSil(int id)
        {
            m.Database.ExecuteSqlCommand("delete from OtelFoto where ID=@fotoID", new SqlParameter("fotoID", id));

        }
        public void OdaFotoSil(int id)
        {
            m.Database.ExecuteSqlCommand("delete from OdaFoto where ID=@fotoID", new SqlParameter("fotoID", id));

        }

        public void OdaSil(int id)
        {

            m.Database.ExecuteSqlCommand("delete from Oda where ID=@odaID",new SqlParameter("odaID",id));


        }
        public ActionResult OdaEkle(Oda o)
        {
            
            int yid = Convert.ToInt32(Session["yid"]);

            Otel ot = m.Otel.Where(x => x.YöneticiID == yid).FirstOrDefault();
            ot.OdaSayısı++;
            o.OtelID =ot.ID;

            m.Oda.Add(o);
            m.SaveChanges();
            //string b = Url.Action("OtelGüncelleForm", "OtelYöneticisi", new { id = ot.ID });

            return RedirectToAction("Odalar","OtelYöneticisi");
        }
        public ActionResult OdaEkleForm()
        {

            return View();
        }
        
        public ActionResult OdaGüncelle(Oda o)
        {

            Oda k = m.Oda.Where(x => x.ID == o.ID).FirstOrDefault();
            k.name = o.name;
            k.OdaTürü = o.OdaTürü;
            k.Kapasite = o.Kapasite;
            m.SaveChanges();
            return RedirectToAction("OdaGüncelleForm", "OtelYöneticisi", new {o.ID });
        }
        public ActionResult OdaGüncelleForm(int id)
        {
            Oda oda = m.Oda.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.oda = oda;
            List<OdaFoto> list = m.OdaFoto.Where(x => x.OdaID == id).ToList();

            return View(list);

        }
        public ActionResult OtelGüncelle(Otel o)
        {

            Otel k = m.Otel.Where(x => x.ID == o.ID).FirstOrDefault();
            k.OtelAdı = o.OtelAdı;
            k.OtelTürüID = o.OtelTürüID;
            k.YıldızSayısı = o.YıldızSayısı;
            m.SaveChanges();
            return RedirectToAction("OtelGüncelleForm", "OtelYöneticisi");
        }
        public ActionResult YöneticiGüncelle(string Ad,string Soyad,string email,string password)
        {
            int yid = Convert.ToInt32(Session["yid"]);
            OtelYöneticisi o = m.OtelYöneticisi.Where(x => x.ID == yid).FirstOrDefault();
            o.Ad = Ad;
            o.Soyad = Soyad;
            o.Mail = email;
            o.Şifre = password;
            Session["name"] = Ad;
            m.SaveChanges();
            return RedirectToAction("OtelGüncelleForm", "OtelYöneticisi");
        }
        
        public ActionResult OtelGüncelleForm()
        {
            int yid = Convert.ToInt32(Session["yid"]);
            OtelYöneticisi o = m.OtelYöneticisi.Where(x => x.ID == yid).FirstOrDefault();
            ViewBag.oy = o;
            Otel otel = m.Otel.Where(x => x.YöneticiID == yid).FirstOrDefault();
            return View(otel);

        }

        public ActionResult ResimYükle(HttpPostedFileBase file,int oid,string otel_oda)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path;
                if (otel_oda == "oda")
                {
                    path = System.IO.Path.Combine(
                                          Server.MapPath("~/Content/assets/images/required/Fotolar/OdaFotolar"), pic);
                    file.SaveAs(path);
                    OdaFoto o = new OdaFoto();
                    o.OdaID = oid;
                    o.Link = "/Content/assets/images/required/Fotolar/OdaFotolar/" + pic;
                    m.OdaFoto.Add(o);
                    m.SaveChanges();
                }
                else
                {
                    path = System.IO.Path.Combine(
                                         Server.MapPath("~/Content/assets/images/required/Fotolar/OtelFotolar"), pic);

                    file.SaveAs(path);
                    OtelFoto o = new OtelFoto();
                    o.OtelID = oid;
                    o.Link = "/Content/assets/images/required/Fotolar/OtelFotolar/" + pic;
                    m.OtelFoto.Add(o);
                    m.SaveChanges();
                
                }
                
                // file is uploaded
               

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            if (otel_oda == "oda")
            {
                return RedirectToAction("OdaGüncelleForm", "OtelYöneticisi", new { id = oid });
            }
            else
            {
                return RedirectToAction("Fotolar", "OtelYöneticisi");

            }
        }

       

    }
}
