using Application.New.Common;
using MediatR;

namespace Application.New.Settings.Commands;

public class CreateSettingsCommand : IRequest
{
    public string UserId { get; set; }
    
    public class Handler : IRequestHandler<CreateSettingsCommand>
    {
        private readonly IEkmekBotDbContext _ekmekBotDbContext;
        private const int DefaultTimeZoneMoscow = 3;
        
        public Handler(IEkmekBotDbContext ekmekBotDbContext)
        {
            _ekmekBotDbContext = ekmekBotDbContext;
        }
        
        public async Task<Unit> Handle(CreateSettingsCommand request, CancellationToken cancellationToken)
        {
            _ekmekBotDbContext.Settings.Add(new Domain.New.Entities.Settings
            {
                SettingsId = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                TimeZone = DefaultTimeZoneMoscow,
            });

            await _ekmekBotDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}