using Data.Models;


namespace Business.ViewModels
{
    public class MobileViewModel
    {
        public Mobile Mobile {get; set;}
        public IEnumerable<Company>? Companies { get; set;}
        public IEnumerable<Processor>? Processors { get; set;}
    }
}
