using Microsoft.AspNetCore.Mvc;
using HealthCalendar.Services;
using HealthCalendar.ViewModels;
using HealthCalendar.Models;
using Microsoft.AspNetCore.Http;

public class PatientController : Controller
{
    private readonly IUserService _userService;

    public PatientController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

    var (patient, status) = await _userService.PatientLogin(model.Email ?? "", model.Password ?? "");

        if (status == HealthCalendar.Shared.OperationStatus.Success && patient != null)
        {
            HttpContext.Session.SetInt32("PatientId", patient.PatientId);
            return RedirectToAction("PatientEvents", "Event");
        }

        ModelState.AddModelError("", "Invalid email or password.");
        return View(model);
    }
}