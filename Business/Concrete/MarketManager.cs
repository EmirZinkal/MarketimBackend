using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
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
    public class MarketManager : IMarketService
    {
        private readonly IMarketDal _marketDal;

        public MarketManager(IMarketDal marketDal)
        {
            _marketDal = marketDal;
        }
        [ValidationAspect(typeof(MarketValidator))]
        public IResult Add(Market market)
        {
            //ValidationTools.Validate(new MarketValidator(), market);
            market.CreatedAt = DateTime.Now;
            _marketDal.Add(market);
            return new SuccessResult(Messages.MarketAdded);
        }

        public IResult Delete(Market market)
        {
            _marketDal.Delete(market);
            return new SuccessResult(Messages.MarketDeleted);
        }

        public IDataResult<List<Market>> GetAll()
        {
            return new SuccessDataResult<List<Market>>(_marketDal.GetList().ToList());
        }

        public IDataResult<Market> GetById(int id)
        {
            return new SuccessDataResult<Market>(_marketDal.Get(u=>u.Id == id));
        }
        [ValidationAspect(typeof(MarketValidator))]
        public IResult Update(Market market)
        {
            _marketDal.Update(market);
            return new SuccessResult(Messages.MarketUpdated);
        }
    }
}
