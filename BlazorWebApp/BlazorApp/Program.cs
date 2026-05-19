using BlazorApp.Components;
using BlazorApp.Data;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseInMemoryDatabase("BlazorContactsDemoDb"));
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("contacts-api", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseAntiforgery();

var contactsApi = app.MapGroup("/api/v1/contacts")
    .RequireRateLimiting("contacts-api");

contactsApi.MapGet("/", async (ContactDbContext dbContext) =>
    await dbContext.Contacts.AsNoTracking().ToListAsync());

contactsApi.MapGet("/{id:int}", async (int id, ContactDbContext dbContext) =>
{
    var contact = await dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.ContactId == id);
    return contact is null ? Results.NotFound() : Results.Ok(contact);
});

contactsApi.MapPost("/", async (ContactRequest request, ContactDbContext dbContext) =>
{
    if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["FullName"] = ["FullName is required."],
            ["Email"] = ["Email is required."]
        });
    }

    var contact = new BlazorApp.Models.Contact
    {
        FullName = request.FullName,
        Email = request.Email,
        Address = request.Address,
        City = request.City,
        State = request.State,
        Zip = request.Zip
    };

    dbContext.Contacts.Add(contact);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/api/v1/contacts/{contact.ContactId}", contact);
});

contactsApi.MapPut("/{id:int}", async (int id, ContactRequest request, ContactDbContext dbContext) =>
{
    if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["FullName"] = ["FullName is required."],
            ["Email"] = ["Email is required."]
        });
    }

    var contact = await dbContext.Contacts.FindAsync(id);
    if (contact is null)
    {
        return Results.NotFound();
    }

    contact.FullName = request.FullName;
    contact.Email = request.Email;
    contact.Address = request.Address;
    contact.City = request.City;
    contact.State = request.State;
    contact.Zip = request.Zip;

    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

contactsApi.MapDelete("/{id:int}", async (int id, ContactDbContext dbContext) =>
{
    var contact = await dbContext.Contacts.FindAsync(id);
    if (contact is null)
    {
        return Results.NotFound();
    }

    dbContext.Contacts.Remove(contact);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

internal sealed record ContactRequest(
    string? FullName,
    string? Email,
    string? Address,
    string? City,
    string? State,
    string? Zip);
