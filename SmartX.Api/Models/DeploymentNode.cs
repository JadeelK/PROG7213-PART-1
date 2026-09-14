namespace SmartX.Api.Models
{
    // A node in the nested device-deployment tree,
    // e.g. Facility A -> Zone 1 -> Sub-Zone B.
    public class DeploymentNode
    {
        public string Name { get; set; } = string.Empty;
        public List<DeploymentNode> Children { get; set; } = new();
    }
}