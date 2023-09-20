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
            return Save();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(u => u.Id).ToList();
        }

        public bool UpsertUser(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}