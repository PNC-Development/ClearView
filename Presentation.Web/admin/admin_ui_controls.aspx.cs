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
using NCC.ClearView.Application.UI.Entities;
using NCC.ClearView.Application.UI.BusinessLogic;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class admin_ui_controls : BasePage
    {
        protected int intProfile;
        protected UIFactory oUIFactory = new UIFactory();
        protected List<UIWebPage> oUIWebPages;
        protected List<UIWebUserControl> oUIWebUserControl;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/admin_ui_controls.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");

            if (!IsPostBack)
            {
                ddlSourceType.Items.Insert(0, new ListItem("--Select--", "-1"));
                ddlSourceType.Items.Insert(1, new ListItem("Web Page", "1"));
                ddlSourceType.Items.Insert(2, new ListItem("Web User Control", "2"));

                
                ddlDataType.DataSource = oUIFactory.GetUIControlDataTypes(); ;
                ddlDataType.DataValueField = "DataTypeId";
                ddlDataType.DataTextField = "Name";
                ddlDataType.DataBind();
                ddlDataType.Items.Insert(0, new ListItem("-- SELECT --", "-1"));

                ddlValRegularExp.DataSource = oUIFactory.GetUIControlValidationRegularExps(); ;
                ddlValRegularExp.DataValueField = "RegularExpId";
                ddlValRegularExp.DataTextField = "Name";
                ddlValRegularExp.DataBind();
                ddlValRegularExp.Items.Insert(0, new ListItem("-- SELECT --", "-1"));

               
               // btnUpdate.Attributes.Add("onclick", "return confirm('Are you sure you want to update this control?');");

                btnUpdate.Attributes.Add("onclick", "return ValidateNumber('" + txtValMinLen.ClientID + "','Please enter a valid number for the minimum length')" +
                " && ValidateNumber('" + txtValMaxLen.ClientID + "','Please enter a valid number for the maximum length')" +
                " && validateUserInputs()" +
                " && confirm('Are you sure you want to update this control?')" +
                ";");
            }
           
        }

        protected void dlUIControls_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                UIControl oUIControl= new UIControl();
                oUIControl = (UIControl)e.Item.DataItem;

                LinkButton lnkDLbtnSelectControl = (LinkButton)e.Item.FindControl("lnkDLbtnSelectControl");
                lnkDLbtnSelectControl.Text = "Select";
                lnkDLbtnSelectControl.CommandName = "SELECT";
                lnkDLbtnSelectControl.CommandArgument = oUIControl.ControlId.ToString();

                Label lblDlLabelName = (Label)e.Item.FindControl("lblDlLabelName");
                lblDlLabelName.Text = oUIControl.LabelName;

                Label lblDlLabelText = (Label)e.Item.FindControl("lblDlLabelText");
                lblDlLabelText.Text = oUIControl.LabelText;

                Label lblDlShortName = (Label)e.Item.FindControl("lblDlShortName");
                lblDlShortName.Text = oUIControl.ShortName;

                Label lblDlControlName = (Label)e.Item.FindControl("lblDlControlName");
                lblDlControlName.Text = oUIControl.ControlName;

                //Label lblDlToolTipText = (Label)e.Item.FindControl("lblDlToolTipText");
                //lblDlToolTipText.Text = oUIControl.ToolTipText;

                //Label lblDlHTMLHelp = (Label)e.Item.FindControl("lblDlHTMLHelp");
                //lblDlHTMLHelp.Text = oUIControl.HTMLHelp;

                //Label lblDlDataType = (Label)e.Item.FindControl("lblDlDataType");
                //if (oUIControl.ControlDataType!=null)
                //    lblDlDataType.Text = oUIControl.ControlDataType.Name;

                //Label lblDlValRequiredField = (Label)e.Item.FindControl("lblDlValRequiredField");
                //lblDlValRequiredField.Text = (oUIControl.ValidationRequired==0?"No":"Yes");

                //Label lblDlValRegularExp = (Label)e.Item.FindControl("lblDlValRegularExp");
                //if (oUIControl.ValidationRegularExp != null)
                //    lblDlValRegularExp.Text = oUIControl.ValidationRegularExp.Name;

                //Label lblDlValCompareControl = (Label)e.Item.FindControl("lblDlValCompareControl");
                //lblDlValCompareControl.Text = oUIControl.ValidationCompareControl;

                //Label lblDlValMinLen = (Label)e.Item.FindControl("lblDlValMinLen");
                //lblDlValMinLen.Text = oUIControl.ValidationMinLen.ToString();

                //Label lblDlValMaxLen = (Label)e.Item.FindControl("lblDlValMaxLen");
                //lblDlValMaxLen.Text = oUIControl.ValidationMaxLen.ToString();

                //Label lblDlValMinValue = (Label)e.Item.FindControl("lblDlValMinValue");
                //lblDlValMinValue.Text = oUIControl.ValidationRangeFrom;

                //Label lblDlValMaxValue = (Label)e.Item.FindControl("lblDlValMaxValue");
                //lblDlValMaxValue.Text = oUIControl.ValidationRangeTo;

                //Label lblDlValClientSideFunName = (Label)e.Item.FindControl("lblDlValClientSideFunName");
                //lblDlValClientSideFunName.Text = oUIControl.ValidationCustomClientSideFunction;

                //Label lblDlValServerSideFunName = (Label)e.Item.FindControl("lblDlValServerSideFunName");
                //lblDlValServerSideFunName.Text = oUIControl.ValidationCustomServerSideFunction;

                //Label lblDlDisplayOrder = (Label)e.Item.FindControl("lblDlDisplayOrder");
                //lblDlDisplayOrder.Text = oUIControl.DisplayOrder.ToString();
            }
        }


        protected void dlUIControls_Command(object sender, DataListCommandEventArgs e)
        {
            // Set the SelectedIndex property to select an item in the DataList.
            if (e.CommandName.ToUpper() == "SELECT")
            {
                dlUIControls.SelectedIndex = e.Item.ItemIndex;
                LoadUIControlList();
                LoadUIControlDetails(long.Parse(e.CommandArgument.ToString()));
            }

        }
        protected void ddlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDataType.SelectedValue == "1")
            {
                txtValRangeFrom.Text = "";
                txtValRangeFrom.Enabled = false;
                txtValRangeTo.Text = "";
                txtValRangeTo.Enabled = false;
            }
            else 
            {
                txtValRangeFrom.Text = "";
                txtValRangeFrom.Enabled = true;
                txtValRangeTo.Text = "";
                txtValRangeTo.Enabled = true;
            }
        }

        protected void ddlValRegularExp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlSourceFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();
            LoadUIControlList();
        }

        protected void ddlSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();
            //Load Web Pages OR User Controls
            if (ddlSourceType.SelectedValue.ToString() == "1")
                LoadUIWebPages();
            else if (ddlSourceType.SelectedValue.ToString() == "2")
                LoadUIWebUserControls();
        }

        protected void dlUIControls_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadUIWebPages()
        {
            oUIWebPages = oUIFactory.GetUIWebPages();

            ddlSourceFile.DataSource = oUIWebPages;
            ddlSourceFile.DataValueField = "WebPageId";
            ddlSourceFile.DataTextField = "Path";
            ddlSourceFile.DataBind();
            ddlSourceFile.Items.Insert(0, new ListItem("-- SELECT --", "-1"));
        }

        private void LoadUIWebUserControls()
        {
            oUIWebUserControl = oUIFactory.GetUIWebUserControls();

            ddlSourceFile.DataSource = oUIWebUserControl;
            ddlSourceFile.DataValueField = "WebUserControlId";
            ddlSourceFile.DataTextField = "Path";
            ddlSourceFile.DataBind();
            ddlSourceFile.Items.Insert(0, new ListItem("-- SELECT --", "-1"));
        }

        private void LoadUIControlDetails(long lngControlId)
        {
            try
            {
                UIControl oUIControl = new UIControl();
                oUIControl = oUIFactory.GetUIControl(lngControlId);
                txtControlId.Text = oUIControl.ControlId.ToString();
                txtLabelName.Text = oUIControl.LabelName;
                txtLabelText.Text = oUIControl.LabelText;
                txtShortName.Text = oUIControl.ShortName;
                txtControlName.Text = oUIControl.ControlName;
                txtToolTipText.Text = oUIControl.ToolTipText;
                txtHTMLHelp.Text = oUIControl.HTMLHelp;
                if (oUIControl.ControlDataType != null)
                    if (oUIControl.ControlDataType.DataTypeId > 0)
                        ddlDataType.SelectedValue = oUIControl.ControlDataType.DataTypeId.ToString();
                    else
                        ddlDataType.SelectedValue = "-1";
                else
                    ddlDataType.SelectedValue = "-1";

                chkValRequiredField.Checked = (oUIControl.ValidationRequired == 1 ? true : false);

                if (oUIControl.ValidationRegularExp != null)
                    if (oUIControl.ControlDataType.DataTypeId > 0)
                        ddlValRegularExp.SelectedValue = oUIControl.ValidationRegularExp.RegularExpId.ToString();
                    else
                        ddlValRegularExp.SelectedValue = "-1";
                else
                    ddlValRegularExp.SelectedValue = "-1";

                txtValCompareControl.Text = oUIControl.ValidationCompareControl;
                txtValMinLen.Text = oUIControl.ValidationMinLen.ToString();
                txtValMaxLen.Text = oUIControl.ValidationMaxLen.ToString();
                txtValRangeFrom.Text = oUIControl.ValidationRangeFrom;
                txtValRangeTo.Text = oUIControl.ValidationRangeTo;
                txtValClientSideFunName.Text = oUIControl.ValidationCustomClientSideFunction;
                txtValServerSideFunName.Text = oUIControl.ValidationCustomServerSideFunction;
                txtValMsg.Text = oUIControl.ValidationMsg;
                txtDisplayOrder.Text = oUIControl.DisplayOrder.ToString();

                pnlControlDetails.Visible = true;
            }
            catch 
            {
                pnlControlDetails.Visible = false;
            }
        }

        private void LoadUIControlList()
        {
            try
            {
                List<UIControl> oUIControlList = new List<UIControl>();
                if (ddlSourceType.SelectedValue.ToString() == "1") //Web Page
                {
                    UIWebPage oUIWebPage = oUIFactory.GetUIWebPage(ddlSourceFile.SelectedItem.Text.ToString());
                    oUIControlList = oUIWebPage.ControlList;
                }
                else if (ddlSourceType.SelectedValue.ToString() == "2") //User Control
                {
                    UIWebUserControl oUIWebUserControl = oUIFactory.GetUIWebUserControl(ddlSourceFile.SelectedItem.Text.ToString());
                    oUIControlList = oUIWebUserControl.ControlList;
                }
                dlUIControls.DataSource = oUIControlList;
                dlUIControls.DataBind();
                if (oUIControlList == null || oUIControlList.Count == 0)
                {
                    pnlNoResults.Visible = true;
                    pnlResults.Visible = false;
                }
                else
                {
                    pnlNoResults.Visible = false;
                    pnlResults.Visible = true;
                }
            }
            catch 
            { 
                
            }
                
        }

        private void ResetControls()
        {
            txtControlId.Text="";
            txtLabelName.Text="";
            txtLabelName.Enabled = false;
            txtLabelText.Text = "";
            txtShortName.Text = "";
            txtControlName.Text="";
            txtToolTipText.Text="";
            txtHTMLHelp.Text="";
            ddlDataType.SelectedValue = "-1";
            chkValRequiredField.Checked = false;
            ddlValRegularExp.SelectedValue = "-1";
            txtValCompareControl.Text="";
            txtValMinLen.Text = "0";
            txtValMaxLen.Text = "0";
            txtValRangeFrom.Text="";
            txtValRangeTo.Text="";
            txtValClientSideFunName.Text="";
            txtValServerSideFunName.Text="";
            txtValMsg.Text="";
            txtDisplayOrder.Text="";

            dlUIControls.SelectedIndex = -1;
            dlUIControls.DataSource = null;

            pnlNoResults.Visible = false;
            pnlResults.Visible = false;
            pnlControlDetails.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UIControl oUIControl = new UIControl();
            oUIControl.ControlId = long.Parse(txtControlId.Text.Trim());
            oUIControl.LabelName = txtLabelName.Text.Trim();
            oUIControl.LabelText = txtLabelText.Text.Trim();
            oUIControl.ShortName = txtShortName.Text.Trim();
            oUIControl.ControlName = txtControlName.Text.Trim();
            oUIControl.ToolTipText = txtToolTipText.Text.Trim();
            oUIControl.HTMLHelp = txtHTMLHelp.Text.Trim();

            if (ddlDataType.SelectedValue != "-1")
            {
                UIControlDataType oUIControlDataType = new UIControlDataType();
                oUIControlDataType.DataTypeId = Int64.Parse(ddlDataType.SelectedValue.ToString());
                oUIControl.ControlDataType = oUIControlDataType;
            }
            
            oUIControl.ValidationRequired =(chkValRequiredField.Checked==true?1:0);

            if (ddlValRegularExp.SelectedValue != "-1")
            {
                UIControlValidationRegularExp oUIControlValidationRegularExp = new UIControlValidationRegularExp();
                oUIControlValidationRegularExp.RegularExpId = Int32.Parse(ddlValRegularExp.SelectedValue.ToString());
                oUIControl.ValidationRegularExp = oUIControlValidationRegularExp;
            }

            oUIControl.ValidationCompareControl = txtValCompareControl.Text.Trim();

            if ( txtValMinLen.Text.Trim()!="")
                oUIControl.ValidationMinLen = long.Parse(txtValMinLen.Text.Trim());
            if (txtValMaxLen.Text.Trim() != "")
                oUIControl.ValidationMaxLen = long.Parse(txtValMaxLen.Text.Trim());

            oUIControl.ValidationRangeFrom = txtValRangeFrom.Text.Trim();
            oUIControl.ValidationRangeTo = txtValRangeTo.Text.Trim();
            oUIControl.ValidationCustomClientSideFunction = txtValClientSideFunName.Text.Trim();
            oUIControl.ValidationCustomServerSideFunction = txtValServerSideFunName.Text.Trim();
            oUIControl.ValidationMsg = txtValMsg.Text.Trim();
            oUIControl.ModifiedBy = intProfile;
            if (txtDisplayOrder.Text.Trim()!="")
                 oUIControl.DisplayOrder = long.Parse(txtDisplayOrder.Text);

            oUIControl.Deleted = 0;
            //oUIControl.ControlId = txtControlId.Text;

            oUIFactory.UpdateUIControl(oUIControl);

            LoadUIControlList();
           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            dlUIControls.SelectedIndex = -1;
            pnlControlDetails.Visible = false;
        }
    }
}
