using Data.Models;
using MobileIn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepository.Repoistory
{
	public class ShoppingListRepo : Repository<ShoppingList>, IShoppingListRepo
	{
		private readonly ApplicationDbContext _db;

		public ShoppingListRepo(ApplicationDbContext db) : base(db) 
		{
			_db = db;
		}
        public void Update(ShoppingList shoppingList)
        {
            _db.Update(shoppingList);
        }
  

        public void PlusByOne(ShoppingList shoppingList)
        {
            shoppingList.count += 1;
            Update(shoppingList);
        }

		public void MinusByOne(ShoppingList shoppingList)
		{
			shoppingList.count -= 1;
			Update(shoppingList);
		}


	}
}
