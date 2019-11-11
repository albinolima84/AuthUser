using System.Collections.Generic;

namespace Sky.Auth.Command.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IReadOnlyCollection<PhoneDto> PhoneNumbers { get; set; }
    }
}
