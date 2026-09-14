namespace SmartX.Api.Models
{
    public static class DeploymentTreeValidator
    {
        // Recursively walks the deployment tree to confirm that a given
        // path (e.g. ["Facility A","Zone 1","Sub-Zone B"]) is a real,
        // safely-configured location in the hierarchy.
        public static bool ValidatePath(DeploymentNode node, List<string> path, int depth = 0)
        {
            if (depth >= path.Count) return false;
            if (!string.Equals(node.Name, path[depth], StringComparison.OrdinalIgnoreCase)) return false;
            if (depth == path.Count - 1) return true;

            foreach (var child in node.Children)
            {
                if (ValidatePath(child, path, depth + 1)) return true;
            }
            return false;
        }
    }
}