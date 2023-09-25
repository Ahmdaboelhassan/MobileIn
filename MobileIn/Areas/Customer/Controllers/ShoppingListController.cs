using Business.IRepository;
using Business.SD;
using Business.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileIn.Data;
using Stripe.Checkout;
using System.Security.Claims;

namespace MobileIn.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingListController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

		public ShoppingListController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

        [Authorize]
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return NotFound();

            IEnumerable<ShoppingList> ShoppingListItems = _unitOfWork.ShoppingList.GetAllWhere(S => S.ApplicationUserId == userId, new string[] { SD.Mobile });

            return View(ShoppingListItems);
        }

        
        [Authorize]
        public IActionResult AddToShoppingList(int mobileId)
        {
            Mobile mobile = _unitOfWork.Mobiles.GetFirstOrDefault(m => m.id == mobileId);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (mobile is null || userId is null)
                return NotFound();

            ShoppingList ShoppingListItem = _unitOfWork.ShoppingList.GetFirstOrDefault(S => S.MobileId == mobileId && S.ApplicationUserId == userId);

            if (ShoppingListItem != null)
            {
                TempData["Error"] = "This Mobile Already Exixt In Shopping list You Can Increase Count From Shopping List";
                return RedirectToAction(nameof(Index));
            }

            ShoppingList shoppingList = new ShoppingList()
            {
                ApplicationUserId = userId,
                MobileId = mobileId,
                count = 1

            };

            _unitOfWork.ShoppingList.Add(shoppingList);
            _unitOfWork.SaveChanges();
            TempData["Succsess"] = "This Mobile Added Sucsessfully ";

            return RedirectToAction(nameof(Index));
        }

        // manage count of item in shopping list 
        public IActionResult Plus(int mobileId)
        {
            Mobile mobile = _unitOfWork.Mobiles.GetFirstOrDefault(m => m.id == mobileId);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingList ShoppingListItem = _unitOfWork.ShoppingList.GetFirstOrDefault(S => S.MobileId == mobileId && S.ApplicationUserId == userId);


             if (mobile is null || userId is null || ShoppingListItem is null)
                return NotFound("There Are Some Problems");

            _unitOfWork.ShoppingList.PlusByOne(ShoppingListItem);
            _unitOfWork.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int mobileId)
        {
            Mobile mobile = _unitOfWork.Mobiles.GetFirstOrDefault(m => m.id == mobileId);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingList ShoppingListItem = _unitOfWork.ShoppingList.GetFirstOrDefault(S => S.MobileId == mobileId && S.ApplicationUserId == userId);

            if (mobile is null || userId is null || ShoppingListItem is null)
                return NotFound("There Are Some Problems");


            if(ShoppingListItem.count > 1)
            {
                _unitOfWork.ShoppingList.MinusByOne(ShoppingListItem);
            }
            else
            {
                _unitOfWork.ShoppingList.Delete(ShoppingListItem);
            }

            _unitOfWork.SaveChanges();
			return RedirectToAction(nameof(Index));

		}

		// delete shoppinglist item
        public IActionResult Delete(int mobileId)
        {
            Mobile mobile = _unitOfWork.Mobiles.GetFirstOrDefault(m => m.id == mobileId);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingList ShoppingListItem = _unitOfWork.ShoppingList.GetFirstOrDefault(S => S.MobileId == mobileId && S.ApplicationUserId == userId);

            if (mobile is null || userId is null || ShoppingListItem is null)
                return NotFound("There Are Some Problems");


            _unitOfWork.ShoppingList.Delete(ShoppingListItem);
            _unitOfWork.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

        // place order => summary
        public IActionResult SummaryGet()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(user => user.Id == userId);

            var shoppingListFromDb =
                _unitOfWork.ShoppingList.GetAllWhere(s => s.ApplicationUserId == userId, new string[] { SD.Mobile });

            if (userFromDb is null || shoppingListFromDb is null)
                return NotFound();

            SummaryViewModel Model = new SummaryViewModel()
            {
                shoppingList = shoppingListFromDb,
                orderHeader = new()
            };

            Model.orderHeader.firstName = userFromDb.firstName;
            Model.orderHeader.lastName = userFromDb.lastName;
            Model.orderHeader.address = userFromDb.adress;
            Model.orderHeader.PhoneNumber = userFromDb.PhoneNumber;
            Model.orderHeader.finalPrice = GetFinalPrice(shoppingListFromDb);
         
            return View(Model);
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(SummaryViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);

			var shoppingListFromDb =
				   _unitOfWork.ShoppingList.GetAllWhere(s => s.ApplicationUserId == userId, new string[] { SD.Mobile });

			if (userFromDb is null || shoppingListFromDb is null)
				return NotFound();

            model.shoppingList = shoppingListFromDb;

			//// Seed Order Header
			OrderHeader orderHeader = new OrderHeader()
            {
                ApplicationUserId = userId,
                ApplicationUser = userFromDb,
                firstName = model.orderHeader.firstName,
                lastName = model.orderHeader.lastName,
                address = model.orderHeader.address,
                PhoneNumber = model.orderHeader.PhoneNumber,
                OrderDate = DateTime.Now,
                OrderStatus = SD.Pending,
                PaymentStatus = SD.Pending,
                finalPrice = GetFinalPrice(model.shoppingList),
            };

            _unitOfWork.OrderHeader.Add(orderHeader);
            _unitOfWork.SaveChanges();

            // Post the shopping list item in order datails table
            List<OrderDatials> OrdersDatialsList = new List<OrderDatials>();
            foreach (var mobile in model.shoppingList)
            {
                OrderDatials order = new OrderDatials()
                {
                    OrderId = orderHeader.Id,
                    Count = mobile.count,
                    Price = mobile.Mobile.price,
                    ProductId = mobile.MobileId,
                    Mobile = mobile.Mobile,
                };
				OrdersDatialsList.Add(order);
            }

		     // add to database
            _unitOfWork.OrderDatials.addRange(OrdersDatialsList);
            _unitOfWork.SaveChanges();

			// apply stripe 
			#region Stripe Payment

			var domain = Request.Scheme + "://" + Request.Host.Value;
			var options = new SessionCreateOptions
			{
				SuccessUrl = domain + $"/Customer/ShoppingList/OrderConfrim/?id=" + orderHeader.Id,
				CancelUrl = domain + "/Customer/ShoppingList/OrderConfrim/?id=" + orderHeader.Id,
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
			};

			foreach (var item in model.shoppingList)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Mobile.price * 100), // $20.50 => 2050
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Mobile.name
						}
					},
					Quantity = item.count
				};
				options.LineItems.Add(sessionLineItem);
			}
			var stripeService = new SessionService();
			Session stripeSession = stripeService.Create(options);
            #endregion

            _unitOfWork.OrderHeader.UpdateSessionId(orderHeader, stripeSession.Id);

			// redirect to stripe page 
			Response.Headers.Add("Location", stripeSession.Url);
            return new StatusCodeResult(303);

        }
        public IActionResult OrderConfrim(int id)
        { 
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Order = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id);
            var OrderDatials = _unitOfWork.OrderDatials.GetAllWhere(OD => OD.OrderId == id);
            var ShopingCartItems = _unitOfWork.ShoppingList.GetAllWhere(s => s.ApplicationUserId == userId);
            

			// check if orders found
			if (Order == null || OrderDatials.Count() == 0)
                return NotFound();

            // Get session 
            var stripeSession = new SessionService().Get(Order.SessionId);

			// check Payment
            if(stripeSession == null || stripeSession.PaymentStatus != "paid")
            {
                _unitOfWork.OrderDatials.DeleteRange(OrderDatials);
                _unitOfWork.OrderHeader.Delete(Order);
                _unitOfWork.SaveChanges();
				TempData["Error"] = "You Should Pay To Make Order Shiped";
				return RedirectToAction(nameof(Index));
			}

            _unitOfWork.OrderHeader.UpdatePaymentId(Order, stripeSession.PaymentIntentId);

            Order.PaymentStatus = SD.Approved;
            Order.PaymentDate = DateTime.Now;
            Order.PaymentDueDate = DateTime.Now.AddMonths(1);
            _unitOfWork.ShoppingList.DeleteRange(ShopingCartItems);
            _unitOfWork.SaveChanges();

			TempData["Success"] = "Your Order Has Been Added Successfully";
            return RedirectToAction(nameof(Index));
        }
		private decimal GetFinalPrice(IEnumerable<ShoppingList> shoppingLists)
        {
            decimal finalprice = 0;
            foreach(var item in shoppingLists)
            {
                finalprice += (item.Mobile.price * item.count);
            }
            return finalprice;
        }
	}
}
