using System.Collections.ObjectModel;

namespace PINON.SampleApp.Data.Models
{
    public class Hospital : Common
    {
        public string HospitalName { get; set; }

        public virtual Collection<Patient> Patients { get; set; }
    }
}