using DevExpress.Web;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;

namespace Dash
{
    public partial class Search : System.Web.UI.UserControl
    {
        protected void SearchResults_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                var text = e.Parameter;
                var results = DoSearch(text);
                if (results.Count > 0)
                {
                    BindSearchResultsNavBar(results);
                    SearchResultsNavBar.Visible = true;
                    noResultsContainer.Visible = false;
                }
                else
                {
                    SearchResultsNavBar.Visible = false;
                    requestText.InnerHtml = text;
                    noResultsContainer.Visible = true;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void BindSearchResultsNavBar(IList<SearchResultItem> data)
        {
            try
            {
                var group = new NavBarGroup();
                SearchResultsNavBar.Groups.Add(group);
                foreach (var item in data)
                {
                    var navBarItem = new NavBarItem(
                        text: string.Format("<span class='title'>{0}</span> <span class='tags'>Tags: {1}</span>", item.Title, GetFromattedTags(item.Tags)),
                        name: "",
                        imageUrl: "",
                        navigateUrl: item.Url
                    );
                    group.Items.Add(navBarItem);
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private string GetFromattedTags(string tags)
        {
            try
            {
                return string.Concat(tags.Split(',').Select(i => string.Format("<span class='tag'>{0}</span>", i.Trim())));
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

        private IList<SearchResultItem> DoSearch(string text)
        {
            try
            {
                var pagesNodes = XmlDataSource1.GetXmlDocument().GetElementsByTagName("page").OfType<XmlNode>();
                return pagesNodes
                    .Where(n => n.Attributes["Title"].Value.ToUpper().Contains(text.ToUpper())
                        || n.Attributes["Tags"].Value.ToUpper().Contains(text.ToUpper()))
                    .Select(n => new SearchResultItem()
                    {
                        Title = n.Attributes["Title"].Value,
                        Tags = n.Attributes["Tags"].Value,
                        Url = n.Attributes["Url"].Value,
                    }).ToList();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return new List<SearchResultItem>();
            }
        }

        private class SearchResultItem
        {
            public string Title { get; set; }
            public string Tags { get; set; }
            public string Url { get; set; }
        }
    }
}