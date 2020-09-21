using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.Configuration
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScope =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };


        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    
                    //** It will use client id/secret to authenticate
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    //** scope client can access
                    AllowedScopes = 
                    { 
                        "api1"
                    }
                }
            };
    }
}