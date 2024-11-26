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
    public class UsersController : Controller
    {
        private IUsersService svc;
        public UsersController(IUsersService svc)
        {
            this.svc = svc;
        }

        public async Task<IActionResult> Index()
        {
            List<UserViewModel> users = await svc.GetUsersAsync();

            return View(users);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel user)
        {
            await svc.AddUserAsync(user);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            UserViewModel user = await svc.GetUserById(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel user)
        {
            await svc.EditUserAsync(user);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            await svc.DeleteUserAsync(userId);

            return RedirectToAction("Index");
        }
    }
}

