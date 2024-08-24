using Dapper;
using EchoPage.Interface;
using EchoPage.Models;
using EchoPage.Security;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EchoPage.Services
{
    public class userServices : IUserServices
    {
        private readonly ILogger<userServices> logger;
        private readonly IDbConnection connection;
        private readonly IConfiguration config;
        public userServices(ILogger<userServices> logger, IConfiguration config)
        {
            this.config = config;
            this.connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.logger = logger;
        }

        public async Task<bool> Register(Users user)
        {
            try
            {
                user.password = PasswordHasher.Hashpassword(user.password);

                var parameter = new DynamicParameters();
                parameter.Add("@email", user.email);
                parameter.Add("@name", user.name);
                parameter.Add("@password", user.password);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                var response = await connection.ExecuteAsync("insert_user", parameter, commandType: CommandType.StoredProcedure);
                string? Errormsg = parameter.Get<string>("@Errormsg");
                if (response < 1)
                {
                    logger.LogError(Errormsg);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> Login(Users user)
        {
            try
            {

                var parameter = new DynamicParameters();
                parameter.Add("@email", user.email);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var existing = await connection.QueryAsync<Users>("get_user", parameter, commandType: CommandType.StoredProcedure);
                var Errormsg = parameter.Get<string?>("@Errormsg");
                var existingUser = existing.FirstOrDefault();
                logger.LogInformation(existingUser.name);
                if (existingUser == null)
                {
                    logger.LogError(Errormsg);
                    return null;
                }

                var isVerified = PasswordHasher.VerifyPassword(user.password, existingUser.password);
                if (isVerified)
                {
                    return GetToken(existingUser);
                }
                logger.LogInformation("Incorrect password");
                return null;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        private string GetToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
