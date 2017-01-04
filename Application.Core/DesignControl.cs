using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NCC.ClearView.Application.Core
{
	public class DesignControl : System.Web.UI.UserControl
	{
		private int intDesign;
        private bool boolValid;
        private bool boolDemo;
        private HtmlGenericControl panInvalid;
        private HtmlGenericControl panValid;
        private HtmlGenericControl panReject;
        private HtmlGenericControl panException;
        private RadioButton radButton1;
        private RadioButton radButton2;
        private RadioButton radException;
        private Label lblValid;
        private Button btnSubmit;
        private int intExceptionServiceFolder;
        private Panel panExceptionServiceFolder;

        public DesignControl() { }

		public int DesignId
		{
            get { return intDesign; }
            set { intDesign = value; }
		}
        public bool Valid
        {
            get { return boolValid; }
            set { boolValid = value; }
        }
        public bool Demo
        {
            get { return boolDemo; }
            set { boolDemo = value; }
        }
        public HtmlGenericControl InvalidPanel
        {
            get { return panInvalid; }
            set { panInvalid = value; }
        }
        public HtmlGenericControl ValidPanel
        {
            get { return panValid; }
            set { panValid = value; }
        }
        public HtmlGenericControl RejectPanel
        {
            get { return panReject; }
            set { panReject = value; }
        }
        public HtmlGenericControl ExceptionPanel
        {
            get { return panException; }
            set { panException = value; }
        }
        public Label ValidationLabel
        {
            get { return lblValid; }
            set { lblValid = value; }
        }
        public RadioButton CompleteRadio
        {
            get { return radButton1; }
            set { radButton1 = value; }
        }
        public RadioButton ScheduleRadio
        {
            get { return radButton2; }
            set { radButton2 = value; }
        }
        public int ExceptionServiceFolder
        {
            get { return intExceptionServiceFolder; }
            set { intExceptionServiceFolder = value; }
        }
        public Panel ExceptionServiceFolderPanel
        {
            get { return panExceptionServiceFolder; }
            set { panExceptionServiceFolder = value; }
        }
        public RadioButton ExceptionRadio
        {
            get { return radException; }
            set { radException = value; }
        }
    }
}
