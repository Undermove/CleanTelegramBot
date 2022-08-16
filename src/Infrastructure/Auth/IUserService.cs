namespace Infrastructure.New.Auth;

public interface IUserService
{
    bool ValidateCredentials(string? username, string? password);
}