using Lib.Database;

public interface IUserService
{
    User Get(Guid id);

    Task<User> Add(RegisterUserModel model);
}

public class RegisterUserModel
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}