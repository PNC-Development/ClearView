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
    public partial class service_editor_conditions : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected ServiceEditor oServiceEditor;
        protected Services oService;
        protected Users oUser;
        private int intID = 0;
        private int intService = 0;
        private int intNextService = 0;
        protected string strIndent = "15";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oService = new Services(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            Int32.TryParse(Request.QueryString["id"], out intID);
            Int32.TryParse(Request.QueryString["serviceid"], out intService);
            Int32.TryParse(Request.QueryString["nextservice"], out intNextService);

            if (Request.QueryString["saved"] != null)
                trSaved.Visible = true;
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["id"]) == false)
                {
                    panCondition.Visible = true;
                    DataSet dsConfigs = oServiceEditor.GetConfigs(intService, -1, 1);
                    //DataSet dsConfigs = oServiceEditor.GetConfigs(intService, 0, 1);
                    foreach (DataRow dr in dsConfigs.Tables[0].Rows)
                    {
                        int _configid = Int32.Parse(dr["id"].ToString());
                        TreeNode oNode = new TreeNode();
                        oNode.Text = dr["question"].ToString();
                        oNode.ToolTip = dr["question"].ToString();
                        oNode.Value = dr["id"].ToString();
                        oNode.SelectAction = TreeNodeSelectAction.None;
                        oNode.Checked = false;
                        treConditional.Nodes.Add(oNode);

                        DataSet dsValues = oServiceEditor.GetConfigValues(_configid);
                        //dr["code"].ToString() == "DROPDOWN" || dr["code"].ToString() == "RADIOLIST"
                        if (dsValues.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drValue in dsValues.Tables[0].Rows)
                            {
                                TreeNode oChild = new TreeNode();
                                oChild.Text = drValue["value"].ToString();
                                oChild.ToolTip = drValue["value"].ToString();
                                oChild.Value = drValue["id"].ToString();
                                oChild.SelectAction = TreeNodeSelectAction.None;
                                oChild.Checked = oServiceEditor.IsWorkflowConditionValue(intID, Int32.Parse(drValue["id"].ToString()));
                                oNode.ChildNodes.Add(oChild);
                            }
                        }
                        else
                        {
                            TreeNode oChild = new TreeNode();
                            oChild.Text = "<b>Unavailable.</b> Only &quot;valued&quot; controls can be used for conditional workflow";
                            oChild.Value = "0";
                            oChild.SelectAction = TreeNodeSelectAction.None;
                            oChild.ShowCheckBox = false;
                            oNode.ChildNodes.Add(oChild);
                        }
                    }

                    if (intID > 0)
                    {
                        DataSet ds = oServiceEditor.GetWorkflowCondition(intID);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        }
                    }
                    btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Enter a name for this condition')" +
                        ";");
                }
                else if (intService > 0 && intNextService > 0)
                {
                    DataSet ds = oService.GetWorkflow(intService, intNextService);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        panList.Visible = true;
                        radConditionOnly.Checked = (ds.Tables[0].Rows[0]["only"].ToString() == "1");
                        radConditionUnless.Checked = (ds.Tables[0].Rows[0]["only"].ToString() == "0");
                        radContinueYes.Checked = (ds.Tables[0].Rows[0]["continue"].ToString() == "1");
                        radContinueNo.Checked = (ds.Tables[0].Rows[0]["continue"].ToString() == "0");
                        btnSave.Attributes.Add("onclick", "return ValidateRadioButtons3('" + radConditionOnly.ClientID + "','" + radConditionUnless.ClientID + "','" + radConditionNeither.ClientID + "','Please select your condition for initiating this workflow')" +
                            " && ValidateRadioButtons('" + radContinueYes.ClientID + "','" + radContinueNo.ClientID + "','Please select how you want to handle subsequent workflows if this workflow is not initiated')" +
                            ";");

                        rptConditions.DataSource = oServiceEditor.GetWorkflowConditions(intService, intNextService, 0);
                        rptConditions.DataBind();
                        lblNone.Visible = (rptConditions.Items.Count == 0);
                        foreach (RepeaterItem ri in rptConditions.Items)
                            ((LinkButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this condition set?');");
                    }
                }
                if (radConditionOnly.Checked == false && radConditionUnless.Checked == false)
                    radConditionNeither.Checked = true;
                if (radContinueNo.Checked == false && radContinueYes.Checked == false)
                    radContinueNo.Checked = true;
            }
        }
        protected void btnEdit_Click(Object Sender, EventArgs e)
        {
            LinkButton _button = (LinkButton)Sender;
            Response.Redirect(Request.Path + "?serviceid=" + intService.ToString() + "&nextservice=" + intNextService.ToString() + "&id=" + _button.CommandArgument);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton _button = (LinkButton)Sender;
            oServiceEditor.DeleteWorkflowCondition(Int32.Parse(_button.CommandArgument));
            Response.Redirect(Request.Path + "?serviceid=" + intService.ToString() + "&nextservice=" + intNextService.ToString());
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?serviceid=" + intService.ToString() + "&nextservice=" + intNextService.ToString() + "&id=0");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oService.UpdateWorkflow(intService, intNextService, (radConditionNeither.Checked ? -1 : (radConditionOnly.Checked ? 1 : 0)), (radContinueYes.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?serviceid=" + intService.ToString() + "&nextservice=" + intNextService.ToString() + "&saved=true");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intID == 0)
                intID = oServiceEditor.AddWorkflowCondition(txtName.Text, intService, intNextService, (chkEnabled.Checked ? 1 : 0));
            else
                oServiceEditor.UpdateWorkflowCondition(intID, txtName.Text, (chkEnabled.Checked ? 1 : 0));

            oServiceEditor.DeleteWorkflowConditionValue(intID);
            foreach (TreeNode oNode in treConditional.Nodes)
            {
                foreach (TreeNode oChild in oNode.ChildNodes)
                {
                    if (oChild.Checked == true)
                        oServiceEditor.AddWorkflowConditionValue(intID, Int32.Parse(oChild.Value));
                }
            }
            Response.Redirect(Request.Path + "?serviceid=" + intService.ToString() + "&nextservice=" + intNextService.ToString() + "&saved=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?serviceid=" + intService.ToString() + "&nextservice=" + intNextService.ToString());
        }
    }
}
