using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ITMO.SoftwareTesting.Dates.Services.Tools
{
    public static class JwtTool
    {
        public static string IssueToken(int userId)
        {
            var now = DateTime.Now;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "Dates",
                audience: "Users",
                notBefore: now,
                claims: new[] {new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())},
                expires: now.Add(TimeSpan.FromDays(30)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("CompleteKickAssOmgThatIsSoBad")),
                    SecurityAlgorithms.HmacSha256
                )
            );

            var encodedJwt = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return encodedJwt;
        }

        public static JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS;
        }
    }
}