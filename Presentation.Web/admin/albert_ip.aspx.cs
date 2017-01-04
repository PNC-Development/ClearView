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
using System.Data.OleDb;

namespace NCC.ClearView.Presentation.Web
{
    public partial class albert_ip : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected IPAddresses oIPAddresses;
        protected Environments oEnvironment;
        protected Classes oClass;
        protected Servers oServer;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intCount = 0;
        protected int intProfile = 0;
        private Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/albert_ip.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oEnvironment = new Environments(0, dsn);
            oClass = new Classes(0, dsn);
            oServer = new Servers(0, dsn);
            if (!IsPostBack)
            {
                if (Request.QueryString["done"] != null)
                {
                    panDone.Visible = true;
                    lblImport.Text = Request.QueryString["done"];
                }
                LoadClasses();
                btnImport.Attributes.Add("onclick", "return ValidateHidden0('" + hdnVLAN.ClientID + "','" + txtVLAN.ClientID + "','Please select a Network') && ValidateText('" + oFile.ClientID + "','Please select a file');");
            }
        }
        private void LoadClasses()
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadEnvironments(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTreeview.ExpandDepth = 2;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadEnvironments(int _parent, TreeNode oParent)
        {
            DataSet ds = oClass.GetEnvironment(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadAddress(_parent, Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadAddress(int _classid, int _environmentid, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetVlansAddress(_classid, _environmentid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadVlans(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadVlans(int _classid, int _environmentid, int _addressid, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetVlansAddress(_classid, _environmentid, _addressid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["vlan"].ToString();
                oNode.ToolTip = dr["vlan"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadNetworks(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadNetworks(int _parent, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetNetworks(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.ToolTip = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Select;
                oNode.NavigateUrl = "javascript:SelectNetwork('" + dr["id"].ToString() + "','" + oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString())) + "','" + hdnVLAN.ClientID + "','" + txtVLAN.ClientID + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnImport_Click(Object Sender, EventArgs e)
        {
            if (oFile.PostedFile != null && oFile.FileName != "")
            {
                oVariables = new Variables(intEnvironment);
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + oFile.FileName;
                oFile.PostedFile.SaveAs(oVariables.UploadsFolder() + strFile);
                int intNetwork = Int32.Parse(Request.Form[hdnVLAN.UniqueID]);
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + oVariables.UploadsFolder() + strFile + ";Extended Properties=Excel 8.0;";
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "ExcelInfo");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0].ToString().Trim() == "")
                        break;
                    string strIP = dr[0].ToString().Trim();
                    string strServer = dr[1].ToString().Trim();

                    int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP4 = Int32.Parse(strIP);
                    int intIP = oIPAddresses.Add(intNetwork, intIP1, intIP2, intIP3, intIP4, -100);
                    int intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strServer, 0, 0, 0, 0, "", "", "", "", 0, 0);
                    oIPAddresses.AddDetail(intIP, intDetail);
                    intCount++;
                }
                Response.Redirect(Request.Path + "?done=" + intCount.ToString());
            }
        }
    }
}
