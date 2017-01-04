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
using System.Text;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class service_editor_workflow_print : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;

        protected ServiceEditor oServiceEditor;
        protected Services oService;
        protected Users oUser;
        private int intService = 0;
        List<StringBuilder> lstServices;
        protected StringBuilder strTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oService = new Services(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            Int32.TryParse(Request.QueryString["serviceid"], out intService);
            Page.Title = "Workflow | " + oService.GetName(intService);
            if (!IsPostBack)
            {
                lstServices = new List<StringBuilder>();
                // Get back to the original one.
                int intWorkflowService = intService;
                DataSet dsReceive = oService.GetWorkflowsReceive(intWorkflowService);
                while (dsReceive.Tables[0].Rows.Count > 0)
                {
                    intWorkflowService = Int32.Parse(dsReceive.Tables[0].Rows[0]["serviceid"].ToString());
                    dsReceive = oService.GetWorkflowsReceive(intWorkflowService);
                }
                if (dsReceive.Tables[0].Rows.Count > 0)
                    intWorkflowService = Int32.Parse(dsReceive.Tables[0].Rows[0]["serviceid"].ToString());
                LoadWorkflow(intWorkflowService, intService, 0);

                // Build Table
                strTable = new StringBuilder();
                foreach (StringBuilder lstService in lstServices)
                {
                    if (strTable.ToString() != "")
                        strTable.Append("<td valign=\"top\"><p>&nbsp;</p><p>&nbsp;</p><img src=\"/images/arrow_right.gif\" border=\"0\" /></td>");
                    strTable.Append("<td valign=\"top\">");
                    strTable.Append(lstService.ToString());
                    strTable.Append("<td>");
                }
                strTable.Insert(0, "<tr>");
                strTable.Append("</tr>");

            }
        }
        private void LoadWorkflow(int _serviceid, int _previous_serviceid, int _step)
        {
            DataSet dsWorkflow = oService.GetWorkflows(_serviceid);
            StringBuilder strForm = LoadWorkflowForm(_serviceid, _previous_serviceid, dsWorkflow.Tables[0].Rows.Count, (_step == 0));

            if (lstServices.Count > _step)
                lstServices[_step].Append(strForm);
            else
                lstServices.Add(strForm);

            // Load Workflow services..
            foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
            {
                int intWorkflowService = Int32.Parse(drWorkflow["nextservice"].ToString());
                LoadWorkflow(intWorkflowService, _serviceid, _step + 1);
            }
        }
        private StringBuilder LoadWorkflowForm(int _serviceid, int _previous_serviceid, int _children, bool _first)
        {
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append("<table width=\"350\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"border:dotted 1px #999\">");
            strReturn.Append("<tr><td colspan=\"3\" class=\"box_blue header\">");
            strReturn.Append(oService.GetName(_serviceid));
            strReturn.Append("</td></tr>");

            // From
            if (_first == false)
            {
                strReturn.Append("<tr><td colspan=\"3\" align=\"right\" class=\"smalldefault\">");
                strReturn.Append(oService.GetName(_previous_serviceid));
                strReturn.Append("</td></tr>");
            }

            // Conditions
            bool boolConditional = false;
            DataSet dsConditions = oServiceEditor.GetWorkflowConditions(_previous_serviceid, _serviceid, 1);
            if (dsConditions.Tables[0].Rows.Count > 0)
            {
                boolConditional = true;
                if (oService.GetWorkflow(_previous_serviceid, _serviceid, "only") == "1")
                    strReturn.Append("<tr><td colspan=\"3\" class=\"redbold\">Initiate ONLY when:</td></tr>");
                else if (oService.GetWorkflow(_previous_serviceid, _serviceid, "only") == "0")
                    strReturn.Append("<tr><td colspan=\"3\" class=\"redbold\">Always initiate UNLESS:</td></tr>");
                else
                    boolConditional = false;

                if (boolConditional)
                {
                    for (int cc = 0; cc < dsConditions.Tables[0].Rows.Count; cc++)
                    {
                        DataSet dsConditionValues = oServiceEditor.GetWorkflowConditionValues(Int32.Parse(dsConditions.Tables[0].Rows[cc]["id"].ToString()), 0, 0);
                        if (dsConditionValues.Tables[0].Rows.Count > 0)
                        {
                            if (cc > 0)
                                strReturn.Append("<tr><td></td><td colspan=\"2\" class=\"redbold\"> -- -- -- OR -- -- -- </td></tr>");
                            strReturn.Append("<tr><td></td><td colspan=\"2\">");
                            for (int vv = 0; vv < dsConditionValues.Tables[0].Rows.Count; vv++)
                            {
                                if (vv > 0)
                                    strReturn.Append(" <b>and</b> ");
                                strReturn.Append(dsConditionValues.Tables[0].Rows[vv]["question"].ToString());
                                strReturn.Append(" = &quot;");
                                strReturn.Append(dsConditionValues.Tables[0].Rows[vv]["value"].ToString());
                                strReturn.Append("&quot;<br/>");
                            }
                            strReturn.Append("</td></tr>");
                        }
                    }
                }
            }

            // Managers
            int intWorkflowServiceID = 0;
            if (Int32.TryParse(oService.Get(_serviceid, "workflow_userid"), out intWorkflowServiceID) == true && intWorkflowServiceID > 0)
            {
                strReturn.Append("<tr><td colspan=\"3\" class=\"greenbold\">" + (boolConditional ? "<br/>" : "") + "<b>Assign To (Service):</td></tr>");
                strReturn.Append("<tr><td></td><td colspan=\"2\">");
                strReturn.Append(oService.GetName(intWorkflowServiceID));
                strReturn.Append("</td></tr>");
            }
            else
            {
                DataSet dsTechnicians = oService.GetUser(_serviceid, 0);
                if (dsTechnicians.Tables[0].Rows.Count > 0)
                {
                    strReturn.Append("<tr><td colspan=\"3\" class=\"greenbold\">" + (boolConditional ? "<br/>" : "") + "<b>Assign To (Technician):</td></tr>");
                    foreach (DataRow drTechnician in dsTechnicians.Tables[0].Rows)
                    {
                        strReturn.Append("<tr><td></td><td colspan=\"2\">");
                        strReturn.Append(oUser.GetFullName(Int32.Parse(drTechnician["userid"].ToString())));
                        strReturn.Append("</td></tr>");
                    }
                }
                else
                {
                    strReturn.Append("<tr><td colspan=\"3\" class=\"greenbold\">" + (boolConditional ? "<br/>" : "") + "<b>Assign By:</td></tr>");
                    DataSet dsManagers = oService.GetUser(_serviceid, 1);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                    {
                        strReturn.Append("<tr><td></td><td colspan=\"2\">");
                        strReturn.Append(oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())));
                        strReturn.Append("</td></tr>");
                    }
                }
            }

            if (_first)
            {
                DataSet dsSR = oServiceEditor.GetConfigs(_serviceid, 0, 1);    // Original Request Details
                strReturn.Append("<tr><td colspan=\"3\"><br/><b>Service Request Questions:</b></td></tr>");
                foreach (DataRow drSR in dsSR.Tables[0].Rows)
                {
                    strReturn.Append("<tr><td nowrap valign=\"top\" nowrap><img src=\"/images/help.gif\" border=\"0\"/></td><td colspan=\"2\" width=\"100%\">");
                    strReturn.Append(drSR["question"].ToString() + "<br/>");
                    strReturn.Append("</td></tr>");

                    DataSet dsConfigs = oServiceEditor.GetConfigValues(Int32.Parse(drSR["id"].ToString()));
                    foreach (DataRow drConfig in dsConfigs.Tables[0].Rows)
                    {
                        strReturn.Append("<tr><td></td><td valign=\"top\" nowrap>-</td><td width=\"100%\">");
                        strReturn.Append(drConfig["value"].ToString() + "<br/>");
                        strReturn.Append("</td></tr>");
                    }
                }
            }
            
            // Workload Manager
            DataSet dsWM = oServiceEditor.GetConfigs(_serviceid, 1, 1);
            DataView dvWM = dsWM.Tables[0].DefaultView;
            dvWM.Sort = "wm ASC";
            strReturn.Append("<tr><td colspan=\"3\"><br/><b>Workload Manager Fields:</b></td></tr>");
            foreach (DataRowView drWM in dvWM)
            {
                bool boolWM = (drWM["wm"].ToString() == "1");
                bool boolInherited = (drWM["inherited"].ToString() == "1");
                strReturn.Append("<tr>");
                if (boolInherited)
                    strReturn.Append("<td nowrap valign=\"top\" nowrap><img src=\"/images/hand_right.gif\" border=\"0\"/></td><td colspan=\"2\" width=\"100%\">");
                else
                    strReturn.Append("<td nowrap valign=\"top\" nowrap><img src=\"/images/help.gif\" border=\"0\"/></td><td colspan=\"2\" width=\"100%\">");
                //strReturn.Append(boolWM ? "" : "<i>");
                //strReturn.Append(boolInherited ? "<span class=\"component_unavailable\">" : "");
                strReturn.Append(drWM["question"].ToString());
                //strReturn.Append(boolInherited ? "</span>" : "");
                //strReturn.Append(boolWM ? "" : "</i>");
                strReturn.Append("</td></tr>");

                DataSet dsConfigs = oServiceEditor.GetConfigValues(Int32.Parse(drWM["id"].ToString()));
                foreach (DataRow drConfig in dsConfigs.Tables[0].Rows)
                {
                    if (boolInherited)
                        strReturn.Append("<tr><td></td><td valign=\"top\" nowrap>-&gt;</td><td width=\"100%\">");
                    else
                        strReturn.Append("<tr><td></td><td valign=\"top\" nowrap>-</td><td width=\"100%\">");
                    //strReturn.Append(boolInherited ? "<span class=\"component_unavailable\">" : "");
                    strReturn.Append(drConfig["value"].ToString() + "<br/>");
                    //strReturn.Append(boolInherited ? "</span>" : "");
                    strReturn.Append("</td></tr>");
                }
            }

            strReturn.Append("</table><br/>");
            return strReturn;
        }
    }
}
