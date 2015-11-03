using System;
using umbraco.BusinessLogic;
using umbraco.BusinessLogic.Actions;

namespace CogItemUsage
{
    public class DeleteAction : ApplicationBase
    {
        public DeleteAction()
        {
            umbraco.cms.presentation.Trees.BaseTree.BeforeNodeRender += BaseTree_BeforeNodeRender;
        }

        void BaseTree_BeforeNodeRender(ref umbraco.cms.presentation.Trees.XmlTree sender, ref umbraco.cms.presentation.Trees.XmlTreeNode node, EventArgs e)
        {
            if (node.NodeType == "content")
            {
                if (node.Menu != null)
                {
                    // Replacing the built in delete with our own
                    int currentDeleteIndex = node.Menu.IndexOf(ActionDelete.Instance);
                    node.Menu.Remove(ActionDelete.Instance);
                    node.Menu.Insert(currentDeleteIndex, new DeleteActionItem());
                }
            }
        }
    }
}