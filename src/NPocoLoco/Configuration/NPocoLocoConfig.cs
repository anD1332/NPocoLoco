using System.Configuration;
using log4net;

namespace NPocoLoco.Configuration
{
    public class NPocoLocoConfig : INPocoLocoConfig
    {
        private const string ConfigSectionName = "nPocoLocoConfigGroup/nPocoLocoSection";
        private static readonly ILog Log = LogManager.GetLogger(typeof (NPocoLocoConfig));
        
        public NPocoLocoSection GetConfigSection()
        {
            var configSection = ConfigurationManager.GetSection(ConfigSectionName) as NPocoLocoSection;

            if (configSection == null)
            {
                var errorMessage = "The NPocoLoco section is missing from the application configuration. Please refer to the NPocoLoco project page for example configuration.";

                Log.Fatal(errorMessage);

                throw new ConfigurationErrorsException(errorMessage);
            }

            return configSection;
        }
    }
}