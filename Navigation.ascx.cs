using DevExpress.Web;
using Sentry;
using System;
using System.Web.UI;
using System.Xml;

namespace Dash
{
    public partial class Navigation : System.Web.UI.UserControl
    {
        protected void NavigationTreeView_NodeDataBound(object source, DevExpress.Web.TreeViewNodeEventArgs e)
        {
            try
            {
                XmlNode dataNode = ((e.Node.DataItem as IHierarchyData).Item as XmlNode);
                if (dataNode.Name == "group")
                    e.Node.NodeStyle.CssClass += " group";
                if (dataNode.ParentNode != null && dataNode.ParentNode.Name != "group")
                    e.Node.NodeStyle.CssClass += " introPage";
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void NavigationTreeView_PreRender(object sender, EventArgs e)
        {
            try
            {
                TreeViewNode node = NavigationTreeView.SelectedNode;
                if (node != null)
                {
                    NavigationTreeView.ExpandToNode(node);
                    node.Expanded = true;
                    while (node != null)
                    {
                        if (node.Parent != null && node.Parent.Parent == null)
                            node.Expanded = false;
                        node = node.Parent;
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void NavigationTreeView_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Initalize the view bag.", "load()", true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void NavigationTreeView_Init(object sender, EventArgs e)
        {
        }

        protected void NavigationTreeView_NodeClick(object source, TreeViewNodeEventArgs e)
        {
        }

        protected void NavigationTreeView_CheckedChanged(object source, TreeViewNodeEventArgs e)
        {
        }

        protected void NavigationTreeView_CheckedChanged1(object source, TreeViewNodeEventArgs e)
        {
        }
    }
}