using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NPoco;
using NPocoLoco.Configuration;
using NPocoLoco.Deployment;
using NPocoLoco.Models;
using NUnit.Framework;

namespace NPocoLoco.Tests.Unit.Deployment
{
    public class DatabaseDeploymentTests
    {
        [Test]
        public void GetDeploymentScripts_Returns_TWO_DeploymentScripts()
        {
            var configSection = new NPocoLocoSection {Connection = "", ResourcesAssemblyName = "NPocoLoco.Tests.Unit"};

            var mockDb = new Mock<IDatabase>();

            var mockConfig = new Mock<INPocoLocoConfig>();
            mockConfig.Setup(x => x.GetConfigSection()).Returns(configSection);

            var databaseDeployment = new DatabaseDeployment(mockDb.Object, mockConfig.Object);

            var result = databaseDeployment.GetDeploymentScripts().ToList();
            result.Count().Should().Be(2);
            result[0].Name.Should().Be("201609202014_CreateTestTable1");
            result[1].Name.Should().Be("201609202015_CreateTestTable2");
        }

        [Test]
        public void GivenThereAreFailedScripts_WhenRunDeploymentScripts_ExceptionIsThrown()
        {
            var failedList = new List<MigrationHistory>
            {
                new MigrationHistory {Completed = false, DateProcessed = DateTime.Now, ScriptName = "TestFailed"}
            };

            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(x => x.Query<MigrationHistory>(It.IsAny<string>())).Returns(failedList);

            var mockConfig = new Mock<INPocoLocoConfig>();

            var deployment = new DatabaseDeployment(mockDb.Object, mockConfig.Object);

            Action action = () => deployment.RunDeploymentScripts(new List<DeploymentScript>());

            action.ShouldThrow<Exception>()
                .And.Message.Should()
                .Be("There are failed deployments which need to be resolved before any more can be executed");
        }

        [Test]
        public void GivenAMigrationHasAlreadyRun_WhenRunDeploymentScript_NoDatabaseInteractionIsInvoked()
        {
            var script = new DeploymentScript("Test", "Script");

            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(x => x.Exists<MigrationHistory>(It.IsAny<string>())).Returns(true);

            var mockConfig = new Mock<INPocoLocoConfig>();

            var deployment = new DatabaseDeployment(mockDb.Object, mockConfig.Object);
            deployment.RunDeploymentScript(script);

            mockDb.Verify(x => x.BeginTransaction(), Times.Never);
        }

        [Test]
        public void GivenRunDeploymentScriptFails_ExceptionShouldBeCaught()
        {
            var script = new DeploymentScript("Test", "Script");

            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(x => x.Exists<MigrationHistory>(It.IsAny<string>())).Returns(false);
            mockDb.Setup(x => x.Insert(It.IsAny<MigrationHistory>())).Throws<Exception>();

            var mockConfig = new Mock<INPocoLocoConfig>();

            var deployment = new DatabaseDeployment(mockDb.Object, mockConfig.Object);

            Action action = () => deployment.RunDeploymentScript(script);

            action.ShouldThrow<Exception>();
        }

        [Test]

        public void GivenRunningDeploymentScriptFails_TransactionIsAborted()
        {
            var script = new DeploymentScript("Test", "Script");

            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(x => x.Exists<MigrationHistory>(It.IsAny<string>())).Returns(false);
            mockDb.Setup(x => x.Insert(It.IsAny<MigrationHistory>())).Throws<Exception>();
            mockDb.Setup(x => x.AbortTransaction()).Verifiable();

            var mockConfig = new Mock<INPocoLocoConfig>();

            var deployment = new DatabaseDeployment(mockDb.Object, mockConfig.Object);

            Action action = () => deployment.RunDeploymentScript(script);

            action.ShouldThrow<Exception>();

            mockDb.Verify(x => x.AbortTransaction(), Times.Once);
        }
    }
}