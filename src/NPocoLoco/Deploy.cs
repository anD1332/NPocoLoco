using NPoco;
using NPocoLoco.Configuration;
using NPocoLoco.Deployment;

namespace NPocoLoco
{
    public static class Deploy
    {
        public static void Run()
        {
            var configuration = new NPocoLocoConfig();

            var database = new Database(configuration.GetConfigSection().Connection);
            
            var deployment = new DatabaseDeployment(database, configuration);
            deployment.Run();
        }
    }
}