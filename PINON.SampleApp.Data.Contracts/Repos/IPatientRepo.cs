using System.Linq;
using PINON.SampleApp.Data.Models;
using PINON.SampleApp.Common;

namespace PINON.SampleApp.Data.Contracts.Repos
{
    public interface IPatientRepo
    {
        Patient Get(int id);
        IQueryable<Patient> GetAll();
        TransactionResult Save(Patient record, string userName);        
    }
}