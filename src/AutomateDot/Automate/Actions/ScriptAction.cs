using AutomateDot.Configurations;

using System.Diagnostics;

namespace AutomateDot.Actions;

public sealed class ScriptAction(ILogger<ScriptAction> Logger) : IActionHandler<ScriptConfiguration>
{
    public Task ExecuteAsync(ScriptConfiguration configuration)
    {
        Logger.LogInformation("ScriptAction starting new process");

        var process = new Process();
        process.StartInfo.FileName = configuration.File;
        process.StartInfo.Arguments = configuration.Arguments;

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