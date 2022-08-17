using System;
using System.Threading.Tasks;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Persistence.IntegrationTests;

public class EkmekBotDbContextTests
{
#pragma warning disable CS8618
    private EkmekBotDbContext _context;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EkmekBotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new EkmekBotDbContext(options);
    }

    [Test]
    public async Task ShouldGetUserAfterSave()
    {
        User user = new User
        {
            UserId = Guid.NewGuid().ToString(), 
            TelegramId = 1111, 
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var savedUser = await _context.Users.FindAsync(user.UserId);
        savedUser.Should().Be(user);
    }
}