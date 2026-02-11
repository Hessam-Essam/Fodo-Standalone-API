using Microsoft.AspNetCore.Mvc;

namespace Fodo.Web.Controllers
{
    public class BranchesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _AddUserBranch()
        {
            return View();
        }
    }
}
