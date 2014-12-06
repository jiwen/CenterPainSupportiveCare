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
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(await db.Medications.ToListAsync());
            }
            return RedirectToAction("Index","Home");
        }

        // GET: Medications/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Medication medication = await db.Medications.FindAsync(id);
            db.Medications.Remove(medication);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
