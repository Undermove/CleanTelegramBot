using Infrastructure.New.StateManagement.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.New.StateManagement.BotCommands;

public class SettingsCommand: IBotCommand
{
    private readonly TelegramBotClient _client;

    public SettingsCommand(TelegramBotClient client)
    {
        _client = client;
    }
    
    public Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken)
    {
        var commandPayload = request.Text;
        return Task.FromResult(commandPayload.StartsWith(CommandNames.Settings) ||
                               commandPayload.StartsWith(CommandNames.SettingsIcon));
    }
    
    public async Task Execute(UserRequest request, CancellationToken token)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("⏰ Время оповещений", CommandNames.ChangeMeasuresTime),
            InlineKeyboardButton.WithCallbackData("🌐 Часовой пояс", CommandNames.ChooseTimeZone),
        });
        
        await _client.SendTextMessageAsync(request.UserTelegramId,
            $"{CommandNames.SettingsIcon} Настройки:", 
            replyMarkup:keyboard, 
            cancellationToken:token 
        );
    }
}