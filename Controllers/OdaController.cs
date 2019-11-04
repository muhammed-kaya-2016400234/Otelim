using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Otelim.Models;
using System.Data;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
namespace Mvc_Otelim.Controllers
{
    public class OdaController : Controller
    {

        OtelModel1 m = new OtelModel1();
        //
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Görüntüle(int OdaID,int? kisi,string giriş="",string çıkış="")
        {
            ViewBag.girisTarih = giriş;
            ViewBag.cıkısTarih = çıkış;
            ViewBag.kisi = kisi;
            Oda k = m.Oda.Where(x => x.ID == OdaID).FirstOrDefault();
            ViewBag.otel = m.Otel.Where(x => x.ID == k.OtelID).FirstOrDefault();
            ViewBag.oda = k;
            List<OdaFoto> list = m.OdaFoto.Where(x => x.OdaID == OdaID).ToList();

            OdaViewModel model = new OdaViewModel();
            DataTable table = Yorumlar(OdaID);
            model.fotolar = list;
            model.yorumlar = table;
            int kid = Convert.ToInt32(Session["id"]);
            ViewBag.user = m.Kullanıcı.Where(x=>x.ID==kid).FirstOrDefault();

            return View(model);
        }
        public DataTable Yorumlar(int id)
        {
            Oda o = m.Oda.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.oda = o;
            int oid = o.OtelID;
            Otel ot = m.Otel.Where(x => x.ID == oid).FirstOrDefault();
            ViewBag.otel=ot;
            int kid = Convert.ToInt32(Session["id"]);
            ViewBag.kullanıcı = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();


            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["OtelModel1"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(constr);
            constr = builder.ProviderConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                string com = "select oy.*,k.Ad as kAd,k.Soyad as kSoyad from (select * from OdaYorum where OdaID=" + id + ") as oy inner join Kullanıcı as k on oy.KullanıcıID=k.ID";
                using (SqlCommand cmd = new SqlCommand(com))
                {
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }

            return ds.Tables[0];
            //Url.Action("Yorumlar", "Otel", new { id=ot.ID})
           // return View("Yorumlar", ds);
        }
        public void YorumEkle(int userID, int oID, string comment)
        {
            OdaYorum o = new OdaYorum();
            o.KullanıcıID = userID;
            o.OdaID = oID;
            o.Yorum = comment;
            m.OdaYorum.Add(o);
            m.SaveChanges();

            //return RedirectToAction("/Otel/Yorumlar",new {id=oID});
        }
        public void YorumSil(int oid)
        {

            m.Database.ExecuteSqlCommand("delete from OdaYorum where ID=@oID", new SqlParameter("oID", oid));

        }


        [HttpPost]
        public ActionResult RezervasyonEkle(int odaID,int KişiSayısı,string giris,string cıkıs)
            
        {
            if (giris == "" || cıkıs == "")
            {
                Session["tfail"] = "Tarihler Hatalı";
                return RedirectToAction("RezervasyonEkleForm","Oda",new {giris=giris,cıkıs=cıkıs,id=odaID,kisi=KişiSayısı });
            }
            else
            {

                DateTime a = Convert.ToDateTime(giris);
                giris = a.ToString("yyyy-MM-dd");
                DateTime b = Convert.ToDateTime(cıkıs);
                cıkıs = b.ToString("yyyy-MM-dd");

                if (b <= a)
                {
                    Session["tfail"] ="Tarihler Hatalı";
                    return RedirectToAction("RezervasyonEkleForm", "Oda", new { giris = giris, cıkıs = cıkıs, id = odaID, kisi = KişiSayısı });

                }
            }
            Session.Remove("tfail");


            int kulID = Convert.ToInt32(Session["id"]);
            DateTime giriş = Convert.ToDateTime(giris);
            DateTime çıkış = Convert.ToDateTime(cıkıs);
            string q= "select * from Rezervasyon as r where r.OdaID ="+odaID+" and((r.GirişTarih > '" + giris + "'  and r.GirişTarih < '" + cıkıs + "') or(r.ÇıkışTarih < '" + cıkıs + "'  and r.ÇıkışTarih > '" + giris + "') or(r.GirişTarih <= '" + giris + "')and r.ÇıkışTarih >= '" + cıkıs + "'  )";
            List<Rezervasyon> list = m.Database.SqlQuery<Rezervasyon>(q).ToList();
            if (list.Count==0)
            {
                
                Rezervasyon r = new Rezervasyon();
                r.GirişTarih = giriş;
                r.ÇıkışTarih = çıkış;
                r.OdaID =odaID;
                r.KullanıcıID = kulID;
                m.Rezervasyon.Add(r);
                m.SaveChanges();
                Session.Remove("tfail");
                Session["rez"] =1;
                return RedirectToAction("Rezervasyonlar","Kullanıcı");
            }
            else
            {
                Session["tfail"] = "Oda bu tarihlerde müsait değil!";
                return RedirectToAction("RezervasyonEkleForm", "Oda", new { giris = giris, cıkıs = cıkıs, id = odaID, kisi = KişiSayısı });

            }
           // return View();
        }

        public ActionResult RezervasyonEkleForm(int id,int? kisi , string giris="" ,string cıkıs="")
        {
            if (giris == "" || cıkıs == "")
            {
                ViewBag.gt = "";
                ViewBag.ct = "";

            }
            else
            {
                DateTime a = Convert.ToDateTime(giris);
                giris = a.ToString("dd-MM-yyyy");
                DateTime b = Convert.ToDateTime(cıkıs);
                cıkıs = b.ToString("dd-MM-yyyy");
                ViewBag.gt = giris;
                ViewBag.ct = cıkıs;
            }

            Oda o = m.Oda.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.kisiSayı = kisi;
            ViewBag.oda = o;
            int oid = o.OtelID;
            Otel k = m.Otel.Where(x => x.ID == oid).FirstOrDefault();
            ViewBag.oteladı=k.OtelAdı;

            return View();
        }

       



    }
}
