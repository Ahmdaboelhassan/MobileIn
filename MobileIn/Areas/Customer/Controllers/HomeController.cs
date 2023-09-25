using Business.IRepository;
using Business.SD;
using Business.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileIn.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace MobileIn.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        public HomeController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel()
            {
                companies = _unitOfWork.Companies.GetAll(),
                mobiles = _unitOfWork.Mobiles.GetAllOrderDesc(new string[] { SD.COMPANY, SD.PROCESSOR }),
                latestMobile = _unitOfWork.Mobiles.GetAllOrderDesc(),
            };

            return View(model);
        }

        public IActionResult Searching(string search)
        {
            var mobiles = _unitOfWork.Mobiles.GetAllWhere(
                m => m.name.ToLower().Contains(search.ToLower()) || m.company.name.ToLower() == search.ToLower()
            , new string[] { SD.COMPANY, SD.PROCESSOR }) ;
           
            return View(mobiles);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}