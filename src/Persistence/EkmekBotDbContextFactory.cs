using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class EkmekBotDbContextFactory : DesignTimeDbContextFactoryBase<EkmekBotDbContext>
{
    protected override EkmekBotDbContext CreateNewInstance(DbContextOptions<EkmekBotDbContext> options)
    {
        return new EkmekBotDbContext(options);
    }
}