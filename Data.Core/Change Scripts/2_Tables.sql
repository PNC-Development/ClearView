/*==============================================================
The purpose of this script is to create /alter tables, indexes, and constraints
==============================================================*/

CREATE TABLE [dbo].[cv_design_approval_conditional](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[approve_by_field] [varchar](30) NULL,
	[approve_by_group] [int] NULL,
	[approve_by_requestor] [int] NULL,
	[approve_by_app_owner] [int] NULL,
	[approve_by_atl] [int] NULL,
	[approve_by_asm] [int] NULL,
	[approve_by_sd] [int] NULL,
	[approve_by_dm] [int] NULL,
	[approve_by_cio] [int] NULL,
	[display] [int] NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_design_approval_conditional] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[cv_design_approval_conditional_sets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[approvalid] [int] NULL,
	[field] [varchar](30) NULL,
	[is_lt] [int] NULL,
	[is_lte] [int] NULL,
	[is_gt] [int] NULL,
	[is_gte] [int] NULL,
	[is_eq] [int] NULL,
	[is_neq] [int] NULL,
	[is_in] [int] NULL,
	[is_nin] [int] NULL,
	[is_ends] [int] NULL,
	[is_starts] [int] NULL,
	[dt_int] [int] NULL,
	[dt_date] [int] NULL,
	[value] [varchar](100) NULL,
	[or_group] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_design_approval_conditional_sets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE dbo.Tmp_cv_storage_luns_disks
	(
	id int NOT NULL IDENTITY (1, 1),
	lunid int NULL,
	virtual_bus_number int NULL,
	virtual_unit_number int NULL,
	vmware_disk_uuid varchar(100) NULL,
	created datetime NULL,
	modified datetime NULL,
	deleted int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_cv_storage_luns_disks SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_cv_storage_luns_disks ON
GO
IF EXISTS(SELECT * FROM dbo.cv_storage_luns_disks)
	 EXEC('INSERT INTO dbo.Tmp_cv_storage_luns_disks (id, lunid, virtual_bus_number, virtual_unit_number, created, modified, deleted)
		SELECT id, lunid, virtual_bus_number, virtual_unit_number, created, modified, deleted FROM dbo.cv_storage_luns_disks WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_cv_storage_luns_disks OFF
GO
DROP TABLE dbo.cv_storage_luns_disks
GO
EXECUTE sp_rename N'dbo.Tmp_cv_storage_luns_disks', N'cv_storage_luns_disks', 'OBJECT' 
GO
ALTER TABLE dbo.cv_storage_luns_disks ADD CONSTRAINT
	PK_cv_storage_luns_disks PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

CREATE TABLE [dbo].[cv_clustering](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[answerid] [int] NULL,
	[created] [datetime] NULL,
	[started] [datetime] NULL,
	[completed] [datetime] NULL,
	[output] [varchar](max) NULL,
	[latest] [int] NULL,
	[error] [int] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_clustering] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE dbo.cv_servers
	DROP CONSTRAINT DF_cv_servers_paused
GO
CREATE TABLE dbo.Tmp_cv_servers
	(
	id int NOT NULL IDENTITY (1, 1),
	requestid int NULL,
	answerid int NULL,
	modelid int NULL,
	csmconfigid int NULL,
	clusterid int NULL,
	number int NULL,
	osid int NULL,
	spid int NULL,
	templateid int NULL,
	domainid int NULL,
	test_domainid int NULL,
	infrastructure int NULL,
	ha int NULL,
	dr int NULL,
	dr_exist int NULL,
	dr_name varchar(30) NULL,
	dr_consistency int NULL,
	dr_consistencyid int NULL,
	configured int NULL,
	local_storage int NULL,
	accounts int NULL,
	fdrive int NULL,
	dba int NULL,
	pnc int NULL,
	vmware_clusterid int NULL,
	nameid int NULL,
	dhcp varchar(15) NULL,
	mhs int NULL,
	zeus_error int NULL,
	step int NULL,
	step_skip_start int NULL,
	step_skip_goto int NULL,
	tsm_schedule int NULL,
	tsm_cloptset int NULL,
	tsm_register varchar(300) NULL,
	tsm_define varchar(300) NULL,
	tsm_output varchar(MAX) NULL,
	tsm_bypass int NULL,
	tsm_registered datetime NULL,
	dns_auto int NULL,
	dns_output varchar(MAX) NULL,
	build_started datetime NULL,
	build_completed datetime NULL,
	build_ready datetime NULL,
	mis_audits datetime NULL,
	rebuild datetime NULL,
	rebuilding int NULL,
	decommissioned datetime NULL,
	reclaimed_storage float(53) NULL,
	reclaimed_backup float(53) NULL,
	reclaimed_amt float(53) NULL,
	reclaimed_tier int NULL,
	reclaimed_environment varchar(50) NULL,
	reclaimed_storage_precooldown datetime NULL,
	reclaimed_storage_cooldown datetime NULL,
	reclaimed_storage_cr2 varchar(50) NULL,
	reclaimed_storage_classification varchar(10) NULL,
	reclaimed_storage_vendor varchar(10) NULL,
	reclaimed_storage_location int NULL,
	reclaimed_storage_array varchar(10) NULL,
	reclaimed_storage_notes varchar(MAX) NULL,
	paused int NULL,
	storage_configured datetime NULL,
	created datetime NULL,
	modified datetime NULL,
	deleted int NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_cv_servers SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_cv_servers ADD CONSTRAINT
	DF_cv_servers_paused DEFAULT ((0)) FOR paused
GO
SET IDENTITY_INSERT dbo.Tmp_cv_servers ON
GO
IF EXISTS(SELECT * FROM dbo.cv_servers)
	 EXEC('INSERT INTO dbo.Tmp_cv_servers (id, requestid, answerid, modelid, csmconfigid, clusterid, number, osid, spid, templateid, domainid, test_domainid, infrastructure, ha, dr, dr_exist, dr_name, dr_consistency, dr_consistencyid, configured, local_storage, accounts, fdrive, dba, pnc, vmware_clusterid, nameid, dhcp, mhs, zeus_error, step, step_skip_start, step_skip_goto, tsm_schedule, tsm_cloptset, tsm_register, tsm_define, tsm_output, tsm_bypass, tsm_registered, dns_auto, dns_output, build_started, build_completed, build_ready, mis_audits, rebuild, rebuilding, decommissioned, reclaimed_storage, reclaimed_backup, reclaimed_amt, reclaimed_tier, reclaimed_environment, reclaimed_storage_precooldown, reclaimed_storage_cooldown, reclaimed_storage_cr2, reclaimed_storage_classification, reclaimed_storage_vendor, reclaimed_storage_location, reclaimed_storage_array, reclaimed_storage_notes, paused, created, modified, deleted)
		SELECT id, requestid, answerid, modelid, csmconfigid, clusterid, number, osid, spid, templateid, domainid, test_domainid, infrastructure, ha, dr, dr_exist, dr_name, dr_consistency, dr_consistencyid, configured, local_storage, accounts, fdrive, dba, pnc, vmware_clusterid, nameid, dhcp, mhs, zeus_error, step, step_skip_start, step_skip_goto, tsm_schedule, tsm_cloptset, tsm_register, tsm_define, tsm_output, tsm_bypass, tsm_registered, dns_auto, dns_output, build_started, build_completed, build_ready, mis_audits, rebuild, rebuilding, decommissioned, reclaimed_storage, reclaimed_backup, reclaimed_amt, reclaimed_tier, reclaimed_environment, reclaimed_storage_precooldown, reclaimed_storage_cooldown, reclaimed_storage_cr2, reclaimed_storage_classification, reclaimed_storage_vendor, reclaimed_storage_location, reclaimed_storage_array, reclaimed_storage_notes, paused, created, modified, deleted FROM dbo.cv_servers WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_cv_servers OFF
GO
DROP TABLE dbo.cv_servers
GO
EXECUTE sp_rename N'dbo.Tmp_cv_servers', N'cv_servers', 'OBJECT' 
GO
ALTER TABLE dbo.cv_servers ADD CONSTRAINT
	PK_cv_servers PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

CREATE TABLE [dbo].[cv_forecast_answer_errors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[itemid] [int] NULL,
	[number] [int] NULL,
	[answerid] [int] NULL,
	[step] [int] NULL,
	[reason] [varchar](max) NULL,
	[incident] [varchar](20) NULL,
	[assigned] [int] NULL,
	[errorid] [int] NULL,
	[userid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[fixed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_forecast_answer_errors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[cv_server_rebuilds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[requestid] [int] NULL,
	[serviceid] [int] NULL,
	[number] [int] NULL,
	[serverid] [int] NULL,
	[change] [varchar](20) NULL,
	[submitted] [datetime] NULL,
	[completed] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_server_rebuilds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE dbo.Tmp_cv_storage_drive_letters
	(
	id int NOT NULL IDENTITY (1, 1),
	letter varchar(1) NULL,
	volume varchar(25) NULL,
	enabled int NULL,
	created datetime NULL,
	modified datetime NULL,
	deleted int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_cv_storage_drive_letters SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_cv_storage_drive_letters ON
GO
IF EXISTS(SELECT * FROM dbo.cv_storage_drive_letters)
	 EXEC('INSERT INTO dbo.Tmp_cv_storage_drive_letters (id, letter, enabled, created, modified, deleted)
		SELECT id, letter, enabled, created, modified, deleted FROM dbo.cv_storage_drive_letters WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_cv_storage_drive_letters OFF
GO
DROP TABLE dbo.cv_storage_drive_letters
GO
EXECUTE sp_rename N'dbo.Tmp_cv_storage_drive_letters', N'cv_storage_drive_letters', 'OBJECT' 
GO
ALTER TABLE dbo.cv_storage_drive_letters ADD CONSTRAINT
	PK_cv_storage_drive_letters PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

CREATE TABLE dbo.Tmp_cv_location_address
	(
	id int NOT NULL IDENTITY (1, 1),
	cityid int NULL,
	name varchar(100) NULL,
	factory_code varchar(5) NULL,
	common int NULL,
	commonname varchar(50) NULL,
	storage int NULL,
	tsm int NULL,
	dr int NULL,
	offsite_build int NULL,
	manual_build int NULL,
	building_code varchar(20) NULL,
	service_now varchar(20) NULL,
	recovery int NULL,
	vmware_ipaddress int NULL,
	prod int NULL,
	qa int NULL,
	test int NULL,
	display int NULL,
	enabled int NULL,
	created datetime NULL,
	modified datetime NULL,
	deleted int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_cv_location_address SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_cv_location_address ON
GO
IF EXISTS(SELECT * FROM dbo.cv_location_address)
	 EXEC('INSERT INTO dbo.Tmp_cv_location_address (id, cityid, name, factory_code, common, commonname, storage, tsm, dr, offsite_build, manual_build, building_code, recovery, vmware_ipaddress, prod, qa, test, display, enabled, created, modified, deleted)
		SELECT id, cityid, name, factory_code, common, commonname, storage, tsm, dr, offsite_build, manual_build, building_code, recovery, vmware_ipaddress, prod, qa, test, display, enabled, created, modified, deleted FROM dbo.cv_location_address WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_cv_location_address OFF
GO
DROP TABLE dbo.cv_location_address
GO
EXECUTE sp_rename N'dbo.Tmp_cv_location_address', N'cv_location_address', 'OBJECT' 
GO
ALTER TABLE dbo.cv_location_address ADD CONSTRAINT
	PK_cv_location_address PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_cv_location_address_id ON dbo.cv_location_address
	(
	id
	) INCLUDE (deleted) 
 WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_cv_location_address_cityid ON dbo.cv_location_address
	(
	cityid
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

