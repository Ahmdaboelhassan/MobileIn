using Data.Models;
using MobileIn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepository.Repoistory
{
    public class ProcessorRepo : Repository<Processor> , IProcessorRepo
    {
        readonly ApplicationDbContext _context;

        public ProcessorRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Processor Item)
        {
            _context.processors.Update(Item);
        }

      
    }
}
