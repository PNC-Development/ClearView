<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_server.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_server" %>

<script type="text/javascript">
function UpdateDDL(strQuestion) {
    var oTemp = document.getElementById('hdnQ_' + strQuestion);
    var oDropDown = document.getElementById('divA_' + strQuestion);
    var strValue = oDropDown.options[oDropDown.selectedIndex].value;
    if (strValue == "0")
        LoadJavascript("RESET=" + oTemp.value);
    else
        LoadJavascript(strValue);
    oTemp.value = strValue;
}
function UpdateQuestion(strQuestion, strValue) {
    var oTemp = document.getElementById('hdnQ_' + strQuestion);
    oTemp.value = strValue;
    LoadJavascript(strValue);
}
var oActiveXForecast = null;
function LoadJavascript(strResponse) {
    LoadCustoms(strResponse);
    LoadAffects(strResponse);
    LoadVariance(strResponse);
}
function LoadCustoms(strResponse) {
    oActiveXForecast = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXForecast.onreadystatechange = LoadCustoms_a;
    oActiveXForecast.open("GET", "/frame/ajax/ajax_forecast_custom.aspx?u=GET", false);
    oActiveXForecast.send("<ajax>" + escape(strResponse) + "</ajax>");
}
function LoadCustoms_a() {
    if (oActiveXForecast.readyState == 4)
    {
        if (oActiveXForecast.status == 200) {
            var or = oActiveXForecast.responseXML.documentElement.childNodes;
            PopulateCustomForecast(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateCustomForecast(or) {
    var strScript = "";
    for (var ii=0; ii<or.length; ii=ii+2) {
        var oTemp = document.getElementById('divC_' + or[ii].childNodes[0].text);
        if (oTemp != null) {
            oTemp.style.display = or[ii+1].childNodes[0].text;
//            alert(or[ii].childNodes[0].text + ' = ' + or[ii+1].childNodes[0].text);
        }
    }
}
function LoadAffects(strResponse) {
    oActiveXForecast = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXForecast.onreadystatechange = LoadAffects_a;
    oActiveXForecast.open("GET", "/frame/ajax/ajax_forecast_affects.aspx?u=GET", false);
    oActiveXForecast.send("<ajax>" + escape(strResponse) + "</ajax>");
}
function LoadAffects_a() {
    if (oActiveXForecast.readyState == 4)
    {
        if (oActiveXForecast.status == 200) {
            var or = oActiveXForecast.responseXML.documentElement.childNodes;
            PopulateAffectForecast(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateAffectForecast(or) {
    var strScript = "";
    for (var ii=0; ii<or.length; ii=ii+2) {
        var oTemp = document.getElementById('divQ_' + or[ii].childNodes[0].text);
        if (oTemp != null) {
            oTemp.style.display = or[ii+1].childNodes[0].text;
//            alert(or[ii].childNodes[0].text + ' = ' + or[ii+1].childNodes[0].text);
        }
    }
}


function LoadVariance(strResponse) {
    oActiveXForecast = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXForecast.onreadystatechange = LoadVariance_a;
    oActiveXForecast.open("GET", "/frame/ajax/ajax_forecast_variance.aspx?u=GET", false);
    oActiveXForecast.send("<ajax>" + escape(strResponse) + "</ajax>");
}
function LoadVariance_a() {
    if (oActiveXForecast.readyState == 4)
    {
        if (oActiveXForecast.status == 200) {
            var or = oActiveXForecast.responseXML.documentElement.childNodes;
            PopulateVarianceForecast(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateVarianceForecast(or) {
    var strScript = "";
    for (var ii=0; ii<or.length; ii=ii+2) {
        var oTemp = document.getElementById('divV_' + or[ii].childNodes[0].text);
        if (oTemp != null) {
            oTemp.style.display = or[ii+1].childNodes[0].text;
        }
    }
}


function EnsureDropDown(oDiv, oObject, strAlert) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display != "none") {
        oObject = document.getElementById(oObject);
        if (oObject != null && oObject.selectedIndex == 0 && boolValidate == true) {
            oObject.focus();
            alert(strAlert);
            return false;
        }
        return true;
    }
    else
        return true;
}
function EnsureRadios(oDiv, strStart, intCount, strError) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display != "none") {
	    var boolValid = false;
	    for(var ii=1; ii<=intCount; ii++) {
		    var oTemp = document.getElementById(strStart + ii);
		    if (oTemp.checked == true)
		    {
			    boolValid = true;
			    break;
		    }
	    }
        if (boolValid == false) 
        {
            alert(strError);
            oTemp.focus();
            return false;
        }
	    return true;
	}
	else
	    return true;
}
var oSaveCheckDiv123 = null;
function UpdateCheck(oHidden, oCheck, strValue) {
    oHidden = document.getElementById(oHidden);
    var strHidden = oHidden.value;
    oSaveCheckDiv123 = null;
    if (strValue.indexOf("_") > -1)
        CheckDiv1(strValue);
    if (oCheck.checked == true) {
        if (strHidden.indexOf(strValue) == -1)
            strHidden += strValue;
    }
    else {
        if (strHidden.indexOf(strValue) > -1) {
            var strBefore = strHidden.substring(0, strHidden.indexOf(strValue));
            strHidden = strHidden.substring(strHidden.indexOf(strValue));
            var strAfter = strHidden.substring(strHidden.indexOf("|") + 1);
            strHidden = strBefore + strAfter;
        }
    }
    // Format of response VAL1_VAL2_VAL3_
    if (strValue.indexOf("_") > -1)
        CheckDiv2(strHidden);
    if (strHidden == "")
        LoadJavascript("RESET=" + strValue);
    else
        LoadJavascript(strHidden);
    oHidden.value = strHidden;
}
function CheckDiv1(strValue) {
    strValue = strValue.substring(strValue.indexOf("_") + 1);
    oSaveCheckDiv123 = document.getElementById(strValue.substring(0, strValue.indexOf("&")));
    oSaveCheckDiv123.style.display = "none";
}
function CheckDiv2(strHidden) {
    var boolStop = false;
    while (strHidden != "" && boolStop == false)
    {
        if (strHidden.indexOf("_") > -1) {
            strHidden = strHidden.substring(strHidden.indexOf("_") + 1);
            var oDiv = document.getElementById(strHidden.substring(0, strHidden.indexOf("&")));
            if (oDiv == oSaveCheckDiv123) 
            {
                strHidden = strHidden.substring(strHidden.indexOf("&") + 1);
                var strDisplay = strHidden.substring(0, strHidden.indexOf("|"));
                if (strDisplay == "inline") {
                    oSaveCheckDiv123.style.display = strDisplay;
                    boolStop = true;
                }
            }
        }
        strHidden = strHidden.substring(strHidden.indexOf("|") + 1);
    }
}
function EnsureTextDDL(oText, oDDL, strValue, strAlert) {
    oDDL = document.getElementById(oDDL);
    if (oDDL.options[oDDL.selectedIndex].value == strValue) {
        return ValidateText(oText, strAlert);
    }
    return true;
}
function EnsureTextRadio(oText, oItem, strAlert) {
    oItem = document.getElementById(oItem);
    if (oItem.checked == true) {
        return ValidateText(oText, strAlert);
    }
    return true;
}
function UpdateCustomValue(oText, oHidden) {
	oHidden = document.getElementById(oHidden);
	oHidden.value = oText.value;
}
</script>


<table width="100%" cellpadding="4" cellspacing="2" border="0"  >
   <tr>
      
        <td id="tdQuestions"  colspan="2" runat="server"></td>
        <%--<td  colspan="2"><%=strQuestions %><%=strHidden %></td>--%>
    </tr>
</table>
<table width="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="3"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td class="required">* = Required Field</td>
        <td align="center">
            <asp:Panel ID="panNavigation" runat="server" Visible="false">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
            </asp:Panel>
            <asp:Panel ID="panUpdate" runat="server" Visible="false">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
            </asp:Panel>
        </td>
        <td align="right">
            <asp:Button ID="btnHundred" runat="server" OnClick="btnCancel_Click" Text="Back" CssClass="default" Width="75" Visible="false" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
        </td>
    </tr>
</table>
<asp:Label ID="lblPlatform" runat="server" Visible="false" />
<asp:Label ID="lblRecovery" runat="server" Visible="false" />
<asp:Label ID="lblQuantity" runat="server" Visible="false" />
