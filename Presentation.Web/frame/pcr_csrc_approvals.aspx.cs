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
    public partial class pcr_csrc_approvals : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected Pages oPage;
        protected Variables oVariable;
        protected TPM oTPM;
        protected Customized oCustomized;
        protected Functions oFunction;

        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intPCRPage = Int32.Parse(ConfigurationManager.AppSettings["PCR_WORKFLOW"]);
        protected int intCSRCPage = Int32.Parse(ConfigurationManager.AppSettings["CSRC_WORKFLOW"]);

        protected int intProfile;
        protected int intWorking;
        protected int intExecutive;

        protected int intId;
        protected string strRoute;
        protected string strView;
        protected int intCC;

        protected string strAttachement = "";


        private string strEMailIdsBCC = "";
        protected DataSet ds;

        protected string[] strChecks;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oTPM = new TPM(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oCustomized = new Customized(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            if (Request.QueryString["route"] != "" && Request.QueryString["route"] != null)
            {
                strRoute = Request.QueryString["route"];
                panRoute.Visible = true;
            }

            if (Request.QueryString["view"] != "" && Request.QueryString["view"] != null)
            {
                strView = Request.QueryString["view"];
                panView.Visible = true;
            }

            if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                intId = Int32.Parse(Request.QueryString["id"]);

            if (Request.QueryString["work"] != "" && Request.QueryString["work"] != null)
                intWorking = Int32.Parse(Request.QueryString["work"]);

            if (Request.QueryString["exec"] != "" && Request.QueryString["exec"] != null)
                intExecutive = Int32.Parse(Request.QueryString["exec"]);

            if (Request.QueryString["checks"] != "" && Request.QueryString["checks"] != null)
                strChecks = Request.QueryString["checks"].Split(' ');

            if (strView == "PCR")
            {
                ds = oTPM.GetPCRDetail(intId);
                rptView.DataSource = ds;
                rptView.DataBind();
                panNoView.Visible = rptView.Items.Count == 0;
            }
            else if (strView == "CSRC")
            {
                ds = oTPM.GetCSRCDetail(intId);
                rptView.DataSource = ds;
                rptView.DataBind();
                panNoView.Visible = rptView.Items.Count == 0;
            }
            else
            {

                lblW.Text = oUser.GetFullName(intWorking);
                lblE.Text = oUser.GetFullName(intExecutive);
                strAttachement += "<tr><td class=\"greenheader\" colspan=\"2\"><b>Attachement(s):</b></td></tr>";
                foreach (string str in strChecks)
                {

                    if (str != "")
                    {
                        string strPath = "";
                        if (strRoute == "CSRC")
                            strPath = oTPM.GetCSRCPath(Int32.Parse(str));
                        else if (strRoute == "PCR")
                            strPath = oTPM.GetPCRPath(Int32.Parse(str));
                        else
                        {
                            ds = oTPM.GetProjectClosurePDF(Int32.Parse(str));
                            strPath = ds.Tables[0].Rows[0]["path"].ToString();
                        }
                        string strURL = oVariable.URL() + "/" + strPath.Replace("\\", "/");
                        strAttachement += "<tr><td valign=\"top\" nowrap><a href=\"" + strURL + "\" target=\"_blank\"><img src=\"/images/icons/pdf.gif \" align=\"absmiddle\" border=\"0\" />&nbsp;" + strPath.Substring(strPath.IndexOf("\\") + 1) + "</a></td></tr> ";

                    }
                }

            }

            txtD.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divD.ClientID + "','" + lstD.ClientID + "','" + hdnD.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstD.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtC.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divC.ClientID + "','" + lstC.ClientID + "','" + hdnC.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstC.Attributes.Add("ondblclick", "AJAXClickRow();");


            if (strRoute == "PCR" || strRoute == "CSRC")
            {
                btnRoute.Text = "Route " + strRoute;
                btnRoute.Attributes.Add("onclick", " return confirm('NOTE:This will route the " + strRoute + " to the Approvers listed in the form.\\n Are you sure?');");

            }
            else
            {
                btnRoute.Text = "Send Notification";
                btnRoute.Width = Unit.Pixel(170);
            }


        }

        protected void btnRoute_Click(object sender, EventArgs e)
        {
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            if (strRoute == "CSRC")
            {

                foreach (string str in strChecks)
                {
                    if (str != "")
                    {
                        if (Request.Form[hdnC.UniqueID] != "")
                        {
                            intCC = Int32.Parse(Request.Form[hdnC.UniqueID]);
                            oTPM.UpdateCSRCCC(intId, intCC);
                        }
                        intId = Int32.Parse(str);
                        oTPM.AddCSRCDetail(intId, 1, intWorking, 0);
                        oTPM.AddCSRCDetail(intId, 2, intExecutive, -10);
                        //strBCC = oUser.GetFullName(intCC);
                        if (Request.Form[hdnD.UniqueID] != "")
                            oTPM.AddCSRCDetail(intId, 3, Int32.Parse(Request.Form[hdnD.UniqueID]), -10);
                        oTPM.ApproveCSRC(intId, 1, 0, intCSRCPage, intWorkloadManager,  oVariable.DocumentsFolder() + oTPM.GetCSRCPath(intId));

                    }
                }

            }
            else if (strRoute == "PCR")
            {

                foreach (string str in strChecks)
                {
                    if (str != "")
                    {
                        if (Request.Form[hdnC.UniqueID] != "")
                        {
                            intCC = Int32.Parse(Request.Form[hdnC.UniqueID]);
                            oTPM.UpdatePCRCC(intId, intCC);
                        }
                        intId = Int32.Parse(str);
                        oTPM.AddPCRDetail(intId, 1, intWorking, 0);
                        oTPM.AddPCRDetail(intId, 2, intExecutive, -10);
                        //strBCC = oUser.GetFullName(intCC);
                        if (Request.Form[hdnD.UniqueID] != "")
                            oTPM.AddPCRDetail(intId, 3, Int32.Parse(Request.Form[hdnD.UniqueID]), -10);

                        oTPM.ApprovePCR(intId, 1, 0, intPCRPage, intWorkloadManager,  oVariable.DocumentsFolder() + oTPM.GetPCRPath(intId));
                    }

                }

            }
            else
            {
                foreach (string str in strChecks)
                {
                    if (str != "")
                    {
                        if (Request.Form[hdnC.UniqueID] != "")
                            intCC = Int32.Parse(Request.Form[hdnC.UniqueID]);
                        string strPath = oTPM.GetProjectClosurePDF(Int32.Parse(str)).Tables[0].Rows[0]["path"].ToString();
                        string strName = strPath.Substring(strPath.IndexOf("\\") + 1, strPath.IndexOf("_") - 8);
                        oFunction.SendEmail("ClearView Project Closure Notification", oUser.GetName(intWorking), oUser.GetFullName(intCC), strEMailIdsBCC, "ClearView Project Closure Notification", "<p><b>This notification is sent to confirm the closure of Project \" " + strName + " \" . Click on the attachement to view the report.</b></p>", true, false, oVariable.DocumentsFolder() + strPath);
                        oFunction.SendEmail("ClearView Project Closure Notification", oUser.GetName(intExecutive), oUser.GetFullName(intCC), strEMailIdsBCC, "ClearView Project Closure Notification", "<p><b>This notification is sent to confirm the closure of Project \" " + strName + " \" . Click on the attachement to view the report.</b></p>", true, false, oVariable.DocumentsFolder() + strPath);
                        if (Request.Form[hdnD.UniqueID] != "")
                            oFunction.SendEmail("ClearView Project Closure Notification", oUser.GetName(Int32.Parse(Request.Form[hdnD.UniqueID])), oUser.GetFullName(intCC), strEMailIdsBCC, "ClearView Project Closure Notification", "<p><b>This notification is sent to confirm the closure of Project \" " + strName + " \" . Click on the attachement to view the report.</b></p>", true, false, oVariable.DocumentsFolder() + strPath);
                    }

                }
            }

            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);

        }

        protected string GetRole(int intStep)
        {
            string strRole = "Additional";

            if (intStep == 1)
                strRole = "Working Sponsor";
            else if (intStep == 2)
                strRole = "Executive Sponsor";

            return strRole;
        }

        protected string GetStatus(int intStatus)
        {
            string strStatus = "Pending";
            if (intStatus == 1)
                strStatus = "Approved";
            if (intStatus == -1)
                strStatus = "Denied";

            return strStatus;
        }
    }
}
