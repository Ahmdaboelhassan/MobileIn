using Data.Models;
using MobileIn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepository.Repoistory
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Companies = new CompanyRepo(_context);
            Processors = new ProcessorRepo(_context);
            Mobiles = new MobileRepo(_context);
            ShoppingList = new ShoppingListRepo(_context);
            ShoppingList = new ShoppingListRepo(_context);
			ApplicationUser = new ApplicationUserRepo(_context);
			OrderDatials = new OrderDatailsRepo(_context);
			OrderHeader = new OrderHeaderRepo(_context);
		}

        public ICompanyRepo Companies { get; private set; }

        public IProcessorRepo Processors { get; private set; }

        public IMobileRepo Mobiles{ get; private set; }
        public IShoppingListRepo ShoppingList { get; private set; }
		public IApplicationUserRepo ApplicationUser { get; private set; }
		public IOrderDatialsRepo OrderDatials { get; private set; }
		public IOrderHeaderRepo OrderHeader { get; private set; }

		public void SaveChanges()
        {
          _context.SaveChanges();
        }
    }
}
