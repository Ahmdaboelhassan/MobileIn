using Business.IRepository;
using Business.SD;
using Business.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace MobileIn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Admin},{SD.SuperAdmin}")]

    public class DashbourdController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashbourdController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var compnies = _unitOfWork.Companies.GetAll();
            return View(compnies);
        }
        [Authorize(Roles = SD.Admin)]
        public IActionResult Processors()
        {
            var processors = _unitOfWork.Processors.GetAll(new string[] { SD.COMPANY });
            return View(processors);
        }
        [Authorize(Roles = SD.Admin)]
        public IActionResult Mobiles()
        {
            return View();
        }
        [Authorize(Roles = SD.Admin)]
        public IActionResult MobilesJson()
        {
            var mobiles = _unitOfWork.Mobiles.GetAll(new string[] { SD.COMPANY , SD.PROCESSOR});
            return Json(new { data = mobiles });
        }

        [Authorize(Roles = SD.SuperAdmin)]
        public IActionResult Users()
        {
           var users =  _userManager.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = SD.SuperAdmin)]
        public IActionResult UsersJson()
        {
           var users =  _userManager.Users.ToList();
            return Json(new {data = users});
        }

        [Authorize(Roles = SD.Admin)]
        public IActionResult ManageOrder()
        {
            return View();
        }

        [Authorize(Roles = SD.Admin)]
        public IActionResult OrdersJson()
        {
            var orderHeader = _unitOfWork.OrderHeader.GetAll();
            return Json(new { data = orderHeader });
        }
    }

}
