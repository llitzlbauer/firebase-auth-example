using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreBackend.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
