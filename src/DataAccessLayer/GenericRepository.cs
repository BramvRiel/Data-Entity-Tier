using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataEntityTier.DataAccessLayer
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
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
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                using (ITransaction trans = session.Transaction)
                {
                    foreach (T item in items)
                        session.Save(item);

                    trans.Commit();
                }
            }
        }

        public void Update(params T[] items)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                using (ITransaction trans = session.Transaction)
                {
                    foreach (T item in items)
                        session.Update(item);

                    trans.Commit();
                }
            }
        }

        public void Remove(params T[] items)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                using (ITransaction trans = session.Transaction)
                {
                    foreach (T item in items)
                        session.Delete(item);

                    trans.Commit();
                }
            }
        }
    }
}