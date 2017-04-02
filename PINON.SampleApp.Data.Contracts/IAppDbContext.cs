using System.Data.Entity;
using PINON.SampleApp.Data.Models;
using System.Data.Entity.Infrastructure;

namespace PINON.SampleApp.Data.Contracts
{
    public interface IAppDbContext
    {
        DbSet<Hospital> Hospitals { get; set; }
        DbSet<Patient> Patients { get; set; }
        void Dispose();
        DbEntityEntry Entry(object o);
        int SaveChanges();
    }
}