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
    
    public partial class PatientHistory
    {
        public int PatientHistoryId { get; set; }
        public int PatientId { get; set; }
        public int HistoryTypeId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
    
        public virtual Patient Patient { get; set; }
        public virtual Patient Patient1 { get; set; }
    }
}
