	var strColor = null;
	var strSubColor = null;
	var strRowEnter = "#333333";
    var oDrag = false;
    var oClock = null;
    var oClockSep = null;
    var oWindow = null;
    var oSaveControl = null;
    var oDivMenu = null;
    var oFrameMenu = null;
    var oDivSubMenu = null;
    var oFrameSubMenu = null;
    var oHideMenus = null;
    var oHideSubMenus = null;
    var strWidth1 = "";
    var strWidth2 = "";
	function EnablePostBack(boolTop) {
        if (window.parent == null || window.parent == window)
            window.navigate("/admin/admin_index.aspx");
        boolTop = true;
        if (boolTop == true) {
	        var oMainTool = Get("divToolbar");
	        var oMainFrame = Get("frmToolbar");
            ToolBarOnTop(oMainTool, oMainFrame);
            DHTMLHelp(oMainTool, oMainFrame);
        }
	    document.frmMain.action = '/' + document.frmMain.action;
	}
	function DHTMLHelp(oMainTool, oMainFrame) {
	    oMainTool.style.zIndex = 100;
        oMainFrame.style.width = oMainTool.offsetWidth;
        oMainFrame.style.height = parseInt(oMainTool.offsetHeight);
        oMainFrame.style.top = oMainTool.style.top;
        oMainFrame.style.left = oMainTool.style.left;
        oMainFrame.style.zIndex = oMainTool.style.zIndex - 1;
        oMainFrame.style.display = "inline";
	}
    function AdminMouseOver(oRow) {
	    strColor = oRow.bgColor;
	    oRow.bgColor = strRowEnter;
	    oRow.style.cursor = "hand";
	    oRow.className = "whitedefault";
	    //clearTimeout(oHideMenus);
    }
    function AdminMouseOut(oRow) {
	    oRow.bgColor = strColor;
	    oRow.style.cursor = "default";
	    oRow.className = "default";
	    //clearTimeout(oHideMenus);
	    //oHideMenus = setTimeout("HideMenus()",1000);
    }
	function Get(strObject) {
	    var oObject = document.getElementById(strObject);
	    var oFrames = window.top.frames;
	    if (oObject == null && oFrames.length > 0) {
	        for (var ii=0; ii<oFrames.length; ii++) {
	            oObject = oFrames(ii).document.getElementById(strObject);
	            if (oObject != null)
	                break;
	            else
	                return GetObject(strObject, oFrames(ii));
	        }
	    }
	    return oObject;
	}
	function GetObject(strObject, oFrame) {
	    var oObject = null;
	    var oFrames = oFrame.frames;
	    if (oObject == null && oFrames.length > 0) {
	        for (var ii=0; ii<oFrames.length; ii++) {
	            oObject = oFrames(ii).document.getElementById(strObject);
	            if (oObject == null)
	                return GetObject(strObject, oFrames(ii));
	        }
	    }
	    return oObject;
	}
    function RefreshParent() {
        window.top.location.reload();
    }
    function overDrag() {
        // tell onOverDrag handler not to do anything:
        window.event.returnValue = false;
    }
    function enterDrag() {
        // allow target object to read clipboard:
        window.event.dataTransfer.getData('Text');
    }
    function onDrop(strPH, oHidden) {
        oHidden = Get(oHidden);
        oHidden.value = strPH + "_" + oDrag.options[oDrag.selectedIndex].value;
        document.forms[0].submit();
    }
    function ShowMenu(oButton, oDiv, intWidth, intHeight) {
        HideMenus();
	    document.detachEvent("onclick", HideMenus);
	    clearTimeout(oHideMenus);
	    oDiv.style.posLeft = findPosX(oButton) - intWidth;
	    oDiv.style.posTop = findPosY(oButton) - intHeight + 25;
	    oDiv.style.display = "inline";
	    oFrameMenu = Get("frmHelper");
	    DHTMLHelp(oDiv, oFrameMenu);
	    oDivMenu = oDiv;
        setTimeout("SetHideMenus()",200);
        return false;
    }
    function ShowSubMenu(oDiv, intWidth, intHeight) {
        var oFrame = window.top.GetFrame(0);
        if (parseInt(oDivMenu.style.posLeft) + parseInt(oDivMenu.style.width) > parseInt(oFrame.document.body.clientWidth) - 250) {
	        oDiv.style.posLeft = parseInt(findPosX(oDivMenu)) - parseInt(oDiv.style.width);
	        oDiv.style.posTop = parseInt(findPosY(oDivMenu)) - parseInt(intHeight);
	    }
        else {
	        oDiv.style.posLeft = parseInt(findPosX(oDivMenu)) + parseInt(intWidth);
	        oDiv.style.posTop = parseInt(findPosY(oDivMenu)) - parseInt(intHeight);
        }
	    oDiv.style.display = "inline";
	    oFrameSubMenu = Get("frmSubHelper");
	    DHTMLHelp(oDiv, oFrameSubMenu);
	    oDivSubMenu = oDiv;
        setTimeout("SetHideMenus()",200);
        return false;
    }
    function SetHideMenus() {
	    document.attachEvent("onclick", HideMenus);
    }
    function HideMenus() {
	    document.detachEvent("onclick", HideMenus);
	    if (oDivMenu != null)
		    oDivMenu.style.display = "none";
	    if (oDivSubMenu != null)
		    oDivSubMenu.style.display = "none";
	    if (oFrameMenu != null)
		    oFrameMenu.style.display = "none";
	    if (oFrameSubMenu != null)
		    oFrameSubMenu.style.display = "none";
//        ShowSelects(true);
    }
    function HideSubMenu() {
	    if (oDivSubMenu != null)
		    oDivSubMenu.style.display = "none";
	    if (oFrameSubMenu != null)
		    oFrameSubMenu.style.display = "none";
    }
    
    function OpenWindowBasedOnParentSelection(strType, oObject,oObjectParent, strVar, boolWindow, strWidth, strHeight)
    {
     if (boolWindow == true && oWindow != null && !oWindow.closed)
		    oWindow.close();
		if (oObject != null && oObject != "")
		    oObject = Get(oObject);
        if (oObjectParent != null && oObjectParent != "")
		    oObjectParent = Get(oObjectParent);
		    
		 if (strType == "ROOM_BROWSER") 
		 {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_room_browser.aspx?id=" + oObject.value +"&parentid="+ oObjectParent.value + strVar, strWidth, strHeight);
         }                
        
         return false;
		    
    }
    
    function OpenWindow(strType, oObject, strVar, boolWindow, strWidth, strHeight) {
	    if (boolWindow == true && oWindow != null && !oWindow.closed)
		    oWindow.close();
		if (oObject != null && oObject != "")
		    oObject = Get(oObject);
        if (strType == "FILEBROWSER") {
            if (boolWindow == false) {
                if (strVar == "")
                    ShowPanel(false, "/admin/frame/frame_file_browser.aspx?control=" + oObject.id, strWidth, strHeight);
                else
                    ShowPanel(false, "/admin/frame/frame_file_browser.aspx?control=" + oObject.id + "&location=" + strVar, strWidth, strHeight);
            }
        }
        if (strType == "REPORTFILEBROWSER") {
            if (boolWindow == false) {
                if (strVar == "")
                    ShowPanel(false, "/admin/frame/frame_report_file_browser.aspx?control=" + oObject.id, strWidth, strHeight);
                else
                    ShowPanel(false, "/admin/frame/frame_report_file_browser.aspx?control=" + oObject.id + "&location=" + strVar, strWidth, strHeight);
            }
        }
        else if (strType == "INSERTIMAGE") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_image.aspx?type=INSERT&" + strVar, strWidth, strHeight);
        }
        else if (strType == "UPDATEIMAGE") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_image.aspx?type=UPDATE&" + strVar, strWidth, strHeight);
        }
        else if (strType == "IMAGEPATH") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_image.aspx?control=" + strVar + "&type=PATH", strWidth, strHeight);
        }
        else if (strType == "INSERTLINK") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/link_browser.aspx?type=INSERT&" + strVar, strWidth, strHeight);
        }
        else if (strType == "UPDATELINK") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/link_browser.aspx?type=UPDATE&" + strVar, strWidth, strHeight);
        }
        else if (strType == "LINKPATH") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/link_browser.aspx?control=" + strVar + "&type=PATH", strWidth, strHeight);
        }
        else if (strType == "PAGEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_page_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "FORECASTQUESTIONBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_forecast_question_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "FORECASTRESPONSEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_forecast_response_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "AFFECTS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_question_affects.aspx" + strVar, strWidth, strHeight);
        }
        else if (strType == "AFFECTED") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_question_affected.aspx" + strVar, strWidth, strHeight);
        }
        else if (strType == "PAGEORDER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_order.aspx?id=" + oObject.value + "&control=" + strVar, strWidth, strHeight);
        }
        else if (strType == "SUPPORTORDER") {
            var strParent = "";
            if (oObject.selectedIndex == null)
                strParent = oObject.value;
            else
                strParent = oObject.options[oObject.selectedIndex].value;
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_support_order.aspx?id=" + strParent + "&control=" + strVar, strWidth, strHeight);
        }
        else if (strType == "CONTROLS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_controls.aspx?id=" + oObject.value + "&type=" + strVar, strWidth, strHeight);
        }
        else if (strType == "PAGECONTROLS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_controls.aspx?id=" + strVar + "&type=1", strWidth, strHeight);
        }
        else if (strType == "COLOR") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_color.aspx?control=" + strVar, strWidth, strHeight);
        }
        else if (strType == "CALENDAR") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_calendar.aspx?control=" + strVar, strWidth, strHeight);
        }
        else if (strType == "SHARECONTROL") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/share_control.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "ROTATOR") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_rotator_control.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "ROTATORS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_rotator.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "HTMLSOURCE") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_source.aspx?control=" + oObject.id, strWidth, strHeight);
        }
        else if (strType == "HTML") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/html.aspx?control=" + oObject.id, strWidth, strHeight);
        }
        else if (strType == "PROPERTIES") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_page.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "FORM") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_form.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "FORMS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_form_controls.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "FORMCONTROLS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/manage_form_control.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "APPLICATIONBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_application_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "SERVICEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_service_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "SERVICEFOLDERBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_service_folder_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "LOCATION_BROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_location_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "LOCATION_S_BROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_location_s_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "LOCATION_C_BROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_location_c_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "SOLUTIONCODEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_solution_code_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "MODELBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_asset_model_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "SUBMODELBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_asset_submodel_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "TYPEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_asset_type_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "TABLEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_table_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "PLATFORMBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_platform_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "REPORTBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_report_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "ITEMBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_item_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "DOCUMENTBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_document_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "SQLTABLEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_sql_table_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "SERVICEDETAILBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_service_detail_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "APPPAGEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_app_page_browser.aspx?applicationid=" + strVar, strWidth, strHeight);
        }
        else if (strType == "USERPAGEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_user_page_browser.aspx?userid=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "TABLEITEMBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_table_item_browser.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "TABLESERVICEBROWSER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_table_service_browser.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "PERMISSIONS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_permissions.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "REPORTPERMISSIONS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_report_permissions.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "REPORTAPPLICATIONS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_report_applications.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "REPORTUSERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_report_users.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "SUBAPPLICATIONS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_subapplications.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "USERROLES") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_user_roles.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "BOOTGROUPSTEPS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_models_boot_group_steps.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "WEBSERVICEUSERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_webservice_users.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "REPORTGROUPS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_report_groups.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "SERVICEDETAILROLES") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_service_details.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "SERVICELOCATIONS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_service_locations.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "QUESTIONPLATFORMS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_question_platforms.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "FORECASTSTEPRESET") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_forecast_steps_reset.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "DOMAINCLASSENVIRONMENTS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_domain_class_environment.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "VMWARETEMPLATECLASSENVIRONMENTS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_vmware_templates_class_environment.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "SERVICE_USERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_service_users.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "PLATFORM_USERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_platform_users.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "REQUESTITEM_EDITOR") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_request_item_editor.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "MODEL_ENVIRONMENTS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_model_environments.aspx" + strVar, strWidth, strHeight);
        }
        else if (strType == "SERVERNAMELOCATIONS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_servername_locations.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "SELECTIONCRITERIA") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_selection_criteria.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "RESPONSES_ADDITIONAL") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_responses_addtional.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "CLASSJOINS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_class_joins.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "CLASSENVIRONMENTS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_class_environments.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "CLASSENVIRONMENTSAP") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_class_environments_ap.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "APPLICATIONSOS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_server_applications_os.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "APPLICATIONSMODELS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_server_applications_models.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "SUBAPPLICATIONSMODELS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_server_subapplications_models.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "SERVER_COMPONENTS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_server_components_os.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "WORKSTATION_COMPONENTS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_workstation_components_os.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "NETWORKSRELATED") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_networks_related.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "OSSERVICEPACKS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_os_service_packs.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "GLOBAL_SOLUTION_CODE") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_solution_codes_location.aspx", strWidth, strHeight);
        }
        else if (strType == "WORKLOAD_MGR_TAB") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_workload_manager_tabs_view.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "PROJECTREQUESTQA") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_project_request_qa.aspx?questionid=" + oObject.value, strWidth, strHeight);
        }
         else if (strType == "PROJECTREQUESTCLASS") {
            if (boolWindow == false)               
                ShowPanel(false, "/admin/frame/frame_project_request_question_class.aspx?questionid=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "VMWARE_VIRTUALCENTERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_vmware_virtual_centers.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "VMWARE_DATACENTERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_vmware_datacenters.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "VMWARE_OS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_vmware_os.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "VMWARE_FOLDERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_vmware_folders.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "VMWARE_VLANS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_vmware_vlans.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "TSM_BROWSER_SERVER") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_tsm_server_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "TSM_BROWSER_DOMAIN") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_tsm_domain_browser.aspx?id=" + oObject.value + strVar, strWidth, strHeight);
        }
        else if (strType == "RDP_ZONES") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_build_locations_rdp_zones.aspx?id=" + strVar, strWidth, strHeight);
        }
        else if (strType == "DESIGN_USERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/design/approval_users.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "ENHANCEMENT_APPROVERS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/enhancement_approval_group_users.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        else if (strType == "OPERATING_SYSTEM_GROUPS") {
            if (boolWindow == false)
                ShowPanel(false, "/admin/frame/frame_os_groups.aspx?id=" + oObject.value, strWidth, strHeight);
        }
        return false;
    }
    function GetHTML(strControl) {
        strControl = Get(strControl);
        return strControl.innerHTML;
    }
	function GetFrame(_ii) {
	    var oFrames = window.top.frames;
	    if (oFrames.length > 0) {
            for (var ii=0; ii<oFrames.length; ii++) {
                if (_ii == ii)
                    return oFrames(ii);
            }
	    }
	    return null;
	}
    function UpdateWindow(strValue, strControlValue, strText, strControlText) {
           
            if (strControlValue != null && strValue != null) 
            {
                strControlValue = Get(strControlValue);
                strControlValue.value = strValue;
            }
        
            if (strText != null && strControlText != null) 
            {
                strControlText = Get(strControlText);
                strControlText.innerText = strText;
            }
    }
    function UpdateDiv(strValue, strControl) {
        strControl = Get(strControl);
        strControl.innerHTML = strValue;
        cleanUp(strControl);
    }
    var URL_Host ='http://' + window.location.hostname + '/';
    var URL_Dir = 'http://' + window.location.hostname +  window.location.pathname;
    URL_Dir = URL_Dir.substr(0,URL_Dir.lastIndexOf("/"));
    var URL = window.location.toString();
    function fixurl(strRef)
    {
	    if (strRef.substr(0,URL_Host.length)==URL_Host)
	    {
		    if(strRef.substr(0,URL_Dir.length)==URL_Dir)
		    {
			    if(strRef.substr(0,URL.length)==URL)
				    strRef =  strRef.substr(URL.length);
			    else
				    strRef =  strRef.substr(URL_Dir.length);
		    }
		    else
			    strRef = '/'+ strRef.substr(URL_Host.length);
	    }
	    return strRef; 
    }

    function cleanUp(oObject)
    {
	    for(ii = 0; ii <= oObject.all.tags('A').length -1; ii++) {               
		    oTemp = oObject.all.tags('A').item(ii);
		    oTemp.href= fixurl(oTemp.href);
	    }
	    for(ii = 0; ii <= oObject.all.tags('IMG').length -1; ii++) {               
		    oTemp = oObject.all.tags('IMG').item(ii);
		    oTemp.src = fixurl(oTemp.src);
	    }
    }
	function ShowPanel(boolEnable, strFile, intWidth, intHeight) {
		var oDivLeft = window.top.document.getElementById('divLeftMenu');
		var oFrameLeft = window.top.document.getElementById('frmLeftMenu');
		var oFrameCover = window.top.document.getElementById('frmCover');
	    oDivLeft.style.display = "inline";
	    oDivLeft.style.posTop = 0;
		oFrameLeft.src = "";
	    oFrameLeft.style.posTop = window.top.document.body.scrollTop + 50;
        oDivLeft.style.height = '100%';
	    oFrameLeft.style.display = "none";
	    oFrameLeft.style.border = "solid 2px #999999";
        oDivLeft.style.width = intWidth + "px";
//        oFrameLeft.contentWindow.location = strFile;
        oFrameLeft.contentWindow.navigate(strFile);
	    oFrameLeft.style.display = "inline";
        oFrameLeft.style.posLeft = 10;
        oFrameLeft.style.width = (intWidth - 20) + "px";
        var intWindowHeight = (window.top.document.body.clientHeight);
        if ((intWindowHeight - 50) > intHeight)
            oFrameLeft.style.height = intHeight + "px";
        else
            oFrameLeft.style.height = (intWindowHeight - 100) + "px";
        DHTMLHelp(oDivLeft, oFrameCover);
		return false;
	}
	function HidePanel() {
	    oDivLeft = window.top.document.getElementById('divLeftMenu');
	    oFrameLeft = window.top.document.getElementById('frmLeftMenu');
		oFrameLeft.src = "";
		oFrameLeft.style.display = "none";
		oDivLeft.style.width = "0px";
		oDivLeft.style.display = "none";
		return false;
	}
    function findPosX(obj)
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
    function findPosY(obj)
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
    function ApproveAll_(strPath) {
        if (oEditing == null) {
            if (confirm('Are you sure you want to approve all of the controls on this page?') == true) {
                strPath += "&type=APPROVE";
                oxml = new ActiveXObject("Microsoft.XMLHTTP");
                oxml.onreadystatechange = ApproveAll_ajax;
                oxml.open("POST", strPath, false);
                oxml.send(null);
	        }
	    }
	    else
            alert('You are currently editing an HTML control on this page.\n\nPlease Save or Cancel your changes before you continue...');
    }
    function ApproveAll_ajax() {
        if (oxml.readyState == 4)
        {
            if (oxml.status == 200) {
                alert('All controls have been approved!');
                window.location.reload();
            }
            else 
                alert('AJAX Error');
        }
    }
    function Publish_(strPath, strPublish) {
        strPath += "&type=PUBLISH";
        oxml = new ActiveXObject("Microsoft.XMLHTTP");
        oxml.onreadystatechange = Publish_ajax;
        oxml.open("POST", strPath, false);
        oxml.send("<ajax>" + strPublish + "</ajax>");
        return false;
    }
    function Publish_ajax() {
        if (oxml.readyState == 4)
        {
            if (oxml.status == 200) {
                alert('The page has been updated!');
                window.location.reload();
            }
            else 
                alert('AJAX Error');
        }
    }
    function Start_(strPath) {
        if (confirm('Are you sure you want to change the start page for this website?') == true) {
            strPath += "&type=START";
            oxml = new ActiveXObject("Microsoft.XMLHTTP");
            oxml.onreadystatechange = Publish_ajax;
            oxml.open("POST", strPath, false);
            oxml.send(null);
        }
        return false;
    }
    function AddPageSchema(intPId, intSchema, strPath) {
        if (confirm('Are you sure you want to replace the current control with this new control?') == true) {
            strPath += "&type=ADDPAGE";
            oxml = new ActiveXObject("Microsoft.XMLHTTP");
            oxml.onreadystatechange = AddSchema_ajax;
            oxml.open("POST", strPath, false);
            oxml.send("<ajax><page>" + intPId + "</page><schema>" + intSchema + "</schema></ajax>");
        }
    }
    function AddSchema_ajax() {
        if (oxml.readyState == 4)
        {
            if (oxml.status == 200) {
                window.location.reload();
            }
            else 
                alert('AJAX Error');
        }
    }
    function AddTemplateSchema(intTId, intSchema, strPath) {
        if (confirm('Are you sure you want to replace the current control with this new control?') == true) {
            strPath += "&type=ADDTEMPLATE";
            oxml = new ActiveXObject("Microsoft.XMLHTTP");
            oxml.onreadystatechange = AddSchema_ajax;
            oxml.open("POST", strPath, false);
            oxml.send("<ajax><template>" + intTId + "</template><schema>" + intSchema + "</schema></ajax>");
        }
    }
	function ShowSelects(bool) {
		var oChildren = document.body.all.tags("SELECT");
		for (ii=0; ii<oChildren.length; ii++) {
			if (bool)
				oChildren(ii).style.visibility = "visible";
			else
				oChildren(ii).style.visibility = "hidden";
		}
	}
	function Preview(oText) {
	    oText = Get(oText);
        try {
            window.open(oText.value);
        }
        catch(e) {
            alert('There was a problem previewing the page');
        }
        return false;
	}
	function TopMenuSwitch(oElement, strClass) {
		oElement.className = strClass;
		if (oElement != null && oElement.children.length > 0) {
    			for (var ii=0; ii<oElement.children.length; ii++) {
                		TopMenuSwitch(oElement.children(ii), strClass);
	    		}
        	}
	}
function DatabaseName() {
    if ((window.event.keyCode > 96 && window.event.keyCode < 123) || (window.event.keyCode > 47 && window.event.keyCode < 58) || (window.event.keyCode == 95))
        return true;
    else
        return false;
}
//function setCookie(name, value, expires, path, domain, secure)
//{
//    if (expires != null) {
//        var today = new Date();
//        today.setTime( today.getTime() );
//        expires = new Date( today.getTime() + (expires * 1000 * 60 * 60 * 24));
//    }
//    document.cookie= name + "=" + escape(value) +
//	    ((expires) ? "; expires=" + expires.toGMTString() : "") +
//	    ((path) ? "; path=" + path : "") +
//	    ((domain) ? "; domain=" + domain : "") +
//	    ((secure) ? "; secure" : "");
//}

//function getCookie(name)
//{
//    var dc = document.cookie;
//    var prefix = name + "=";
//    var begin = dc.indexOf("; " + prefix);
//    if (begin == -1) {
//	    begin = dc.indexOf(prefix);
//	    if (begin != 0) return null;
//    }
//    else
//	    begin += 2;
//    var end = document.cookie.indexOf(";", begin);
//    if (end == -1)
//	    end = dc.length;
//    return unescape(dc.substring(begin + prefix.length, end));
//}
function CopyTextBox(oFrom, oTo) {
    oFrom = document.getElementById(oFrom);
    oTo = document.getElementById(oTo);
    oTo.value = oFrom.value;
}
var oProcessButton = null;
function ProcessButton(oButton) {
    oProcessButton = oButton;
    setTimeout("ProcessButton2()", 100);
    return true;
}
function ProcessButton2() {
    oProcessButton.value = "Processing...";
    oProcessButton.disabled = true;
}
var oWaitDiv23 = null;
function WaitDDL(oDiv) {
    oWaitDiv23 = document.getElementById(oDiv);
    oWaitDiv23.style.display = "inline";
    oWaitDiv23.style.display = "none";
    setTimeout("WaitDDL23()",50);
    return true;
}
function WaitDDL23() {
    oWaitDiv23.style.display = "inline";
}


var boolValidate = true;

function ValidateText(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && trim(oObject.value) == "" && boolValidate == true) {
        alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNumber(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && isNumber(oObject.value) == false && boolValidate == true) {
        alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}

function ValidateDropDown(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && oObject.selectedIndex == 0 && boolValidate == true) {
        alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateHidden0(oObject, oText, strAlert) {
    oObject = document.getElementById(oObject);
    oText = document.getElementById(oText);
    if (oObject != null && SetFocusTry(oText) && (trim(oObject.value) == "" || trim(oObject.value) == "0" || isNumber(oObject.value) == false) && boolValidate == true) {
        alert(strAlert);
        SetFocus(oText);
        return false;
    }
    return true;
}

function trim(str) { 
     str = str.replace( /^\s+/g, "" );
     str = str.replace( /\s+$/g, "" );
     return str; 
} 
function isNumber(str) {
   var ValidChars = "0123456789."; 
   var Char;
   for (ii = 0; ii < str.length; ii++) {
       Char = str.charAt(ii);
       if (ValidChars.indexOf(Char) == -1)
          return false;
    }
    if (str.length == 0)
        return false;
   return true;
}
function SetFocusTry(oObject) {
    var boolSuccess = false;
    try {
        oObject.focus();
        boolSuccess = true;
    }
    catch (ex) {
    }
    return boolSuccess;
}
function SetFocus(oObject) {
    try {
        oObject.focus();
    }
    catch (ex) {
        // Change Tab Divs
        var oParent = oObject.parentElement;
        var oParentDiv = null;
        while (oParent.tagName != "BODY" && oParent.className != "tabbing") {
            oParentDiv = oParent;
            oParent = oParent.parentElement;
        }
        var intParentTabNew = 1;
        var intParentTabOld = 1;
        var boolParentTabNewFound = false;
        var boolParentTabOldFound = false;
        if (oParent.className == "tabbing") {
	        for(var ii=0; ii<oParent.children.length; ii++) {
	            if (oParentDiv != oParent.children.item(ii) && boolParentTabNewFound == false)
	                intParentTabNew++;
	            else
	                boolParentTabNewFound = true;
	            if (oParent.children.item(ii).style.display == "none" && boolParentTabOldFound == false)
	                intParentTabOld++;
	            else
	                boolParentTabOldFound = true;
            }
            var oParentTab = null;
            while (oParent.tagName != "BODY" && oParentTab == null) {
    	        for(var ii=0; ii<oParent.children.length; ii++) {
	                if (oParent.children.item(ii).id == "tsmenu" + intParentTabOld)
	                    oParentTab = oParent.children.item(ii);
                }
                oParent = oParent.parentElement;
            }
            if (oParentTab != null) {
                var oTabUL = oParentTab;
                while (oTabUL.tagName != "UL")
                    oTabUL = oTabUL.firstChild
                // <ul><li id="li_tsmenu1"><a href="javascript:void(0);" onclick="NewTab('divMenu1','hdnTab',this,1);" title="Asset Information"><strong>Asset Information</strong></a></li>
                var oTabLI = oTabUL.children.item(intParentTabNew - 1);
                var oTabA = oTabLI.firstChild;
                oTabA.click();
                oObject.focus();
            }
            else
                alert('There was a problem changing the tab\nMSG: 201');
        }
    }
}
var oSelection = null;
var boolSelectionRepeat = false;
function SearchText(oText, oRepeat) {
    oText = document.getElementById(oText);
    oRepeat = document.getElementById(oRepeat);
	if (oText.value != "") {
		if (oSelection == null)
			oSelection = document.body.createTextRange();
		SearchTextNext(oText.value, oRepeat);
	}
	else {
		alert('Please enter a name to find');
	}
	return false;
}

function SearchTextNext(strText, oRepeat) {
	if (oSelection.findText(strText) == true) {
	    boolSelectionRepeat = false;
		var oParent = oSelection.parentElement();
		while ((oParent != null) && (oParent.id != oRepeat.id)) {
			oParent = oParent.parentElement;
		}
		if (oParent != null) {
			oSelection.select();
			oSelection.scrollIntoView();
			oSelection.collapse(false);
		}
		else {
			oSelection.collapse(false);
			SearchTextNext(strText, oRepeat);
		}
	}
	else if (boolSelectionRepeat == false) {
	    boolSelectionRepeat = true;
		oSelection = document.body.createTextRange();
		SearchTextNext(strText, oRepeat);
	}
	else {
		alert('The search text was not found');
		oSelection = document.body.createTextRange();
	}
}


function LoadLocationRoomRack( strType,hdnId, ctrlLocation, ctrlRoom, ctrlZone, ctrlRack)
{
   var strVar="";
   var hdnId1 = document.getElementById(hdnId);
   
   strVar=strVar+"type="+strType;
   strVar=strVar+"&Id="+hdnId1.value;
   strVar=strVar+"&hdnId="+hdnId;
   strVar=strVar+"&ctrlLocation="+ctrlLocation;
   strVar=strVar+"&ctrlRoom="+ctrlRoom;
   strVar=strVar+"&ctrlZone="+ctrlZone;
   strVar=strVar+"&ctrlRack="+ctrlRack;
   var oCallingWindow=window.parent;
   oCallingWindow=window.top;
   if (oCallingWindow!=null)
    { 
        ShowPanel(false,'/admin/frame/frame_location_room_rack.aspx?'+strVar,600,300);
    }
    else
    {
        oCallingWindow=window.opener;
        var intHeight=300;
        var intWidth=600;
        var intposLeft=0;
        var intposTop=0;
        intposLeft = ((parseInt(window.top.document.body.clientWidth) / 2) - parseInt(intWidth / 2)) + parseInt(window.top.document.body.scrollLeft);
        intposTop = ((parseInt(window.top.document.body.clientHeight) / 2) - parseInt(intHeight / 2)) + parseInt(window.top.document.body.scrollTop);
        var strPath="/admin/frame/frame_location_room_rack.aspx?"+strVar;
        window.open(strPath,"_blank","height=" + intHeight + ",width=" + intWidth + ",menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no,top="+intposTop+",left="+intposLeft);
    }
   
    return true;

}
    function ReloadAdminFrame() {
        var oOpener = window.parent.document.getElementById('frmPage');
        oOpener.contentWindow.location.reload(true);
        parent.HidePanel();
    }
