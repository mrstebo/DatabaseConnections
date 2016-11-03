using System.Collections.Generic;

namespace DatabaseConnections
{
    public class DatabaseCommand
    {
        public DatabaseCommand()
        {
            Parameters = new List<DbParam>();
        }

        public string CommandText { get; set; }
        public IList<DbParam> Parameters { get; set; }
    }
}