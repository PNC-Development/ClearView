USE [ClearViewIP]
GO
/****** Object:  Table [dbo].[cv_ip_addresses]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ip_addresses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[networkid] [int] NULL,
	[add1] [int] NULL,
	[add2] [int] NULL,
	[add3] [int] NULL,
	[add4] [int] NULL,
	[dhcp] [int] NULL,
	[userid] [int] NULL,
	[available] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ip_addresses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ip_addresses_detail]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ip_addresses_detail](
	[ipaddressid] [int] NULL,
	[detailid] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ip_addresses_details]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ip_addresses_details](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[url] [varchar](100) NULL,
	[projectid] [int] NULL,
	[instance] [varchar](50) NULL,
	[vlan] [int] NULL,
	[serial] [varchar](50) NULL,
	[server_name] [varchar](100) NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[csm] [int] NULL,
	[type] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ip_addresses_details] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ip_dhcp]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ip_dhcp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[networkid] [int] NULL,
	[min4] [int] NULL,
	[max4] [int] NULL,
	[ips_notify] [varchar](max) NULL,
	[ips_left] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ip_dhcp] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ip_networks]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ip_networks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vlanid] [int] NULL,
	[add1] [int] NULL,
	[add2] [int] NULL,
	[add3] [int] NULL,
	[min4] [int] NULL,
	[max4] [int] NULL,
	[mask] [varchar](15) NULL,
	[gateway] [varchar](15) NULL,
	[starting] [int] NULL,
	[maximum] [int] NULL,
	[reverse] [int] NULL,
	[routable] [int] NULL,
	[notify] [varchar](100) NULL,
	[cluster_inuse] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ip_networks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ip_networks_relations]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ip_networks_relations](
	[networkid] [int] NULL,
	[related] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cv_ip_vlans]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cv_ip_vlans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vlan] [int] NULL,
	[physical_windows] [int] NULL,
	[physical_unix] [int] NULL,
	[ecom_production] [int] NULL,
	[ecom_service] [int] NULL,
	[ipx] [int] NULL,
	[virtual_workstation] [int] NULL,
	[vmware_workstation] [int] NULL,
	[vmware_host] [int] NULL,
	[vmware_vmotion] [int] NULL,
	[vmware_windows] [int] NULL,
	[vmware_linux] [int] NULL,
	[blades] [int] NULL,
	[apv] [int] NULL,
	[mainframe] [int] NULL,
	[csm] [int] NULL,
	[csm_soa] [int] NULL,
	[replicates] [int] NULL,
	[pxe] [int] NULL,
	[ilo] [int] NULL,
	[csm_vip] [int] NULL,
	[ltm_web] [int] NULL,
	[ltm_app] [int] NULL,
	[ltm_middle] [int] NULL,
	[ltm_vip] [int] NULL,
	[windows_cluster] [int] NULL,
	[unix_cluster] [int] NULL,
	[accenture] [int] NULL,
	[ha] [int] NULL,
	[sun_cluster] [int] NULL,
	[storage] [int] NULL,
	[switchname] [varchar](30) NULL,
	[vtpdomain] [varchar](30) NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ip_vlans] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cv_ip_vlans_ha]    Script Date: 07/31/2009 13:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cv_ip_vlans_ha](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[original_vlan] [int] NULL,
	[ha_vlan] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_ip_vlans_ha] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
