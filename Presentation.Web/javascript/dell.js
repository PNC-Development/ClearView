    var xmlRequestObject;
    var urlDRAC;
    
    
    function specialEncode(str) {
        var tmp = "";
        var encoded = "";
        for (var n = 0; n < str.length; n++) {
            var c = str.charCodeAt(n);
            tmp = String.fromCharCode(c);

            if(c == 167) {
                encoded += escape(tmp);
            } else {
                encoded += encodeURIComponent(tmp);
            }
        }
        return encoded;
    }
    
    
    function sendPost( reqUrl, postData, renderPageCallback, strDracURL )
    {
        urlDRAC = strDracURL;
      if( renderPageCallback != null )
          document.chainedCallback = renderPageCallback;
      else
          document.chainedCallback = setFieldListFromXML
      loadXMLDocument( reqUrl, waitWithCallback, postData );
        //alert('done with sendPost');
    }
    
    
     function loadXMLDocument( url, callback, postData ) 
    {
        var xDoc;
        //
        if( window.XMLHttpRequest ) 
        {
            xmlRequestObject = new XMLHttpRequest();
        } 
        else if( window.ActiveXObject )
        //     if( window.ActiveXObject )  
        {
            try 
            {
                xmlRequestObject = new ActiveXObject("Microsoft.XMLHTTP");
                //xmlRequestObject = new ActiveXObject("Msxml2.XMLHTTP");
                //alert("XMLHttpRequest is executed"); 
            }
            catch ( e ) 
            {
                try 
                {
                    xmlRequestObject = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch ( E ) 
                {    
                }
            }
         }
         //
         if( xmlRequestObject ) 
         {
             xmlRequestObject.onreadystatechange = callback;
            // Causes security warning...
             xmlRequestObject.open("POST", url, true );
             //alert('done with post: ' + url);
             if( postData == null )
                 postData = '';
             xmlRequestObject.setRequestHeader("Content-Type", "application/x-www-form-urlencoded" );
             //alert('sending data: ' + postData);
             xmlRequestObject.send( postData );
             //alert('done with data send');
         }
            //alert('done with loadXMLDocument');
    }
    
    
    function loginRequestChange() {
        // Only do something if req shows that the response was loaded.
        var errorMessage;
        if (xmlRequestObject.readyState == 4) {
            // Was the requests sucessfull?
            if (xmlRequestObject.status == 200) {
                var xmlDoc = xmlRequestObject.responseXML;
                if( xmlDoc == null ) {
                    errorMessage = badResponseMsg;
                } else {                        
                    //  Did we get a valid response?
                    var reqStatus = getXMLValue( xmlDoc, 'status' );
                    if( reqStatus != 'ok' ) {
                        errorMessage = getXMLValue( xmlDoc, 'errorMessage' );
                    } 
                    else {
                        var authResult = getXMLValue( xmlDoc, 'authResult' );
                        if (authResult != 0) { errorMessage = "Invalid Login"; } 
                        else {
                            var url = getXMLValue( xmlDoc, "forwardUrl" );
                            //window.navigate(window.location.toString() + "&showimage=true");
                            onLoadConsolePrevImage(1);
                            //document.location = url;
                        }
                    }
                }
            } else
                errorMessage = commFailedMsg;
            //
            if( errorMessage != null ) {
                alert('Error:' + errorMessage);
            }
        }
        //alert('done with loginRequestChange');
    }


    function getXMLValue( xmlDoc, elementName )
    {
        //alert("getXMLValue( xmlDoc, " + elementName + ");");
        var rtn = "";
        var i;
        //
        if( xmlDoc == null || xmlDoc.childNodes.length == 0 )
        {
          alert("Received bad XML document.");
        }
        else 
        {
           var elements = xmlDoc.getElementsByTagName( elementName );
           //
           if( elements != null && elements.length > 0  && elements[0].childNodes != null  )
           {
               if( elements[0].childNodes.length == 0 )
                   return null;
               else if( elements[0].childNodes[0].nodeType == 3){
                    var j = elements[0].childNodes.length;
                    for (i = 0; i < j; i++){
                        rtn = rtn + elements[0].childNodes[i].nodeValue;
                    }
                    //Fix for FireFox. Long returns are broken up into 4096 byte child nodes.  IE is all in one.
                    //alert("rtn len=" + rtn.length + "ChildNode count=" + j + "\n" );
               }
               else {
                   rtn = elements[0];
		       }
           }
        }
       return rtn
     }

    function waitWithCallback()
    {
        //Only execute for state "loaded", all other states have 
        //  no processing.
        if (xmlRequestObject.readyState == 4)
        {
            // only if "OK"
            if (xmlRequestObject.status == 200 )
            {
                //alert('200');
                var xmlDoc = xmlRequestObject.responseXML;
                var reqStatus = getXMLValue( xmlDoc, 'status' );
                if( reqStatus != 'ok' )
                {
                    var message = getXMLValue( xmlDoc, 'message' );
                    //alert(" Request failed: " + message );
                    //If we fail perform the callback with a null doc 
                    //  to signal the chainedCallback that the server'
                    //  did not recognize the request
                    document.chainedCallback(null);
                }
                else
                {
                    //It might be wise at somepoint to implement 
                    //  chainedCallback as a stack to avoid accidentally
                    //  stepping on a callback by overwriting it with
                    //  a value that hasn't been called back yet.  This
                    //  would introduce substantial complexity though...
                    document.chainedCallback(xmlDoc);
                    /*
                    if( requestCtxt.updateComplete != null)
                    {
                        requestCtxt.updateComplete( requestCtxt, xmlDoc );
                    }
                    */
                }
            }
            else if( xmlRequestObject.status == 401 )
            {
                alert('login');
                document.location = urlDRAC + "/login.html";
            }
            else
            {
                //showErrorMessage(" Could not retrieve data from server ( status=" +
                //                                 xmlRequestObject.status + ", " + xmlRequestObject.statusText + ")" );
                alert(xmlRequestObject.status);
                alert(xmlRequestObject.statusText);
                showContentPanel();
            }
        }
    } //end of waitWithCallback


    function showContentPanel()
    {
    }








    var ver = getInternetExplorerVersion();
function getInternetExplorerVersion()
// Returns the version of Internet Explorer or a -1
// (indicating the use of another browser).
{
  var rv = -1; // Return value assumes failure.
  if (navigator.appName == 'Microsoft Internet Explorer')
  {
    var ua = navigator.userAgent;
    var re  = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
    if (re.exec(ua) != null)
      rv = parseFloat( RegExp.$1 );
  }
  return rv;
}
    var isPrvLoad = false;
	function loadConsolePrvCallback(xmlDoc) {
        // Put a + new Date() on the end so it's always a new image
        window.navigate(window.location.toString() + "&showimage=" + "/capconsole/scapture0.png" + '?' + (new Date()).getTime());        
        //var prvObj = document["console_preview_img_id"];
        //prvObj.src = urlDRAC + "/capconsole/scapture0.png" + '?' + (new Date()).getTime();
        //prvObj.width = 400;
        //prvObj.height = 300;
        isPrvLoad = false;
        return true;
    }
    function imageLoadError(source) {
	    // Put a + new Date() on the end so it's always a new image
        source.src = "/images/noImage.gif" + '?' + (new Date()).getTime();
        // disable onerror to prevent endless loop
        source.onerror = "/images/noImage.gif";
        return true;
    }
	    var isPrvLoad = false;

    function onLoadConsolePrevImage(bManualRfr) {
          if(isPrvLoad) {
             // Allows only one image load at a time.
             return false;
          }
          isPrvLoad = true;
 
          if(bManualRfr != 1) {
	      //
	      // Do not load image only if Power is OFF
	      //
	      var pwstateOff   = "OFF"; //OFF
	      var pwstateUnknown = "UNKNOWN"; // UNKNOWN

	      var pwstate = document.getElementById('pwState').innerHTML;
	      if(pwstate != null) {
	         if(pwstate == pwstateUnknown) {
	            // initial page load
	            loadConsolePrvCallback();
	            return false;
	         } else if(pwstate == pwstateOff) {
	            loadConsolePrvCallback();
	            //isPrvLoad = false;
	            return false;
	         }
	      }
          }

	   var reqUrl = urlDRAC + "/data?get=consolepreview[auto " + (new Date()).getTime() +"]";
	   if(bManualRfr != null && bManualRfr == 1) {
	      //alert('rfr: ' + bManualRfr);
              reqUrl = urlDRAC + "/data?get=consolepreview[manual " + (new Date()).getTime() +"]";
	   }
	   
	   // document.chainedCallback = loadConsolePrvCallback;
           //syncAjaxLoad( reqUrl, waitWithCallback );
           syncAjaxLoad( reqUrl, bManualRfr );
	}
    function syncAjaxLoad( url, bManualRfr ) 
    {
        if( window.XMLHttpRequest ) {
           xmlObj = new XMLHttpRequest();
        } 
        else if( window.ActiveXObject ) {
           try {
              xmlObj = new ActiveXObject("Msxml2.XMLHTTP");
           } catch ( e ) {
              try {
                xmlObj = new ActiveXObject("Microsoft.XMLHTTP");
              }
              catch ( E ) {  }
           }
        }
		
        if( xmlObj ) {
           //In FF the onreadystatechange event is not fired on a synchronous call and is really not even needed 
		   //at all in IE either (though it does fire). So for a synchrounous call you could have a function like this:

	   xmlObj.onreadystatechange= function(){
		if(xmlObj.readyState == 4){
			if (xmlObj.status == 200) {				
                               loadConsolePrvCallback();
			} else {
				alert("There was a problem retrieving the XML data (" + xmlObj.status + ")");
				return false;
			}
     	} 
	}

           xmlObj.open("GET", url, false);
           if(bManualRfr != null && bManualRfr == 1) {
              // manual refresh
              xmlObj.setRequestHeader("idracAutoRefresh", "0");
           } else {
              // auto refresh
              xmlObj.setRequestHeader("idracAutoRefresh", "1" );
           }
           xmlObj.send( null );

           // FF work around.
           // This will not be called until xmlObj.send() returns.
		   if ( ver > -1 ) {
		      // do nothing, handled by onreadystatechange event
		   } else {
              loadConsolePrvCallback();
		   }
        }
    }
