using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    // Thread-safe-enough in-memory store of registered sensors for this simulation.
    public class SensorRegistry
    {
        private readonly List<SensorRegistration> _sensors = new();
        public IReadOnlyList<SensorRegistration> All => _sensors;

        public void Add(SensorRegistration sensor) => _sensors.Add(sensor);
        public SensorRegistration? Find(string id) => _sensors.FirstOrDefault(s => s.Id == id);
    }
}