using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixteenClothing.Models;
using SixteenClothing.ViewModels.UserViewModels;

namespace SixteenClothing.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager,RoleManager<IdentityRole> _roleManager
        ) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new()
            {
                Email = vm.Email,
                UserName = vm.Username,
                Fullname = vm.Fullname,

            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user, "Member");

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)

                return View(vm);
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, true);

            if (!result.Succeeded)
            {

                ModelState.AddModelError("", "Username or password is incorrect");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
                


        }

        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Admin"
            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Member"
            });
            return Ok("Roles was created");
        }
    }
}
