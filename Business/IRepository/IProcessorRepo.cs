using Data.Models;

namespace Business.IRepository
{
    public interface IProcessorRepo : IRepository<Processor>
    {
        void Update(Processor Item);
    }
}
