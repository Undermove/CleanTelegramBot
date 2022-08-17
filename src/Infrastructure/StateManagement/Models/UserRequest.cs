using Telegram.Bot.Types;

namespace Infrastructure.StateManagement.Models;

public class UserRequest
{
    public int MessageId { get; }
    public long UserTelegramId { get; }
    public string Text { get; }
    public string UserName { get; }
    
    public UserRequest(Update request)
    {
        UserTelegramId = request.Message?.From?.Id ?? request.CallbackQuery?.From.Id ?? throw new ArgumentException();
        MessageId = request.Message?.MessageId ?? request.CallbackQuery?.Message?.MessageId ?? throw new ArgumentException();
        Text = request.Message?.Text ?? request.CallbackQuery?.Data ?? throw new ArgumentException();
        UserName = request.Message?.Chat.FirstName ?? request.CallbackQuery?.From.Username ?? throw new ArgumentException();
    }
}