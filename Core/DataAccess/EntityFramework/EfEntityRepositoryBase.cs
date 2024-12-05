using Castle.Components.DictionaryAdapter.Xml;
using Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

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
                //var entity = context.Set<TEntity>().SingleOrDefault(filter);

                //var status = typeof(TEntity).GetProperty("Status").GetValue(entity);
                //if (status == null || status.Equals(false))
                //{
                //    return null;
                //}

                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
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