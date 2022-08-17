using Infrastructure.StateManagement.BotCommands;
using Infrastructure.StateManagement.Models;

namespace Infrastructure.StateManagement;

public class DialogProcessor: IDialogProcessor
{
    private readonly List<IBotCommand> _commands;

    public DialogProcessor(IEnumerable<IBotCommand> processorsList)
    {
        _commands = processorsList.ToList();
    }
    
    public async Task ProcessCommand(UserRequest request, CancellationToken token)
    {
        foreach (var command in _commands)
        {
            if (await command.IsApplicable(request, token))
            {
                await command.Execute(request, token);
                return;
            }
        }
    }
}