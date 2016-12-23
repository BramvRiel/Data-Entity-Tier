﻿using NHibernate;
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

        public static void Migrate(IMigrationLogger MigrationLogger)
        {
            Migrator.Migrate(new SchemaUpdate(_configuration), MigrationLogger);
        }

        private static Configuration _configuration
        {
            get
            {
                var cfg = new Configuration();
                cfg.DataBaseIntegration(c =>
                {
                    // Console
                    c.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["connString"];
                    // Web
                    c.ConnectionStringName = "connString";
                    // Dialect
                    c.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
                });

                var mapper = new ModelMapper();
                string MappingNamespaces = System.Configuration.ConfigurationManager.AppSettings["MappingNamespaces"];
                if (!string.IsNullOrWhiteSpace(MappingNamespaces))
                {
                    foreach (string MappingNamespace in MappingNamespaces.Split(','))
                    {
                        mapper.AddMappings(Assembly.Load(MappingNamespace).GetExportedTypes());
                    }
                }
                mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
                cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
                return cfg;
            }
        }
    }
}
