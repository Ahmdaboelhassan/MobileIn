using Data.Models;
using MobileIn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepository.Repoistory
{
    public class CompanyRepo : Repository<Company>, ICompanyRepo
    {
        readonly ApplicationDbContext _context;

        public CompanyRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Company Item)
        {
            _context.companies.Update(Item);
        }
    }
}
