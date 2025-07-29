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
        void Add(Market market);
        void Update(Market market);
        void Delete(Market market);
        Market GetById(int id);
        List<Market> GetAll();
    }
}
