using Microsoft.AspNetCore.Mvc;

namespace Fodo.Web.Controllers
{
    public class UserManagmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
