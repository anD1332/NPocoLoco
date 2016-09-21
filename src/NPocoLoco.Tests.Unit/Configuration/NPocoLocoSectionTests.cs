using System.Configuration;
using FluentAssertions;
using NPocoLoco.Configuration;
using NUnit.Framework;

namespace NPocoLoco.Tests.Unit.Configuration
{
    public class NPocoLocoSectionTests
    {
        [Test]
        public void GivenNPocoLocoConfiguration_WhenGetNPocoConnectionStringName_CorrectValueIsReturned()
        {
            var config = (NPocoLocoSection)ConfigurationManager.GetSection("nPocoLocoConfigGroup/nPocoLocoSection");

            config.Connection.Should().Be("NPocoLocoConnectionString");
        }

        [Test]
        public void GivenNPocoLocoConfiguration_WhenGetResourcesAssemblyName_CorrectValueIsReturned()
        {
            var config = (NPocoLocoSection)ConfigurationManager.GetSection("nPocoLocoConfigGroup/nPocoLocoSection");

            config.ResourcesAssemblyName.Should().Be("NPocoLoco.Tests.Unit");
        }

        //[Test]
        //public void GivenNPocoLocoConfiguration_WhenGetResourcesBaseName_CorrectValueIsReturned()
        //{
        //    var config = (NPocoLocoSection)ConfigurationManager.GetSection("nPocoLocoConfigGroup/nPocoLocoSection");

        //    config.ResourceInformation.ResourcesBaseName.Should().Be("Resources");
        //}
    }
}