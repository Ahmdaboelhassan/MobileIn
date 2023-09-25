
using Data.Models;

namespace Business.IRepository
{
	public interface IShoppingListRepo:IRepository<ShoppingList>
	{
		void Update(ShoppingList shoppingList);
		void PlusByOne(ShoppingList shoppingList);
		void MinusByOne(ShoppingList shoppingList);
	}
}
