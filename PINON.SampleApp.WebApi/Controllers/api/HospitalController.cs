using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PINON.SampleApp.Data.Contracts.Repos;
using PINON.SampleApp.Data.Models;
using PINON.SampleApp.Identity.Contracts;
using PINON.SampleApp.Tokens.Filters;
using PINON.SampleApp.WebApi.Helpers;
using PINON.SampleApp.WebApi.Models;

namespace PINON.SampleApp.WebApi.Controllers.api
{
    [JwtAuthentication]
    public class HospitalController : AppBaseController
    {
        private readonly IHospitalRepo _hospitalRepo;
        private readonly IIdentityManager _identityManager;

        public HospitalController(IHospitalRepo hospitalRepo, IIdentityManager identityManager)
        {
            _hospitalRepo = hospitalRepo;
            _identityManager = identityManager;
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
            if (!this.CurrentUserIsAdmin())
            {
                return this.Unauthorized();
            }

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
            var userAccounts = _identityManager.GetUserAccounts();

            var hospital = _hospitalRepo.GetAll().Where(p => p.Id == id).ToList().Select(p => new
            {
                p.Id,
                p.HospitalName,
                Patients = from hp in p.Patients
                           join ua in userAccounts on hp.UserAccountId equals ua.Id
                           select new
                           {
                               ua.FirstName,
                               ua.LastName,
                               hp.EyeColor,
                               hp.HairColor,
                               hp.Weight,
                               hp.Height
                           }
            }).FirstOrDefault();                        
            return this.Ok(hospital);
        }

        [HttpPost]
        [Route("api/Hospital/SaveHospital")]
        public IHttpActionResult SaveHospital(Hospital hospital)
        {
            if (!this.CurrentUserIsAdmin())
            {
                return this.Unauthorized();
            }
            var result = _hospitalRepo.Save(hospital, this.CurrentUserName());
            return this.Ok(result);
        }

        [HttpGet]
        [Route("api/Hospital/DeleteHospital/{id}")]
        public IHttpActionResult DeleteHospital(int id)
        {
            if (!this.CurrentUserIsAdmin())
            {
                return this.Unauthorized();
            }

            var hospitalToDelete = _hospitalRepo.GetAll().FirstOrDefault(p => p.Id == id);
            if (hospitalToDelete == null)
            {
                return this.BadRequest("Hospital not found");
            }

            hospitalToDelete.IsDeleted = true;
            var result = _hospitalRepo.Save(hospitalToDelete, this.CurrentUserName());

            return this.Ok(result);
        }

    }
}