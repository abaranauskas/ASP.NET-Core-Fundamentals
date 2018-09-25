using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Entities;
using OdeToFood.ViewModels;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OdeToFood.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _sighInManager;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> sighInManager)
        {
            _userManager = userManager;
            _sighInManager = sighInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.UserName = model.UserName;
                //user.PasswordHash = model.Password;
                //taip passwordas negali but priskirta 
                //jei taip padarysiu netikrins paswordo stiptumo


                //tik sitaip galima priskirt pasworda
                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    await _sighInManager.SignInAsync(user, false); //false duoda kad isejus neprisimins reiks prisijungt per nauja
                    return RedirectToAction("Index", "Home"); //redirectinam i home controller index action
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _sighInManager.PasswordSignInAsync(model.UserName,
                                model.Password, model.RememberMe, false);
                //paskutinis false reiskia ar lockoutint jei suklys

                if (loginResult.Succeeded)
                {
                    if (Url.IsLocalUrl(model.returnUrl))  //prevents open redirect
                    {
                        return Redirect(model.returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

            ModelState.AddModelError("", "Coun not login");
            //bendrinis error  kad nezinotu jok paswordas neteisingas o tarkim usser toks yra
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _sighInManager.SignOutAsync(); //pasalins cookie is browser
            return RedirectToAction("Index", "Home");
        }
    }
}
