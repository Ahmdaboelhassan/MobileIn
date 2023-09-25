using Data.Models;

namespace Business.IRepository
{
    public interface ICompanyRepo: IRepository<Company>
    {
        void Update(Company Item);
    }
}
