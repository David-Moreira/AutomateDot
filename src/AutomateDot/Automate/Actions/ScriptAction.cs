using AutomateDot.Configurations;

using System.Diagnostics;

namespace AutomateDot.Actions;

public sealed class ScriptAction(ILogger<ScriptAction> Logger) : IActionHandler<ScriptConfiguration>
{
    public Task ExecuteAsync(ScriptConfiguration configuration, object? triggerPayload = null)
    {
        Logger.LogInformation("ScriptAction starting new process");

        var process = new Process();
        process.StartInfo.FileName = ActionHelper.ReplacePlaceholders(configuration.File, triggerPayload);
        process.StartInfo.Arguments = ActionHelper.ReplacePlaceholders(configuration.Arguments, triggerPayload);

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        var output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();
        if (process.ExitCode == 0)
        {
            Logger.LogInformation("ScriptAction finished successfully");
            Logger.LogInformation("ScriptAction output was: {output}", output);
        }
        else
        {
            Logger.LogError("ScriptAction finished unsuccessfully");
            Logger.LogError("ScriptAction output was: {output}", output);
        }
        process.Dispose();
        return Task.CompletedTask;
    }
}