using Data.Models;
using Microsoft.EntityFrameworkCore;
using MobileIn.Data;
using System.Linq.Expressions;

namespace Business.IRepository.Repoistory;

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _set;
    

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

    public IEnumerable<T> GetAll(string[]? Include = null)
    {
        IQueryable<T> list = _set;

        if(Include is not null)
        {
            foreach(var item in Include)
            {
                list = list.Include(item);
            }
        }
        return list.ToList();
    }

    public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> filter, string[]? Include =null)
    {
        IQueryable<T> list = _set.Where(filter);

        if(Include is not null)
        {
          foreach(var item in Include)
            {
              list = list.Include(item);
            }
        }
        return list.ToList();
    }


    public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string[]? Include)
    {
        IQueryable<T> list = _set.Where(filter);

        if (Include is not null)
        {
            foreach (var itm in Include)
            {
               list =  list.Include(itm);
            }
        }
        return list.FirstOrDefault();
    }

    public void Delete(T item)
    {
       _set.Remove(item);
    }

    public void Add(T item)
    {
        _set.Add(item);
    }

	public void DeleteRange(IEnumerable<T> items)
	{
		_set.RemoveRange(items);
	}

	public void addRange(List<T> ordres)
	{
        _set.AddRange(ordres);
	}
}
