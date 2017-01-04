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

namespace NCC.ClearView.Presentation.Web
{
    public partial class virtual_guests : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected VMWare oVMWare;
        private DataSet ds;
        protected int intId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intId = Int32.Parse(Request.QueryString["id"]);


            btnMoveGuest.Attributes.Add("onclick", "return ValidateDropDown('" + ddlHost.ClientID + "','Please select a host') && CheckSelectedGuests('');");
            btnDecommissionGuests.Attributes.Add("onclick", "return CheckSelectedGuests('NOTE: This will turn off and decommission the guest immediately');");
            btnDeleteWOdecommissionGuest.Attributes.Add("onclick", "return CheckSelectedGuests('NOTE: If the guests exist on a MS Virtual Workstation Host, they will not be removed');");
            btnExpandCollapseTree.Attributes.Add("onclick", "return expandcollapseTreeNodes('" + tvVirtualHostnGuests.ClientID + "');");

            if (!Page.IsPostBack)
            {

            }
            oVMWare = new VMWare(intProfile, dsn);
            ds = oVMWare.GetVirtualHostAndGuests();

            //Load virtual host and guests
            LoadControls();


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=X");
        }

        private void LoadControls()
        {
            ds = oVMWare.GetVirtualHostAndGuests();
            //Load Tree view - Get the Virtual Hosts and Guests
            tvVirtualHostnGuests.Nodes.Clear();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["VirtualHostName"].ToString();
                oNode.Text = "<b>" + oNode.Text + "</b>";
                oNode.Value = dr["virtualhostid"].ToString();

                oNode.ToolTip = dr["VirtualHostName"].ToString();

                oNode.SelectAction = TreeNodeSelectAction.None;

                //Get the child nodes
                foreach (DataRow drGuests in ds.Tables[1].Rows)
                {
                    if (dr["virtualhostid"].ToString() == drGuests["virtualhostid"].ToString())
                    {
                        string strGuestDetails;
                        strGuestDetails = "";
                        if (drGuests["GuestName"] == DBNull.Value)
                            strGuestDetails = strGuestDetails + "";
                        else
                            strGuestDetails = strGuestDetails + drGuests["GuestName"].ToString();

                        if (drGuests["StepStatus"] == DBNull.Value)
                            strGuestDetails = strGuestDetails + "- Status: , ";
                        else
                            strGuestDetails = strGuestDetails + "- Status: " + drGuests["StepStatus"].ToString() + " , "; ;

                        if (drGuests["RAM"] == DBNull.Value)
                            strGuestDetails = strGuestDetails + " RAM , ";
                        else
                            strGuestDetails = strGuestDetails + drGuests["RAM"].ToString() + " RAM , ";

                        if (drGuests["RAM"] == DBNull.Value)
                            strGuestDetails = strGuestDetails + " HDD ";
                        else
                            strGuestDetails = strGuestDetails + drGuests["HDD"].ToString() + " HDD ";

                        TreeNode oChildNode = new TreeNode();
                        oChildNode.Text = strGuestDetails;
                        if (drGuests["step"].ToString() == "1" || drGuests["step"].ToString() == "2")
                            oChildNode.Text = "<font color = red>" + oChildNode.Text + "</font>";

                        oChildNode.Value = drGuests["Id"].ToString();
                        oChildNode.ToolTip = strGuestDetails;
                        oChildNode.SelectAction = TreeNodeSelectAction.None;
                        oNode.ChildNodes.Add(oChildNode);
                        oNode.Expand();
                    }
                }

                tvVirtualHostnGuests.Nodes.Add(oNode);
                tvVirtualHostnGuests.CollapseAll();

                //Load Drop down - Get the Virtual Hosts
                ddlHost.Items.Clear();
                ddlHost.DataTextField = "VirtualHostName";
                ddlHost.DataValueField = "virtualhostid";
                ddlHost.DataSource = ds.Tables[0];
                ddlHost.DataBind();
                ddlHost.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }


        }

        //decommission Guest 
        protected void btnDecommissionGuests_Click(object sender, EventArgs e)
        {
            Asset oAsset = new Asset(intProfile, dsnAsset);
            Workstations oRemote = new Workstations(intProfile, dsnRemote);
            Workstations oWorkstation = new Workstations(intProfile, dsn);
            Domains oDomain = new Domains(intProfile, dsn);

            foreach (TreeNode node in tvVirtualHostnGuests.CheckedNodes)
            {

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (dr["id"].ToString() == node.Value.ToString())
                    {
                        int intID = Int32.Parse(dr["id"].ToString());
                        int intName = Int32.Parse(dr["nameid"].ToString());
                        string strName = oWorkstation.GetName(intName);
                        int intHost = Int32.Parse(dr["virtualhostid"].ToString());
                        int intAsset = Int32.Parse(dr["assetid"].ToString());
                        int intOS = Int32.Parse(dr["osid"].ToString());
                        string strHost = oAsset.GetServerOrBlade(intHost, "name");
                        string strVirtualDir = "";
                        DataSet dsOS = oAsset.GetVirtualHostOs(intHost);
                        foreach (DataRow drOS in dsOS.Tables[0].Rows)
                        {
                            if (Int32.Parse(drOS["osid"].ToString()) == intOS)
                            {
                                strVirtualDir = drOS["virtualdir"].ToString();
                                break;
                            }
                        }
                        int intDomain = Int32.Parse(dr["domainid"].ToString());
                        int intEnv = Int32.Parse(oDomain.Get(intDomain, "environment"));
                        oRemote.AddRemoteVirtualDecom(intEnv, strHost, strVirtualDir, strName);
                        // Clean up database
                        oWorkstation.DeleteVirtual(intID);
                        oWorkstation.UpdateName(intName, 1);
                        oAsset.DeleteGuest(intAsset);
                        oAsset.AddStatus(intAsset, "", (int)AssetStatus.Decommissioned, intProfile, DateTime.Now);

                    }
                }

            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intId + "&div=X");
            //LoadControls(); 

        }

        //Delete Guest without decommission
        protected void btnDeleteWOdecommissionGuest_Click(object sender, EventArgs e)
        {
            Workstations oWorkstation = new Workstations(intProfile, dsn);
            foreach (TreeNode node in tvVirtualHostnGuests.CheckedNodes)
            {
                int intID = Int32.Parse(node.Value.ToString());
                oWorkstation.DeleteVirtual(intID);
            }

            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intId + "&div=X");
            //LoadControls(); 
        }

        //Move guest to selected host
        protected void btnMoveGuest_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in tvVirtualHostnGuests.CheckedNodes)
            {
                int intGuest = Int32.Parse(node.Value.ToString());
                int intHost = Int32.Parse(ddlHost.SelectedItem.Value.ToString());
                oVMWare.moveGuest(intHost, intGuest);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intId + "&div=X");
            //LoadControls(); 
        }
    }
}