using Core.Utilities.Results;
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
        IResult Add(ShoppingList shoppingList);
        IResult Update(ShoppingList shoppingList);
        IResult Delete(ShoppingList shoppingList);
        IDataResult<ShoppingList> GetById(int id);
        IDataResult<List<ShoppingList>> GetAll();
    }
}
