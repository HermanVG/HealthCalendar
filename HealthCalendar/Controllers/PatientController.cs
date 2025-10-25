using HealthCalendar.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class PatientController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        // TODO: Implement real authentication logic here
        // For now, always redirect to PatientEvents for demo/testing
        return RedirectToAction("PatientEvents");
    }

}