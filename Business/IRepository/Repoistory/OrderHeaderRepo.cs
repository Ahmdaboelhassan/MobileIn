using Data.Models;
using MobileIn.Data;


namespace Business.IRepository.Repoistory
{
	internal class OrderHeaderRepo : Repository<OrderHeader> , IOrderHeaderRepo
	{
		readonly ApplicationDbContext _context;

		public OrderHeaderRepo(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

        public void Update(OrderHeader orderHeader)
        {
            _context.Update(orderHeader);
        }

        public void UpdatePaymentId(OrderHeader orderHeader, string newpaymentId)
		{
			orderHeader.PaymentIntendId = newpaymentId;
			_context.Update(orderHeader);
			_context.SaveChanges();
		}

		public void UpdateSessionId(OrderHeader orderHeader, string SessionId)
		{
			orderHeader.SessionId = SessionId;
			_context.Update(orderHeader);
			_context.SaveChanges();
		}
	}
}
