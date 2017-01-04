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
    public partial class service_editor_display : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected ServiceEditor oServiceEditor;
        protected Users oUser;
        private int intService = 0;
        private int intConfig = 0;
        private int intWM = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oUser = new Users(intProfile, dsn);
            Int32.TryParse(Request.QueryString["wm"], out intWM);
            Int32.TryParse(Request.QueryString["serviceid"], out intService);
            Int32.TryParse(Request.QueryString["configid"], out intConfig);
            if (Request.QueryString["saved"] != null)
                trSaved.Visible = true;
            if (!IsPostBack)
            {
                if (intService > 0 && intConfig > 0)
                {
                    DataSet ds = oServiceEditor.GetConfigs(intService, intWM, 1);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int _configid = Int32.Parse(dr["id"].ToString());
                        if (intConfig == _configid)
                            break;
                        TreeNode oNode = new TreeNode();
                        oNode.Text = dr["question"].ToString();
                        oNode.ToolTip = dr["question"].ToString();
                        oNode.Value = dr["id"].ToString();
                        oNode.SelectAction = TreeNodeSelectAction.None;
                        oNode.Checked = false;
                        oTree.Nodes.Add(oNode);
                        if (dr["code"].ToString() == "DROPDOWN" || dr["code"].ToString() == "RADIOLIST")
                        {
                            DataSet dsValues = oServiceEditor.GetConfigValues(_configid);
                            foreach (DataRow drValue in dsValues.Tables[0].Rows)
                            {
                                TreeNode oChild = new TreeNode();
                                oChild.Text = drValue["value"].ToString();
                                oChild.ToolTip = drValue["value"].ToString();
                                oChild.Value = drValue["id"].ToString();
                                oChild.SelectAction = TreeNodeSelectAction.None;
                                DataSet dsResponse = oServiceEditor.GetConfigAffects(intConfig, Int32.Parse(drValue["id"].ToString()));
                                oChild.Checked = (dsResponse.Tables[0].Rows.Count > 0);
                                oNode.ChildNodes.Add(oChild);
                            }
                        }
                        else
                        {
                            TreeNode oChild = new TreeNode();
                            oChild.Text = "<b>Unavailable.</b> Only &quot;Drop Down List&quot; and &quot;Radio Button List&quot; controls can be used for dynamic display";
                            oChild.Value = "0";
                            oChild.SelectAction = TreeNodeSelectAction.None;
                            oChild.ShowCheckBox = false;
                            oNode.ChildNodes.Add(oChild);
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oServiceEditor.DeleteConfigAffects(intConfig);
            foreach (TreeNode oNode in oTree.Nodes)
            {
                foreach (TreeNode oChild in oNode.ChildNodes)
                {
                    if (oChild.Checked == true)
                        oServiceEditor.AddConfigAffect(intConfig, Int32.Parse(oChild.Value));
                }
            }
            Response.Redirect(Request.Path + "?wm=" + intWM.ToString() + "&serviceid=" + intService.ToString() + "&configid=" + intConfig.ToString() + "&saved=true");
        }
    }
}
