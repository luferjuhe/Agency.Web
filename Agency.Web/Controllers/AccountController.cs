using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agency.Data.Entities;
using Agency.Web.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Agency.Web.Controllers
{
    public class AccountController : Controller
    { 
        private IUsersService svc;
        public AccountController(IUsersService svc)
        {
            this.svc = svc;
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel vm)
        {

            //if (ModelState.IsValid)
            //{
            User user = await svc.GetUserByEmail(vm.Email);
            if (user != null)
            {

                if (user.InactiveDate != null)
                {
                    ModelState.AddModelError("", "El usuario se encuentra dado de baja");

                    return View(vm);
                }
                if (user.Password == vm.Password)
                {
                    return await Claims(user.UserId, vm.ReturnUrl);
                }

                ModelState.AddModelError("", "Usuario o contraseña incorrecta");
                //}
            }

            return View(vm);
        }

        [AllowAnonymous]
        private async Task<IActionResult> Claims(int userId, string returnUrl)
        {
            UserViewModel user = await svc.GetUserById(userId);
            var claims = new List<Claim>
             {
                 new Claim("UserId", user.UserId.ToString(), ClaimValueTypes.Integer),
                 new Claim("Name",user.Name),
                 //new Claim("UserActions", await svc.GetActionsForUserAsync(user.UserId))
             };
            var claimsIdentity = new ClaimsIdentity(claims,"Agency.Web");
            await HttpContext.SignInAsync("Agency.Web", new ClaimsPrincipal(claimsIdentity));

            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
            {
                returnUrl = returnUrl.Replace("/Search", "");
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Users");
        }
    }
}

