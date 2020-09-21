using System;
using System.Net.Http;
using IdentityModel.Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting.....");
            Client.TestIdentityServer().GetAwaiter().GetResult();
        }
    }
}
