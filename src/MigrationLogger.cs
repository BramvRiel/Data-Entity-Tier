using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using System.IO;

namespace DataEntityTier
{
    public static class MigrationLogger
    {
        public static void Migrate(SchemaUpdate schemaUpdate)
        {
            string dirname = ConfigurationManager.AppSettings["NHibernateSQLMigrations"];

            if (dirname == null)
            {
                throw new ArgumentNullException("AppSetting NHibernateSQLMigrations not found.");
            }
            if (!Directory.Exists(dirname))
            {
                throw new DirectoryNotFoundException("Directory NHibernateSQLMigrations not found.");
            }

            string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".sql";
            string path = Path.Combine(dirname, filename);

            Action<string> LogSql = (string s) =>
            {
                if (!String.IsNullOrWhiteSpace(s))
                {
                    using (var sw = File.AppendText(path))
                    {
                        sw.WriteLine(s);
                        sw.Close();
                    }
                }
            };

            try
            {
                schemaUpdate.Execute(LogSql, false);
            }
            catch
            {
                throw new Exception("Failed to write migration log.");
            }
        }
    }
}
