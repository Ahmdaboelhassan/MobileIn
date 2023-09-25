using Data.Models;
using MobileIn.Data;

namespace Business.IRepository.Repoistory
{
	public class OrderDatailsRepo : Repository<OrderDatials> , IOrderDatialsRepo
	{
		readonly ApplicationDbContext _context;

		public OrderDatailsRepo(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		
	}
}
