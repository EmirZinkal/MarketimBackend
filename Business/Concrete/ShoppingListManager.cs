using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ShoppingListManager : IShoppingListService
    {
        private readonly IShoppingListDal _shoppingListDal;

        public ShoppingListManager(IShoppingListDal shoppingListDal)
        {
            _shoppingListDal = shoppingListDal;
        }
        [ValidationAspect(typeof(ShoppingListValidator))]
        public IResult Add(ShoppingList shoppingList)
        {
            _shoppingListDal.Add(shoppingList);
            return new SuccessResult(Messages.ShoppingListAdded);
        }

        public IResult Delete(ShoppingList shoppingList)
        {
            _shoppingListDal.Delete(shoppingList);
            return new SuccessResult(Messages.ShoppingListDeleted);
        }

        public IDataResult<List<ShoppingList>> GetAll()
        {
            return new SuccessDataResult<List<ShoppingList>>(_shoppingListDal.GetList().ToList());
        }

        public IDataResult<ShoppingList> GetById(int id)
        {
            return new SuccessDataResult<ShoppingList>(_shoppingListDal.Get(u => u.Id == id));
        }
        [ValidationAspect(typeof(ShoppingListValidator))]
        public IResult Update(ShoppingList shoppingList)
        {
            _shoppingListDal.Update(shoppingList);
            return new SuccessResult(Messages.ShoppingListUpdated);
        }
    }
}
