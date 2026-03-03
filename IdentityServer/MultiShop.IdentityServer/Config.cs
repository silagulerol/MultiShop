// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MultiShop.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResource => new ApiResource[]
        {
           // ResourceCatalog ismindeki Key'e sahip bir mikroservice user Catalog Full Permission işlemini gerçekleştirebilecek.
           new ApiResource("ResourceCatalog"){ Scopes={"CatalogFullPermission", "CatalogReadPermission"} },
           new ApiResource("ResourceDiscount"){ Scopes={ "DiscountFullPermission"} },
           new ApiResource("ResourceOrder"){Scopes={ "OrderFullPermission", "OrderReadPermission" } },
           new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        //Token'ını aldığım kullanıcın o token içerisinde bilgilerine erişim sağlayacağımı bildirmiş oldum
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

        // Burada bir kullanıcının sahip olduğu resource'a göre yapabileceği işlemler yer alıyor
        // Token alan kişi CatalogFullPermision yetkisine sahipse kişinin yapabileceği işlemler
        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("CatalogFullPermission", "Full authority for catalog operations"),
            new ApiScope("CatalogReadPermission", "Read authority for catalog operations"),
            new ApiScope("DiscountFullPermission", "Full authority  for discount operations"),
            new ApiScope("OrderFullPermission" , "Full authority  for order operations"),
            new ApiScope("OrderReadPermission" , "Read authority for order operations"), 
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)

        };

        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "MultiShopVisitorId",
                ClientName = "Multi Shop Visitor User",
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                ClientSecrets= {new Secret("multishopsecret".Sha256()) },
                AllowedScopes= { "CatalogReadPermission"}
            },

            new Client
            {
                ClientId = "MultiShopManagerId",
                ClientName = "Multi Shop Manager User",
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                ClientSecrets= {new Secret("multishopsecret".Sha256()) },
                AllowedScopes= { "CatalogReadPermission", "CatalogFullPermission"}
            },

            new Client
            {
                ClientId = "MultiShopAdminId",
                ClientName = "Multi Shop Admin User",
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                ClientSecrets= {new Secret("multishopsecret".Sha256()) },
                AllowedScopes= { "CatalogReadPermission", "CatalogFullPermission", "DiscountFullPermission" , "OrderFullPermission", "OrderReadPermission",
                IdentityServerConstants.LocalApi.ScopeName,
                IdentityServerConstants.StandardScopes.Email,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OpenId,
                },
                AccessTokenLifetime= 600
            }
        };
    }
}