using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;

namespace MultiShop.IdentityServer.Tools
{
    public class JwtTokenGenerator
    {
        public static TokenResponseViewModel GenerateToken(GetCheckAppUserViewModel model)
        {
            //JWT içindeki bilgiler = claims
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(model.Role))
                // Role ekleniyor.ClaimTypes.Role, JWT içindeki rol bilgisini tutar.authorization için kullanılır
                claims.Add(new Claim(ClaimTypes.Role, model.Role));

            // User ID ekleniyor. ClaimTypes.NameIdentifier, JWT içindeki kullanıcı ID bilgisini tutar.authorization için kullanılır
            claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Id));

            if (!string.IsNullOrWhiteSpace(model.Username))
                // User Name ekleniyor. ClaimTypes.Name, JWT içindeki kullanıcı adı bilgisini tutar.authorization için kullanılır
                claims.Add(new Claim("UserName", model.Username));

            // JWT'nin imzalanması için gerekli olan anahtar ve algoritma belirleniyor
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));

            // HmacSha256 algoritması kullanılarak JWT imzalanıyor
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // JWT'nin geçerlilik süresi belirleniyor. Expire, JWT'nin kaç gün geçerli olacağını belirtir.
            var expireDate = DateTime.UtcNow.AddDays(JwtTokenDefaults.Expire);

            // JWT oluşturuluyor.
            // Issuer, JWT'nin hangi sunucu tarafından oluşturulduğunu belirtir.
            // Audience, JWT'nin hangi sunucular tarafından kabul edileceğini belirtir.
            // Claims, JWT içindeki bilgileri tutar. NotBefore, JWT'nin hangi tarihten itibaren geçerli olduğunu belirtir.
            // Expires, JWT'nin hangi tarihte geçersiz olacağını belirtir. SigningCredentials, JWT'nin imzalanması için gerekli olan anahtar ve algoritmayı belirtir.
            JwtSecurityToken token = new JwtSecurityToken(issuer: JwtTokenDefaults.ValidIssuer, audience: JwtTokenDefaults.ValidAudience, claims: claims, notBefore: DateTime.UtcNow, expires: expireDate, signingCredentials: signingCredentials);

            // JWT'nin string formatında yazılması için JwtSecurityTokenHandler kullanılır.
            // WriteToken, JWT'yi string formatına dönüştürür.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            // TokenResponseViewModel, JWT'nin string formatında ve geçerlilik süresi bilgisiyle birlikte döndürülür.
            return new TokenResponseViewModel(tokenHandler.WriteToken(token), expireDate);
        }
    }
}