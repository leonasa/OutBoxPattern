namespace Shared.InboxServices;

public sealed class InboxOptions
{
    public const string ConfigurationSectionName = "Inbox";
    public bool Enabled { get; set; } = true;
    public int MaxRetries { get; set; } = 3;
    public int RetryDelay { get; set; } = 1000;
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);
}