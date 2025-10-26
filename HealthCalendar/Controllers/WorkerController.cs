using Microsoft.AspNetCore.Mvc;
using HealthCalendar.Services;
using HealthCalendar.ViewModels;
using HealthCalendar.Models;
using Microsoft.AspNetCore.Http;

public class WorkerController : Controller
{
    private readonly IUserService _userService;

    public WorkerController(IUserService userService)
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

    var (worker, status) = await _userService.WorkerLogin(model.Email ?? "", model.Password ?? "");

        if (status == HealthCalendar.Shared.OperationStatus.Success && worker != null)
        {
            HttpContext.Session.SetInt32("WorkerId", worker.WorkerId);
            return RedirectToAction("WorkerEvents", "Event");
        }

        ModelState.AddModelError("", "Invalid email or password.");
        return View(model);
    }
}