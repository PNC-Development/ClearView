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
    public partial class cluster : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected bool boolUseCSM = (ConfigurationManager.AppSettings["USE_CSM_EXECUTION"] == "1");
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Cluster oCluster;
        protected Requests oRequest;
        protected ModelsProperties oModelsProperties;
        protected Classes oClass;
        protected int intAnswer = 0;
        protected int intCluster = 0;
        protected int intRequest = 0;
        protected bool boolMidrange = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Cluster Configuration";
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intCluster = Int32.Parse(Request.QueryString["id"]);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "refresh", "<script type=\"text/javascript\">RefreshOpeningWindow();<" + "/" + "script>");
            int intServer = 0;
            int intDR = 0;
            int intHA = 0;
            if (intAnswer > 0)
            {
                Page.Title = "ClearView Cluster Configuration | Design # " + intAnswer.ToString();
                DataSet ds = oForecast.GetAnswer(intAnswer);
                int intModel = oForecast.GetModel(intAnswer);
                int intType = oModelsProperties.GetType(intModel);
                if (oForecast.IsOSMidrange(intAnswer) == true)
                    boolMidrange = true;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intServer = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString()) - oForecast.TotalServerCount(intAnswer, boolUseCSM);
                    intDR = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString()) - oForecast.TotalDRCount(intAnswer, boolUseCSM);
                    intHA = Int32.Parse(ds.Tables[0].Rows[0]["ha"].ToString()) - oForecast.TotalHACount(intAnswer, boolUseCSM);
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    intRequest = oForecast.GetRequestID(intAnswer, true);
                    if (!IsPostBack)
                    {
                        panView.Visible = true;
                        btnQuorum.Visible = (boolMidrange == false);
                        if (oClass.Get(intClass, "prod") != "1" || oForecast.GetAnswerPlatform(intAnswer, intUnder48Q, intUnder48A) == false)
                        {
                            txtDR.Text = "0";
                            txtDR.Enabled = false;
                        }
                        if (oClass.Get(intClass, "prod") != "1" || oForecast.IsHARoom(intAnswer) == false)
                        {
                            txtHA.Text = "0";
                            txtHA.Enabled = false;
                        }
                        ds = oCluster.Get(intCluster);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Page.Title = "ClearView Cluster Configuration | Design # " + intAnswer.ToString() + " | Cluster # " + intCluster.ToString();
                            imgSave.Visible = false;
                            txtName.Text = ds.Tables[0].Rows[0]["nickname"].ToString();
                            txtNodes.Text = ds.Tables[0].Rows[0]["nodes"].ToString();
                            intServer += Int32.Parse(txtNodes.Text);
                            txtDR.Text = ds.Tables[0].Rows[0]["dr"].ToString();
                            intDR += Int32.Parse(txtDR.Text);
                            txtHA.Text = ds.Tables[0].Rows[0]["ha"].ToString();
                            intHA += Int32.Parse(txtHA.Text);
                            if (ds.Tables[0].Rows[0]["local_nodes"].ToString() == "1")
                            {
                                if (ds.Tables[0].Rows[0]["non_shared"].ToString() == "0")
                                    imgNodes.Visible = true;
                                btnNodes.Attributes.Add("onclick", "return OpenWindow('ONDEMAND_SERVER','?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=0&num=0');");
                            }
                            else
                                btnNodes.Enabled = false;
                            if (ds.Tables[0].Rows[0]["non_shared"].ToString() == "1")
                            {
                                if (ds.Tables[0].Rows[0]["add_instance"].ToString() == "0")
                                    imgStorage.Visible = true;
                                btnStorage.Attributes.Add("onclick", "return OpenWindow('ONDEMAND_STORAGE','?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=0&num=0');");
                            }
                            else
                                btnStorage.Enabled = false;
                            if (ds.Tables[0].Rows[0]["add_instance"].ToString() == "1")
                            {
                                if (ds.Tables[0].Rows[0]["quorum"].ToString() == "0")
                                    imgAdd.Visible = true;
                                btnAdd.Attributes.Add("onclick", "return NewInstance('" + intAnswer.ToString() + "','" + intCluster.ToString() + "','');");
                            }
                            else
                                btnAdd.Enabled = false;
                            if (boolMidrange == false && ds.Tables[0].Rows[0]["quorum"].ToString() == "1")
                            {
                                imgQuorum.Visible = true;
                                btnQuorum.Attributes.Add("onclick", "return OpenWindow('ONDEMAND_CLUSTER_QUORUM','?aid=" + intAnswer + "&id=" + intCluster + "');");
                            }
                            else
                                btnQuorum.Enabled = false;

                            rptInstances.DataSource = oCluster.GetInstances(intCluster);
                            rptInstances.DataBind();
                            foreach (RepeaterItem ri in rptInstances.Items)
                                ((LinkButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                            lblNone.Visible = (rptInstances.Items.Count == 0);
                            //btnSave.Enabled = false;
                        }
                        else
                        {
                            imgSave.Visible = true;
                            btnAdd.Enabled = false;
                            btnQuorum.Enabled = false;
                            btnNodes.Enabled = false;
                            btnStorage.Enabled = false;
                            panNote.Visible = true;
                            lblNone.Visible = true;
                        }
                    }
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            btnDenied.Attributes.Add("onclick", "return window.close();");
            btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a custom name for this cluster')" +
                " && ValidateNumber0('" + txtNodes.ClientID + "','Please enter a valid number for the number of nodes')" +
                " && ValidateNumberLess('" + txtNodes.ClientID + "'," + intServer + ",'You cannot add any more than " + intServer + " nodes')" +
                " && ValidateNumber('" + txtDR.ClientID + "','Please enter a valid number for the number of DR servers')" +
                " && ValidateNumberLess('" + txtDR.ClientID + "'," + intDR + ",'You cannot add any more than " + intDR + " DR servers')" +
                " && ValidateNumber('" + txtHA.ClientID + "','Please enter a valid number for the number of HA servers')" +
                " && ValidateNumberLess('" + txtHA.ClientID + "'," + intHA + ",'You cannot add any more than " + intHA + " HA servers')" +
                ";");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            if (intCluster == 0)
                intCluster = oCluster.Add(intRequest, txtName.Text, Int32.Parse(txtNodes.Text), Int32.Parse(txtDR.Text), Int32.Parse(txtHA.Text));
            else
                oCluster.Update(intCluster, txtName.Text, Int32.Parse(txtNodes.Text), Int32.Parse(txtDR.Text), Int32.Parse(txtHA.Text));
            oCluster.UpdateLocalNodes(intCluster, 1);
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&id=" + intCluster);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">RefreshOpeningWindow();window.navigate('" + Request.Path + "?aid=" + intAnswer + "&id=" + intCluster + "');<" + "/" + "script>");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oCluster.DeleteInstance(Int32.Parse(oButton.CommandArgument), intCluster, intAnswer);
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&id=" + intCluster + "&delete=true");
        }
    }
}
