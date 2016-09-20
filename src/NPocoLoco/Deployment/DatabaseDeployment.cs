using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using NPoco;
using NPocoLoco.Models;

namespace NPocoLoco.Deployment
{
    public class DatabaseDeployment : IDatabaseDeployment
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DatabaseDeployment));

        private readonly IDatabase _db;

        public DatabaseDeployment(IDatabase database)
        {
            _db = database;
        }

        public void Run()
        {
            Log.Info("Starting database deployment...");

            Initialize();

            var scripts = GetDeploymentScripts();
            RunDeploymentScripts(scripts);

            Log.Info("Database deployment successful");
        }

        public IEnumerable<DeploymentScript> GetDeploymentScripts()
        {
            var scriptNameRegex = new Regex(@"(.*)\.Scripts\.(?<name>\w+)\.sql$");

            var migrations = new List<DeploymentScript>();

            var assemblyName = "NPocoLoco.Tests.Unit";
            var resourceBaseName = "Resources";

            var assembly = Assembly.Load(assemblyName);

            var resources = assembly.GetManifestResourceNames().OrderBy(x => x).ToList();

            foreach (var resource in resources)
            {
                var regexMatch = scriptNameRegex.Match(resource);
                var name = regexMatch.Groups["name"].Value;

                string sql;

                var manifestResourceStream = assembly.GetManifestResourceStream(resource);

                using (var sr = new StreamReader(manifestResourceStream))
                {
                    sql = sr.ReadToEnd();
                }

                yield return new DeploymentScript(name, sql);
            }
        }

        public bool CheckForFailedScripts()
        {
            var failedScripts = _db.Query<MigrationHistory>("where Completed = 0");

            return failedScripts.Any();
        }

        public void RunDeploymentScripts(IEnumerable<DeploymentScript> scripts)
        {
            if (CheckForFailedScripts())
            {
                Log.Fatal("An error occurred when trying to process a previous migration");
                throw new Exception("There are failed deployments which need to be resolved before any more can be executed");
            }

            foreach (var deploymentScript in scripts)
            {
                RunDeploymentScript(deploymentScript);
            }
        }

        public void RunDeploymentScript(DeploymentScript script)
        {
            try
            {
                if (_db.Exists<MigrationHistory>(script.Name))
                {
                    Log.Debug($"Migration {script.Name} has already been executed");
                    return;
                }

                Log.Info($"Processing migration {script.Name}");

                _db.BeginTransaction();

                var migration = new MigrationHistory
                {
                    ScriptName = script.Name,
                    Completed = false,
                    DateProcessed = DateTime.Now
                };

                _db.Insert(migration);
                _db.CompleteTransaction();

                _db.Execute(script.Sql);
                migration.Completed = true;
                _db.Save(migration);
            }
            catch (Exception ex)
            {
                _db.AbortTransaction();
                Log.Error($"Failed to run deployment migration {script.Name}", ex);
                throw;
            }
        }

        internal void Initialize()
        {
            const string sql = @"IF (NOT EXISTS (   SELECT    *
                                                    FROM    INFORMATION_SCHEMA.TABLES
                                                    WHERE   TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'MigrationHistory'))
                                BEGIN
                                    CREATE TABLE dbo.MigrationHistory (
                                        ScriptName VARCHAR(255) NOT NULL,
                                        DateProcessed DATETIME NOT NULL,
                                        Completed BIT NOT NULL,
                                        CONSTRAINT PK_MigrationHistory PRIMARY KEY CLUSTERED ( ScriptName )
                                    );
                                END";

            _db.Execute(sql);
        }
    }
}