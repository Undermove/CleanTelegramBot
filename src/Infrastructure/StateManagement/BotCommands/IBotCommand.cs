using Infrastructure.StateManagement.Models;

namespace Infrastructure.StateManagement.BotCommands;

public interface IBotCommand
{
    Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken);
    Task Execute(UserRequest request, CancellationToken token);
}