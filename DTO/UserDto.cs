using System.ComponentModel.DataAnnotations;

namespace LoginAuthDemo.DTO
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int RoleId { get; set; }

    }
}
