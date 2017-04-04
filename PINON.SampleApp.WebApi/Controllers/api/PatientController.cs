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
    public class PatientController : AppBaseController
    {
        private readonly IPatientRepo _patientRepo;
        private readonly IHospitalRepo _hospitalRepo;
        private readonly IIdentityManager _identityManager;

        public PatientController(IPatientRepo patientRepo, IHospitalRepo hospitalRepo, IIdentityManager identityManager)
        {
            _patientRepo = patientRepo;
            _hospitalRepo = hospitalRepo;
            _identityManager = identityManager;
        }

        [Route("api/Patient/Search")]
        public IHttpActionResult Search(PatientSearch searchQuery)
        {
            if (!this.CurrentUserIsAdmin())
            {
                return this.BadRequest("Admin privileges required");
            }

            var patientQuery = _patientRepo.GetAll().OrderBy(p => p.Id).Where(p => !p.IsDeleted);

            var userAccounts = _identityManager.GetUserAccounts();

            if (!string.IsNullOrEmpty(searchQuery.FirstName))
            {
                userAccounts = userAccounts.Where(p => p.FirstName.StartsWith(searchQuery.FirstName));
            }
            if (!string.IsNullOrEmpty(searchQuery.LastName))
            {
                userAccounts = userAccounts.Where(p => p.LastName.StartsWith(searchQuery.LastName));
            }
            if (!string.IsNullOrEmpty(searchQuery.HospitalName))
            {
                patientQuery = patientQuery.Where(p => p.Hospital.HospitalName.StartsWith(searchQuery.HospitalName));
            }

            var patients = from p in patientQuery.Skip(searchQuery.Page * searchQuery.PageSize).Take(searchQuery.PageSize).ToList()
                           join ua in userAccounts.ToList() on p.UserAccountId equals ua.Id
                           select new PatientSearchViewModel
                           {
                               FirstName = ua.FirstName,
                               LastName = ua.LastName,
                               EyeColor = p.EyeColor,
                               HairColor = p.HairColor,
                               Weight = p.Weight,
                               Height = p.Height
                           };

            var totalNumberOfRecordsFound = patientQuery.Count();

            var result = new PatientSearchResult
            {
                Patients = patients.ToList(),
                TotalNumberOfRecords = totalNumberOfRecordsFound
            };

            return this.Ok(result);
        }

        /// <summary>
        /// Patient action/role. Patients can only see their own "profile" (details)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Patient/GetPatientDetails")]
        public async Task<IHttpActionResult> GetPatientDetails()
        {
            var email = this.GetUserClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            if (string.IsNullOrEmpty(email))
            {
                return this.Ok(BadRequest());
            }
            var user = await _identityManager.FindByEmailAsync(email);

            var patient = _patientRepo.GetAll().Where(p => p.UserAccountId == user.Id).ToList().Select(p => new
            {
                p.Id,
                p.HospitalId,
                Hospital = _hospitalRepo.GetAll().Where(h => h.Id == p.HospitalId).ToList().Select(h => new
                {
                    h.Id,
                    h.HospitalName
                }).FirstOrDefault(),
                p.UserAccountId,
                p.EyeColor,
                p.HairColor,
                p.Height,
                p.Weight
            }).FirstOrDefault();
            if (patient == null)
            {
                return this.Ok(BadRequest());
            }

            var response = new
            {
                UserAccount = user,
                Patient = patient
            };

            return this.Ok(response);
        }

        [HttpPost]
        [Route("api/Patient/SavePatient")]
        public IHttpActionResult SavePatient(Patient patient)
        {
            var result = _patientRepo.Save(patient, this.CurrentUserName());
            return this.Ok(result);
        }

        /// <summary>
        /// Patient action. Patient deleting his/her own account
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Patient/DeletePatient")]
        public async Task<IHttpActionResult> DeletePatient(Patient patient)
        {
            var email = this.GetUserClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            if (string.IsNullOrEmpty(email))
            {
                return this.Ok(BadRequest());
            }
            var user = await _identityManager.FindByEmailAsync(email);
            var patientToDelete = _patientRepo.GetAll().FirstOrDefault(p => p.UserAccountId == user.Id);
            if (patientToDelete == null)
            {
                return this.BadRequest("Patient not found");
            }
            if (patientToDelete.UserAccountId != patient.UserAccountId)
            {
                return this.BadRequest("Patient mismatch");
            }

            //part 1 update user account
            await _identityManager.DeleteAsync(user.Id);

            //part 1 update patient record
            patient.IsDeleted = true;
            var result = _patientRepo.Save(patientToDelete, this.CurrentUserName());

            return this.Ok(result);
        }
    }
}