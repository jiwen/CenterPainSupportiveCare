using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CenterPainSupportiveCareModels;
using Newtonsoft.Json;

namespace CenterPainSupportiveCare.Controllers
{
    [Authorize]
    public class PrescriptionsController : Controller
    {
        private Entities db = new Entities();

        // GET: Prescriptions
        public async Task<ActionResult> Index()
        {
            try
            {
                var prescriptions = db.Prescriptions.Include(p => p.Patient).Include(p => p.Provider).Where(p => p.StatusId == (int)Models.Status.Statuses.Active);
                return View(await prescriptions.ToListAsync());
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "Index()");
            }

            return View();
        }

        // GET: Prescriptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Prescription prescription = await db.Prescriptions.FindAsync(id);
                if (prescription == null)
                {
                    return HttpNotFound();
                }
                return View(prescription);
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "Details()");
            }

            return View(new Prescription());
        }

        // GET: Prescriptions/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            try
            {
                ViewBag.ProviderId =  GetProviderList();
                ViewBag.VolumeChoices = GetVolumeList();
                ViewBag.Units = GetUnitList();
                ViewBag.Medications = GetMedicationList();
                ViewBag.States = GetStatesList();
                return View();
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "Create()");
                return View();
            }
        }

        // POST: Prescriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PrescriptionId,DateFilled,RxNumber,Quantity,PatientId,ProviderId,Patient,AppointmentDate, Syringes")] CenterPainSupportiveCare.Models.Prescription prescription)
        {
            try
            {

                ViewBag.ProviderId = GetProviderList();
                ViewBag.VolumeChoices = GetVolumeList();
                ViewBag.Units = GetUnitList();
                ViewBag.Medications = GetMedicationList();
                ViewBag.States = GetStatesList();

                if (ModelState.IsValid)
                {
                    if (prescription.Patient.Id == null)
                    {
                        //check if there is a patient in db
                        var firstName = prescription.Patient.FirstName.ToLower();// prescription.Patient.PatientName.Split(' ')[0];
                        var lastName = prescription.Patient.LastName.ToLower();// prescription.Patient.PatientName.Split(' ')[1];
                        var dateOfBirth = Convert.ToDateTime(prescription.Patient.DateOfBirth);

                        var patientDb = db.Patients.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName);
                        
                        if ((patientDb!= null && patientDb.DOB.Value.ToString("MM/dd/yyyy") != dateOfBirth.ToString("MM/dd/yyyy")) || patientDb == null)
                        {
                            var patient = new CenterPainSupportiveCareModels.Patient()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                DOB = Convert.ToDateTime(prescription.Patient.DateOfBirth),
                                DateCreated = DateTime.UtcNow,
                                Addresses = new List<Address>(){
                                new Address(){
                                    Street = prescription.Patient.Street,
                                    State = prescription.Patient.State,
                                    City = prescription.Patient.City,
                                    Zip = Convert.ToInt32(prescription.Patient.ZipCode),
                                   
                                }
                            }
                            };

                            db.Patients.Add(patient);
                            await db.SaveChangesAsync();
                            prescription.Patient.Id = patient.PatientId.ToString();
                        }
                        else
                        {
                            prescription.Patient.Id = patientDb.PatientId.ToString();
                        }
                    }

                    var dbPrescription = new CenterPainSupportiveCareModels.Prescription()
                    {
                        DateCreated = DateTime.UtcNow,
                        DateFilled = Convert.ToDateTime(prescription.DateFilled),
                        DateModified = DateTime.UtcNow,
                        PatientId = Convert.ToInt32(prescription.Patient.Id),
                        ProviderId = Convert.ToInt32(prescription.ProviderId),
                        Quantity = prescription.Quantity,
                        Rx = prescription.RxNumber,
                        AppointmentDate = Convert.ToDateTime(prescription.AppointmentDate),
                        StatusId = (int)Models.Status.Statuses.Active
                    };

                    db.Prescriptions.Add(dbPrescription);
                    await db.SaveChangesAsync();

                    var dbSyringe = new CenterPainSupportiveCareModels.Syrinx()
                    {
                        PrescriptionId = Convert.ToInt32(dbPrescription.PrescriptionId),
                    };
                    db.Syringes.Add(dbSyringe);
                    await db.SaveChangesAsync();

                    foreach (var medication in prescription.Syringes.FirstOrDefault().Medications)
                    {
                        var dbPrescriptionMedicationSyringe = new CenterPainSupportiveCareModels.PrescriptionMedicationSyrinx()
                        {

                            Quantity = medication.PrescriptionQuantity,
                            MedicationId = Convert.ToInt32(medication.Id),
                            PrescriptionId = Convert.ToInt32(dbPrescription.PrescriptionId),
                            Unit = medication.PrescriptionUnit,
                            SyringeId = dbSyringe.SyringeId
                        };

                        db.PrescriptionMedicationSyringes.Add(dbPrescriptionMedicationSyringe);
                    }

                    await db.SaveChangesAsync();
                    ModelState.Clear();
                    ViewBag.Message = "Successfully created prescription.";
                    return View(prescription);
                }

                if (prescription != null && prescription.Syringes != null)
                {
                    for (int i = 0; i < prescription.Syringes.FirstOrDefault().Medications.Count(); i++)
                    {
                        var medication = prescription.Syringes.FirstOrDefault().Medications[i];
                        var medicationModel = await db.Medications.FindAsync(Convert.ToInt32(medication.Id));

                        medication = new CenterPainSupportiveCare.Models.Medication(medicationModel, Convert.ToInt32(medication.PrescriptionQuantity), medication.PrescriptionUnit);
                        prescription.Syringes.FirstOrDefault().Medications[i] = medication;
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Errors = HelperController.ErrorMessage(ex, "Create()");                
            }

            return View(prescription);
        }

        // GET: Prescriptions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Prescription prescription = await db.Prescriptions.FindAsync(id);
                if (prescription == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", prescription.PatientId);
                ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "ProviderName", prescription.ProviderId);
                return View(prescription);
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "Edit()");
            }

            return View(new Prescription());
        }

        // POST: Prescriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PrescriptionId,DateFilled,Rx,Quantity,DateCreated,DateModified,PatientId,ProviderId")] Prescription prescription)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(prescription).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", prescription.PatientId);
                ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "ProviderName", prescription.ProviderId);
                return View(prescription);
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "Edit()");
            }

            return View(new Prescription());
        }

        // GET: Prescriptions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Prescription prescription = await db.Prescriptions.FindAsync(id);
                if (prescription == null)
                {
                    return HttpNotFound();
                }
                return View(prescription);
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "Delete()");
            }

            return View(new Prescription());
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Prescription prescription = await db.Prescriptions.FindAsync(id);
                prescription.StatusId = (int)Models.Status.Statuses.Deleted;
                db.Entry(prescription).State = EntityState.Modified;
                await db.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "DeleteConfirmed()");
            }

            return RedirectToAction("Index");
            
        }

        [Authorize]
        [HttpPost]
        public async Task<string> PatientSearch(CenterPainSupportiveCare.Models.Prescription prescription)
        {
            try
            {
                if (prescription != null && prescription.Patient != null && !string.IsNullOrEmpty(prescription.Patient.LastName) && !string.IsNullOrEmpty(prescription.Patient.FirstName) && !string.IsNullOrEmpty(prescription.Patient.DateOfBirth))
                {
                    //var patientName = prescription.Patient.PatientName;
                    var lastName = prescription.Patient.LastName.ToLower();
                    var firstName = prescription.Patient.FirstName.ToLower();
                    var dob = Convert.ToDateTime(prescription.Patient.DateOfBirth);
                    var dbPatient = await db.Patients.FirstOrDefaultAsync(p => p.FirstName.Contains(firstName) && p.LastName.Contains(lastName));

                    if (dbPatient != null && dbPatient.DOB.Value.ToString("MM/dd/yyyy") == dob.ToString("MM/dd/yyyy"))
                    {
                        var pt = JsonConvert.SerializeObject(new CenterPainSupportiveCare.Models.Patient(dbPatient));

                        return pt;
                    }
                }
                
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "PatientSearch()");
            }

            return string.Empty;
        }

        [Authorize]
        [HttpPost]
        public async Task<string> MedicationSearch(CenterPainSupportiveCare.Models.Medication medication)
        {
            try
            {
                if (medication != null)
                {
                    var medicationModel = await db.Medications.FirstOrDefaultAsync(m => m.MedicationName == medication.MedicationName
                    && m.MinimumVolumeLimit <= medication.PrescriptionQuantity
                    && m.MaximumVolumeLimit >= medication.PrescriptionQuantity
                    && m.Volume == medication.Volume
                    && m.Unit == medication.PrescriptionUnit);
                    if (medicationModel != null)
                    {
                        var pt = JsonConvert.SerializeObject(new CenterPainSupportiveCare.Models.Medication(medicationModel,Convert.ToInt32(medication.PrescriptionQuantity),medication.PrescriptionUnit));

                        return pt;
                    }
                }

            }
            catch (Exception ex)
            {
                HelperController.ErrorMessage(ex, "MedicationSearch()");
            }

            return string.Empty;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Helpers
        public SelectList GetVolumeList()
        {
            var choices = new SelectList(
               new List<SelectListItem>
                {
                    new SelectListItem { Selected = false, Text = "20ml", Value = "20ml"},
                    new SelectListItem { Selected = false, Text = "40ml", Value = "40ml"},
                }, "Value", "Text", 1);

            return choices;
        }

        public SelectList GetVolumeList(string quantity)
        {
            var choices = new SelectList(
               new List<SelectListItem>()
                {
                    new SelectListItem { Selected = false, Text = "20ml", Value = "20ml"},                   
                    new SelectListItem { Selected = false, Text = "40ml", Value = "40ml"},
                }, "Value", "Text", 1);

            if (quantity == "20ml")
            {
                choices.ElementAt(0).Selected = true;
            }
            else
            {
                choices.ElementAt(1).Selected = true;
            }
            
            return choices;
        }

        public SelectList GetUnitList()
        {
            var units = new SelectList(
                 new List<SelectListItem>
                {
                    new SelectListItem { Selected = false, Text = "mcg/ml", Value = "mcg/ml"},
                    new SelectListItem { Selected = false, Text = "mg/ml", Value = "mg/ml"},
                }, "Value", "Text", 1);

            return units;
        }

        public SelectList GetProviderList()
        {
            try
            {
                return new SelectList(db.Providers, "ProviderId", "ProviderName");
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "GetProviderList()");
            }

            return new SelectList(new List<CenterPainSupportiveCareModels.Provider>(), "","");
        }

        public List<SelectListItem> GetMedicationList()
        {
            try
            {
                var list = new List<SelectListItem>();
                var medications = db.Medications.GroupBy(m => m.MedicationName).ToList();
                list.AddRange(medications.Select(m => new SelectListItem() { Text = m.Key, Value = m.Key }));
                return list;
            }
            catch(Exception ex)
            {
                HelperController.ErrorMessage(ex, "GetMedicationList()");
            }

            return new List<SelectListItem>();
        }

        public SelectList GetStatesList()
        {
            var states = new SelectList(
                 new List<SelectListItem>
                {
                    new SelectListItem { Selected = false, Text = "AL", Value = "AL"},
                    new SelectListItem { Selected = false, Text = "AK", Value = "AK"},
                    new SelectListItem { Selected = true, Text = "AZ", Value = "AZ"},
                    new SelectListItem { Selected = false, Text = "AR", Value = "AR"},
                    new SelectListItem { Selected = false, Text = "CA", Value = "CA"},
                    new SelectListItem { Selected = false, Text = "CO", Value = "CO"},
                    new SelectListItem { Selected = false, Text = "CT", Value = "CT"},
                    new SelectListItem { Selected = false, Text = "DE", Value = "DE"},
                    new SelectListItem { Selected = false, Text = "FL", Value = "FL"},
                    new SelectListItem { Selected = false, Text = "GA", Value = "GA"},
                    new SelectListItem { Selected = false, Text = "HI", Value = "HI"},
                    new SelectListItem { Selected = false, Text = "ID", Value = "ID"},
                    new SelectListItem { Selected = false, Text = "IL", Value = "IL"},
                    new SelectListItem { Selected = false, Text = "IN", Value = "IN"},
                    new SelectListItem { Selected = false, Text = "IA", Value = "IA"},
                    new SelectListItem { Selected = false, Text = "KS", Value = "KS"},
                    new SelectListItem { Selected = false, Text = "KY", Value = "KY"},
                    new SelectListItem { Selected = false, Text = "LA", Value = "LA"},
                    new SelectListItem { Selected = false, Text = "ME", Value = "ME"},
                    new SelectListItem { Selected = false, Text = "MD", Value = "MD"},
                    new SelectListItem { Selected = false, Text = "MA", Value = "MA"},
                    new SelectListItem { Selected = false, Text = "MI", Value = "MI"},
                    new SelectListItem { Selected = false, Text = "MN", Value = "MN"},
                    new SelectListItem { Selected = false, Text = "MS", Value = "MS"},
                    new SelectListItem { Selected = false, Text = "MO", Value = "MO"},
                    new SelectListItem { Selected = false, Text = "MT", Value = "MT"},
                    new SelectListItem { Selected = false, Text = "NE", Value = "NE"},
                    new SelectListItem { Selected = false, Text = "NV", Value = "NV"},
                    new SelectListItem { Selected = false, Text = "NH", Value = "NH"},
                    new SelectListItem { Selected = false, Text = "NJ", Value = "NJ"},
                    new SelectListItem { Selected = false, Text = "NH", Value = "NH"},
                    new SelectListItem { Selected = false, Text = "NM", Value = "NM"},
                    new SelectListItem { Selected = false, Text = "NY", Value = "NY"},
                    new SelectListItem { Selected = false, Text = "NC", Value = "NC"},
                    new SelectListItem { Selected = false, Text = "ND", Value = "ND"},
                    new SelectListItem { Selected = false, Text = "OH", Value = "OH"},
                    new SelectListItem { Selected = false, Text = "OK", Value = "OK"},
                    new SelectListItem { Selected = false, Text = "OR", Value = "OR"},
                    new SelectListItem { Selected = false, Text = "PA", Value = "PA"},
                    new SelectListItem { Selected = false, Text = "RI", Value = "RI"},
                    new SelectListItem { Selected = false, Text = "SC", Value = "SC"},
                    new SelectListItem { Selected = false, Text = "SD", Value = "SD"},
                    new SelectListItem { Selected = false, Text = "TN", Value = "TN"},
                    new SelectListItem { Selected = false, Text = "TX", Value = "TX"},
                    new SelectListItem { Selected = false, Text = "UT", Value = "UT"},
                    new SelectListItem { Selected = false, Text = "VT", Value = "VT"},
                    new SelectListItem { Selected = false, Text = "VA", Value = "VA"},
                    new SelectListItem { Selected = false, Text = "WA", Value = "WA"},
                    new SelectListItem { Selected = false, Text = "WV", Value = "WV"},
                    new SelectListItem { Selected = false, Text = "WI", Value = "WI"},
                    new SelectListItem { Selected = false, Text = "WY", Value = "WY"},
                }, "Value", "Text",selectedValue: "AZ");

            return states;
        }

        #endregion
    }
}
