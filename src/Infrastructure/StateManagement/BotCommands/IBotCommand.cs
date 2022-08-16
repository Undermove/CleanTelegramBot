using Infrastructure.New.StateManagement.Models;

namespace Infrastructure.New.StateManagement.BotCommands;

public interface IBotCommand
{
    Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken);
    Task Execute(UserRequest request, CancellationToken token);
}