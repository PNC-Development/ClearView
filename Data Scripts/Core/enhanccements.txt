-- ***********************
-- ENHANCEMENTS
-- ***********************

CREATE TABLE [dbo].[cv_enhancements_approvals_groups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[enabled] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_enhancements_approvals_groups] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[cv_enhancements_approvals_groups_users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[groupid] [int] NULL,
	[userid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_enhancements_approvals_groups_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[cv_enhancements_approvals](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[enhancementid] [int] NULL,
	[step] [int] NULL,
	[groupid] [int] NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[deleted] [int] NULL,
 CONSTRAINT [PK_cv_enhancements_approvals] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_cv_enhancements_steps
	(
	id int NOT NULL IDENTITY (1, 1),
	enhancementid int NULL,
	step int NULL,
	created datetime NULL,
	completed datetime NULL,
	approved datetime NULL,
	rejected datetime NULL,
	reopened datetime NULL,
	deleted int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_cv_enhancements_steps SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_cv_enhancements_steps ON
GO
IF EXISTS(SELECT * FROM dbo.cv_enhancements_steps)
	 EXEC('INSERT INTO dbo.Tmp_cv_enhancements_steps (id, enhancementid, step, completed, approved, rejected, reopened, deleted)
		SELECT id, enhancementid, step, completed, approved, rejected, reopened, deleted FROM dbo.cv_enhancements_steps WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_cv_enhancements_steps OFF
GO
DROP TABLE dbo.cv_enhancements_steps
GO
EXECUTE sp_rename N'dbo.Tmp_cv_enhancements_steps', N'cv_enhancements_steps', 'OBJECT' 
GO
ALTER TABLE dbo.cv_enhancements_steps ADD CONSTRAINT
	PK_cv_enhancements_steps PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
GO















-- ***********************
-- ENHANCEMENTS
-- ***********************

CREATE PROCEDURE pr_addEnhancementApprovalGroup
	@name varchar(100),
	@any int,
	@enabled int
AS
INSERT INTO
	cv_enhancements_approvals_groups
VALUES
(
	@name,
	@any,
	@enabled,
	getdate(),
	getdate(),
	0
)
GO

CREATE PROCEDURE pr_updateEnhancementApprovalGroup
	@id int,
	@name varchar(100),
	@any int,
	@enabled int
AS
UPDATE
	cv_enhancements_approvals_groups
SET
	[name] = @name,
	[any] = @any,
	[enabled] = @enabled,
	[modified] = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE pr_updateEnhancementApprovalGroupEnabled
	@id int,
	@enabled int
AS
UPDATE
	cv_enhancements_approvals_groups
SET
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE pr_deleteEnhancementApprovalGroup
	@id int
AS
UPDATE
	cv_enhancements_approvals_groups
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE pr_getEnhancementApprovalGroup
	@id int
AS
SELECT
	*
FROM
	cv_enhancements_approvals_groups
WHERE
	id = @id
GO

CREATE PROCEDURE pr_getEnhancementApprovalGroups
	@enabled int
AS
SELECT
	*
FROM
	cv_enhancements_approvals_groups
WHERE
	deleted = 0
	AND enabled >= @enabled
ORDER BY
	name
GO

create PROCEDURE [dbo].[pr_deleteEnhancementApprovalGroupUser]
	@id int
AS
UPDATE
	cv_enhancements_approvals_groups_users
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id
GO

create PROCEDURE [dbo].[pr_addEnhancementApprovalGroupUser]
	@groupid int,
	@userid int
AS
INSERT INTO
	cv_enhancements_approvals_groups_users
VALUES
(
	@groupid,
	@userid,
	getdate(),
	getdate(),
	0
)
GO

create PROCEDURE [dbo].[pr_getEnhancementApprovalGroupUsers]
	@groupid int
AS
SELECT
	cv_enhancements_approvals_groups_users.*,
	cv_users.fname + ' ' + cv_users.lname AS username
FROM
	cv_enhancements_approvals_groups_users
		INNER JOIN
			cv_users
		ON
			cv_enhancements_approvals_groups_users.userid = cv_users.userid
			AND cv_users.enabled = 1
			AND cv_users.deleted = 0
WHERE
	cv_enhancements_approvals_groups_users.deleted = 0
	AND cv_enhancements_approvals_groups_users.groupid = @groupid
GO

ALTER procedure [dbo].[pr_addEnhancementStep]
	@enhancementid int,
	@step int,
	@completed datetime,
	@approved datetime
AS
INSERT INTO
	cv_enhancements_steps
(
	enhancementid,
	step,
	created,
	completed,
	approved,
	deleted
)
VALUES
(
	@enhancementid,
	@step,
	GETDATE(),
	@completed,
	@approved,
	0
)
GO


 