using Application.New.Common;
using Domain.New.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.New.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public long TelegramId { get; set; }

    public class Handler: IRequestHandler<CreateUserCommand, User>
    {
        private readonly IEkmekBotDbContext _dbContext;

        public Handler(IEkmekBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(
                user => user.TelegramId == request.TelegramId,
                cancellationToken: cancellationToken);
            if (user != null)
            {
                return user;
            }

            user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                TelegramId = request.TelegramId
            };
            _dbContext.Users.Add(user);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            // send notification about user created
            return user;
        }
    }
}