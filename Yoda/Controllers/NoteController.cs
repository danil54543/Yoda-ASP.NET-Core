using Microsoft.AspNetCore.Mvc;

namespace Yoda.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
