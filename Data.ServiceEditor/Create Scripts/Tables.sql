USE [ClearViewServiceEditor]
GO
/****** Object:  Table [dbo].[set_config]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_config](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serviceid] [int] NULL,
	[fieldid] [int] NULL,
	[question] [text] NULL,
	[dbfield] [int] NULL,
	[length] [int] NULL,
	[width] [int] NULL,
	[height] [int] NULL,
	[checked] [int] NULL,
	[direction] [int] NULL,
	[multiple] [int] NULL,
	[tip] [text] NULL,
	[required] [int] NULL,
	[required_text] [text] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_set_config] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_config_values]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_config_values](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[configid] [int] NULL,
	[value] [text] NULL,
	[display] [int] NULL,
	[created] [datetime] NOT NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_set_config_values] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_fields]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[set_fields](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[type] [varchar](10) NULL,
	[code] [varchar](10) NULL,
	[description] [text] NULL,
	[image] [varchar](100) NULL,
	[length] [int] NULL,
	[width] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_set_fields] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[set_fields_db]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_fields_db](
	[id] [int] IDENTITY(1000000,1) NOT NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_set_db] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_0]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_0](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000135] [text] NULL,
	[1000279] [text] NULL,
	[1000642] [text] NULL,
	[1000645] [int] NULL,
	[1000674] [datetime] NULL,
	[1000680] [int] NULL,
	[1000682] [text] NULL,
	[1000694] [datetime] NULL,
	[1000700] [datetime] NULL,
	[1000701] [datetime] NULL,
	[1000703] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_326]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_326](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000040] [text] NULL,
	[1000041] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_330]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_330](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000054] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_344]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_344](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000006] [text] NULL,
	[1000475] [text] NULL,
	[1000493] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_345]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_345](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000010] [text] NULL,
	[1000479] [text] NULL,
	[1000499] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_346]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_346](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000014] [text] NULL,
	[1000015] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_347]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_347](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000047] [text] NULL,
	[1000131] [text] NULL,
	[1000132] [text] NULL,
	[1000489] [text] NULL,
	[1000512] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_348]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_348](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000487] [text] NULL,
	[1000509] [text] NULL,
	[1000510] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_349]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_349](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000250] [text] NULL,
	[1000252] [text] NULL,
	[1000253] [text] NULL,
	[1000482] [text] NULL,
	[1000503] [text] NULL,
	[1000504] [text] NULL,
	[1000630] [text] NULL,
	[1000844] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_350]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_350](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000481] [text] NULL,
	[1000501] [text] NULL,
	[1000502] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_351]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_351](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000021] [text] NULL,
	[1000079] [text] NULL,
	[1000130] [text] NULL,
	[1000485] [text] NULL,
	[1000507] [text] NULL,
	[1000524] [text] NULL,
	[1000525] [text] NULL,
	[1000526] [text] NULL,
	[1000528] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_352]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_352](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000090] [text] NULL,
	[1000091] [text] NULL,
	[1000254] [text] NULL,
	[1000255] [text] NULL,
	[1000488] [text] NULL,
	[1000511] [text] NULL,
	[1000843] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_353]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_353](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000077] [text] NULL,
	[1000078] [text] NULL,
	[1000318] [text] NULL,
	[1000484] [text] NULL,
	[1000506] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_354]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_354](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_355]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_355](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000072] [text] NULL,
	[1000472] [text] NULL,
	[1000490] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_356]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_356](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000083] [text] NULL,
	[1000473] [text] NULL,
	[1000491] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_358]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_358](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000075] [text] NULL,
	[1000084] [text] NULL,
	[1000474] [text] NULL,
	[1000492] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_359]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_359](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000076] [text] NULL,
	[1000483] [text] NULL,
	[1000505] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_360]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_360](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000476] [text] NULL,
	[1000494] [text] NULL,
	[1000495] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_363]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_363](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000002] [int] NULL,
	[1000003] [text] NULL,
	[1000004] [int] NULL,
	[1000023] [text] NULL,
	[1000025] [text] NULL,
	[1000027] [text] NULL,
	[1000028] [text] NULL,
	[1000029] [text] NULL,
	[1000033] [text] NULL,
	[1000045] [text] NULL,
	[1000046] [text] NULL,
	[1000181] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_364]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_364](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000049] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_374]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_374](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000388] [text] NULL,
	[1000389] [text] NULL,
	[1000393] [int] NULL,
	[1000394] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_385]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_385](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_386]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_386](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_387]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_387](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_421]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_421](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000011] [text] NULL,
	[1000012] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_422]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_422](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000017] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_423]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_423](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000065] [text] NULL,
	[1000067] [text] NULL,
	[1000081] [text] NULL,
	[1000082] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_424]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_424](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000330] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_425]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_425](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_426]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_426](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_427]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_427](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_428]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_428](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_429]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_429](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_430]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_430](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_431]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_431](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_432]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_432](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000270] [text] NULL,
	[1000271] [text] NULL,
	[1000272] [text] NULL,
	[1000273] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_433]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_433](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000319] [text] NULL,
	[1000320] [text] NULL,
	[1000321] [text] NULL,
	[1000322] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_434]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_434](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_435]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_435](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000323] [text] NULL,
	[1000324] [text] NULL,
	[1000326] [text] NULL,
	[1000327] [text] NULL,
	[1000328] [text] NULL,
	[1000329] [text] NULL,
	[1000346] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_436]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_436](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_437]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_437](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_438]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_438](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_439]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_439](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_440]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_440](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_441]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_441](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_442]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_442](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_443]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_443](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_444]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_444](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_445]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_445](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000110] [text] NULL,
	[1000111] [text] NULL,
	[1000116] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_446]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_446](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000112] [text] NULL,
	[1000113] [text] NULL,
	[1000117] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_447]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_447](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_448]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_448](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000125] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_449]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_449](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000101] [text] NULL,
	[1000265] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_450]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_450](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000127] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_451]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_451](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000123] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_452]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_452](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000126] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_453]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_453](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000122] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_454]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_454](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000128] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_455]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_455](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000124] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_456]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_456](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000121] [text] NULL,
	[1000597] [text] NULL,
	[1000598] [text] NULL,
	[1000602] [text] NULL,
	[1000603] [text] NULL,
	[1000604] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_457]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_457](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_458]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_458](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000599] [text] NULL,
	[1000600] [text] NULL,
	[1000601] [text] NULL,
	[1000605] [text] NULL,
	[1000607] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_459]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_459](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_460]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_460](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000413] [text] NULL,
	[1000414] [text] NULL,
	[1000416] [text] NULL,
	[1000631] [text] NULL,
	[1000634] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_463]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_463](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_464]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_464](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_465]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_465](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_466]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_466](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_467]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_467](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_468]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_468](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000050] [text] NULL,
	[1000051] [text] NULL,
	[1000559] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_469]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_469](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_470]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_470](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_471]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_471](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_472]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_472](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_473]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_473](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_474]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_474](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_475]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_475](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_476]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_476](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_477]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_477](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000260] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_478]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_478](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_479]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_479](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_480]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_480](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_481]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_481](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_482]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_482](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_483]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_483](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_484]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_484](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000189] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_485]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_485](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_486]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_486](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_487]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_487](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_488]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_488](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000191] [text] NULL,
	[1000192] [text] NULL,
	[1000193] [text] NULL,
	[1000194] [text] NULL,
	[1000196] [text] NULL,
	[1000197] [text] NULL,
	[1000200] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_489]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_489](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000259] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_490]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_490](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000056] [text] NULL,
	[1000058] [text] NULL,
	[1000059] [text] NULL,
	[1000060] [text] NULL,
	[1000062] [text] NULL,
	[1000063] [int] NULL,
	[1000552] [text] NULL,
	[1000554] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_491]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_491](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000236] [text] NULL,
	[1000237] [text] NULL,
	[1000556] [text] NULL,
	[1000557] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_492]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_492](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000233] [text] NULL,
	[1000234] [text] NULL,
	[1000235] [text] NULL,
	[1000238] [text] NULL,
	[1000553] [text] NULL,
	[1000555] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_493]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_493](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_494]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_494](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_495]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_495](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000064] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_496]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_496](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000042] [text] NULL,
	[1000043] [int] NULL,
	[1000044] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_497]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_497](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_499]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_499](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000208] [text] NULL,
	[1000478] [text] NULL,
	[1000497] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_500]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_500](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000214] [text] NULL,
	[1000477] [text] NULL,
	[1000496] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_503]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_503](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000068] [text] NULL,
	[1000070] [text] NULL,
	[1000080] [text] NULL,
	[1000486] [text] NULL,
	[1000508] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_504]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_504](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_505]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_505](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000105] [text] NULL,
	[1000106] [text] NULL,
	[1000119] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_506]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_506](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000085] [text] NULL,
	[1000087] [text] NULL,
	[1000088] [text] NULL,
	[1000198] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_507]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_507](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000089] [text] NULL,
	[1000109] [text] NULL,
	[1000115] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_508]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_508](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000103] [text] NULL,
	[1000104] [text] NULL,
	[1000118] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_509]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_509](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000107] [text] NULL,
	[1000108] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_510]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_510](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_511]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_511](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_512]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_512](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000120] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_513]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_513](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_514]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_514](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_515]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_515](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_516]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_516](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_517]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_517](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_518]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_518](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000093] [int] NULL,
	[1000100] [text] NULL,
	[1000102] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_519]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_519](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000133] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_520]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_520](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000134] [text] NULL,
	[1000411] [text] NULL,
	[1000539] [text] NULL,
	[1000632] [text] NULL,
	[1000633] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_521]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_521](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000136] [text] NULL,
	[1000137] [text] NULL,
	[1000138] [int] NULL,
	[1000139] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_524]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_524](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000154] [int] NULL,
	[1000156] [text] NULL,
	[1000159] [text] NULL,
	[1000161] [text] NULL,
	[1000164] [text] NULL,
	[1000165] [text] NULL,
	[1000166] [text] NULL,
	[1000182] [text] NULL,
	[1000183] [text] NULL,
	[1000187] [text] NULL,
	[1000213] [text] NULL,
	[1000256] [text] NULL,
	[1000288] [text] NULL,
	[1000289] [text] NULL,
	[1000290] [text] NULL,
	[1000291] [text] NULL,
	[1000292] [text] NULL,
	[1000293] [text] NULL,
	[1000294] [text] NULL,
	[1000295] [text] NULL,
	[1000352] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_525]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_525](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000140] [int] NULL,
	[1000141] [int] NULL,
	[1000142] [int] NULL,
	[1000143] [text] NULL,
	[1000145] [text] NULL,
	[1000146] [text] NULL,
	[1000147] [text] NULL,
	[1000148] [text] NULL,
	[1000149] [text] NULL,
	[1000150] [text] NULL,
	[1000151] [text] NULL,
	[1000152] [text] NULL,
	[1000153] [text] NULL,
	[1000158] [text] NULL,
	[1000160] [text] NULL,
	[1000162] [text] NULL,
	[1000167] [text] NULL,
	[1000168] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_526]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_526](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000169] [int] NULL,
	[1000170] [text] NULL,
	[1000171] [text] NULL,
	[1000172] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_527]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_527](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000173] [int] NULL,
	[1000175] [int] NULL,
	[1000179] [text] NULL,
	[1000180] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_528]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_528](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000176] [int] NULL,
	[1000177] [text] NULL,
	[1000178] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_529]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_529](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_530]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_530](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_531]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_531](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_532]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_532](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000190] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_533]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_533](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000201] [text] NULL,
	[1000202] [text] NULL,
	[1000471] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_534]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_534](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000210] [text] NULL,
	[1000211] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_538]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_538](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000498] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_539]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_539](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000215] [text] NULL,
	[1000216] [text] NULL,
	[1000217] [int] NULL,
	[1000218] [text] NULL,
	[1000222] [text] NULL,
	[1000223] [datetime] NULL,
	[1000224] [text] NULL,
	[1000225] [text] NULL,
	[1000226] [text] NULL,
	[1000227] [text] NULL,
	[1000228] [text] NULL,
	[1000230] [text] NULL,
	[1000232] [datetime] NULL,
	[1000400] [text] NULL,
	[1000402] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_540]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_540](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_541]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_541](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000239] [text] NULL,
	[1000240] [text] NULL,
	[1000241] [int] NULL,
	[1000244] [text] NULL,
	[1000257] [text] NULL,
	[1000830] [text] NULL,
	[1000831] [text] NULL,
	[1000832] [text] NULL,
	[1000833] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_542]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_542](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_543]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_543](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_544]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_544](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000246] [text] NULL,
	[1000247] [text] NULL,
	[1000261] [text] NULL,
	[1000558] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_545]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_545](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_548]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_548](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_549]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_549](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_550]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_550](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_551]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_551](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000262] [text] NULL,
	[1000263] [text] NULL,
	[1000480] [text] NULL,
	[1000500] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_552]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_552](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_553]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_553](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_554]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_554](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_555]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_555](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_559]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_559](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000266] [int] NULL,
	[1000267] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_560]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_560](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_561]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_561](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_562]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_562](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_563]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_563](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000268] [int] NULL,
	[1000269] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_564]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_564](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_565]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_565](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_566]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_566](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_567]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_567](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000277] [text] NULL,
	[1000421] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_568]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_568](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000278] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_569]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_569](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_570]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_570](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_571]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_571](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000284] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_572]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_572](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000299] [text] NULL,
	[1000300] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_573]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_573](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000301] [text] NULL,
	[1000302] [text] NULL,
	[1000566] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_574]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_574](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_575]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_575](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000274] [text] NULL,
	[1000275] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_576]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_576](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000285] [text] NULL,
	[1000286] [text] NULL,
	[1000287] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_577]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_577](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000297] [text] NULL,
	[1000298] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_579]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_579](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000303] [int] NULL,
	[1000304] [text] NULL,
	[1000311] [text] NULL,
	[1000312] [text] NULL,
	[1000313] [text] NULL,
	[1000316] [text] NULL,
	[1000317] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_580]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_580](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000331] [text] NULL,
	[1000332] [text] NULL,
	[1000333] [text] NULL,
	[1000334] [text] NULL,
	[1000335] [text] NULL,
	[1000347] [text] NULL,
	[1000348] [text] NULL,
	[1000349] [text] NULL,
	[1000350] [text] NULL,
	[1000618] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_581]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_581](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000336] [text] NULL,
	[1000337] [text] NULL,
	[1000338] [text] NULL,
	[1000339] [text] NULL,
	[1000340] [text] NULL,
	[1000341] [text] NULL,
	[1000342] [text] NULL,
	[1000343] [text] NULL,
	[1000344] [text] NULL,
	[1000345] [text] NULL,
	[1000619] [text] NULL,
	[1000620] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_583]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_583](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000351] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_584]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_584](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000353] [int] NULL,
	[1000355] [text] NULL,
	[1000356] [text] NULL,
	[1000359] [text] NULL,
	[1000365] [text] NULL,
	[1000366] [text] NULL,
	[1000375] [text] NULL,
	[1000376] [text] NULL,
	[1000377] [text] NULL,
	[1000378] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_585]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_585](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_586]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_586](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000380] [text] NULL,
	[1000381] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_587]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_587](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000382] [text] NULL,
	[1000383] [text] NULL,
	[1000384] [text] NULL,
	[1000386] [text] NULL,
	[1000387] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_588]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_588](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_589]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_589](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_590]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_590](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_591]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_591](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000385] [text] NULL,
	[1000390] [text] NULL,
	[1000391] [text] NULL,
	[1000392] [text] NULL,
	[1000397] [text] NULL,
	[1000398] [text] NULL,
	[1000399] [text] NULL,
	[1000449] [text] NULL,
	[1000450] [text] NULL,
	[1000459] [text] NULL,
	[1000460] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_592]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_592](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_593]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_593](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000396] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_594]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_594](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_595]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_595](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_596]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_596](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000401] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_598]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_598](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000403] [text] NULL,
	[1000404] [text] NULL,
	[1000405] [text] NULL,
	[1000406] [text] NULL,
	[1000407] [text] NULL,
	[1000408] [text] NULL,
	[1000412] [text] NULL,
	[1000417] [text] NULL,
	[1000418] [text] NULL,
	[1000419] [text] NULL,
	[1000420] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_599]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_599](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_600]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_600](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000422] [text] NULL,
	[1000423] [text] NULL,
	[1000424] [text] NULL,
	[1000425] [text] NULL,
	[1000426] [text] NULL,
	[1000427] [text] NULL,
	[1000428] [text] NULL,
	[1000429] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_601]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_601](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000430] [text] NULL,
	[1000431] [text] NULL,
	[1000432] [text] NULL,
	[1000433] [text] NULL,
	[1000434] [text] NULL,
	[1000435] [text] NULL,
	[1000436] [text] NULL,
	[1000538] [text] NULL,
	[1000562] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_602]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_602](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000437] [int] NULL,
	[1000438] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_603]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_603](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000442] [text] NULL,
	[1000443] [text] NULL,
	[1000444] [text] NULL,
	[1000445] [text] NULL,
	[1000446] [text] NULL,
	[1000447] [text] NULL,
	[1000448] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_605]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_605](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_606]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_606](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_607]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_607](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000440] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_608]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_608](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000441] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_609]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_609](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_610]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_610](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000439] [int] NULL,
	[1000470] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_611]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_611](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_612]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_612](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000452] [text] NULL,
	[1000453] [text] NULL,
	[1000454] [text] NULL,
	[1000456] [text] NULL,
	[1000457] [text] NULL,
	[1000458] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_613]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_613](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_614]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_614](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_615]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_615](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_616]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_616](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000461] [text] NULL,
	[1000462] [text] NULL,
	[1000463] [datetime] NULL,
	[1000464] [text] NULL,
	[1000465] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_617]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_617](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000466] [text] NULL,
	[1000467] [datetime] NULL,
	[1000468] [text] NULL,
	[1000469] [text] NULL,
	[1000540] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_618]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_618](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_619]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_619](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_621]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_621](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000513] [text] NULL,
	[1000514] [text] NULL,
	[1000515] [text] NULL,
	[1000516] [text] NULL,
	[1000517] [text] NULL,
	[1000518] [text] NULL,
	[1000519] [text] NULL,
	[1000520] [datetime] NULL,
	[1000521] [text] NULL,
	[1000522] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_622]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_622](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000523] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_624]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_624](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_625]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_625](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000529] [int] NULL,
	[1000530] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_626]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_626](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000531] [text] NULL,
	[1000532] [datetime] NULL,
	[1000533] [text] NULL,
	[1000534] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_627]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_627](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000535] [text] NULL,
	[1000536] [text] NULL,
	[1000537] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_628]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_628](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000541] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_629]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_629](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_630]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_630](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000543] [int] NULL,
	[1000544] [int] NULL,
	[1000545] [datetime] NULL,
	[1000548] [text] NULL,
	[1000549] [text] NULL,
	[1000550] [datetime] NULL,
	[1000551] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_631]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_631](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_632]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_632](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_633]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_633](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_634]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_634](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000561] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_635]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_635](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_636]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_636](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_637]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_637](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000564] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_638]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_638](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000563] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_639]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_639](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000565] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_645]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_645](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_646]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_646](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000567] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_647]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_647](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000568] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_648]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_648](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000569] [text] NULL,
	[1000570] [text] NULL,
	[1000571] [text] NULL,
	[1000573] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_649]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_649](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000574] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_650]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_650](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000575] [text] NULL,
	[1000576] [text] NULL,
	[1000577] [text] NULL,
	[1000578] [text] NULL,
	[1000579] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_651]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_651](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000580] [text] NULL,
	[1000581] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_652]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_652](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000582] [datetime] NULL,
	[1000583] [text] NULL,
	[1000584] [text] NULL,
	[1000585] [text] NULL,
	[1000586] [text] NULL,
	[1000587] [text] NULL,
	[1000588] [text] NULL,
	[1000589] [text] NULL,
	[1000590] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_654]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_654](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000591] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_655]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_655](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000592] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_656]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_656](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000593] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_658]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_658](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000594] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_659]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_659](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000595] [text] NULL,
	[1000596] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_660]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_660](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_661]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_661](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_663]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_663](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_664]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_664](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_665]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_665](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_666]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_666](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_668]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_668](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_669]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_669](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_671]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_671](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000804] [datetime] NULL,
	[1000805] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_672]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_672](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000816] [datetime] NULL,
	[1000817] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_673]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_673](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000808] [datetime] NULL,
	[1000809] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_674]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_674](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000810] [datetime] NULL,
	[1000811] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_675]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_675](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000771] [datetime] NULL,
	[1000772] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_676]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_676](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000824] [datetime] NULL,
	[1000825] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_677]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_677](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000818] [datetime] NULL,
	[1000819] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_678]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_678](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000773] [datetime] NULL,
	[1000774] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_679]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_679](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000781] [datetime] NULL,
	[1000782] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_680]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_680](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000617] [text] NULL,
	[1000828] [datetime] NULL,
	[1000829] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_682]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_682](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000820] [datetime] NULL,
	[1000821] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_683]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_683](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000791] [datetime] NULL,
	[1000792] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_684]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_684](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_685]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_685](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_686]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_686](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_687]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_687](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_688]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_688](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_689]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_689](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000624] [text] NULL,
	[1000625] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_690]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_690](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000626] [text] NULL,
	[1000627] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_691]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_691](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_692]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_692](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000777] [datetime] NULL,
	[1000778] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_693]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_693](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000783] [datetime] NULL,
	[1000784] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_694]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_694](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000785] [datetime] NULL,
	[1000786] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_695]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_695](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000812] [datetime] NULL,
	[1000813] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_696]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_696](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000789] [datetime] NULL,
	[1000790] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_697]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_697](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000787] [datetime] NULL,
	[1000788] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_698]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_698](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000814] [datetime] NULL,
	[1000815] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_699]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_699](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000779] [datetime] NULL,
	[1000780] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_700]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_700](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000775] [datetime] NULL,
	[1000776] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_701]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_701](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_702]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_702](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000628] [text] NULL,
	[1000629] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_703]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_703](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000802] [datetime] NULL,
	[1000803] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_704]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_704](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000806] [datetime] NULL,
	[1000807] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_705]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_705](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_706]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_706](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_707]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_707](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_708]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_708](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_710]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_710](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_711]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_711](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000751] [datetime] NULL,
	[1000752] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_712]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_712](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000743] [datetime] NULL,
	[1000744] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_713]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_713](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000745] [datetime] NULL,
	[1000746] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_714]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_714](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000749] [datetime] NULL,
	[1000750] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_715]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_715](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000740] [datetime] NULL,
	[1000742] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_716]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_716](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000759] [datetime] NULL,
	[1000760] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_717]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_717](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000761] [datetime] NULL,
	[1000762] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_718]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_718](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000763] [datetime] NULL,
	[1000764] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_720]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_720](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000757] [datetime] NULL,
	[1000758] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_721]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_721](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_722]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_722](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000765] [datetime] NULL,
	[1000766] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_723]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_723](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000755] [datetime] NULL,
	[1000756] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_724]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_724](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000767] [datetime] NULL,
	[1000768] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_725]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_725](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000769] [datetime] NULL,
	[1000770] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_726]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_726](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000822] [datetime] NULL,
	[1000823] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_727]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_727](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_728]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_728](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000753] [datetime] NULL,
	[1000754] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_729]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_729](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000800] [datetime] NULL,
	[1000801] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_730]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_730](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000826] [datetime] NULL,
	[1000827] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_731]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_731](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000635] [text] NULL,
	[1000636] [text] NULL,
	[1000637] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_732]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_732](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000638] [int] NULL,
	[1000639] [int] NULL,
	[1000640] [int] NULL,
	[1000641] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_733]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_733](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000643] [text] NULL,
	[1000647] [text] NULL,
	[1000649] [text] NULL,
	[1000650] [text] NULL,
	[1000651] [datetime] NULL,
	[1000652] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_734]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_734](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000653] [text] NULL,
	[1000654] [datetime] NULL,
	[1000655] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_735]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_735](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000656] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_737]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_737](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000663] [text] NULL,
	[1000665] [text] NULL,
	[1000845] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_738]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_738](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000658] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_739]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_739](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000659] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_740]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_740](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000660] [text] NULL,
	[1000661] [text] NULL,
	[1000662] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_741]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_741](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_742]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_742](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000667] [text] NULL,
	[1000668] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_743]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_743](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000669] [text] NULL,
	[1000670] [text] NULL,
	[1000671] [text] NULL,
	[1000672] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_744]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_744](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000673] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_745]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_745](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000675] [text] NULL,
	[1000676] [text] NULL,
	[1000677] [text] NULL,
	[1000678] [text] NULL,
	[1000679] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_746]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_746](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000681] [text] NULL,
	[1000683] [text] NULL,
	[1000684] [text] NULL,
	[1000685] [text] NULL,
	[1000686] [text] NULL,
	[1000687] [int] NULL,
	[1000688] [text] NULL,
	[1000689] [text] NULL,
	[1000690] [text] NULL,
	[1000691] [text] NULL,
	[1000692] [text] NULL,
	[1000693] [text] NULL,
	[1000695] [datetime] NULL,
	[1000696] [text] NULL,
	[1000697] [text] NULL,
	[1000698] [text] NULL,
	[1000699] [text] NULL,
	[1000702] [datetime] NULL,
	[1000704] [text] NULL,
	[1000705] [text] NULL,
	[1000706] [text] NULL,
	[1000707] [text] NULL,
	[1000708] [datetime] NULL,
	[1000709] [text] NULL,
	[1000710] [text] NULL,
	[1000711] [text] NULL,
	[1000712] [text] NULL,
	[1000713] [datetime] NULL,
	[1000714] [text] NULL,
	[1000715] [text] NULL,
	[1000716] [text] NULL,
	[1000717] [text] NULL,
	[1000718] [text] NULL,
	[1000719] [datetime] NULL,
	[1000720] [text] NULL,
	[1000721] [text] NULL,
	[1000722] [text] NULL,
	[1000723] [text] NULL,
	[1000724] [text] NULL,
	[1000725] [datetime] NULL,
	[1000726] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_750]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_750](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000727] [text] NULL,
	[1000728] [text] NULL,
	[1000729] [text] NULL,
	[1000730] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_751]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_751](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_752]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_752](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_753]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_753](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_754]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_754](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_755]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_755](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_756]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_756](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_757]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_757](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_758]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_758](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_759]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_759](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_760]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_760](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_761]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_761](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_762]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_762](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_763]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_763](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_764]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_764](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_765]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_765](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_766]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_766](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_767]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_767](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_768]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_768](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_769]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_769](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_770]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_770](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_771]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_771](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_772]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_772](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_773]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_773](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_774]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_774](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000731] [datetime] NULL,
	[1000732] [int] NULL,
	[1000739] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_775]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_775](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000734] [text] NULL,
	[1000735] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_776]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_776](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000736] [text] NULL,
	[1000737] [text] NULL,
	[1000738] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_777]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_777](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000747] [text] NULL,
	[1000795] [text] NULL,
	[1000796] [text] NULL,
	[1000846] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_778]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_778](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000797] [int] NULL,
	[1000798] [text] NULL,
	[1000799] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_779]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_779](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_782]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_782](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_783]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_783](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_795]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_795](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000834] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_796]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_796](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000835] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_797]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_797](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000836] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_798]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_798](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000837] [text] NULL,
	[1000838] [text] NULL,
	[1000839] [text] NULL,
	[1000840] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_GEN_799]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[set_GEN_799](
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[modified] [datetime] NULL,
	[1000841] [int] NULL,
	[1000842] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[set_requests]    Script Date: 07/31/2009 13:42:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[set_requests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[title] [varchar](100) NULL,
	[priority] [int] NULL,
	[statement] [text] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[expedite] [int] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_set_requests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF