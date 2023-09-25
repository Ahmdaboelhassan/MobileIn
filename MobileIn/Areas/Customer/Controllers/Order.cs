using Business.IRepository;
using Business.SD;
using Business.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MobileIn.Areas.Customer.Controllers
{
    [Area("Customer")]
	public class Order : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public Order(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[Authorize]
		public IActionResult Index()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
	
			if (userId is null)
				return NotFound();

			var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.ApplicationUserId == userId , new string[] { SD.ApplicationUser});

            var orderDetails = _unitOfWork.OrderDatials.GetAllWhere(o => o.OrderHeader.ApplicationUserId == userId, new string[] { SD.OrderHeader, SD.Mobile });

			OrderViewModel userOrder = new OrderViewModel()
			{
				orderDatials = orderDetails,
				orderHeader = orderHeader
			};
			return View(userOrder);
		}
	}
}
