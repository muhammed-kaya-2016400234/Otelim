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
    
    public partial class FavoriOtel
    {
        public int ID { get; set; }
        public int KullanıcıID { get; set; }
        public int OtelID { get; set; }
    
        public virtual Kullanıcı Kullanıcı { get; set; }
        public virtual Otel Otel { get; set; }
        public virtual Kullanıcı Kullanıcı1 { get; set; }
        public virtual Otel Otel1 { get; set; }
    }
}
