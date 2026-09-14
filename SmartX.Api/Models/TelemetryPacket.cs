namespace SmartX.Api.Models
{
    // Generic wrapper so any sensor value type (float, int, bool) can be
    // handled uniformly by the gateway without boxing/unboxing overhead.
    public class TelemetryPacket<T>
    {
        public string SensorId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public T Value { get; set; } = default!;
        public string Unit { get; set; } = string.Empty;

        public TelemetryPacket() { }

        public TelemetryPacket(string sensorId, T value, string unit)
        {
            SensorId = sensorId;
            Value = value;
            Unit = unit;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString() => $"[{Timestamp:HH:mm:ss}] {SensorId}: {Value}{Unit}";
    }
}