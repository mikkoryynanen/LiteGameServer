// sing NuGet package https://github.com/tonerdo/dotnet-env

using System;

namespace GameSever.Utils
{
    public struct EnvironmentVariables
    {
        public ushort Port;
        public int MaxClients;

        public string MySqlServer;
        public string MySqlUserId;
        public string MySqlPassword;
        public string MySqlDatabase;
    }
    
    public class EnvironmentParameters
    {
        public static void Init()
        {
             DotNetEnv.Env.TraversePath().Load(".env");
        }

        public static EnvironmentVariables? GetEnvironmentVariables()
        {
            try
            {
                return new EnvironmentVariables
                {
                    Port = ushort.Parse(DotNetEnv.Env.GetString("Port")),
                    MaxClients = int.Parse(DotNetEnv.Env.GetString("MaxClients")),
                    
                    MySqlServer = DotNetEnv.Env.GetString("MySqlServer"),
                    MySqlUserId = DotNetEnv.Env.GetString("MySqlUserId"),
                    MySqlPassword = DotNetEnv.Env.GetString("MySqlPassword"),
                    MySqlDatabase = DotNetEnv.Env.GetString("MySqlDatabase")
                };
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error: .env file is missing arguments");
            }

            return null;
        }
    }
}