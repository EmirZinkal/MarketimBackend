using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IShoppingListService
    {
        void Add(ShoppingList shoppingList);
        void Update(ShoppingList shoppingList);
        void Delete(ShoppingList shoppingList);
        ShoppingList GetById(int id);
        List<ShoppingList> GetAll();
    }
}
