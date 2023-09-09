using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBoards.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<MyBoardsContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

var users = dbContext.Users.ToList();
if (!users.Any())
{
    var user1 = new User()
    {
        Email = "stefan@test.pl",
        FullName = "Stefan User",
        Address = new Address()
        {
            City = "Bia³ystok",
            Street = "Sienkiewicza"
        }
    };

    var user2 = new User()
    {
        Email = "adam@test.pl",
        FullName = "Adam User",
        Address = new Address()
        {
            City = "Bia³ystok",
            Street = "Pietrewicza"
        }
    };

    dbContext.Users.AddRange(user1, user2);

    dbContext.SaveChanges();
}

app.MapGet("firstExervice", async (MyBoardsContext db) =>
{
    var epics = await db.Epics
    .Where(e => e.State.Value == "On Hold")
    .OrderBy(e => e.Priority)
    .ToListAsync();

    return epics;
});

app.MapGet("secondExercise", async (MyBoardsContext db) =>
{
    var authorWithMaximumComments = db.Comments
    .GroupBy(c => c.AuthorId)
    .Select(c => new { authorId = c.Key, count = c.Count() })
    .OrderByDescending(c => c.count)
    .First();

    var author = db.Users
    .First(u => u.Id == authorWithMaximumComments.authorId);

    return author;
});

app.MapPost("update", async (MyBoardsContext db) =>
{
    var epic = await db.Epics.FirstAsync(epic => epic.Id == 1);

    epic.Area = "Updated area";
    epic.Priority = 1;
    epic.StartDate = DateTime.Now;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("create", async (MyBoardsContext db) =>
{
    Tag tagMVC = new Tag()
    {
        Value = "MVC"
    };
    Tag tagASP = new Tag()
    {
        Value = "ASP"
    };

    var tags = new List<Tag>() { tagASP, tagMVC };

    await db.Tags.AddRangeAsync(tags);
    await db.SaveChangesAsync();

    return tags;
});

app.MapPost("createUser", async (MyBoardsContext db) =>
{
    var address = new Address()
    {
        Id = Guid.NewGuid(),
        City = "Kraków",
        Country = "Poland",
        Street = "D³uga"
    };

    var user = new User()
    {
        Email = "userAdd@test.com",
        FullName = "Test User",
        Address = address
    };

    await db.Users.AddAsync(user);

    await db.SaveChangesAsync();

    return user;
});

app.MapGet("getUserComments", async (MyBoardsContext db) =>
{
    var userComments = await db.Users
    .Include(u => u.Comments)
    .ThenInclude(c => c.WorkItem)
    .Include(u => u.Address)
    .FirstAsync(u => u.Id == Guid.Parse("5CB27C3F-32D9-4474-CBC2-08DA10AB0E61"));

    return userComments;
});

app.MapDelete("deleteWorkItemTags", async (MyBoardsContext db) =>
{
    var workItemTags12 = await db.WorkItemTag.Where( c => c.WorkItemId == 12).ToListAsync();
    db.WorkItemTag.RemoveRange(workItemTags12);

    var workItem16 = await db.WorkItemTag.Where(c => c.WorkItemId == 16).ToListAsync();

    db.RemoveRange(workItem16);

    await db.SaveChangesAsync();

});

app.MapDelete("deleteUser", async (MyBoardsContext db) =>
{
    var userId = Guid.Parse("6EB04543-F56B-4827-CC11-08DA10AB0E61");

    var comments = await db.Comments.Where( c => c.AuthorId == userId).ToListAsync();
    db.Comments.RemoveRange(comments);

    var users = await db.Users
    .Include(u => u.Comments)
    .FirstAsync(c => c.Id == userId);

    db.Users.RemoveRange(users);

    await db.SaveChangesAsync();

});

app.MapGet("data", async (MyBoardsContext db) =>
{
    var user = await db.Users.FirstAsync(u => u.Id == Guid.Parse("6AFC3A1D-CF04-4D8A-CBCF-08DA10AB0E61"));

    var entries1 = db.ChangeTracker.Entries();

    user.Email = "test@test.com";
    
    var entries2 = db.ChangeTracker.Entries();

    db.SaveChanges();

});

app.Run();
