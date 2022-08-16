using Application.New.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.New.Settings.Queries;

public class GetUserSettingsByTelegramIdQuery: IRequest<Domain.New.Entities.Settings>
{
    public long TelegramId { get; set; }

    public class Handler : IRequestHandler<GetUserSettingsByTelegramIdQuery, Domain.New.Entities.Settings>
    {
        private readonly IEkmekBotDbContext _dbContext;

        public Handler(IEkmekBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.New.Entities.Settings> Handle(GetUserSettingsByTelegramIdQuery request, CancellationToken cancellationToken)
        {
            var settings = await _dbContext.Settings
                .Where(settings => settings.User.TelegramId == request.TelegramId)
                .FirstAsync(cancellationToken);

            return settings;
        }
    }
}