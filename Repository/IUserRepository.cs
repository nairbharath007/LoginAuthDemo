using LoginAuthDemo.DTO;
using LoginAuthDemo.Models;

namespace LoginAuthDemo.Repository
{
    public interface IUserRepository
    {
        public string AddUser(UserDto userDto);
        public User FindUser(string userName);
        public string GetRoleName(User user);

    }
}
