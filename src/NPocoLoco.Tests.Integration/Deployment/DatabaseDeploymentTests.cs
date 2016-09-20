using System.Configuration;
using System.Data.SqlClient;
using FluentAssertions;
using NUnit.Framework;

namespace NPocoLoco.Tests.Integration.Deployment
{
    public class DatabaseDeploymentTests
    {
        private readonly string ConnectionString =
           ConfigurationManager.ConnectionStrings["NPocoIntegrationConnString"].ConnectionString;

        private string GetInformationSchemaTableCheckFor(string tableName)
        {
            string sql = $@"SELECT  COUNT(*)
                            FROM    INFORMATION_SCHEMA.TABLES
                            WHERE   TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{tableName}'";

            return sql;
        }

        private bool TableExistsInTableSchema(string tableName, out int count)
        {
            using(var connection = new SqlConnection(ConnectionString))
            {
                using(var command = new SqlCommand(GetInformationSchemaTableCheckFor(tableName), connection))
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                    connection.Close();
                }
            }

            return count > 0;
        }

        [Test]
        public void GivenDeploymentRuns_MigrationHistoryTableExists()
        {
            var count = 0;

            var exists = TableExistsInTableSchema("MigrationHistory", out count);

            exists.Should().BeTrue();
            count.Should().Be(1);
        }

        [Test]
        public void GivenDeploymentRuns_TestTable1Exists()
        {
            var count = 0;

            var exists = TableExistsInTableSchema("TestTable1", out count);

            exists.Should().BeTrue();
            count.Should().Be(1);
        }

        [Test]
        public void GivenDeploymentRuns_TestTable2Exists()
        {
            var count = 0;

            var exists = TableExistsInTableSchema("TestTable2", out count);

            exists.Should().BeTrue();
            count.Should().Be(1);
        }
    }
}