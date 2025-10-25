using Microsoft.AspNetCore.Mvc;

public class PatientController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}