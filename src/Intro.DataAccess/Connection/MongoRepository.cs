using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Intro.DataAccess.Connection
{
    public class MongoRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        protected internal MongoCollection<TEntity> Collection;

        public MongoRepository() : this(new MongoConnectionStringProvider())
        {
        }

        public MongoRepository(IConnectionStringProvider connectionStringProvider)
        {
            this.Collection = new MongoDbAccess(connectionStringProvider).GetCollection<TEntity>();
        }

        public virtual TEntity GetById(TKey id)
        {
            return this.Collection.FindOneByIdAs<TEntity>(typeof(TEntity).IsSubclassOf(typeof(Entity)) 
                        ? new ObjectId(id as string) 
                        : BsonValue.Create(id));
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return this.Collection.AsQueryable();
        }

        public virtual TEntity Add(TEntity entity)
        {
            this.Collection.Insert<TEntity>(entity);

            return entity;
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            this.Collection.InsertBatch<TEntity>(entities);
        }

        public virtual TEntity Update(TEntity entity)
        {
            this.Collection.Save<TEntity>(entity);

            return entity;
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Collection.Save<TEntity>(entity);
            }
        }

        public virtual void Delete(TKey id)
        {
            this.Collection.Remove(typeof(TEntity).IsSubclassOf(typeof(Entity))
                ? Query.EQ("_id", new ObjectId(id as string))
                : Query.EQ("_id", BsonValue.Create(id)));
        }

        public virtual void Delete(TEntity entity)
        {
            this.Delete(entity.Id);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in this.Collection.AsQueryable().Where(predicate))
            {
                this.Delete(entity.Id);
            }
        }

        public virtual void DeleteAll()
        {
            this.Collection.RemoveAll();
        }

        public virtual long Count()
        {
            return this.Collection.Count();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Collection.AsQueryable().Any(predicate);
        }

        #region IQueryable<T>
        
        public virtual IEnumerator<TEntity> GetEnumerator()
        {
            return this.Collection.AsQueryable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Collection.AsQueryable().GetEnumerator();
        }

        public virtual Type ElementType
        {
            get { return this.Collection.AsQueryable().ElementType; }
        }

        public virtual Expression Expression
        {
            get { return this.Collection.AsQueryable().Expression; }
        }

        public virtual IQueryProvider Provider
        {
            get { return this.Collection.AsQueryable().Provider; }
        }

        #endregion
    }

    public class MongoRepository<T> : MongoRepository<T, string>, IRepository<T> where T : IEntity<string>
    {
        public MongoRepository()
            : this(new MongoConnectionStringProvider())
        {
        }

        public MongoRepository(IConnectionStringProvider connectionStringProvider) 
            : base(connectionStringProvider)
        {
        }
    }
}