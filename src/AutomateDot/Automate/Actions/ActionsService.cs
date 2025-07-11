using AutomateDot.Configurations;
using AutomateDot.Data.Enums;

namespace AutomateDot.Actions;

public class ActionsService(SendWebhookAction SendWebhookAction, GotifyAction GotifyAction, ScriptAction ScriptAction)
{
    public async Task Execute(ActionType actionType, string configuration)
    {
        switch (actionType)
        {
            case ActionType.SendWebhook:
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<SendWebhookConfiguration>(configuration);
                    await SendWebhookAction.ExecuteAsync(config!);
                    break;
                }
            case ActionType.Gotify:
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<GotifyConfiguration>(configuration);
                    await GotifyAction.ExecuteAsync(config!);
                    break;
                }
            case ActionType.Script:
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<ScriptConfiguration>(configuration);
                    await ScriptAction.ExecuteAsync(config!);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}