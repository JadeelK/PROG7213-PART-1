namespace SmartX.Api.Models
{
    public class SensorRegistration
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MacAddress { get; set; } = string.Empty;
        public string DeploymentLocation { get; set; } = string.Empty;
        public SensorCategory Category { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public List<string> AttachedFileNames { get; set; } = new();
        public bool LocationValidated { get; set; } = false;
    }
}