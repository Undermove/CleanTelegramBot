using Application.New.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.New.Users.Queries.GetUsersList;

public class GetUsersListQuery: IRequest<UsersListVm>
{
    public class Handler: IRequestHandler<GetUsersListQuery, UsersListVm>
    {
        private readonly IEkmekBotDbContext _dbContext;

        public Handler(IEkmekBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UsersListVm> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var response = new UsersListVm
            {
                Users = await _dbContext.Users.ToListAsync(cancellationToken) 
            };
            return response;
        }
    }
}