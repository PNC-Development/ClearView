<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected int intCount = 0;
    private VMWare oVMWare;
    private void Page_Load()
    {
        DateTime datStart = DateTime.Now;
        oVMWare = new VMWare(0, dsn);
        string strConnect = oVMWare.ConnectDEBUG("https://ohcleutl100a/sdk", 4, "Cleveland Operations");
        if (strConnect == "")
        {
            VimService _service = oVMWare.GetService();
            ServiceContent _sic = oVMWare.GetSic();
            string strName = "WCMHS200W";
            ManagedObjectReference vmRef = GetVM(strName);
            DateTime datEnd = DateTime.Now;
            TimeSpan oSpan = datEnd.Subtract(datStart);
            if (_service != null)
            {
                _service.Abort();
                if (_service.Container != null)
                    _service.Container.Dispose();
                try
                {
                    _service.Logout(_sic.sessionManager);
                }
                catch { }
                _service.Dispose();
                _service = null;
                _sic = null;
            }
            if (vmRef != null && vmRef.Value != "")
                Response.Write("Found = " + oSpan.Seconds.ToString() + " seconds... Value = " + vmRef.Value);
        }
        else
            Response.Write("LOGIN error: " + strConnect);
    }
    public ManagedObjectReference GetVM(string _name)
    {
        ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
        ManagedObjectReference vmFolderRef = (ManagedObjectReference)oVMWare.getObjectProperty(datacenterRef, "vmFolder");
        ManagedObjectReference[] vmList = (ManagedObjectReference[])oVMWare.getObjectProperty(vmFolderRef, "childEntity");
        ManagedObjectReference vmRef = new ManagedObjectReference();
        bool boolQuick = true;
        string strVIM = "";
        DataSet ds = oVMWare.GetGuest(_name);
        if (ds.Tables[0].Rows.Count > 0)
            strVIM = ds.Tables[0].Rows[0]["VIM"].ToString();
        if (strVIM == "")
            boolQuick = false;

        try
        {
            Response.Write("Starting!...there are " + vmList.Length.ToString() + " VMs...<br/>");
            bool boolQuickFound = false;
            if (boolQuick == true)
            {
                Response.Write("boolQuick == true...<br/>");
                for (int ii = 0; ii < vmList.Length; ii++)
                {
                    Response.Write("...Checking " + vmList[ii].Value + " = " + strVIM + "...<br/>");
                    if (vmList[ii].type == "VirtualMachine" && vmList[ii].Value == strVIM)
                    {
                        Response.Write("FOUND! - Checking " + strVIM + "...<br/>");
                        Object[] vmProps = oVMWare.getProperties(vmList[ii], new String[] { "name", "config.name", "config.template" });
                        if (((String)vmProps[0]).ToUpper() == _name.ToUpper())
                        {
                            Response.Write("OK!<br/>");
                            boolQuickFound = true;
                            vmRef = vmList[ii];
                            break;
                        }
                        else
                        {
                            Response.Write("Bad - looping through all...<br/>");
                            boolQuick = false;
                            break;
                        }
                    }
                }
                Response.Write("DONE with boolQuick == true...<br/>");
            }

            if (boolQuick == false || boolQuickFound == false)
            {
                Response.Write("boolQuick == false...<br/>");
                for (int ii = 0; ii < vmList.Length; ii++)
                {
                    Response.Write("ObjectID = " + vmList[ii].Value + "...");
                    if (vmList[ii].type == "VirtualMachine")
                    {
                        Object[] vmProps = oVMWare.getProperties(vmList[ii], new String[] { "name", "config.name", "config.template" });
                        Response.Write("...Checking " + ((String)vmProps[0]).ToUpper() + " = " + _name.ToUpper() + "...");
                        if (((String)vmProps[0]).ToUpper() == _name.ToUpper())
                        {
                            Response.Write("YES!<br/>");
                            vmRef = vmList[ii];
                            //oVMWare.UpdateGuestVIM(_name, vmRef.Value);
                            break;
                        }
                        else
                            Response.Write("no<br/>");
                    }
                    else
                    {
                        Object[] vmProps = oVMWare.getProperties(vmList[ii], new String[] { "name" });
                        Response.Write("FOLDER?? " + ((String)vmProps[0]).ToUpper() + " = TYPE: " + vmList[ii].type + "...<br/>");
                        ManagedObjectReference[] vmList2 = (ManagedObjectReference[])oVMWare.getObjectProperty(vmList[ii], "childEntity");
                for (int jj = 0; jj < vmList2.Length; jj++)
                {
                    Response.Write("ObjectID (FOLDER) = " + vmList2[jj].Value + "...");
                    if (vmList2[jj].type == "VirtualMachine")
                    {
                        Object[] vmProps2 = oVMWare.getProperties(vmList2[jj], new String[] { "name", "config.name", "config.template" });
                        Response.Write("...Checking " + ((String)vmProps2[0]).ToUpper() + " = " + _name.ToUpper() + "...");
                        if (((String)vmProps2[0]).ToUpper() == _name.ToUpper())
                        {
                            Response.Write("YES!<br/>");
                            vmRef = vmList2[jj];
                            //oVMWare.UpdateGuestVIM(_name, vmRef.Value);
                            break;
                        }
                        else
                            Response.Write("no<br/>");
                    }
                }
                    }
                }
                Response.Write("DONE with boolQuick == false...<br/>");
            }
        }
        catch(Exception exVM) {}
        return vmRef;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
done!
</asp:Content>