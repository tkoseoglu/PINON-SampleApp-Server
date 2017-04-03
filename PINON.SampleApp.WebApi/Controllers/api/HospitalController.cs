using System.Linq;
using System.Web.Http;
using PINON.SampleApp.Data.Contracts.Repos;
using PINON.SampleApp.Tokens.Filters;

namespace PINON.SampleApp.WebApi.Controllers.api
{
    [JwtAuthentication]
    public class HospitalController : ApiController
    {
        private readonly IHospitalRepo _hospitalRepo;

        public HospitalController(IHospitalRepo hospitalRepo)
        {
            _hospitalRepo = hospitalRepo;
        }

        public string Get()
        {
            return "Hospital";
        }

        [AllowAnonymous]
        [Route("api/Hospital/GetAll")]
        public IHttpActionResult GetAll()
        {
            var hospitals = _hospitalRepo.GetAll().Where(p => !p.IsDeleted).ToList().Select(h => new
            {
                h.Id,
                h.HospitalName
            }).ToList();
            return this.Ok(hospitals);
        }
    }
}