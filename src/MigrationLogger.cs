using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using System.IO;

namespace DataEntityTier
{
    public static class Migrator
    {
        public static void Migrate(SchemaUpdate schemaUpdate, IMigrationLogger migrationLogger)
        {
            schemaUpdate.Execute(migrationLogger.Log(), false);
        }
    }

    public interface IMigrationLogger
    {
        Action<string> Log();
    }

    public sealed class MigrationFileWriter : IMigrationLogger
    {
        private string _directory { get; set; }
        private string _filename { get; set; }

        private string _path { get { return Path.Combine(_directory, _filename); } }

        public MigrationFileWriter()
        {
            this._directory = ConfigurationManager.AppSettings["MigrationsFolder"];
            this._filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".sql";
        }

        public MigrationFileWriter(string directory = null, string filename = null) : base()
        {
            if (directory != null)
            {
                this._directory = directory;
            }

            if (filename != null)
            {
                this._filename = filename;
            }

            Validate();
        }

        private void Validate()
        {
            if (!Directory.Exists(this._directory))
            {
                throw new DirectoryNotFoundException("MigrationsFolder not found.");
            }
        }

        public Action<string> Log()
        {
            return (string s) =>
            {
                if (!String.IsNullOrWhiteSpace(s))
                {
                    using (var sw = File.AppendText(_path))
                    {
                        sw.WriteLine(s);
                        sw.Close();
                    }
                }
            };
        }
    }
}
