using CenterPainSupportiveCare.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CenterPainSupportiveCare.Models
{
    public class Prescription
    {
        public Prescription()
        {

        }

        public Prescription(CenterPainSupportiveCareModels.Prescription prescription)
        {
            Id = prescription.PrescriptionId.ToString();
            Patient = new Patient(prescription.Patient);
            DateFilled = prescription.DateFilled.Value.ToString("MM/dd/yyyy");
            RxNumber = prescription.Rx;
            ProviderId = prescription.ProviderId;
            Provider = new Provider(prescription.Provider);
            AppointmentDate = prescription.AppointmentDate.Value.ToString("MM/dd/yyyy");

            if(prescription.Syringes.Any())
            {
                Syringes = new List<Syringe>();
                Syringes.AddRange(prescription.Syringes.Select(s => new Syringe(s)));
            }
            Quantity = prescription.Quantity;
        }

        public string Id { get; set; }        
        public Patient Patient { get; set; }
        [Display(Name = "Date Filled")]
        [RegularExpression(@"^(0[1-9]|1[012])[/.](0[1-9]|[12][0-9]|3[01])[/.](19|20)\d\d$")]
        [Required(ErrorMessage = "Date Filled is required")]
        public string DateFilled { get; set; }
        [Required(ErrorMessage = "Rx number is required")]
        public string RxNumber { get; set; }
        [Required(ErrorMessage = "Provider is required")]
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
        public List<Syringe> Syringes { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public string Quantity { get; set; }
        [Display(Name = "Appointment Date")]
        [RegularExpression(@"^(0[1-9]|1[012])[/.](0[1-9]|[12][0-9]|3[01])[/.](19|20)\d\d$")]
        [Required(ErrorMessage = "Appointment date is required")]
        public string AppointmentDate { get; set; }
        public string StatusId { get; set; }
    }
}