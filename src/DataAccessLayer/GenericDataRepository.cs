using DataEntityTier.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataEntityTier.DataAccessLayer
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                IQueryable<T> query = session.Query<T>();

                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    query = query.Fetch(navigationProperty);
                
                list = query.ToList();
            }
            return list;
        }

        public virtual IList<T> GetList(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                IQueryable<T> query = session.Query<T>();

                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    query.Fetch(navigationProperty);

                list = query
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingle(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                IQueryable<T> query = session.Query<T>();

                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    query.Fetch(navigationProperty);

                item = query
                    .Where(where)
                    .SingleOrDefault();
            }
            return item;
        }

        public void Add(params T[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params T[] items)
        {
            throw new NotImplementedException();
        }

        public void Remove(params T[] items)
        {
            throw new NotImplementedException();
        }
    }
}