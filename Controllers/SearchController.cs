using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc_Otelim.Models;
namespace Mvc_Otelim.Controllers
{


    public class SearchController : Controller
    {
        // GET: Search

        OtelModel1 m = new OtelModel1();

        public ActionResult Index()
        {

            List<Otel> k = m.Otel.ToList();
            return View(k);
        }
        public ActionResult Arama(string arama,string şehir,int KişiSayısı,string giriş,string çıkış)
        {
            
            if (giriş==""||çıkış=="")
            {
                Session["fail"] = true;
                return RedirectToAction("../Home/Index");
            }
            else { 
           
            DateTime a=Convert.ToDateTime(giriş);
            giriş = a.ToString("yyyy-MM-dd");
            DateTime b = Convert.ToDateTime(çıkış);
            çıkış = b.ToString("yyyy-MM-dd");

                if (b <= a)
                {
                    Session["fail"] = true;
                    return RedirectToAction("../Home/Index");

                }
            }
            Session.Remove("fail");
            ViewBag.giriş = giriş;
            ViewBag.çıkış = çıkış;
            ViewBag.kisiSayısı = KişiSayısı;
            ViewBag.Sehir = şehir;

            // var query1 = m.Database.SqlQuery<Oda>("select * from Oda as o where o.OtelID in(select ID from Otel where Şehir='"+şehir+"') and o.Kapasite>="+KişiSayısı+" and not exists (select * from Rezervasyon as r where r.OdaID=o.ID and ((r.GirişTarih>'"+giriş+"'  and r.GirişTarih<'"+çıkış+"' ) or (r.ÇıkışTarih<'"+çıkış+"'  and r.ÇıkışTarih>'"+giriş+"' ) or (r.GirişTarih<='"+giriş+"' )and r.ÇıkışTarih>='"+çıkış+"' ) )").ToList();
            //Dictionary<int, List<Oda>> a = query1.GroupBy(c => c.OtelID).ToDictionary(t=>t.Key,t=>t.ToList());


            //List<Oda> o = m.Oda.Where(x => x.Kapasite >= KişiSayısı).ToList();
            //List<Otel> k = m.Otel.Where(x =>  x.Şehir == şehir).ToList();
            List<Otel> query = new List<Otel>();

            if (arama == "şehir"||arama==null)
            {
                string q = "select * from Otel where Otel.Şehir='" + şehir + "' and Otel.ID in (select OtelID from (select * from Oda as o where o.Kapasite>=" + KişiSayısı + " and not exists (select * from Rezervasyon as r where r.OdaID=o.ID and ((r.GirişTarih>'" + giriş + "'  and r.GirişTarih<'" + çıkış + "' ) or (r.ÇıkışTarih<'" + çıkış + "'  and r.ÇıkışTarih>'" + giriş + "' ) or (r.GirişTarih<='" + giriş + "' )and r.ÇıkışTarih>='" + çıkış + "' ) )) as o2)";
                query = m.Database.SqlQuery<Otel>(q).ToList();
            }
            else
            {

                
                    string q2 = "select * from Otel where Otel.OtelAdı like '" + şehir + "%' and Otel.ID in (select OtelID from (select * from Oda as o where o.Kapasite>=" + KişiSayısı + " and not exists (select * from Rezervasyon as r where r.OdaID=o.ID and ((r.GirişTarih>'" + giriş + "'  and r.GirişTarih<'" + çıkış + "' ) or (r.ÇıkışTarih<'" + çıkış + "'  and r.ÇıkışTarih>'" + giriş + "' ) or (r.GirişTarih<='" + giriş + "' )and r.ÇıkışTarih>='" + çıkış + "' ) )) as o2)";
                    query = m.Database.SqlQuery<Otel>(q2).ToList();
                   // query.AddRange(query2);

               
            }
            

            var dict = new Dictionary<Otel, string>();
            foreach(Otel o in query)
            {
                int id = o.ID;
                OtelFoto f=m.OtelFoto.Where(x => x.OtelID == id).FirstOrDefault();
                if (f != null) {
                    dict[o] = f.Link;
                }
                else
                {
                    dict[o] = "/Content/assets/images/required/otelim_logo.png";
                }
            }

            return View(dict);
        }
    }
}