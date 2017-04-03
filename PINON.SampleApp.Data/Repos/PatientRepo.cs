using System;
using System.Linq;
using PINON.SampleApp.Common;
using PINON.SampleApp.Data.Contracts;
using PINON.SampleApp.Data.Contracts.Repos;
using PINON.SampleApp.Data.Models;

namespace PINON.SampleApp.Data.Repos
{
    public class PatientRepo : IPatientRepo
    {
        private readonly IAppDbContext _db;

        public PatientRepo()
        {
        }

        public PatientRepo(IAppDbContext db)
        {
            this._db = db;
        }

        public Patient Get(int id)
        {
            return _db.Patients.Find(id);
        }

        public IQueryable<Patient> GetAll()
        {
            return _db.Patients;
        }

        public TransactionResult Save(Patient record, string userName)
        {
            var result = new TransactionResult();
            try
            {
                var dbRecord = this._db.Patients.Find(record.Id) ?? new Patient();
                dbRecord.HospitalId = record.HospitalId;
                dbRecord.UserAccountId = record.UserAccountId;
                dbRecord.EyeColor = record.EyeColor;
                dbRecord.HairColor = record.HairColor;
                dbRecord.Weight = record.Weight;
                dbRecord.Height = record.Height;
                dbRecord.CreatedBy = userName;
                dbRecord.ModifiedBy = userName;
                dbRecord.CreatedOn = DateTime.UtcNow;
                dbRecord.ModifiedOn = DateTime.UtcNow;
                dbRecord.IsDeleted = record.IsDeleted;

                if (record.Id == 0)
                    this._db.Patients.Add(dbRecord);

                this._db.SaveChanges();
                result.Id = dbRecord.Id;
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}