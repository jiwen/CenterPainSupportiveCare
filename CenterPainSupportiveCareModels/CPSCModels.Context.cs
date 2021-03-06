﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Medication> Medications { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AbdomenExam> AbdomenExams { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Allergy> Allergies { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AssessmentAndPlan> AssessmentAndPlans { get; set; }
        public virtual DbSet<CardioExam> CardioExams { get; set; }
        public virtual DbSet<ChiefComplaint> ChiefComplaints { get; set; }
        public virtual DbSet<ExtremityExam> ExtremityExams { get; set; }
        public virtual DbSet<GoalsOfCare> GoalsOfCares { get; set; }
        public virtual DbSet<HistoryPresentIllness> HistoryPresentIllnesses { get; set; }
        public virtual DbSet<HistoryType> HistoryTypes { get; set; }
        public virtual DbSet<LabType> LabTypes { get; set; }
        public virtual DbSet<LabValueType> LabValueTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MedicationType> MedicationTypes { get; set; }
        public virtual DbSet<MusculoskeltalExam> MusculoskeltalExams { get; set; }
        public virtual DbSet<NeuroPsychExam> NeuroPsychExams { get; set; }
        public virtual DbSet<OtherExam> OtherExams { get; set; }
        public virtual DbSet<PatientLabResult> PatientLabResults { get; set; }
        public virtual DbSet<PatientLab> PatientLabs { get; set; }
        public virtual DbSet<PatientMedication> PatientMedications { get; set; }
        public virtual DbSet<PhysicalExam> PhysicalExams { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<PulmonaryExam> PulmonaryExams { get; set; }
        public virtual DbSet<RadiologyResult> RadiologyResults { get; set; }
        public virtual DbSet<ReviewOfSystem> ReviewOfSystems { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Sensation> Sensations { get; set; }
        public virtual DbSet<StrengthExam> StrengthExams { get; set; }
        public virtual DbSet<PatientHistory> PatientHistories { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<PrescriptionMedicationSyrinx> PrescriptionMedicationSyringes { get; set; }
        public virtual DbSet<Error> Errors { get; set; }
        public virtual DbSet<Syrinx> Syringes { get; set; }
    }
}
