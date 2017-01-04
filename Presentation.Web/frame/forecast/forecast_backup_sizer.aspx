<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="forecast_backup_sizer.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_backup_sizer" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table cellpadding="2" cellspacing="2">
    <tr>
        <td class="bold">Application / Project Name:</td>
        <td><asp:Label ID="lblAppName" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold">Project Number:</td>
        <td><asp:Label ID="lblPlanView" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold">Project Lead:</td>
        <td><asp:Label ID="lblLead" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold">Integration Engineer:</td>
        <td><asp:Label ID="lblEngineer" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold">Estimate completed By:</td>
        <td><asp:Label ID="lblEstimate" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold">Data Service Required:</td>
        <td><asp:Label ID="lblDataService" runat="server" CssClass="default" /></td>
    </tr>
</table>
<br />
<table cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td valign="top">
            <table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr class="header" bgcolor="#EEEEEE">
                    <td>Inputs</td>
                    <td>Default</td>
                    <td>Actual</td>
                </tr>
                <tr>
                    <td colspan="3" class="bold">TSM Client Information</td>
                </tr>
                <tr>
                    <td>Qty of base client CPUs</td>
                    <td><asp:Label ID="lblDefaultQBCC" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualQBCC" runat="server" CssClass="redbold" /></td>
                </tr>
                <tr>
                    <td>Qty of Exchage client CPUs</td>
                    <td><asp:Label ID="lblDefaultQECC" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualQECC" runat="server" CssClass="redbold" /></td>
                </tr>
                <tr>
                    <td colspan="3" class="bold">Client File System Data</td>
                </tr>
                <tr>
                    <td>Total file system data (GB)</td>
                    <td><asp:Label ID="lblDefaultTFSD" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualTFSD" runat="server" CssClass="redbold" /></td>
                </tr>
                <tr>
                    <td>Percent changed daily</td>
                    <td><asp:Label ID="lblDefaultPCD" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualPCD" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Compression Ratio</td>
                    <td><asp:Label ID="lblDefaultCR" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualCR" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Avg file size</td>
                    <td><asp:Label ID="lblDefaultAFS" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualAFS" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Backup version ratio</td>
                    <td><asp:Label ID="lblDefaultBVR" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualBVR" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Archive ratio</td>
                    <td><asp:Label ID="lblDefaultAR" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualAR" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Backup start time</td>
                    <td><asp:Label ID="lblDefaultBST" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualBST" runat="server" CssClass="redbold" /></td>
                </tr>
                <tr>
                    <td>Backup window (Hours)</td>
                    <td><asp:Label ID="lblDefaultBW" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualBW" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Backup sets</td>
                    <td><asp:Label ID="lblDefaultBS" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualBS" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="3" class="bold">Client Database Data</td>
                </tr>
                <tr>
                    <td>Total database data (GB)</td>
                    <td><asp:Label ID="lblDefaultDbData" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualDbData" runat="server" CssClass="redbold" /></td>
                </tr>
                <tr>
                    <td>Database type</td>
                    <td><asp:Label ID="lblDefaultDbType" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualDbType" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Percent changed daily</td>
                    <td><asp:Label ID="lblDefaultPctChg" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualPctChg" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Compression Ratio</td>
                    <td><asp:Label ID="lblDefaultCRDbData" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualCRDbData" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Number of backup versions</td>
                    <td><asp:Label ID="lblDefaultBackupVers" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualBackupVers" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Backup start time</td>
                    <td><asp:Label ID="lblDefaultBSTDbData" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualBSTDbData" runat="server" CssClass="redbold" /></td>
                </tr>
                <tr>
                    <td>Backup window (Hours)</td>
                    <td><asp:Label ID="lblDefaultBWDbData" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualBWDbData" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Network Connection</td>
                    <td><asp:Label ID="lblDefaultNwConn" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualNwConn" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Growth factor</td>
                    <td><asp:Label ID="lblDefaultGF" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualGF" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="3" class="bold">TSM Server Information</td>
                </tr>
                <tr>
                    <td>TSM server location</td>
                    <td><asp:Label ID="lblDefaultTSM" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualTSM" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Tape drive type</td>
                    <td><asp:Label ID="lblDefaultTDT" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualTDT" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>GB per tape cartridge</td>
                    <td><asp:Label ID="lblDefaultGB" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualGB" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Migration throughput (GB/HR)</td>
                    <td><asp:Label ID="lblDefaultMT" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualMT" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Max file size for disk pool (GB)</td>
                    <td><asp:Label ID="lblDefaultMFS" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualMFS" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Reclamation threshold</td>
                    <td><asp:Label ID="lblDefaultRT" runat="server" CssClass="default" /></td>
                    <td><asp:Label ID="lblActualRT" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr class="header" bgcolor="#EEEEEE">
                    <td colspan="2">Calculations</td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">Client</td>
                </tr>
                <tr>
                    <td>Total client files</td>
                    <td><asp:Label ID="lblTotalClientFiles" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Total TSM server GB for file system data</td>
                    <td><asp:Label ID="lblTotalTSMSrvrGB" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Total database GB on TSM server</td>
                    <td><asp:Label ID="lblTotalDbGB" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">TSM Server</td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">&nbsp;&nbsp;Disk</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;TSM Server Database (MB)</td>
                    <td><asp:Label ID="lblTSMServerDB" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;File System disk pool (GB)</td>
                    <td><asp:Label ID="lblFileSystemDiskPool" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;Database disk pool (GB)</td>
                    <td><asp:Label ID="lblDbDiskPool" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;Total Disk (GB)</td>
                    <td><asp:Label ID="lblTotalDisk" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">&nbsp;&nbsp;Tape</td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">&nbsp;&nbsp;&nbsp;&nbsp;Onsite cartridges</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Onsite file system tapes</td>
                    <td><asp:Label ID="lblOnsiteFileTapes" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Onsite database tapes</td>
                    <td><asp:Label ID="lblOnsiteDbTapes" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">&nbsp;&nbsp;&nbsp;&nbsp;Offsite cartridges</td>
                </tr>
                <tr>
                    <td>Offsite file system tapes</td>
                    <td><asp:Label ID="lblOffsiteFileTapes" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Offsite database tapes</td>
                    <td><asp:Label ID="lblOffsiteDbTapes" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Backupset tapes</td>
                    <td><asp:Label ID="lblBackupTapes" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td class="bold">Library Slots</td>
                    <td><asp:Label ID="lblLibrarySlots" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">Tape drives</td>
                </tr>
                <tr>
                    <td>Database backup data (drive hours)</td>
                    <td><asp:Label ID="lblDbBak" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Storage pool migration (drive hours)</td>
                    <td><asp:Label ID="lblStoragePool" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td colspan="2" class="bold">Network</td>
                </tr>
                <tr>
                    <td>Nightly backup network traffic (GB)</td>
                    <td><asp:Label ID="lblNightlyBackup" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>Full restore network data traffic (GB)</td>
                    <td><asp:Label ID="lblFullRestore" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
<table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
    <tr class="header" bgcolor="#EEEEEE">
        <td colspan="5">Pricing</td>
    </tr>
    <tr class="bold">
        <td>Description</td>
        <td>Qty</td>
        <td>Override</td>
        <td>Unit Cost</td>
        <td>Extended Price</td>
    </tr>
    <tr>
        <td class="bold" colspan="5">Hardware</td>
    </tr>
    <tr>
        <td>Magstar tape drives</td>
        <td><asp:Label ID="lblMagstarQty" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblORMagstar" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblUCMagstar" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblEPMagstar" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Tape Library Slots</td>
        <td><asp:Label ID="lblTLSQty" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblORTLS" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblUCTLS" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblEPTLS" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Fibre Ports</td>
        <td><asp:Label ID="lblFPQty" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblORFP" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblUCFP" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblEPFP" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold" colspan="5">Software</td>
    </tr>
    <tr>
        <td>TSM base client CPU licenses</td>
        <td><asp:Label ID="lblBaseClientQty" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblORBaseClient" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblUCBaseClient" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblEPBaseClient" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>TSM Exchange client CPU licenses</td>
        <td><asp:Label ID="lblExchangeClientQty" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblORExchangeClient" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblUCExchangeClient" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblEPExchangeClient" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold" colspan="5">Supplies</td>
    </tr>
    <tr>
        <td>Tape cartridges</td>
        <td><asp:Label ID="lblTCQty" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblORTC" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblUCTC" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblEPTC" runat="server" CssClass="default" /></td>
    </tr>
    <tr class="bold">
        <td colspan="4">Total One-Time Purchase</td>
        <td><asp:Label ID="lblEPTOP" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td class="bold" colspan="5">Monthly Lease Expense</td>
    </tr>
    <tr>
        <td>Disk GB</td>
        <td><asp:Label ID="lblDiskQty" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblORDisk" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblUCDisk" runat="server" CssClass="default" /></td>
        <td><asp:Label ID="lblEPDisk" runat="server" CssClass="default" /></td>
    </tr>
    <tr class="bold">
        <td colspan="4">Total Monthly Lease Expense</td>
        <td><asp:Label ID="lblEPTMLE" runat="server" CssClass="default" /></td>
    </tr>
</table>
<br />
<table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
    <tr class="header" bgcolor="#EEEEEE">
        <td colspan="6">Data flow diagram (All data in GB)</td>
    </tr>
    <tr>
        <td nowrap>TSM Client</td>
        <td></td>
        <td nowrap>TSM Server</td>
        <td></td>
        <td nowrap>Disk Pool</td>
        <td nowrap>Tape Pool</td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td rowspan="3"><img src="/images/client.gif" border="0" align="absmiddle" /></td>
                    <td nowrap>Full Backup</td>
                </tr>
                <tr>
                    <td nowrap>Incremental BU</td>
                </tr>
                <tr>
                    <td nowrap>Database data</td>
                </tr>
            </table>
            <br />
        </td>
        <td>
            <table>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblFB" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblIBU" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lbldbdata" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
        <td><img src="/images/server.gif" border="0" align="absmiddle" /></td>
        <td>
            <table>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblServerFB" runat="server" CssClass="default" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblServerIBU" runat="server" CssClass="default" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                </tr>
                <tr>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblServerDD" runat="server" CssClass="default" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                </tr>
            </table>
        </td>
        <td align="center">
            <table>
                <tr>
                    <td><img src="/images/storage.gif" border="0" align="absmiddle" /></td>
                    <td><img src="/images/arrow1.gif" border="0" align="absmiddle" /></td>
                    <td><asp:Label ID="lblDiskFB" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
        <td><img src="/images/tape.gif" border="0" align="absmiddle" /></td>
    </tr>
</table>
<br />
<table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
    <tr class="header" bgcolor="#EEEEEE">
        <td colspan="2">Performance</td>
    </tr>
    <tr>
        <td class="bold" colspan="2">File System Data</td>
    </tr>
    <tr>
        <td>Estimated backup throughput (GB/HR)</td>
        <td><asp:Label ID="lblEBTFSD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Estimated restore throughput (GB/HR)</td>
        <td><asp:Label ID="lblERTFSD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Estimated full backup duration (Hours)</td>
        <td><asp:Label ID="lblEFBFSD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Estimated full restore duration (Hours)</td>
        <td><asp:Label ID="lblEFRFSD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Estimated incr. backup duration (Hours)</td>
        <td><asp:Label ID="lblEIBFSD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold" colspan="2">Database Data</td>
    </tr>
    <tr>
        <td>Estimated backup throughput (GB/HR)</td>
        <td><asp:Label ID="lblEBTDD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Estimated restore throughput (GB/HR)</td>
        <td><asp:Label ID="lblERTDD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Estimated full backup duration (Hours)</td>
        <td><asp:Label ID="lblEFBDD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td>Estimated full restore duration (Hours)</td>
        <td><asp:Label ID="lblEFRDD" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td class="bold">Estimated migration duration (Hours)</td>
        <td><asp:Label ID="lblEMD" runat="server" CssClass="default" /></td>
    </tr>
</table>
<br />
<table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
    <tr class="header" bgcolor="#EEEEEE">
        <td>Assumptions</td>
    </tr>
    <tr>
        <td>All file system data is backed up to disk pool on TSM Server.</td>
    </tr>
    <tr>
        <td>Sunguard costs not included.</td>
    </tr>
    <tr>
        <td>EPS Support costs not included.</td>
    </tr>
    <tr>
        <td>Prices not valid until after EPS reviews and provides overrides.</td>
    </tr>
    <tr>
        <td>Model does not size content manager workloads.</td>
    </tr>
    <tr>
        <td>All warnings below must be resolved.</td>
    </tr>
    <tr>
        <td>DB2 performance metrics based on AIX TSM client.</td>
    </tr>
</table>
</asp:Content>
