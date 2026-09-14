using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class TelemetryHistoryStore
    {
        // Jagged array: each row is one polling-cycle batch, and each batch
        // can contain a different number of sensor readings, before being
        // optimised into a flat generic List<T> for processing.
        private float[][] _soilMoistureBatches = Array.Empty<float[]>();
        private readonly List<TelemetryPacket<float>> _processedReadings = new();

        public void SeedMockBatches(int batchCount, int sensorsPerBatchMax)
        {
            var rnd = new Random();
            _soilMoistureBatches = new float[batchCount][];

            for (int i = 0; i < batchCount; i++)
            {
                int sensorsThisBatch = rnd.Next(3, sensorsPerBatchMax + 1);
                _soilMoistureBatches[i] = new float[sensorsThisBatch];

                for (int j = 0; j < sensorsThisBatch; j++)
                {
                    _soilMoistureBatches[i][j] = (float)Math.Round(rnd.NextDouble() * 100, 2);
                }
            }

            FlattenIntoProcessedList();
        }

        private void FlattenIntoProcessedList()
        {
            _processedReadings.Clear();
            for (int batch = 0; batch < _soilMoistureBatches.Length; batch++)
            {
                for (int sensor = 0; sensor < _soilMoistureBatches[batch].Length; sensor++)
                {
                    var packet = new TelemetryPacket<float>(
                        sensorId: $"SOIL-{batch:D2}-{sensor:D2}",
                        value: _soilMoistureBatches[batch][sensor],
                        unit: "%"
                    );
                    _processedReadings.Add(packet);
                }
            }
        }

        public List<TelemetryPacket<float>> GetProcessedReadings() => _processedReadings;
        public float[][] GetRawBatches() => _soilMoistureBatches;
    }
}