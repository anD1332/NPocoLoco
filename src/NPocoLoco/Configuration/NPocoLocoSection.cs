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

        [ConfigurationProperty("resourceInformation")]
        public NPocoLocoResources ResourceInformation
        {
            get { return this["resourceInformation"] as NPocoLocoResources; }
            set { this["resourceInformation"] = value; }
        }

        public class NPocoLocoResources : ConfigurationElement
        {
            //[ConfigurationProperty("resourcesBaseName", IsRequired = true)]
            //public string ResourcesBaseName
            //{
            //    get { return this["resourcesBaseName"] as string; }
            //    set { this["resourcesBaseName"] = value; }
            //}

            [ConfigurationProperty("resourcesAssemblyName", IsRequired = true)]
            public string ResourcesAssemblyName
            {
                get { return this["resourcesAssemblyName"] as string; }
                set { this["resourcesAssemblyName"] = value; }
            }
        }
    }
}