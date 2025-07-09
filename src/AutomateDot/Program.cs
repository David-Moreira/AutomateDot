using AutomateDot.Actions;
using AutomateDot.Components;
using AutomateDot.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

//For now we'll be using dbContext directly in the pages to develop faster. Ideally we would encapsulate it in a repository/service.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite($"DataSource=automatedot.db");
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<ActionsService>();
builder.Services.AddScoped<SendWebhookAction>();
builder.Services.AddScoped<GotifyAction>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
app.Run();