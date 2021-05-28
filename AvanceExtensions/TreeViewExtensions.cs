using System;
using System.Linq;
using System.Windows.Forms;

public static class TreeNodeCollectionUtils
{
    public static TreeNode FindTreeNodeByFullPath(this TreeNodeCollection collection, string fullPath, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        var foundNode = collection.Cast<TreeNode>().FirstOrDefault(tn => string.Equals(tn.FullPath, fullPath, comparison));
        if (null == foundNode)
        {
            foreach (var childNode in collection.Cast<TreeNode>())
            {
                var foundChildNode = FindTreeNodeByFullPath(childNode.Nodes, fullPath, comparison);
                if (null != foundChildNode)
                {
                    return foundChildNode;
                }
            }
        }

        return foundNode;
    }
}
