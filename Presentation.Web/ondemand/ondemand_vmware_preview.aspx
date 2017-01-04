<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ondemand_vmware_preview.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ondemand_vmware_preview" %>

<html>
<head>
<title>Auto-Provisioning</title>
<script type="text/javascript">
    function connect(strHost, strPath, strTicket, strPort)
    {
        if (strHost != null) {
            var oObject = document.getElementById("mks");
            var oConnection = oObject.Connect(strHost, parseInt(strPort), strPath, strTicket, strTicket);
            
        }
    }
    function shut() { 
        var oObject = document.getElementById("mks");
        oObject.disconnect(); 
    }
</script>
</head>
<body leftmargin="0" topmargin="0" onunload="shut();">
<form id="Form1" runat="server">
  <script id=win for="mks" event="OnWindowStateChange(windowState)"> 
     if (windowState == 3) {
        if (confirm("You are about to enter fullscreen mode." + 
                    " Press Ctl+Alt to return to windowed mode.")) {
          mks.setFullScreen(true);
        }
     }
  </script>
  <script id=grab for="mks" event="OnGrabStateChange(grabState)"> 
    mks.setinputrelease(true);
     if (grabState == 1) {
        window.status = "Press Ctl+Alt to release cursor.";
     } else {
        window.status = "";
     }
  </script>
  <script id=size for="mks" event="OnMessage(type, message)"> 
     alert(message);
  </script>
  <script id=conn for="mks" event="OnConnectionStateChange(cntd)"> 
  </script>
  <script id=Script1 for="mks" event="OnDeviceConnectionStateChange(cntd, x, y)"> 
  </script>
  <script id=Script2 for="mks" event="OnSizeChange()"> 
      window.parent.VMWarePreviewResize(mks.VMScreenWidth + 1,mks.VMScreenHeight + 1);
  </script>
    <object width="100%" height="100%" id="mks" classid="CLSID:338095E4-1806-4ba3-AB51-38A3179200E9" codebase='plugin/msie/vmware-mks.cab#version=2,1,0,0'></object>
</form>
</body>
</html>