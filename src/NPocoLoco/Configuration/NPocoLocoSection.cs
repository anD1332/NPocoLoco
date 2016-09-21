using System;
using System.Configuration;

namespace NPocoLoco.Configuration
{
    public class NPocoLocoSection : ConfigurationSection
    {
        [ConfigurationProperty("connection", IsRequired = true)]
        public string Connection
        {
            get { return this["connection"] as string; }
            set { this["connection"] = value; }
        }

        [ConfigurationProperty("resourcesAssemblyName", IsRequired = true)]
        public string ResourcesAssemblyName
        {
            get { return this["resourcesAssemblyName"] as string; }
            set { this["resourcesAssemblyName"] = value; }
        }
    }
}