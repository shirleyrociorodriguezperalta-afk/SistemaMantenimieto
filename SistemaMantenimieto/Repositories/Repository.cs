using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaMantenimiento.Data;

namespace SistemaMantenimiento.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll() => _dbSet.ToList();

        public virtual T? GetById(int id) => _dbSet.Find(id);

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).ToList();

        public virtual void Add(T entity) => _dbSet.Add(entity);

        public virtual void Update(T entity) => _dbSet.Update(entity);

        public virtual void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual void Save() => _context.SaveChanges();
    }
}