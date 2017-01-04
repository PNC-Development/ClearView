using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    /// <summary>
    /// Dynamically built DHTML drop down menu navigation control.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// To control the maximum number of columns in a menu, add a tag such as follows to appSettings
    /// &lt;add key="Services Max Column Count" value="4"/&gt;
    /// where "Services" is the name of the page being displayed as the root node of the menu
    /// and the value is the maximum number of desired columns
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// If the number of columns specified is greater than the number of subheadings to put in the menu, the # of
    /// columns generated will be the number of subheadings, with one subheading in each column
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// If no max # of columns is specified, or a value &lt; 1 is specified, the control will default is 2 columns
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The following equation determines the number of subheadings put into a menu column: Ceil(x/y)
    /// Where x = # of subheadings
    ///       y = maximum # of columns
    /// It is possible to specify a maximum number of columns big enough that not all columns will be used.
    /// Example: 4 columns specified as maximum, and 5 subheadings exist. The equation will return 2, causing
    /// the control to only render 3 total columns for the menu, with 2 subheadings in the first 2 columns and 1
    /// in the 3rd column.
    /// </description>
    /// </item>
    /// </list>
    ///</remarks>
    public partial class TopNavMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
            {
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                oApplication = new Applications(intProfile, dsn);
                oPage = new Pages(intProfile, dsn);
                oPageControl = new PageControls(intProfile, dsn);
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                    intPage = Int32.Parse(Request.QueryString["pageid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                RegisterJavaScript();
                RegisterStyleSheets();
                this.Visible = true;
            }
            else //not logged in, so don't display menu bar
            {
                this.Visible = false;
            }
            
        }
        protected override void CreateChildControls()
        {
            List<Pair> rootNodes = GetMenuData();
            for (int rootNodeIndex = 0; rootNodeIndex < rootNodes.Count; rootNodeIndex++)
			{
                Pair rootNode = rootNodes[rootNodeIndex];
                ListItem rootNodeItem = (ListItem)rootNode.First;
                List<Pair> rootNodeMenuData = (List<Pair>)rootNode.Second;
                int columnCount;
                if (!Int32.TryParse(ConfigurationManager.AppSettings[String.Format("{0} Max Column Count", rootNodeItem.Text)], out columnCount))
                {
                    columnCount = 2;
                }

                HtmlGenericControl menuDiv = GenerateMenuDiv(columnCount, rootNodeItem, rootNodeMenuData);
                if (menuDiv.Controls.Count != 0)
                {
                    this.Controls.Add(menuDiv);
                    //must add menuDiv to Controls collection before generating root anchor,
                    //because the correct ClientID doesn't get set until after addition to the collection
                    this.Controls.Add(GenerateRootNodeAnchor(menuDiv.ClientID, rootNodeItem, rootNodeMenuData));
                }
                else
                {
                    //Root node doesn't have a corresponding menu,
                    //so don't add the menu div to controls & generate anchor w/o rel attribute
                    this.Controls.Add(GenerateRootNodeAnchor(null, rootNodeItem, rootNodeMenuData));
                }
            }
            base.CreateChildControls();
        }
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", "menuContainer");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.RenderChildren(writer);
            writer.RenderEndTag(); //</div>
            writer.AddAttribute("id", "navOrangeBarSpacer");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag(); //</div>
        }

        #region Private Methods
        private static void AddSubGroupToMenuColumn(HtmlGenericControl column, Pair currentSubGroup)
        {
            ListItem subGroupHeader = (ListItem)currentSubGroup.First;
            List<ListItem> subGroupItems = (List<ListItem>)currentSubGroup.Second;
            StringBuilder headerText = new StringBuilder(subGroupHeader.Text);
            headerText.Replace(" ", "&nbsp;");
            HtmlGenericControl subGroupHeaderControl = new HtmlGenericControl("h2");
            subGroupHeaderControl.InnerHtml = headerText.ToString();
            if (subGroupItems.Count > 0)
            {
                BulletedList subGroupList = new BulletedList();
                subGroupList.CssClass = "innerList";
                subGroupList.Items.AddRange(subGroupItems.ToArray());
                subGroupList.DisplayMode = BulletedListDisplayMode.HyperLink;
                column.Controls.Add(subGroupHeaderControl);
                column.Controls.Add(subGroupList);
            }
            else
            {
                HtmlGenericControl subGroupHeaderAnchor = new HtmlGenericControl("a");
                subGroupHeaderAnchor.Attributes.Add("href", subGroupHeader.Value);
                subGroupHeaderAnchor.Controls.Add(subGroupHeaderControl);
                column.Controls.Add(subGroupHeaderAnchor);
            }
        }
        private static HtmlGenericControl GenerateMenuDiv(int maxNumberOfColumns, ListItem rootNodeItem, List<Pair> rootNodeMenuData)
        {
            HtmlGenericControl column;
            HtmlGenericControl menu = new HtmlGenericControl("div");
            menu.Attributes.Add("class", "anylinkcsscols dropDownMenu");

            //if # of subGroups is less than the column count, reduce the column count to match the # of subgroups
            if (rootNodeMenuData.Count < maxNumberOfColumns)
            {
                maxNumberOfColumns = rootNodeMenuData.Count;
            }
            //just in case someone gets sneaky in the web.config and tries to enter 0/negative column value
            if (maxNumberOfColumns < 1)
            {
                maxNumberOfColumns = 2;
            }
            int maxSubGroupsPerColumn = (int)Math.Ceiling((double)rootNodeMenuData.Count / (double)maxNumberOfColumns);
            
            int subGroupIndex = 0;
            int columnCount = 0;
            while(subGroupIndex < rootNodeMenuData.Count && columnCount < maxNumberOfColumns)
            {
                columnCount++;
                column = new HtmlGenericControl("div");
                column.Attributes.Add("class", "column");

                int subGroupsInColumn = 0;
                while(subGroupIndex < rootNodeMenuData.Count && subGroupsInColumn < maxSubGroupsPerColumn )
                {
                    Pair currentSubGroup = rootNodeMenuData[subGroupIndex];
                    AddSubGroupToMenuColumn(column, currentSubGroup);
                    subGroupIndex++;
                    subGroupsInColumn++;
                }
                menu.Controls.Add(column);
            }

            return menu;
        }
        private static HyperLink GenerateRootNodeAnchor(string relAttributeValue, ListItem rootNodeItem, List<Pair> rootNodeMenuData)
        {
            HyperLink rootLink = new HyperLink();
            StringBuilder rootLinkText = new StringBuilder(rootNodeItem.Text);
            rootLinkText.Replace(" ", "&nbsp;");
            rootLink.Text = rootLinkText.ToString();
            
            if (rootNodeMenuData.Count > 0)
            {
                rootLink.Attributes.Add("rel", relAttributeValue);
                rootLink.CssClass = "navRootAnchor";
            }
            else
            {
                rootLink.NavigateUrl = rootNodeItem.Value;
            }
            return rootLink;
        }
        private List<Pair> GetMenuData()
        {
            //first object in pair is listitem containing root menu node's name/url, second object is list of Pairs from GetSubmenuData
            List<Pair> menus = new List<Pair>();
            DataSet rootNodes;
//TODO: This is here because we don't want to currently break the development env. menu system. The debug conditional can probably go away eventually.
#if DEBUG
            rootNodes = oPage.Gets(intApplication, intProfile, 107, 1, 1);
#else
            rootNodes = oPage.Gets(intApplication, intProfile, 0, 1, 1);       
#endif
            foreach (DataRow rootNode in rootNodes.Tables[0].Rows)
            {
                List<Pair> menuData = GetSubmenuData(Convert.ToInt32(rootNode["pageid"]));
                menus.Add(new Pair(new ListItem(rootNode["menutitle"].ToString(), oPage.GetFullLink(Convert.ToInt32(rootNode["pageid"]))), menuData));
            }

            return menus;
        }
        private List<Pair> GetSubmenuData(int parentPageId)
        {
            //First object in pair is subheading listitem, second object is a List of ListItems that should live under subheading
            List<Pair> subHeadings = new List<Pair>();
            DataSet subHeadingsData;

            subHeadingsData = oPage.Gets(intApplication, intProfile, parentPageId, 1, 1);
            foreach (DataRow subHeading in subHeadingsData.Tables[0].Rows)
            {
                Pair subHeadingElements = new Pair();
                List<ListItem> subHeadingChildren = new List<ListItem>();
                int subHeadingPageId = Convert.ToInt32(subHeading["pageid"]);
                subHeadingElements.First = new ListItem(subHeading["menutitle"].ToString(), oPage.GetFullLink(subHeadingPageId));
                
                DataSet subHeadingChildPages = oPage.Gets(intApplication, intProfile, subHeadingPageId, 1, 1);
                foreach (DataRow subHeadingChild in subHeadingChildPages.Tables[0].Rows)
                {
                    subHeadingChildren.Add(new ListItem(subHeadingChild["menutitle"].ToString(), oPage.GetFullLink(Convert.ToInt32(subHeadingChild["pageid"]))));
                }
                subHeadingElements.Second = subHeadingChildren;
                subHeadings.Add(subHeadingElements);
            }
            return subHeadings;

        }
        private void RegisterJavaScript()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("anyLinkMenuScript"))
            {
                StringBuilder anyLinkMenuScript = new StringBuilder();
                anyLinkMenuScript.AppendLine("<script type=\"text/javascript\" src=\"/javascript/anylinkcssmenu.js\">");
                anyLinkMenuScript.AppendLine("/***********************************************");
                anyLinkMenuScript.AppendLine("* AnyLink CSS Menu script v2.0- © Dynamic Drive DHTML code library (www.dynamicdrive.com)");
                anyLinkMenuScript.AppendLine("* This notice MUST stay intact for legal use");
                anyLinkMenuScript.AppendLine("* Visit Project Page at http://www.dynamicdrive.com/dynamicindex1/anylinkcss.htm for full source code");
                anyLinkMenuScript.AppendLine("***********************************************/");
                anyLinkMenuScript.AppendLine("</script>");
                anyLinkMenuScript.AppendLine("<script type=\"text/javascript\">");
                anyLinkMenuScript.AppendLine("anylinkcssmenu.init('navRootAnchor')");
                anyLinkMenuScript.AppendLine("</script>");
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "anyLinkMenuScript", anyLinkMenuScript.ToString(), false);
            }
        }
        private void RegisterStyleSheets()
        {
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"/css/anylinkcssmenu.css\" />"));
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"/css/menuStyles.css\" />"));
        }
        #endregion

        #region Private Members
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Applications oApplication;
        protected Pages oPage;
        protected PageControls oPageControl;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intPage = 0;
        #endregion
    }
}