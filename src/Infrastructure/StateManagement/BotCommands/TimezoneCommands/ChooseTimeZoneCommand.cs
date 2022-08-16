using Application.New.Settings.Commands;
using Application.New.Settings.Queries;
using Infrastructure.New.StateManagement.Models;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.New.StateManagement.BotCommands.TimezoneCommands;

public class ChooseTimeZoneCommand : IBotCommand
{
    private readonly IMediator _mediator;
    private readonly TelegramBotClient _client;

    public ChooseTimeZoneCommand(
        TelegramBotClient client, 
        IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }
    
    public Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken)
    {
        var commandPayload = request.Text;
        return Task.FromResult(commandPayload.Contains(CommandNames.ChooseTimeZone));
    }

    public async Task Execute(UserRequest request, CancellationToken token)
    {
        var timeZoneCommand = new TimeZoneCallback(request.Text);
        if (timeZoneCommand.Action == null)
        {
            await SendMarkup(request, token);
        }
        else
        {
            await ChangeValue(request, timeZoneCommand.Action, token);
        }
    }

    private async Task ChangeValue(UserRequest botCommand, Action? action, CancellationToken token)
    {
        switch (action)
        {
            case Action.Up:
            case Action.Down:
                await EditTimeZone(botCommand, action, token);
                break;
            case Action.Save:
                await SaveTimeZone(botCommand, token);
                return;
        }
    }

    private async Task EditTimeZone(UserRequest botCommand, Action? action, CancellationToken token)
    {
        timeZoneOffset = action == Action.Up ? timeZoneOffset + 1 : timeZoneOffset - 1;
        
        var timeZoneOffsetView = new TimeZoneView(timeZoneOffset);
        await _client.EditMessageReplyMarkupAsync(botCommand.UserTelegramId,
            botCommand.MessageId,
            replyMarkup: GetTimeZoneCountdownMarkup(botCommand.MessageId, timeZoneOffsetView), 
            cancellationToken: token);
    }

    private async Task SaveTimeZone(UserRequest botCommand, CancellationToken token)
    {
        await _mediator.Send(
            new UpdateSettingsCommandByUserTelegramId
            {
                UserTelegramId = botCommand.UserTelegramId, 
                TimeZone = botCommand.Text.GetTimeZone()
            }, token);

        await _client.EditMessageTextAsync(botCommand.UserTelegramId, botCommand.MessageId,
            $"🕗 Часовой пояс сохранён: {botCommand.Text.GetTimeZone()}",
            replyMarkup: null,
            cancellationToken:token 
        );
    }

    private async Task SendMarkup(UserRequest botCommand, CancellationToken token)
    {
        var userSettings = await _mediator.Send(new GetUserSettingsByTelegramIdQuery
        {
            TelegramId = botCommand.UserTelegramId
        }, token);
        
        await _client.EditMessageTextAsync(botCommand.UserTelegramId, botCommand.MessageId,
            $"🕗 Часовой пояс:",
            replyMarkup: GetTimeZoneCountdownMarkup(botCommand.MessageId, userSettings.TimeZone),
            cancellationToken:token 
        );
    }

    private static InlineKeyboardMarkup GetTimeZoneCountdownMarkup(int messageId, int timeZoneOffsetView)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("🔽", $"{CommandNames.ChooseTimeZone} {Action.Down} {messageId} {timeZoneOffsetView}"),
                InlineKeyboardButton.WithCallbackData($"{timeZoneOffsetView}", $"{CommandNames.ChooseTimeZone} {Action.Save} {messageId} {timeZoneOffsetView}"),
                InlineKeyboardButton.WithCallbackData("🔼", $"{CommandNames.ChooseTimeZone} {Action.Up} {messageId} {timeZoneOffsetView}")
            },
            new []{ InlineKeyboardButton.WithCallbackData("Сохранить", $"{CommandNames.ChooseTimeZone} {Action.Save} {messageId} {timeZoneOffsetView}")}
        });
    }
}