using Duende.IdentityServer.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
                {  new ApiScope(name: "api1", displayName: "My API") };

        public static IEnumerable<Client> Clients =>
            new Client[]
                {
                    new Client()
                    {
                        // ha no interactive user, use the clientId and clientSecret for authentication.
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        
                        ClientId = "client",
                        ClientSecrets = { new Secret("secret".Sha256()) },

                        // scopes that will use by client for accessing to relevant API that will check access token whether it has api1 scope 
                        // the list of scopes that the client is allowed to ask for.
                        // Notice that the allowed scope must match the name of ApiScope above.
                        AllowedScopes = { "api1"},
                    }
                };
    }
}