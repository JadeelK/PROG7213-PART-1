namespace SmartX.Api.Models
{
    public static class DeploymentTreeSeed
    {
        public static DeploymentNode Root { get; } = new DeploymentNode
        {
            Name = "Facility A",
            Children = new List<DeploymentNode>
            {
                new DeploymentNode
                {
                    Name = "Zone 1",
                    Children = new List<DeploymentNode>
                    {
                        new DeploymentNode { Name = "Sub-Zone A" },
                        new DeploymentNode { Name = "Sub-Zone B" }
                    }
                },
                new DeploymentNode
                {
                    Name = "Zone 2",
                    Children = new List<DeploymentNode>
                    {
                        new DeploymentNode { Name = "Sub-Zone C" }
                    }
                }
            }
        };
    }
}