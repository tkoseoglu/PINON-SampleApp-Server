using System.Linq;
using PINON.SampleApp.Common;
using PINON.SampleApp.Data.Models;

namespace PINON.SampleApp.Data.Contracts.Repos
{
    public interface IHospitalRepo
    {
        Hospital Get(int id);
        IQueryable<Hospital> GetAll();
        TransactionResult Save(Hospital record, string userName);        
    }
}