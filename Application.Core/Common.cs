using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.WebControls;

namespace NCC.ClearView.Application.Core
{
	public class Common
	{
        public Common()
		{
		}

        public void AddAttribute(WebControl oControl, string _event, string _action)
        {
            if (oControl.Attributes[_event] == null || oControl.Attributes[_event] == "")
                oControl.Attributes.Add(_event, _action);
            else
            {
                string strOldFunction = oControl.Attributes[_event];
                oControl.Attributes.Add(_event, strOldFunction + _action);
            }
        }
    }
}
