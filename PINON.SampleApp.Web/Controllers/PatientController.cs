using System.Web.Mvc;
using PINON.SampleApp.Data.Contracts.Repos;

namespace PINON.SampleApp.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepo _patientRepo;

        public PatientController(IPatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        [Authorize(Roles = "Admin, Patient")]
        public ActionResult TestSecure()
        {
            return Json("Secure data", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TestSecureAdmin()
        {
            return Json("Admin data only", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult TestUnSecure()
        {
            return Json("Unsecure data", JsonRequestBehavior.AllowGet);
        }
    }
}