using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.IRepository
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(string[]? Include =null);
        public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> filter, string[]? Include = null);
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string[]? Include =null);
        public void Delete(T item);
        public void DeleteRange(IEnumerable<T> item);
        public void Add(T item);
        public void addRange(List<T> ordres);


	}
}
