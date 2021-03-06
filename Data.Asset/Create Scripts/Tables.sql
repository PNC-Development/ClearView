USE [ClearViewAsset]
GO
/****** Object:  Table [dbo].[cva_assets]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_assets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[orderid] [int] NULL,
	[modelid] [int] NULL,
	[serial] [varchar](50) NULL,
	[asset] [varchar](20) NULL,
	[bad] [int] NULL,
	[validated] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_assets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_blades]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_blades](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[enclosureid] [int] NULL,
	[ilo] [varchar](15) NULL,
	[dummy_name] [varchar](50) NULL,
	[macaddress] [varchar](50) NULL,
	[vlan] [int] NULL,
	[slot] [int] NULL,
	[spare] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_blades] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_decommissions]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_decommissions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[assetid] [int] NULL,
	[userid] [int] NULL,
	[reason] [text] NULL,
	[decom] [datetime] NULL,
	[turnedoff] [datetime] NULL,
	[destroy] [datetime] NULL,
	[destroyed] [datetime] NULL,
	[vmware] [int] NULL,
	[name] [varchar](50) NULL,
	[active] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_decoms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_enclosures]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_enclosures](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[roomid] [int] NULL,
	[rackid] [int] NULL,
	[rackposition] [varchar](10) NULL,
	[vlan] [int] NULL,
	[oa_ip] [varchar](15) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_enclosures] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_enclosures_dr]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_enclosures_dr](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[enclosureid] [int] NULL,
	[drid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_enclosures_dr] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_enclosures_vc]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_enclosures_vc](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[enclosureid] [int] NULL,
	[virtual_connect] [varchar](15) NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_enclosures_vc] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_guests]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_guests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[hostid] [int] NULL,
	[ram] [float] NULL,
	[processors] [float] NULL,
	[storage] [float] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[classid_move] [int] NULL,
	[environmentid_move] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_guests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_hba]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_hba](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[name] [varchar](100) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_hba] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_hosts_locations]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_hosts_locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_hosts_environments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_hosts_virtual]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_hosts_virtual](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[hostid] [int] NULL,
	[guests] [int] NULL,
	[processors] [float] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_virtual_hosts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_hosts_virtual_environment]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_hosts_virtual_environment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[environment] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_hosts_virtual_environment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_hosts_virtual_os]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_hosts_virtual_os](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[osid] [int] NULL,
	[virtualdir] [varchar](100) NULL,
	[gzippath] [varchar](100) NULL,
	[image] [varchar](100) NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_hosts_virtual_os] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_hosts_vmware]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_hosts_vmware](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[hostid] [int] NULL,
	[guests] [int] NULL,
	[processors] [float] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_assets_hosts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_ips]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_ips](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[ipaddressid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ips] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_network]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_network](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[depotid] [int] NULL,
	[depotroomid] [int] NULL,
	[shelfid] [int] NULL,
	[available_ports] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[roomid] [int] NULL,
	[rackid] [int] NULL,
	[rackposition] [varchar](10) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_wan_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_orders]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tracking] [varchar](50) NULL,
	[name] [varchar](50) NULL,
	[quantity] [int] NULL,
	[modelid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[confidenceid] [int] NULL,
	[ordered] [datetime] NULL,
	[status] [int] NULL,
	[comments] [varchar](max) NULL,
	[received] [int] NULL,
	[show] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_orders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_reservations]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cva_reservations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[buildid] [int] NULL,
	[reserveid] [int] NULL,
	[removable] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_reservations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cva_search]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_search](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [int] NULL,
	[type] [varchar](1) NULL,
	[name] [varchar](100) NULL,
	[serial] [varchar](50) NULL,
	[asset] [varchar](20) NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[platformid] [int] NULL,
	[typeid] [int] NULL,
	[modelid] [int] NULL,
	[depotid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_assets_search] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_server]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_server](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[roomid] [int] NULL,
	[rackid] [int] NULL,
	[rackposition] [varchar](10) NULL,
	[ilo] [varchar](15) NULL,
	[dummy_name] [varchar](50) NULL,
	[macaddress] [varchar](50) NULL,
	[vlan] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_server_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Asset ID - matches cva_assets table
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cva_server', @level2type=N'COLUMN',@level2name=N'assetid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'matches cv_classs table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cva_server', @level2type=N'COLUMN',@level2name=N'classid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'matches cv_environment table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cva_server', @level2type=N'COLUMN',@level2name=N'environmentid'
GO
/****** Object:  Table [dbo].[cva_status]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[name] [varchar](100) NULL,
	[status] [int] NULL,
	[userid] [int] NULL,
	[datestamp] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_assets_status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_status_list]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_status_list](
	[id] [int] NOT NULL,
	[name] [varchar](50) NULL,
	[display] [int] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cva_vsg]    Script Date: 07/31/2009 13:22:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cva_vsg](
	[name] [varchar](50) NULL,
	[type] [varchar](20) NULL,
	[created] [datetime] NULL,
	[assignedon] [datetime] NULL,
	[assignedto] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF