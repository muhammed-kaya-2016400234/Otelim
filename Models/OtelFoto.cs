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
    
    public partial class OtelFoto
    {
        public int ID { get; set; }
        public int OtelID { get; set; }
        public string Link { get; set; }
    
        public virtual Otel Otel { get; set; }
        public virtual Otel Otel1 { get; set; }
    }
}
