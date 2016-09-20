namespace NPocoLoco.Models
{
    public class DeploymentScript
    {
        public DeploymentScript(string name, string sql)
        {
            Name = name;
            Sql = sql;
        }

        public DeploymentScript()
        {
        }

        public string Name { get; set; }
        public string Sql { get; set; }    
    }
}