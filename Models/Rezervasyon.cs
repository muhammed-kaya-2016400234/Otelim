//------------------------------------------------------------------------------
// <auto-generated>
//    Bu kod bir şablondan oluşturuldu.
//
//    Bu dosyada el ile yapılan değişiklikler uygulamanızda beklenmedik davranışa neden olabilir.
//    Kod yeniden oluşturulursa, bu dosyada el ile yapılan değişikliklerin üzerine yazılacak.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mvc_Otelim.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rezervasyon
    {
        public int rezID { get; set; }
        public int KullanıcıID { get; set; }
        public int OdaID { get; set; }
        public Nullable<System.DateTime> GirişTarih { get; set; }
        public Nullable<System.DateTime> ÇıkışTarih { get; set; }
    
        public virtual Kullanıcı Kullanıcı { get; set; }
        public virtual Kullanıcı Kullanıcı1 { get; set; }
        public virtual Oda Oda { get; set; }
        public virtual Oda Oda1 { get; set; }
    }
}
