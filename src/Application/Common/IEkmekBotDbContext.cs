using Domain.New.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.New.Common;

public interface IEkmekBotDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}