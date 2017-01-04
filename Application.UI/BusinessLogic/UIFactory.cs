using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NCC.ClearView.Application.UI.Entities;
using NCC.ClearView.Application.UI.DataAccess;
namespace NCC.ClearView.Application.UI.BusinessLogic
{
    public class UIFactory 
    {

        //string strWebPagePath = "";
        private UIDal objUIDal = new UIDal(ConfigurationManager.AppSettings["DSN"].ToString());
        
        public UIFactory()
        {
           

        }

        public UIWebPage GetUIWebPage(string strWebPagePath)
        {
            
            return objUIDal.GetUIWebPage(0, strWebPagePath);
        }

        public List<UIWebPage> GetUIWebPages()
        {

            return objUIDal.GetUIWebPages();
        }

        public UIWebUserControl GetUIWebUserControl(string strWebUserControlPath)
        {
            
            return objUIDal.GetUIWebUserControl(0, strWebUserControlPath);
        }

        public List<UIWebUserControl> GetUIWebUserControls()
        {
            return objUIDal.GetUIWebUserControls();
        }

        public UIControl GetUIControl(long lngControlId)
        {
            
            return objUIDal.GetUIControl(lngControlId);
        }


        public UIControlDataType GetUIControlDataType(long lngDataTypeId)
        {
            return objUIDal.GetUIControlDataType(lngDataTypeId);
           
        }

        public void UpdateUIControl(UIControl oUIControl)
        {
            objUIDal.UpdateUIControl(oUIControl);
        }
        public List<UIControlDataType> GetUIControlDataTypes()
        {
            return objUIDal.GetUIControlDataTypes();

        }
        public UIControlValidationRegularExp GetUIControlValidationRegularExp(int intRegularExpId)
        {
            return objUIDal.GetUIControlValidationRegularExp(intRegularExpId);
        
        }

        public List<UIControlValidationRegularExp> GetUIControlValidationRegularExps()
        {
            return objUIDal.GetUIControlValidationRegularExps();

        }
    }
}
