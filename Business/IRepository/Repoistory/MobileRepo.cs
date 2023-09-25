using Data.Models;
using Microsoft.EntityFrameworkCore;
using MobileIn.Data;

namespace Business.IRepository.Repoistory
{
    public class MobileRepo : Repository<Mobile> , IMobileRepo
    {
        readonly ApplicationDbContext _context;

        public MobileRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Mobile> GetAllOrderDesc(string[]? include = null)
        {
            IEnumerable<Mobile> Mobiles = _context.mobiles.Take(10).OrderByDescending(m => m.yearRelease).ToList();
            if(include != null)
            {
                foreach(string item in include)
                {
                    Mobiles = _context.mobiles.Include(item)
                        .OrderByDescending(m => m.id).ToList();
                }
            }
            return Mobiles;
        }

        public void Update(Mobile Item)
        {
            _context.mobiles.Update(Item);
        }


    }
}
