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
    
    public partial class PatientLab
    {
        public PatientLab()
        {
            this.PatientLabResults = new HashSet<PatientLabResult>();
            this.Reviews = new HashSet<Review>();
        }
    
        public int PatientLabId { get; set; }
        public int PatientId { get; set; }
        public int LabTypeId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
    
        public virtual LabType LabType { get; set; }
        public virtual ICollection<PatientLabResult> PatientLabResults { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
