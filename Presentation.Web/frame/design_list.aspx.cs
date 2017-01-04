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

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_list : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Design oDesign;
        protected Users oUser;
        protected ModelsProperties oModelsProperties;
        protected Forecast oForecast;
        protected Models oModel;
        protected int intID;
        private string strType = "";
        protected bool boolWindows = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDesign = new Design(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);

            if (Request.QueryString["id"] != null)
                Int32.TryParse(Request.QueryString["id"], out intID);
            if (Request.QueryString["type"] != null)
                strType = Request.QueryString["type"];

            if (!IsPostBack)
            {
                if (intID > 0)
                {
                    StringBuilder strLiteral = new StringBuilder();
                    int intModel = oDesign.GetModelProperty(intID);
                    int intQuantity = 0;
                    Int32.TryParse(oDesign.Get(intID, "quantity"), out intQuantity);

                    switch (strType)
                    {
                        case "HELP":
                            Page.Title = "ClearView Help";
                            panHelp.Visible = true;
                            litHelpHeader.Text = oDesign.GetPhase(intID, "title");
                            litHelp.Text = oDesign.GetPhase(intID, "help");
                            break;
                        case "ACCOUNTS":
                            Page.Title = "ClearView Account Configuration";
                            panAccounts.Visible = true;
                            rptAccounts.DataSource = oDesign.GetAccounts(intID);
                            rptAccounts.DataBind();
                            foreach (RepeaterItem ri in rptAccounts.Items)
                            {
                                Label _permissions = (Label)ri.FindControl("lblPermissions");
                                switch (_permissions.Text)
                                {
                                    case "0":
                                        _permissions.Text = "-----";
                                        break;
                                    case "D":
                                        _permissions.Text = "Developer";
                                        break;
                                    case "P":
                                        _permissions.Text = "Promoter";
                                        break;
                                    case "S":
                                        _permissions.Text = "AppSupport";
                                        break;
                                    case "U":
                                        _permissions.Text = "AppUsers";
                                        break;
                                }
                                if (_permissions.ToolTip == "1")
                                    _permissions.Text += " (R/D)";
                            }
                            if (rptAccounts.Items.Count == 0)
                                lblNone.Visible = true;
                            break;
                        case "STORAGE_LUNS":
                            Page.Title = "ClearView Storage Configuration";
                            panStorage.Visible = true;
                            rptStorage.DataSource = oDesign.GetStorageDrives(intID);
                            rptStorage.DataBind();
                            foreach (RepeaterItem ri in rptStorage.Items)
                            {
                                CheckBox _shared = (CheckBox)ri.FindControl("chkStorageSize");
                                _shared.Checked = (_shared.Text == "1");
                                _shared.Text = "";
                            }
                            boolWindows = oDesign.IsWindows(intID);
                            if (boolWindows)
                            {
                                trStorageApp.Visible = true;
                                DataSet dsApp = oDesign.GetStorageDrive(intID, -1000);
                                if (dsApp.Tables[0].Rows.Count > 0)
                                {
                                    int intTemp = 0;
                                    if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intTemp) == true)
                                        txtStorageSizeE.Text = intTemp.ToString();
                                }
                            }
                            break;
                        case "BACKUP_EXCLUSION":
                            Page.Title = "ClearView Backup Exclusion Configuration";
                            panExclusions.Visible = true;
                            rptExclusions.DataSource = oDesign.GetExclusions(intID);
                            rptExclusions.DataBind();
                            if (rptExclusions.Items.Count == 0)
                                lblExclusion.Visible = true;
                            break;
                        case "QUANTITY":
                            Page.Title = "ClearView Server Count Configuration";
                            panQuantity.Visible = true;
                            lblQuantity.Text = intQuantity.ToString();
                            if (lblQuantity.Text == "")
                                lblQuantity.Text = "0";
                            if (oDesign.IsProd(intID))
                            {
                                if (oDesign.Get(intID, "dr") == "0")
                                    lblQuantityDR.Text = "0";
                                else
                                {
                                    lblQuantityDR.Text = lblQuantity.Text;
                                    trQuantityDR.Visible = true;
                                }
                            }
                            else
                                lblQuantityDR.Text = "0";
                            int intTotal = Int32.Parse(lblQuantity.Text) + Int32.Parse(lblQuantityDR.Text);
                            lblQuantityTotal.Text = intTotal.ToString();
                            break;
                        case "STORAGE_AMOUNT":
                            Page.Title = "ClearView Storage Amount Configuration";
                            panStorageAmount.Visible = true;
                            double dblReplicate = 0.00;
                            double.TryParse(oModelsProperties.Get(intModel, "replicate_times"), out dblReplicate);
                            double dblAmount = 0.00;
                            double dblReplicated = 0.00;
                            double dblTemp = 0.00;
                            DataSet dsStorage = oDesign.GetStorageDrives(intID);
                            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                            {
                                if (double.TryParse(drStorage["size"].ToString(), out dblTemp) == true)
                                {
                                    dblAmount += dblTemp;
                                    dblReplicated += (dblTemp * dblReplicate);
                                }
                            }
                            DataSet dsApp2 = oDesign.GetStorageDrive(intID, -1000);
                            if (dsApp2.Tables[0].Rows.Count > 0 && oDesign.IsWindows(intID))
                            {
                                if (double.TryParse(dsApp2.Tables[0].Rows[0]["size"].ToString(), out dblTemp) == true)
                                {
                                    dblAmount += dblTemp;
                                    dblReplicated += (dblTemp * dblReplicate);
                                }
                            }
                            dblAmount = dblAmount * intQuantity;
                            lblStorage.Text = dblAmount.ToString("0");
                            dblReplicated = dblReplicated * intQuantity;
                            if (oDesign.IsProd(intID))
                            {
                                if (oDesign.Get(intID, "dr") == "0")
                                    dblReplicated = 0.00;
                                else
                                {
                                    trStorageDR.Visible = true;
                                    lblStorageDR.Text = dblReplicated.ToString("0");
                                }
                            }
                            else
                                dblReplicated = 0.00;
                            double dblTotal = dblAmount + dblReplicated;
                            lblStorageTotal.Text = dblTotal.ToString("0");
                            break;
                        case "ACQUISITION":
                            panLiteral.Visible = true;
                            double dblQuantity = 0.00;
                            double dblRecovery = 0.00;
                            double.TryParse(oDesign.Get(intID, "quantity"), out dblQuantity);
                            if (oDesign.IsProd(intID))
                            {
                                if (oDesign.Get(intID, "dr") != "0")
                                    dblRecovery = dblQuantity;
                            }
                            else
                                lblQuantityDR.Text = "0";
                            if (intModel > 0)
                                intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            strLiteral.Append("<tr>");
                            strLiteral.Append("<td><b>Model:</b></td>");
                            strLiteral.Append("<td align=\"right\">");
                            strLiteral.Append(oModel.Get(intModel, "name"));
                            strLiteral.Append("</td>");
                            strLiteral.Append("</tr>");
                            double dblAcquisition = 0.00;
                            DataSet dsAcquisition = oForecast.GetAcquisitions(intModel, 1);
                            foreach (DataRow dr in dsAcquisition.Tables[0].Rows)
                            {
                                strLiteral.Append("<tr>");
                                strLiteral.Append("<td><b>");
                                strLiteral.Append(dr["name"].ToString());
                                strLiteral.Append(":</b></td>");
                                double dblCost = double.Parse(dr["cost"].ToString());
                                dblAcquisition += dblCost;
                                strLiteral.Append("<td align=\"right\">$");
                                strLiteral.Append(dblCost.ToString("N"));
                                strLiteral.Append("</td>");
                                strLiteral.Append("</tr>");
                            }
                            strLiteral.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            strLiteral.Append("<tr><td>Total Amount:</td><td class=\"reddefault\" align=\"right\">$");
                            strLiteral.Append(dblAcquisition.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td colspan=\"2\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"1\"/></td></tr>");
                            strLiteral.Append("<tr><td>Quantity:</td><td align=\"right\">");
                            strLiteral.Append(dblQuantity.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td>Recovery:</td><td align=\"right\">+ ");
                            strLiteral.Append(dblRecovery.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            dblQuantity = dblQuantity + dblRecovery;
                            strLiteral.Append("<tr><td>Total Quantity:</td><td align=\"right\">");
                            strLiteral.Append(dblQuantity.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td>Total Amount:</td><td class=\"reddefault\" align=\"right\">x $");
                            strLiteral.Append(dblAcquisition.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            dblAcquisition = dblAcquisition * dblQuantity;
                            strLiteral.Append("<tr><td><b>Grand Total:</b></td><td class=\"reddefault\" align=\"right\"><b>$");
                            strLiteral.Append(dblAcquisition.ToString("N"));
                            strLiteral.Append("</b></td></tr>");
                            strLiteral.Insert(0, "<table width=\"350\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                            strLiteral.Append("</table>");
                            litLiteral.Text = strLiteral.ToString();
                            break;
                        case "OPERATIONAL":
                            panLiteral.Visible = true;
                            double dblQuantityO = 0.00;
                            double dblRecoveryO = 0.00;
                            double.TryParse(oDesign.Get(intID, "quantity"), out dblQuantityO);
                            if (oDesign.IsProd(intID))
                            {
                                if (oDesign.Get(intID, "dr") != "0")
                                    dblRecoveryO = dblQuantityO;
                            }
                            else
                                lblQuantityDR.Text = "0";
                            if (intModel > 0)
                                intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            strLiteral.Append("<tr>");
                            strLiteral.Append("<td><b>Model:</b></td>");
                            strLiteral.Append("<td align=\"right\">");
                            strLiteral.Append(oModel.Get(intModel, "name"));
                            strLiteral.Append("</td>");
                            strLiteral.Append("</tr>");
                            double dblOperational = 0.00;
                            DataSet dsOperational = oForecast.GetOperations(intModel, 1);
                            foreach (DataRow dr in dsOperational.Tables[0].Rows)
                            {
                                strLiteral.Append("<tr>");
                                strLiteral.Append("<td><b>");
                                strLiteral.Append(dr["name"].ToString());
                                strLiteral.Append(":</b></td>");
                                double dblCost = double.Parse(dr["cost"].ToString());
                                dblOperational += dblCost;
                                strLiteral.Append("<td align=\"right\">$");
                                strLiteral.Append(dblCost.ToString("N"));
                                strLiteral.Append("</td>");
                                strLiteral.Append("</tr>");
                            }
                            strLiteral.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            strLiteral.Append("<tr><td>Total Amount:</td><td class=\"reddefault\" align=\"right\">$");
                            strLiteral.Append(dblOperational.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td colspan=\"2\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"1\"/></td></tr>");
                            strLiteral.Append("<tr><td>Quantity:</td><td align=\"right\">");
                            strLiteral.Append(dblQuantityO.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td>Recovery:</td><td align=\"right\">+ ");
                            strLiteral.Append(dblRecoveryO.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            dblQuantityO = dblQuantityO + dblRecoveryO;
                            strLiteral.Append("<tr><td>Total Quantity:</td><td align=\"right\">");
                            strLiteral.Append(dblQuantityO.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td>Total Amount:</td><td class=\"reddefault\" align=\"right\">x $");
                            strLiteral.Append(dblOperational.ToString("N"));
                            strLiteral.Append("</td></tr>");
                            strLiteral.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            dblOperational = dblOperational * dblQuantityO;
                            strLiteral.Append("<tr><td><b>Grand Total:</b></td><td class=\"reddefault\" align=\"right\"><b>$");
                            strLiteral.Append(dblOperational.ToString("N"));
                            strLiteral.Append("</b></td></tr>");
                            strLiteral.Insert(0, "<table width=\"350\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                            strLiteral.Append("</table>");
                            litLiteral.Text = strLiteral.ToString();
                            break;
                    }
                }
            }
        }
    }
}
