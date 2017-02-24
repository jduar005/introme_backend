using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Intro.DataAccess.Connection
{
    public interface ILogger
    {
        void Debug(string s);
    }

    public class MongoRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        private readonly ILogger _log;
        private readonly MongoCollection<TEntity> collection;

        public MongoRepository() 
            : this(new MongoConnectionStringProvider(), null)
        {
        }

        public MongoRepository(IConnectionStringProvider connectionStringProvider, ILogger logger = null)
        {
            this._log = logger;
            this.collection = new MongoDbAccess(connectionStringProvider).GetCollection<TEntity>();
        }

        public virtual TEntity GetById(TKey id)
        {
            this._log?.Debug($"MONGO :: Getting entity '{id}' of type '{nameof(TEntity)}'");
            
            return this.collection.FindOneByIdAs<TEntity>(typeof(TEntity).GetTypeInfo().IsSubclassOf(typeof(Entity)) 
                        ? new ObjectId(id as string) 
                        : BsonValue.Create(id));
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            this._log?.Debug($"MONGO :: Getting all enities of type '{nameof(TEntity)}'");

            return this.collection.AsQueryable();
        }

        public virtual GeoNearResult<TEntity> GeoNear(GeoNearArgs nearArgs)
        {
            this._log?.Debug($"MONGO :: Performing Geo distance query [{nearArgs}]");

            return this.collection.GeoNear(nearArgs);
        }

        public virtual TEntity Add(TEntity entity)
        {
            this._log?.Debug($"MONGO :: Inserting entity '{entity.Id}' of type '{nameof(entity)}'");

            this.collection.Insert<TEntity>(entity);

            return entity;
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            this._log?.Debug($"MONGO :: Inserting multiple entities - '{nameof(entities)}'");

            this.collection.InsertBatch<TEntity>(entities);
        }

        public virtual TEntity Update(TEntity entity)
        {
            this.collection.Save<TEntity>(entity);

            return entity;
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.collection.Save<TEntity>(entity);
            }
        }

        public virtual void Delete(TKey id)
        {
            this.collection.Remove(typeof(TEntity).GetTypeInfo().IsSubclassOf(typeof(Entity))
                ? Query.EQ("_id", new ObjectId(id as string))
                : Query.EQ("_id", BsonValue.Create(id)));
        }

        public virtual void Delete(TEntity entity)
        {
            this.Delete(entity.Id);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in this.collection.AsQueryable().Where(predicate))
            {
                this.Delete(entity.Id);
            }
        }

        public virtual void DeleteAll()
        {
            this.collection.RemoveAll();
        }

        public virtual long Count()
        {
            return this.collection.Count();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return this.collection.AsQueryable().Any(predicate);
        }

        #region IQueryable<T>
        
        public virtual IEnumerator<TEntity> GetEnumerator()
        {
            return this.collection.AsQueryable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.collection.AsQueryable().GetEnumerator();
        }

        public virtual Type ElementType => this.collection.AsQueryable().ElementType;

        public virtual Expression Expression => this.collection.AsQueryable().Expression;

        public virtual IQueryProvider Provider => this.collection.AsQueryable().Provider;

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