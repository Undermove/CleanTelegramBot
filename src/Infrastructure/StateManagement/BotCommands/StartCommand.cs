using Application.Users.Commands;
using Infrastructure.StateManagement.Models;
using MediatR;
using Telegram.Bot;

namespace Infrastructure.StateManagement.BotCommands;

public class StartCommand : IBotCommand
{
    private readonly IMediator _mediator;
    private readonly TelegramBotClient _client;

    public StartCommand(IMediator mediator, TelegramBotClient client)
    {
        _mediator = mediator;
        _client = client;
    }
    
    public Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken)
    {
        var commandPayload = request.Text;
        return Task.FromResult(commandPayload.Contains(CommandNames.Start));
    }

    public async Task Execute(UserRequest request, CancellationToken token)
    {
        await _mediator.Send(new CreateUserCommand
        {
            TelegramId = request.UserTelegramId
        }, token);

        await _client.SendTextMessageAsync(request.UserTelegramId,
            "Hi i'm your template bot! 🥖",
            cancellationToken: token);
    }
}