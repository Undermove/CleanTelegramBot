using System;
using System.Threading;
using System.Threading.Tasks;
using Application.New.Users.Queries;
using Infrastructure.New.StateManagement;
using Infrastructure.New.StateManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EkmekBot.Controllers;

[ApiController]
[Route("[controller]")]
public class TelegramController : Controller
{
    private readonly BotConfiguration _configuration;
    private readonly TelegramBotClient _telegramBotClient;
    private readonly ILogger _logger;
    private readonly IDialogProcessor _dialogProcessor;

    public TelegramController(
        BotConfiguration configuration,
        TelegramBotClient telegramBotClient,
        ILoggerFactory logger,
        IDialogProcessor dialogProcessor)
    {
        _configuration = configuration;
        _telegramBotClient = telegramBotClient;
        _dialogProcessor = dialogProcessor;
        _logger = logger.CreateLogger(typeof(TelegramController));
    }

    [HttpPost("{token?}")]
    public async Task Webhook(string token, [FromBody] Update request, CancellationToken cancellationToken)
    {
        if (token != _configuration.WebhookToken)
        {
            return;
        }

        // todo: 
        var userRequest = new UserRequest(request);

        try
        {
            await _dialogProcessor.ProcessCommand(userRequest, cancellationToken);
        }
        catch (Exception e)
        {
            await _telegramBotClient.SendTextMessageAsync(userRequest.UserTelegramId,
                "Прости, кажется у меня что-то сломалось 😞 Попробуй еще раз через несколько минут.", cancellationToken: cancellationToken);
            _logger.LogInformation(e, "Exception while processing request from user: {User} with command {Command}",
                userRequest.UserTelegramId, userRequest.Text);
        }
    }

    [Authorize]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateWebhook(string url)
    {
        try
        {
            await _telegramBotClient.SetWebhookAsync($"{url.Trim()}", dropPendingUpdates: true);
            return Ok("Success");
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}