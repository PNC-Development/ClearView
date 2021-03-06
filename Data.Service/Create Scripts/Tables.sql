USE [ClearViewService]
GO
/****** Object:  Table [dbo].[cvs_change]    Script Date: 07/31/2009 13:36:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cvs_change](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_number] [varchar](25) NULL,
	[implementor_name] [varchar](50) NULL,
	[implementor_dept] [varchar](100) NULL,
	[addtl_implementor_name] [varchar](300) NULL,
	[severity] [int] NULL,
	[category] [varchar](50) NULL,
	[change_what] [varchar](50) NULL,
	[change_how] [varchar](50) NULL,
	[priority] [varchar](15) NULL,
	[portfolio_ncc] [varchar](50) NULL,
	[business_area_ncc] [varchar](50) NULL,
	[change_startdatetime] [datetime] NULL,
	[description] [text] NULL,
	[change_enddatetime] [datetime] NULL,
	[close_datetime] [datetime] NULL,
	[completion_code] [varchar](25) NULL,
	[num_incidents] [int] NULL,
	[open_time] [datetime] NULL,
	[approval_groups] [varchar](50) NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cvs_change] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cvs_incident]    Script Date: 07/31/2009 13:36:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cvs_incident](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[incident_number] [varchar](25) NULL,
	[description_brief] [text] NULL,
	[status] [varchar](25) NULL,
	[open_datetime] [datetime] NULL,
	[close_datetime] [datetime] NULL,
	[assigned_group] [varchar](50) NULL,
	[severity] [int] NULL,
	[assignee_name] [varchar](25) NULL,
	[contact_name] [varchar](50) NULL,
	[category] [varchar](50) NULL,
	[subcategory] [varchar](50) NULL,
	[component] [varchar](100) NULL,
	[change_control] [varchar](25) NULL,
	[cause_code] [varchar](50) NULL,
	[ack] [varchar](30) NULL,
	[description_other_ack] [text] NULL,
	[closed_by] [varchar](25) NULL,
	[sla_status] [varchar](25) NULL,
	[count_ack] [int] NULL,
	[non_ack_placeholder] [varchar](25) NULL,
	[total_nonack] [int] NULL,
	[isack] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cvs_incident] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF