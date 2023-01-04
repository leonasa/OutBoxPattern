namespace OutBoxPattern.Services;

public sealed record OutboxOptions(string ConfigurationSectionName = "Outbox", bool Enabled = true, int MaxRetries = 3, int RetryDelay = 1000, TimeSpan Interval = default);