using NCrontab;

using System.ComponentModel.DataAnnotations;

public class CronExpressionAttribute : ValidationAttribute
{
    public CronExpressionAttribute()
    {
        ErrorMessage = "The cron expression is invalid.";
    }

    public override bool IsValid(object? value)
    {
        if (value is string cron && !string.IsNullOrWhiteSpace(cron))
        {
            var schedule = CrontabSchedule.TryParse(cron);
            if (schedule is not null)
            {
                return true;
            }
            else
                return false;
        }
        return false;
    }
}