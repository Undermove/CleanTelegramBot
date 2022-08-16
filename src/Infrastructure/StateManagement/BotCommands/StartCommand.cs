using Application.New.Settings.Commands;
using Application.New.Users.Commands;
using Application.New.Users.Queries;
using Infrastructure.New.StateManagement.Models;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.New.StateManagement.BotCommands;

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
        var user = await _mediator.Send(new GetUserByTelegramId {TelegramId = request.UserTelegramId}, token);
        if (user?.TableUrl != null)
        {
            var replyKeyboardMarkup = new ReplyKeyboardMarkup
            (
                new[]
                {
                    new KeyboardButton($"{CommandNames.GetTableIcon} Моя таблица"),
                    new KeyboardButton($"{CommandNames.SettingsIcon} Настройки")
                }
            );
            await _client.SendTextMessageAsync(
                request.UserTelegramId,
                $"{CommandNames.SettingsIcon} Твоя таблица:\r\n{user.TableUrl}",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: token);
            
            return;
        }

        // todo: use transaction pipeline form MediatR
        // https://medium.com/swlh/transaction-management-with-mediator-pipelines-in-asp-net-core-39317a19bb8d
        user = await _mediator.Send(new CreateUserCommand
        {
            TelegramId = request.UserTelegramId
        }, token);
        await _mediator.Send(new CreateSettingsCommand
        {
            UserId = user.UserId
        }, token);

        await _client.SendTextMessageAsync(request.UserTelegramId,
            "Привет! Меня зовут Экмек 🥖" +
            "\r\nЯ буду помогать тебе вести дневник своих мыслей." +
            "\r\nВсе записи я буду хранить в Google-таблице." +
            "\r\nСкинь мне свой gmail, чтобы я мог создать её для тебя",
            cancellationToken: token);
    }
}