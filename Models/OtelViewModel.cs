using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Mvc_Otelim.Models
{
    public class OtelViewModel
    {
       public Dictionary<Oda,string> odalar;
       public List<OtelFoto> fotolar;
        public DataTable yorumlar;
    }
}