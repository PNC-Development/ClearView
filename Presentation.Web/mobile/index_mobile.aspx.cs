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

namespace NCC.ClearView.Presentation.Web
{
    public partial class index_mobile : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected Asset oAsset;
        protected Orders oOrder;
        protected Variables oVariable;
        protected Functions oFunction;
        protected void Page_Load(object sender, EventArgs e)
        {
            oAsset = new Asset(0, dsnAsset);
            oOrder = new Orders(0, dsnAsset);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
        }
        protected void btnUpload_Click(Object Sender, EventArgs e)
        {
            int intSuccess = 0;
            int intDuplicate = 0;
            int intError = 0;
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            if (oFile.PostedFile != null && oFile.FileName != "")
            {
                string strPhysical = oVariable.DocumentsFolder() + "xml\\";
                if (Directory.Exists(strPhysical) == false)
                {
                    lblResults.Text = "<b>Created XML directory &quot;" + strPhysical + "&quot;...SUCCESS!</b><br/><br/>";
                    Directory.CreateDirectory(strPhysical);
                }
                else
                    lblResults.Text = "<b>XML directory &quot;" + strPhysical + "&quot; already exists...</b><br/><br/>";
                lblResults.Text += "<b>Results of File &quot;" + oFile.FileName + "&quot;</b><br/>";
                string strFile = strPhysical + oFile.FileName;
                oFile.PostedFile.SaveAs(strFile);
                DataSet ds = new DataSet();
                ds.ReadXml(strFile);
                switch (oFile.FileName[0])
                {
                    case 'E':
                        // Existing
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intModel = 0;
                            try
                            {
                                intModel = Int32.Parse(dr["svrModel"].ToString());
                            }
                            catch
                            {
                                intModel = oModelsProperties.Get(dr["svrModel"].ToString());
                            }
                            if (intModel > 0)
                            {
                                if (oAsset.Get(dr["svrSerial"].ToString().Trim().ToUpper(), intModel).Tables[0].Rows.Count > 0)
                                {
                                    lblResults.Text += "DUPLICATE: " + dr["svrSerial"].ToString().Trim().ToUpper() + " [" + dr["svrAsset"].ToString().Trim().ToUpper() + "]<br/>";
                                    intDuplicate++;
                                }
                                else
                                {
                                    int intAsset = oAsset.Add("", intModel, dr["svrSerial"].ToString().Trim().ToUpper(), dr["svrAsset"].ToString().Trim().ToUpper(), (int)AssetStatus.Arrived, -1, DateTime.Now, 0, 1);
                                    lblResults.Text += "success: " + dr["svrSerial"].ToString().Trim().ToUpper() + " [" + dr["svrAsset"].ToString().Trim().ToUpper() + "]<br/>";
                                    intSuccess++;
                                }
                            }
                            else
                            {
                                ErrorMessage(dr["svrModel"].ToString());
                                lblResults.Text += "### ERROR ###: " + dr["svrModel"].ToString().Trim().ToUpper() + " does not exist<br/>";
                                intError++;
                            }
                        }
                        lblResults.Text += "<p><hr size=1 noshade/></p>";
                        lblResults.Text += "Successful: " + intSuccess.ToString() + "<br/>";
                        lblResults.Text += "Duplicates: " + intDuplicate.ToString() + "<br/>";
                        lblResults.Text += "Errors: " + intError.ToString() + "<br/>";
                        break;

                    case 'S':
                        // New
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intModel = 0;
                            try
                            {
                                intModel = Int32.Parse(dr["svrModel"].ToString());
                            }
                            catch
                            {
                                intModel = oModelsProperties.Get(dr["svrModel"].ToString());
                            }
                            if (intModel > 0)
                            {
                                if (oAsset.Get(dr["svrSerial"].ToString().Trim().ToUpper(), intModel).Tables[0].Rows.Count > 0)
                                {
                                    lblResults.Text += "DUPLICATE: " + dr["svrSerial"].ToString().Trim().ToUpper() + " [" + dr["svrAsset"].ToString().Trim().ToUpper() + "]<br/>";
                                    intDuplicate++;
                                }
                                else
                                {
                                    int intAsset = oAsset.Add(dr["svrTracking"].ToString().Trim().ToUpper(), intModel, dr["svrSerial"].ToString().Trim().ToUpper(), dr["svrAsset"].ToString().Trim().ToUpper(), (int)AssetStatus.Arrived, -1, DateTime.Now, 0, 1);
                                    oOrder.UpdateReceived(dr["svrTracking"].ToString().Trim().ToUpper(), 1);
                                    lblResults.Text += "success: " + dr["svrSerial"].ToString().Trim().ToUpper() + " [" + dr["svrAsset"].ToString().Trim().ToUpper() + "]<br/>";
                                    intSuccess++;
                                }
                            }
                            else
                            {
                                ErrorMessage(dr["svrModel"].ToString());
                                lblResults.Text += "### ERROR ###: " + dr["svrModel"].ToString().Trim().ToUpper() + " does not exist<br/>";
                                intError++;
                            }
                        }
                        lblResults.Text += "<p><hr size=1 noshade/></p>";
                        lblResults.Text += "Successful: " + intSuccess.ToString() + "<br/>";
                        lblResults.Text += "Duplicates: " + intDuplicate.ToString() + "<br/>";
                        lblResults.Text += "Errors: " + intError.ToString() + "<br/>";
                        break;

                    default:
                        lblResults.Text = "Invalid XML File";
                        break;
                }
            }
        }
        protected int GetParent(DataSet dsParent, int intParent)
        {
            int intReturn = 0;
            foreach (DataRow drParent in dsParent.Tables[0].Rows)
            {
                if (Int32.Parse(drParent["svrIndex"].ToString()) == intParent)
                {
                    DataSet dsSerial = oAsset.Get(drParent["svrSerial"].ToString());
                    if (dsSerial.Tables[0].Rows.Count > 0)
                    {
                        intReturn = Int32.Parse(dsSerial.Tables[0].Rows[0]["id"].ToString());
                        break;
                    }
                }
            }
            return intReturn;
        }
        private void ErrorMessage(string _model)
        {
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
            oFunction.SendEmail("ClearView Action Required", strEMailIdsBCC, "", "", "ClearView Action Required", "<p><b>When importing models from the mobile application, there was a problem importing the following model...</b></p><p>" + _model + "</p>", false, false);
        }
    }
}
