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
    public partial class pcr_csrc_view_approvals : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Users oUser;
        protected Pages oPage;
        protected Variables oVariable;
        protected TPM oTPM;
        protected Customized oCustomized;

        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intPCRPage = Int32.Parse(ConfigurationManager.AppSettings["PCR_WORKFLOW"]);
        protected int intCSRCPage = Int32.Parse(ConfigurationManager.AppSettings["CSRC_WORKFLOW"]);

        protected int intProfile;
        protected int intWorking;
        protected int intExecutive;
        protected int intPCR;

        protected int intLead;
        protected int intId;
        protected string strRoute;
        protected int intCC;
        protected int intDetailCount;
        protected string strAttachement = "";


        protected DataSet ds;
        protected string[] strChecks;
        private Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oTPM = new TPM(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oCustomized = new Customized(intProfile, dsn);

            if (Request.QueryString["route"] != "" && Request.QueryString["route"] != null)
                strRoute = Request.QueryString["route"];

            if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                intId = Int32.Parse(Request.QueryString["id"]);

            if (Request.QueryString["work"] != "" && Request.QueryString["work"] != null)
                intWorking = Int32.Parse(Request.QueryString["work"]);

            if (Request.QueryString["exec"] != "" && Request.QueryString["exec"] != null)
                intExecutive = Int32.Parse(Request.QueryString["exec"]);

            if (Request.QueryString["checks"] != "" && Request.QueryString["checks"] != null)
                strChecks = Request.QueryString["checks"].Split(' ');

            if (strRoute == "CSRC")
                intDetailCount = oTPM.GetCSRCDetail(intId).Tables[0].Rows.Count;
            else
                intDetailCount = oTPM.GetPCRDetail(intId).Tables[0].Rows.Count;



            lblW.Text = oUser.GetFullName(intWorking);
            lblE.Text = oUser.GetFullName(intExecutive);
            strAttachement += "<tr><td class=\"greenheader\" colspan=\"2\"><b>PCR Attachement(s):</b></td></tr>";
            foreach (string str in strChecks)
            {

                if (str != "")
                {
                    if (strRoute == "CSRC")
                        ExportCSRCtoPDF(Int32.Parse(str));
                    else
                        ExportToPDF(Int32.Parse(str));
                }
            }

            txtD.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divD.ClientID + "','" + lstD.ClientID + "','" + hdnD.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstD.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtC.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divC.ClientID + "','" + lstC.ClientID + "','" + hdnC.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstC.Attributes.Add("ondblclick", "AJAXClickRow();");
            //btnRoute.Attributes.Add("onclick", "return confirm('NOTE:This will route the PCR to the Approvers listed in the form.\\n Are you sure?');");
            btnRoute.Attributes.Add("onclick", " return confirm('NOTE:This will route the PCR to the Approvers listed in the form.\\n Are you sure?');");

        }


        private void ExportToPDF(int id)
        {
            oVariables = new Variables(intEnvironment);
            Document doc = new Document();
            Cell cell;
            iTextSharp.text.Table oTable = new iTextSharp.text.Table(2);
            oTable.BorderWidth = 0;
            oTable.BorderColor = new iTextSharp.text.Color(255, 255, 255);
            oTable.Padding = 2;
            oTable.Width = 100;

            iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
            iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);

            DataSet ds;
            if (strRoute == "CSRC")
                ds = oTPM.GetCSRC(id);
            else
                ds = oTPM.GetPCR(id);

            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
            int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());

            DataSet dsResource = oCustomized.GetTPM(intRequest, intItem, intNumber);

            string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + ".pdf";
            string strPath = oVariables.UploadsFolder() + strFile;
            string strVirtualPath = oVariables.UploadsFolder() + strFile;

            FileStream fs = new FileStream(strPath, FileMode.Create);

            PdfWriter.GetInstance(doc, fs);
            //  PdfWriter.GetInstance(doc, Response.OutputStream);
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

            cell = new Cell(new Phrase("Project Change Request Report", oFontHeader));
            cell.Colspan = 2;
            cell.BackgroundColor = new iTextSharp.text.Color(169, 162, 141);
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Scope:", oFontBold)));
            oTable.AddCell(new Cell(new Phrase((ds.Tables[0].Rows[0]["scope"].ToString() == "1" ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Schedule:", oFontBold)));
            oTable.AddCell(new Cell(new Phrase((ds.Tables[0].Rows[0]["s"].ToString() == "1" ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Financial:", oFontBold)));
            oTable.AddCell(new Cell(new Phrase((ds.Tables[0].Rows[0]["f"].ToString() == "1" ? "Yes" : "No"), oFont)));

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

            if (ds.Tables[0].Rows[0]["sds"] == DBNull.Value || ds.Tables[0].Rows[0]["sde"] == DBNull.Value)
                oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable2.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["sds"].ToString()) + " - " + GetDate(ds.Tables[0].Rows[0]["sde"].ToString()), oFont)));

            oTable2.AddCell(new Cell(new Phrase("Planning", oFont)));
            if (dsResource.Tables[0].Rows[0]["appsp"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appep"] == DBNull.Value)
                oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable2.AddCell(new Cell(new Phrase(GetDate(dsResource.Tables[0].Rows[0]["appsp"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appep"].ToString()), oFont)));
            if (ds.Tables[0].Rows[0]["sps"] == DBNull.Value || ds.Tables[0].Rows[0]["spe"] == DBNull.Value)
                oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable2.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["sps"].ToString()) + " - " + GetDate(ds.Tables[0].Rows[0]["spe"].ToString()), oFont)));

            oTable2.AddCell(new Cell(new Phrase("Execution", oFont)));
            if (dsResource.Tables[0].Rows[0]["appse"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appee"] == DBNull.Value)
                oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable2.AddCell(new Cell(new Phrase(GetDate(dsResource.Tables[0].Rows[0]["appse"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appee"].ToString()), oFont)));

            if (ds.Tables[0].Rows[0]["ses"] == DBNull.Value || ds.Tables[0].Rows[0]["see"] == DBNull.Value)
                oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable2.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["ses"].ToString()) + " - " + GetDate(ds.Tables[0].Rows[0]["see"].ToString()), oFont)));

            oTable2.AddCell(new Cell(new Phrase("Closing", oFont)));
            if (dsResource.Tables[0].Rows[0]["appsc"] == DBNull.Value || dsResource.Tables[0].Rows[0]["appec"] == DBNull.Value)
                oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable2.AddCell(new Cell(new Phrase(GetDate(dsResource.Tables[0].Rows[0]["appsc"].ToString()) + " - " + GetDate(dsResource.Tables[0].Rows[0]["appec"].ToString()), oFont)));

            if (ds.Tables[0].Rows[0]["scs"] == DBNull.Value || ds.Tables[0].Rows[0]["sce"] == DBNull.Value)
                oTable2.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable2.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["scs"].ToString()) + " - " + GetDate(ds.Tables[0].Rows[0]["sce"].ToString()), oFont)));
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
            double dblAppD = dblAppDI + dblAppDE + dblAppDH;

            double dblFD = GetFloat(ds.Tables[0].Rows[0]["fd"].ToString());
            double dblFP = GetFloat(ds.Tables[0].Rows[0]["fp"].ToString());
            double dblFE = GetFloat(ds.Tables[0].Rows[0]["fe"].ToString());
            double dblFC = GetFloat(ds.Tables[0].Rows[0]["fc"].ToString());



            double dblAppPI = GetFloat(dsResource.Tables[0].Rows[0]["appip"].ToString());
            double dblAppPE = GetFloat(dsResource.Tables[0].Rows[0]["appexp"].ToString());
            double dblAppPH = GetFloat(dsResource.Tables[0].Rows[0]["apphp"].ToString());
            double dblAppP = dblAppPI + dblAppPE + dblAppPH;

            double dblAppEI = GetFloat(dsResource.Tables[0].Rows[0]["appie"].ToString());
            double dblAppEE = GetFloat(dsResource.Tables[0].Rows[0]["appexe"].ToString());
            double dblAppEH = GetFloat(dsResource.Tables[0].Rows[0]["apphe"].ToString());
            double dblAppE = dblAppEI + dblAppEE + dblAppEH;

            double dblAppCI = GetFloat(dsResource.Tables[0].Rows[0]["appic"].ToString());
            double dblAppCE = GetFloat(dsResource.Tables[0].Rows[0]["appexc"].ToString());
            double dblAppCH = GetFloat(dsResource.Tables[0].Rows[0]["apphc"].ToString());
            double dblAppC = dblAppCI + dblAppCE + dblAppCH;


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


            iTextSharp.text.Table oTable4 = new iTextSharp.text.Table(3);
            oTable4.BorderWidth = 0;
            oTable4.BorderColor = new iTextSharp.text.Color(255, 255, 255);
            oTable4.Padding = 2;
            oTable4.Width = 100;
            cell = new Cell(new Phrase("Reason for PCR ", oFontBold));
            cell.Colspan = 3;
            oTable4.AddCell(cell);

            string[] strReasons = ds.Tables[0].Rows[0]["reasons"].ToString().Split(';');
            foreach (string str in strReasons)
            {
                if (str != "")
                    oTable4.AddCell(new Cell(new Phrase(str, oFont)));
            }
            doc.Add(oTable4);
            doc.Close();
            fs.Close();

            string strURL = oVariables.UploadsFolder() + strFile;
            oTPM.UpdatePCRPath(id, oVariables.UploadsFolder() + strFile);
            strAttachement += "<tr><td><a href=\"" + strURL + "\" target=\"_blank\"><img src=\"/images/icons/pdf.gif \" align=\"absmiddle\" border=\"0\" /> View PCR Document (" + ds.Tables[0].Rows[0]["name"] + ")</a></td></tr> ";
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment; filename=closure_form.pdf");
            //Response.End();
            //Response.Flush();
        }

        private void ExportCSRCtoPDF(int id)
        {
            oVariables = new Variables(intEnvironment);
            Document doc = new Document();
            Cell cell;
            iTextSharp.text.Table oTable = new iTextSharp.text.Table(2);
            oTable.BorderWidth = 0;
            oTable.BorderColor = new iTextSharp.text.Color(255, 255, 255);
            oTable.Padding = 2;
            oTable.Width = 100;

            iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
            iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);


            ds = oTPM.GetCSRC(id);


            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
            int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());

            DataSet dsResource = oCustomized.GetTPM(intRequest, intItem, intNumber);

            string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + ".pdf";
            string strPath = oVariables.UploadsFolder() + strFile;
            string strVirtualPath = oVariables.UploadsFolder() + strFile;

            FileStream fs = new FileStream(strPath, FileMode.Create);

            PdfWriter.GetInstance(doc, fs);
            //  PdfWriter.GetInstance(doc, Response.OutputStream);
            string strHeader = "ClearView CSRC Information";
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

            cell = new Cell(new Phrase("Project Capital Service Review Committee Report", oFontHeader));
            cell.Colspan = 2;
            cell.BackgroundColor = new iTextSharp.text.Color(169, 162, 141);
            oTable.AddCell(cell);

            cell = new Cell(new Phrase("PMM Phase", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);
            oTable.AddCell(new Cell(new Phrase("Discovery", oFont)));
            oTable.AddCell(new Cell(new Phrase((ds.Tables[0].Rows[0]["d"].ToString() == "1" ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Planning", oFont)));
            oTable.AddCell(new Cell(new Phrase((ds.Tables[0].Rows[0]["p"].ToString() == "1" ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Execution", oFont)));
            oTable.AddCell(new Cell(new Phrase((ds.Tables[0].Rows[0]["e"].ToString() == "1" ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Closing", oFont)));
            oTable.AddCell(new Cell(new Phrase((ds.Tables[0].Rows[0]["c"].ToString() == "1" ? "Yes" : "No"), oFont)));


            cell = new Cell(new Phrase("Discovery", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (ds.Tables[0].Rows[0]["ds"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["ds"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (ds.Tables[0].Rows[0]["de"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["de"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (ds.Tables[0].Rows[0]["di"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["di"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (ds.Tables[0].Rows[0]["dex"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["dex"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (ds.Tables[0].Rows[0]["dh"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["dh"].ToString()).ToString("F"), oFont)));


            cell = new Cell(new Phrase("Planning", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (ds.Tables[0].Rows[0]["ps"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["ps"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (ds.Tables[0].Rows[0]["pe"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["pe"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (ds.Tables[0].Rows[0]["pi"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["pi"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (ds.Tables[0].Rows[0]["pex"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["pex"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (ds.Tables[0].Rows[0]["ph"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["ph"].ToString()).ToString("F"), oFont)));


            cell = new Cell(new Phrase("Execution", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (ds.Tables[0].Rows[0]["es"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["es"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (ds.Tables[0].Rows[0]["ee"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["ee"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (ds.Tables[0].Rows[0]["ei"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["ei"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (ds.Tables[0].Rows[0]["eex"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["eex"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (ds.Tables[0].Rows[0]["eh"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["eh"].ToString()).ToString("F"), oFont)));


            cell = new Cell(new Phrase("Closing", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (ds.Tables[0].Rows[0]["cs"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["cs"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (ds.Tables[0].Rows[0]["ce"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(ds.Tables[0].Rows[0]["ce"].ToString()), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (ds.Tables[0].Rows[0]["ci"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["ci"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (ds.Tables[0].Rows[0]["cex"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["cex"].ToString()).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (ds.Tables[0].Rows[0]["ch"] == DBNull.Value)
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(ds.Tables[0].Rows[0]["ch"].ToString()).ToString("F"), oFont)));



            doc.Add(oTable);
            doc.Close();
            fs.Close();

            string strURL = oVariables.UploadsFolder() + strFile;
            oTPM.UpdateCSRCPath(id, oVariables.UploadsFolder() + strFile);
            strAttachement += "<tr><td><a href=\"" + strURL + "\" target=\"_blank\"><img src=\"/images/icons/pdf.gif \" align=\"absmiddle\" border=\"0\" /> View PCR Document (" + ds.Tables[0].Rows[0]["name"] + ")</a></td></tr> ";
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment; filename=closure_form.pdf");
            //Response.End();
            //Response.Flush();
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
        protected void btnRoute_Click(object sender, EventArgs e)
        {
            if (strRoute == "CSRC")
            {
                foreach (string str in strChecks)
                {
                    if (str != "")
                    {
                        if (Request.Form[hdnC.UniqueID] != "")
                            intCC = Int32.Parse(Request.Form[hdnC.UniqueID]);
                        oTPM.UpdateCSRCCC(intId, intCC);
                        //intWorking = 39331;
                        //intExecutive = 39331;    
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
            else
            {

                foreach (string str in strChecks)
                {
                    if (str != "")
                    {
                        if (Request.Form[hdnC.UniqueID] != "")
                            intCC = Int32.Parse(Request.Form[hdnC.UniqueID]);
                        oTPM.UpdatePCRCC(intId, intCC);
                        //intWorking = 39331;
                        //intExecutive = 39331;
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
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);

        }

        private string GetRole(int intStep)
        {
            string strRole = "Additional";

            if (intStep == 1)
                strRole = "Working Sponsor";
            else if (intStep == 2)
                strRole = "Executive Sponsor";

            return strRole;
        }

        private string GetStatus(int intStatus)
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
