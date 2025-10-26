using Microsoft.AspNetCore.Mvc;

public class WorkerController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}