using LoginAuthDemo.Data;
using LoginAuthDemo.DTO;
using LoginAuthDemo.Models;

namespace LoginAuthDemo.Repository
{
    public class UserRepository : IUserRepository
    {
        private MyContext _context;
        public UserRepository(MyContext context)
        {
            _context = context;
        }

        public string AddUser(UserDto userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            _context.Users.Add(new User()
            {
                UserName = userDto.UserName,
                PasswordHash = passwordHash,
                RoleId = userDto.RoleId
            });
            _context.SaveChanges();
            return userDto.UserName;
        }

        public User FindUser(string userName)
        {
            return _context.Users
                .Where(user => user.UserName == userName)
                .FirstOrDefault();
        }
        public string GetRoleName(User user)
        {
            return _context.Roles
                .Where(role => role.Id == user.RoleId)
                .FirstOrDefault()
                .RoleName;
        }
    }
}
