using System;
using MySqlConnector;

namespace GameSever.Utils
{
    public static class MySqlConnectionString
    {
        public static string Get()
        {
            var env = EnvironmentParameters.GetEnvironmentVariables();
            if (!env.HasValue)
            {
                throw new Exception("Missing environment variables");
            }
            
            return new MySqlConnectionStringBuilder
            {
                Server = env.Value.MySqlServer,
                UserID = env.Value.MySqlUserId,
                Password = env.Value.MySqlPassword,
                Database = env.Value.MySqlDatabase
            }.ConnectionString;
        }
    }
}