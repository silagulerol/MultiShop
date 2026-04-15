// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MultiShop.IdentityServer
{
    /* bu dosyayı hazırlayarak IdentityServer'a şunları öğretmiş oldun:
       1.Hangi mikroservislerim var? (ApiResources)
       2.Bu servislerde hangi yetki isimleri geçerli? (ApiScopes)
       3.Kimlik kartında hangi bilgiler yazsın? (IdentityResources)
       4.Sisteme kimler, hangi yöntemle ve hangi yetki sınırıyla girebilir? (Clients)  */
    public static class Config
    {
        // ApiResource, sistemindeki korunması gereken her bir mikroservisi (veya kaynağı) temsil eder.
        // "Benim ResourceCatalog adında bir binam var ve bu binaya girmek isteyenlerin elinde
        // ya CatalogFullPermission ya da CatalogReadPermission anahtarı olmalı."
        // Burada sistemdeki tüm servisler(Cargo, Order, Basket vb.) tek tek "korunan kaynak" olarak işaretlenir.
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
           new ApiResource("ResourceCatalog"){Scopes={"CatalogFullPermission","CatalogReadPermission"} },
           new ApiResource("ResourceDiscount"){Scopes={"DiscountFullPermission"} },
           new ApiResource("ResourceOrder"){Scopes={"OrderFullPermisson"}},
           new ApiResource("ResourceCargo"){Scopes={"CargoFullPermission"} },
           new ApiResource("ResourceBasket"){Scopes={"BasketFullPermission"} },
           new ApiResource("ResourceComment"){Scopes={"CommentFullPermission"} },
           new ApiResource("ResourcePayment"){Scopes={ "PaymentFullPermission" } },
           new ApiResource("ResourceImage"){Scopes={ "ImageFullPermission" } },
           new ApiResource("ResourceOcelot"){Scopes={"OcelotFullPermission"} },
           new ApiResource("ResourceMessage"){Scopes={"MessageFullPermission"} },
           new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        //IdentityResource, kullanıcı giriş yaptığında token içinde kullanıcıya dair hangi "kimlik" bilgilerinin (Claim) taşınacağını belirler.
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
          new IdentityResources.OpenId(),
          new IdentityResources.Profile(),
          new IdentityResources.Email()
        };

        //ApiScope, sistemde tanımlı olan yetki seviyeleridir. Birer "yetki etiketi" veya "anahtar" gibi.
        //Örnek: CatalogFullPermission anahtarına sahip olan biri katalogda her şeyi yapabilirken,
        //CatalogReadPermission anahtarı olan sadece ürünleri görebilir.
        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("CatalogFullPermission","Full authority for catalog operations"),
            new ApiScope("CatalogReadPermission","Reading authority for catalog operations"),
            new ApiScope("DiscountFullPermission","Full authority for discount operations"),
            new ApiScope("OrderFullPermisson","Full authority for order operations"),
            new ApiScope("CargoFullPermission","Full authority for cargo operations"),
            new ApiScope("BasketFullPermission","Full authority for basket operations"),
            new ApiScope("CommentFullPermission","Full authority for comment operations"),
            new ApiScope("PaymentFullPermission","Full authority for payment operations"),
            new ApiScope("ImageFullPermission","Full authority for image operations"),
            new ApiScope("OcelotFullPermission","Full authority for ocelot operations"),
            new ApiScope("MessageFullPermission","Full authority for message operations"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

        //Burada sisteme erişmek isteyen "istemciler" tanımlanır. bu senaryonda 3 farklı erişim türü var:
        public static IEnumerable<Client> Clients => new Client[]
        {
            //Visitor
            new Client
            {
                ClientId="MultiShopVisitorId",
                ClientName="Multi Shop Visitor User",
                AllowedGrantTypes=GrantTypes.ClientCredentials, //(Kullanıcı adı/şifre gerekmez, uygulamanın kendi kimliğiyle giriş yapılır).
                //Özellik: Sadece okuma veya temel yetkilere (CatalogReadPermission gibi) sahiptir.
                //Genellikle üye olmayan kullanıcılar için arka planda veri çekmek için kullanılır.
                ClientSecrets={new Secret("multishopsecret".Sha256())},
                AllowedScopes={"CatalogReadPermission","CatalogFullPermission","OcelotFullPermission","CommentFullPermission","ImageFullPermission", "CommentFullPermission",  IdentityServerConstants.LocalApi.ScopeName },
                AllowAccessTokensViaBrowser=true
            },

            //Manager
            new Client
            {
                ClientId="MultiShopManagerId",
                ClientName="Multi Shop Manager User",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,//(Gerçek bir kullanıcı adı ve şifre gerektirir).
                //Özellik: Neredeyse tüm yetkilere sahiptir. Kendi hesabı üzerinden işlem yapar.
                ClientSecrets={new Secret("multishopsecret".Sha256()) },
                AllowedScopes={ "CatalogReadPermission", "CatalogFullPermission", "BasketFullPermission", "OcelotFullPermission", "CommentFullPermission", "PaymentFullPermission", "ImageFullPermission","DiscountFullPermission","OrderFullPermisson","MessageFullPermission","CargoFullPermission",
                IdentityServerConstants.LocalApi.ScopeName,
                IdentityServerConstants.StandardScopes.Email,
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile }
            },

            //Admin
            new Client
            {
                ClientId="MultiShopAdminId",
                ClientName="Multi Shop Admin User",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                ClientSecrets={new Secret("multishopsecret".Sha256()) },
                AllowedScopes={ "CatalogFullPermission", "CatalogReadPermission", "DiscountFullPermission", "OrderFullPermisson","CargoFullPermission","BasketFullPermission","OcelotFullPermission","CommentFullPermission","PaymentFullPermission","ImageFullPermission","CargoFullPermission",
                IdentityServerConstants.LocalApi.ScopeName,
                IdentityServerConstants.StandardScopes.Email,
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile
                },
                AccessTokenLifetime=600
            }
        };
    }
}