using Business.SD;
using Business.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Data;
using System.Drawing;
using System.Net.Mail;

namespace MobileIn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdmin)]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public UserController(UserManager<ApplicationUser> usermanager, 
            RoleManager<IdentityRole> rolemanager,
            IWebHostEnvironment webHostEnvironment,
            IUserStore<ApplicationUser> userStore
            )
        {
            _usermanager = usermanager;
            _rolemanager = rolemanager;
            _webHostEnvironment = webHostEnvironment;
            _userStore = userStore;
            _emailStore = GetEmailStore();
        }
        public IActionResult UserGet(string? id = null)
        {
            var allRoles = _rolemanager.Roles.ToListAsync().Result;

            UsersAndRolesViewModel user = new();
            if (id is null)
            {
                user.userRoles = allRoles.Select(
                    r => new Roles { Name = r.Name, IsSelected = false, }).ToList();
                return View(user);

            }

            ApplicationUser? userFromDb = _usermanager.FindByIdAsync(id).Result;

            if (userFromDb is null)
            {
                return NotFound();
            }

           
            user = new()
            {
                Id = userFromDb.Id,
                firstName = userFromDb.firstName,
                lastName = userFromDb.lastName,
                adress = userFromDb.adress,
                Email = userFromDb.Email,
                BD = userFromDb.BD,
                phoneNumber = userFromDb.PhoneNumber,
                profilePicture = userFromDb.profilePicture,
                /// mange roles
                userRoles = allRoles.Select(
                    r => new Roles
                    {
                        Name = r.Name,
                        IsSelected = _usermanager.IsInRoleAsync(userFromDb, r.Name).Result,
                    }).ToList()
            };
            return View(user);
        }
        public IActionResult UserPost(UsersAndRolesViewModel Model , IFormFile? profilePicture)
        {
            ApplicationUser newUser = new();
            // handle image
            if(profilePicture != null)
            {
                if (!Checkimage(profilePicture))
                    return View(SD.UserGetView, Model);

                string filename = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                string storeUrl = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "User", filename);
                using (var filestream = new FileStream(storeUrl, FileMode.Create))
                {
                    profilePicture.CopyTo(filestream);
                }

                newUser.profilePicture = Path.Combine(@"\", "Images", "User", filename);
            }

            // check password and make it requird
            if(Model.Password is null || Model.ConfirmPassword is null)
            {
                ModelState.AddModelError("Password", "Password And Confrim Password Are Requird");
                return View(SD.UserGetView, Model);
            }
            
            // check password and confrim password
            if(Model.Password != Model.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password And Confrim Password Are Not The Same");
                return View(SD.UserGetView, Model);
            }    

            // check the validtion of Model
            if (!ModelState.IsValid)
                return View(SD.UserGetView, Model);

            // check if email exists
            var emailexists = _usermanager.FindByEmailAsync(Model.Email).Result;
            if (emailexists is not null)
            {
                ModelState.AddModelError("Email", "The email is exist Before");
                return View(SD.UserGetView, Model);
            }
             
           
            newUser.firstName = Model.firstName.Trim();
            newUser.lastName = Model.lastName.Trim();
            newUser.adress = Model.adress.Trim();
            newUser.BD = Model.BD;
            newUser.PhoneNumber = Model.phoneNumber.Trim();

            _userStore.SetUserNameAsync(newUser, new MailAddress(Model.Email).User, CancellationToken.None).GetAwaiter().GetResult();

            _emailStore.SetEmailAsync(newUser, Model.Email, CancellationToken.None).GetAwaiter().GetResult();

            var result =  _usermanager.CreateAsync(newUser,Model.Password).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                foreach (var role in Model.userRoles)
                {
                    if (role.IsSelected)
                        _usermanager.AddToRoleAsync(newUser, role.Name).GetAwaiter().GetResult();
                }
            }
            return RedirectToAction("Users", "Dashbourd", new { active = "Users" });
        }
        public IActionResult UserUpdate(UsersAndRolesViewModel Model)
        {
            // get the user 
            var oldUser = _usermanager.FindByIdAsync(Model.Id).Result;
            if (oldUser is null)
                return NotFound("Invalid User");

            if (!ModelState.IsValid)
                return View(SD.UserGetView, Model);

            //handle image
            var image = Request.Form.Files.FirstOrDefault();
            if (image is not null)
            {
                string oldPath = _webHostEnvironment.WebRootPath + oldUser.profilePicture;
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "User", fileName);

                using (var file = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(file);
                }
                oldUser.profilePicture = Path.Combine(@"\", "Images", "User", fileName);
            }

            // update email
            _emailStore.SetEmailAsync(oldUser, Model.Email, CancellationToken.None).GetAwaiter().GetResult();

            // update password
            if (Model.Password != null || Model.ConfirmPassword != null)
            {
                string newPassword = Model.ConfirmPassword;

                var result = _usermanager.ChangePasswordAsync(oldUser, Model.Password, newPassword).Result;

                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(SD.UserGetView, Model);
                }
            }

            oldUser.firstName = Model.firstName;
            oldUser.lastName = Model.lastName;
            oldUser.adress = Model.adress;
            oldUser.BD = Model.BD;
            oldUser.PhoneNumber = Model.phoneNumber;
            oldUser.Email = Model.Email;

           

             _usermanager.UpdateAsync(oldUser).GetAwaiter().GetResult();
            
            // Manage role
            var roles = _usermanager.GetRolesAsync(oldUser).Result;

           foreach (var role in Model.userRoles)
            {
                if(role.IsSelected && !roles.Any(R => R == role.Name))
                {
                    _usermanager.AddToRoleAsync(oldUser, role.Name).GetAwaiter().GetResult();
                }

                if (!role.IsSelected && roles.Any(R => R == role.Name))
                {
                    _usermanager.RemoveFromRoleAsync(oldUser, role.Name).GetAwaiter().GetResult(); ;
                }
            }

            return RedirectToAction("Users", "Dashbourd",new {active = "Users"});
        }

        private bool Checkimage(IFormFile image)
        {
            //check Extistion and Length
            List<string> AllowedExtinstions = new List<string> { ".png", ".jpg" };
            if (!AllowedExtinstions.Contains(Path.GetExtension(image.FileName.ToLower())))
            {
                ModelState.AddModelError("profilePicture", "Extistion Not Allowed!!");
                return false;
            }
            if (image.Length > 2_097_152)
            {
                ModelState.AddModelError("profilePicture", "Length Not Allowed!! , The Max Length Is 2MB");
                return false;
            }
            return true;

        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_usermanager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }

   
}
