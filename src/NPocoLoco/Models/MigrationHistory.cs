using System;
using System.ComponentModel;
using NPoco;

namespace NPocoLoco.Models
{
    [TableName("MigrationHistory")]
    [PrimaryKey("ScriptName", AutoIncrement = false)]
    public class MigrationHistory
    {
        public string ScriptName { get; set; }
        public DateTime DateProcessed { get; set; }
        public bool Completed { get; set; }
    }
}