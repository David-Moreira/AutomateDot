using AutomateDot.Actions;
using AutomateDot.Automate;
using AutomateDot.Components;
using AutomateDot.Components.Automation;
using AutomateDot.Data;
using AutomateDot.Services;

using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

using Serilog;
using Serilog.Events;

using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
#if DEBUG
    .WriteTo.Console()
#endif
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    Addlogging(builder);

    builder.Services.AddControllers();
    builder.Services
        .AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    });

    //For now we'll be using dbContext directly in the pages to develop faster. Ideally we would encapsulate it in a repository/service.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlite($"DataSource={builder.Configuration.GetConnectionString("SqlLite")}");
    });

    builder.Services.AddHttpClient();
    builder.Services.AddScoped<AutomationService>();
    builder.Services.AddScoped<AutomationExecutionService>();
    builder.Services.AddScoped<ActionsService>();
    builder.Services.AddScoped<SendWebhookAction>();
    builder.Services.AddScoped<GotifyAction>();
    builder.Services.AddScoped<ScriptAction>();

    var actionConfigurationTypes = AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => typeof(IActionConfiguration).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
    .ToList();

    foreach (var actionConfigurationType in actionConfigurationTypes)
    {
        builder.Services.AddTransient(actionConfigurationType);
    }

    var formActionTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t =>
            t.IsClass &&
            !t.IsAbstract &&
            t.BaseType is not null &&
            t.BaseType.IsGenericType &&
            t.BaseType.GetGenericTypeDefinition() == typeof(ActionFormBase<>)
        )
        .ToList();

    foreach (var formActionType in formActionTypes)
    {
        var genericType = formActionType.BaseType?.GetGenericArguments().FirstOrDefault();
        var existingConfigurationType = actionConfigurationTypes.FirstOrDefault(x => x == genericType);
        if (existingConfigurationType is null)
        {
            Log.Warning("No matching IActionConfiguration found for {FormActionType}. Skipping registration.", formActionType.Name);
            continue;
        }

        var actionNameAttribute = formActionType
            .GetCustomAttribute<AutomateActionNameAttribute>();

        var formName = actionNameAttribute?.Name ?? formActionType.Name;
        AutomateFormRegistry.ActionForms.Add(new AutomateActionFormDefinition(formName, formActionType, existingConfigurationType));
    }

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

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    app.Run();
}
finally
{
    Log.Warning("Shutting down application and flushing logger...");
    Log.CloseAndFlush();
}

void Addlogging(WebApplicationBuilder builder)
{
    builder.Services.AddLogging(options =>
    {
        options.ClearProviders();
        options.AddSerilog();
    });

    var logBuilder = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext();
#if DEBUG
    logBuilder.WriteTo.Console();
#endif

    Log.Logger = logBuilder.CreateLogger();
}