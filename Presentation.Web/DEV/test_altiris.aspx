<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        string strJob = "netZEUS for Windows Boot Option";
        string strName = "HEALYTEST";
        string strSerial = "HEALYSERIAL";
        string strAsset = "HEALYASSET";
        string strMACAddressStrip = "123456789012";
        string strRDPScheduleWebService = "http://OHCINUTL1009/dsjob/dsjob_fromurl.asmx";
        string strRDPComputerWebService = "http://OHCINUTL1009/Altiris.ASDK.DS/ComputerManagementService.asmx";
        //string strRDPScheduleWebService = "http://OHCLEUTL100M/dsjob/dsjob_fromurl.asmx";
        //string strRDPComputerWebService = "http://OHCLEUTL100M/Altiris.ASDK.DS/ComputerManagementService.asmx";

        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());

        NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService oComputer = new NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService();
        oComputer.Credentials = oCredentials;
        oComputer.Url = strRDPComputerWebService;

        if (Request.QueryString["delete"] != null)
        {
            int intDeleteComputer = oComputer.GetComputerID(strName, 1);
            if (intDeleteComputer > 0)
            {
                bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                Response.Write("Found Duplicate Computer Object....Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebService + "...result = " + boolDelete.ToString() + "<br/>");
            }
            else
                Response.Write("Did Not Find Computer Object on " + strRDPComputerWebService + "<br/>");
        }
        else
        {
            // Delete Computer Object if it Exists
            int intDeleteComputer = oComputer.GetComputerID(strName, 1);
            if (intDeleteComputer > 0)
            {
                bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                Response.Write("Found Duplicate Computer Object....Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebService + "...result = " + boolDelete.ToString() + "<br/>");
            }
            else
                Response.Write("Did Not Find Computer Object on " + strRDPComputerWebService + "<br/>");
            // Add Computer Object
            Response.Write("Configuring Altiris (MAC: " + strMACAddressStrip + ") on " + strRDPScheduleWebService + "<br/>");
            int intComputer = oComputer.AddBasicVirtualComputer(-1, strName, strAsset, strSerial, strMACAddressStrip, 2, "");
            // Assign Schedule
            NCC.ClearView.Application.Core.altirisws.dsjob oJob = new NCC.ClearView.Application.Core.altirisws.dsjob();
            oJob.Credentials = oCredentials;
            oJob.Url = strRDPScheduleWebService;
            Response.Write("Adding Altiris Job (" + strJob + ")<br/>");
            oJob.ScheduleNow(strName, strJob);
        }
        Response.Write("Done!<br/>");
    }
    
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
</asp:Content>