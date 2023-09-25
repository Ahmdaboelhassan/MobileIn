using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<Mobile> mobiles { get; set; }
        public IEnumerable<Mobile> latestMobile {  get; set; }
    }
}
