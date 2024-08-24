using EchoPage.Models;

namespace EchoPage.Interface
{
    public interface IBlogServices
    {
        Task<bool> createBlog(Blogs blog);
        Task<IEnumerable<Blogs>> GetBlogs();
        Task<bool> UpdateBlog(Blogs blog);
        Task<IEnumerable<Blogs>> SearchBlogs(string prompt);
        Task<Blogs> GetSingleBlog(int id);
    }
}
