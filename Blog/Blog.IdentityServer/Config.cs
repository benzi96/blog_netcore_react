// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Blog.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                   Name = "roles",
                   DisplayName = "Roles",
                   UserClaims = { JwtClaimTypes.Role }
                }
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api1", "My API #1", new[] { JwtClaimTypes.Role, JwtClaimTypes.Name })
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "api1" }
                },

                // MVC client using code flow + pkce
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    RequireConsent = true,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "https://blogmvcfrontendsd2625.azurewebsites.net/signin-oidc" },
                    PostLogoutRedirectUris = { "https://blogmvcfrontendsd2625.azurewebsites.net/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api1", "offline_access", "roles" }
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "https://blogreactsd2625.azurewebsites.net/callback",
                        "https://blogreactsd2625.azurewebsites.net/silent_renew.html",
                    },

                    PostLogoutRedirectUris = { "https://blogreactsd2625.azurewebsites.net/callback" },
                    AllowedCorsOrigins = { "https://blogreactsd2625.azurewebsites.net" },

                    AllowedScopes = { "openid", "profile", "api1" }
                }
            };
    }
}