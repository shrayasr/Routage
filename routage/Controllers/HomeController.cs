using routage.Analysis;
using System.Linq;
using System.Web.Mvc;

namespace routage.Controllers
{
    public class HomeController : Controller
    {
        [Route("~/")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("routes")]
        public ActionResult GetRoutes()
        {
            var routesIndexer = new RoutesIndexer();
            return Json(routesIndexer.GetRoutes(), JsonRequestBehavior.AllowGet);
        }
    }
}