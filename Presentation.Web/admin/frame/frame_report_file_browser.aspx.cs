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
using System.Net;

namespace NCC.ClearView.Presentation.Web
{
    public partial class frame_report_file_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private NCC.ClearView.Application.Core.RSDev.ReportingService2005 rs2005;
        protected int intProfile = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            Variables oVariable = new Variables(intEnvironment);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            string strURL = "";
            DataSet dsKey = oFunction.GetSetupValuesByKey(Environment.MachineName + "_REPORTING");
            if (dsKey.Tables[0].Rows.Count > 0)
                strURL = dsKey.Tables[0].Rows[0]["Value"].ToString();
            if (strURL != "")
            {
                System.Net.NetworkCredential oNetwork = new NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                rs2005 = new NCC.ClearView.Application.Core.RSDev.ReportingService2005();
                rs2005.Credentials = oNetwork;
                rs2005.Url = strURL + oVariable.ReportServiceASMX();
                //NCC.ClearView.Application.Core.RSDev.CatalogItem[] oCatalog = rs2005.ListChildren("/", false);
                LoadReports("/", oTreeview, null);
                oTreeview.ExpandDepth = 0;
                oTreeview.Attributes.Add("oncontextmenu", "return false;");
                btnClose.Attributes.Add("onclick", "return window.top.HidePanel();");
                string strControl = "";
                if (Request.QueryString["control"] != null)
                    strControl = Request.QueryString["control"];
                btnSave.Attributes.Add("onclick", "return Update('" + hdnId.ClientID + "','" + strControl + "');");
            }
            else
            {
                Response.Write("Invalid Reporting URL ~ " + Environment.MachineName + "_REPORTING");
                btnSave.Enabled = false;
            }
        }
        private void LoadReports(string strPath, TreeView oTree, TreeNode oParent)
        {
            NCC.ClearView.Application.Core.RSDev.CatalogItem[] oCatalog = rs2005.ListChildren(strPath, false);
            for (int ii = 0; ii < oCatalog.Length; ii++)
            {
                TreeNode oNode = new TreeNode();
                string strName = oCatalog[ii].Name.ToString();
                oNode.Text = strName;
                oNode.ToolTip = strName;
                if (oCatalog[ii].Type == NCC.ClearView.Application.Core.RSDev.ItemTypeEnum.Folder)
                    oNode.SelectAction = TreeNodeSelectAction.Expand;
                else
                    oNode.NavigateUrl = "javascript:Select('" + oCatalog[ii].Path + "','" + hdnId.ClientID + "','" + txtName.ClientID + "');";
                if (oParent == null)
                    oTreeview.Nodes.Add(oNode);
                else
                    oParent.ChildNodes.Add(oNode);
                if (oCatalog[ii].Type == NCC.ClearView.Application.Core.RSDev.ItemTypeEnum.Folder)
                {
                    if (strPath == "/")
                        LoadReports("/" + strName, null, oNode);
                    else
                        LoadReports(strPath + "/" + strName, null, oNode);
                }
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
