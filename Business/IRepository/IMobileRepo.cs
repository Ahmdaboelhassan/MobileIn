using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepository
{
    public interface IMobileRepo : IRepository<Mobile>
    {
        void Update(Mobile Item);

        IEnumerable<Mobile> GetAllOrderDesc(string[]? include = null);
    }
}
