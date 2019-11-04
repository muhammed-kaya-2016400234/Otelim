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
namespace Mvc_Otelim.Controllers
{
    public class OtelController : Controller
    {

        OtelModel1 m = new OtelModel1();
        //
        // GET: /OtelSayfası/

        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult Görüntüle(int id,int kisiSayı,string giriş,string çıkış)
        
         {
            
          
            SonGörüntülenenEkle(id);
            ViewBag.girisTarih = giriş;
            ViewBag.cıkısTarih = çıkış;
            ViewBag.kisi = kisiSayı;
            ViewBag.tumOdalar = false;
            ViewBag.otel = m.Otel.Where(x => x.ID == id).FirstOrDefault();
            List<Oda> odalar = m.Database.SqlQuery<Oda>("select * from Oda as o where OtelID=" + id + " and Kapasite>=" + kisiSayı + " and not exists (select * from Rezervasyon as r where r.OdaID=o.ID and ((r.GirişTarih>'" + giriş + "'  and r.GirişTarih<'" + çıkış + "' ) or (r.ÇıkışTarih<'" + çıkış + "'  and r.ÇıkışTarih>'" + giriş + "' ) or (r.GirişTarih<='" + giriş + "' )and r.ÇıkışTarih>='" + çıkış + "' ) ) order by Kapasite asc").ToList<Oda>();
            var dict = new Dictionary<Oda, string>();
            foreach (Oda o in odalar)
            {
                int oid = o.ID;
                OdaFoto f = m.OdaFoto.Where(x => x.OdaID == oid).FirstOrDefault();
                if (f != null)
                {
                    dict[o] = f.Link;
                }
                else
                {
                    dict[o] = "/Content/assets/images/required/otelim_logo.png";
                }
            }
            OtelViewModel model = new OtelViewModel();
            model.odalar = dict;
            model.fotolar = m.OtelFoto.Where(x => x.OtelID == id).ToList();
            model.yorumlar = Yorumlar(id);

            int kid = Convert.ToInt32(Session["id"]);
            ViewBag.user = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();



            ViewBag.ps = m.OtelPuanlandırma.Where(x => x.OtelID == id).ToList().Count;



            return View(model);
        }
        //checks if user has the hotel with given id in his favourites
        public Boolean FavoriKontrol(int id)
        {
            if (Session["id"] != null)
            {
                int kid = Convert.ToInt32(Session["id"]);
                FavoriOtel f = m.FavoriOtel.Where(x => x.KullanıcıID == kid && x.OtelID == id).FirstOrDefault();
                if (f != null)
                {
                    return true;

                }
            }




            return false;
        }
        public ActionResult TumOdalar(int id)
        {
            Boolean fav = FavoriKontrol(id);
            ViewBag.fav = fav;

            int kid = Convert.ToInt32(Session["id"]);
            OtelPuanlandırma op = m.OtelPuanlandırma.Where(x => x.KullanıcıID == kid && x.OtelID == id).FirstOrDefault();
            if (op != null) {
                ViewBag.userrate =op .Puan;
            }
            else
            {
                ViewBag.userrate = 1;
            }

            SonGörüntülenenEkle(id);

            ViewBag.tumOdalar = true;
            ViewBag.otel = m.Otel.Where(x => x.ID == id).FirstOrDefault();
            List<Oda> odalar = m.Oda.Where(x => x.OtelID == id).ToList();
            var dict = new Dictionary<Oda, string>();
            foreach (Oda o in odalar)
            {
                int oid = o.ID;
                OdaFoto f = m.OdaFoto.Where(x => x.OdaID == oid).FirstOrDefault();
                if (f != null)
                {
                    dict[o] = f.Link;
                }
                else
                {
                    dict[o] = "/Content/assets/images/required/otelim_logo.png";
                }
            }
            OtelViewModel model = new OtelViewModel();
            model.odalar = dict;
            model.fotolar = m.OtelFoto.Where(x => x.OtelID == id).ToList();
            model.yorumlar = Yorumlar(id);
            
            ViewBag.user = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();


            ViewBag.ps = m.OtelPuanlandırma.Where(x => x.OtelID == id).ToList().Count;
            
           
                if(m.OtelPuanlandırma.Where(x => x.OtelID == id && x.KullanıcıID == kid).FirstOrDefault()==null)
            {
                ViewBag.pcheck = 0;
            }
            else
            {
                ViewBag.pcheck = 1;
            }

            return View("Görüntüle",model);
        }
        public DataTable Yorumlar(int id)
        {
            
            Otel ot= m.Otel.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.otel = ot;
            
            int kid = Convert.ToInt32(Session["id"]);
            ViewBag.kullanıcı = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();


            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["OtelModel1"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(constr);
            constr = builder.ProviderConnectionString;
            
            using (SqlConnection con = new SqlConnection(constr))
            {
                string com = "select oy.*,k.Ad as kAd,k.Soyad as kSoyad from (select * from OtelYorum where OtelID="+id+") as oy inner join Kullanıcı as k on oy.KullanıcıID=k.ID";
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
          //  return View("Yorumlar",ds);
        }

        public void YorumEkle(int userID,int oID,string comment)
        {
            OtelYorum o = new OtelYorum();
            o.KullanıcıID = userID;
            o.OtelID = oID;
            o.Yorum = comment;
            m.OtelYorum.Add(o);
            m.SaveChanges();

            //return RedirectToAction("/Otel/Yorumlar",new {id=oID});
        }
        public void YorumSil(int oid)
        {
            
            m.Database.ExecuteSqlCommand("delete from OtelYorum where ID=@oID", new SqlParameter("oID", oid));

        }

        public void SonGörüntülenenEkle(int OtelID)
        {
            if (Session["id"] != null)
            {
                int a = Convert.ToInt32(Session["id"]);
                SonGörüntülenen s = new SonGörüntülenen();
                s.KullanıcıID = a;
                s.OtelID = OtelID;
                s.GörüntülenmeTarihi = DateTime.Now;
                m.SonGörüntülenen.Add(s);
                m.SaveChanges();

                if (m.SonGörüntülenen.Where(x => x.KullanıcıID == a).ToList().Count > 5)
                {
                    m.Database.ExecuteSqlCommand("delete from SonGörüntülenen where KullanıcıID='" + a + "' and GörüntülenmeTarihi=(select min(GörüntülenmeTarihi) from SonGörüntülenen where KullanıcıID='" + a + "')");
                }

            }



        }

        public void FavorilereEkle(int id)
        {
            int kid = Convert.ToInt32(Session["id"]);
            Kullanıcı k = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();
            FavoriOtel f = new FavoriOtel();
            f.KullanıcıID = kid;
            f.OtelID = id;
            m.FavoriOtel.Add(f);
            m.SaveChanges();


        }

        public void FavorilerdenÇıkar(int id)
        {
            int kid = Convert.ToInt32(Session["id"]);
            Kullanıcı k = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();
            m.Database.ExecuteSqlCommand("delete from FavoriOtel where OtelID=@oid and KullanıcıID=@kid", new SqlParameter("oid", id), new SqlParameter("kid", kid));

            List<Oda> model = m.Oda.Where(x => x.OtelID == id).ToList();
            ViewBag.otel = m.Otel.Where(x => x.ID == id).FirstOrDefault();

        }

        public float? PuanGüncelle(int rate,int id)
        {
            int kid = Convert.ToInt32(Session["id"]);
            OtelPuanlandırma op = m.OtelPuanlandırma.Where(x => x.KullanıcıID == kid&&x.OtelID==id).FirstOrDefault();
           
            if (op == null)
            {
                OtelPuanlandırma newop = new OtelPuanlandırma();
                newop.KullanıcıID = kid;
                newop.OtelID = id;
                newop.Puan = rate;
                m.OtelPuanlandırma.Add(newop);

            }
            else
            {
                op.Puan = rate;
            }
            m.SaveChanges();

            int? sumrates = m.OtelPuanlandırma.Where(x => x.OtelID == id).Sum(x => x.Puan);
            int? count = m.OtelPuanlandırma.Where(x => x.OtelID == id).ToList().Count;
            float? newrate=(float)(sumrates * (1.0) / count);
            m.Otel.Where(x => x.ID == id).FirstOrDefault().Puan = newrate;
            m.SaveChanges();
            return newrate;
           

        }
      


    }
}
