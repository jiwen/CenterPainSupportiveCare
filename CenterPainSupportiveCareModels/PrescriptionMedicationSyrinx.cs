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
    
    public partial class PrescriptionMedicationSyrinx
    {
        public int PrescriptionMedicationSyringeId { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicationId { get; set; }
        public int SyringeId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Unit { get; set; }
    
        public virtual Medication Medication { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual Syrinx Syrinx { get; set; }
    }
}
