using System.IdentityModel.Tokens.Jwt;

namespace EchoPage.Security
{
    public class TokenDecoder
    {
        public static int decodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decoded = handler.ReadJwtToken(token);
            var userid = decoded.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            return int.Parse(userid);
        }
    }
}
