using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PajoPhone.Models;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() { return View();}
    
    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    public IActionResult Register()
    {
        ViewBag.Roles = new List<string> { "Customer", "Admin" };
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        ViewBag.Roles = new List<string> { "Customer", "Admin" };
        return RedirectToAction("Index", "Home");
    }
}