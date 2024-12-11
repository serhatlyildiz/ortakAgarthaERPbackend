using Core.Entities;
using Core.Entities.Concrete;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var addedEntity = context.Add(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var existingEntity = context.Set<TEntity>().FirstOrDefault(e => e == entity);

                if (existingEntity != null)
                {
                    var entry = context.Entry(existingEntity);
                    entry.Property("Status").CurrentValue = false;
                    context.SaveChanges();
                }
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                if (typeof(TEntity) == typeof(Users))
                {
                    return context.Set<TEntity>().SingleOrDefault(filter);
                }
                else
                {
                    Expression<Func<TEntity, bool>> statusCondition = x => EF.Property<bool>(x, "Status") == true;
                    var combinedFilter = filter.And(statusCondition);
                    return context.Set<TEntity>().SingleOrDefault(combinedFilter);
                }
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {

                if (typeof(TEntity) == typeof(Users))
                {
                    return filter == null
                       ? context.Set<TEntity>().ToList()
                       : context.Set<TEntity>().Where(filter).ToList();
                }
                else
                {
                    Expression<Func<TEntity, bool>> statusCondition = x => EF.Property<bool>(x, "Status") == true;
                    if (filter != null)
                    {
                        var combinedFilter = filter.And(statusCondition);
                        return context.Set<TEntity>().Where(combinedFilter).ToList();
                    }
                    else
                    {
                        return context.Set<TEntity>().Where(statusCondition).ToList();
                    }
                }
            }
        }

        public void HardDelete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void Restore(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var existingEntity = context.Set<TEntity>().FirstOrDefault(e => e == entity);

                if (existingEntity != null)
                {
                    var entry = context.Entry(existingEntity);
                    entry.Property("Status").CurrentValue = true;
                    context.SaveChanges();
                }
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}