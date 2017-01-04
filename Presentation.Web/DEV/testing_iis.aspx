<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private void Page_Load()
    {
        //System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("psexec");
        //info.WorkingDirectory = @"C:\PSTools\";
        //info.Arguments = @"\\10.49.255.213 -u onevoice -p 4AdminW03 -e cmd /c (net use v: \\ohcleutl4107\svrapps T3st1ng /USER:corptest\slabadm /PERSISTENT:YES & V:\Apps\SrvrApp\IIS60Standard\install.vbs & net use v: /DELETE)";
        //System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
        //proc.WaitForExit();
        //proc.Close();

        //string psExec = "C:\\PSTools\\" + "psexec.exe";
        //string strArgs = "\\10.49.255.213 -u onevoice -p 4AdminW03 -e c:\temp\test.bat";
        //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(psExec, strArgs);
        //System.Diagnostics.Process proc = new System.Diagnostics.Process();
        //proc.StartInfo = startInfo;
        //proc.Start();
        //proc.Close();

        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("c:\\pstools\\psexec");
        psi.WorkingDirectory = @"C:\PSTools\";
        psi.Arguments = @"\\10.49.255.213 -u onevoice -p 4AdminW03 -e cmd /c (net use v: \\ohcleutl4107\svrapps T3st1ng /USER:corptest\slabadm /PERSISTENT:YES & V:\Apps\SrvrApp\IIS60Standard\install.vbs & net use v: /DELETE)";
        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;
        System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);
        System.IO.StreamReader sOut = proc.StandardOutput;
        proc.Close();
        string results = sOut.ReadToEnd().Trim();
        sOut.Close();
        string fmtStdOut = "<font face=courier size=0>{0}</font>";
        Response.Write(String.Format(fmtStdOut, results.Replace(System.Environment.NewLine, "<br>")));

        //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe");
        //psi.UseShellExecute = false;
        //psi.RedirectStandardOutput = true;
        //psi.RedirectStandardInput = true;
        //psi.RedirectStandardError = true;
        //psi.WorkingDirectory = @"C:\PSTools\";
        //System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);
        //System.IO.StreamReader sOut = proc.StandardOutput;
        //System.IO.StreamWriter sIn = proc.StandardInput;
        //sIn.WriteLine("psexec \\10.49.255.213 -u onevoice -p 4AdminW03 -e c:\temp\test.bat");
        //proc.Close();
        //string results = sOut.ReadToEnd().Trim();
        //sIn.Close();
        //sOut.Close();
        //string fmtStdOut = "<font face=courier size=0>{0}</font>";
        //Response.Write(String.Format(fmtStdOut, results.Replace(System.Environment.NewLine, "<br>")));
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
done!
</asp:Content>