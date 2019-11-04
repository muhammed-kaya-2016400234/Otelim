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
    [UserAuthorize(Roles ="Kullanıcı")]
    public class KullanıcıController : Controller
    {
        //
        // GET: /Kullanıcı/
        OtelModel1 m = new OtelModel1();
        public ActionResult Index()
        {
            return View();
        }
        
       
        public ActionResult Profil()
        {
            int kid = Convert.ToInt32(Session["id"]);
            ViewBag.k = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();

            return View();
        }
        public ActionResult ProfilGüncelle(string Ad, string Soyad, string email, string password)
        {
            int kid = Convert.ToInt32(Session["id"]);
            Kullanıcı o = m.Kullanıcı.Where(x => x.ID == kid).FirstOrDefault();
            o.Ad = Ad;
            o.Soyad = Soyad;
            o.Mail = email;
            o.Sifre = password;
            m.SaveChanges();
            Session["name"] = Ad;

            return RedirectToAction("Profil","Kullanıcı");
        }
        
        public ActionResult SonGoruntulenen()
        {
            int id = Convert.ToInt32(Session["id"]);
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["OtelModel1"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(constr);
            constr = builder.ProviderConnectionString;
            ViewBag.print = constr;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string com = "select top(5)o.*,gr from Otel as o inner join (select OtelID as ot,GörüntülenmeTarihi as gr from SonGörüntülenen where KullanıcıID='" + id + "'  )as k on o.ID=k.ot order by gr desc";
                using (SqlCommand cmd = new SqlCommand(com))
                {
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }


            
            return View(ds);
        }
        //parametre ismi id olmazsa route'layamıyor.
        public ActionResult Rezervasyonlar()
        {
            int id = Convert.ToInt32(Session["id"]);
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["OtelModel1"].ConnectionString;
            var builder = new EntityConnectionStringBuilder(constr);
            constr = builder.ProviderConnectionString;
            ViewBag.print = constr;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string com = "select o.*,k.OtelAdı from (select * from Rezervasyon where KullanıcıID='"+id+"') as o inner join (select * from Otel   )as k on (o.OdaID in (select ID from (select * from Oda where ID=o.OdaID) as t where t.OtelID=k.ID ))";
                    
                    using (SqlCommand cmd = new SqlCommand(com))
                {
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }
            return View(ds);
        }

        [HttpPost]
        public void RezervasyonSil(int id,int kid=0)
        {

            if (Session["id"] != null || Session["yid"] != null)
            {
                m.Database.ExecuteSqlCommand("delete from Rezervasyon where rezID='" + id + "'");
                
                if (Session["yid"] != null) {
                    BildirimYolla(kid, "" + id + " ID'li rezervasyonunuz otel yöneticisi iptal edildi!");
                    
                }
            }
        }
        public void BildirimYolla(int kid, string text)
        {

            Bildirim b = new Bildirim();
            b.KullanıcıID = kid;
            b.text = text;
            b.tarih = DateTime.Now;
            m.Bildirim.Add(b);
            m.SaveChanges();

        }

        public ActionResult Favoriler()
        {
            int kid = Convert.ToInt32(Session["id"]);
            List<Otel> list = m.Database.SqlQuery<Otel>("select * from Otel where ID in (select OtelID from FavoriOtel where KullanıcıID=@id)",new SqlParameter("id",kid)).ToList();


            return View(list);
        }
        public ActionResult Bildirimler()
        {
            int kid = Convert.ToInt32(Session["id"]);
            List<Bildirim> list = m.Bildirim.Where(x => x.KullanıcıID == kid).ToList();
            return View(list);
        }
        public void BildirimSil(int id)
        {
            int kid = Convert.ToInt32(Session["id"]);
            m.Database.ExecuteSqlCommand("delete from Bildirim where KullanıcıID=@kid and ID=@id",new SqlParameter("kid",kid),new SqlParameter("id",id));

        }

    }
}
