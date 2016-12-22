using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;
using System.Threading;

namespace DataEntityTier
{
    public class NHibernateSessionFactory
    {
        private static volatile ISessionFactory _sessionFactory;
        private static Mutex _mutex = new Mutex();

        public static ISession OpenSession()
        {
            if (_sessionFactory == null)
            {
                _mutex.WaitOne();

                if (_sessionFactory == null)
                {
                    _sessionFactory = _configuration.BuildSessionFactory();
                }

                _mutex.ReleaseMutex();
            }

            return _sessionFactory.OpenSession();
        }

        public static void Migrate()
        {
            MigrationLogger.Migrate(new SchemaUpdate(_configuration));
        }

        private static Configuration _configuration
        {
            get
            {
                var cfg = new Configuration();
                cfg.DataBaseIntegration(c =>
                {
                    c.ConnectionStringName = "connString";
                    c.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
                });

                var mapper = new ModelMapper();
                mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
                cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
                return cfg;
            }
        }
    }
}
