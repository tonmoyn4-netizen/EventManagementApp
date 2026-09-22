
using EventServer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create the database / apply migrations automatically when the server starts
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var events = app.MapGroup("/api/events");

// GET all (with attendees)
events.MapGet("/", async (AppDbContext db) =>
    await db.Events.Include(e => e.Attendees).ToListAsync());

// GET by id
events.MapGet("/{id:int}", async (int id, AppDbContext db) =>
    await db.Events.Include(e => e.Attendees).FirstOrDefaultAsync(e => e.Id == id)
        is Event ev ? Results.Ok(ev) : Results.NotFound());

// POST - one SaveChanges saves the event AND all its attendees
events.MapPost("/", async (Event ev, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(ev.Title))
        return Results.BadRequest("Title is required.");

    db.Events.Add(ev);
    await db.SaveChangesAsync();
    return Results.Created($"/api/events/{ev.Id}", ev);
});

// PUT - update the event, replace its attendees
events.MapPut("/{id:int}", async (int id, Event input, AppDbContext db) =>
{
    var ev = await db.Events.Include(e => e.Attendees)
        .FirstOrDefaultAsync(e => e.Id == id);

    if (ev is null)
        return Results.NotFound();

    if (string.IsNullOrWhiteSpace(input.Title))
        return Results.BadRequest("Title is required.");

    ev.Title = input.Title;
    ev.EventDate = input.EventDate;
    ev.IsActive = input.IsActive;
    ev.ImageUrl = input.ImageUrl;

    // Simple way to update the details: remove all old attendees, add the new list
    ev.Attendees.Clear();
    foreach (var a in input.Attendees)
        ev.Attendees.Add(new Attendee { Name = a.Name, Email = a.Email });

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE - the attendees are deleted automatically (cascade delete)
events.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var ev = await db.Events.FindAsync(id);
    if (ev is null)
        return Results.NotFound();

    db.Events.Remove(ev);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
