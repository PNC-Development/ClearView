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
    public partial class file_permissions : BasePage
    {
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Documents oDocument;
        protected int intDocument = 0;
        protected Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDocument = new Documents(intProfile, dsn);
            oVariables = new Variables(intEnvironment);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intDocument = Int32.Parse(Request.QueryString["id"]);
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.opener.location.reload();alert('Document Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["add"] != null && Request.QueryString["add"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add", "<script type=\"text/javascript\">window.opener.location.reload();alert('Permission Added Successfully');<" + "/" + "script>");
                if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">window.opener.location.reload();alert('Permission Deleted Successfully');<" + "/" + "script>");
                if (!IsPostBack)
                {
                    DataSet ds = oDocument.Get(intDocument);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (oDocument.CanEdit(intDocument, intProfile) == true)
                        {
                            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            txtName.Visible = true;
                            txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                            ddlSecurity.SelectedValue = ds.Tables[0].Rows[0]["security"].ToString();
                            if (ddlSecurity.SelectedItem.Value == "0")
                            {
                                panShared.Visible = true;
                                btnAdd.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter the LAN ID of the user');");
                                rptPermissions.DataSource = oDocument.GetPermissions(intDocument);
                                rptPermissions.DataBind();
                                lblNone.Visible = (rptPermissions.Items.Count == 0);
                                foreach (RepeaterItem ri in rptPermissions.Items)
                                    ((LinkButton)ri.FindControl("btnDeleteUser")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this permission?');");
                            }
                            panEdit.Visible = true;
                        }
                        else
                        {
                            lblName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            lblName.Visible = true;
                            panDenied.Visible = true;
                        }
                    }
                }
            }
            Variables oVariable = new Variables(intEnvironment);
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this document?');");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            oDocument.Update(intDocument, txtName.Text, txtDescription.Text, Int32.Parse(ddlSecurity.SelectedItem.Value));
            Response.Redirect(Request.Path + "?id=" + intDocument.ToString() + "&save=true");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oDocument.AddPermission(intDocument, Int32.Parse(Request.Form[hdnUser.UniqueID]), Int32.Parse(radSecurity.SelectedItem.Value));
            Response.Redirect(Request.Path + "?id=" + intDocument.ToString() + "&add=true");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            bool boolDelete = oDocument.Delete(intDocument, true, oVariables.DocumentsFolder());
            if (boolDelete == true)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('There was a problem deleting the file');<" + "/" + "script>");
        }
        protected void btnDeleteUser_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oDocument.DeletePermission(intDocument, Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intDocument.ToString() + "&delete=true");
        }
    }
}