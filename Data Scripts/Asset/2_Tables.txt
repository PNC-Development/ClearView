/*==============================================================
The purpose of this script is to create /alter tables, indexes, and constraints
==============================================================*/

CREATE TABLE dbo.Tmp_cva_decommissions
	(
	id int NOT NULL IDENTITY (1, 1),
	requestid int NULL,
	itemid int NULL,
	number int NULL,
	assetid int NULL,
	userid int NULL,
	reason varchar(MAX) NULL,
	decom datetime NULL,
	turnedoff datetime NULL,
	destroy datetime NULL,
	destroyed datetime NULL,
	vmware int NULL,
	name varchar(50) NULL,
	active int NULL,
	running int NULL,
	dr int NULL,
	confirmed datetime NULL,
	confirmed_by int NULL,
	recommissioned datetime NULL,
	recommissioned_by int NULL,
	recommissioned_reason varchar(MAX) NULL,
	bypassed datetime NULL,
	bypassed_by int NULL,
	bypassed_reason varchar(MAX) NULL,
	bypassed_ptm varchar(20) NULL,
	service_now_operational_status int NULL,
	created datetime NULL,
	modified datetime NULL,
	deleted int NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_cva_decommissions SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_cva_decommissions ON
GO
IF EXISTS(SELECT * FROM dbo.cva_decommissions)
	 EXEC('INSERT INTO dbo.Tmp_cva_decommissions (id, requestid, itemid, number, assetid, userid, reason, decom, turnedoff, destroy, destroyed, vmware, name, active, running, dr, confirmed, confirmed_by, recommissioned, recommissioned_by, recommissioned_reason, bypassed, bypassed_by, bypassed_reason, bypassed_ptm, created, modified, deleted)
		SELECT id, requestid, itemid, number, assetid, userid, reason, decom, turnedoff, destroy, destroyed, vmware, name, active, running, dr, confirmed, confirmed_by, recommissioned, recommissioned_by, recommissioned_reason, bypassed, bypassed_by, bypassed_reason, bypassed_ptm, created, modified, deleted FROM dbo.cva_decommissions WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_cva_decommissions OFF
GO
DROP TABLE dbo.cva_decommissions
GO
EXECUTE sp_rename N'dbo.Tmp_cva_decommissions', N'cva_decommissions', 'OBJECT' 
GO
ALTER TABLE dbo.cva_decommissions ADD CONSTRAINT
	PK_cva_decoms PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

