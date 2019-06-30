using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GenericRepository;
using LinqToDB;
using zbW.ProjNuGet.Model;
using System.Linq.Expressions;
using MySql.Data;

namespace zbW.ProjNuGet.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : ModelBase, new()
    {
        
        public virtual string ConnectionString { get; set; }
        protected string ProviderName = "MySql.Data.MySqlClient";
        protected string RepositoryName = "Semesterprojekt";

        protected RepositoryBase(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public T GetSingle<P>(P pkValue)
        {
            var result = new T();
            using (var ctx = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    result = (from e in ctx.GetTable<T>() where e.Id.Equals(pkValue) select e).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        public void Add(T entity)
        {
            using (var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    db.Insert<T>(entity);
                    db.BeginTransaction();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void Delete(T entity)
        {
            using (var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    var foundRowToEntityKey = (from e in db.GetTable<T>() where e.Id.Equals(entity.Id) select e).FirstOrDefault();
                    if (foundRowToEntityKey != null)
                    {
                        db.Delete<T>(foundRowToEntityKey);
                    }
                    db.BeginTransaction();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        
        public void Update(T entity)
        {
            using(var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    db.Update<T>(entity);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        
        public IQueryable<T> GetAll(Expression<Func<T, bool>> whereClause)
        {
            IQueryable<T> result = Enumerable.Empty<T>().AsQueryable();
            using (var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    result = db.GetTable<T>().Where<T>(whereClause);
                    
                }
                catch (Exception e)
                {
                    throw e;
                }
                return result;
            }
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> result = Enumerable.Empty<T>().AsQueryable();
            using (var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    result = db.GetTable<T>();
                }
                catch (Exception e)
                {
                    throw e;
                }
                return result;
            }
        }
   
        public virtual IQueryable<T> Query(string whereCondition, Dictionary<string, object> parameterValues)
        {
            throw new NotImplementedException();
        }

        public long Count(Expression<Func<T, bool>> whereClause)
        {
            IQueryable<T> result = Enumerable.Empty<T>().AsQueryable();
            using (var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    result = db.GetTable<T>().Where<T>(whereClause);
                }
                catch (Exception e)
                {
                    throw e;
                }
                return result.Count();
            }
        }


        public virtual long Count()
        {
            IQueryable<T> result = Enumerable.Empty<T>().AsQueryable();
            using (var db = new LinqToDB.DataContext(ProviderName, ConnectionString))
            {
                try
                {
                    result = db.GetTable<T>();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result.Count();

        }

        //public abstract string TableName { get; }

        //public abstract string Order { get; }

        //public abstract T CreateEntry(IDataReader reader);

    }
}
