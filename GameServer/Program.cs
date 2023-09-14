using GameSever.Utils;

namespace GameSever
{
    class Program
    {   
        static void Main(string[] args)
        {
            // Load .env file located in project root
            EnvironmentParameters.Init();
            var server = new Server();
        }
    }
}

