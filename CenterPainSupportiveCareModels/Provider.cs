//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CenterPainSupportiveCareModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Provider
    {
        public Provider()
        {
            this.Prescriptions = new HashSet<Prescription>();
        }
    
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public Nullable<int> StatusId { get; set; }
    
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual Status Status { get; set; }
    }
}
