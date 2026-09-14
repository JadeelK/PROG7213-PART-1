namespace SmartX.Api.Models
{
    // Demonstrates operator overloading for direct aggregation and
    // delta comparison of smart meter readings (e.g. Meter3 = Meter1 + Meter2).
    public class PowerMeterReading
    {
        public string MeterId { get; set; } = string.Empty;
        public int Watts { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static PowerMeterReading operator +(PowerMeterReading a, PowerMeterReading b)
        {
            return new PowerMeterReading
            {
                MeterId = $"{a.MeterId}+{b.MeterId}",
                Watts = a.Watts + b.Watts,
                Timestamp = DateTime.UtcNow
            };
        }

        public static PowerMeterReading operator -(PowerMeterReading a, PowerMeterReading b)
        {
            return new PowerMeterReading
            {
                MeterId = $"{a.MeterId}-{b.MeterId}",
                Watts = a.Watts - b.Watts,
                Timestamp = DateTime.UtcNow
            };
        }

        public static bool operator >(PowerMeterReading a, PowerMeterReading b) => a.Watts > b.Watts;
        public static bool operator <(PowerMeterReading a, PowerMeterReading b) => a.Watts < b.Watts;

        public override string ToString() => $"{MeterId}: {Watts}W";
    }
}