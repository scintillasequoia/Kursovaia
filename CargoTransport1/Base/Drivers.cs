//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CargoTransport1.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class Drivers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Drivers()
        {
            this.Deliveries = new HashSet<Deliveries>();
        }
    
        public int Dr_ID { get; set; }
        public string Dr_F { get; set; }
        public string Dr_I { get; set; }
        public string Dr_O { get; set; }
        public string Dr_Adress { get; set; }
        public string Dr_Phone { get; set; }
        public string Dr_License { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deliveries> Deliveries { get; set; }
    }
}
