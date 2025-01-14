﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace EkmekBot.HostedServices;

public class CreateWebhook : IHostedService
{
    private readonly BotConfiguration _config;
    private readonly TelegramBotClient _telegramBotClient;

    public CreateWebhook(BotConfiguration config, TelegramBotClient telegramBotClient)
    {
        _config = config;
        _telegramBotClient = telegramBotClient;
    }
        
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_config.HostAddress))
        {
            await _telegramBotClient.SetWebhookAsync($"{_config.HostAddress}/telegram/{_config.WebhookToken}", dropPendingUpdates: true, cancellationToken: cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _telegramBotClient.DeleteWebhookAsync(true, cancellationToken);
    }
}