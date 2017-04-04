using System.Collections.Generic;
using PINON.SampleApp.Data.Models;

namespace PINON.SampleApp.WebApi.Models
{
    public abstract class SearchBase
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }

    public class HospitalSearch : SearchBase
    {
        public string HospitalName { get; set; }
    }

    public class HospitalSearchViewModel
    {
        public int Id { get; set; }
        public string HospitalName { get; set; }
        public int NumberOfPatients { get; set; }
    }

    public class HospitalSearchResult
    {
        public List<HospitalSearchViewModel> Hospitals { get; set; }
        public int TotalNumberOfRecords { get; set; }
    }

    public class PatientSearch : SearchBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HospitalName { get; set; }
    }

    public class PatientSearchViewModel
    {
        //db id
        public int Id { get; set; }
        //identity id
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Hospital Hospital { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
    }

    public class PatientSearchResult
    {
        public List<PatientSearchViewModel> Patients { get; set; }
        public int TotalNumberOfRecords { get; set; }
    }
}