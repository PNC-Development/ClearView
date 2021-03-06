USE [Clearview]
GO
/****** Object:  Table [dbo].[cv_account_maintenance]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_account_maintenance](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[maintenance] [varchar](10) NULL,
	[username] [varchar](10) NULL,
	[domain] [int] NULL,
	[approval] [int] NULL,
	[reason] [text] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_account_maintenance_parameters]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_account_maintenance_parameters](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[value] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_account_maintenance_parameters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_account_request_approval]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_account_request_approval](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[domainid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_account_request_approval] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_account_request_exceptions]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_account_request_exceptions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[domainid] [int] NULL,
	[exact] [int] NULL,
	[starts] [int] NULL,
	[ends] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_account_request_exceptions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_account_requests]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_account_requests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[xid] [varchar](10) NULL,
	[domain] [int] NULL,
	[adgroups] [text] NULL,
	[localgroups] [text] NULL,
	[email] [int] NULL,
	[approval] [int] NULL,
	[completed] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_account_requests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_active_directory_groups]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_active_directory_groups](
	[name] [varchar](200) NULL,
	[domainid] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ad_objects]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ad_objects](
	[adid] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[environment] [int] NULL,
	[name] [varchar](100) NULL,
	[path] [varchar](300) NULL,
	[reason] [varchar](200) NULL,
	[objectdate] [datetime] NULL,
	[done_on] [datetime] NULL,
	[done_by] [int] NULL,
	[disabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ad_objects] PRIMARY KEY CLUSTERED 
(
	[adid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_app_pages]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_app_pages](
	[apppageid] [int] IDENTITY(1,1) NOT NULL,
	[pageid] [int] NULL,
	[applicationid] [int] NULL,
	[refresh] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_app_pages] PRIMARY KEY CLUSTERED 
(
	[apppageid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_applications]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_applications](
	[applicationid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[url] [varchar](50) NULL,
	[service_title] [varchar](100) NULL,
	[orgchart] [varchar](100) NULL,
	[description] [text] NULL,
	[image] [varchar](100) NULL,
	[userid] [int] NULL,
	[parent] [int] NULL,
	[priority1] [int] NULL,
	[priority2] [int] NULL,
	[tpm] [int] NULL,
	[disable_manager] [int] NULL,
	[manager_approve] [int] NULL,
	[platform_approve] [int] NULL,
	[deliverables_doc] [varchar](100) NULL,
	[lead1] [int] NULL,
	[lead2] [int] NULL,
	[lead3] [int] NULL,
	[lead4] [int] NULL,
	[lead5] [int] NULL,
	[approve_vacation] [int] NULL,
	[employees_needed] [int] NULL,
	[service_search_items] [int] NULL,
	[send_reminders] [int] NULL,
	[request_items] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_applications] PRIMARY KEY CLUSTERED 
(
	[applicationid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_banks]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_banks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_banks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_build_locations]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_build_locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[build_classid] [int] NULL,
	[build_environmentid] [int] NULL,
	[build_addressid] [int] NULL,
	[modelid] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_build_locations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_build_locations_rdp]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_build_locations_rdp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[classid] [int] NULL,
	[addressid] [int] NULL,
	[rdp_schedule_ws] [varchar](100) NULL,
	[rdp_computer_ws] [varchar](100) NULL,
	[blade_vlan] [varchar](50) NULL,
	[vmware_vlan] [varchar](50) NULL,
	[source] [varchar](30) NULL,
	[workstation] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_build_locations_rdp] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_c101_upload]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_c101_upload](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[class] [varchar](100) NULL,
	[projectid] [varchar](100) NULL,
	[projectname] [varchar](100) NULL,
	[ce] [varchar](50) NULL,
	[status] [varchar](100) NULL,
	[finalstatus] [varchar](100) NULL,
	[stage] [varchar](100) NULL,
	[category] [varchar](50) NULL,
	[portfolio] [varchar](100) NULL,
	[segment] [varchar](100) NULL,
	[appcode] [varchar](20) NULL,
	[sponsor] [varchar](100) NULL,
	[bio] [varchar](100) NULL,
	[sio] [varchar](100) NULL,
	[groupmanager] [varchar](100) NULL,
	[fm] [varchar](100) NULL,
	[pm] [varchar](100) NULL,
	[codeowner] [varchar](100) NULL,
	[rsrc] [varchar](100) NULL,
	[startdate] [datetime] NULL,
	[enddate] [datetime] NULL,
	[deploydate] [datetime] NULL,
	[updatedby] [varchar](100) NULL,
	[dlc] [datetime] NULL,
	[createddate] [datetime] NULL,
	[projectdescription] [text] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_c101_upload] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_c120_upload]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_c120_upload](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[projectid] [varchar](25) NULL,
	[projectname] [varchar](100) NULL,
	[portfolio] [varchar](25) NULL,
	[fm] [varchar](100) NULL,
	[class] [varchar](25) NULL,
	[status] [varchar](25) NULL,
	[resource] [varchar](100) NULL,
	[resource_dept] [varchar](25) NULL,
	[resource_costcenter] [int] NULL,
	[vendor] [varchar](50) NULL,
	[etype] [char](1) NULL,
	[ce] [varchar](25) NULL,
	[ytd_actual] [float] NULL,
	[ytd_etc] [float] NULL,
	[ytd_total] [float] NULL,
	[jan_actual] [float] NULL,
	[jan_etc] [float] NULL,
	[feb_actual] [float] NULL,
	[feb_etc] [float] NULL,
	[march_actual] [float] NULL,
	[march_etc] [float] NULL,
	[april_actual] [float] NULL,
	[april_etc] [float] NULL,
	[may_actual] [float] NULL,
	[may_etc] [float] NULL,
	[june_actual] [float] NULL,
	[june_etc] [float] NULL,
	[july_actual] [float] NULL,
	[july_etc] [float] NULL,
	[aug_actual] [float] NULL,
	[aug_etc] [float] NULL,
	[sept_actual] [float] NULL,
	[sept_etc] [float] NULL,
	[oct_actual] [float] NULL,
	[oct_etc] [float] NULL,
	[nov_actual] [float] NULL,
	[nov_etc] [float] NULL,
	[dec_actual] [float] NULL,
	[dec_etc] [float] NULL,
	[next_yr_etc] [float] NULL,
	[next_jan] [float] NULL,
	[next_feb] [float] NULL,
	[next_march] [float] NULL,
	[next_april] [float] NULL,
	[next_may] [float] NULL,
	[next_june] [float] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_c120_upload] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_category]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_category_list]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_category_list](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[categoryid] [int] NULL,
	[itemid] [int] NULL,
	[userid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_category_list] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_class_environments]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_class_environments](
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_class_environments_ap]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_class_environments_ap](
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_class_joins]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_class_joins](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[class1] [int] NULL,
	[class2] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_classs_joins] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_classs]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_classs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[factory_code] [varchar](3) NULL,
	[forecast] [int] NULL,
	[workstation_vmware] [int] NULL,
	[prod] [int] NULL,
	[qa] [int] NULL,
	[test] [int] NULL,
	[dr] [int] NULL,
	[pnc] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_classs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_clusters]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_clusters](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[name] [varchar](50) NULL,
	[nodes] [float] NULL,
	[dr] [int] NULL,
	[ha] [int] NULL,
	[quorum] [int] NULL,
	[local_nodes] [int] NULL,
	[non_shared] [int] NULL,
	[add_instance] [int] NULL,
	[sql] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_clusters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_clusters_instances]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_clusters_instances](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[clusterid] [int] NULL,
	[name] [varchar](50) NULL,
	[sql] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_clusters_instances] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_confidence]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_confidence](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_confidence] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_consistency_groups]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_consistency_groups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_consistency_groups] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_controls]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_controls](
	[controlid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](30) NULL,
	[description] [varchar](100) NULL,
	[path] [varchar](50) NULL,
	[super] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_controls] PRIMARY KEY CLUSTERED 
(
	[controlid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_costavoidance]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_costavoidance](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[opportunity] [varchar](100) NULL,
	[description] [text] NULL,
	[path] [varchar](250) NULL,
	[addtlcostavoidance] [float] NULL,
	[date] [datetime] NULL,
	[applicationid] [int] NULL,
	[userid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_costavoidance] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_costs]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_costs](
	[costid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_costs] PRIMARY KEY CLUSTERED 
(
	[costid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_csm_configs]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_csm_configs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[name] [varchar](50) NULL,
	[servers] [float] NULL,
	[users] [int] NULL,
	[local_nodes] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_csm_configs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_csrc]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_csrc](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[name] [varchar](100) NULL,
	[d] [int] NULL,
	[p] [int] NULL,
	[e] [int] NULL,
	[c] [int] NULL,
	[ds] [datetime] NULL,
	[de] [datetime] NULL,
	[di] [float] NULL,
	[dex] [float] NULL,
	[dh] [float] NULL,
	[ps] [datetime] NULL,
	[pe] [datetime] NULL,
	[pi] [float] NULL,
	[pex] [float] NULL,
	[ph] [float] NULL,
	[es] [datetime] NULL,
	[ee] [datetime] NULL,
	[ei] [float] NULL,
	[eex] [float] NULL,
	[eh] [float] NULL,
	[cs] [datetime] NULL,
	[ce] [datetime] NULL,
	[ci] [float] NULL,
	[cex] [float] NULL,
	[ch] [float] NULL,
	[path] [varchar](500) NULL,
	[cc] [int] NULL,
	[step] [int] NULL,
	[status] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_CSRC] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_csrc_detail]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_csrc_detail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[step] [int] NULL,
	[userid] [int] NULL,
	[comments] [text] NULL,
	[status] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_csrc_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_datapoint_applications]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_datapoint_applications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[applicationid] [int] NULL,
	[key] [varchar](50) NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_cv_datapoint_applications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_datapoint_fields]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_datapoint_fields](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[key] [varchar](50) NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_cv_datapoint_fields] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_datapoint_models]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_datapoint_models](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[modelid] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_cv_datapoint_models] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_datapoint_search]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_datapoint_search](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[userid] [int] NULL,
	[type] [varchar](50) NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_cv_datapoint_search] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_delegates]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_delegates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[delegate] [int] NULL,
	[rights] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_delegates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_depot]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_depot](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_depot] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_depot_rooms]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_depot_rooms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_depot_rooms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_designer]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_designer](
	[designid] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[controlid] [int] NOT NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NOT NULL,
 CONSTRAINT [PK_cv_designer] PRIMARY KEY CLUSTERED 
(
	[designid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_did_you_knows]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_did_you_knows](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_did_you_knows] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_document_repository]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_document_repository](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[applicationid] [int] NULL,
	[profileid] [int] NULL,
	[name] [varchar](100) NULL,
	[type] [varchar](25) NULL,
	[path] [varchar](250) NULL,
	[parent] [varchar](250) NULL,
	[size] [int] NULL,
	[security] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_documents] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_document_repository_share]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_document_repository_share](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[docid] [int] NULL,
	[security] [int] NULL,
	[sharetype] [varchar](25) NULL,
	[applicationid] [int] NULL,
	[profileid] [int] NULL,
	[ownerid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_documents_share] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_documents]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_documents](
	[documentid] [int] IDENTITY(1,1) NOT NULL,
	[projectid] [int] NULL,
	[requestid] [int] NULL,
	[name] [varchar](100) NULL,
	[path] [varchar](300) NULL,
	[description] [text] NULL,
	[security] [int] NULL,
	[userid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_documents] PRIMARY KEY CLUSTERED 
(
	[documentid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_documents_permission]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_documents_permission](
	[documentid] [int] NULL,
	[userid] [int] NULL,
	[security] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_domain_controllers]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_domain_controllers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[environment] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_domain_controllers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_domains]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_domains](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[environment] [int] NULL,
	[account_maintenance] [int] NULL,
	[group_maintenance] [int] NULL,
	[test_domain] [int] NULL,
	[move] [int] NULL,
	[dns_ip1] [varchar](15) NULL,
	[dns_ip2] [varchar](15) NULL,
	[dns_ip3] [varchar](15) NULL,
	[dns_ip4] [varchar](15) NULL,
	[wins_ip1] [varchar](15) NULL,
	[wins_ip2] [varchar](15) NULL,
	[wins_ip3] [varchar](15) NULL,
	[wins_ip4] [varchar](15) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_domains] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_domains_admin_groups]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_domains_admin_groups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[domainid] [int] NULL,
	[name] [varchar](50) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_domains_admin_groups] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_domains_class_environments]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_domains_class_environments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[domainid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_domains_class_environments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_domains_suffix]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_domains_suffix](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[domainid] [int] NULL,
	[name] [varchar](50) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_domains_suffix] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_enhancement]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_enhancement](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[title] [varchar](100) NULL,
	[description] [text] NULL,
	[pageid] [int] NULL,
	[num_users] [int] NULL,
	[url] [varchar](250) NULL,
	[path] [varchar](250) NULL,
	[startdate] [datetime] NULL,
	[enddate] [datetime] NULL,
	[userid] [int] NULL,
	[status] [int] NULL,
	[new] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_enhancements] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_enhancement_versions]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_enhancement_versions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](10) NULL,
	[compiled] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_enhancement_versions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_enhancements]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_enhancements](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[description] [text] NULL,
	[versionid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_enhancements] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_environment]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_environment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[forecast] [int] NULL,
	[ecom] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_environment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_field_names]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_field_names](
	[nameid] [int] NULL,
	[name] [varchar](200) NULL,
	[datatype] [varchar](2) NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_floors]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_floors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_floors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NOT NULL,
	[pnc_project] [varchar](20) NULL,
	[userid] [int] NULL,
	[active] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_acquisitions]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_acquisitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelid] [int] NULL,
	[lineitemid] [int] NULL,
	[cost] [float] NULL,
	[prod] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_acquisitions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_affected]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_affected](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionid] [int] NULL,
	[affectedid] [int] NULL,
	[responseid] [int] NULL,
	[state] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_affected] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_affects]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_affects](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionid] [int] NULL,
	[affectedid] [int] NULL,
	[state] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_affects] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_answers]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[forecastid] [int] NULL,
	[platformid] [int] NULL,
	[hostid] [int] NULL,
	[storage] [int] NULL,
	[backup] [int] NULL,
	[override] [int] NULL,
	[overrideby] [int] NULL,
	[breakfix] [int] NULL,
	[change] [varchar](20) NULL,
	[nameid] [int] NULL,
	[workstation] [varchar](50) NULL,
	[code] [varchar](50) NULL,
	[name] [varchar](50) NULL,
	[modelid] [int] NULL,
	[vendorid] [int] NULL,
	[quantity] [int] NULL,
	[implementation] [datetime] NULL,
	[execution] [datetime] NULL,
	[confidenceid] [int] NULL,
	[classid] [int] NULL,
	[test] [int] NULL,
	[environmentid] [int] NULL,
	[maintenanceid] [int] NULL,
	[applicationid] [int] NULL,
	[subapplicationid] [int] NULL,
	[addressid] [int] NULL,
	[ha] [int] NULL,
	[recovery_number] [int] NULL,
	[step] [int] NULL,
	[comments] [varchar](max) NULL,
	[serviceid] [int] NULL,
	[requestid] [int] NULL,
	[version] [varchar](10) NULL,
	[appname] [varchar](100) NULL,
	[appcode] [varchar](10) NULL,
	[mnemonicid] [int] NULL,
	[dr_criticality] [int] NULL,
	[appcontact] [int] NULL,
	[admin1] [int] NULL,
	[admin2] [int] NULL,
	[networkengineer] [int] NULL,
	[costid] [int] NULL,
	[poolid] [int] NULL,
	[storage_override] [int] NULL,
	[storage_overrideby] [int] NULL,
	[executed] [datetime] NULL,
	[executed_by] [int] NULL,
	[completed] [datetime] NULL,
	[userid] [int] NULL,
	[production] [datetime] NULL,
	[finished] [datetime] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_backup]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_backup](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[daily] [int] NULL,
	[weekly] [int] NULL,
	[weekly_day] [varchar](15) NULL,
	[monthly] [int] NULL,
	[monthly_day] [varchar](15) NULL,
	[monthly_days] [varchar](20) NULL,
	[time] [int] NULL,
	[time_hour] [varchar](10) NULL,
	[time_switch] [varchar](2) NULL,
	[start_date] [varchar](10) NULL,
	[recoveryid] [int] NULL,
	[cf_percent] [varchar](4) NULL,
	[cf_compression] [varchar](4) NULL,
	[cf_average] [varchar](15) NULL,
	[cf_backup] [varchar](4) NULL,
	[cf_archive] [varchar](4) NULL,
	[cf_window] [varchar](4) NULL,
	[cf_sets] [varchar](4) NULL,
	[cd_type] [varchar](15) NULL,
	[cd_percent] [varchar](4) NULL,
	[cd_compression] [varchar](4) NULL,
	[cd_versions] [varchar](4) NULL,
	[cd_window] [varchar](4) NULL,
	[cd_growth] [varchar](4) NULL,
	[average_one] [int] NULL,
	[documentation] [varchar](100) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_backup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_backup_exclusions]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_backup_exclusions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[path] [varchar](200) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_backup_exclusions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_backup_inclusions]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_backup_inclusions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[path] [varchar](200) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_backup_inclusions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_backup_retention]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_backup_retention](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[path] [varchar](200) NULL,
	[first] [datetime] NULL,
	[number] [int] NULL,
	[type] [varchar](30) NULL,
	[hour] [varchar](10) NULL,
	[switch] [varchar](2) NULL,
	[occurence] [varchar](10) NULL,
	[occurs] [varchar](50) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_backup_retention] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_platform]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_platform](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[questionid] [int] NULL,
	[responseid] [int] NULL,
	[custom] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_platform] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_answers_reset]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_reset](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[requestid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_reset] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_answers_storage]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_storage](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[high] [int] NULL,
	[high_total] [float] NULL,
	[high_qa] [float] NULL,
	[high_test] [float] NULL,
	[high_replicated] [float] NULL,
	[high_level] [varchar](20) NULL,
	[high_ha] [float] NULL,
	[standard] [int] NULL,
	[standard_total] [float] NULL,
	[standard_qa] [float] NULL,
	[standard_test] [float] NULL,
	[standard_replicated] [float] NULL,
	[standard_level] [varchar](20) NULL,
	[standard_ha] [float] NULL,
	[low] [int] NULL,
	[low_total] [float] NULL,
	[low_qa] [float] NULL,
	[low_test] [float] NULL,
	[low_replicated] [float] NULL,
	[low_level] [varchar](20) NULL,
	[low_ha] [float] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_storage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_storage_os]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_storage_os](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[high] [int] NULL,
	[high_total] [float] NULL,
	[high_qa] [float] NULL,
	[high_test] [float] NULL,
	[high_replicated] [float] NULL,
	[high_level] [varchar](20) NULL,
	[high_ha] [float] NULL,
	[standard] [int] NULL,
	[standard_total] [float] NULL,
	[standard_qa] [float] NULL,
	[standard_test] [float] NULL,
	[standard_replicated] [float] NULL,
	[standard_level] [varchar](20) NULL,
	[standard_ha] [float] NULL,
	[low] [int] NULL,
	[low_total] [float] NULL,
	[low_qa] [float] NULL,
	[low_test] [float] NULL,
	[low_replicated] [float] NULL,
	[low_level] [varchar](20) NULL,
	[low_ha] [float] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_storage_os] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_unlocks]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_unlocks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[userid] [int] NULL,
	[reason] [text] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_cv_forecast_answers_unlocks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_answers_update]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_update](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[completed] [int] NULL,
	[valid] [int] NULL,
	[comments] [text] NULL,
	[userid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_update] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_answers_update_sent]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_update_sent](
	[answerid] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_answers_vendor]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_vendor](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[platformid] [int] NULL,
	[typeid] [int] NULL,
	[make] [varchar](50) NULL,
	[modelname] [varchar](50) NULL,
	[size_w] [varchar](10) NULL,
	[size_h] [varchar](10) NULL,
	[amp] [varchar](10) NULL,
	[description] [text] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_vendor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_answers_workstation]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_answers_workstation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[ramid] [int] NULL,
	[osid] [int] NULL,
	[recovery] [int] NULL,
	[hddid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answers_workstation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_line_items]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_line_items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_line_items] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_operations]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_operations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelid] [int] NULL,
	[lineitemid] [int] NULL,
	[cost] [float] NULL,
	[prod] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_operations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_question_platforms]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_question_platforms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionid] [int] NULL,
	[platformid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_question_platforms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_questions]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_questions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[question] [text] NULL,
	[type] [int] NULL,
	[hide_override] [int] NULL,
	[required] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_questions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_responses]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_responses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionid] [int] NULL,
	[name] [varchar](100) NULL,
	[response] [text] NULL,
	[variance] [int] NULL,
	[custom] [int] NULL,
	[os_distributed] [int] NULL,
	[os_midrange] [int] NULL,
	[cores] [int] NULL,
	[ram] [int] NULL,
	[web] [int] NULL,
	[dbase] [int] NULL,
	[ha_none] [int] NULL,
	[ha_cluster] [int] NULL,
	[ha_csm] [int] NULL,
	[ha_room] [int] NULL,
	[dr_under] [int] NULL,
	[dr_over] [int] NULL,
	[one_one] [int] NULL,
	[many_one] [int] NULL,
	[components] [varchar](50) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_responses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_responses_additional]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_responses_additional](
	[responseid] [int] NULL,
	[additionalid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_steps]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_steps](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[platformid] [int] NULL,
	[name] [varchar](50) NULL,
	[subtitle] [text] NULL,
	[path] [varchar](100) NULL,
	[override_path] [varchar](100) NULL,
	[image_path] [varchar](100) NULL,
	[additional] [int] NULL,
	[step] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_steps] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_steps_additional]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_forecast_steps_additional](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[path] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_steps_additional] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_forecast_steps_additional_done]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_steps_additional_done](
	[answerid] [int] NULL,
	[additionalid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_steps_done]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_steps_done](
	[answerid] [int] NULL,
	[step] [int] NULL,
	[done] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_steps_reset]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_steps_reset](
	[platformid] [int] NULL,
	[step] [int] NULL,
	[reset] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_forecast_street_values]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_forecast_street_values](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelid] [int] NULL,
	[lineitemid] [int] NULL,
	[cost] [float] NULL,
	[prod] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_street_values] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_group_maintenance]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_group_maintenance](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[maintenance] [varchar](10) NULL,
	[name] [varchar](50) NULL,
	[domain] [int] NULL,
	[approval] [int] NULL,
	[reason] [text] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_group_maintenance_parameters]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_group_maintenance_parameters](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[value] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_group_maintenance_parameters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_groups]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_groups](
	[groupid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[description] [varchar](200) NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_groups] PRIMARY KEY CLUSTERED 
(
	[groupid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_holidays]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_holidays](
	[holidayid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[happens] [datetime] NULL,
	[enabled] [int] NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_holidays] PRIMARY KEY CLUSTERED 
(
	[holidayid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_hosts]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_hosts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[modelid] [int] NULL,
	[platformid] [int] NULL,
	[path] [varchar](100) NULL,
	[prefix] [varchar](3) NULL,
	[storage] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_hosts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_icons]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_icons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[extension] [varchar](5) NULL,
	[small] [varchar](100) NULL,
	[large] [varchar](100) NULL,
	[content_type] [varchar](50) NULL,
	[iframe] [int] NULL,
	[preview] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_icons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_idcasset_types]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_idcasset_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [nchar](10) NULL,
 CONSTRAINT [PK_cv_idcasset_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_issues]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_issues](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[title] [varchar](100) NULL,
	[description] [text] NULL,
	[pageid] [int] NULL,
	[num_users] [int] NULL,
	[url] [varchar](250) NULL,
	[path] [varchar](250) NULL,
	[userid] [int] NULL,
	[status] [int] NULL,
	[new] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_support] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_items]    Script Date: 07/31/2009 11:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[amount] [float] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_items] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_location_address]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_location_address](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cityid] [int] NULL,
	[name] [varchar](100) NULL,
	[factory_code] [varchar](5) NULL,
	[common] [int] NULL,
	[commonname] [varchar](50) NULL,
	[storage] [int] NULL,
	[dr] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_location_address] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_location_city]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_location_city](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[stateid] [int] NULL,
	[name] [varchar](100) NULL,
	[code] [varchar](10) NULL,
	[zip] [varchar](50) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_location_city] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_location_state]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_location_state](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[code] [varchar](10) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_location_state] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_log]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_log](
	[userid] [int] NULL,
	[action] [text] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_logins]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_logins](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[xid] [varchar](30) NULL,
	[logins] [int] NULL,
	[first] [datetime] NULL,
	[last] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_logins] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_logins_invalid]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_logins_invalid](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](100) NULL,
	[password] [varchar](100) NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_logins_invalid] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_maintenance_windows]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_maintenance_windows](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_maintenance_windows] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_messages]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_messages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[type] [char](1) NULL,
	[message] [text] NULL,
	[path] [varchar](250) NULL,
	[applicationid] [int] NULL,
	[userid] [int] NULL,
	[admin] [int] NULL,
	[new] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_vijay_messages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_mnemonics]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_mnemonics](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[factory_code] [varchar](10) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_mnemonics] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_models]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_models](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typeid] [int] NULL,
	[name] [varchar](100) NULL,
	[make] [varchar](100) NULL,
	[pdf] [varchar](100) NULL,
	[sale] [int] NULL,
	[grouping] [int] NULL,
	[hostid] [int] NULL,
	[destroy] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_models] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_models_property]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_models_property](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelid] [int] NULL,
	[available] [int] NULL,
	[replicate_times] [int] NULL,
	[amp] [float] NULL,
	[network_ports] [int] NULL,
	[storage_ports] [int] NULL,
	[name] [varchar](100) NULL,
	[ram] [int] NULL,
	[cpu_count] [int] NULL,
	[cpu_speed] [float] NULL,
	[high_availability] [int] NULL,
	[high_performance] [int] NULL,
	[low_performance] [int] NULL,
	[enforce_1_1_recovery] [int] NULL,
	[no_many_1_recovery] [int] NULL,
	[vmware_virtual] [int] NULL,
	[ibm_virtual] [int] NULL,
	[storage_db_boot_local] [int] NULL,
	[storage_db_boot_san_windows] [int] NULL,
	[storage_db_boot_san_unix] [int] NULL,
	[storage_de_fdrive_can_local] [int] NULL,
	[storage_de_fdrive_must_san] [int] NULL,
	[storage_de_fdrive_only] [int] NULL,
	[storage_de_filesystems] [int] NULL,
	[not_executable] [int] NULL,
	[client_can_execute] [int] NULL,
	[type_blade] [int] NULL,
	[type_physical] [int] NULL,
	[type_vmware] [int] NULL,
	[type_enclosure] [int] NULL,
	[config_service_pack] [int] NULL,
	[config_vmware_template] [int] NULL,
	[config_maintenance_level] [int] NULL,
	[midrange] [int] NULL,
	[vio] [int] NULL,
	[fabric] [int] NULL,
	[inventory] [int] NULL,
	[StorageThresholdMin] [float] NULL,
	[StorageThresholdMax] [float] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_models_property] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_models_property_thresholds]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_models_property_thresholds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[propertyid] [int] NULL,
	[qty_from] [int] NULL,
	[qty_to] [int] NULL,
	[number_days] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_models_property_thresholds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_models_reservations]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_models_reservations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_models_reservations_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_models_reservations_list]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_models_reservations_list](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[use_reservation] [int] NULL,
	[display] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_models_reservations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_new]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_new](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](100) NULL,
	[description] [text] NULL,
	[attachment] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ondemand_sending]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ondemand_sending](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[class] [int] NULL,
	[environment] [int] NULL,
	[result] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_sending] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ondemand_sending_config]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_sending_config](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[name] [nchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_sending_config] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_steps]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ondemand_steps](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typeid] [int] NULL,
	[name] [varchar](50) NULL,
	[title] [varchar](50) NULL,
	[path] [varchar](100) NULL,
	[script] [text] NULL,
	[done] [varchar](100) NULL,
	[interact_path] [varchar](100) NULL,
	[zeus] [int] NULL,
	[power] [int] NULL,
	[accounts] [int] NULL,
	[installs] [int] NULL,
	[groups] [int] NULL,
	[type] [int] NULL,
	[resume_error] [int] NULL,
	[step] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_steps_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ondemand_steps_done_server]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_steps_done_server](
	[serverid] [int] NULL,
	[step] [int] NULL,
	[result] [text] NULL,
	[error] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_steps_done_workstation]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_steps_done_workstation](
	[workstationid] [int] NULL,
	[step] [int] NULL,
	[result] [text] NULL,
	[error] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_blade_ii]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_blade_ii](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[modelid] [int] NULL,
	[prod] [datetime] NULL,
	[chk1] [int] NULL,
	[chk3] [int] NULL,
	[chk4] [int] NULL,
	[chk5] [int] NULL,
	[chk6] [int] NULL,
	[chk7] [int] NULL,
	[chk8] [int] NULL,
	[chk9] [int] NULL,
	[chk10] [int] NULL,
	[chk11] [int] NULL,
	[chk12] [int] NULL,
	[notifications] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_generic_ii]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_generic_ii](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[modelid] [int] NULL,
	[prod] [datetime] NULL,
	[chk1] [int] NULL,
	[chk3] [int] NULL,
	[chk4] [int] NULL,
	[chk5] [int] NULL,
	[chk6] [int] NULL,
	[chk7] [int] NULL,
	[chk8] [int] NULL,
	[chk9] [int] NULL,
	[chk10] [int] NULL,
	[chk11] [int] NULL,
	[chk12] [int] NULL,
	[chk13] [int] NULL,
	[chk14] [int] NULL,
	[chk15] [int] NULL,
	[notifications_test] [int] NULL,
	[notifications_prod] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_pending]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_pending](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[resourceid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_tasks_pending] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_physical_ii]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_physical_ii](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[modelid] [int] NULL,
	[step] [int] NULL,
	[prod] [datetime] NULL,
	[chk1] [int] NULL,
	[chk3] [int] NULL,
	[chk4] [int] NULL,
	[chk5] [int] NULL,
	[chk6] [int] NULL,
	[chk7] [int] NULL,
	[chk8] [int] NULL,
	[chk9] [int] NULL,
	[chk10] [int] NULL,
	[chk11] [int] NULL,
	[chk12] [int] NULL,
	[chk13] [int] NULL,
	[chk14] [int] NULL,
	[chk15] [int] NULL,
	[notifications] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_server_backup]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_server_backup](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[modelid] [int] NULL,
	[chk1] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_tasks_server_backup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_server_other]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_server_other](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[modelid] [int] NULL,
	[chk1] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_tasks_server_other] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_server_storage]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_server_storage](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[prod] [int] NULL,
	[modelid] [int] NULL,
	[chk1] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_tasks_server_storage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_success]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_success](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[type] [varchar](20) NULL,
	[success] [int] NULL,
	[comments] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_tasks_success] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ondemand_tasks_vmware_ii]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_tasks_vmware_ii](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[modelid] [int] NULL,
	[prod] [datetime] NULL,
	[chk1] [int] NULL,
	[chk3] [int] NULL,
	[chk4] [int] NULL,
	[chk5] [int] NULL,
	[chk6] [int] NULL,
	[chk7] [int] NULL,
	[chk8] [int] NULL,
	[chk9] [int] NULL,
	[chk10] [int] NULL,
	[chk11] [int] NULL,
	[notifications] [int] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ondemand_wizard_steps]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ondemand_wizard_steps](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typeid] [int] NULL,
	[name] [varchar](50) NULL,
	[subtitle] [text] NULL,
	[path] [varchar](100) NULL,
	[show_cluster] [int] NULL,
	[show_csm] [int] NULL,
	[skip_cluster] [int] NULL,
	[skip_csm] [int] NULL,
	[step] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ondemand_steps] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ondemand_wizard_steps_done]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ondemand_wizard_steps_done](
	[answerid] [int] NULL,
	[step] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_operating_systems]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_operating_systems](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[cluster_name] [int] NULL,
	[workstation] [int] NULL,
	[code] [varchar](2) NULL,
	[factory_code] [varchar](3) NULL,
	[zeus_os] [varchar](20) NULL,
	[zeus_os_version] [varchar](20) NULL,
	[zeus_build_type] [varchar](50) NULL,
	[zeus_build_type_pnc] [varchar](50) NULL,
	[vmware_os] [varchar](50) NULL,
	[bootptab_os] [varchar](50) NULL,
	[altiris] [varchar](50) NULL,
	[midrange] [int] NULL,
	[linux] [int] NULL,
	[e1000] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_operating_systems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_operating_systems_service_packs]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_operating_systems_service_packs](
	[osid] [int] NULL,
	[spid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_order_report]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_order_report](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[title] [varchar](100) NULL,
	[data_source] [varchar](50) NULL,
	[chart_type] [varchar](25) NULL,
	[report_upload] [varchar](100) NULL,
	[instructions] [varchar](100) NULL,
	[data_exclusion] [varchar](100) NOT NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_order_report] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_order_report_applications]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_order_report_applications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reportid] [int] NULL,
	[appname] [varchar](100) NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_order_report_applications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_order_report_calculation]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_order_report_calculation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reportid] [int] NULL,
	[fieldid] [int] NULL,
	[formula] [varchar](100) NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_order_report_calculation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_order_report_charts]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_order_report_charts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[url] [varchar](200) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_orderreport_charts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_order_report_datafields]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_order_report_datafields](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reportid] [int] NULL,
	[name] [varchar](50) NULL,
	[type] [varchar](50) NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_report_datafield] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_order_report_datasource]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_order_report_datasource](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_orderreport_datasource] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_organizations]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_organizations](
	[organizationid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[userid] [int] NULL,
	[nodisc] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_organizations] PRIMARY KEY CLUSTERED 
(
	[organizationid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_pagecontrols]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_pagecontrols](
	[pagecontrolid] [int] IDENTITY(1,1) NOT NULL,
	[schemaid] [int] NULL,
	[placeholder] [varchar](10) NULL,
	[pageid] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_pagecontrols] PRIMARY KEY CLUSTERED 
(
	[pagecontrolid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_pages]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_pages](
	[pageid] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[title] [varchar](100) NULL,
	[urltitle] [varchar](50) NULL,
	[menutitle] [varchar](50) NULL,
	[browsertitle] [varchar](100) NULL,
	[templateid] [int] NULL,
	[related] [int] NULL,
	[navimage] [varchar](100) NULL,
	[navoverimage] [varchar](100) NULL,
	[description] [text] NULL,
	[tooltip] [text] NULL,
	[sproc] [varchar](100) NULL,
	[window] [int] NULL,
	[url] [varchar](100) NULL,
	[target] [varchar](10) NULL,
	[navigation] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_pages] PRIMARY KEY CLUSTERED 
(
	[pageid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_pcr]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_pcr](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[name] [varchar](100) NULL,
	[scope] [int] NULL,
	[s] [int] NULL,
	[sds] [datetime] NULL,
	[sde] [datetime] NULL,
	[sps] [datetime] NULL,
	[spe] [datetime] NULL,
	[ses] [datetime] NULL,
	[see] [datetime] NULL,
	[scs] [datetime] NULL,
	[sce] [datetime] NULL,
	[f] [int] NULL,
	[fd] [float] NULL,
	[fp] [float] NULL,
	[fe] [float] NULL,
	[fc] [float] NULL,
	[reasons] [text] NULL,
	[scopecomments] [text] NULL,
	[schcomments] [text] NULL,
	[fincomments] [text] NULL,
	[path] [varchar](200) NULL,
	[cc] [int] NULL,
	[step] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_pcr] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_pcr_detail]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_pcr_detail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[step] [int] NULL,
	[userid] [int] NULL,
	[comments] [text] NULL,
	[status] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_pcr_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_permissions]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_permissions](
	[permissionid] [int] IDENTITY(1,1) NOT NULL,
	[applicationid] [int] NULL,
	[groupid] [int] NULL,
	[permission] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_permissions] PRIMARY KEY CLUSTERED 
(
	[permissionid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_platforms]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_platforms](
	[platformid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[userid] [int] NULL,
	[managerid] [int] NULL,
	[image] [varchar](100) NULL,
	[big_image] [varchar](100) NULL,
	[asset] [int] NULL,
	[forecast] [int] NULL,
	[system] [int] NULL,
	[inventory] [int] NULL,
	[asset_checkin_path] [varchar](100) NULL,
	[asset_checkin_path_excel] [varchar](100) NULL,
	[action_form] [varchar](100) NULL,
	[demand_form] [varchar](100) NULL,
	[supply_form] [varchar](100) NULL,
	[order_form] [varchar](100) NULL,
	[add_form] [varchar](100) NULL,
	[settings_form] [varchar](100) NULL,
	[forms_form] [varchar](100) NULL,
	[alert_form] [varchar](100) NULL,
	[max_inventory1] [int] NULL,
	[max_inventory2] [int] NULL,
	[max_inventory3] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_platforms] PRIMARY KEY CLUSTERED 
(
	[platformid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_project_closure_PDF]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_project_closure_PDF](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[path] [varchar](250) NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_vijay_TPM_closure_upload] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_project_financials]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_financials](
	[FinancialID] [int] IDENTITY(1,1) NOT NULL,
	[BudgetTypeID] [int] NOT NULL,
	[CostTypeID] [int] NOT NULL,
	[PhaseTypeID] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_numbers]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_numbers](
	[projectid] [int] IDENTITY(1200000,1) NOT NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_project_numbers] PRIMARY KEY CLUSTERED 
(
	[projectid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_platforms]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_platforms](
	[requestid] [int] NULL,
	[platformid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_request_classes]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_project_request_classes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_project_request_classes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_project_request_comments]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_request_comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[userid] [int] NULL,
	[comment] [text] NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_project_comments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_request_priority]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_request_priority](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[expected_cost] [float] NULL,
	[cost_avoidance] [float] NULL,
	[impact_analysis] [float] NULL,
	[overall_priority] [float] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_project_request_priority] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_request_qa]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_project_request_qa](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[bd] [varchar](30) NULL,
	[organizationid] [int] NULL,
	[questionid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_project_request_qa] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_project_request_questions]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_project_request_questions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[question] [text] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[required] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_project_request_questions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_project_request_questions_class]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_request_questions_class](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionid] [int] NULL,
	[classid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_project_request_questions_class] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_request_responses]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_project_request_responses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionid] [int] NULL,
	[name] [varchar](100) NULL,
	[response] [text] NULL,
	[weight] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_project_request_responses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_project_request_status]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_request_status](
	[requeststatusid] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[manager] [int] NULL,
	[platform] [int] NULL,
	[board] [int] NULL,
	[director] [int] NULL,
	[step] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_project_request_status] PRIMARY KEY CLUSTERED 
(
	[requeststatusid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_request_status_detail]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_request_status_detail](
	[detailid] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[userid] [int] NULL,
	[approval] [int] NULL,
	[step] [int] NULL,
	[comments] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_project_request_status_detail] PRIMARY KEY CLUSTERED 
(
	[detailid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_request_submission]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_request_submission](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[questionid] [int] NULL,
	[responseid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_project_request_submission] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_request_weight_priority]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_project_request_weight_priority](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[classid] [int] NULL,
	[weight] [float] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_project_request_weight_priority] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_project_requests]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_project_requests](
	[id] [int] IDENTITY(10000,1) NOT NULL,
	[requestid] [int] NULL,
	[req_type] [int] NULL,
	[req_date] [datetime] NULL,
	[interdependency] [varchar](50) NULL,
	[projects] [varchar](200) NULL,
	[capability] [text] NULL,
	[man_hours] [int] NULL,
	[expected_capital] [varchar](30) NULL,
	[internal_labor] [varchar](30) NULL,
	[external_labor] [varchar](30) NULL,
	[maintenance_increase] [varchar](30) NULL,
	[project_expenses] [varchar](30) NULL,
	[estimated_avoidance] [varchar](30) NULL,
	[estimated_savings] [varchar](30) NULL,
	[realized_savings] [varchar](30) NULL,
	[business_avoidance] [varchar](30) NULL,
	[maintenance_avoidance] [varchar](30) NULL,
	[asset_reusability] [varchar](75) NULL,
	[internal_impact] [varchar](20) NULL,
	[external_impact] [varchar](20) NULL,
	[business_impact] [varchar](30) NULL,
	[strategic_opportunity] [varchar](30) NULL,
	[acquisition] [varchar](30) NULL,
	[technology_capabilities] [varchar](50) NULL,
	[c1] [int] NULL,
	[endlife] [int] NULL,
	[endlife_date] [datetime] NULL,
	[tpm] [int] NULL,
	[notify] [int] NULL,
	[created] [datetime] NULL,
	[updated] [datetime] NULL,
	[shelved] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_project_requests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_projects]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_projects](
	[projectid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[bd] [varchar](20) NULL,
	[number] [varchar](30) NULL,
	[userid] [int] NULL,
	[organization] [int] NULL,
	[segmentid] [int] NULL,
	[lead] [int] NULL,
	[working] [int] NULL,
	[executive] [int] NULL,
	[technical] [int] NULL,
	[engineer] [int] NULL,
	[other] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_projects] PRIMARY KEY CLUSTERED 
(
	[projectid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_projects_approval]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_projects_approval](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_projects_approval] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_projects_pending]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_projects_pending](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[name] [varchar](50) NULL,
	[bd] [varchar](20) NULL,
	[number] [varchar](30) NULL,
	[userid] [int] NULL,
	[organization] [int] NULL,
	[segmentid] [int] NULL,
	[lead] [int] NULL,
	[working] [int] NULL,
	[executive] [int] NULL,
	[technical] [int] NULL,
	[engineer] [int] NULL,
	[other] [int] NULL,
	[task] [int] NULL,
	[description] [text] NULL,
	[reason] [text] NULL,
	[modified] [datetime] NULL,
	[rejected] [datetime] NULL,
	[completed] [datetime] NULL,
 CONSTRAINT [PK_cv_projects_pending] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_rack_positions]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_rack_positions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_rack_positions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_racks]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_racks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_racks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_recovery_locations]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_recovery_locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_recovery_locations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_report_applications]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_report_applications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reportid] [int] NULL,
	[applicationid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_report_applications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_report_favorites]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_report_favorites](
	[userid] [int] NULL,
	[reportid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_report_groups]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_report_groups](
	[groupid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_report_groups] PRIMARY KEY CLUSTERED 
(
	[groupid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_report_permissions]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_report_permissions](
	[permissionid] [int] IDENTITY(1,1) NOT NULL,
	[reportid] [int] NULL,
	[groupid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_report_permissions] PRIMARY KEY CLUSTERED 
(
	[permissionid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_report_roles]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_report_roles](
	[roleid] [int] IDENTITY(1,1) NOT NULL,
	[applicationid] [int] NULL,
	[groupid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_report_roles] PRIMARY KEY CLUSTERED 
(
	[roleid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_report_users]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_report_users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reportid] [int] NULL,
	[userid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_report_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_reports]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_reports](
	[reportid] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](100) NULL,
	[path] [varchar](200) NULL,
	[physical] [varchar](200) NULL,
	[description] [text] NULL,
	[about] [text] NULL,
	[image] [varchar](100) NULL,
	[parent] [int] NULL,
	[percentage] [int] NULL,
	[toggle] [int] NULL,
	[application] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_reports] PRIMARY KEY CLUSTERED 
(
	[reportid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_request_field_assign]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_request_field_assign](
	[itemid] [int] NULL,
	[nameid] [int] NULL,
	[assign] [varchar](2) NULL,
	[hide] [int] NULL,
	[display] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_request_field_values]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_request_field_values](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[nameid] [int] NULL,
	[value] [text] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_request_field_values_OLD]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_request_field_values_OLD](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[nameid] [int] NULL,
	[value] [text] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_request_forms]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_request_forms](
	[formid] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[done] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_request_forms] PRIMARY KEY CLUSTERED 
(
	[formid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_request_items]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_request_items](
	[itemid] [int] IDENTITY(1,1) NOT NULL,
	[applicationid] [int] NULL,
	[name] [varchar](100) NULL,
	[service_title] [varchar](100) NULL,
	[image] [varchar](100) NULL,
	[platformid] [int] NULL,
	[activity_type] [int] NULL,
	[show] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resources] PRIMARY KEY CLUSTERED 
(
	[itemid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_request_items_tabs]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_request_items_tabs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tabid] [int] NULL,
	[itemid] [int] NULL,
	[display] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_request_items_tabs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_request_items_tasktabs]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_request_items_tasktabs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tabid] [int] NULL,
	[itemid] [int] NULL,
	[display] [nchar](10) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_request_items_tasktabs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_request_results]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_request_results](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[result] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_requests]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_requests](
	[requestid] [int] IDENTITY(25000,1) NOT NULL,
	[projectid] [int] NULL,
	[userid] [int] NULL,
	[description] [text] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_requests] PRIMARY KEY CLUSTERED 
(
	[requestid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_resource_assignment]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_assignment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[resourcetypeid] [int] NULL,
	[requestedby] [varchar](50) NULL,
	[requesteddate] [datetime] NULL,
	[fulfilldate] [datetime] NULL,
	[resourceassigned] [varchar](50) NULL,
	[status] [varchar](100) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_resource_assignment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_resource_request_change_controls]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_request_change_controls](
	[changeid] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[number] [varchar](20) NULL,
	[implementation] [datetime] NULL,
	[comments] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_request_change_controls] PRIMARY KEY CLUSTERED 
(
	[changeid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_resource_request_milestones]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_request_milestones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[approved] [datetime] NULL,
	[forecasted] [datetime] NULL,
	[complete] [int] NULL,
	[milestone] [varchar](100) NULL,
	[description] [text] NULL,
	[latest] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_request_milestones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_resource_request_prc]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_request_prc](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[scope] [int] NULL,
	[schedule] [int] NULL,
	[cost] [int] NULL,
	[path] [varchar](150) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_request_prc] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_resource_request_update]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_resource_request_update](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[status] [int] NULL,
	[comments] [text] NULL,
	[latest] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_request_update] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_resource_request_update_tpm]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_resource_request_update_tpm](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[scope] [int] NULL,
	[timeline] [int] NULL,
	[budget] [int] NULL,
	[datestamp] [datetime] NULL,
	[comments] [text] NULL,
	[thisweek] [text] NULL,
	[nextweek] [text] NULL,
	[latest] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_request_update_tpm] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_resource_requests]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_requests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[name] [varchar](200) NULL,
	[devices] [int] NULL,
	[allocated] [float] NULL,
	[status] [int] NULL,
	[solo] [int] NULL,
	[accepted] [int] NULL,
	[reason] [text] NULL,
	[platform_approval] [int] NULL,
	[assignedby] [int] NULL,
	[assigned] [datetime] NULL,
	[SLANotificationDate] [datetime] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_requests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_resource_requests_details]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_resource_requests_details](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[resourceid] [int] NULL,
	[detailid] [int] NULL,
	[done] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_requests_details] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_resource_requests_hours]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_resource_requests_hours](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[used] [float] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_requests_hours] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_resource_requests_sharing]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_resource_requests_sharing](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sharedid] [int] NULL,
	[userid] [int] NULL,
	[rights] [int] NULL,
	[expiration] [datetime] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_requests_sharing] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_resource_requests_workflow]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_requests_workflow](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[workflowid] [int] NULL,
	[name] [varchar](200) NULL,
	[userid] [int] NULL,
	[devices] [int] NULL,
	[used] [float] NULL,
	[allocated] [float] NULL,
	[status] [int] NULL,
	[joined] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[modifiedby] [int] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_requests_workflow] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_resource_requests_workflow_assign]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_resource_requests_workflow_assign](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[userid] [int] NULL,
	[devices] [int] NULL,
	[allocated] [float] NULL,
	[created] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_requests_workflow_assign] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_resource_scheduling]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_scheduling](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[title] [varchar](50) NULL,
	[startdate] [datetime] NULL,
	[enddate] [datetime] NULL,
	[starttime] [varchar](15) NULL,
	[endtime] [varchar](15) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_resource_types]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_resource_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_resource_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_restart]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_restart](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_restart] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_restart_requests]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_restart_requests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[restartid] [int] NULL,
	[environment] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_restart_requests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_roles]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_roles](
	[roleid] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[groupid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_roles] PRIMARY KEY CLUSTERED 
(
	[roleid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_rooms]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_rooms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_rooms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_rotator_header]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_rotator_header](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[imageurl] [varchar](100) NULL,
	[impressions] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_rotator_header] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_RR_virtual_workstations_accounts]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_RR_virtual_workstations_accounts](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[workstationid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_scheduling]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_scheduling](
	[schd_id] [int] IDENTITY(1,1) NOT NULL,
	[event] [varchar](50) NULL,
	[facilitator] [varchar](50) NULL,
	[NetMeeting] [varchar](30) NULL,
	[ConfLine] [varchar](20) NULL,
	[PassCode] [varchar](10) NULL,
	[date_sch] [datetime] NULL,
	[start_time] [varchar](15) NULL,
	[end_time] [varchar](15) NULL,
	[max_people] [int] NULL,
	[location] [varchar](50) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_scheduling] PRIMARY KEY CLUSTERED 
(
	[schd_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_scheduling_registered_users]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_scheduling_registered_users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[profile_id] [int] NULL,
	[lname] [varchar](50) NULL,
	[fname] [varchar](50) NULL,
	[phone] [varchar](20) NULL,
	[dept] [varchar](50) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
	[schd_id] [int] NULL,
 CONSTRAINT [PK_cv_scheduling_registration] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_schema]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_schema](
	[schemaid] [int] IDENTITY(1,1) NOT NULL,
	[controlid] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_schema] PRIMARY KEY CLUSTERED 
(
	[schemaid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_scripts]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_scripts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[script] [text] NULL,
	[environment] [int] NULL,
	[created] [datetime] NULL,
	[completed] [datetime] NULL,
 CONSTRAINT [PK_cv_scripts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_search]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_search](
	[searchid] [int] IDENTITY(26000,7) NOT NULL,
	[userid] [int] NULL,
	[applicationid] [int] NULL,
	[type] [varchar](1) NULL,
	[oname] [varchar](50) NULL,
	[onumber] [varchar](30) NULL,
	[oorganizationid] [int] NULL,
	[osegmentid] [int] NULL,
	[ostatus] [int] NULL,
	[oby] [int] NULL,
	[ostart] [datetime] NULL,
	[oend] [datetime] NULL,
	[department] [int] NULL,
	[dstatus] [int] NULL,
	[dstart] [datetime] NULL,
	[dend] [datetime] NULL,
	[technician] [int] NULL,
	[tstatus] [int] NULL,
	[tstart] [datetime] NULL,
	[tend] [datetime] NULL,
	[itemid] [int] NULL,
	[gstatus] [int] NULL,
	[gstart] [datetime] NULL,
	[gend] [datetime] NULL,
	[lead] [int] NULL,
	[lstatus] [int] NULL,
	[lstart] [datetime] NULL,
	[lend] [datetime] NULL,
	[skill] [varchar](100) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_search] PRIMARY KEY CLUSTERED 
(
	[searchid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_search_task]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_search_task](
	[searchid] [int] IDENTITY(26000,7) NOT NULL,
	[userid] [int] NULL,
	[applicationid] [int] NULL,
	[type] [varchar](1) NULL,
	[oname] [varchar](50) NULL,
	[onumber] [varchar](30) NULL,
	[ostatus] [int] NULL,
	[oby] [int] NULL,
	[ostart] [datetime] NULL,
	[oend] [datetime] NULL,
	[department] [int] NULL,
	[dstatus] [int] NULL,
	[dstart] [datetime] NULL,
	[dend] [datetime] NULL,
	[technician] [int] NULL,
	[tstatus] [int] NULL,
	[tstart] [datetime] NULL,
	[tend] [datetime] NULL,
	[itemid] [int] NULL,
	[gstatus] [int] NULL,
	[gstart] [datetime] NULL,
	[gend] [datetime] NULL,
	[lead] [int] NULL,
	[lstatus] [int] NULL,
	[lstart] [datetime] NULL,
	[lend] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_search_task] PRIMARY KEY CLUSTERED 
(
	[searchid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_segments]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_segments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[organizationid] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_segments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servername_applications]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servername_applications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[code] [varchar](10) NULL,
	[factory_code] [varchar](3) NULL,
	[factory_code_specific] [varchar](3) NULL,
	[zeus_array_config] [varchar](20) NULL,
	[zeus_os] [varchar](20) NULL,
	[zeus_os_version] [varchar](20) NULL,
	[zeus_build_type] [varchar](50) NULL,
	[ad_move_location] [varchar](100) NULL,
	[forecast] [int] NULL,
	[permit_no_replication] [int] NULL,
	[SolutionCode] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servername_applications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servername_applications_os]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servername_applications_os](
	[applicationid] [int] NULL,
	[osid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servername_codes]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servername_codes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sitecode] [varchar](2) NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_server_naming] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servername_components]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servername_components](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[code] [varchar](10) NULL,
	[factory_code] [varchar](3) NULL,
	[factory_code_specific] [varchar](3) NULL,
	[install] [int] NULL,
	[script] [text] NULL,
	[zeus_code] [varchar](50) NULL,
	[location] [varchar](300) NULL,
	[ad_move_location] [varchar](100) NULL,
	[pnc] [int] NULL,
	[dbase] [int] NULL,
	[reset_storage] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servername_components] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servername_components_os]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servername_components_os](
	[componentid] [int] NULL,
	[osid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servername_subapplications]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servername_subapplications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[applicationid] [int] NULL,
	[name] [varchar](100) NULL,
	[code] [varchar](10) NULL,
	[factory_code] [varchar](3) NULL,
	[factory_code_specific] [varchar](3) NULL,
	[zeus_array_config] [varchar](20) NULL,
	[zeus_os] [varchar](20) NULL,
	[zeus_os_version] [varchar](20) NULL,
	[zeus_build_type] [varchar](50) NULL,
	[ad_move_location] [varchar](100) NULL,
	[permit_no_replication] [int] NULL,
	[SolutionCode] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servername_subapplications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servernames]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servernames](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[codeid] [int] NULL,
	[prefix1] [varchar](10) NULL,
	[prefix2] [varchar](10) NULL,
	[sitecode] [varchar](10) NULL,
	[name1] [varchar](1) NULL,
	[name2] [varchar](1) NULL,
	[userid] [int] NULL,
	[name] [varchar](50) NULL,
	[available] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servernames] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servernames_factory]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servernames_factory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[os] [varchar](3) NULL,
	[location] [varchar](5) NULL,
	[mnemonic] [varchar](10) NULL,
	[environment] [varchar](3) NULL,
	[name1] [varchar](1) NULL,
	[name2] [varchar](1) NULL,
	[func] [varchar](3) NULL,
	[specific] [varchar](3) NULL,
	[userid] [int] NULL,
	[name] [varchar](50) NULL,
	[available] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servernames_pnc] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servernames_related]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servernames_related](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[clusterid] [int] NULL,
	[nameid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servernames_related] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servers]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[answerid] [int] NULL,
	[modelid] [int] NULL,
	[csmconfigid] [int] NULL,
	[clusterid] [int] NULL,
	[number] [int] NULL,
	[osid] [int] NULL,
	[spid] [int] NULL,
	[templateid] [int] NULL,
	[domainid] [int] NULL,
	[test_domainid] [int] NULL,
	[infrastructure] [int] NULL,
	[ha] [int] NULL,
	[dr] [int] NULL,
	[dr_exist] [int] NULL,
	[dr_name] [varchar](30) NULL,
	[dr_consistency] [int] NULL,
	[dr_consistencyid] [int] NULL,
	[configured] [int] NULL,
	[local_storage] [int] NULL,
	[accounts] [int] NULL,
	[fdrive] [int] NULL,
	[dba] [int] NULL,
	[pnc] [int] NULL,
	[nameid] [int] NULL,
	[dhcp] [varchar](15) NULL,
	[zeus_error] [int] NULL,
	[step] [int] NULL,
	[tsm_schedule] [int] NULL,
	[tsm_cloptset] [int] NULL,
	[tsm_register] [varchar](300) NULL,
	[tsm_define] [varchar](300) NULL,
	[tsm_output] [varchar](max) NULL,
	[tsm_bypass] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servers_accounts]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servers_accounts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[xid] [varchar](10) NULL,
	[domain] [int] NULL,
	[admin] [int] NULL,
	[localgroups] [text] NULL,
	[email] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_accounts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servers_assets]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servers_assets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[assetid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[removable] [int] NULL,
	[latest] [int] NULL,
	[dr] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_assets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servers_components]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servers_components](
	[serverid] [int] NULL,
	[componentid] [int] NULL,
	[done] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servers_components_scripts]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servers_components_scripts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[componentid] [int] NULL,
	[name] [varchar](50) NULL,
	[script] [text] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_components_scripts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servers_errors]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servers_errors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[step] [int] NULL,
	[reason] [text] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_errors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servers_generic]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servers_generic](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[vio1] [varchar](50) NULL,
	[vio2] [varchar](50) NULL,
	[vio1_dr] [varchar](50) NULL,
	[vio2_dr] [varchar](50) NULL,
	[vio1_prod] [varchar](50) NULL,
	[vio2_prod] [varchar](50) NULL,
	[ww1] [varchar](50) NULL,
	[ww2] [varchar](50) NULL,
	[ww1_dr] [varchar](50) NULL,
	[ww2_dr] [varchar](50) NULL,
	[ww1_prod] [varchar](50) NULL,
	[ww2_prod] [varchar](50) NULL,
	[dummy_name] [varchar](50) NULL,
	[dummy_name_dr] [varchar](50) NULL,
	[dummy_name_prod] [varchar](50) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_generic] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_servers_ha]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servers_ha](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[serverid_ha] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_ha] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servers_ips]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_servers_ips](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[ipaddressid] [int] NULL,
	[auto_assign] [int] NULL,
	[final] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_names_ips] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_servers_output]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_servers_output](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[type] [varchar](50) NULL,
	[output] [varchar](max) NULL,
	[created] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_servers_output] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_service_packs]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_service_packs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[number] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_service_packs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_service_requests]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_service_requests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[name] [varchar](100) NULL,
	[manager_approval] [int] NULL,
	[checkout] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_service_requests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_service_types]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_service_types](
	[typeid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[path] [varchar](200) NULL,
	[ondemand] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_service_types] PRIMARY KEY CLUSTERED 
(
	[typeid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_services]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_services](
	[serviceid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[description] [text] NULL,
	[image] [varchar](100) NULL,
	[itemid] [int] NULL,
	[typeid] [int] NULL,
	[show] [int] NULL,
	[project] [int] NULL,
	[step] [int] NULL,
	[hours] [float] NULL,
	[sla] [float] NULL,
	[can_automate] [int] NULL,
	[statement] [int] NULL,
	[upload] [int] NULL,
	[expedite] [int] NULL,
	[rr_path] [varchar](100) NULL,
	[wm_path] [varchar](100) NULL,
	[cp_path] [varchar](100) NULL,
	[vw_path] [varchar](100) NULL,
	[rejection] [int] NULL,
	[automate] [int] NULL,
	[disable_hours] [int] NULL,
	[quantity_is_device] [int] NULL,
	[multiple_quantity] [int] NULL,
	[notify_pc] [int] NULL,
	[notify_client] [int] NULL,
	[disable_customization] [int] NULL,
	[tasks] [int] NULL,
	[email] [varchar](100) NULL,
	[sametime] [int] NULL,
	[notify_green] [int] NULL,
	[notify_yellow] [int] NULL,
	[notify_red] [int] NULL,
	[workflow] [int] NULL,
	[title_override] [int] NULL,
	[title_name] [varchar](100) NULL,
	[no_slider] [int] NULL,
	[hide_sla] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_services] PRIMARY KEY CLUSTERED 
(
	[serviceid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_services_detail]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_services_detail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serviceid] [int] NULL,
	[name] [varchar](300) NULL,
	[parent] [int] NULL,
	[hours] [float] NULL,
	[additional] [float] NULL,
	[checkbox] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_services_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_services_favorites]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_services_favorites](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[serviceid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_services_favorites] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_services_folder]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_services_folder](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[description] [text] NULL,
	[image] [varchar](100) NULL,
	[parent] [int] NULL,
	[userid] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_services_folder] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_services_folders]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_services_folders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serviceid] [int] NULL,
	[folderid] [int] NULL,
	[display] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_services_folders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_services_selected]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_services_selected](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[quantity] [int] NULL,
	[cancelled] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_services_users]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_services_users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serviceid] [int] NULL,
	[userid] [int] NULL,
	[assign] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_services_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_services_workflow]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_services_workflow](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serviceid] [int] NULL,
	[nextservice] [int] NULL,
	[display] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_services_workflow] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_setting]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_setting](
	[site] [varchar](100) NULL,
	[username] [nvarchar](100) NULL,
	[password] [nvarchar](100) NULL,
	[down] [int] NULL,
	[maintenance] [datetime] NULL,
	[floating] [int] NULL,
	[personal] [int] NULL,
	[tpm_app_id] [int] NULL,
	[dr_location_id] [int] NULL,
	[confidence_100] [int] NULL,
	[pinging] [datetime] NULL,
	[os_question] [int] NULL,
	[ad_sync] [char](3) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_shelfs]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_shelfs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_shelfs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_sites]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_sites](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_sites] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_solution_codes]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_solution_codes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](30) NULL,
	[serviceid] [int] NULL,
	[modelid] [int] NULL,
	[priority] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_solution_codes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_solution_codes_locations]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_solution_codes_locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[codeid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_models_locations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_solution_selection]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_solution_selection](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[codeid] [int] NULL,
	[responseid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_solution_selection_criteria] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_status]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusID] [int] NULL,
	[StatusDesc] [varchar](250) NULL,
	[Html] [varchar](250) NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_cv_status_Created]  DEFAULT (getdate()),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_cv_status_CreatedBy]  DEFAULT ((0)),
	[Modified] [datetime] NOT NULL CONSTRAINT [DF_cv_status_Modified]  DEFAULT (getdate()),
	[ModifiedBy] [int] NOT NULL CONSTRAINT [DF_cv_status_ModifiedBy]  DEFAULT ((0)),
	[deleted] [int] NOT NULL CONSTRAINT [DF_cv_status_deleted]  DEFAULT ((0)),
 CONSTRAINT [PK_cv_status] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_storage_3rd]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_storage_3rd](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[servername] [varchar](100) NULL,
	[os] [varchar](100) NULL,
	[maintenance] [varchar](100) NULL,
	[currently] [varchar](100) NULL,
	[type] [varchar](100) NULL,
	[dr] [varchar](100) NULL,
	[performance] [varchar](100) NULL,
	[change] [varchar](100) NULL,
	[cluster] [varchar](100) NULL,
	[sql] [varchar](100) NULL,
	[version] [varchar](100) NULL,
	[dba] [int] NULL,
	[cluster_group_new] [varchar](100) NULL,
	[tsm] [int] NULL,
	[networkname] [varchar](100) NULL,
	[ipaddress] [varchar](100) NULL,
	[cluster_group_existing] [varchar](100) NULL,
	[databasesql0x] [int] NULL,
	[backupsql0x] [int] NULL,
	[newdriveletter] [varchar](100) NULL,
	[newmountpoint] [varchar](100) NULL,
	[increase] [varchar](100) NULL,
	[description] [text] NULL,
	[classid] [varchar](100) NULL,
	[environmentid] [varchar](100) NULL,
	[addressid] [int] NULL,
	[fabric] [varchar](100) NULL,
	[replicated] [varchar](100) NULL,
	[ha] [int] NULL,
	[shared] [varchar](100) NULL,
	[expand] [varchar](100) NULL,
	[amount] [float] NULL,
	[luns] [text] NULL,
	[www] [varchar](200) NULL,
	[uid] [varchar](100) NULL,
	[node] [varchar](100) NULL,
	[encname] [varchar](100) NULL,
	[encslot] [varchar](10) NULL,
	[repservername] [varchar](100) NULL,
	[repwww] [varchar](200) NULL,
	[repencname] [varchar](100) NULL,
	[repencslot] [varchar](10) NULL,
	[allocated] [float] NULL,
	[midrange] [int] NULL,
	[userid] [int] NULL,
	[end_date] [datetime] NULL,
	[filesystem] [varchar](50) NULL,
	[client_amount] [varchar](20) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_storage_3rd] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_storage_3rd_flow]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_storage_3rd_flow](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[itemid2] [int] NULL,
	[number2] [int] NULL,
	[itemid3] [int] NULL,
	[number3] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NOT NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_storage_3rd_flow] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_storage_drive_letters]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_storage_drive_letters](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[letter] [varchar](1) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_storage_drive_letters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_storage_luns]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_storage_luns](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[clusterid] [int] NULL,
	[instanceid] [int] NULL,
	[csmconfigid] [int] NULL,
	[number] [int] NULL,
	[driveid] [int] NULL,
	[performance] [varchar](30) NULL,
	[path] [varchar](100) NULL,
	[size] [float] NULL,
	[actual_size] [float] NULL,
	[size_qa] [float] NULL,
	[actual_size_qa] [float] NULL,
	[size_test] [float] NULL,
	[actual_size_test] [float] NULL,
	[replicated] [int] NULL,
	[actual_replicated] [int] NULL,
	[high_availability] [int] NULL,
	[actual_high_availability] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_storage_luns] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_storage_mount_points]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_storage_mount_points](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[lunid] [int] NULL,
	[path] [varchar](100) NULL,
	[performance] [varchar](30) NULL,
	[size] [float] NULL,
	[actual_size] [float] NULL,
	[size_qa] [float] NULL,
	[actual_size_qa] [float] NULL,
	[size_test] [float] NULL,
	[actual_size_test] [float] NULL,
	[replicated] [int] NULL,
	[actual_replicated] [int] NULL,
	[high_availability] [int] NULL,
	[actual_high_availability] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_storage_mount_points] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_subscriptions]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_subscriptions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[version] [varchar](10) NULL,
	[created] [datetime] NULL,
	[modifed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_subscriptions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_support]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_support](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[pageid] [int] NULL,
	[type] [int] NULL,
	[description] [text] NULL,
	[userid] [int] NULL,
	[comments] [text] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_support] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_survey]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_survey](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[hear] [varchar](100) NULL,
	[likes] [varchar](100) NULL,
	[usage] [varchar](25) NULL,
	[purpose] [varchar](200) NULL,
	[planning] [varchar](100) NULL,
	[expected] [varchar](100) NULL,
	[performance] [varchar](25) NULL,
	[task] [varchar](200) NULL,
	[friendly] [varchar](100) NULL,
	[support] [varchar](100) NULL,
	[functionality] [varchar](25) NULL,
	[comments] [varchar](500) NULL,
	[userid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_survey] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_tables]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_tables](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tablename] [varchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_tables] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_tables_fields]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_tables_fields](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tableid] [int] NULL,
	[fieldname] [varchar](50) NULL,
	[name] [varchar](200) NULL,
	[datatype] [varchar](2) NULL,
	[join_table] [varchar](50) NULL,
	[join_on] [varchar](30) NULL,
	[join_field] [varchar](30) NULL,
	[hidden] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_tables_fields] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_tables_fields_permissions]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_tables_fields_permissions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serviceid] [int] NULL,
	[tableid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_tables_fields_permissions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_tabs]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_tabs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[tabname] [varchar](50) NULL,
	[path] [varchar](100) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_TechAssets]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_TechAssets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[asset_typeid] [int] NULL,
	[assetname] [varchar](100) NULL,
	[salestatus] [varchar](30) NULL,
	[lastmodified] [varchar](30) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_TechAssets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_templates]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_templates](
	[templateid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](30) NULL,
	[description] [varchar](100) NULL,
	[path] [varchar](50) NULL,
	[enabled] [int] NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_templates] PRIMARY KEY CLUSTERED 
(
	[templateid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_tsm]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_tsm](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[port] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_tsm] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_tsm_cloptsets]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_tsm_cloptsets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_tsm_cloptsets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_tsm_domains]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_tsm_domains](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tsm] [int] NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_tsm_domains] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_tsm_schedules]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_tsm_schedules](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[domain] [int] NULL,
	[name] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_tsm_schedules] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_types]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[platformid] [int] NULL,
	[name] [varchar](100) NULL,
	[asset_checkin_path] [varchar](100) NULL,
	[asset_commission_path] [varchar](100) NULL,
	[asset_update_path] [varchar](100) NULL,
	[asset_decommission_path] [varchar](100) NULL,
	[asset_deploy_path] [varchar](100) NULL,
	[forecast_execution_path] [varchar](100) NULL,
	[ondemand_execution_path] [varchar](100) NULL,
	[ondemand_steps_path] [varchar](100) NULL,
	[inventory_warning] [int] NULL,
	[inventory_critical] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_userguide]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_userguide](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[pageid] [int] NULL,
	[path] [varchar](250) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_userguide] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_users]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_users](
	[userid] [int] IDENTITY(1,1) NOT NULL,
	[xid] [varchar](30) NULL,
	[fname] [varchar](100) NULL,
	[lname] [varchar](100) NULL,
	[manager] [int] NULL,
	[ismanager] [int] NULL,
	[board] [int] NULL,
	[director] [int] NULL,
	[pager] [varchar](20) NULL,
	[atid] [int] NULL,
	[phone] [varchar](20) NULL,
	[other] [text] NULL,
	[vacation] [int] NULL,
	[multiple_apps] [int] NULL,
	[add_location] [int] NULL,
	[admin] [int] NULL,
	[enabled] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_users] PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_users_at]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_users_at](
	[atid] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[deleted] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_users_at] PRIMARY KEY CLUSTERED 
(
	[atid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_users_pages]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_users_pages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[pageid] [int] NULL,
	[userid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_users_pages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_vacation]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vacation](
	[vacationid] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[application] [int] NULL,
	[start_date] [datetime] NULL,
	[morning] [int] NULL,
	[afternoon] [int] NULL,
	[vacation] [int] NULL,
	[holiday] [int] NULL,
	[personal] [int] NULL,
	[reason] [varchar](100) NULL,
	[approved] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vacation] PRIMARY KEY CLUSTERED 
(
	[vacationid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_virtual_hdds]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_virtual_hdds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[value] [float] NULL,
	[vmware] [int] NULL,
	[virtual] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_virtual_hdds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_virtual_rams]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_virtual_rams](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[value] [float] NULL,
	[vmware] [int] NULL,
	[virtual] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_virtual_rams] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_anti_affinity]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_anti_affinity](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](80) NULL,
	[clusterid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_anti_affinity] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_anti_affinitys]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_anti_affinitys](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[antiaffinityid] [int] NULL,
	[servername] [varchar](50) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_anti_affinitys] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_cluster]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_cluster](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[folderid] [int] NULL,
	[modelid] [int] NULL,
	[name] [varchar](50) NULL,
	[windows] [int] NULL,
	[linux] [int] NULL,
	[server] [int] NULL,
	[workstation] [int] NULL,
	[maximum] [int] NULL,
	[datastores_notify] [varchar](100) NULL,
	[datastores_left] [int] NULL,
	[datastores_size] [int] NULL,
	[pnc] [int] NULL,
	[at_max] [int] NULL,
	[input_failures] [float] NULL,
	[input_cpu_utilization] [float] NULL,
	[input_ram_utilization] [float] NULL,
	[input_max_ram] [float] NULL,
	[input_avg_utilization] [float] NULL,
	[input_lun_size] [float] NULL,
	[input_lun_utilization] [float] NULL,
	[input_vms_per_lun] [float] NULL,
	[input_time_lun] [float] NULL,
	[input_time_cluster] [float] NULL,
	[input_max_vms_server] [float] NULL,
	[input_max_vms_lun] [float] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_cluster_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_datacenter]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_datacenter](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[virtualcenterid] [int] NULL,
	[name] [varchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_datacenter] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_datacenters]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_vmware_datacenters](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[datacenterid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_datacenters] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_vmware_datastore]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_datastore](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[clusterid] [int] NULL,
	[name] [varchar](50) NULL,
	[storage_type] [int] NULL,
	[replicated] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_datastore] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_exceptions_nodes]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_exceptions_nodes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
 CONSTRAINT [PK_cv_vmware_exceptions_nodes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_folder]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_folder](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[datacenterid] [int] NULL,
	[name] [varchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_folder] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_folders]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_vmware_folders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[folderid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_cluster] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_vmware_guests]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_guests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[hostid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[datastoreid] [int] NULL,
	[vlanid] [int] NULL,
	[poolid] [int] NULL,
	[name] [varchar](100) NULL,
	[allocated] [float] NULL,
	[macaddress] [varchar](50) NULL,
	[done] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_guests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_host]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_host](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[clusterid] [int] NULL,
	[name] [varchar](50) NULL,
	[maximum] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_host] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_host_new]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_vmware_host_new](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[clusterid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[maintenance_mode] [int] NULL,
	[join_cluster] [int] NULL,
	[assetid] [int] NULL,
	[nameid] [int] NULL,
	[ipaddressid] [int] NULL,
	[ipaddressid_vmotion] [int] NULL,
	[step] [int] NULL,
	[error] [text] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_host_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_vmware_host_new_results]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_vmware_host_new_results](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[results] [text] NULL,
	[error] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_vmware_host_new_results] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_vmware_pool]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_pool](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[clusterid] [int] NULL,
	[name] [varchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_pools] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_templates]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_templates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_templates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_templates_class_environments]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_vmware_templates_class_environments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[templateid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_templates_class_env] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_vmware_virtual_center]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_virtual_center](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[url] [varchar](100) NULL,
	[environment] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_virtual_center] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_virtual_centers]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_vmware_virtual_centers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[virtualcenterid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_virtual_centers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_vmware_vlan]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_vmware_vlan](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[clusterid] [int] NULL,
	[name] [varchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_vlan] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_vmware_vlans]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_vmware_vlans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vmware_vlanid] [int] NULL,
	[vlanid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_vmware_vlans] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_WM_decommission_server]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_decommission_server](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[servername] [varchar](50) NULL,
	[serverid] [int] NULL,
	[poweroff] [datetime] NULL,
	[change] [varchar](20) NULL,
	[poweredoff] [datetime] NULL,
	[reason] [varchar](max) NULL,
	[blackedout] [datetime] NULL,
	[renamed] [datetime] NULL,
	[destroy] [datetime] NULL,
	[destroyed] [datetime] NULL,
	[SAN] [int] NULL,
	[CSM] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_wm_decommission_server] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_decommission_server_IM]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_WM_decommission_server_IM](
	[requestid] [int] NOT NULL,
	[itemid] [int] NOT NULL,
	[number] [int] NOT NULL,
	[serverid] [int] NULL,
	[ServerDestroyed] [int] NULL,
	[DestroyUnRack] [int] NULL,
	[DestroyWipeDrives] [int] NULL,
	[DestroyDispose] [int] NULL,
	[ServerRedeployed] [int] NULL,
	[RedeployVerifyServerModel] [nchar](10) NULL,
	[RedeployMoveServerToDeploy] [nchar](10) NULL,
	[modified] [datetime] NOT NULL,
	[deleted] [int] NOT NULL,
 CONSTRAINT [PK_cv_WM_ServerDecommission_IM] PRIMARY KEY CLUSTERED 
(
	[requestid] ASC,
	[itemid] ASC,
	[number] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_WM_generic]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_WM_generic](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[priority] [int] NULL,
	[statement] [text] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[expedite] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_WM_IDC]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_IDC](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[investigated] [varchar](50) NULL,
	[investigated_by] [int] NULL,
	[followup_date] [datetime] NULL,
	[date_engaged] [datetime] NULL,
	[phase_engaged] [varchar](50) NULL,
	[effort_size] [varchar](25) NULL,
	[involvement] [varchar](10) NULL,
	[eit_testing] [varchar](10) NULL,
	[project_class] [varchar](15) NULL,
	[enterprise_release] [varchar](50) NULL,
	[no_involve] [varchar](50) NULL,
	[idc_spoc] [int] NULL,
	[comments] [text] NULL,
	[slide_statement] [float] NULL,
	[slide_alternatives] [float] NULL,
	[slide_recommendations] [float] NULL,
	[slide_high_level] [float] NULL,
	[slide_detailed] [float] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_vijay_WM_IDC] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_iis]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_iis](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[reason] [varchar](50) NULL,
	[statement] [text] NULL,
	[expedite] [int] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_project_coordinator]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_WM_project_coordinator](
	[requestid] [int] NOT NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[project_start_date] [datetime] NULL,
	[start_d] [datetime] NULL,
	[end_d] [datetime] NULL,
	[start_p] [datetime] NULL,
	[end_p] [datetime] NULL,
	[start_e] [datetime] NULL,
	[end_e] [datetime] NULL,
	[start_c] [datetime] NULL,
	[end_c] [datetime] NULL,
	[costs] [text] NULL,
	[appsd] [datetime] NULL,
	[apped] [datetime] NULL,
	[appsp] [datetime] NULL,
	[appep] [datetime] NULL,
	[appse] [datetime] NULL,
	[appee] [datetime] NULL,
	[appsc] [datetime] NULL,
	[appec] [datetime] NULL,
	[appid] [float] NULL,
	[appexd] [float] NULL,
	[apphd] [float] NULL,
	[actid] [float] NULL,
	[acted] [float] NULL,
	[acthd] [float] NULL,
	[estid] [float] NULL,
	[ested] [float] NULL,
	[esthd] [float] NULL,
	[appip] [float] NULL,
	[appexp] [float] NULL,
	[apphp] [float] NULL,
	[actip] [float] NULL,
	[actep] [float] NULL,
	[acthp] [float] NULL,
	[estip] [float] NULL,
	[estep] [float] NULL,
	[esthp] [float] NULL,
	[appie] [float] NULL,
	[appexe] [float] NULL,
	[apphe] [float] NULL,
	[actie] [float] NULL,
	[actee] [float] NULL,
	[acthe] [float] NULL,
	[estie] [float] NULL,
	[estee] [float] NULL,
	[esthe] [float] NULL,
	[appic] [float] NULL,
	[appexc] [float] NULL,
	[apphc] [float] NULL,
	[actic] [float] NULL,
	[actec] [float] NULL,
	[acthc] [float] NULL,
	[estic] [float] NULL,
	[estec] [float] NULL,
	[esthc] [float] NULL,
	[d_done] [datetime] NULL,
	[p_done] [datetime] NULL,
	[e_done] [datetime] NULL,
	[c_done] [datetime] NULL,
	[d_hrs] [float] NULL,
	[p_hrs] [float] NULL,
	[e_hrs] [float] NULL,
	[c_hrs] [float] NULL,
	[better] [text] NULL,
	[worse] [text] NULL,
	[lessons] [text] NULL,
	[userid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_WM_project_coordinator] PRIMARY KEY CLUSTERED 
(
	[requestid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_WM_project_coordinator_hours]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_project_coordinator_hours](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[phase] [varchar](1) NULL,
	[used] [float] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_remediation]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_remediation](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[reason] [varchar](50) NULL,
	[component] [varchar](50) NULL,
	[funding] [varchar](50) NULL,
	[priority] [int] NULL,
	[tpm] [int] NULL,
	[statement] [text] NULL,
	[devices] [int] NULL,
	[hours] [float] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[cc_number] [varchar](15) NULL,
	[cc_date] [varchar](10) NULL,
	[cc_time] [varchar](10) NULL,
	[cc_comments] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_server_archive]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_server_archive](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[servername] [varchar](50) NULL,
	[modelid] [int] NULL,
	[appcode] [varchar](3) NULL,
	[classid] [int] NULL,
	[end_date] [datetime] NULL,
	[statement] [text] NULL,
	[T1] [int] NULL,
	[T2] [int] NULL,
	[T3] [int] NULL,
	[G1] [int] NULL,
	[G2] [int] NULL,
	[G3] [int] NULL,
	[G4] [int] NULL,
	[G5] [int] NULL,
	[G6] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_server_retrieve]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_server_retrieve](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[servername] [varchar](50) NULL,
	[backto] [varchar](30) NULL,
	[modelid] [int] NULL,
	[appcode] [varchar](3) NULL,
	[classid] [int] NULL,
	[end_date] [datetime] NULL,
	[statement] [text] NULL,
	[G1] [int] NULL,
	[G2] [int] NULL,
	[G3] [int] NULL,
	[G4] [int] NULL,
	[G5] [int] NULL,
	[hostname] [varchar](50) NULL,
	[TV1] [int] NULL,
	[TV2] [int] NULL,
	[TP1] [int] NULL,
	[TP2] [int] NULL,
	[TP3] [int] NULL,
	[TP4] [int] NULL,
	[TP5] [int] NULL,
	[TP6] [int] NULL,
	[TP7] [int] NULL,
	[TP8] [int] NULL,
	[TP9] [int] NULL,
	[TP10] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_storage]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_storage](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[assetid] [int] NULL,
	[lunid] [int] NULL,
	[mountid] [int] NULL,
	[amount] [float] NULL,
	[high] [int] NULL,
	[high_total] [float] NULL,
	[high_qa] [float] NULL,
	[high_test] [float] NULL,
	[high_replicated] [float] NULL,
	[high_level] [varchar](20) NULL,
	[standard] [int] NULL,
	[standard_total] [float] NULL,
	[standard_qa] [float] NULL,
	[standard_test] [float] NULL,
	[standard_replicated] [float] NULL,
	[standard_level] [varchar](20) NULL,
	[low] [int] NULL,
	[low_total] [float] NULL,
	[low_qa] [float] NULL,
	[low_test] [float] NULL,
	[low_replicated] [float] NULL,
	[low_level] [varchar](20) NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_third_tier_distributed]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_WM_third_tier_distributed](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[priority] [int] NULL,
	[statement] [text] NULL,
	[hours] [float] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_WM_tpm]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_tpm](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NOT NULL,
	[priority] [int] NULL,
	[statement] [text] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[financials_exclude] [int] NULL,
	[start_d] [datetime] NULL,
	[end_d] [datetime] NULL,
	[start_p] [datetime] NULL,
	[end_p] [datetime] NULL,
	[start_e] [datetime] NULL,
	[end_e] [datetime] NULL,
	[start_c] [datetime] NULL,
	[end_c] [datetime] NULL,
	[costs] [text] NULL,
	[ppm] [varchar](20) NULL,
	[appsd] [datetime] NULL,
	[apped] [datetime] NULL,
	[appsp] [datetime] NULL,
	[appep] [datetime] NULL,
	[appse] [datetime] NULL,
	[appee] [datetime] NULL,
	[appsc] [datetime] NULL,
	[appec] [datetime] NULL,
	[appid] [float] NULL,
	[appexd] [float] NULL,
	[apphd] [float] NULL,
	[actid] [float] NULL,
	[acted] [float] NULL,
	[acthd] [float] NULL,
	[estid] [float] NULL,
	[ested] [float] NULL,
	[esthd] [float] NULL,
	[appip] [float] NULL,
	[appexp] [float] NULL,
	[apphp] [float] NULL,
	[actip] [float] NULL,
	[actep] [float] NULL,
	[acthp] [float] NULL,
	[estip] [float] NULL,
	[estep] [float] NULL,
	[esthp] [float] NULL,
	[appie] [float] NULL,
	[appexe] [float] NULL,
	[apphe] [float] NULL,
	[actie] [float] NULL,
	[actee] [float] NULL,
	[acthe] [float] NULL,
	[estie] [float] NULL,
	[estee] [float] NULL,
	[esthe] [float] NULL,
	[appic] [float] NULL,
	[appexc] [float] NULL,
	[apphc] [float] NULL,
	[actic] [float] NULL,
	[actec] [float] NULL,
	[acthc] [float] NULL,
	[estic] [float] NULL,
	[estec] [float] NULL,
	[esthc] [float] NULL,
	[sharepoint] [varchar](200) NULL,
	[better] [text] NULL,
	[worse] [text] NULL,
	[lessons] [text] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_WM_workstation]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_WM_workstation](
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[reason] [varchar](50) NULL,
	[statement] [text] NULL,
	[expedite] [int] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_workstation_components]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_workstation_components](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[zeus_build_type] [varchar](50) NULL,
	[ad_move_location] [varchar](100) NULL,
	[sms_install] [int] NULL,
	[script] [text] NULL,
	[workstation_group] [varchar](100) NULL,
	[user_group] [varchar](100) NULL,
	[notifications] [varchar](100) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstation_components] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_workstation_components_os]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_workstation_components_os](
	[componentid] [int] NULL,
	[osid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_workstation_names]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_workstation_names](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[environment] [varchar](1) NULL,
	[code] [varchar](2) NULL,
	[identifier] [varchar](1) NULL,
	[prefix1] [varchar](1) NULL,
	[prefix2] [varchar](1) NULL,
	[prefix3] [varchar](1) NULL,
	[prefix4] [varchar](1) NULL,
	[prefix5] [varchar](1) NULL,
	[prefix6] [varchar](1) NULL,
	[available] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstation_names] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_workstation_pools]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_workstation_pools](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[description] [varchar](max) NULL,
	[contact1] [int] NULL,
	[contact2] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[modifiedby] [int] NULL,
	[deleted] [int] NOT NULL,
 CONSTRAINT [PK_cv_workstation_pools] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_workstation_pools_status]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_workstation_pools_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workstationid] [int] NULL,
	[xid] [varchar](30) NULL,
	[checkedout] [datetime] NULL,
	[checkedin] [datetime] NULL,
 CONSTRAINT [PK_cv_workstation_pools_status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_workstation_pools_workstations]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_workstation_pools_workstations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[poolid] [int] NULL,
	[workstationid] [int] NULL,
	[display] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[modifiedby] [int] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstation_pools_workstations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_workstation_virtual]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_workstation_virtual](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[answerid] [int] NULL,
	[number] [int] NULL,
	[vmware] [int] NULL,
	[modelid] [int] NULL,
	[osid] [int] NULL,
	[spid] [int] NULL,
	[virtualhostid] [int] NULL,
	[domainid] [int] NULL,
	[ramid] [int] NULL,
	[hddid] [int] NULL,
	[nameid] [int] NULL,
	[configured] [int] NULL,
	[accounts] [int] NULL,
	[assetid] [int] NULL,
	[networkid] [int] NULL,
	[remoteid] [int] NULL,
	[dhcp] [varchar](15) NULL,
	[zeus_error] [int] NULL,
	[step] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_virtual_builds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_workstation_virtual_accounts]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_workstation_virtual_accounts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[workstationid] [int] NULL,
	[userid] [int] NULL,
	[admin] [int] NULL,
	[remote] [int] NULL,
	[done] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstation_virtual_accounts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_workstation_virtual_decom]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_workstation_virtual_decom](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[nameid] [int] NULL,
	[created] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstation_virtual_decom] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_workstation_virtual_errors]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_workstation_virtual_errors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workstationid] [int] NULL,
	[step] [int] NULL,
	[reason] [text] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstation_virtual_errors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_workstations]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_workstations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[answerid] [int] NULL,
	[number] [int] NULL,
	[osid] [int] NULL,
	[spid] [int] NULL,
	[domainid] [int] NULL,
	[configured] [int] NULL,
	[assetid] [int] NULL,
	[name] [varchar](50) NULL,
	[ipaddressid] [int] NULL,
	[dhcp] [varchar](15) NULL,
	[step] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_workstations_components]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_workstations_components](
	[workstationid] [int] NULL,
	[componentid] [int] NULL,
	[done] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_workstations_components_scripts]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_workstations_components_scripts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[componentid] [int] NULL,
	[name] [varchar](50) NULL,
	[script] [text] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstations_components_scripts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_zeus_builds]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_zeus_builds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serverid] [int] NULL,
	[serial] [varchar](50) NULL,
	[asset] [varchar](20) NULL,
	[name] [varchar](100) NULL,
	[array_config] [varchar](50) NULL,
	[os] [varchar](20) NULL,
	[os_version] [varchar](20) NULL,
	[sp] [int] NULL,
	[build_type] [varchar](50) NULL,
	[domain] [varchar](30) NULL,
	[status] [int] NULL,
	[dhcp] [varchar](15) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_zeus_builds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rp_Forecast]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rp_Forecast](
	[db] [int] NULL,
	[web] [int] NULL,
	[os] [int] NULL,
	[cores] [int] NULL,
	[ram] [int] NULL,
	[special] [int] NULL,
	[high_available] [int] NULL,
	[dr] [int] NULL,
	[recovery] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[rpt_Portfolios]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rpt_Portfolios](
	[portfolio] [varchar](100) NULL,
	[value] [varchar](100) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sv_workstation_ping]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sv_workstation_ping](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[workstation] [varchar](20) NULL,
	[enabled] [int] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_workstation_ping] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sv_workstation_ping_results]    Script Date: 07/31/2009 11:07:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sv_workstation_ping_results](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent] [int] NULL,
	[online] [int] NULL,
	[modified] [datetime] NULL,
 CONSTRAINT [PK_cv_workstation_ping_results] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
