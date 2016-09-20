using System.Collections.Generic;
using NPocoLoco.Models;

namespace NPocoLoco.Deployment
{
    public interface IDatabaseDeployment
    {
        void Run();
        IEnumerable<DeploymentScript> GetDeploymentScripts();
        void RunDeploymentScripts(IEnumerable<DeploymentScript> scripts);
        void RunDeploymentScript(DeploymentScript script);
        bool CheckForFailedScripts();
    }
}