/*==============================================================
The purpose of this script is to create procedures, functions and synonyms
==============================================================*/

CREATE procedure [dbo].[pr_updateDecommissionServiceNowOperationalStatus]
	@name varchar(50),
	@service_now_operational_status int
AS
UPDATE
	cva_decommissions
SET
	service_now_operational_status = @service_now_operational_status,
	modified = getdate()
WHERE
	name = @name
	AND deleted = 0
	AND recommissioned IS NULL
GO

ALTER procedure [dbo].[pr_addDecommission]
	@requestid int,
	@itemid int,
	@number int,
	@assetid int,
	@userid int,
	@reason varchar(max),
	@decom datetime,
	@name varchar(50),
	@dr int
AS
UPDATE
	cva_decommissions
SET
	deleted = 1
WHERE
	assetid = @assetid
	AND recommissioned IS NOT NULL
	AND deleted = 0

INSERT INTO
	cva_decommissions
VALUES
(
	@requestid,
	@itemid,
	@number,
	@assetid,
	@userid,
	@reason,
	@decom,
	null,	-- turnedoff
	null,	-- destroy
	null,	-- destroyed
	null,	-- vmware
	@name,
	0,		-- active
	0,		-- running
	@dr,
	--null,		-- confirmed
	--0,		-- confirmed_by
	getdate(),	-- confirmed
	-999,		-- confirmed_by
	null,	-- recommissioned
	0,		-- recommissioned_by
	null,	-- recommissioned_reason
	null,	-- bypassed
	0,		-- bypassed_by
	null,	-- bypassed_reason
	null,	-- bypassed_ptm
	0,		-- service_now_operational_status
	getdate(),
	null,
	0
)
GO

CREATE procedure [dbo].[pr_getDecommissionName]
	@name varchar(50)
AS
-- EXEC pr_getDecommissionName 'OHCLEIIS402Q'
SELECT
	*
FROM
	cva_decommissions
WHERE
	name = @name
	AND active = 1
	AND recommissioned IS NULL
	AND deleted = 0
GO

