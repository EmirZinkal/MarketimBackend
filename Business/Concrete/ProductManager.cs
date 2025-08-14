using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager:IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IMarketDal _marketDal;

        public ProductManager(IProductDal productDal, IMarketDal marketDal)
        {
            _productDal = productDal;
            _marketDal = marketDal;
        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {

            var marketExists = _marketDal.Get(m => m.Id == product.MarketId);
            if (marketExists == null)
            {
                return new ErrorResult(Messages.MarketNotFound);
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            // Ürün var mı?
            var productExists = _productDal.Get(p => p.Id == product.Id);
            if (productExists == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            // Aynı markette aynı isimde başka ürün var mı?
            var existingProduct = _productDal.Get(p => p.Name == product.Name && p.MarketId == product.MarketId && p.Id != product.Id);
            if (existingProduct != null)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExistsInMarket);
            }

            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        public IResult Delete(Product product)
        {
            var existingProduct = _productDal.Get(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return new ErrorResult(Messages.ProductNotFound);
            }

            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IDataResult<Product> GetById(int id)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.Id == id));
        }

        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
        }
    }
}
