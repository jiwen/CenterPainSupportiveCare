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
    
    public partial class LabValueType
    {
        public LabValueType()
        {
            this.PatientLabResults = new HashSet<PatientLabResult>();
        }
    
        public int LabValueTypeId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<PatientLabResult> PatientLabResults { get; set; }
    }
}
