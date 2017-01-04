    function BrowserDetect() 
    {
        var browser=navigator.appName;
        var b_version = navigator.appVersion;
        var b_agent = navigator.userAgent;
        var version = parseFloat(b_version);
        var IsIE = false;
        if ((browser != "Microsoft Internet Explorer") || (version < 4)) {
            //alert("Not IE");
            if (b_agent.indexOf('Trident/') != -1) {
                IsIE = true;
                //alert("Is IE");
                version = b_agent.substring(b_agent.indexOf('rv:') + 3);
                // trim the version string
                if ((ix = version.indexOf(';')) != -1) version = version.substring(0, ix);
                if ((ix = version.indexOf(' ')) != -1) version = version.substring(0, ix);
                if ((ix = version.indexOf(')')) != -1) version = version.substring(0, ix);
                //alert(version);

                version = parseInt('' + version, 10);
                //alert(version);
            }
        }
        else
            IsIE = true;
        if (IsIE == false) 
        {
            //alert('Still Not IE');
            document.write("<table cellpadding=\"4\" cellspacing=\"3\" border=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:14px; color:#404040\">");
            document.write("<tr>");
            document.write("<td><img src=\"/images/bigError.gif\" border=\"0\" align=\"absmiddle\"/> <b>ClearView Requires Microsoft Internet Explorer Version 4.0 or Later to Run!</b></td>");
            document.write("</tr>");
            document.write("<tr>");
            document.write("<td>Please close this browser and open Internet Explorer to use ClearView</td>");
            document.write("</tr>");
            document.write("<tr>");
            document.write("<td>Current Browser Information:</td>");
            document.write("</tr>");
            document.write("<tr>");
            document.write("<td>Browser name: " + browser + " (Version " + version + ")</td>");
            document.write("</tr>");
            document.write("</table>");
        }
    }
