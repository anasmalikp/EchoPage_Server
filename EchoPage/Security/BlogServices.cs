using Dapper;
using EchoPage.Models;
using System.Data;
using System.Data.SqlClient;

namespace EchoPage.Security
{
    public class BlogServices
    {
        private readonly ILogger<BlogServices> logger;
        private readonly IConfiguration config;
        private readonly IDbConnection connection;
        public BlogServices(ILogger<BlogServices> logger, IConfiguration config)
        {
            this.config = config;
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.logger = logger;
        }

        /*public async Task<bool> CreateBlog(Blogs blog)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@title", blog.title);
            parameters.Add("@descrptn", blog.descrptn);
            parameters.Add("@createdBy",)
        }*/
    }
}
