    var oAJAXDiv = null;
    var oAJAXButton = null;
    var oAJAXMultiple = null;
    var oAJAXList = null;
    var oAJAXText = null;
    var oAJAXImage = null;
    var oAJAXSpan = null;
    var oAJAXTimerL = null;
    var oAJAXTimerH = null;
    var oAJAXHidden = null;
    var oAJAXHiddens = null;
    var oAJAXTableAdd = null;
    var oAJAXRequest = "";
    var oAJAXNumber = null;
    var oAJAXParam1 = null;
    var oAJAXParam2 = null;
    function AJAXTextBoxGet(oText,oWidth,oHeight,oDiv,oList,oHidden,oRequest,oNumber,oMultiple,param1,param2,oHiddens,oTableAdd) {
        var oKeyCode = window.event.keyCode;
        // 40 - down, 38 - up, 27 - ESC
        if (oKeyCode == 40 && oAJAXDiv != null && oAJAXDiv.style.display == "inline") {
            if (oAJAXList.selectedIndex > -1) {
	            var oldIndex = oAJAXList.selectedIndex + 1;
	            if (oldIndex < oAJAXList.length)
    			    oAJAXList.selectedIndex = oldIndex;
    			oAJAXList.focus();
    		}
            return;
        }
        if (oKeyCode == 38 && oAJAXDiv != null && oAJAXDiv.style.display == "inline") {
	        if (oAJAXList.selectedIndex > -1) {
		        var oldIndex = oAJAXList.selectedIndex - 1;
		        if (oldIndex > -1)
    			    oAJAXList.selectedIndex = oldIndex;
    			oAJAXList.focus();
    		}
            return;
        }
        if (oKeyCode == 27 && oAJAXDiv != null && oAJAXDiv.style.display == "inline") {
            oAJAXDiv.style.display = "none";
            return;
        }
        //if (oKeyCode != 13) {
        if ((oKeyCode >= 48 && oKeyCode <= 90) || (oKeyCode >= 96 && oKeyCode <= 105) || (oKeyCode == 8) || (oKeyCode == 46) || (oKeyCode == 222) || (oKeyCode == 32)) {
            clearTimeout(oAJAXTimerL);
            oAJAXDiv = document.getElementById(oDiv);
            oAJAXList = document.getElementById(oList);
            oAJAXList.style.top = "-3px";
            oAJAXList.style.left = "-2px";
            oAJAXList.style.width = parseInt(oWidth) + 3;
            oAJAXList.style.height = parseInt(oHeight) + 9;
            oAJAXList.style.position = "absolute";
            if (oAJAXDiv == null){
                alert('ERROR: You must have a DIV field called ' + oDiv + ' on the page');
                return;
            }
            else {
                oAJAXDiv.style.width=oWidth;
                oAJAXDiv.style.height=oHeight;
            }
            oAJAXHidden = oHidden;
            oAJAXHiddens = oHiddens;
            oAJAXTableAdd = oTableAdd;
            oAJAXText = oText;
            oAJAXRequest = oRequest;
            oAJAXNumber = oNumber;
            oAJAXMultiple = document.getElementById(oMultiple);
            oAJAXParam1 = param1;
            oAJAXParam2 = param2;
            // This might not work in service editor preview, because there are more than one instance of the DIV (Edit Service Request, Preview Service Request, Preview Workload Manager)
            oAJAXDiv.style.display = "none";
            if (oAJAXText.value.length > oAJAXNumber)
                oAJAXTimerL = setTimeout("GetList();", 500);
        }
    }
    function AJAXButtonGet(oButton,oText,oWidth,oHeight,oDiv,oList,oHidden,oRequest,oNumber,oMultiple,param1,param2,oHiddens,oTableAdd) {
        clearTimeout(oAJAXTimerL);
        oAJAXDiv = document.getElementById(oDiv);
        oAJAXList = document.getElementById(oList);
        oAJAXList.style.top = "-3px";
        oAJAXList.style.left = "-2px";
        oAJAXList.style.width = parseInt(oWidth) + 3;
        oAJAXList.style.height = parseInt(oHeight) + 9;
        oAJAXList.style.position = "absolute";
        if (oAJAXDiv == null){
            alert('ERROR: You must have a DIV field called ' + oDiv + ' on the page');
            return;
        }
        else {
            oAJAXDiv.style.width=oWidth;
            oAJAXDiv.style.height=oHeight;
        }
        oAJAXHidden = oHidden;
        oAJAXHiddens = oHiddens;
        oAJAXTableAdd = oTableAdd;
        oAJAXButton = oButton;
        oAJAXText = document.getElementById(oText);
        oAJAXRequest = oRequest;
        oAJAXNumber = oNumber;
        oAJAXMultiple = document.getElementById(oMultiple);
        oAJAXMultiple.style.display = "none";
        oAJAXParam1 = param1;
        oAJAXParam2 = param2;
        oAJAXDiv.style.display = "none";
        if (oAJAXText.value.length > oAJAXNumber) {
            if (oAJAXButton != null)
                setTimeout("SearchListButton();", 100)
            oAJAXTimerL = setTimeout("GetList();", 500);
        }
        return false;
    }
	var ox = null;
    var or = null;
    function GetList() {
        setTimeout("HideTextBox();", 100);
        setTimeout("GetList2();", 300);
    }
    function HideTextBox() {
        addEvent(document, 'keydown', holdBackspace);
        oAJAXText.style.display = "none";
        var oParent = oAJAXText.parentElement;
        if (oAJAXImage == null) {
            oAJAXImage = document.createElement("IMG");
            oAJAXImage.src = "/images/active.gif";
            oAJAXImage.border = "0";
            oAJAXImage.align = "absmiddle";
            oParent.appendChild(oAJAXImage);
        }
        if (oAJAXSpan == null) {
            oAJAXSpan = document.createElement("SPAN");
            oAJAXSpan.innerText = " Loading...";
            oAJAXSpan.className = "approved";
            oParent.appendChild(oAJAXSpan);
        }
    }
    function ShowTextBox() {
        oAJAXText.style.display = "inline";
        oAJAXText.focus();
        if (oAJAXImage != null)
            oAJAXImage.style.display = "none";
        oAJAXImage = null;
        if (oAJAXSpan != null)
            oAJAXSpan.style.display = "none";
        oAJAXSpan = null;
        removeEvent(document, 'keydown', holdBackspace);
    }
    function GetList2() {
        //alert('here');
        clearTimeout(oAJAXTimerL);
        oAJAXList.length = 0;
        ox = new ActiveXObject("Microsoft.XMLHTTP");
        ox.onreadystatechange = GA_a;
        ox.open("GET", oAJAXRequest + "?u=GET", false);
        if (oAJAXParam1 == null && oAJAXParam2 == null)
            ox.send("<ajax>" + escape(oAJAXText.value) + "</ajax>");
        else if (oAJAXParam2 == null)
            ox.send("<ajax><value>" + escape(oAJAXText.value) + "</value><value>" + oAJAXParam1 + "</value></ajax>");
        else if (oAJAXParam1 == null)
            ox.send("<ajax><value>" + escape(oAJAXText.value) + "</value><value>" + oAJAXParam2 + "</value></ajax>");
        else
            ox.send("<ajax><value>" + escape(oAJAXText.value) + "</value><value>" + oAJAXParam1 + "</value><value>" + oAJAXParam2 + "</value></ajax>");
    }
    function GA_a() {
        if (ox.readyState == 4)
        {
            if (ox.status == 200) {
                if (ox.responseXML.documentElement != null) {
                    or = ox.responseXML.documentElement.childNodes;
                    setTimeout("PopulateList();", 100);
                }
            }
            else  {
                alert('There was a problem getting the information');
                ShowTextBox();
            }
        }
    }
    function PopulateList() {
        clearTimeout(oAJAXTimerL);
        if (oAJAXButton != null)
            SearchListButtonDone();
        if (or.length > 500) {
            alert('There are too many records to display - please try to limit your search further');
        }
        else {
            if (or.length == 1 && or[0].childNodes[0].text == "MULTIPLE")
                oAJAXMultiple.style.display = "inline";
            else {
                for (var ii = 0; ii < or.length; ii = ii + 2) {
                    try {
                        var oOption = document.createElement("OPTION");
                        oAJAXList.add(oOption);
                        oOption.text = or[ii + 1].childNodes[0].text;
                        oOption.value = or[ii].childNodes[0].text;
                    }
                    catch (ex) {
                    }
                }
                if (or.length == 1 || or.length == 2 && oAJAXHiddens != null) {
                    var oHiddens = document.getElementById(oAJAXHiddens);
                    var oTableAdd = document.getElementById(oAJAXTableAdd);
                    if (oHiddens == null)
                        alert('ERROR: You must have a hidden field called ' + oAJAXHiddens + ' on the page');
                    else if (oTableAdd == null)
                        alert('ERROR: You must have a table called ' + oAJAXTableAdd + ' on the page');
                    else {
                        if (CheckList(or[0].childNodes[0].text, oAJAXHiddens) == true) {
                            oHiddens.value += or[0].childNodes[0].text + ";";
                            var oTBody = oTableAdd.firstChild;
                            var oTR = document.createElement("TR");
                            var oTD1 = document.createElement("TD");
                            oTD1.innerText = or[0].childNodes[0].text;
                            var oTD2 = document.createElement("TD");
                            oTD2.innerHTML = "[<a href=\"javascript:void(0);\" onclick=\"DeleteList(this,'" + or[0].childNodes[0].text + "','" + oAJAXHiddens + "');\">Delete</a>]&nbsp;&nbsp;<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/>";
                            oTR.appendChild(oTD1);
                            oTR.appendChild(oTD2);
                            oTBody.appendChild(oTR);
                        }
                        oAJAXText.value = "";
                    }
                }
                else if (or.length > 0)
                    ShowResults();
            }
        }
        ShowTextBox();
    }
    function ShowResults() {
        if (oAJAXList.length > 0)
            oAJAXList.selectedIndex = 0;
        var oParent = oAJAXDiv.parentElement;
        oParent.removeChild(oAJAXDiv);
        document.body.appendChild(oAJAXDiv);
	    oAJAXDiv.style.posLeft = findAjaxX(oAJAXText);
	    oAJAXDiv.style.posTop = findAjaxY(oAJAXText) + 20;
        oAJAXDiv.style.display = "inline";
        //document.detachEvent("onkeydown", AJAXClick);
        removeEvent(document, 'keydown', AJAXClick);
        //document.attachEvent("onkeydown", AJAXClick);
        addEvent(document, 'keydown', AJAXClick);
    }
    function AJAXClick() {
        //alert(window.event.keyCode);
        if (window.event.keyCode == 13) {
            AJAXClickRow();
            event.cancelBubble = true;
            event.returnValue = false;
        }
    }
    function AJAXClickRow() {
		document.detachEvent("onkeydown", AJAXClick);
		if (oAJAXList.selectedIndex > -1) {
            oAJAXText.value = oAJAXList.options[oAJAXList.selectedIndex].text;
            if (oAJAXHiddens != null) {
                var oHiddens = document.getElementById(oAJAXHiddens);
                var oTableAdd = document.getElementById(oAJAXTableAdd);
                if (oHiddens == null)
                    alert('ERROR: You must have a hidden field called ' + oAJAXHiddens + ' on the page');
                else if (oTableAdd == null)
                    alert('ERROR: You must have a table called ' + oAJAXTableAdd + ' on the page');
                else {
                    if (CheckList(oAJAXList.options[oAJAXList.selectedIndex].value, oAJAXHiddens) == true) {
                        oHiddens.value += oAJAXList.options[oAJAXList.selectedIndex].value + ";";
                        var oTBody = oTableAdd.firstChild;
                        var oTR = document.createElement("TR");
                        var oTD1 = document.createElement("TD");
                        oTD1.innerText = oAJAXList.options[oAJAXList.selectedIndex].value;
                        var oTD2 = document.createElement("TD");
                        oTD2.innerHTML = "[<a href=\"javascript:void(0);\" onclick=\"DeleteList(this,'" + oAJAXList.options[oAJAXList.selectedIndex].value + "','" + oAJAXHiddens + "');\">Delete</a>]&nbsp;&nbsp;<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/>";
                        oTR.appendChild(oTD1);
                        oTR.appendChild(oTD2);
                        oTBody.appendChild(oTR);
                    }
                    oAJAXText.value = "";
                }
            }
            else {
                var oHidden = document.getElementById(oAJAXHidden);
                if (oHidden == null)
                    alert('ERROR: You must have a hidden field called ' + oAJAXHidden + ' on the page');
                else {
                    oHidden.value = oAJAXList.options[oAJAXList.selectedIndex].value;
                    try {
                        AfterAJAXFunction();
			        }
			        catch (ex) {
			        }
                }
            }
		}
		else {
		    alert('Invalid selection - please try again...');
		    return;
		}
        oAJAXDiv.style.display = "none";
        oAJAXText.focus();
    }
	function findAjaxX(obj)
	{
		var curleft = 0;
		if (obj.offsetParent)
		{
			while (obj.offsetParent)
			{
				curleft += obj.offsetLeft
				obj = obj.offsetParent;
			}
		}
		else if (obj.x)
			curleft += obj.x;
		return curleft;
	}
	function findAjaxY(obj)
	{
		var curtop = 0;
		if (obj.offsetParent)
		{
			while (obj.offsetParent)
			{
				curtop += obj.offsetTop
				obj = obj.offsetParent;
			}
		}
		else if (obj.y)
			curtop += obj.y;
		return curtop;
	}
	function DeleteList(oLink, strName, oHidden) {
	    if (confirm('Are you sure you want to delete this item?') == true) {
            oHidden = document.getElementById(oHidden);
            var strHidden = oHidden.value;
            if (strHidden.indexOf(strName + ";") > -1) {
                var strBefore = strHidden.substring(0, strHidden.indexOf(strName + ";"));
                strHidden = strHidden.substring(strHidden.indexOf(strName + ";"));
                var strAfter = strHidden.substring(strHidden.indexOf(";") + 1);
                strHidden = strBefore + strAfter;
            }
            oHidden.value = strHidden;
            var oTR = oLink.parentElement.parentElement;
            var oTBody = oTR.parentElement;
            oTBody.removeChild(oTR);
        }
	}
	function CheckList(strName, oHidden) {
        oHidden = document.getElementById(oHidden);
        var strHidden = oHidden.value;
        if (strHidden.indexOf(strName + ";") > -1)
            return false;
        else
            return true;
	}
    function SearchListButton() {
        oAJAXButton.value = "Searching...";
        oAJAXButton.disabled = true;
    }
    function SearchListButtonDone() {
        oAJAXButton.value = "Check Names";
        oAJAXButton.disabled = false;
    }
    function AjaxGetChartURL(ddlChart)
    {
        var ddlCT = document.getElementById(ddlChart);
        var xmlHttp;
        try
        {  // Firefox, Opera 8.0+, Safari 
            xmlHttp=new XMLHttpRequest(); 
        }
        catch (e)
        {  // Internet Explorer 
            try
            {  
                xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");   
            }
            catch (e)
            { 
                try
                { 
                    xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");    
                }
                catch (e)
                {
                    alert("Your browser does not support AJAX!");   
                    return false;  
                }   
            }
        }
        xmlHttp.onreadystatechange=function()
        {
            if(xmlHttp.readyState==4)
            {
                var strImage = xmlHttp.responseText;
                if (strImage == "")
                    document.getElementById('imgTest').style.display = "none";
                else {
                    document.getElementById('imgTest').src = strImage;
                    document.getElementById('imgTest').style.display = "inline";
                }
            }
        }  
        xmlHttp.open("GET","/frame/images.aspx?cid="+ddlCT.options[ddlCT.selectedIndex].value,true);
        xmlHttp.send(null); 
    }         

   function AjaxCheckNickName(strType,oName,rid,item,number)
    {
        var oKeyCode = window.event.keyCode;
        var oName = document.getElementById(oName);
        var xmlHttp;
        try
        {  // Firefox, Opera 8.0+, Safari 
            xmlHttp=new XMLHttpRequest(); 
        }
        catch (e)
        {  // Internet Explorer 
            try
            {  
                xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");   
            }
            catch (e)
            { 
                try
                { 
                    xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");    
                }
                catch (e)
                {
                    alert("Your browser does not support AJAX!");   
                    return false;  
                }   
            }
        }
        xmlHttp.onreadystatechange=function()
        {
            if(xmlHttp.readyState==4)
            {       
               var result = xmlHttp.responseText;                       
               if(result == "True")
               {               
                  alert('This Nickname is already taken! Please enter a different name');
                  oName.value ="";
                  oName.focus();
               }
            }
        }  
        xmlHttp.open("GET","/frame/pcr_csrc_nicknames.aspx?type="+ strType +"&name="+oName.value+"&rid="+rid+"&item="+item+"&num="+number,true);
        xmlHttp.send(null); 
    }   
    
     function AjaxGetItemAmount(ddlItem,oLabel,strId)
    {
       
        if(strId == "")
        {
          ddlItem = document.getElementById(ddlItem);  
          oLabel = document.getElementById(oLabel);               
        }       
        var xmlHttp;
        try
        {  // Firefox, Opera 8.0+, Safari 
            xmlHttp=new XMLHttpRequest(); 
        }
        catch (e)
        {  // Internet Explorer 
            try
            {  
                xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");   
            }
            catch (e)
            { 
                try
                { 
                    xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");    
                }
                catch (e)
                {
                    alert("Your browser does not support AJAX!");   
                    return false;  
                }   
            }
        }
        xmlHttp.onreadystatechange=function()
        {
            if(xmlHttp.readyState==4)
            {
                var strValue = xmlHttp.responseText;                                                
                oLabel.innerText = strValue;                             
            }
        }  
        var idval = strId !="" ? strId: ddlItem.options[ddlItem.selectedIndex].value;
        xmlHttp.open("GET","/frame/item_amount.aspx?id="+idval,true);
        xmlHttp.send(null); 
    }         
var oActiveXEditor = null;
function LoadEditorAffectsDropDown(oDDL) {
    if (oDDL.selectedIndex > 0) {
        var strValue = oDDL.options[oDDL.selectedIndex].title;
        //alert('LoadEditorAffectsDropDown = ' + strValue);
        LoadEditorAffects(strValue);
    }
}
function LoadEditorAffectsRadio(oRadio) {
    var strValue = oRadio.title;
    //alert('LoadEditorAffectsRadio = ' + strValue);
    LoadEditorAffects(strValue);
}
function LoadEditorAffects(strResponse) {
    oActiveXEditor = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXEditor.onreadystatechange = LoadEditorAffects_a;
    oActiveXEditor.open("GET", "/frame/ajax/ajax_service_editor_affects.aspx?u=GET", false);
    //alert(escape(strResponse));
    oActiveXEditor.send("<ajax>" + escape(strResponse) + "</ajax>");
}
function LoadEditorAffects_a() {
    if (oActiveXEditor.readyState == 4)
    {
        if (oActiveXEditor.status == 200) {
            var or = oActiveXEditor.responseXML.documentElement.childNodes;
            //for (var ii=0; ii<or.length; ii=ii+2)
            //    alert('div' + or[ii].childNodes[0].text + ' = ' + or[ii + 1].childNodes[0].text);
            PopulateAffectEditor(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateAffectEditor(or) {
    var strScript = "";
    for (var ii=0; ii<or.length; ii=ii+2) {
        var oTemp = document.getElementById('div' + or[ii].childNodes[0].text);
        if (oTemp != null) {
            oTemp.style.display = or[ii+1].childNodes[0].text;
            //alert('div' + or[ii].childNodes[0].text + ' = ' + or[ii + 1].childNodes[0].text);
            if (oTemp.style.display == "inline") {
                var oDDLs = oTemp.getElementsByTagName("select");
                for(jj=0;jj<oDDLs.length;jj++)
                    LoadEditorAffectsDropDown(oDDLs[jj]);
            }
        }
    }
}


function addEvent(obj, type, fn) {
    if (obj.attachEvent) {
        obj['e' + type + fn] = fn;
        obj[type + fn] = function () { obj['e' + type + fn](window.event); }
        obj.attachEvent('on' + type, obj[type + fn]);
    } else
        obj.addEventListener(type, fn, false);
}
function removeEvent(obj, type, fn) {
    if (obj.detachEvent) {
        try {
            obj.detachEvent('on' + type, obj[type + fn]);
            obj[type + fn] = null;
        }
        catch (ex) { }
    } else
        obj.removeEventListener(type, fn, false);
}

function holdBackspace() {
    if (window.event.keyCode == 8)  // backspace
        window.event.returnValue = false;
}