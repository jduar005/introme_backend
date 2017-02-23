using System.Configuration;

namespace Intro
{
    using System;
    using Nancy.Hosting.Self;

    class Program
    {
        static void Main(string[] args)
        {
            var uri =
                new Uri(ConnectionString);

            using (var host = new NancyHost(uri))
            {
                host.Start();

                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }

        private static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NancyConnectionString"].ConnectionString;
            }
        }
    }
}
