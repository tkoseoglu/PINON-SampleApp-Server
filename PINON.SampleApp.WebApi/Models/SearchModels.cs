using System.Collections.Generic;

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
}