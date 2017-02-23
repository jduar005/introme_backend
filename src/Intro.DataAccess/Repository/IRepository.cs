using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Driver;

using Intro.Domain.PersistentModels;

namespace Intro.DataAccess.Repository
{
    public interface IRepository<TEntity, in TKey> : IQueryable<TEntity> where TEntity : IEntity<TKey>
    {
        TEntity GetById(TKey id);

        IQueryable<TEntity> GetAll();

        GeoNearResult<TEntity> GeoNear(GeoNearArgs nearArgs);

        TEntity Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);

        void Delete(TKey id);
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> predicate);
        void DeleteAll();

        long Count();

        bool Exists(Expression<Func<TEntity, bool>> predicate);
    }

    public interface IRepository<T> : IRepository<T, string> where T : IEntity<string> { }
}
