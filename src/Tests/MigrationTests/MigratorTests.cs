using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataEntityTier;
using System.IO;

namespace MigrationTests
{
    [TestClass]
    public class MigratorTest
    {
        [TestMethod]
        public void MigratorBlank()
        {
            NHibernateSessionFactory.Migrate(new MigrationFileWriter());
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void MigratorNotExistingDirectory()
        {
            NHibernateSessionFactory.Migrate(new MigrationFileWriter(directory: "DoesntExists"));
        }
    }
}
