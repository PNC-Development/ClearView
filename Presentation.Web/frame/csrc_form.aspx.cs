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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NCC.ClearView.Presentation.Web
{
    public partial class csrc_form : BasePage
    {
        protected Users oUser;
        protected Pages oPage;
        protected Variables oVariable;
        protected Projects oProject;
        protected TPM oTPM;
        protected Organizations oOrganization;
        protected Customized oCustomized;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intCSRCPage = Int32.Parse(ConfigurationManager.AppSettings["CSRC_WORKFLOW"]);

        protected int intProfile;
        protected int intWorking;
        protected int intExecutive;
        protected int intRequest;
        protected int intProject;

        protected int intLead;

        protected int intId;
        private DataSet ds;
        protected string strStatus = "Pending";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);

            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oTPM = new TPM(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oProject = new Projects(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);

            if (Request.QueryString["pid"] != "" && Request.QueryString["pid"] != null)
                intProject = Int32.Parse(Request.QueryString["pid"]);

            if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                intId = Int32.Parse(Request.QueryString["id"]);

            if (Request.QueryString["work"] != "" && Request.QueryString["work"] != null)
                intWorking = Int32.Parse(Request.QueryString["work"]);

            if (Request.QueryString["exec"] != "" && Request.QueryString["exec"] != null)
                intExecutive = Int32.Parse(Request.QueryString["exec"]);

            if (!IsPostBack)
            {
                ds = oTPM.GetCSRC(intId);

                chkDiscovery.Checked = ds.Tables[0].Rows[0]["d"].ToString() == "1" ? true : false;
                divDiscovery.Style["display"] = chkDiscovery.Checked ? "inline" : "none";
                chkPlanning.Checked = ds.Tables[0].Rows[0]["p"].ToString() == "1" ? true : false;
                divPlanning.Style["display"] = chkPlanning.Checked ? "inline" : "none";
                chkExecution.Checked = ds.Tables[0].Rows[0]["e"].ToString() == "1" ? true : false;
                divExecution.Style["display"] = chkExecution.Checked ? "inline" : "none";
                chkClosing.Checked = ds.Tables[0].Rows[0]["c"].ToString() == "1" ? true : false;
                divClosing.Style["display"] = chkClosing.Checked ? "inline" : "none";

                //Discovery
                txtCSRCSD.Text = ds.Tables[0].Rows[0]["ds"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["ds"].ToString()).ToShortDateString() : "";
                txtCSRCED.Text = ds.Tables[0].Rows[0]["de"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["de"].ToString()).ToShortDateString() : "";
                txtCSRCID.Text = ds.Tables[0].Rows[0]["di"].ToString();
                txtCSRCExD.Text = ds.Tables[0].Rows[0]["dex"].ToString();
                txtCSRCHD.Text = ds.Tables[0].Rows[0]["dh"].ToString();

                //Planning
                txtCSRCSP.Text = ds.Tables[0].Rows[0]["ps"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["ps"].ToString()).ToShortDateString() : "";
                txtCSRCEP.Text = ds.Tables[0].Rows[0]["pe"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["pe"].ToString()).ToShortDateString() : "";
                txtCSRCIP.Text = ds.Tables[0].Rows[0]["pi"].ToString();
                txtCSRCExP.Text = ds.Tables[0].Rows[0]["pex"].ToString();
                txtCSRCHP.Text = ds.Tables[0].Rows[0]["ph"].ToString();

                // Execution
                txtCSRCSE.Text = ds.Tables[0].Rows[0]["es"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["es"].ToString()).ToShortDateString() : "";
                txtCSRCEE.Text = ds.Tables[0].Rows[0]["ee"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["ee"].ToString()).ToShortDateString() : "";
                txtCSRCIE.Text = ds.Tables[0].Rows[0]["ei"].ToString();
                txtCSRCExE.Text = ds.Tables[0].Rows[0]["eex"].ToString();
                txtCSRCHE.Text = ds.Tables[0].Rows[0]["eh"].ToString();

                //Closing
                txtCSRCSC.Text = ds.Tables[0].Rows[0]["cs"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["cs"].ToString()).ToShortDateString() : "";
                txtCSRCEC.Text = ds.Tables[0].Rows[0]["ce"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["ce"].ToString()).ToShortDateString() : "";
                txtCSRCIC.Text = ds.Tables[0].Rows[0]["ci"].ToString();
                txtCSRCExC.Text = ds.Tables[0].Rows[0]["cex"].ToString();
                txtCSRCHC.Text = ds.Tables[0].Rows[0]["ch"].ToString();


                if (ds.Tables[0].Rows[0]["status"].ToString() == "1")
                    strStatus = "Approved";
                if (ds.Tables[0].Rows[0]["status"].ToString() == "-1")
                    strStatus = "Denied";

                hdnStatus.Value = strStatus;

            }

            chkDiscovery.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divDiscovery.ClientID + "',this);");
            chkPlanning.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divPlanning.ClientID + "',this);");
            chkExecution.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divExecution.ClientID + "',this);");
            chkClosing.Attributes.Add("onclick", "CheckCSRC('" + chkDiscovery.ClientID + "','" + chkPlanning.ClientID + "','" + chkExecution.ClientID + "','" + chkClosing.ClientID + "','" + divCSRC.ClientID + "');ShowHideDivCheck('" + divClosing.ClientID + "',this);");

            if (strStatus != "Pending")
                btnUpdate.Attributes.Add("onclick", "return confirm('This CSRC has already been " + strStatus.ToUpper() + "!! By updating this CSRC will initiate the re-routal process.\\nAre you sure ?');");

        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            UpdatePDF(oTPM.GetCSRCPath(intId));
            if (Request.Form[hdnStatus.UniqueID] != "Pending")
            {
                oTPM.UpdateCSRC(intId, 0, 0);
                oTPM.UpdateCSRCDetail(intId, 1);
                oTPM.UpdateCSRCDetail(intId, 2);
                oTPM.ApproveCSRC(intId, 1, 0, intCSRCPage, intWorkloadManager,  oVariable.DocumentsFolder() + oTPM.GetCSRCPath(intId));
            }
            else
                oTPM.UpdateCSRC(intId, (chkDiscovery.Checked ? 1 : 0), (chkPlanning.Checked ? 1 : 0), (chkExecution.Checked ? 1 : 0), (chkClosing.Checked ? 1 : 0), txtCSRCSD.Text, txtCSRCED.Text, txtCSRCID.Text, txtCSRCExD.Text, txtCSRCHD.Text, txtCSRCSP.Text, txtCSRCEP.Text, txtCSRCIP.Text, txtCSRCExP.Text, txtCSRCHP.Text, txtCSRCSE.Text, txtCSRCEE.Text, txtCSRCIE.Text, txtCSRCExE.Text, txtCSRCHE.Text, txtCSRCSC.Text, txtCSRCEC.Text, txtCSRCIC.Text, txtCSRCExC.Text, txtCSRCHC.Text);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
        }

        private void UpdatePDF(string strPath)
        {

            Document doc = new Document();
            Cell cell;
            string strPhysicalPath = oVariable.DocumentsFolder() + strPath;
            FileStream fs = new FileStream(strPhysicalPath, FileMode.Create);
            PdfWriter.GetInstance(doc, fs);

            iTextSharp.text.Table oTable = new iTextSharp.text.Table(2);
            oTable.BorderWidth = 0;
            oTable.BorderColor = new iTextSharp.text.Color(255, 255, 255);
            oTable.Padding = 2;
            oTable.Width = 100;

            iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
            iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);



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

            ds = oTPM.GetCSRC(intId);

            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
            int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());

            DataSet dsResource = oCustomized.GetTPM(intRequest, intItem, intNumber);

            cell = new Cell(new Phrase("Project Capital Service Review Committee Report", oFontHeader));
            cell.Colspan = 2;
            cell.BackgroundColor = new iTextSharp.text.Color(169, 162, 141);
            oTable.AddCell(cell);

            cell = new Cell(new Phrase("PMM Phase", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);
            oTable.AddCell(new Cell(new Phrase("Discovery", oFont)));
            oTable.AddCell(new Cell(new Phrase((chkDiscovery.Checked ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Planning", oFont)));
            oTable.AddCell(new Cell(new Phrase((chkPlanning.Checked ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Execution", oFont)));
            oTable.AddCell(new Cell(new Phrase((chkExecution.Checked ? "Yes" : "No"), oFont)));
            oTable.AddCell(new Cell(new Phrase("Closing", oFont)));
            oTable.AddCell(new Cell(new Phrase((chkClosing.Checked ? "Yes" : "No"), oFont)));


            cell = new Cell(new Phrase("Discovery", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (txtCSRCSD.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSD.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (txtCSRCED.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCED.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (txtCSRCID.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCID.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (txtCSRCExD.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExD.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (txtCSRCHD.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHD.Text).ToString("F"), oFont)));


            cell = new Cell(new Phrase("Planning", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (txtCSRCSP.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSP.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (txtCSRCEP.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCEP.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (txtCSRCIP.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCIP.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (txtCSRCExP.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExP.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (txtCSRCHP.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHP.Text).ToString("F"), oFont)));


            cell = new Cell(new Phrase("Execution", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (txtCSRCSE.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSE.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (txtCSRCEE.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCEE.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (txtCSRCIE.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCIE.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (txtCSRCExE.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExE.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (txtCSRCHE.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHE.Text).ToString("F"), oFont)));


            cell = new Cell(new Phrase("Closing", oFontBold));
            cell.Colspan = 2;
            oTable.AddCell(cell);

            oTable.AddCell(new Cell(new Phrase("Phase start date", oFont)));
            if (txtCSRCSC.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCSC.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Phase end date", oFont)));
            if (txtCSRCEC.Text == "")
                oTable.AddCell(new Cell(new Phrase("N / A", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase(GetDate(txtCSRCEC.Text), oFont)));

            oTable.AddCell(new Cell(new Phrase("Internal Labor", oFont)));
            if (txtCSRCIC.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCIC.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("External Labor", oFont)));
            if (txtCSRCExC.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCExC.Text).ToString("F"), oFont)));


            oTable.AddCell(new Cell(new Phrase("HW/SW/One Time Cost", oFont)));
            if (txtCSRCHC.Text == "")
                oTable.AddCell(new Cell(new Phrase("$0.0", oFont)));
            else
                oTable.AddCell(new Cell(new Phrase("$" + GetFloat(txtCSRCHC.Text).ToString("F"), oFont)));


            doc.Add(oTable);
            doc.Close();
            fs.Close();
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
