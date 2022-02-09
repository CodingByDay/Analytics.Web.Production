using System;
using System.Web.UI;
using System.Xml;
using DevExpress.Web;

namespace AdaptiveSlideNavigation.UserControls {
    public partial class Navigation : System.Web.UI.UserControl {
        protected void NavigationTreeView_NodeDataBound(object source, DevExpress.Web.TreeViewNodeEventArgs e) {
            XmlNode dataNode = ((e.Node.DataItem as IHierarchyData).Item as XmlNode);
            if(dataNode.Name == "group")
                e.Node.NodeStyle.CssClass += " group";
            if(dataNode.ParentNode != null && dataNode.ParentNode.Name != "group")
                e.Node.NodeStyle.CssClass += " introPage";
        }

        protected void NavigationTreeView_PreRender(object sender, EventArgs e) {
            TreeViewNode node = NavigationTreeView.SelectedNode;
            if(node != null) {
                NavigationTreeView.ExpandToNode(node);
                node.Expanded = true;
                while(node != null) {
                    if(node.Parent != null && node.Parent.Parent == null)
                        node.Expanded = false;
                    node = node.Parent;
                }
            }
        }
    }
}