using Application.New.Common;
using Application.New.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.New.Settings.Commands;

public class UpdateSettingsCommandByUserTelegramId: IRequest<Domain.New.Entities.Settings>
{
    public long UserTelegramId { get; set; }
    public int? TimeZone { get; set; }
    public TimeOnly? MeasuresTime { get; set; } = null!;

    public class Handler: IRequestHandler<UpdateSettingsCommandByUserTelegramId, Domain.New.Entities.Settings>
    {
        private readonly IEkmekBotDbContext _ekmekBotDbContext;

        public Handler(IEkmekBotDbContext ekmekBotDbContext)
        {
            _ekmekBotDbContext = ekmekBotDbContext;
        }

        public async Task<Domain.New.Entities.Settings> Handle(UpdateSettingsCommandByUserTelegramId request, CancellationToken cancellationToken)
        {
            var settings = await _ekmekBotDbContext.Settings.FirstOrDefaultAsync(
                s => s.User.TelegramId == request.UserTelegramId, cancellationToken: cancellationToken);
            if (settings == null)
            {
                throw new NotFoundException(nameof(Settings), request.UserTelegramId);
            }

            settings.TimeZone = request.TimeZone ?? settings.TimeZone;

            await _ekmekBotDbContext.SaveChangesAsync(cancellationToken);
            
            return settings;
        }
    }
}