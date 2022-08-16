using System;
using System.Threading;
using System.Threading.Tasks;
using Application.New.Users.Queries.GetUsersList;
using MediatR;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace EkmekBot.Jobs;

public class ReminderJob : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly TelegramBotClient _client;
    
    public ReminderJob(IMediator mediator, TelegramBotClient client)
    {
        _mediator = mediator;
        _client = client;
    }
    
    // todo: move to hangfire https://www.youtube.com/watch?v=iilRdmNILC8&ab_channel=codaza
    // https://habr.com/ru/post/280732/
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (DateTime.UtcNow.Hour == 16 && DateTime.UtcNow.Minute == 21)
            {
                var usersListVm = await _mediator.Send(new GetUsersListQuery(), token);
                foreach (var user in usersListVm!.Users)
                {
                    await _client.SendTextMessageAsync(user.TelegramId,
                        "Привет! ⏰" +
                        "\r\nДень подходит к концу – не забудь внести записи в дневник. 📖",
                        cancellationToken: token);
                }
            }
            await Task.Delay(TimeSpan.FromMinutes(1), token);
        }
    }
}