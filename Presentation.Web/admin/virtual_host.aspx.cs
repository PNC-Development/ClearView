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
using Microsoft.ApplicationBlocks.Data;
namespace NCC.ClearView.Presentation.Web
{
    public partial class virtual_host : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intHostTest = Int32.Parse(ConfigurationManager.AppSettings["VHOST_TEST"]);
        protected int intHostProd = Int32.Parse(ConfigurationManager.AppSettings["VHOST_PROD"]);
        protected int intProfile;
        protected string strResults = "";
        protected int intCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/virtual_host.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            btnParent.Attributes.Add("onclick", "return OpenWindow('SUBMODELBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            int intAnswer = 0;
            if (radEnvironment.SelectedItem.Value == "TEST")
                intAnswer = intHostTest;
            if (radEnvironment.SelectedItem.Value == "PROD")
                intAnswer = intHostProd;
            if (intAnswer > 0)
            {
                string strName = txtName.Text.ToUpper();
                string strName1 = strName.Substring(0, 5);
                strName = strName.Substring(5);
                string strName2 = strName.Substring(0, 3);
                strName = strName.Substring(3);
                string strName3 = strName.Substring(0, 2);
                strName = strName.Substring(2);
                string strName4 = strName.Substring(0, 1);
                strName = strName.Substring(1);
                int intName = 0;
                DataSet dsNames = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_servernames WHERE prefix1 = '" + strName1 + "' AND prefix2 = '" + strName2 + "' AND sitecode = '" + strName3 + "' AND name1 = '" + strName4 + "' AND name2 = '" + strName + "' AND deleted = 0");
                if (dsNames.Tables[0].Rows.Count > 0)
                    intName = Int32.Parse(dsNames.Tables[0].Rows[0]["id"].ToString());
                else
                {
                    ServerName oServerName = new ServerName(0, dsn);
                    intName = oServerName.Add(0, strName1, strName2, strName3, strName4, strName, 0, txtSerial.Text, 0);
                }
                Forecast oForecast = new Forecast(0, dsn);
                int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                int intAddress = Int32.Parse(oForecast.GetAnswer(intAnswer, "addressid"));
                int intNumber = (int)SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT MAX(number) FROM cv_servers WHERE answerid = " + intAnswer.ToString());
                intNumber = intNumber + 1;
                DataSet dsServers = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_servers WHERE answerid = " + intAnswer.ToString());
                DataRow drServer = dsServers.Tables[0].Rows[0];
                int intOldServer = Int32.Parse(drServer["id"].ToString());
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_servers VALUES(" + drServer["requestid"] + "," + drServer["answerid"] + "," + drServer["modelid"] + "," + drServer["csmconfigid"] + "," + drServer["clusterid"] + "," + intNumber.ToString() + "," + drServer["osid"] + "," + drServer["spid"] + "," + drServer["templateid"] + "," + drServer["domainid"] + "," + drServer["test_domainid"] + "," + drServer["infrastructure"] + "," + drServer["dr"] + "," + drServer["dr_exist"] + ",'" + drServer["dr_name"] + "'," + drServer["dr_consistency"] + "," + drServer["dr_consistencyid"] + "," + drServer["configured"] + "," + drServer["local_storage"] + "," + drServer["accounts"] + "," + drServer["fdrive"] + ",0,0," + intName.ToString() + ",'" + drServer["dhcp"] + "',0,999,0,0,null,null,null,0,getdate(),getdate(),0)");
                int intServer = (int)SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT TOP 1 id FROM cv_servers WHERE answerid = " + intAnswer.ToString() + " ORDER BY created DESC");
                Asset oAsset = new Asset(0, dsnAsset);
                int intAsset = oAsset.Add("", Int32.Parse(Request.Form[hdnParent.UniqueID]), txtSerial.Text.ToUpper(), txtAsset.Text.ToUpper(), (int)AssetStatus.Arrived, intProfile, DateTime.Now, 0, 1);
                oAsset.AddServer(intAsset, (int)AssetStatus.Available, intProfile, DateTime.Now, intClass, intEnv, intAddress, GetRoom(txtRoom.Text), GetRack(txtRack.Text), txtRackPos.Text, txtILO.Text, txtSerial.Text.ToUpper(), txtMAC.Text, Int32.Parse(txtVlan.Text), 0);
                oAsset.AddStatus(intAsset, txtName.Text.ToUpper(), (int)AssetStatus.InUse, intProfile, DateTime.Now);
                Servers oServer = new Servers(0, dsn);
                oServer.AddAsset(intServer, intAsset, intClass, intEnv, 0, 0);
                lblDone.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView added the host";
            }
            else
                lblDone.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Select a Type!";
        }
        protected int GetRack(string _name)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_racks WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                Racks oRack = new Racks(0, dsn);
                oRack.Add(_name, 1);
                return GetRack(_name);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        protected int GetRoom(string _name)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_rooms WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                Rooms oRoom = new Rooms(0, dsn);
                oRoom.Add(_name, 1);
                return GetRoom(_name);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
    
    }
}
