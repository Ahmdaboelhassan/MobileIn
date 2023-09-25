using Business.IRepository;
using Business.SD;
using Business.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MobileIn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OrderDetails(int id)
        {
            OrderViewModel order = new()
            {
                orderHeader = _unitOfWork.OrderHeader.
                GetFirstOrDefault(o => o.Id == id , new string[] {SD.ApplicationUser}),
                orderDatials = _unitOfWork.OrderDatials.
                GetAllWhere(o => o.OrderId == id, new string[] { SD.OrderHeader,SD.Mobile }),
            };

            if(order.orderHeader is null) 
                return NotFound(); 
  
            return View(order);
        }

        public IActionResult ShipOrder(int id) {
            var order = _unitOfWork.OrderHeader.GetFirstOrDefault(order => order.Id == id);
            if (order is null)
                return NotFound();

            order.OrderStatus = SD.Shipping;
            order.ShippingDate = DateTime.Now;
            _unitOfWork.OrderHeader.Update(order);
            _unitOfWork.SaveChanges();

            return RedirectToAction("ManageOrder", "Dashbourd", new {active = "Orders"});
        }
        public IActionResult CancelOrder(int id)
        {
            var order = _unitOfWork.OrderHeader.GetFirstOrDefault(order => order.Id == id);
            if (order is null)
                return NotFound();

            _unitOfWork.OrderHeader.Delete(order);
            _unitOfWork.SaveChanges();

            return RedirectToAction("ManageOrder", "Dashbourd", new { active = "Orders" });
        }
        public IActionResult ShipAll()
        {
            var orders = _unitOfWork.OrderHeader.GetAllWhere(order => order.OrderStatus == SD.Pending);
            if (orders is null || orders.Count() == 0)
            {
                TempData["Error"] = "There are not Pending Orders";
                return RedirectToAction("ManageOrder", "Dashbourd", new { active = "Orders" });
            }
            foreach (var order in orders)
            {
                order.OrderStatus = SD.Shipping;
                order.ShippingDate = DateTime.Now;
                _unitOfWork.OrderHeader.Update(order);
            }
            _unitOfWork.SaveChanges();
            return RedirectToAction("ManageOrder", "Dashbourd", new { active = "Orders" });

        }

    }
}
