 use ClearViewAsset
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_module_models](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[typeid] [int] NULL,
	[name] [varchar](100) NULL,
	[part_number] [varchar](50) NULL,
	[ports] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_module_models] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_fabrics](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_fabrics] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fabricid] [int] NULL,
	[classid] [int] NULL,
	[environmentid] [int] NULL,
	[addressid] [int] NULL,
	[fabric_side] [varchar](10) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_locations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_device_aliases](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[hbaid] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_device_aliases] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_vsans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[number] [varchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_vsans] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_zonesets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vsanid] [int] NULL,
	[locationid] [int] NULL,
	[name] [varchar](100) NULL,
	[active] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_zonesets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_zone_aliases](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[zonesetid] [int] NULL,
	[name] [varchar](100) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_zone_aliases] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_module_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](max) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_module_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_switches](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[ipaddressid] [int] NULL,
	[modelid] [int] NULL,
	[os_version] [varchar](25) NULL,
	[ssh] [int] NULL,
	[rackid] [int] NULL,
	[rackposition] [varchar](10) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_switches] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_location_switches](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[locationid] [int] NULL,
	[switchid] [int] NULL,
	[core] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_location_switches] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_switch_models](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[part_number] [varchar](50) NULL,
	[slots] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_switch_models] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_ports](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[switchid] [int] NULL,
	[moduleid] [int] NULL,
	[slot] [int] NULL,
	[port] [int] NULL,
	[speedid] [int] NULL,
	[vsanid] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_ports] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_vsans_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vsanid] [int] NULL,
	[typeid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_vsans_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_vsan_types](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](100) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_vsan_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_modules](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assetid] [int] NULL,
	[modelid] [int] NULL,
	[ports_available] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_modules] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_assignments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[hbaid] [int] NULL,
	[portid] [int] NULL,
	[assigned] [datetime] NULL,
	[reclaimed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_assignments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_module_models_speeds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelid] [int] NULL,
	[speedid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_module_models_speeds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cva_storage_port_speeds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[speed] [varchar](50) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cva_storage_port_speeds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

