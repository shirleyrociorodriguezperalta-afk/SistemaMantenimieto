using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaMantenimiento.Controllers
{
    [AllowAnonymous] // Permite el acceso público a la portada
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Si el usuario ya tiene la sesión activa en el navegador, va al tablero
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Mantenimiento");
            }

            // Muestra la vista de presentación (Views/Home/Index.cshtml)
            return View();
        }
    }
}