using Infrastructure.StateManagement.Models;

namespace Infrastructure.StateManagement;

public interface IDialogProcessor
{
    Task ProcessCommand(UserRequest request, CancellationToken cancellationToken);
}