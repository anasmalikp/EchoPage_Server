using Dapper;
using EchoPage.Interface;
using EchoPage.Models;
using System.Data;
using System.Data.SqlClient;

namespace EchoPage.Services
{
    public class BlogServices:IBlogServices
    {
        private readonly ILogger<BlogServices> logger;
        private readonly IConfiguration config;
        private readonly IDbConnection connection;
        private readonly UserCreds creds;
        public BlogServices(ILogger<BlogServices> logger, IConfiguration config, UserCreds creds)
        {
            this.config = config;
            this.logger = logger;
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.creds = creds;
        }

        public async Task<bool> createBlog(Blogs blog)
        {
            try
            {

                blog.createdBy = creds.userid;
                blog.createdAt = DateTime.Now;
                var parameter = new DynamicParameters();
                parameter.Add("@title", blog.title);
                parameter.Add("@descrptn", blog.descrptn);
                parameter.Add("@createdBy", blog.createdBy);
                parameter.Add("@createdAt", blog.createdAt);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var response = await connection.ExecuteAsync("create_blog", parameter, commandType: CommandType.StoredProcedure);

                string? Errormsg = parameter.Get<string?>("@Errormsg");

                if (response < 1)
                {
                    logger.LogError(Errormsg);
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Blogs>> GetBlogs()
        {
            try
            {
                var blogs = await connection.QueryAsync<Blogs>("select * from Blogs");
                return blogs;
            } catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateBlog(Blogs blog)
        {
            try
            {
                blog.createdBy = creds.userid;
                var parameter = new DynamicParameters();
                parameter.Add("@id", blog.id);
                parameter.Add("@title", blog.title);
                parameter.Add("@descrptn", blog.descrptn);
                parameter.Add("@createdBy", blog.createdBy);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var response = await connection.ExecuteAsync("update_blog", parameter, commandType: CommandType.StoredProcedure);

                string? Errormsg = parameter.Get<string?>("@Errormsg");

                if(response < 1)
                {
                    logger.LogError(Errormsg);
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Blogs>> SearchBlogs(string prompt)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@prompt", prompt);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var response = await connection.QueryAsync<Blogs>("search_blog", parameter, commandType: CommandType.StoredProcedure);

                string? Errormsg = parameter.Get<string?>("@Errormsg");

                if (response.Count() == 0)
                {
                    logger.LogError(Errormsg);
                    return null;
                }
                return response;
            }
            catch( Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Blogs> GetSingleBlog(int id)
        {
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("id", id);
                parameter.Add("@Errormsg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var response = await connection.QueryFirstAsync<Blogs>("Get_single_blog", parameter, commandType: CommandType.StoredProcedure);
                string? Errormsg = parameter.Get<string?>("@Errormsg");
                if(response == null)
                {
                    logger.LogError(Errormsg);
                    return null;
                }
                return response;
            }
            catch ( Exception ex )
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
