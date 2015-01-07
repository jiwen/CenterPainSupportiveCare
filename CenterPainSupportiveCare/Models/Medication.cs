using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CenterPainSupportiveCare.Models
{
    public class Medication
    {
        public Medication()
        { }

        public Medication(CenterPainSupportiveCareModels.Medication medication)
        {
            Id = medication.MedicationId.ToString();
            Measure = medication.Measure;
            MaximumVolumeLimit = medication.MaximumVolumeLimit.ToString();
            MinimumVolumeLimit = medication.MinimumVolumeLimit.ToString();
            Price = medication.Price;
            Unit = medication.Unit;
            Volume = medication.Volume;
            MedicationName = medication.MedicationName;

            if(medication.PrescriptionMedicationSyringes.FirstOrDefault() != null)
            {
                PrescriptionQuantity = medication.PrescriptionMedicationSyringes.FirstOrDefault().Quantity;
                PrescriptionUnit = medication.PrescriptionMedicationSyringes.FirstOrDefault().Unit;
            }
           
        }

        public Medication(CenterPainSupportiveCareModels.Medication medication, int prescriptionQuantity, string prescriptionUnit)
        {
            Id = medication.MedicationId.ToString();
            Measure = medication.Measure;
            MaximumVolumeLimit = medication.MaximumVolumeLimit.ToString();
            MinimumVolumeLimit = medication.MinimumVolumeLimit.ToString();
            Price = medication.Price;
            Unit = medication.Unit;
            Volume = medication.Volume;
            MedicationName = medication.MedicationName;

            PrescriptionQuantity = prescriptionQuantity;
            PrescriptionUnit = prescriptionUnit;
            

        }

        [HiddenInput]
        public string Id { get; set; }
        [HiddenInput]
        public string Measure { get; set; }
        [HiddenInput]
        public string MaximumVolumeLimit { get; set; }
        [HiddenInput]
        public string MinimumVolumeLimit { get; set; }
        [HiddenInput]
        public string Price { get; set; }
        [HiddenInput]
        public string Unit { get; set; }
        [HiddenInput]
        public string Volume { get; set; }
        [HiddenInput]
        public string MedicationName { get; set; }
        [HiddenInput]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Quantity must be a natural number")]
        public int? PrescriptionQuantity { get; set; }
        [HiddenInput]
        public string PrescriptionUnit { get; set; }
    }
}