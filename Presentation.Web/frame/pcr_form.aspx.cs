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
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace NCC.ClearView.Presentation.Web
{
    public partial class pcr_form : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Users oUser;
        protected Pages oPage;
        protected TPM oTPM;
        protected Variables oVariable;

        protected Customized oCustomized;
        protected Costs oCost;

        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intPCRPage = Int32.Parse(ConfigurationManager.AppSettings["PCR_WORKFLOW"]);

        protected int intProfile;
        protected int intWorking;
        protected int intExecutive;



        protected int intId;
        protected DataSet ds;
        protected DataSet dsResource;
        protected string strStatus = "Pending";
        protected string strPath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);

            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oTPM = new TPM(intProfile, dsn, intEnvironment);
            oCustomized = new Customized(intProfile, dsn);
            oCost = new Costs(intProfile, dsn);
            oVariable = new Variables(intEnvironment);

            if (Request.QueryString["status"] != "" && Request.QueryString["status"] != null)
                strStatus = Request.QueryString["status"];

            if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                intId = Int32.Parse(Request.QueryString["id"]);

            if (Request.QueryString["work"] != "" && Request.QueryString["work"] != null)
                intWorking = Int32.Parse(Request.QueryString["work"]);

            if (Request.QueryString["exec"] != "" && Request.QueryString["exec"] != null)
                intExecutive = Int32.Parse(Request.QueryString["exec"]);


            if (!IsPostBack)
            {
                ds = oTPM.GetPCR(intId);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                int intParent = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                strPath = ds.Tables[0].Rows[0]["path"].ToString();

                dsResource = oCustomized.GetTPM(intRequest, intItem, intNumber);
                string strCosts = dsResource.Tables[0].Rows.Count > 0 ? dsResource.Tables[0].Rows[0]["costs"].ToString() : "No Cost Center(s)";
                string[] strCost;
                char[] strSplit = { '&' };
                strCost = strCosts.Split(strSplit);
                string strText = strCosts == "" ? "None" : "";
                foreach (string str in strCost)
                {
                    if (str != "")
                        strText += oCost.GetName(Int32.Parse(str)) + ";";
                }


                chkScope.Checked = ds.Tables[0].Rows[0]["scope"].ToString() == "1" ? true : false;
                chkSchedule.Checked = ds.Tables[0].Rows[0]["s"].ToString() == "1" ? true : false;
                chkFinancial.Checked = ds.Tables[0].Rows[0]["f"].ToString() == "1" ? true : false;

                divScope.Style["display"] = chkScope.Checked ? "inline" : "none";
                divSchedule.Style["display"] = chkSchedule.Checked ? "inline" : "none";
                divFinancial.Style["display"] = chkFinancial.Checked ? "inline" : "none";

                txtPCRScheduleDS.Text = ds.Tables[0].Rows[0]["sds"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["sds"].ToString()).ToShortDateString() : "";
                txtPCRScheduleDE.Text = ds.Tables[0].Rows[0]["sde"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["sde"].ToString()).ToShortDateString() : "";
                txtPCRSchedulePS.Text = ds.Tables[0].Rows[0]["sps"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["sps"].ToString()).ToShortDateString() : "";
                txtPCRSchedulePE.Text = ds.Tables[0].Rows[0]["spe"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["spe"].ToString()).ToShortDateString() : "";
                txtPCRScheduleES.Text = ds.Tables[0].Rows[0]["ses"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["ses"].ToString()).ToShortDateString() : "";
                txtPCRScheduleEE.Text = ds.Tables[0].Rows[0]["see"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["see"].ToString()).ToShortDateString() : "";
                txtPCRScheduleCS.Text = ds.Tables[0].Rows[0]["scs"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["scs"].ToString()).ToShortDateString() : "";
                txtPCRScheduleCE.Text = ds.Tables[0].Rows[0]["sce"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["sce"].ToString()).ToShortDateString() : "";

                chkPCRScheduleD.Checked = txtPCRScheduleDS.Text != "" || txtPCRScheduleDE.Text != "";
                chkPCRScheduleP.Checked = txtPCRSchedulePS.Text != "" || txtPCRSchedulePE.Text != "";
                chkPCRScheduleE.Checked = txtPCRScheduleES.Text != "" || txtPCRScheduleEE.Text != "";
                chkPCRScheduleC.Checked = txtPCRScheduleCS.Text != "" || txtPCRScheduleCE.Text != "";

                if (dsResource.Tables[0].Rows[0]["appsd"] == DBNull.Value || dsResource.Tables[0].Rows[0]["apped"] == DBNull.Value)
                {
                    chkPCRScheduleD.Enabled = false;
                    lblPCRScheduleD.Text = "N / A";
                }
                else
                    lblPCRScheduleD.Text = GetDate(dsResource.Tables[0].Rows[0]["appsd"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["apped"].ToString());
                if (dsResource.Tables[0].Rows[0]["appsp"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appep"] == DBNull.Value)
                {
                    chkPCRScheduleP.Enabled = false;
                    lblPCRScheduleP.Text = "N / A";
                }
                else
                    lblPCRScheduleP.Text = GetDate(dsResource.Tables[0].Rows[0]["appsp"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appep"].ToString());
                if (dsResource.Tables[0].Rows[0]["appse"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appee"] == DBNull.Value)
                {
                    chkPCRScheduleE.Enabled = false;
                    lblPCRScheduleE.Text = "N / A";
                }
                else
                    lblPCRScheduleE.Text = GetDate(dsResource.Tables[0].Rows[0]["appse"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appee"].ToString());
                if (dsResource.Tables[0].Rows[0]["appsc"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appec"] == DBNull.Value)
                {
                    chkPCRScheduleC.Enabled = false;
                    lblPCRScheduleC.Text = "N / A";
                }
                else
                    lblPCRScheduleC.Text = GetDate(dsResource.Tables[0].Rows[0]["appsc"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appec"].ToString());


                double dblAppDI = GetFloat(dsResource.Tables[0].Rows[0]["appid"].ToString());
                double dblAppDE = GetFloat(dsResource.Tables[0].Rows[0]["appexd"].ToString());
                double dblAppDH = GetFloat(dsResource.Tables[0].Rows[0]["apphd"].ToString());
                double dblAppD = dblAppDI + dblAppDE + dblAppDH;
                lblPCRFinancialD.Text = "$" + dblAppD.ToString("N");


                double dblAppPI = GetFloat(dsResource.Tables[0].Rows[0]["appip"].ToString());
                double dblAppPE = GetFloat(dsResource.Tables[0].Rows[0]["appexp"].ToString());
                double dblAppPH = GetFloat(dsResource.Tables[0].Rows[0]["apphp"].ToString());
                double dblAppP = dblAppPI + dblAppPE + dblAppPH;
                lblPCRFinancialP.Text = "$" + dblAppP.ToString("N");

                double dblAppEI = GetFloat(dsResource.Tables[0].Rows[0]["appie"].ToString());
                double dblAppEE = GetFloat(dsResource.Tables[0].Rows[0]["appexe"].ToString());
                double dblAppEH = GetFloat(dsResource.Tables[0].Rows[0]["apphe"].ToString());
                double dblAppE = dblAppEI + dblAppEE + dblAppEH;
                lblPCRFinancialE.Text = "$" + dblAppE.ToString("N");


                double dblAppCI = GetFloat(dsResource.Tables[0].Rows[0]["appic"].ToString());
                double dblAppCE = GetFloat(dsResource.Tables[0].Rows[0]["appexc"].ToString());
                double dblAppCH = GetFloat(dsResource.Tables[0].Rows[0]["apphc"].ToString());
                double dblAppC = dblAppCI + dblAppCE + dblAppCH;
                lblPCRFinancialC.Text = "$" + dblAppC.ToString("N");


                txtPCRFinancialD.Text = ds.Tables[0].Rows[0]["fd"].ToString();
                txtPCRFinancialP.Text = ds.Tables[0].Rows[0]["fp"].ToString();
                txtPCRFinancialE.Text = ds.Tables[0].Rows[0]["fe"].ToString();
                txtPCRFinancialC.Text = ds.Tables[0].Rows[0]["fc"].ToString();

                chkPCRFinancialD.Checked = txtPCRFinancialD.Text != "0";
                chkPCRFinancialP.Checked = txtPCRFinancialP.Text != "0";
                chkPCRFinancialE.Checked = txtPCRFinancialE.Text != "0";
                chkPCRFinancialC.Checked = txtPCRFinancialC.Text != "0";

                chkReason.Checked = ds.Tables[0].Rows[0]["reasons"].ToString() != "";
                divReason.Style["display"] = chkReason.Checked ? "inline" : "none";
                string[] strReasons = ds.Tables[0].Rows[0]["reasons"].ToString().Split(';');
                foreach (string str in strReasons)
                {
                    for (int ii = 0; ii < chkPCRReason.Items.Count; ii++)
                    {
                        if (chkPCRReason.Items[ii].Value == str)
                        {
                            chkPCRReason.Items[ii].Selected = true;
                            break;
                        }
                    }
                }
                txtScopeComments.Text = ds.Tables[0].Rows[0]["scopecomments"].ToString();
                txtScheduleComments.Text = ds.Tables[0].Rows[0]["schcomments"].ToString();
                txtFinancialComments.Text = ds.Tables[0].Rows[0]["fincomments"].ToString();

                int intStatus = Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                if (intStatus == 1) strStatus = "Approved";
                else if (intStatus == -1) strStatus = "Denied";
                hdnStatus.Value = strStatus.ToUpper();


            }

            chkScope.Attributes.Add("onclick", "ShowHideDivCheck('" + divScope.ClientID + "',this);");
            chkSchedule.Attributes.Add("onclick", "ShowHideDivCheck('" + divSchedule.ClientID + "',this);");
            chkFinancial.Attributes.Add("onclick", "ShowHideDivCheck('" + divFinancial.ClientID + "',this);");
            chkReason.Attributes.Add("onclick", "ShowHideDivCheck('" + divReason.ClientID + "',this);");
            btnUpdate.Attributes.Add("onclick", "return ValidateStatus('" + hdnStatus.Value + "') && ValidatePCR('" + chkScope.ClientID + "'" +
                                     ",'" + chkSchedule.ClientID + "'" +
                                     ",'" + chkPCRScheduleD.ClientID + "'" +
                                     ",'" + txtPCRScheduleDS.ClientID + "'" +
                                     ",'" + txtPCRScheduleDE.ClientID + "'" +
                                     ",'" + chkPCRScheduleP.ClientID + "'" +
                                     ",'" + txtPCRSchedulePS.ClientID + "'" +
                                     ",'" + txtPCRSchedulePE.ClientID + "'" +
                                     ",'" + chkPCRScheduleE.ClientID + "'" +
                                     ",'" + txtPCRScheduleES.ClientID + "'" +
                                     ",'" + txtPCRScheduleEE.ClientID + "'" +
                                     ",'" + chkPCRScheduleC.ClientID + "'" +
                                     ",'" + txtPCRScheduleCS.ClientID + "'" +
                                     ",'" + txtPCRScheduleCE.ClientID + "'" +
                                     ",'" + chkFinancial.ClientID + "'" +
                                     ",'" + chkPCRFinancialD.ClientID + "'" +
                                     ",'" + txtPCRFinancialD.ClientID + "'" +
                                     ",'" + chkPCRFinancialP.ClientID + "'" +
                                     ",'" + txtPCRFinancialP.ClientID + "'" +
                                     ",'" + chkPCRFinancialE.ClientID + "'" +
                                     ",'" + txtPCRFinancialE.ClientID + "'" +
                                     ",'" + chkPCRFinancialC.ClientID + "'" +
                                     ",'" + txtPCRFinancialC.ClientID + "'" +
                                     ");");
            imgPCRScheduleCE.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRScheduleCE.ClientID + "');");
            imgPCRScheduleCS.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRScheduleCS.ClientID + "');");
            imgPCRScheduleDE.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRScheduleDE.ClientID + "');");
            imgPCRScheduleDS.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRScheduleDS.ClientID + "');");
            imgPCRScheduleEE.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRScheduleEE.ClientID + "');");
            imgPCRScheduleES.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRScheduleES.ClientID + "');");
            imgPCRSchedulePE.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRSchedulePE.ClientID + "');");
            imgPCRSchedulePS.Attributes.Add("onclick", "return OpenCalendar('" + txtPCRSchedulePS.ClientID + "');");
            imgPCRFinancialC.Attributes.Add("onclick", "return OpenCalculator('" + txtPCRFinancialC.ClientID + "');");
            imgPCRFinancialD.Attributes.Add("onclick", "return OpenCalculator('" + txtPCRFinancialD.ClientID + "');");
            imgPCRFinancialE.Attributes.Add("onclick", "return OpenCalculator('" + txtPCRFinancialE.ClientID + "');");
            imgPCRFinancialP.Attributes.Add("onclick", "return OpenCalculator('" + txtPCRFinancialP.ClientID + "');");

        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {

            UpdatePDF(oTPM.GetPCRPath(intId));
            string strReason = "";
            for (int ii = 0; ii < chkPCRReason.Items.Count; ii++)
            {
                if (chkPCRReason.Items[ii].Selected == true)
                    strReason += chkPCRReason.Items[ii].Value + ";";
            }

            oTPM.UpdatePCR(intId, (chkScope.Checked ? 1 : 0), (chkSchedule.Checked ? 1 : 0), txtPCRScheduleDS.Text, txtPCRScheduleDE.Text, txtPCRSchedulePS.Text, txtPCRSchedulePE.Text, txtPCRScheduleES.Text, txtPCRScheduleEE.Text, txtPCRScheduleCS.Text, txtPCRScheduleCE.Text, (chkFinancial.Checked ? 1 : 0), txtPCRFinancialD.Text, txtPCRFinancialP.Text, txtPCRFinancialE.Text, txtPCRFinancialC.Text, strReason, txtScopeComments.Text, txtScheduleComments.Text, txtFinancialComments.Text);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
        }

        private void UpdatePDF(string strPath)
        {
            string strPhysicalPath = oVariable.DocumentsFolder() + strPath;
            Document doc = new Document();
            FileStream fs = null;

            try
            {
                fs = new FileStream(strPhysicalPath, FileMode.Create);
                PdfWriter.GetInstance(doc, fs);
                string strHeader = "ClearView PCR Information";
                HeaderFooter header = new HeaderFooter(new Phrase(strHeader, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                header.Alignment = 2;
                doc.Header = header;
                string strFooter = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                HeaderFooter footer = new HeaderFooter(new Phrase(strFooter, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                footer.Alignment = 2;
                doc.Footer = footer;
                doc.Open();


                Cell cell;
                iTextSharp.text.Table oTable = new iTextSharp.text.Table(2);
                oTable.BorderWidth = 0;
                oTable.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable.Padding = 2;
                oTable.Width = 100;

                iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
                iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
                iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);

                cell = new Cell(new Phrase("Project Change Request Report", oFontHeader));
                cell.Colspan = 2;
                cell.BackgroundColor = new iTextSharp.text.Color(169, 162, 141);
                oTable.AddCell(cell);

                ds = oTPM.GetPCR(intId);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                dsResource = oCustomized.GetTPM(intRequest, intItem, intNumber);


                oTable.AddCell(new Cell(new Phrase("Scope:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase((chkScope.Checked ? "Yes" : "No"), oFont)));
                oTable.AddCell(new Cell(new Phrase("Schedule:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase((chkSchedule.Checked ? "Yes" : "No"), oFont)));
                oTable.AddCell(new Cell(new Phrase("Financial:", oFontBold)));
                oTable.AddCell(new Cell(new Phrase((chkFinancial.Checked ? "Yes" : "No"), oFont)));

                cell = new Cell(new Phrase("Detailed Description of Proposed Change", oFontBold));
                cell.Colspan = 2;
                oTable.AddCell(cell);

                oTable.AddCell(new Cell(new Phrase("Scope:", oFontBold)));
                oTable.AddCell(new Cell(new Paragraph(txtScopeComments.Text, oFont)));

                oTable.AddCell(new Cell(new Phrase("Schedule:", oFontBold)));
                oTable.AddCell(new Cell(new Paragraph(txtScheduleComments.Text, oFont)));

                oTable.AddCell(new Cell(new Phrase("Financial:", oFontBold)));
                oTable.AddCell(new Cell(new Paragraph(txtFinancialComments.Text, oFont)));

                doc.Add(oTable);


                //style=\"border:dashed 1px #CCCCCC\" class=\"lightdefault\"
                iTextSharp.text.Table oTable2 = new iTextSharp.text.Table(3);
                oTable2.BorderWidth = 0;
                oTable2.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable2.Padding = 2;
                oTable2.Width = 100;

                cell = new Cell(new Phrase("Schedule Change Details", oFontBold));
                cell.Colspan = 3;
                oTable2.AddCell(cell);

                oTable2.AddCell(new Cell(new Phrase("Phase", oFontBold)));
                oTable2.AddCell(new Cell(new Phrase("Approved Dates", oFontBold)));
                oTable2.AddCell(new Cell(new Phrase("Modified Dates", oFontBold)));

                oTable2.AddCell(new Cell(new Phrase("Discovery", oFont)));
                if (dsResource.Tables[0].Rows[0]["appsd"] == DBNull.Value || dsResource.Tables[0].Rows[0]["apped"] == DBNull.Value)
                    oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
                else
                    oTable2.AddCell(new Cell(new Phrase(GetDate(dsResource.Tables[0].Rows[0]["appsd"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["apped"].ToString()), oFont)));

                oTable2.AddCell(new Cell(new Phrase(GetDate(txtPCRScheduleDS.Text) + " - " + GetDate(txtPCRScheduleDE.Text), oFont)));

                oTable2.AddCell(new Cell(new Phrase("Planning", oFont)));
                if (dsResource.Tables[0].Rows[0]["appsp"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appep"] == DBNull.Value)
                    oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
                else
                    oTable2.AddCell(new Cell(new Phrase(GetDate(dsResource.Tables[0].Rows[0]["appsp"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appep"].ToString()), oFont)));

                oTable2.AddCell(new Cell(new Phrase(GetDate(txtPCRSchedulePS.Text) + " - " + GetDate(txtPCRSchedulePE.Text), oFont)));

                oTable2.AddCell(new Cell(new Phrase("Execution", oFont)));
                if (dsResource.Tables[0].Rows[0]["appse"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appee"] == DBNull.Value)
                    oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
                else
                    oTable2.AddCell(new Cell(new Phrase(GetDate(dsResource.Tables[0].Rows[0]["appse"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appee"].ToString()), oFont)));

                oTable2.AddCell(new Cell(new Phrase(GetDate(txtPCRScheduleES.Text) + " - " + GetDate(txtPCRScheduleEE.Text), oFont)));

                oTable2.AddCell(new Cell(new Phrase("Closing", oFont)));
                if (dsResource.Tables[0].Rows[0]["appsc"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appec"] == DBNull.Value)
                    oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
                else
                    oTable2.AddCell(new Cell(new Phrase(GetDate(dsResource.Tables[0].Rows[0]["appsc"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appec"].ToString()), oFont)));


                oTable2.AddCell(new Cell(new Phrase(GetDate(txtPCRScheduleCS.Text) + " - " + GetDate(txtPCRScheduleCE.Text), oFont)));
                doc.Add(oTable2);




                iTextSharp.text.Table oTable3 = new iTextSharp.text.Table(3);
                oTable3.BorderWidth = 0;
                oTable3.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable3.Padding = 2;
                oTable3.Width = 100;

                cell = new Cell(new Phrase("Financial Change Details", oFontBold));
                cell.Colspan = 3;
                oTable3.AddCell(cell);

                oTable3.AddCell(new Cell(new Phrase("Phase", oFontBold)));
                oTable3.AddCell(new Cell(new Phrase("Approved Financials", oFontBold)));
                oTable3.AddCell(new Cell(new Phrase("Modified Financials", oFontBold)));


                double dblAppDI = GetFloat(dsResource.Tables[0].Rows[0]["appid"].ToString());
                double dblAppDE = GetFloat(dsResource.Tables[0].Rows[0]["appexd"].ToString());
                double dblAppDH = GetFloat(dsResource.Tables[0].Rows[0]["apphd"].ToString());
                double dblActDI = GetFloat(dsResource.Tables[0].Rows[0]["actid"].ToString());
                double dblActDE = GetFloat(dsResource.Tables[0].Rows[0]["acted"].ToString());
                double dblActDH = GetFloat(dsResource.Tables[0].Rows[0]["acthd"].ToString());
                double dblEstDI = GetFloat(dsResource.Tables[0].Rows[0]["estid"].ToString());
                double dblEstDE = GetFloat(dsResource.Tables[0].Rows[0]["ested"].ToString());
                double dblEstDH = GetFloat(dsResource.Tables[0].Rows[0]["esthd"].ToString());
                double dblForeDI = dblActDI + dblEstDI;
                double dblForeDE = dblActDE + dblEstDE;
                double dblForeDH = dblActDH + dblEstDH;
                double dblAppD = dblAppDI + dblAppDE + dblAppDH;
                double dblActD = dblActDI + dblActDE + dblActDH;
                double dblForeD = dblForeDI + dblForeDE + dblForeDH;




                double dblAppPI = GetFloat(dsResource.Tables[0].Rows[0]["appip"].ToString());
                double dblAppPE = GetFloat(dsResource.Tables[0].Rows[0]["appexp"].ToString());
                double dblAppPH = GetFloat(dsResource.Tables[0].Rows[0]["apphp"].ToString());
                double dblActPI = GetFloat(dsResource.Tables[0].Rows[0]["actip"].ToString());
                double dblActPE = GetFloat(dsResource.Tables[0].Rows[0]["actep"].ToString());
                double dblActPH = GetFloat(dsResource.Tables[0].Rows[0]["acthp"].ToString());
                double dblEstPI = GetFloat(dsResource.Tables[0].Rows[0]["estip"].ToString());
                double dblEstPE = GetFloat(dsResource.Tables[0].Rows[0]["estep"].ToString());
                double dblEstPH = GetFloat(dsResource.Tables[0].Rows[0]["esthp"].ToString());
                double dblForePI = dblActPI + dblEstPI;
                double dblForePE = dblActPE + dblEstPE;
                double dblForePH = dblActPH + dblEstPH;
                double dblAppP = dblAppPI + dblAppPE + dblAppPH;
                double dblActP = dblActPI + dblActPE + dblActPH;
                double dblForeP = dblForePI + dblForePE + dblForePH;

                double dblAppEI = GetFloat(dsResource.Tables[0].Rows[0]["appie"].ToString());
                double dblAppEE = GetFloat(dsResource.Tables[0].Rows[0]["appexe"].ToString());
                double dblAppEH = GetFloat(dsResource.Tables[0].Rows[0]["apphe"].ToString());
                double dblActEI = GetFloat(dsResource.Tables[0].Rows[0]["actie"].ToString());
                double dblActEE = GetFloat(dsResource.Tables[0].Rows[0]["actee"].ToString());
                double dblActEH = GetFloat(dsResource.Tables[0].Rows[0]["acthe"].ToString());
                double dblEstEI = GetFloat(dsResource.Tables[0].Rows[0]["estie"].ToString());
                double dblEstEE = GetFloat(dsResource.Tables[0].Rows[0]["estee"].ToString());
                double dblEstEH = GetFloat(dsResource.Tables[0].Rows[0]["esthe"].ToString());
                double dblForeEI = dblActEI + dblEstEI;
                double dblForeEE = dblActEE + dblEstEE;
                double dblForeEH = dblActEH + dblEstEH;
                double dblAppE = dblAppEI + dblAppEE + dblAppEH;
                double dblActE = dblActEI + dblActEE + dblActEH;
                double dblForeE = dblForeEI + dblForeEE + dblForeEH;

                double dblAppCI = GetFloat(dsResource.Tables[0].Rows[0]["appic"].ToString());
                double dblAppCE = GetFloat(dsResource.Tables[0].Rows[0]["appexc"].ToString());
                double dblAppCH = GetFloat(dsResource.Tables[0].Rows[0]["apphc"].ToString());
                double dblActCI = GetFloat(dsResource.Tables[0].Rows[0]["actic"].ToString());
                double dblActCE = GetFloat(dsResource.Tables[0].Rows[0]["actec"].ToString());
                double dblActCH = GetFloat(dsResource.Tables[0].Rows[0]["acthc"].ToString());
                double dblEstCI = GetFloat(dsResource.Tables[0].Rows[0]["estic"].ToString());
                double dblEstCE = GetFloat(dsResource.Tables[0].Rows[0]["estec"].ToString());
                double dblEstCH = GetFloat(dsResource.Tables[0].Rows[0]["esthc"].ToString());
                double dblForeCI = dblActCI + dblEstCI;
                double dblForeCE = dblActCE + dblEstCE;
                double dblForeCH = dblActCH + dblEstCH;
                double dblAppC = dblAppCI + dblAppCE + dblAppCH;
                double dblActC = dblActCI + dblActCE + dblActCH;
                double dblForeC = dblForeCI + dblForeCE + dblForeCH;


                double dblFD = GetFloat(txtPCRFinancialD.Text);
                double dblFP = GetFloat(txtPCRFinancialP.Text);
                double dblFE = GetFloat(txtPCRFinancialE.Text);
                double dblFC = GetFloat(txtPCRFinancialC.Text);


                oTable3.AddCell(new Cell(new Phrase("Discovery", oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblAppD.ToString("N"), oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblFD.ToString("N"), oFont)));

                oTable3.AddCell(new Cell(new Phrase("Planning", oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblAppP.ToString("N"), oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblFP.ToString("N"), oFont)));

                oTable3.AddCell(new Cell(new Phrase("Execution", oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblAppE.ToString("N"), oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblFE.ToString("N"), oFont)));

                oTable3.AddCell(new Cell(new Phrase("Closing", oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblAppC.ToString("N"), oFont)));
                oTable3.AddCell(new Cell(new Phrase("$" + dblFC.ToString("N"), oFont)));
                doc.Add(oTable3);




                iTextSharp.text.Table oTable5 = new iTextSharp.text.Table(4);
                oTable5.BorderWidth = 0;
                oTable5.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable5.Padding = 2;
                oTable5.Width = 100;



                cell = new Cell(new Phrase("Detail Of Financial Impact Of Proposed Change", oFontBold));
                cell.Colspan = 4;
                oTable5.AddCell(cell);



                cell = new Cell(new Phrase("Discovery", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Change in Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("New Budget Total", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);



                oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                cell = new Cell(new Phrase("$" + dblAppDI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActDI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeDI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);

                oTable5.AddCell(new Phrase("External Labor", oFont));
                cell = new Cell(new Phrase("$" + dblAppDE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActDE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeDE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                cell = new Cell(new Phrase("$" + dblAppDH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActDH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeDH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);




                oTable5.AddCell(new Phrase("Total", oFont));
                cell = new Cell(new Phrase("$" + dblAppD.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActD.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeD.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);



                cell = new Cell();
                cell.Colspan = 4;
                oTable5.AddCell(cell);


                cell = new Cell(new Phrase("Planning", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Change in Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("New Budget Total", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                cell = new Cell(new Phrase("$" + dblAppPI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActPI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForePI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);

                oTable5.AddCell(new Phrase("External Labor", oFont));
                cell = new Cell(new Phrase("$" + dblAppPE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActPE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForePE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                cell = new Cell(new Phrase("$" + dblAppPH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActPH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForePH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);



                oTable5.AddCell(new Phrase("Total", oFont));
                cell = new Cell(new Phrase("$" + dblAppP.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActP.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeP.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                cell = new Cell();
                cell.Colspan = 4;
                oTable5.AddCell(cell);


                cell = new Cell(new Phrase("Execution", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Change in Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("New Budget Total", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);

                oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                cell = new Cell(new Phrase("$" + dblAppEI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActEI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeEI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                oTable5.AddCell(new Phrase("External Labor", oFont));
                cell = new Cell(new Phrase("$" + dblAppEE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActEE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeEE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                cell = new Cell(new Phrase("$" + dblAppEH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActEH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeEH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);



                oTable5.AddCell(new Phrase("Total", oFont));
                cell = new Cell(new Phrase("$" + dblAppE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);

                cell = new Cell();
                cell.Colspan = 4;
                oTable5.AddCell(cell);



                cell = new Cell(new Phrase("Closing", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Current Approved Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("Change in Budget", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("New Budget Total", oFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);

                oTable5.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
                cell = new Cell(new Phrase("$" + dblAppCI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActCI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeCI.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);

                oTable5.AddCell(new Phrase("External Labor", oFont));
                cell = new Cell(new Phrase("$" + dblAppCE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActCE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeCE.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                oTable5.AddCell(new Phrase("HW/SW/One Time Cost", oFont));
                cell = new Cell(new Phrase("$" + dblAppCH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActCH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeCH.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);


                oTable5.AddCell(new Phrase("Total", oFont));
                cell = new Cell(new Phrase("$" + dblAppC.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblActC.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                cell = new Cell(new Phrase("$" + dblForeC.ToString("N"), oFont));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTable5.AddCell(cell);
                doc.Add(oTable5);



                iTextSharp.text.Table oTable4 = new iTextSharp.text.Table(3);
                oTable4.BorderWidth = 0;
                oTable4.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTable4.Padding = 2;
                oTable4.Width = 100;
                cell = new Cell(new Phrase("Reason for PCR ", oFontBold));
                cell.Colspan = 3;
                oTable4.AddCell(cell);


                iTextSharp.text.List list = new List(false, 10);
                list.IndentationRight = 2.5F;
                list.ListSymbol = new Chunk("\u2022", FontFactory.GetFont(FontFactory.HELVETICA, 10));


                for (int ii = 0; ii < chkPCRReason.Items.Count; ii++)
                {
                    if (chkPCRReason.Items[ii].Selected == true)
                        list.Add(new iTextSharp.text.ListItem(chkPCRReason.Items[ii].Value, oFont));
                }

                cell = new Cell(list);
                cell.Colspan = 3;
                oTable4.AddCell(cell);
                doc.Add(oTable4);
            }
            catch { }
            finally
            {
                doc.Close();
                fs.Close();
            }

        }

        private string GetDate(string _date)
        {
            if (_date == "")
                return "";
            else
                return DateTime.Parse(_date).ToShortDateString();
        }
        private double GetFloat(string _float)
        {
            if (_float == "")
                return 0.00;
            else
                return double.Parse(_float);
        }
    }
}
