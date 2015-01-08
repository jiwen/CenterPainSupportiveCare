using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CenterPainSupportiveCare.Models
{
    public class Patient
    {
        public Patient()
        { }
        public Patient(CenterPainSupportiveCareModels.Patient patient)
        {
            Id = patient.PatientId.ToString();
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            PatientName = string.Format("{0} {1}", FirstName, LastName);
            if (patient.Addresses != null && patient.Addresses.FirstOrDefault() != null)
            {
                var firstAddress = patient.Addresses.FirstOrDefault();
                Street = firstAddress.Street;
                State = firstAddress.State;
                City = firstAddress.City;
                ZipCode = firstAddress.Zip.ToString();
            }

            DateOfBirth = patient.DOB.Value.ToString("MM/dd/yyyy"); //string.Format(@"{0}/{1}/{2}",patient.DOB.Value.Month, patient.DOB.Value.Day, patient.DOB.Value.Year);
        }

        public string Id { get; set; }
        [Display(Name="First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zipcode is required")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{5}([\-]?\d{4})?$", ErrorMessage="Zip Code must be match pattern XXXXX OR XXXXX-XXXX and contain only numbers.")]
        public string ZipCode { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is required")]
        [RegularExpression(@"^(0[1-9]|1[012])[/.](0[1-9]|[12][0-9]|3[01])[/.](19|20)\d\d$",ErrorMessage="Required format (mm/dd/yyy)")]
        public string DateOfBirth { get;set; }
        public string PatientName { get; set; }
    }
}