using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepository
{
    public interface IUnitOfWork
    {

        public ICompanyRepo Companies { get; }
        public IProcessorRepo Processors { get; }
        public IMobileRepo Mobiles { get; }
        public IShoppingListRepo ShoppingList { get; }
        public IApplicationUserRepo ApplicationUser { get; }
        public IOrderDatialsRepo OrderDatials { get; }
        public IOrderHeaderRepo OrderHeader { get; }
        void SaveChanges();

    }
}
