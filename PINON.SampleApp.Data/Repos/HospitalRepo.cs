using System;
using System.Linq;
using PINON.SampleApp.Common;
using PINON.SampleApp.Data.Contracts;
using PINON.SampleApp.Data.Contracts.Repos;
using PINON.SampleApp.Data.Models;

namespace PINON.SampleApp.Data.Repos
{
    public class HospitalRepo : IHospitalRepo
    {
        private readonly IAppDbContext _db;

        public HospitalRepo()
        {
        }

        public HospitalRepo(IAppDbContext db)
        {
            this._db = db;
        }

        public Hospital Get(int id)
        {
            return _db.Hospitals.Find(id);
        }

        public IQueryable<Hospital> GetAll()
        {
            return _db.Hospitals;
        }

        public TransactionResult Save(Hospital record, string userName)
        {
            var result = new TransactionResult();
            try
            {
                var dbRecord = this._db.Hospitals.Find(record.Id) ?? new Hospital();
                dbRecord.HospitalName = record.HospitalName;              
                if (record.Id == 0)
                    this._db.Hospitals.Add(dbRecord);

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