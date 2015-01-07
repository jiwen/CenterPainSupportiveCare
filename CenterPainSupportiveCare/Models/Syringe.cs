using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CenterPainSupportiveCare.Models
{
    public class Syringe
    {
        public Syringe()
        {

        }

        public Syringe(CenterPainSupportiveCareModels.Syrinx syringe)
        {
            Id = syringe.SyringeId.ToString();

        }

        public string Id { get; set; }
        public List<Medication> Medications { get; set; }
    }
}