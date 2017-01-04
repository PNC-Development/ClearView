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
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Presentation.Web.Custom;

namespace NCC.ClearView.Presentation.Web
{
    public partial class document_repository_share : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Documents oDocument;
        protected Customized oCustomized;
        protected Pages oPage;
        protected Applications oApplication;
        protected Users oUser;
        protected Variables oVariable;
        protected NCC.ClearView.Application.Core.Roles oRole;
        protected int intDocument = 0;
        protected int intApplication;
        protected int intId;

        protected int intFolderCount = 0;
        protected string strPath = "";
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDocument = new Documents(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oRole = new NCC.ClearView.Application.Core.Roles(intProfile, dsn);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intDocument = Int32.Parse(Request.QueryString["id"]);
                intId = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    Tab oTab = new Tab("", 1, "divMenu1", true, false);
                    oTab.AddTab("Add Permission", "");
                    oTab.AddTab("Allowed Applications", "");
                    oTab.AddTab("Allowed Users", "");
                    strMenuTab1 = oTab.GetTabs();

                    drpApplications.DataValueField = "applicationid";
                    drpApplications.DataTextField = "name";
                    drpApplications.DataSource = oApplication.Gets(1);
                    drpApplications.DataBind();
                    drpApplications.Items.Insert(0, new ListItem("-- SELECT --"));

                    DataSet ds = oCustomized.GetDocumentRepositoryId(intId);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblType.Text = ds.Tables[0].Rows[0]["type"].ToString().ToLower() != "folder" ? "File:" : ds.Tables[0].Rows[0]["type"].ToString() + ":";
                        lblName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        ddlSecurity.SelectedValue = ds.Tables[0].Rows[0]["security"].ToString();
                        strPath = ds.Tables[0].Rows[0]["path"].ToString();
                        intFolderCount = oDocument.GetDocumentCount(strPath);

                        if (ddlSecurity.SelectedItem.Value == "0")
                        {
                            panShared.Visible = true;
                            DataSet dsApplications = oCustomized.GetDocumentRepositorySharesById(intId, "Application");
                            if (dsApplications.Tables[0].Rows.Count > 0)
                            {
                                radSecurity.SelectedValue = dsApplications.Tables[0].Rows[0]["security"].ToString();
                                if (dsApplications.Tables[0].Rows[0]["ownerid"].ToString() != intProfile.ToString())
                                {
                                    ddlSecurity.Enabled = false;
                                    drpApplications.Enabled = false;
                                    btnSubmit.Enabled = false;
                                    btnAdd.Enabled = false;
                                    rblShareType.Enabled = false;
                                    radSecurity.Enabled = false;

                                }
                                rptPermissionsApplications.DataSource = dsApplications;
                                rptPermissionsApplications.DataBind();
                                lblPermissionsApplications.Visible = (rptPermissionsApplications.Items.Count == 0);

                                foreach (RepeaterItem ri in rptPermissionsApplications.Items)
                                {
                                    LinkButton lbDelete = (LinkButton)ri.FindControl("btnDeleteUser");
                                    lbDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this share?');");
                                    lbDelete.Visible = dsApplications.Tables[0].Rows[0]["ownerid"].ToString() == intProfile.ToString();
                                }
                            }
                            DataSet dsUsers = oCustomized.GetDocumentRepositorySharesById(intId, "User");
                            if (dsUsers.Tables[0].Rows.Count > 0)
                            {
                                radSecurity.SelectedValue = dsUsers.Tables[0].Rows[0]["security"].ToString();
                                if (dsUsers.Tables[0].Rows[0]["ownerid"].ToString() != intProfile.ToString())
                                {
                                    ddlSecurity.Enabled = false;
                                    drpApplications.Enabled = false;
                                    btnSubmit.Enabled = false;
                                    btnAdd.Enabled = false;
                                    rblShareType.Enabled = false;
                                    radSecurity.Enabled = false;

                                }
                                rptPermissionsUsers.DataSource = dsUsers;
                                rptPermissionsUsers.DataBind();
                                lblPermissionsUsers.Visible = (rptPermissionsUsers.Items.Count == 0);

                                foreach (RepeaterItem ri in rptPermissionsUsers.Items)
                                {
                                    LinkButton lbDelete = (LinkButton)ri.FindControl("btnDeleteUser");
                                    lbDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this share?');");
                                    lbDelete.Visible = dsUsers.Tables[0].Rows[0]["ownerid"].ToString() == intProfile.ToString();
                                }
                            }
                            if (Request.Cookies["sharetype"] != null && Request.Cookies["sharetype"].Value != "")
                                rblShareType.SelectedValue = Request.Cookies["sharetype"].Value;
                            if (rblShareType.SelectedValue == "")
                                rblShareType.SelectedIndex = 0;
                            if (rblShareType.SelectedIndex == 0)
                            {
                                divUser2.Style["display"] = "inline";
                                divApp.Style["display"] = "none";
                                hdnValue.Value = "User";
                            }
                            if (rblShareType.SelectedIndex == 1)
                            {
                                divUser2.Style["display"] = "none";
                                divApp.Style["display"] = "inline";
                                hdnValue.Value = "Application";
                            }
                        }
                        panEdit.Visible = true;
                    }
                }
            }

            rblShareType.Attributes.Add("onclick", "DynamicShareType('" + rblShareType.ClientID + "');");
            btnSubmit.Attributes.Add("onclick", "return ValidateShare();");
            btnAdd.Attributes.Add("onclick", "return ValidateAdd();");
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            if (lblType.Text.Replace(":", "") == "Folder")
            {
                strPath = oCustomized.GetDocumentRepository(intId, "path");
                oCustomized.UpdateDocumentRepositorySecurity(intId, Int32.Parse(ddlSecurity.SelectedItem.Value));
                DataSet ds = oDocument.GetDocumentID(strPath);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    oCustomized.UpdateDocumentRepositorySecurity(Int32.Parse(dr["id"].ToString()), Int32.Parse(ddlSecurity.SelectedItem.Value));
            }
            else
                oCustomized.UpdateDocumentRepositorySecurity(intId, Int32.Parse(ddlSecurity.SelectedItem.Value));
            Response.Redirect(Request.Path + "?id=" + intId.ToString() + "&save=true");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intAppId = 0;
            //string strShareType = divUser.Visible ? "User" : "Application";
            string strShareType = hdnValue.Value;
            if (strShareType == "Application") 
                intAppId = Int32.Parse(drpApplications.SelectedValue);
            if (strShareType == "User")
            {
                if (lblType.Text.Replace(":", "") == "Folder")
                {
                    strPath = oCustomized.GetDocumentRepository(intId, "path");
                    oCustomized.AddDocumentRepositoryShare(intId, Int32.Parse(radSecurity.SelectedItem.Value), strShareType, intAppId, Int32.Parse(Request.Form[hdnUser.UniqueID]), intProfile);
                    DataSet ds = oDocument.GetDocumentID(strPath);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        oCustomized.AddDocumentRepositoryShare(Int32.Parse(dr["id"].ToString()), Int32.Parse(radSecurity.SelectedItem.Value), strShareType, intAppId, Int32.Parse(Request.Form[hdnUser.UniqueID]), intProfile);
                }
                else
                    oCustomized.AddDocumentRepositoryShare(intId, Int32.Parse(radSecurity.SelectedItem.Value), strShareType, intAppId, Int32.Parse(Request.Form[hdnUser.UniqueID]), intProfile);
            }
            else
            {
                if (lblType.Text.Replace(":", "") == "Folder")
                {
                    strPath = oCustomized.GetDocumentRepository(intId, "path");
                    oCustomized.AddDocumentRepositoryShare(intId, Int32.Parse(radSecurity.SelectedItem.Value), strShareType, intAppId, 0, intProfile);
                    DataSet ds = oDocument.GetDocumentID(strPath);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        oCustomized.AddDocumentRepositoryShare(Int32.Parse(dr["id"].ToString()), Int32.Parse(radSecurity.SelectedItem.Value), strShareType, intAppId, 0, intProfile);
                }
                else
                    oCustomized.AddDocumentRepositoryShare(intId, Int32.Parse(radSecurity.SelectedItem.Value), strShareType, intAppId, 0, intProfile);
            }
            Response.Cookies["sharetype"].Value = strShareType;
            Response.Redirect(Request.Path + "?id=" + intDocument.ToString() + "&add=true");
        }

        protected void btnDeleteUser_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oCustomized.DeleteDocumentRepositoryShare(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intDocument.ToString() + "&delete=true");
        }

        //protected void Share_Changed(Object Sender, EventArgs e)
        //{        
        //    RadioButtonList rbl = (RadioButtonList)Sender;
        //    panUser.Visible = rbl.SelectedValue == "User";
        //    panApp.Visible = !panUser.Visible;         
        //}
    }
}
