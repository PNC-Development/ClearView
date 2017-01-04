using System;
using System.Web;

namespace NCC.ClearView.Application.Core
{
	public class CMControl : System.Web.UI.UserControl
	{
		private int intSchema;
		private int intPage;
		private int intTemplateControl;
		private int intPageControl;

		public CMControl() { }

		public int PageId
		{
			get { return intPage; }
			set { intPage = value; }
		}
		public int SchemaId
		{
			get { return intSchema; }
			set { intSchema = value; }
		}
		public int TemplateCId
		{
			get { return intTemplateControl; }
			set { intTemplateControl = value; }
		}
		public int PageCId
		{
			get { return intPageControl; }
			set { intPageControl = value; }
		}
	}
}
