using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using System.IO;

namespace DataEntityTier
{
    public static class Migrator
    {
        public static void Migrate(SchemaUpdate schemaUpdate, IMigrationLogger migrationLogger, bool update_database = false)
        {
            schemaUpdate.Execute(migrationLogger.Log(), update_database);
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

        private string _path
        {
            get
            {
                string path = Path.Combine(_directory, _filename);
                string orig_name = Path.GetFileNameWithoutExtension(path);
                int i = 0;
                path = Path.Combine(_directory, String.Format("{0}.{1}.sql", orig_name, i));
                while (File.Exists(path))
                {
                    i++;
                    path = Path.Combine(_directory, String.Format("{0}.{1}.sql", orig_name, i));
                }
                return path;
            }
        }

        public MigrationFileWriter()
        {
            this._directory = ConfigurationManager.AppSettings["MigrationsFolder"];
            this._filename = DateTime.Now.ToString("yyyyMMdd") + ".sql";
        }

        public MigrationFileWriter(string directory = null, string filename = null)
            : base()
        {
            if (directory != null)
            {
                this._directory = directory;
            }

            if (filename != null)
            {
                this._filename = filename;
            }
        }

        private bool ValidateMigrationPath()
        {
            if (!Directory.Exists(this._directory))
            {
                // Heeft geen zin, ik kan niet testen of de database bestaat of niet.
                //throw new DirectoryNotFoundException("MigrationsFolder not found.");

                return false;
            }
            return true;
        }

        public Action<string> Log()
        {
            if (!ValidateMigrationPath()) return null;

            string path = _path;

            return (string s) =>
            {
                //if (!String.IsNullOrWhiteSpace(s))
                //{
                using (var sw = File.AppendText(path))
                {
                    sw.WriteLine(s);
                    sw.Close();
                }
                //}
            };
        }
    }
}
