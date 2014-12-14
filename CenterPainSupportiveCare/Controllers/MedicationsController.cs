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

namespace CenterPainSupportiveCare.Controllers
{
    public class MedicationsController : Controller
    {
        private Entities db = new Entities();

        // GET: Medications
        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(await db.Medications.ToListAsync());
            }
            return RedirectToAction("Index","Home");
        }

        // GET: Medications/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = await db.Medications.FindAsync(id);
            if (medication == null)
            {
                return HttpNotFound();
            }
            return View(medication);
        }

        // GET: Medications/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "MedicationId,MedicationName,Measure,Volume,Price")] Medication medication)
        {
            if (ModelState.IsValid)
            {
                db.Medications.Add(medication);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(medication);
        }

        // GET: Medications/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = await db.Medications.FindAsync(id);
            if (medication == null)
            {
                return HttpNotFound();
            }
            return View(medication);
        }

        // POST: Medications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit([Bind(Include = "MedicationId,MedicationName,Measure,Volume,Price")] Medication medication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medication).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(medication);
        }

        // GET: Medications/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = await db.Medications.FindAsync(id);
            if (medication == null)
            {
                return HttpNotFound();
            }
            return View(medication);
        }

        // POST: Medications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Medication medication = await db.Medications.FindAsync(id);
            db.Medications.Remove(medication);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Medications
        [Authorize]
        public async Task<ActionResult> Search(string Medications, string Measures, string Volumes)
        {
            if (Request.IsAuthenticated)
            {
                if(!string.IsNullOrEmpty(Medications) && !string.IsNullOrEmpty(Measures) && !string.IsNullOrEmpty(Volumes))
                {
                     var model = db.Medications.FirstOrDefaultAsync(m => m.MedicationName == Medications && m.Measure == Measures && m.Volume == Volumes);
                     View(await model);
                }

                if (!string.IsNullOrEmpty(Medications) || !string.IsNullOrEmpty(Measures) || !string.IsNullOrEmpty(Volumes))
                {
                    if (!string.IsNullOrEmpty(Medications))
                    {
                        var measureList = new List<SelectListItem>();
                        var medicationsMeasure = await db.Medications.Where(m => m.MedicationName == Medications).GroupBy(m =>m.Measure).ToListAsync();
                        measureList.AddRange(medicationsMeasure.Select(m => new SelectListItem() { Text = m.Key, Value = m.Key }));
                        ViewBag.Measures = measureList;

                    }

                    if (!string.IsNullOrEmpty(Measures))
                    {
                        var volumeList = new List<SelectListItem>();
                        var medicationsVolume = await db.Medications.Where(m => m.MedicationName == Medications && m.Measure == Measures).ToListAsync();
                        volumeList.AddRange(medicationsVolume.Select(m => new SelectListItem() { Text = m.Volume, Value = m.Volume}));
                        ViewBag.Volumes = volumeList;

                    }
                }
                
                    var list = new List<SelectListItem>();
                    var medications = await db.Medications.GroupBy(m => m.MedicationName).ToListAsync();
                    list.AddRange(medications.Select(m => new SelectListItem() { Text = m.Key, Value = m.Key }));
                    ViewBag.Medications = list;
                
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
