using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PracticaParaPracticaJueves.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
