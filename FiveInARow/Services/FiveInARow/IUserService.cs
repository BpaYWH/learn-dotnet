using FiveInARow.Models;

namespace FiveInARow.Services.FiveInARow
{
    public interface IUserService
    {
        bool CreateUser(User user);
        User GetUser(int id);

        ICollection<User> GetUsers();
        bool UpsertUser(User user);

        bool UserExists(int id);
    }
}