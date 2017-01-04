<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false"%>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsnMnemonics = ConfigurationManager.ConnectionStrings["mnemonicDSN"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        VMWare oVMWare = new VMWare(0, dsn);
        string strName = "VDIXPE30003";
        string strCluster = "CLEVMVDT04";
        string strConnect = "";
        if (Request.QueryString["t"] != null)
        {
            strName = "healytesting";
            strCluster = "CLEVDTLAB01";
            strConnect = oVMWare.ConnectDEBUG("https://vwsvt102/sdk", 999, "PNC-LabRoomF");
        }
        else
            strConnect = oVMWare.ConnectDEBUG("https://wcsvt300a/sdk", 999, "PNC-Cleveland");
        if (strConnect == "")
        {
            VimService _service = oVMWare.GetService();
            ServiceContent _sic = oVMWare.GetSic();
            ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
            ManagedObjectReference vmFolderRef = oVMWare.GetVMFolder(datacenterRef);
            ManagedObjectReference clusterRef = oVMWare.GetCluster(strCluster);
            ManagedObjectReference resourcePoolRootRef = (ManagedObjectReference)oVMWare.getObjectProperty(clusterRef, "resourcePool");
            ManagedObjectReference oComputer = oVMWare.GetVM(strName);
            VirtualMachineConfigSpec oConfig = new VirtualMachineConfigSpec();
            VirtualMachineDeviceRuntimeInfo oInfo = (VirtualMachineDeviceRuntimeInfo)oVMWare.getObjectProperty(oComputer, "device");
            VirtualMachineDeviceRuntimeInfoVirtualEthernetCardRuntimeState oState = (VirtualMachineDeviceRuntimeInfoVirtualEthernetCardRuntimeState)oInfo.runtimeState;
            //Response.Write(
            //oState.vmDirectPathGen2InactiveReasonOther = "";
            Response.Write("Done");
            
            // Logout
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
        }
        else
            Response.Write("LOGIN error");

        /*
        DataTable dt = new DataTable();
        using (TextReader tr = File.OpenText(@"\\wxp8yzw5f1\Reports\Clearview\CliSched.TXT"))
        {
            string line;
            int counter = 0;
            while ((line = tr.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                if (dt.Columns.Count == 0)
                {
                    // Create the data columns for the data table based on the number of items
                    // on the first line of the file
                    for (int ii = 0; ii < items.Length; ii++)
                        dt.Columns.Add(new DataColumn("Column" + ii, typeof(string)));
                }
                dt.Rows.Add(items);
                counter++;
                if (counter > 30)
                    break;
            }
        }

        // Print out all the values
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<p>");
            foreach (string s in dr.ItemArray)
                Response.Write(s + "<br/>");
            Response.Write("</p>");
        }
        */

        /*
        Variables oVariable = new Variables(intEnvironment);
        string strTable = oVariable.MnemonicsImportTable();
        DataSet dsCorrect = SqlHelper.ExecuteDataset(dsnMnemonics, CommandType.Text, "SELECT * FROM " + strTable);
        foreach (DataColumn drC in dsCorrect.Tables[0].Columns)
            Response.Write(drC.ColumnName + "<br/>");
        
        /*
        Models oModel = new Models(0, dsn);
        Solaris oSolaris = new Solaris(0, dsn);
        string strMAC = "";
        int _boot_groupid = 4;
        string strUsername = oModel.GetBootGroup(_boot_groupid, "username");
        string strPassword = oModel.GetBootGroup(_boot_groupid, "password");
        string strExpects = oModel.GetBootGroup(_boot_groupid, "regular");
        SshShell oSSHshell = new SshShell("10.33.239.200", strUsername, strPassword);
        oSSHshell.RemoveTerminalEmulationCharacters = true;
        oSSHshell.Connect();
        if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
        {
            // Wait for "sc>"
            string strBanner = oSSHshell.Expect(strExpects);
            // Send Command : showsc sys_enetaddr
            oSSHshell.WriteLine("show /HOST macaddress");
            // Wait for "sc>"
            strMAC = oSSHshell.Expect(strExpects);
            strMAC = oSolaris.ParseOutput(strMAC, "macaddress = ", Environment.NewLine);
        }
        oSSHshell.Close();
        Response.Write(strMAC);
        */
        
        //Asset oAsset = new Asset(0, dsnAsset, dsn);
        
        //int intAsset = oAsset.GetServerOrBladeAvailable(7, 1, 715, 395, 14055, dsn, "", true);
        
        //double dblOverallSize = 842.00;
        //double dblDividend = dblOverallSize / 500.00;
        //dblDividend = Math.Floor(dblDividend);
        //if (dblOverallSize > 750.00 && dblDividend == 1.00)
        //    dblDividend = 2.00;
        //Response.Write(dblDividend.ToString("0") + "<br/>");
        //double dblLUN = (dblOverallSize / dblDividend);
        //Response.Write(dblLUN.ToString() + "<br/>");
        //dblLUN = Math.Ceiling(dblLUN);
        //while (dblLUN % 5.00 != 0.00)
        //{
        //    Response.Write(dblLUN.ToString() + "<br/>");
        //    Response.Write(dblLUN % 5.00 + "<br/>");
        //    dblLUN += 1.00;
        //}
        //dblOverallSize = (dblLUN * dblDividend);
        //Response.Write(dblLUN.ToString() + " x " + dblDividend.ToString() + " = " + dblOverallSize.ToString() + "<br/>");
        
        /*
        DirectoryEntry oEntry = new DirectoryEntry("LDAP://OHCLEWDC1001.corp.ntl-city.net/DC=corp,DC=ntl-city,DC=net", "corpdmn\\SEPS13R", "05EPSiisAD316");
        DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
        oSearcher.Filter = "(samaccountname=XSXH3*)";
        SearchResultCollection oResults = oSearcher.FindAll();
        Response.Write(oResults.Count.ToString());
        */
        
        /*
        AD oAD = new AD(0, dsn, intEnvironment);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_users where xid = pnc_id OR xid like '%-PNC%' OR pnc_id like '%-PNC%'");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strXID = dr["xid"].ToString();
            Response.Write("Searching for &quot;" + strXID + "&quot;...");
            SearchResultCollection oResults = oAD.Search(strXID, "cn");
            if (oResults.Count == 0)
                Response.Write("not found" + "<br/>");
            else if (oResults.Count > 1)
                Response.Write("multiple " + oResults.Count.ToString() + " records found" + "<br/>");
            else
            {
                if (oResults[0].Properties.Contains("extensionattribute10") == true)
                    Response.Write("PNC ID = " + oResults[0].GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString() + "<br/>");
            }
        }
        */
    }
</script>
<html>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
</form>
</body>
</html>