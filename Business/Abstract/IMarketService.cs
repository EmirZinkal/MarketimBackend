using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMarketService
    {
        IResult Add(Market market);
        IResult Update(Market market);
        IResult Delete(Market market);
        IDataResult<Market> GetById(int id);
        IDataResult<List<Market>> GetAll();
    }
}
