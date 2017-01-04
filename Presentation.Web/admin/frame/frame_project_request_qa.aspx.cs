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
    public partial class frame_project_request_qa : BasePage
    {

    private DataSet ds;
    private DataSet dsQA;
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected int intProfile;
    protected  int intQuestion;
    protected int intOrganizationID;
    protected Organizations oOrganization;    
    
    // Vijay Code - Start
    protected ProjectRequest oProjectRequest;   
    // Vijay Code - End
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Reload();        
        oOrganization = new Organizations(intProfile, dsn);       

        // Vijay Code - Start
        oProjectRequest = new ProjectRequest(intProfile, dsn);
     
        // Vijay Code - End
        if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
            lblType.Text = Request.QueryString["type"];
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            lblId.Text = Request.QueryString["id"];
        if (Request.QueryString["questionid"] != null && Request.QueryString["questionid"] != "")
            intQuestion = Int32.Parse(Request.QueryString["questionid"]);      
        string strControl = "";
        if (Request.QueryString["control"] != null)
            strControl = Request.QueryString["control"];
        //btnSave.Attributes.Add("onclick", "return Update('hdnUpdateOrder','" + strControl + "');");

       // btnSave.Attributes.Add("onclick", "OnTreeClick(event);");
        btnClose.Attributes.Add("onclick", "return HidePanel();");
        if (!IsPostBack)
        {
            ds = oOrganization.Gets(1);
            Load();
        }
       
    }    
    
    private void Load()
    {
        TreeNode oNode;
        oNode = new TreeNode();
        oNode.Text = "Base";
        oNode.ToolTip = "Base";        
        oNode.SelectAction = TreeNodeSelectAction.Expand;                
        oTreeview.Nodes.Add(oNode);   
                     
        Load(oNode);
        oNode.Expand();
        oNode = new TreeNode();
        oNode.Text = "Discretionary";
        oNode.ToolTip = "Discretionary";         
        oNode.SelectAction = TreeNodeSelectAction.Expand;
    
        oTreeview.Nodes.Add(oNode);
        Load(oNode);
        oNode.Expand();
    }
    private void Load(TreeNode oParent)
    {
        dsQA = oProjectRequest.GetQAByQuestion(intQuestion);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();                              
            oNode.Value = dr["organizationid"].ToString();
            oNode.SelectAction = TreeNodeSelectAction.None;
            oNode.Checked = false;
             
            foreach (DataRow drQA in dsQA.Tables[0].Rows)
            {
                if (drQA["organizationid"].ToString() == oNode.Value.ToString() && drQA["bd"].ToString() == oParent.Text)
                {
                    oNode.Checked = true;
                                             
                }
                 
            }                     
            oParent.ChildNodes.Add(oNode);
             
        }
        oTreeview.ExpandDepth = 1;
        oTreeview.Attributes.Add("oncontextmenu", "return false;");
    }
    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        oProjectRequest.DeleteQA(intQuestion);
        foreach (TreeNode oNode in oTreeview.Nodes)
        {   
               SaveQA(oNode);            
            
        }
        Reload();
    }

    private void SaveQA(TreeNode oParent)
    { 
        foreach (TreeNode oNode in oParent.ChildNodes)
        {
            if (oNode.Checked == true)
            {
                int _organization_id = Int32.Parse(oNode.Value);
                oProjectRequest.AddQA(oParent.Text, _organization_id, intQuestion);
            }
        }
    }
    private void Reload()
    {
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
    }
    }
}
