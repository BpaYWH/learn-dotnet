using FiveInARow.Models;
using FiveInARow.Context;

namespace FiveInARow.Services.FiveInARow
{
    public class UserService : IUserService
    {
        private readonly FiveInARowDbContext _context;

        public UserService(FiveInARowDbContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool UpsertUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(u => u.Id).ToList();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}