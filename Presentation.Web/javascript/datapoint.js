    var boolIsDataPointChanged = false;
    function DataPointChanged() {
        boolIsDataPointChanged = true;
    }
    function DataPointSaved() {
        boolIsDataPointChanged = false;
        return true;
    }
    function DataPointChange() {
        window.onbeforeunload = DataPointChange2;
    }
    function DataPointChange2() {
        if (boolIsDataPointChanged == true)
            return "WARNING: You have made changes without saving!";
    }
    function UpdateDataPointSearch(oHidden) {
        var oObject = event.srcElement;
        if (oObject.tagName == "INPUT") {
            oHidden = document.getElementById(oHidden);
            oHidden.value = oObject.value;
        }
    }
    var oDataPointDeviceNameReturn = 0;
    var oDataPointDeviceName = null;
    var oDataPointDeviceHidden = null;
    var oActiveXDataPointDeviceName = null;
    function ValidateDataPointDeviceName(boolRequired, oName, strAlert, oHidden) {
        oName = document.getElementById(oName);
        var oValid = (trim(oName.value) != "");
        if (boolRequired == true && ValidateText(oName, strAlert) == false)
            return false;
        else if (oValid == true)
        {
            if (oHidden != null)
                oHidden = document.getElementById(oHidden);
            oDataPointDeviceName = oName;
            oDataPointDeviceHidden = oHidden;
            oActiveXDataPointDeviceName = new ActiveXObject("Microsoft.XMLHTTP");
            oActiveXDataPointDeviceName.onreadystatechange = CheckDataPointDeviceName_a;
            oActiveXDataPointDeviceName.open("GET", "/frame/ajax/datapoint/ajax_datapoint_name.aspx?u=GET", false);
            oActiveXDataPointDeviceName.send("<ajax><value>" + escape(oName.value) + "</value></ajax>");
            while (oDataPointDeviceNameReturn == 0) {
            }
            if (oDataPointDeviceNameReturn == -1)
                return false;
            else
                return true;
        }
        else
            return true;
    }
    function CheckDataPointDeviceName_a() {
        if (oActiveXDataPointDeviceName.readyState == 4)
        {
            if (oActiveXDataPointDeviceName.status == 200) {
                var or = oActiveXDataPointDeviceName.responseXML.documentElement.childNodes;
                if (or[0].childNodes[0].text == "0") {
                    oDataPointDeviceNameReturn = -1;
                    if (oDataPointDeviceHidden != null) 
                    {
                        if (confirm('The device name you entered was not found. You will be prompted to choose to which company this device name should be associated.\n\nClick OK to continue...') == true)
                        {
                            if (confirm('Does this device name belong to PNC?') == true)
                                oDataPointDeviceHidden.value = "1";
                            else if (confirm('Does this device name belong to National City?') == true)
                                oDataPointDeviceHidden.value = "2";
                            else
                                oDataPointDeviceHidden.value = "0";
                        }
                        else
                            oDataPointDeviceHidden.value = "0";
                        oDataPointDeviceNameReturn = 1;
                    }
                    else {
                        oDataPointDeviceNameReturn = -1;
                        alert('The device name you entered was not found. Please enter a valid device name.\n\nIf you think this device exists, please contact your ClearView administrator');
                        oDataPointDeviceName.focus();
                        return false;
                    }
                }
                else {
                    oDataPointDeviceNameReturn = 1;
                }
            }
            else 
                alert('There was a problem getting the information');
        }
    }
