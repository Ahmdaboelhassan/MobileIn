using Data.Models;
namespace Business.ViewModels
{
    public class ProcessorViewModel
    {
        public Processor processor { get; set; }

        public IEnumerable<Company>? Companies { get; set; }
    }
}
