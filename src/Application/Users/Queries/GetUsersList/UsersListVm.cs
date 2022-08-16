using Domain.New.Entities;

namespace Application.New.Users.Queries.GetUsersList;

public class UsersListVm
{
    public IList<User> Users { get; init; } = null!;
}