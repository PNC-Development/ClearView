using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using NCC.ClearView.Application.UI;
using NCC.ClearView.Application.UI.Entities;
using NCC.ClearView.Application.UI.BusinessLogic;

namespace NCC.ClearView.Application.UI.DataAccess
{
    public class UIDal: BaseDal
    {
        //private BaseDal db = new BaseDal(DataSource.devDSN);
        public UIDal(string connectionName) :base(connectionName)
        {
        }

        public UIDal(DataSource dataSource): base(dataSource)
        {
        }

        public UIWebPage GetUIWebPage(long? lngWebPageId, string strWebPagePath)
        {

            UIWebPage objWebPage = new UIWebPage();
            List<UIControl> lstControls = new List<UIControl>();

            ProcedureName = "pr_GetUIWebPage";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                Database.AddInParameter(dbCommand, "@WebPageId", DbType.Int64, lngWebPageId);
                Database.AddInParameter(dbCommand, "@WebPagePath", DbType.String, strWebPagePath);

                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DataRow drWebPage = ds.Tables[0].Rows[0];
                        objWebPage.WebPageId = long.Parse(drWebPage["WebPageId"].ToString());
                        objWebPage.Path = drWebPage["Path"].ToString();
                        objWebPage.Title = (drWebPage["Title"] != DBNull.Value ? drWebPage["Title"].ToString() : "");
                        objWebPage.HTMLHelp = (drWebPage["HelpHtml"] != DBNull.Value ? drWebPage["HelpHtml"].ToString() : "");
                        objWebPage.DisplayOrder = (drWebPage["DisplayOrder"] != DBNull.Value ? long.Parse(drWebPage["DisplayOrder"].ToString()) : 0);
                        objWebPage.Enabled = (drWebPage["Enabled"] != DBNull.Value ? Int32.Parse(drWebPage["Enabled"].ToString()) : 0);
                        objWebPage.Created = (drWebPage["Created"] != DBNull.Value ? DateTime.Parse(drWebPage["Created"].ToString()) : DateTime.Now);
                        objWebPage.CreatedBy = (drWebPage["CreatedBy"] != DBNull.Value ? long.Parse(drWebPage["CreatedBy"].ToString()) : 0);
                        objWebPage.Modified = (drWebPage["Modified"] != DBNull.Value ? DateTime.Parse(drWebPage["Modified"].ToString()) : DateTime.Now);
                        objWebPage.ModifiedBy = (drWebPage["ModifiedBy"] != DBNull.Value ? long.Parse(drWebPage["ModifiedBy"].ToString()) : 0);
                        objWebPage.Deleted = (drWebPage["Deleted"] != DBNull.Value ? Int32.Parse(drWebPage["Deleted"].ToString()) : 0);
                    }

                }
            }

            objWebPage.ControlList = GetUIControls(objWebPage.WebPageId, 0);
            return objWebPage;


        }

        public List<UIWebPage> GetUIWebPages()
        {
            List<UIWebPage> lstUIWebPages = new List<UIWebPage>();
            
            ProcedureName = "pr_GetUIWebPage";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                //Database.AddInParameter(dbCommand, "@WebPageId", DbType.Int64, lngWebPageId);
                //Database.AddInParameter(dbCommand, "@WebPagePath", DbType.String, strWebPagePath);

                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    foreach (DataRow drWebPage in ds.Tables[0].Rows)
                    {
                        UIWebPage objWebPage = new UIWebPage();
                        objWebPage.WebPageId = long.Parse(drWebPage["WebPageId"].ToString());
                        objWebPage.Path = drWebPage["Path"].ToString();
                        objWebPage.Title = (drWebPage["Title"] != DBNull.Value ? drWebPage["Title"].ToString() : "");
                        objWebPage.HTMLHelp = (drWebPage["HelpHtml"] != DBNull.Value ? drWebPage["HelpHtml"].ToString() : "");
                        objWebPage.DisplayOrder = (drWebPage["DisplayOrder"] != DBNull.Value ? long.Parse(drWebPage["DisplayOrder"].ToString()) : 0);
                        objWebPage.Enabled = (drWebPage["Enabled"] != DBNull.Value ? Int32.Parse(drWebPage["Enabled"].ToString()) : 0);
                        objWebPage.Created = (drWebPage["Created"] != DBNull.Value ? DateTime.Parse(drWebPage["Created"].ToString()) : DateTime.Now);
                        objWebPage.CreatedBy = (drWebPage["CreatedBy"] != DBNull.Value ? long.Parse(drWebPage["CreatedBy"].ToString()) : 0);
                        objWebPage.Modified = (drWebPage["Modified"] != DBNull.Value ? DateTime.Parse(drWebPage["Modified"].ToString()) : DateTime.Now);
                        objWebPage.ModifiedBy = (drWebPage["ModifiedBy"] != DBNull.Value ? long.Parse(drWebPage["ModifiedBy"].ToString()) : 0);
                        objWebPage.Deleted = (drWebPage["Deleted"] != DBNull.Value ? Int32.Parse(drWebPage["Deleted"].ToString()) : 0);

                        objWebPage.ControlList = GetUIControls(objWebPage.WebPageId, 0);

                        lstUIWebPages.Add(objWebPage);
                    }

                }
            }

            
            return lstUIWebPages;


        }

        public UIWebUserControl GetUIWebUserControl(long? lngWebUserControlId, string strWebUserControlPath)
        {

            UIWebUserControl objWebUserControl = new UIWebUserControl();
            List<UIControl> lstControls = new List<UIControl>();


            ProcedureName = "pr_GetUIWebUserControl";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                Database.AddInParameter(dbCommand, "@WebUserControlId", DbType.Int64, lngWebUserControlId);
                Database.AddInParameter(dbCommand, "@WebUserControlPath", DbType.String, strWebUserControlPath);

                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DataRow drWUC = ds.Tables[0].Rows[0];
                        objWebUserControl.WebUserControlId = long.Parse(drWUC["ControlId"].ToString());
                        objWebUserControl.Path = drWUC["Path"].ToString();
                        objWebUserControl.Title = (drWUC["Name"] != DBNull.Value ? drWUC["Name"].ToString() : "");
                        objWebUserControl.HTMLHelp = "";//(drWUC["HelpHtml"] != DBNull.Value ? drWUC["HelpHtml"].ToString() : "");
                        objWebUserControl.DisplayOrder = 0;// (drWUC["DisplayOrder"] != DBNull.Value ? long.Parse(drWUC["DisplayOrder"].ToString()) : 0);
                        objWebUserControl.Enabled = (drWUC["Enabled"] != DBNull.Value ? Int32.Parse(drWUC["Enabled"].ToString()) : 0);
                        objWebUserControl.Created = (drWUC["Modified"] != DBNull.Value ? DateTime.Parse(drWUC["Modified"].ToString()) : DateTime.Now);
                        objWebUserControl.CreatedBy = 0; //(drWUC["CreatedBy"] != DBNull.Value ? long.Parse(drWUC["CreatedBy"].ToString()) : 0);
                        objWebUserControl.Modified = (drWUC["Modified"] != DBNull.Value ? DateTime.Parse(drWUC["Modified"].ToString()) : DateTime.Now);
                        objWebUserControl.ModifiedBy = 0;// (drWUC["ModifiedBy"] != DBNull.Value ? long.Parse(drWUC["ModifiedBy"].ToString()) : 0);
                        objWebUserControl.Deleted = (drWUC["Deleted"] != DBNull.Value ? Int32.Parse(drWUC["Deleted"].ToString()) : 0);
                    }

                }
            }

            objWebUserControl.ControlList = GetUIControls(0, objWebUserControl.WebUserControlId);
            return objWebUserControl;


        }

        public List<UIWebUserControl> GetUIWebUserControls()
        {
            List<UIWebUserControl> lstUIWebUserControls = new List<UIWebUserControl>();
          
            ProcedureName = "pr_GetUIWebUserControl";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                //Database.AddInParameter(dbCommand, "@WebUserControlId", DbType.Int64, lngWebUserControlId);
                //Database.AddInParameter(dbCommand, "@WebUserControlPath", DbType.String, strWebUserControlPath);

                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    foreach (DataRow drWUC in ds.Tables[0].Rows)
                    {
                        UIWebUserControl objWebUserControl = new UIWebUserControl();
                        objWebUserControl.WebUserControlId = long.Parse(drWUC["ControlId"].ToString());
                        objWebUserControl.Path = drWUC["Path"].ToString();
                        objWebUserControl.Title = (drWUC["Name"] != DBNull.Value ? drWUC["Name"].ToString() : "");
                        objWebUserControl.HTMLHelp = "";//(drWUC["HelpHtml"] != DBNull.Value ? drWUC["HelpHtml"].ToString() : "");
                        objWebUserControl.DisplayOrder = 0;// (drWUC["DisplayOrder"] != DBNull.Value ? long.Parse(drWUC["DisplayOrder"].ToString()) : 0);
                        objWebUserControl.Enabled = (drWUC["Enabled"] != DBNull.Value ? Int32.Parse(drWUC["Enabled"].ToString()) : 0);
                        objWebUserControl.Created = (drWUC["Modified"] != DBNull.Value ? DateTime.Parse(drWUC["Modified"].ToString()) : DateTime.Now);
                        objWebUserControl.CreatedBy = 0; //(drWUC["CreatedBy"] != DBNull.Value ? long.Parse(drWUC["CreatedBy"].ToString()) : 0);
                        objWebUserControl.Modified = (drWUC["Modified"] != DBNull.Value ? DateTime.Parse(drWUC["Modified"].ToString()) : DateTime.Now);
                        objWebUserControl.ModifiedBy = 0;// (drWUC["ModifiedBy"] != DBNull.Value ? long.Parse(drWUC["ModifiedBy"].ToString()) : 0);
                        objWebUserControl.Deleted = (drWUC["Deleted"] != DBNull.Value ? Int32.Parse(drWUC["Deleted"].ToString()) : 0);
                        objWebUserControl.ControlList = GetUIControls(0, objWebUserControl.WebUserControlId);
                        lstUIWebUserControls.Add(objWebUserControl);
                    }

                }
            }


            return lstUIWebUserControls;


        }

        public List<UIControl> GetUIControls(long lngWebPageId, long lngWebUserControlId)
        {
            List<UIControl> lstUIControls = new List<UIControl>();

            ProcedureName = "pr_GetUIControls";
            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                Database.AddInParameter(dbCommand, "@WebPageId", DbType.Int64, lngWebPageId);
                Database.AddInParameter(dbCommand, "@WebUserControlId", DbType.Int64, lngWebUserControlId);
                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow drControl in ds.Tables[0].Rows)
                        {
                            UIControl objUIControl = new UIControl();

                            objUIControl.ControlId = long.Parse(drControl["ControlId"].ToString());
                            objUIControl.WebPageId = (drControl["WebPageId"] != DBNull.Value ? long.Parse(drControl["WebPageId"].ToString()) : 0);
                            objUIControl.WebUserControlId = (drControl["WebUserControlId"] != DBNull.Value ? long.Parse(drControl["WebUserControlId"].ToString()) : 0);
                            objUIControl.LabelName = (drControl["LabelName"] != DBNull.Value ? drControl["LabelName"].ToString() : "");
                            objUIControl.LabelText = (drControl["LabelText"] != DBNull.Value ? drControl["LabelText"].ToString() : "");
                            objUIControl.ShortName = (drControl["ShortName"] != DBNull.Value ? drControl["ShortName"].ToString() : "");
                            objUIControl.ControlName = (drControl["ControlName"] != DBNull.Value ? drControl["ControlName"].ToString() : "");
                            //Get Data Type Object
                            if (drControl["DataTypeId"] != DBNull.Value && drControl["DataTypeId"].ToString() != "0")
                                objUIControl.ControlDataType = GetUIControlDataType((drControl["DataTypeId"] != DBNull.Value ? long.Parse(drControl["DataTypeId"].ToString()) : 0));

                            objUIControl.ToolTipText = (drControl["ToolTipText"] != DBNull.Value ? drControl["ToolTipText"].ToString() : "");
                            objUIControl.HTMLHelp = (drControl["HelpHtml"] != DBNull.Value ? drControl["HelpHtml"].ToString() : "");

                            //Validation
                            objUIControl.ValidationRequired = (drControl["ValidationRequired"] != DBNull.Value ? Int32.Parse(drControl["ValidationRequired"].ToString()) : 0);
                           
                            //Get the Object of ValidationReqularExp
                            if (drControl["ValidationReqularExpId"] != DBNull.Value && drControl["ValidationReqularExpId"].ToString() != "0")
                                objUIControl.ValidationRegularExp = GetUIControlValidationRegularExp(Int32.Parse(drControl["ValidationReqularExpId"].ToString()));

                            objUIControl.ValidationCompareControl = (drControl["ValidationCompareControl"] != DBNull.Value ? drControl["ValidationCompareControl"].ToString() : "");
                            objUIControl.ValidationMinLen = (drControl["ValidationMinLen"] != DBNull.Value ? Int32.Parse(drControl["ValidationMinLen"].ToString()) : 0);
                            objUIControl.ValidationMaxLen = (drControl["ValidationMaxLen"] != DBNull.Value ? Int32.Parse(drControl["ValidationMaxLen"].ToString()) : 0);
                            objUIControl.ValidationRangeFrom = (drControl["ValidationRangeFrom"] != DBNull.Value ? drControl["ValidationRangeFrom"].ToString() : "");
                            objUIControl.ValidationRangeTo = (drControl["ValidationRangeTo"] != DBNull.Value ? drControl["ValidationRangeTo"].ToString() : "");
                            objUIControl.ValidationCustomClientSideFunction = (drControl["ValidationCustomClientSideFunction"] != DBNull.Value ? drControl["ValidationCustomClientSideFunction"].ToString() : "");
                            objUIControl.ValidationCustomServerSideFunction = (drControl["ValidationCustomServerSideFunction"] != DBNull.Value ? drControl["ValidationCustomServerSideFunction"].ToString() : "");

                            objUIControl.ValidationMsg = (drControl["ValidationMsg"] != DBNull.Value ? drControl["ValidationMsg"].ToString() : "");

                            //Validation

                            objUIControl.DisplayOrder = (drControl["DisplayOrder"] != DBNull.Value ? long.Parse(drControl["DisplayOrder"].ToString()) : 0);
                            objUIControl.Created = (drControl["Created"] != DBNull.Value ? DateTime.Parse(drControl["Created"].ToString()) : DateTime.Now);
                            objUIControl.CreatedBy = (drControl["CreatedBy"] != DBNull.Value ? long.Parse(drControl["CreatedBy"].ToString()) : 0);
                            objUIControl.Modified = (drControl["Modified"] != DBNull.Value ? DateTime.Parse(drControl["Modified"].ToString()) : DateTime.Now);
                            objUIControl.ModifiedBy = (drControl["ModifiedBy"] != DBNull.Value ? long.Parse(drControl["ModifiedBy"].ToString()) : 0);
                            objUIControl.Deleted = (drControl["Deleted"] != DBNull.Value ? Int32.Parse(drControl["Deleted"].ToString()) : 0);

                            //objUIControl.ControlValidations = GetWebPageControlsValidation(objUIControl.ControlId);
                            lstUIControls.Add(objUIControl);

                        }
                    }
                }
            }
            return lstUIControls;
        }

        public UIControl GetUIControl(long lngControlId)
        {
            UIControl objUIControl = new UIControl();

            ProcedureName = "pr_GetUIControls";
            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                Database.AddInParameter(dbCommand, "@ControlId", DbType.Int64, lngControlId);
                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DataRow drControl = ds.Tables[0].Rows[0];
                        objUIControl.ControlId = long.Parse(drControl["ControlId"].ToString());
                        objUIControl.WebPageId = (drControl["WebPageId"] != DBNull.Value ? long.Parse(drControl["WebPageId"].ToString()) : 0);
                        objUIControl.WebUserControlId = (drControl["WebUserControlId"] != DBNull.Value ? long.Parse(drControl["WebUserControlId"].ToString()) : 0);
                        objUIControl.LabelName = (drControl["LabelName"] != DBNull.Value ? drControl["LabelName"].ToString() : "");
                        objUIControl.LabelText = (drControl["LabelText"] != DBNull.Value ? drControl["LabelText"].ToString() : "");
                        objUIControl.ShortName = (drControl["ShortName"] != DBNull.Value ? drControl["ShortName"].ToString() : "");
                        objUIControl.ControlName = (drControl["ControlName"] != DBNull.Value ? drControl["ControlName"].ToString() : "");
                        //Get Data Type Object
                        if (drControl["DataTypeId"] != DBNull.Value && drControl["DataTypeId"].ToString() != "0")
                            objUIControl.ControlDataType = GetUIControlDataType((drControl["DataTypeId"] != DBNull.Value ? long.Parse(drControl["DataTypeId"].ToString()) : 0));

                        objUIControl.ToolTipText = (drControl["ToolTipText"] != DBNull.Value ? drControl["ToolTipText"].ToString() : "");
                        objUIControl.HTMLHelp = (drControl["HelpHtml"] != DBNull.Value ? drControl["HelpHtml"].ToString() : "");

                        //Validation
                        objUIControl.ValidationRequired = (drControl["ValidationRequired"] != DBNull.Value ? Int32.Parse(drControl["ValidationRequired"].ToString()) : 0);

                        //Get the Object of ValidationReqularExp
                        if (drControl["ValidationReqularExpId"] != DBNull.Value && drControl["ValidationReqularExpId"].ToString() != "0")
                            objUIControl.ValidationRegularExp = GetUIControlValidationRegularExp(Int32.Parse(drControl["ValidationReqularExpId"].ToString()));

                        objUIControl.ValidationCompareControl = (drControl["ValidationCompareControl"] != DBNull.Value ? drControl["ValidationCompareControl"].ToString() : "");
                        objUIControl.ValidationMinLen = (drControl["ValidationMinLen"] != DBNull.Value ? Int32.Parse(drControl["ValidationMinLen"].ToString()) : 0);
                        objUIControl.ValidationMaxLen = (drControl["ValidationMaxLen"] != DBNull.Value ? Int32.Parse(drControl["ValidationMaxLen"].ToString()) : 0);
                        objUIControl.ValidationRangeFrom = (drControl["ValidationRangeFrom"] != DBNull.Value ? drControl["ValidationRangeFrom"].ToString() : "");
                        objUIControl.ValidationRangeTo = (drControl["ValidationRangeTo"] != DBNull.Value ? drControl["ValidationRangeTo"].ToString() : "");
                        objUIControl.ValidationCustomClientSideFunction = (drControl["ValidationCustomClientSideFunction"] != DBNull.Value ? drControl["ValidationCustomClientSideFunction"].ToString() : "");
                        objUIControl.ValidationCustomServerSideFunction = (drControl["ValidationCustomServerSideFunction"] != DBNull.Value ? drControl["ValidationCustomServerSideFunction"].ToString() : "");

                        objUIControl.ValidationMsg = (drControl["ValidationMsg"] != DBNull.Value ? drControl["ValidationMsg"].ToString() : "");

                   
                        //Validation

                        objUIControl.DisplayOrder = (drControl["DisplayOrder"] != DBNull.Value ? long.Parse(drControl["DisplayOrder"].ToString()) : 0);
                        objUIControl.Created = (drControl["Created"] != DBNull.Value ? DateTime.Parse(drControl["Created"].ToString()) : DateTime.Now);
                        objUIControl.CreatedBy = (drControl["CreatedBy"] != DBNull.Value ? long.Parse(drControl["CreatedBy"].ToString()) : 0);
                        objUIControl.Modified = (drControl["Modified"] != DBNull.Value ? DateTime.Parse(drControl["Modified"].ToString()) : DateTime.Now);
                        objUIControl.ModifiedBy = (drControl["ModifiedBy"] != DBNull.Value ? long.Parse(drControl["ModifiedBy"].ToString()) : 0);
                        objUIControl.Deleted = (drControl["Deleted"] != DBNull.Value ? Int32.Parse(drControl["Deleted"].ToString()) : 0);
                    }
                }
            }
            return objUIControl;
        }

        public void UpdateUIControl(UIControl oUIControl)
        { 
            ProcedureName = "pr_updateUIControl";
            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                Database.AddInParameter(dbCommand, "@ControlId", DbType.Int64, oUIControl.ControlId);
                Database.AddInParameter(dbCommand, "@LabelName", DbType.String, oUIControl.LabelName);
                Database.AddInParameter(dbCommand, "@LabelText", DbType.String, oUIControl.LabelText);
                Database.AddInParameter(dbCommand, "@ShortName", DbType.String, oUIControl.ShortName);
                Database.AddInParameter(dbCommand, "@ControlName", DbType.String, oUIControl.ControlName);
                Database.AddInParameter(dbCommand, "@ToolTipText", DbType.String, oUIControl.ToolTipText);
                Database.AddInParameter(dbCommand, "@HelpHtml", DbType.String, oUIControl.HTMLHelp);
                if (oUIControl.ControlDataType!=null)
                Database.AddInParameter(dbCommand, "@DataTypeId", DbType.Int64, oUIControl.ControlDataType.DataTypeId);

                Database.AddInParameter(dbCommand, "@ValidationRequired", DbType.Int64, oUIControl.ValidationRequired);

                if (oUIControl.ValidationRegularExp != null)
                Database.AddInParameter(dbCommand, "@ValidationReqularExpId", DbType.Int64, oUIControl.ValidationRegularExp.RegularExpId);
                
                Database.AddInParameter(dbCommand, "@ValidationCompareControl", DbType.String, oUIControl.ValidationCompareControl);
                Database.AddInParameter(dbCommand, "@ValidationMinLen", DbType.Int64, oUIControl.ValidationMinLen);
                Database.AddInParameter(dbCommand, "@ValidationMaxLen", DbType.Int64, oUIControl.ValidationMaxLen);
                Database.AddInParameter(dbCommand, "@ValidationRangeFrom", DbType.String, oUIControl.ValidationRangeFrom);
                Database.AddInParameter(dbCommand, "@ValidationRangeTo", DbType.String, oUIControl.ValidationRangeTo);
                Database.AddInParameter(dbCommand, "@ValidationCustomClientSideFunction", DbType.String, oUIControl.ValidationCustomClientSideFunction);
                Database.AddInParameter(dbCommand, "@ValidationCustomServerSideFunction", DbType.String, oUIControl.ValidationCustomServerSideFunction);
                Database.AddInParameter(dbCommand, "@ValidationMsg", DbType.String, oUIControl.ValidationMsg);
                Database.AddInParameter(dbCommand, "@DisplayOrder", DbType.Int64, oUIControl.DisplayOrder);
                Database.AddInParameter(dbCommand, "@ModifiedBy", DbType.Int64, oUIControl.ModifiedBy);
                Database.AddInParameter(dbCommand, "@Deleted", DbType.Int32, oUIControl.Deleted);
                ExecuteNonQuery(dbCommand);
            }
                
        
        }

        public UIControlDataType GetUIControlDataType(long lngDataTypeId)
        {


            UIControlDataType objDataType = new UIControlDataType();

            ProcedureName = "pr_GetUIControlDataType";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                Database.AddInParameter(dbCommand, "@DataTypeId", DbType.Int64, lngDataTypeId);
                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DataRow drDataType = ds.Tables[0].Rows[0];
                        objDataType.DataTypeId = long.Parse(drDataType["DataTypeId"].ToString());
                        objDataType.Name = drDataType["Name"].ToString();
                        objDataType.Created = (drDataType["Created"] != DBNull.Value ? DateTime.Parse(drDataType["Created"].ToString()) : DateTime.Now);
                        objDataType.CreatedBy = (drDataType["CreatedBy"] != DBNull.Value ? long.Parse(drDataType["CreatedBy"].ToString()) : 0);
                        objDataType.Modified = (drDataType["Modified"] != DBNull.Value ? DateTime.Parse(drDataType["Modified"].ToString()) : DateTime.Now);
                        objDataType.ModifiedBy = (drDataType["ModifiedBy"] != DBNull.Value ? long.Parse(drDataType["ModifiedBy"].ToString()) : 0);
                        objDataType.Deleted = (drDataType["Deleted"] != DBNull.Value ? Int32.Parse(drDataType["Deleted"].ToString()) : 0);
                    }

                }
            }

            return objDataType;
        }

        public List<UIControlDataType> GetUIControlDataTypes()
        {

            List<UIControlDataType> oUIControlDataTypes = new List<UIControlDataType>(); 
          
            ProcedureName = "pr_GetUIControlDataType";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                //Database.AddInParameter(dbCommand, "@DataTypeId", DbType.Int64, lngDataTypeId);
                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                     foreach (DataRow drDataType in ds.Tables[0].Rows)
                    {
                        UIControlDataType objDataType = new UIControlDataType();
                        
                         objDataType.DataTypeId = long.Parse(drDataType["DataTypeId"].ToString());
                        objDataType.Name = drDataType["Name"].ToString();
                        objDataType.Created = (drDataType["Created"] != DBNull.Value ? DateTime.Parse(drDataType["Created"].ToString()) : DateTime.Now);
                        objDataType.CreatedBy = (drDataType["CreatedBy"] != DBNull.Value ? long.Parse(drDataType["CreatedBy"].ToString()) : 0);
                        objDataType.Modified = (drDataType["Modified"] != DBNull.Value ? DateTime.Parse(drDataType["Modified"].ToString()) : DateTime.Now);
                        objDataType.ModifiedBy = (drDataType["ModifiedBy"] != DBNull.Value ? long.Parse(drDataType["ModifiedBy"].ToString()) : 0);
                        objDataType.Deleted = (drDataType["Deleted"] != DBNull.Value ? Int32.Parse(drDataType["Deleted"].ToString()) : 0);
                        oUIControlDataTypes.Add(objDataType);
                    }

                }
            }

            return oUIControlDataTypes;
        }

        public UIControlValidationRegularExp GetUIControlValidationRegularExp(int intRegularExpId)
        {


            UIControlValidationRegularExp objValidationRegExp = new UIControlValidationRegularExp();

            ProcedureName = "pr_GetUIControlValidationRegularExp";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
                Database.AddInParameter(dbCommand, "@RegularExpId", DbType.Int32, intRegularExpId);
                using (DataSet ds = ExecuteDataSet(dbCommand))
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DataRow drRegExp = ds.Tables[0].Rows[0];
                        objValidationRegExp.RegularExpId = Int32.Parse(drRegExp["RegularExpId"].ToString());
                        objValidationRegExp.Name = drRegExp["Name"].ToString();
                        objValidationRegExp.RegularExp = drRegExp["RegularExp"].ToString();
                        objValidationRegExp.DefaultMsg = drRegExp["DefaultMsg"].ToString();
                        objValidationRegExp.Created = (drRegExp["Created"] != DBNull.Value ? DateTime.Parse(drRegExp["Created"].ToString()) : DateTime.Now);
                        objValidationRegExp.CreatedBy = (drRegExp["CreatedBy"] != DBNull.Value ? long.Parse(drRegExp["CreatedBy"].ToString()) : 0);
                        objValidationRegExp.Modified = (drRegExp["Modified"] != DBNull.Value ? DateTime.Parse(drRegExp["Modified"].ToString()) : DateTime.Now);
                        objValidationRegExp.ModifiedBy = (drRegExp["ModifiedBy"] != DBNull.Value ? long.Parse(drRegExp["ModifiedBy"].ToString()) : 0);
                        objValidationRegExp.Deleted = (drRegExp["Deleted"] != DBNull.Value ? Int32.Parse(drRegExp["Deleted"].ToString()) : 0);
                    }

                }
            }

            return objValidationRegExp;
        }

        public List<UIControlValidationRegularExp> GetUIControlValidationRegularExps()
        {
            List<UIControlValidationRegularExp> oUIControlValidationRegularExps = new List<UIControlValidationRegularExp>(); 

           

            ProcedureName = "pr_GetUIControlValidationRegularExp";

            using (DbCommand dbCommand = Database.GetStoredProcCommand(ProcedureName))
            {
               // Database.AddInParameter(dbCommand, "@RegularExpId", DbType.Int32, intRegularExpId);
                using (DataSet ds = ExecuteDataSet(dbCommand))
                { 
                    foreach (DataRow drRegExp in ds.Tables[0].Rows)
                    {
                        UIControlValidationRegularExp objValidationRegExp = new UIControlValidationRegularExp();
                        
                        objValidationRegExp.RegularExpId = Int32.Parse(drRegExp["RegularExpId"].ToString());
                        objValidationRegExp.Name = drRegExp["Name"].ToString();
                        objValidationRegExp.RegularExp = drRegExp["RegularExp"].ToString();
                        objValidationRegExp.DefaultMsg = drRegExp["DefaultMsg"].ToString();
                        objValidationRegExp.Created = (drRegExp["Created"] != DBNull.Value ? DateTime.Parse(drRegExp["Created"].ToString()) : DateTime.Now);
                        objValidationRegExp.CreatedBy = (drRegExp["CreatedBy"] != DBNull.Value ? long.Parse(drRegExp["CreatedBy"].ToString()) : 0);
                        objValidationRegExp.Modified = (drRegExp["Modified"] != DBNull.Value ? DateTime.Parse(drRegExp["Modified"].ToString()) : DateTime.Now);
                        objValidationRegExp.ModifiedBy = (drRegExp["ModifiedBy"] != DBNull.Value ? long.Parse(drRegExp["ModifiedBy"].ToString()) : 0);
                        objValidationRegExp.Deleted = (drRegExp["Deleted"] != DBNull.Value ? Int32.Parse(drRegExp["Deleted"].ToString()) : 0);
                        oUIControlValidationRegularExps.Add(objValidationRegExp);
                    }

                }
            }

            return oUIControlValidationRegularExps;
        }
    }
}
