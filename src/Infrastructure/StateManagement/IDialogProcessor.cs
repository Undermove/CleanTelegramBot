using Infrastructure.New.StateManagement.Models;

namespace Infrastructure.New.StateManagement;

public interface IDialogProcessor
{
    Task ProcessCommand(UserRequest request, CancellationToken cancellationToken);
}