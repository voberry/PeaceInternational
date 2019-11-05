using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeaceInternational.Web.Models;

namespace PeaceInternational.Web.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private Notification notification;

        public UsersController(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET Users
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                if (id == null)
                {
                    var result = await _userManager.GetUsersInRoleAsync("USER");                  

                    return Json(result.Select(p => new { p.Id, p.UserName, p.PhoneNumber }));
                }
                else
                {
                    var result = await _userManager.FindByIdAsync(id);
                    return Json(result);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //Save User
        [HttpPost]
        public async Task<IActionResult> Save(CreateUserDTO newUser)
        {
            try
            {
                notification = new Notification();

                var user = new IdentityUser()
                {
                    UserName = newUser.Username,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber
                };

                var res = await _userManager.CreateAsync(user, newUser.Password);
                await _userManager.SetLockoutEnabledAsync(user, false);
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, newUser.Role);
                }

                notification.Type = "success";
                notification.Message = "User successfully created.";

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "User creation failed";
                return Json(notification);
            }
        }

        //Change Password
        [HttpPost]
        public async Task<IActionResult> ChangePassword (ChangePasswordDTO changePassword)
        {
            try
            {
                notification = new Notification();
                var user = await _userManager.FindByIdAsync(changePassword.UserId);

                var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, changePassword.NewPassword);
                user.PasswordHash = newPasswordHash;
                var res = await _userManager.UpdateAsync(user);

                notification.Type = "success";
                notification.Message = "Password successfully changed.";

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = $"Failed to change password. ";
                return Json(notification);
            }
        }

    }
}