namespace EasyCron.Api.Models
{
    public enum JobType
    {
        None = 0,
        RecurringJob = 1,
        FireAndForgetJob = 2,
        DelayedJob = 3,
        ContinuationsJob = 4
    }
}
