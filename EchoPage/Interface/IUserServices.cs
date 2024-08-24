using EchoPage.Models;

namespace EchoPage.Interface
{
    public interface IUserServices
    {
        Task<bool> Register(Users user);
        Task<string> Login(Users user);
    }
}
