using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PINON.SampleApp.Data.Contracts.Repos;
using PINON.SampleApp.Data.Models;
using PINON.SampleApp.Tokens.Filters;
using PINON.SampleApp.WebApi.Helpers;
using PINON.SampleApp.WebApi.Models;

namespace PINON.SampleApp.WebApi.Controllers.api
{
    [JwtAuthentication]
    public class HospitalController : AppBaseController
    {
        private readonly IHospitalRepo _hospitalRepo;

        public HospitalController(IHospitalRepo hospitalRepo)
        {
            _hospitalRepo = hospitalRepo;
        }

        [AllowAnonymous]
        [Route("api/Hospital/GetAll")]
        public IHttpActionResult GetAll()
        {
            var hospitals = _hospitalRepo.GetAll().Where(p => !p.IsDeleted).ToList().Select(h => new HospitalSearchViewModel
            {
                Id = h.Id,
                HospitalName = h.HospitalName,
                NumberOfPatients = h.Patients.Count
            }).ToList();
            return this.Ok(hospitals);
        }

        [HttpPost]
        [Route("api/Hospital/Search")]
        public IHttpActionResult Search(HospitalSearch searchQuery)
        {
            var hospitalQuery = _hospitalRepo.GetAll().Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(searchQuery.HospitalName))
            {
                hospitalQuery = hospitalQuery.Where(p => p.HospitalName.StartsWith(searchQuery.HospitalName));
            }

            var hospitals = hospitalQuery.OrderBy(p => p.HospitalName).Skip(searchQuery.Page * searchQuery.PageSize).Take(searchQuery.PageSize).ToList().Select(h => new HospitalSearchViewModel
            {
                Id = h.Id,
                HospitalName = h.HospitalName,
                NumberOfPatients = h.Patients.Count
            }).ToList();

            var totalNumberOfRecordsFound = hospitalQuery.Count();

            var result = new HospitalSearchResult
            {
                Hospitals = hospitals,
                TotalNumberOfRecords = totalNumberOfRecordsFound
            };

            return this.Ok(result);
        }

        [HttpGet]
        [Route("api/Hospital/GetHospitalDetails/{id}")]
        public IHttpActionResult GetHospitalDetails(int id)
        {
            var hospital = _hospitalRepo.GetAll().Where(p => p.Id == id).ToList().Select(p => new
            {
                p.Id,
                p.HospitalName,
            }).FirstOrDefault();
            if (hospital == null)
            {
                return this.Ok(BadRequest());
            }
            return this.Ok(hospital);
        }

        [HttpPost]
        [Route("api/Hospital/SaveHospital")]
        public IHttpActionResult SaveHospital(Hospital hospital)
        {
            var result = _hospitalRepo.Save(hospital, this.CurrentUserName());
            return this.Ok(result);
        }

    }
}