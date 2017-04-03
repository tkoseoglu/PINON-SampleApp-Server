using System.Web.Mvc;

namespace PINON.SampleApp.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }
    }
}