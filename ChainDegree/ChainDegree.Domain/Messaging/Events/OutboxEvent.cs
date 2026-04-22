using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.Domain.Messaging.Events;

public abstract class OutboxEvent
{
    public int Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    /// <summary>
    /// JSON payload chứa dữ liệu cần xử lý
    /// </summary>
    public string Payload { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public bool IsProcessed { get; set; } = false;
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; } = 0;

    private const int MAX_RETRIES = 5;

    public bool CanRetry => RetryCount < MAX_RETRIES;
    public bool ShouldProcess => !IsProcessed && CanRetry;

    // Phương thức helper để set payload an toàn
    public void SetPayload<T>(T data)
    {
        Payload = System.Text.Json.JsonSerializer.Serialize(data);
    }

    // Phương thức helper để parse payload
    public T? GetPayload<T>()
    {
        if (string.IsNullOrEmpty(Payload))
            return default;

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(Payload);
        }
        catch
        {
            return default;
        }
    }
}
