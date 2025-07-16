using AutomateDot.Actions;
using AutomateDot.Automate;
using AutomateDot.Components;
using AutomateDot.Components.Automation;
using AutomateDot.Components.Automation.Actions;
using AutomateDot.Components.Automation.Triggers;
using AutomateDot.Data;
using AutomateDot.Services;

using Hangfire;
using Hangfire.Storage.SQLite;

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
        options.UseSqlite($"DataSource={builder.Configuration.GetConnectionString("Sqlite")}");
    });

    builder.Services.AddHttpClient();
    builder.Services.AddScoped<AutomationService>();
    builder.Services.AddScoped<AutomationExecutionService>();
    builder.Services.AddScoped<ActionsService>();
    builder.Services.AddScoped<SendWebhookAction>();
    builder.Services.AddScoped<GotifyAction>();
    builder.Services.AddScoped<ScriptAction>();

    AddAutomateServices(builder.Services);
    AddHangfire(builder.Services, builder.Configuration);

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

    app.UseHangfireDashboard();

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

static void AddAutomateServices(IServiceCollection services)
{
    var actionConfigurationTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => typeof(IActionConfiguration).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
        .ToList();

    foreach (var actionConfigurationType in actionConfigurationTypes)
    {
        services.AddTransient(actionConfigurationType);
    }

    var actionHandlerTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t =>
            t.IsClass &&
            !t.IsAbstract &&
            t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IActionHandler<>)
            ))
        .ToList();

    foreach (var actionHandlerType in actionHandlerTypes)
    {
        services.AddTransient(actionHandlerType);
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

        var existingActionHandlerType = actionHandlerTypes.FirstOrDefault(x =>
            x.GetInterfaces().Any(
                y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IActionHandler<>)
                && y.GetGenericArguments()[0] == genericType));
        if (existingActionHandlerType is null)
        {
            Log.Warning("No matching IActionHandler found for {FormActionType}. Skipping registration.", formActionType.Name);
            continue;
        }

        var actionNameAttribute = formActionType
            .GetCustomAttribute<AutomateDefinitionAttribute>();

        var formId = actionNameAttribute?.Id ?? formActionType.AssemblyQualifiedName!;
        var formName = actionNameAttribute?.Name ?? formActionType.Name;
        AutomateFormRegistry.ActionForms.Add(new AutomateFormDefinition(formId, formName, formActionType, existingConfigurationType, existingActionHandlerType));
    }

    var triggerConfigurationTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => typeof(ITriggerConfiguration).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
        .ToList();

    foreach (var triggerConfigurationType in triggerConfigurationTypes)
    {
        services.AddTransient(triggerConfigurationType);
    }

    var formTriggerTypes = AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t =>
            t.IsClass &&
            !t.IsAbstract &&
            t.BaseType is not null &&
            t.BaseType.IsGenericType &&
            t.BaseType.GetGenericTypeDefinition() == typeof(TriggerFormBase<>)
        )
        .ToList();

    foreach (var formTriggerType in formTriggerTypes)
    {
        var genericType = formTriggerType.BaseType?.GetGenericArguments().FirstOrDefault();
        var existingConfigurationType = triggerConfigurationTypes.FirstOrDefault(x => x == genericType);
        if (existingConfigurationType is null)
        {
            Log.Warning("No matching ITriggerConfiguration found for {FormTriggerType}. Skipping registration.", formTriggerType.Name);
            continue;
        }

        var actionNameAttribute = formTriggerType
            .GetCustomAttribute<AutomateDefinitionAttribute>();

        var formId = actionNameAttribute?.Id ?? formTriggerType.AssemblyQualifiedName!;
        var formName = actionNameAttribute?.Name ?? formTriggerType.Name;
        AutomateFormRegistry.TriggerForms.Add(new AutomateFormDefinition(formId, formName, formTriggerType, existingConfigurationType, null));
    }
}

static void AddHangfire(IServiceCollection services, IConfiguration configuration)
{
    services.AddHangfire(hangfireConfiguration => hangfireConfiguration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSQLiteStorage(configuration.GetConnectionString("HangfireSqlite")));

    services.AddHangfireServer();
}