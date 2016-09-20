using System.Configuration;
using System.Data.SqlClient;
using NPoco;
using NPocoLoco.Deployment;
using NUnit.Framework;

namespace NPocoLoco.Tests.Integration
{
    [SetUpFixture]
    public class InitialSetup
    {
        private const string DatabaseName = "NPocoLocoIntegration";

        private const string CreateSql = @"IF NOT (EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE NAME = N'NPocoLocoIntegration'))
                                            BEGIN
                                                CREATE DATABASE NPocoLocoIntegration;
                                            END;";

        private const string DropSql = @"IF (EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE NAME = N'NPocoLocoIntegration'))
                                        BEGIN
                                            ALTER DATABASE NPocoLocoIntegration SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                            DROP DATABASE NPocoLocoIntegration;
                                        END;";

        private const string SetupUserSql = @"USE NPocoLocoIntegration;
                                                CREATE USER NPocoLocoUser FOR LOGIN NPocoLocoUser;
                                                EXEC sp_addrolemember 'db_owner', 'NPocoLocoUser';";

        [OneTimeSetUp]
        public void CreateDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["InitialSetupConnString"].ConnectionString;

            using(var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using(var dropDatabaseCommand = new SqlCommand(DropSql, conn))
                {
                    dropDatabaseCommand.ExecuteNonQuery();
                }

                using(var createDatabaseCommand = new SqlCommand(CreateSql, conn))
                {
                    createDatabaseCommand.ExecuteNonQuery();
                }

                using(var setupUserCommand = new SqlCommand(SetupUserSql, conn))
                {
                    setupUserCommand.ExecuteNonQuery();
                }

                conn.Close();
            }

            var database = new Database(DatabaseName);

            var deployment = new DatabaseDeployment(database);
            deployment.Run();
        }

        [OneTimeTearDown]
        public void FullTearDown()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["InitialSetupConnString"].ConnectionString;

            using(var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using(var dropDatabaseCommand = new SqlCommand(DropSql, conn))
                {
                    dropDatabaseCommand.ExecuteNonQuery();
                }

                conn.Close();
            }
        }
    }
}