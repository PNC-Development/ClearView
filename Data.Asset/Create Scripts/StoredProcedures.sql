USE [ClearViewAsset]
GO
/****** Object:  StoredProcedure [dbo].[pr_addAsset]    Script Date: 07/31/2009 13:23:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[pr_addAsset]
	@orderid int,
	@modelid int,
	@serial varchar(50),
	@asset varchar(20),
	@bad int,
	@validated int,
	@id int output
AS
INSERT INTO
	cva_assets
VALUES
(
	@orderid,
	@modelid,
	@serial,
	@asset,
	@bad,
	@validated,
	getdate(),
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()












GO
/****** Object:  StoredProcedure [dbo].[pr_addBlade]    Script Date: 07/31/2009 13:23:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















CREATE PROCEDURE [dbo].[pr_addBlade]
	@assetid int,
	@enclosureid int,
	@ilo varchar(15),
	@dummy_name varchar(50),
	@macaddress varchar(50),
	@vlan int,
	@slot int,
	@spare int
AS
INSERT INTO
	cva_blades
VALUES
(
	@assetid,
	@enclosureid,
	@ilo,
	@dummy_name,
	@macaddress,
	@vlan,
	@slot,
	@spare,
	getdate(),
	getdate(),
	0
)


















GO
/****** Object:  StoredProcedure [dbo].[pr_addDecommission]    Script Date: 07/31/2009 13:23:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[pr_addDecommission]
	@requestid int,
	@itemid int,
	@number int,
	@assetid int,
	@userid int,
	@reason text,
	@decom datetime,
	@name varchar(50)
AS
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
	null,
	null,
	null,
	null,
	@name,
	0,
	getdate(),
	null,
	0
)


GO
/****** Object:  StoredProcedure [dbo].[pr_addEnclosure]    Script Date: 07/31/2009 13:23:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















CREATE PROCEDURE [dbo].[pr_addEnclosure]
	@assetid int,
	@classid int,
	@environmentid int,
	@addressid int,
	@roomid int,
	@rackid int,
	@rackposition varchar(10),
	@vlan int,
	@oa_ip varchar(15)
AS
INSERT INTO
	cva_enclosures
VALUES
(
	@assetid,
	@classid,
	@environmentid,
	@addressid,
	@roomid,
	@rackid,
	@rackposition,
	@vlan,
	@oa_ip,
	getdate(),
	getdate(),
	0
)


















GO
/****** Object:  StoredProcedure [dbo].[pr_addEnclosureDR]    Script Date: 07/31/2009 13:23:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[pr_addEnclosureDR]
	@enclosureid int,
	@drid int
AS
UPDATE
	cva_enclosures_dr
SET
	deleted = 1
WHERE
	enclosureid = @enclosureid

INSERT INTO
	cva_enclosures_dr
VALUES
(
	@enclosureid,
	@drid,
	getdate(),
	0
)

GO
/****** Object:  StoredProcedure [dbo].[pr_addEnclosureVC]    Script Date: 07/31/2009 13:23:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pr_addEnclosureVC]
	@enclosureid int,
	@virtual_connect varchar(15),
	@display int,
	@enabled int
AS
INSERT INTO
	cva_enclosures_vc
VALUES
(
	@enclosureid,
	@virtual_connect,
	@display,
	@enabled,
	getdate(),
	getdate(),
	0
)


GO
/****** Object:  StoredProcedure [dbo].[pr_addGuest]    Script Date: 07/31/2009 13:23:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












create PROCEDURE [dbo].[pr_addGuest]
	@assetid int,
	@hostid int,
	@ram float,
	@processors float,
	@storage float,
	@classid int,
	@environmentid int,
	@addressid int,
	@classid_move int,
	@environmentid_move int
AS
INSERT INTO
	cva_guests
VALUES
(
	@assetid,
	@hostid,
	@ram,
	@processors,
	@storage,
	@classid,
	@environmentid,
	@addressid,
	@classid_move,
	@environmentid_move,
	getdate(),
	getdate(),
	0
)















GO
/****** Object:  StoredProcedure [dbo].[pr_addHBA]    Script Date: 07/31/2009 13:23:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[pr_addHBA]
	@assetid int,
	@name varchar(100)
AS
INSERT INTO
	cva_hba
VALUES
(
	@assetid,
	@name,
	getdate(),
	getdate(),
	0
)












GO
/****** Object:  StoredProcedure [dbo].[pr_addHostLocation]    Script Date: 07/31/2009 13:23:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[pr_addHostLocation]
	@assetid int,
	@classid int,
	@environmentid int,
	@addressid int
AS
INSERT INTO
	cva_hosts_locations
VALUES
(
	@assetid,
	@classid,
	@environmentid,
	@addressid,
	getdate(),
	0
)













GO
/****** Object:  StoredProcedure [dbo].[pr_addHostVirtual]    Script Date: 07/31/2009 13:23:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE PROCEDURE [dbo].[pr_addHostVirtual]
	@assetid int,
	@hostid int,
	@guests int,
	@processors float
AS
UPDATE
	cva_hosts_virtual
SET
	deleted = 1
WHERE
	assetid = @assetid
INSERT INTO
	cva_hosts_virtual
VALUES
(
	@assetid,
	@hostid,
	@guests,
	@processors,
	getdate(),
	getdate(),
	0
)




















GO
/****** Object:  StoredProcedure [dbo].[pr_addHostVirtualEnvironment]    Script Date: 07/31/2009 13:23:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














create PROCEDURE [dbo].[pr_addHostVirtualEnvironment]
	@assetid int,
	@environment int
AS
INSERT INTO
	cva_hosts_virtual_environment
VALUES
(
	@assetid,
	@environment,
	getdate(),
	0
)




















GO
/****** Object:  StoredProcedure [dbo].[pr_addHostVirtualOs]    Script Date: 07/31/2009 13:23:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















CREATE PROCEDURE [dbo].[pr_addHostVirtualOs]
	@assetid int,
	@osid int,
	@virtualdir varchar(100),
	@gzippath varchar(100),
	@image varchar(100)
AS
INSERT INTO
	cva_hosts_virtual_os
VALUES
(
	@assetid,
	@osid,
	@virtualdir,
	@gzippath,
	@image,
	getdate(),
	0
)





















GO
/****** Object:  StoredProcedure [dbo].[pr_addHostVMWare]    Script Date: 07/31/2009 13:23:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[pr_addHostVMWare]
	@assetid int,
	@hostid int,
	@guests int,
	@processors float
AS
INSERT INTO
	cva_hosts_vmware
VALUES
(
	@assetid,
	@hostid,
	@guests,
	@processors,
	getdate(),
	getdate(),
	0
)















GO
/****** Object:  StoredProcedure [dbo].[pr_addIP]    Script Date: 07/31/2009 13:23:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[pr_addIP]
	@assetid int,
	@ipaddressid int
AS
INSERT INTO
	cva_ips
VALUES
(
	@assetid,
	@ipaddressid,
	getdate(),
	getdate(),
	0
)













GO
/****** Object:  StoredProcedure [dbo].[pr_addNetwork]    Script Date: 07/31/2009 13:23:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO













CREATE PROCEDURE [dbo].[pr_addNetwork]
	@assetid int,
	@depotid int,
	@depotroomid int,
	@shelfid int,
	@available_ports int,
	@classid int,
	@environmentid int,
	@addressid int,
	@roomid int,
	@rackid int,
	@rackposition varchar(10)
AS
INSERT INTO
	cva_network
VALUES
(
	@assetid,
	@depotid,
	@depotroomid,
	@shelfid,
	@available_ports,
	@classid,
	@environmentid,
	@addressid,
	@roomid,
	@rackid,
	@rackposition,
	getdate(),
	getdate(),
	0
)
















GO
/****** Object:  StoredProcedure [dbo].[pr_addOrder]    Script Date: 07/31/2009 13:23:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [dbo].[pr_addOrder]
	@tracking varchar(50),
	@name varchar(50),
	@quantity int,
	@modelid int,
	@classid int,
	@environmentid int,
	@addressid int,
	@confidenceid int,
	@ordered datetime
AS
INSERT INTO
	cva_orders
VALUES
(
	@tracking,
	@name,
	@quantity,
	@modelid,
	@classid,
	@environmentid,
	@addressid,
	@confidenceid,
	@ordered,
	0,
	'',
	0,
	1,
	getdate(),
	getdate(),
	null,
	0
)









GO
/****** Object:  StoredProcedure [dbo].[pr_addReservation]    Script Date: 07/31/2009 13:23:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







create PROCEDURE [dbo].[pr_addReservation]
	@buildid int,
	@reserveid int,
	@removable int
AS
INSERT INTO
	cva_reservations
VALUES
(
	@buildid,
	@reserveid,
	@removable,
	getdate(),
	getdate(),
	0
)













GO
/****** Object:  StoredProcedure [dbo].[pr_addSearchClass]    Script Date: 07/31/2009 13:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[pr_addSearchClass]
	@userid int,
	@type varchar(1),
	@platformid int,
	@classid int,
	@environmentid int,
	@id int output
AS
INSERT INTO
	cva_search
(
	userid,
	type,
	platformid,
	classid,
	environmentid,
	modified,
	deleted
)
VALUES
(
	@userid,
	@type,
	@platformid,
	@classid,
	@environmentid,
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()










GO
/****** Object:  StoredProcedure [dbo].[pr_addSearchDepot]    Script Date: 07/31/2009 13:23:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_addSearchDepot]
	@userid int,
	@type varchar(1),
	@platformid int,
	@depotid int,
	@id int output
AS
INSERT INTO
	cva_search
(
	userid,
	type,
	platformid,
	depotid,
	modified,
	deleted
)
VALUES
(
	@userid,
	@type,
	@platformid,
	@depotid,
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()











GO
/****** Object:  StoredProcedure [dbo].[pr_addSearchName]    Script Date: 07/31/2009 13:23:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_addSearchName]
	@userid int,
	@type varchar(1),
	@name varchar(100),
	@serial varchar(50),
	@asset varchar(20),
	@id int output
AS
INSERT INTO
	cva_search
(
	userid,
	type,
	name,
	serial,
	asset,
	modified,
	deleted
)
VALUES
(
	@userid,
	@type,
	@name,
	@serial,
	@asset,
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()











GO
/****** Object:  StoredProcedure [dbo].[pr_addSearchPlatform]    Script Date: 07/31/2009 13:23:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[pr_addSearchPlatform]
	@userid int,
	@type varchar(1),
	@platformid int,
	@typeid int,
	@modelid int,
	@id int output
AS
INSERT INTO
	cva_search
(
	userid,
	type,
	platformid,
	typeid,
	modelid,
	modified,
	deleted
)
VALUES
(
	@userid,
	@type,
	@platformid,
	@typeid,
	@modelid,
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()









GO
/****** Object:  StoredProcedure [dbo].[pr_addServer]    Script Date: 07/31/2009 13:23:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE PROCEDURE [dbo].[pr_addServer]
	@assetid int,
	@classid int,
	@environmentid int,
	@addressid int,
	@roomid int,
	@rackid int,
	@rackposition varchar(10),
	@ilo varchar(15),
	@dummy_name varchar(50),
	@macaddress varchar(50),
	@vlan int
AS
INSERT INTO
	cva_server
VALUES
(
	@assetid,
	@classid,
	@environmentid,
	@addressid,
	@roomid,
	@rackid,
	@rackposition,
	@ilo,
	@dummy_name,
	@macaddress,
	@vlan,
	getdate(),
	getdate(),
	0
)

















GO
/****** Object:  StoredProcedure [dbo].[pr_addStatus]    Script Date: 07/31/2009 13:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[pr_addStatus]
	@assetid int,
	@name varchar(100),
	@status int,
	@userid int,
	@datestamp datetime
AS
UPDATE
	cva_status
SET
	deleted = 1
WHERE
	assetid = @assetid
INSERT INTO
	cva_status
VALUES
(
	@assetid,
	@name,
	@status,
	@userid,
	@datestamp,
	0
)














GO
/****** Object:  StoredProcedure [dbo].[pr_addVSG]    Script Date: 07/31/2009 13:24:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[pr_addVSG]
	@name varchar(50),
	@type varchar(20)
AS
INSERT INTO
	cva_vsg
VALUES
(
	@name,
	@type,
	getdate(),
	null,
	null
)



GO
/****** Object:  StoredProcedure [dbo].[pr_deleteAsset]    Script Date: 07/31/2009 13:24:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[pr_deleteAsset]
	@assetid int
AS
UPDATE 
	cva_assets 
SET 
	deleted = 1 
WHERE 
	id = @assetid






GO
/****** Object:  StoredProcedure [dbo].[pr_deleteBlade]    Script Date: 07/31/2009 13:24:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




create procedure [dbo].[pr_deleteBlade]
	@assetid int
AS
UPDATE 
	cva_blades 
SET 
	deleted = 1 
WHERE 
	assetid = @assetid







GO
/****** Object:  StoredProcedure [dbo].[pr_deleteDecommission]    Script Date: 07/31/2009 13:24:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[pr_deleteDecommission]
	@assetid int
AS
UPDATE
	cva_decommissions
SET
	deleted = 0,
	modified = getdate()
WHERE
	assetid = @assetid
	AND deleted = 0

GO
/****** Object:  StoredProcedure [dbo].[pr_deleteEnclosure]    Script Date: 07/31/2009 13:24:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




create procedure [dbo].[pr_deleteEnclosure]
	@assetid int
AS
UPDATE 
	cva_enclosures 
SET 
	deleted = 1 
WHERE 
	assetid = @assetid







GO
/****** Object:  StoredProcedure [dbo].[pr_deleteEnclosureVC]    Script Date: 07/31/2009 13:24:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_deleteEnclosureVC]
	@id int
AS
UPDATE
	cva_enclosures_vc
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[pr_deleteGuest]    Script Date: 07/31/2009 13:24:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [dbo].[pr_deleteGuest]
	@assetid int
AS
UPDATE 
	cva_guests 
SET 
	deleted = 1 
WHERE 
	assetid = @assetid








GO
/****** Object:  StoredProcedure [dbo].[pr_deleteHBA]    Script Date: 07/31/2009 13:24:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







create PROCEDURE [dbo].[pr_deleteHBA]
	@id int
AS
UPDATE
	cva_hba
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id












GO
/****** Object:  StoredProcedure [dbo].[pr_deleteHostLocations]    Script Date: 07/31/2009 13:24:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






create PROCEDURE [dbo].[pr_deleteHostLocations]
	@assetid int
AS
UPDATE
	cva_hosts_locations
SET
	deleted = 1
WHERE
	assetid = @assetid











GO
/****** Object:  StoredProcedure [dbo].[pr_deleteHostVirtualEnvironment]    Script Date: 07/31/2009 13:24:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[pr_deleteHostVirtualEnvironment]
	@assetid int
AS
UPDATE
	cva_hosts_virtual_environment
SET
	deleted = 1,
	modified = getdate()
WHERE
	assetid = @assetid




GO
/****** Object:  StoredProcedure [dbo].[pr_deleteHostVirtualOs]    Script Date: 07/31/2009 13:24:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[pr_deleteHostVirtualOs]
	@id int
AS
UPDATE
	cva_hosts_virtual_os
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id




GO
/****** Object:  StoredProcedure [dbo].[pr_deleteIPs]    Script Date: 07/31/2009 13:24:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








create PROCEDURE [dbo].[pr_deleteIPs]
	@assetid int
AS
UPDATE
	cva_ips
SET
	deleted = 1
WHERE
	assetid = @assetid













GO
/****** Object:  StoredProcedure [dbo].[pr_deleteOrder]    Script Date: 07/31/2009 13:24:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[pr_deleteOrder]
	@id int
AS
UPDATE
	cva_orders
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id









GO
/****** Object:  StoredProcedure [dbo].[pr_deleteReservation]    Script Date: 07/31/2009 13:24:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







create PROCEDURE [dbo].[pr_deleteReservation]
	@buildid int,
	@reserveid int
AS
UPDATE
	cva_reservations
SET
	deleted = 1,
	modified = getdate()
WHERE
	buildid = @buildid
	AND reserveid = @reserveid













GO
/****** Object:  StoredProcedure [dbo].[pr_deleteServer]    Script Date: 07/31/2009 13:24:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




create procedure [dbo].[pr_deleteServer]
	@assetid int
AS
UPDATE 
	cva_server 
SET 
	deleted = 1 
WHERE 
	assetid = @assetid







GO
/****** Object:  StoredProcedure [dbo].[pr_getAll]    Script Date: 07/31/2009 13:24:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pr_getAll]
	@typeid int,
	@status int
AS
SELECT
	cva_assets.id,
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.status,
	cva_status.datestamp,
	cv_models.make,
	cv_models_property.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cv_users.fname + ' ' + cv_users.lname AS username
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
							AND cv_types.id = @typeid
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_status
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
					AND cv_users.enabled = 1
					AND cv_users.deleted = 0
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.status = @status
			AND cva_status.deleted = 0
WHERE
	cva_assets.bad = 0
	AND cva_assets.deleted = 0


GO
/****** Object:  StoredProcedure [dbo].[pr_getAsset]    Script Date: 07/31/2009 13:24:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[pr_getAsset]
	@assetid int
AS
SELECT
	cva_orders.tracking,
	cva_assets.*,
	cv_models.make,
	cv_models_property.name AS modelname, 
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
		LEFT OUTER JOIN
			cva_orders
		ON
			cva_assets.orderid = cva_orders.id
			AND cva_orders.deleted = 0
WHERE
	cva_assets.id = @assetid
	AND cva_assets.deleted = 0














GO
/****** Object:  StoredProcedure [dbo].[pr_getAssetCount]    Script Date: 07/31/2009 13:24:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE PROCEDURE [dbo].[pr_getAssetCount]
	@modelid int,
	@status int
AS
SELECT
	cva_assets.*
FROM
	cva_assets
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = @status
WHERE
	cva_assets.modelid = @modelid
	AND cva_assets.deleted = 0











GO
/****** Object:  StoredProcedure [dbo].[pr_getAssetDuplicate]    Script Date: 07/31/2009 13:24:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















create PROCEDURE [dbo].[pr_getAssetDuplicate]
	@serial varchar(50),
	@modelid int
AS
SELECT
	cva_assets.*,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	UPPER(RTRIM(cva_assets.serial)) = @serial
	AND cva_assets.modelid = @modelid
	AND cva_assets.deleted = 0












GO
/****** Object:  StoredProcedure [dbo].[pr_getAssets]    Script Date: 07/31/2009 13:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





















CREATE PROCEDURE [dbo].[pr_getAssets]
	@platformid int,
	@status int
AS
SELECT
	cva_orders.tracking,
	cva_assets.*,
	cv_models.make,
	cv_models_property.name AS modelname, 
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
									AND cv_platforms.platformid = @platformid
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = @status
		LEFT OUTER JOIN
			cva_orders
		ON
			cva_assets.orderid = cva_orders.id
			AND cva_orders.deleted = 0
WHERE
	cva_assets.deleted = 0
















GO
/****** Object:  StoredProcedure [dbo].[pr_getAssetSerial]    Script Date: 07/31/2009 13:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















create PROCEDURE [dbo].[pr_getAssetSerial]
	@serial varchar(50)
AS
SELECT
	cva_assets.*,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	UPPER(RTRIM(cva_assets.serial)) = @serial
	AND cva_assets.deleted = 0











GO
/****** Object:  StoredProcedure [dbo].[pr_getDecommissionDestroys]    Script Date: 07/31/2009 13:24:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[pr_getDecommissionDestroys]
	@typeid int,
	@destroy datetime
AS
SELECT
	cva_decommissions.*
FROM
	cva_decommissions
		INNER JOIN
			cva_assets
				INNER JOIN
					clearview.dbo.cv_models_property
						INNER JOIN
							clearview.dbo.cv_models
						ON
							cv_models_property.modelid = cv_models.id
							AND cv_models.deleted = 0
							AND cv_models.typeid = @typeid
				ON
					cva_assets.modelid = cv_models_property.id
					AND cv_models_property.deleted = 0
		ON
			cva_assets.id = cva_decommissions.assetid
			AND cva_assets.deleted = 0
		INNER JOIN
			cva_status
		ON
			cva_status.assetid = cva_decommissions.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = -1
WHERE
	cva_decommissions.active = 1
	AND cva_decommissions.deleted = 0
	AND cva_decommissions.turnedoff IS NOT NULL
	AND cva_decommissions.destroyed IS NULL
	AND DATEPART(yyyy, cva_decommissions.destroy) = DATEPART(yyyy, @destroy)
	AND DATEPART(mm, cva_decommissions.destroy) = DATEPART(mm, @destroy)
	AND DATEPART(dd, cva_decommissions.destroy) = DATEPART(dd, @destroy)




GO
/****** Object:  StoredProcedure [dbo].[pr_getDecommissions]    Script Date: 07/31/2009 13:24:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[pr_getDecommissions]
	@typeid int,
	@decom datetime
AS
SELECT
	cva_decommissions.*
FROM
	cva_decommissions
		INNER JOIN
			cva_assets
				INNER JOIN
					clearview.dbo.cv_models_property
						INNER JOIN
							clearview.dbo.cv_models
						ON
							cv_models_property.modelid = cv_models.id
							AND cv_models.deleted = 0
							AND cv_models.typeid = @typeid
				ON
					cva_assets.modelid = cv_models_property.id
					AND cv_models_property.deleted = 0
		ON
			cva_assets.id = cva_decommissions.assetid
			AND cva_assets.deleted = 0
WHERE
	cva_decommissions.active = 1
	AND cva_decommissions.deleted = 0
	AND cva_decommissions.turnedoff IS NULL
	AND DATEPART(yyyy, cva_decommissions.decom) = DATEPART(yyyy, @decom)
	AND DATEPART(mm, cva_decommissions.decom) = DATEPART(mm, @decom)
	AND DATEPART(dd, cva_decommissions.decom) = DATEPART(dd, @decom)




GO
/****** Object:  StoredProcedure [dbo].[pr_getDemandFacilities]    Script Date: 07/31/2009 13:24:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















CREATE PROCEDURE [dbo].[pr_getDemandFacilities]
AS
DECLARE @confidence_100 int
SET @confidence_100 = (SELECT confidence_100 FROM clearview.dbo.cv_setting)
-- JOIN orders
SELECT
	cva_orders.classid,
	cva_orders.environmentid,
	cva_orders.addressid,
	@confidence_100 AS confidenceid,
	cv_platforms.name AS platform,
	cv_models.name AS model,
	cv_models_property.amp,
	cva_orders.quantity,
	0 AS temptable
INTO
	#temp0
FROM
	cva_orders
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
					AND cv_models.hostid = 0 
		ON 
			cva_orders.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
WHERE
	cva_orders.deleted = 0
	and cva_orders.completed is null

-- JOIN server devices (non-hosts)
SELECT
	cva_server.classid,
	cva_server.environmentid,
	cva_server.addressid,
	@confidence_100 AS confidenceid,
	cv_platforms.name AS platform,
	cv_models.name AS model,
	cv_models_property.amp,
	1 AS quantity,
	1 AS temptable
INTO
	#temp1
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
					AND cv_models.hostid = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_server
		ON
			cva_assets.id = cva_server.assetid
			and cva_server.deleted = 0
WHERE
	cva_assets.deleted = 0

-- JOIN network devices (non-hosts)
SELECT
	cva_network.classid,
	cva_network.environmentid,
	cva_network.addressid,
	@confidence_100 AS confidenceid,
	cv_platforms.name AS platform,
	cv_models.name AS model,
	cv_models_property.amp,
	1 AS quantity,
	2 AS temptable
INTO
	#temp2
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
					AND cv_models.hostid = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_network
		ON
			cva_assets.id = cva_network.assetid
			and cva_network.deleted = 0
WHERE
	cva_assets.deleted = 0

-- JOIN forecasts that have not been completed based on override model
SELECT
	cv_forecast_answers.classid,
	cv_forecast_answers.environmentid,
	cv_forecast_answers.addressid,
	cv_forecast_answers.confidenceid,
	cv_platforms.name AS platform,
	cv_models.name AS model,
	cv_models_property.amp,
	cv_forecast_answers.quantity,
	3 AS temptable
INTO
	#temp3
FROM
	clearview.dbo.cv_forecast_answers
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
					AND cv_models.hostid = 0 
		ON 
			cv_forecast_answers.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
WHERE
	cv_forecast_answers.deleted = 0
	and cv_forecast_answers.completed is null

-- JOIN forecasts that have not been completed based on dynamic model
SELECT
	cv_forecast_answers.id,
	0 AS modelid,
	cv_forecast_answers.quantity,
	cv_forecast_answers.classid,
	cv_forecast_answers.environmentid,
	cv_forecast_answers.addressid,
	cv_forecast_answers.confidenceid
INTO
	#temp4
FROM
	clearview.dbo.cv_forecast_answers
WHERE
	cv_forecast_answers.deleted = 0
	and cv_forecast_answers.completed is null
	and cv_forecast_answers.modelid = 0

DECLARE @id int
DECLARE @modelid int
DECLARE cur CURSOR FOR SELECT id FROM #temp4
OPEN cur
FETCH NEXT FROM cur INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	CREATE TABLE #temp_cur (modelid INT)
	INSERT INTO #temp_cur EXEC clearview.dbo.pr_getForecastModel @id
	SET @modelid = (SELECT TOP 1 modelid FROM #temp_cur)
	print 'model'
	print @modelid
	DROP TABLE #temp_cur
	IF @modelid is not null
		UPDATE #temp4 SET modelid = @modelid WHERE id = @id
	FETCH NEXT FROM cur INTO @id
END
CLOSE cur
DEALLOCATE cur

SELECT * FROM #temp0
UNION ALL
SELECT * FROM #temp1
UNION ALL
SELECT * FROM #temp2
UNION ALL
SELECT 
	classid, 
	environmentid,
	addressid,
	confidenceid,
	CAST (platform AS VARCHAR) COLLATE SQL_Latin1_General_CP1_CI_AS,
	CAST (model AS VARCHAR) COLLATE SQL_Latin1_General_CP1_CI_AS,
	amp,
	quantity,
	temptable
FROM #temp3
UNION ALL
SELECT 
	classid, 
	environmentid,
	addressid,
	confidenceid,
	cv_platforms.name AS platform,
	cv_models.name AS model,
	cv_models_property.amp,
	quantity,
	4 AS temptable
FROM 
	#temp4
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
					AND cv_models.hostid = 0 
		ON 
			#temp4.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
ORDER BY
	platform,
	model

DROP TABLE #temp0
DROP TABLE #temp1
DROP TABLE #temp2
DROP TABLE #temp3
DROP TABLE #temp4








GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosure]    Script Date: 07/31/2009 13:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[pr_getEnclosure]
	@assetid int
AS
SELECT
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.status,
	cva_status.datestamp AS datestamp,
	cv_users.fname + ' ' + cv_users.lname AS statusby,
	cva_status_list.name AS statusname,
	cva_enclosures.*,
	cv_models.make,
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_enclosures.classid,
	cv_classs.name AS class,
	cva_enclosures.environmentid,
	cv_environment.name AS environment,
	cva_enclosures.addressid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cva_enclosures.roomid,
	cv_rooms.name AS room,
	cva_enclosures.rackid,
	cv_racks.name AS rack,
	cva_enclosures.rackposition
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_enclosures
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_enclosures.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_enclosures.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_enclosures.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_rooms
				ON
					cva_enclosures.roomid = cv_rooms.id
					AND cv_rooms.enabled = 1
					AND cv_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_racks
				ON
					cva_enclosures.rackid = cv_racks.id
					AND cv_racks.enabled = 1
					AND cv_racks.deleted = 0
		ON
			cva_assets.id = cva_enclosures.assetid
			AND cva_enclosures.deleted = 0
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.id = @assetid
	AND cva_assets.deleted = 0





















GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosureBlades]    Script Date: 07/31/2009 13:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_getEnclosureBlades]
	@assetid int
AS
SELECT
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.datestamp AS datestamp,
	cv_users.fname + ' ' + cv_users.lname AS statusby,
	cva_status_list.name AS statusname,
	cva_blades.assetid,
	cva_blades.ilo,
	cva_blades.dummy_name,
	cva_blades.macaddress,
	cva_blades.vlan,
	cv_models.make,
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cv_classs.id AS classid,
	cv_classs.name AS class,
	cv_environment.id AS environmentid,
	cv_environment.name AS environment,
	cv_location_address.id AS addressid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_rooms.id AS roomid,
	cv_rooms.name AS room,
	cv_racks.id AS rackid,
	cv_racks.name AS rack,
	cva_enclosures.rackposition,
	cva_enclosures.assetid AS enclosureid,
	cva_blades.slot,
	cva_blades.spare
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_blades
				INNER JOIN
					cva_enclosures
						LEFT OUTER JOIN
							clearview.dbo.cv_classs
						ON
							cva_enclosures.classid = cv_classs.id
							AND cv_classs.enabled = 1
							AND cv_classs.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_environment
						ON
							cva_enclosures.environmentid = cv_environment.id
							AND cv_environment.enabled = 1
							AND cv_environment.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_location_address
								INNER JOIN
									clearview.dbo.cv_location_city
										INNER JOIN
											clearview.dbo.cv_location_state
										ON
											cv_location_city.stateid = cv_location_state.id
											AND cv_location_state.enabled = 1
											AND cv_location_state.deleted = 0
								ON
									cv_location_address.cityid = cv_location_city.id
									AND cv_location_city.enabled = 1
									AND cv_location_city.deleted = 0
						ON
							cva_enclosures.addressid = cv_location_address.id
							AND cv_location_address.enabled = 1
							AND cv_location_address.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_rooms
						ON
							cva_enclosures.roomid = cv_rooms.id
							AND cv_rooms.enabled = 1
							AND cv_rooms.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_racks
						ON
							cva_enclosures.rackid = cv_racks.id
							AND cv_racks.enabled = 1
							AND cv_racks.deleted = 0
				ON
					cva_enclosures.assetid = cva_blades.enclosureid
					AND cva_enclosures.deleted = 0
		ON
			cva_assets.id = cva_blades.assetid
			AND cva_blades.deleted = 0
			AND cva_blades.enclosureid = @assetid
		LEFT OUTER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.deleted = 0
ORDER BY
	cva_blades.slot


























GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosureDR]    Script Date: 07/31/2009 13:24:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[pr_getEnclosureDR]
	@enclosureid int
AS
SELECT
	drid
FROM
	cva_enclosures_dr
WHERE
	enclosureid = @enclosureid
	AND deleted = 0


GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosures]    Script Date: 07/31/2009 13:24:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
























CREATE PROCEDURE [dbo].[pr_getEnclosures]
	@status int
AS
SELECT
	cva_assets.*,
	cv_models.make,
	cv_models_property.name AS modelname, 
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_enclosures
		ON
			cva_assets.id = cva_enclosures.assetid
			AND cva_enclosures.deleted = 0
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = @status
WHERE
	cva_assets.deleted = 0


















GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosuresClass]    Script Date: 07/31/2009 13:24:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[pr_getEnclosuresClass]
	@classid int
AS
SELECT
	cva_assets.*,
	cv_models_property.name AS modelname, 
	cv_classs.name AS class,
	cv_environment.name AS environment,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_rooms.name AS room,
	cv_racks.name AS rack,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp,
	tblRelated.name AS drname
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_enclosures
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_enclosures.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_enclosures.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_enclosures.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_rooms
				ON
					cva_enclosures.roomid = cv_rooms.id
					AND cv_rooms.enabled = 1
					AND cv_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_racks
				ON
					cva_enclosures.rackid = cv_racks.id
					AND cv_racks.enabled = 1
					AND cv_racks.deleted = 0
		ON
			cva_assets.id = cva_enclosures.assetid
			AND cva_enclosures.deleted = 0
			AND cva_enclosures.classid = @classid
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = 10
		OUTER APPLY
		(
			SELECT
				cva_status_dr.name
			FROM
				cva_enclosures_dr
					INNER JOIN
						cva_assets AS cva_assets_dr
							INNER JOIN
								cva_status AS cva_status_dr
							ON
								cva_assets_dr.id = cva_status_dr.assetid
								AND cva_status_dr.deleted = 0
								AND cva_status_dr.status = 10
					ON
						cva_enclosures_dr.drid = cva_assets_dr.id
						AND cva_enclosures_dr.deleted = 0
						AND cva_enclosures_dr.enclosureid = cva_assets.id
			WHERE
				cva_enclosures_dr.deleted = 0
		) AS tblRelated
WHERE
	cva_assets.deleted = 0





















GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosuresClasses]    Script Date: 07/31/2009 13:24:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[pr_getEnclosuresClasses]
	@classes xml
AS
CREATE TABLE #temp (ID int)
INSERT INTO #temp (ID) SELECT ParamValues.ID.value('.','VARCHAR(20)') FROM @classes.nodes('/data/value') as ParamValues(ID) 
SELECT
	cva_assets.*,
	cv_models_property.name AS modelname, 
	cv_classs.name AS class,
	cv_environment.name AS environment,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_rooms.name AS room,
	cv_racks.name AS rack,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp,
	tblRelated.name AS drname
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_enclosures
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_enclosures.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_enclosures.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_enclosures.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_rooms
				ON
					cva_enclosures.roomid = cv_rooms.id
					AND cv_rooms.enabled = 1
					AND cv_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_racks
				ON
					cva_enclosures.rackid = cv_racks.id
					AND cv_racks.enabled = 1
					AND cv_racks.deleted = 0
		ON
			cva_assets.id = cva_enclosures.assetid
			AND cva_enclosures.deleted = 0
			AND cva_enclosures.classid in (SELECT id FROM #temp)
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = 10
		OUTER APPLY
		(
			SELECT
				cva_status_dr.name
			FROM
				cva_enclosures_dr
					INNER JOIN
						cva_assets AS cva_assets_dr
							INNER JOIN
								cva_status AS cva_status_dr
							ON
								cva_assets_dr.id = cva_status_dr.assetid
								AND cva_status_dr.deleted = 0
								AND cva_status_dr.status = 10
					ON
						cva_enclosures_dr.drid = cva_assets_dr.id
						AND cva_enclosures_dr.deleted = 0
						AND cva_enclosures_dr.enclosureid = cva_assets.id
			WHERE
				cva_enclosures_dr.deleted = 0
		) AS tblRelated
WHERE
	cva_assets.deleted = 0























GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosuresDR]    Script Date: 07/31/2009 13:24:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

























create PROCEDURE [dbo].[pr_getEnclosuresDR]
AS
SELECT
	cva_assets.*,
	cv_models.make,
	cv_models_property.name AS modelname, 
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_enclosures
				INNER JOIN
					clearview.dbo.cv_classs
				ON
					cva_enclosures.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
					AND cv_classs.dr = 1
		ON
			cva_assets.id = cva_enclosures.assetid
			AND cva_enclosures.deleted = 0
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = 10
WHERE
	cva_assets.deleted = 0



















GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosureVC]    Script Date: 07/31/2009 13:24:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_getEnclosureVC]
	@id int
AS
SELECT
	*
FROM
	cva_enclosures_vc
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[pr_getEnclosureVCs]    Script Date: 07/31/2009 13:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_getEnclosureVCs]
	@enclosureid int,
	@enabled int
AS
SELECT
	*
FROM
	cva_enclosures_vc
WHERE
	deleted = 0
	and enabled >= @enabled
	and enclosureid = @enclosureid
ORDER BY
	display


GO
/****** Object:  StoredProcedure [dbo].[pr_getGuest]    Script Date: 07/31/2009 13:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO























CREATE PROCEDURE [dbo].[pr_getGuest]
	@assetid int
AS
SELECT
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.status,
	cva_status.datestamp AS datestamp,
	cv_users.fname + ' ' + cv_users.lname AS statusby,
	cva_status_list.name AS statusname,
	cva_guests.*,
	cv_models.make,
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cv_classs.name AS class,
	cv_environment.name AS environment,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_guests
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_guests.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_guests.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_guests.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
		ON
			cva_assets.id = cva_guests.assetid
			AND cva_guests.deleted = 0
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.id = @assetid
	AND cva_assets.deleted = 0


















GO
/****** Object:  StoredProcedure [dbo].[pr_getHBAs]    Script Date: 07/31/2009 13:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[pr_getHBAs]
	@assetid int
AS
SELECT
	*
FROM
	cva_hba
WHERE
	assetid = @assetid
	AND deleted = 0













GO
/****** Object:  StoredProcedure [dbo].[pr_getHostLocations]    Script Date: 07/31/2009 13:24:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






create PROCEDURE [dbo].[pr_getHostLocations]
	@assetid int
AS
SELECT
	*
FROM
	cva_hosts_locations
WHERE
	assetid = @assetid
	AND deleted = 0











GO
/****** Object:  StoredProcedure [dbo].[pr_getHostsStorageVMWare]    Script Date: 07/31/2009 13:24:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[pr_getHostsStorageVMWare]
	@hostid int
AS
SELECT
	cva_hosts_vmware.*,
	cv_storage_luns.actual_size AS l_actual_size,
	cv_storage_luns.actual_size_qa AS l_actual_size_qa,
	cv_storage_luns.actual_size_test AS l_actual_size_test,
	cv_storage_mount_points.actual_size AS m_actual_size,
	cv_storage_mount_points.actual_size_qa AS m_actual_size_qa,
	cv_storage_mount_points.actual_size_test AS m_actual_size_test
FROM
	cva_hosts_vmware
		INNER JOIN
			clearview.dbo.cv_servers_assets
				INNER JOIN
					clearview.dbo.cv_servers
						INNER JOIN
							clearview.dbo.cv_storage_luns
								INNER JOIN
									clearview.dbo.cv_storage_mount_points
								ON
									cv_storage_luns.id = cv_storage_mount_points.lunid
									and cv_storage_mount_points.deleted = 0
						ON
							cv_servers.answerid = cv_storage_luns.answerid
							and cv_storage_luns.deleted = 0
				ON
					cv_servers_assets.serverid = cv_servers.id
					and cv_servers.deleted = 0
		ON
			cva_hosts_vmware.assetid = cv_servers_assets.assetid
			and cv_servers_assets.deleted = 0
WHERE
	cva_hosts_vmware.hostid = @hostid
	AND cva_hosts_vmware.deleted = 0
















GO
/****** Object:  StoredProcedure [dbo].[pr_getHostsVMWare]    Script Date: 07/31/2009 13:24:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[pr_getHostsVMWare]
	@hostid int
AS
SELECT
	cva_hosts_vmware.*,
	cv_models_property.amp,
	cv_models_property.network_ports,
	cv_models_property.storage_ports,
	cv_models_property.ram,
	cv_models_property.cpu_count,
	cv_models_property.cpu_speed
FROM
	cva_hosts_vmware
		INNER JOIN
			cva_assets
				INNER JOIN 
					clearview.dbo.cv_models_property 
						INNER JOIN 
							clearview.dbo.cv_models 
								INNER JOIN 
									clearview.dbo.cv_types 
										INNER JOIN 
											clearview.dbo.cv_platforms 
										ON 
											cv_types.platformid = cv_platforms.platformid 
											AND cv_platforms.enabled = 1 
											AND cv_platforms.deleted = 0 
								ON 
									cv_models.typeid = cv_types.id 
									AND cv_types.enabled = 1 
									AND cv_types.deleted = 0 
						ON 
							cv_models_property.modelid = cv_models.id 
							AND cv_models.enabled = 1 
							AND cv_models.deleted = 0 
				ON 
					cva_assets.modelid = cv_models_property.id 
					AND cv_models_property.deleted = 0 
		ON
			cva_hosts_vmware.assetid = cva_assets.id
			and cva_assets.deleted = 0
WHERE
	cva_hosts_vmware.hostid = @hostid
	AND cva_hosts_vmware.deleted = 0













GO
/****** Object:  StoredProcedure [dbo].[pr_getHostVirtual]    Script Date: 07/31/2009 13:24:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









create PROCEDURE [dbo].[pr_getHostVirtual]
	@assetid int
AS
SELECT
	cva_status.name,
	cva_hosts_virtual.*
FROM
	cva_hosts_virtual
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
		ON
			cva_hosts_virtual.assetid = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_hosts_virtual.assetid = @assetid
	AND cva_hosts_virtual.deleted = 0














GO
/****** Object:  StoredProcedure [dbo].[pr_getHostVirtualEnvironment]    Script Date: 07/31/2009 13:24:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[pr_getHostVirtualEnvironment]
	@assetid int
AS
SELECT
	*
FROM
	cva_hosts_virtual_environment
WHERE
	assetid = @assetid
	AND deleted = 0



GO
/****** Object:  StoredProcedure [dbo].[pr_getHostVirtualOs]    Script Date: 07/31/2009 13:24:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[pr_getHostVirtualOs]
	@assetid int
AS
SELECT
	cv_operating_systems.name AS os,
	cva_hosts_virtual_os.*
FROM
	cva_hosts_virtual_os
		INNER JOIN
			clearview.dbo.cv_operating_systems
		ON
			cva_hosts_virtual_os.osid = cv_operating_systems.id
			AND cv_operating_systems.enabled = 1
			AND cv_operating_systems.deleted = 0
WHERE
	cva_hosts_virtual_os.assetid = @assetid
	AND cva_hosts_virtual_os.deleted = 0



GO
/****** Object:  StoredProcedure [dbo].[pr_getHostVirtuals]    Script Date: 07/31/2009 13:24:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_getHostVirtuals]
	@environment int,
	@osid int
AS
SELECT
	cva_hosts_virtual.*
FROM
	cva_hosts_virtual
		INNER JOIN
			cva_hosts_virtual_os
		ON
			cva_hosts_virtual.assetid = cva_hosts_virtual_os.assetid
			AND cva_hosts_virtual_os.osid = @osid
			AND cva_hosts_virtual_os.deleted = 0
		INNER JOIN
			cva_hosts_virtual_environment
		ON
			cva_hosts_virtual.assetid = cva_hosts_virtual_environment.assetid
			AND cva_hosts_virtual_environment.environment = @environment
			AND cva_hosts_virtual_environment.deleted = 0
WHERE
	cva_hosts_virtual.deleted = 0







GO
/****** Object:  StoredProcedure [dbo].[pr_getHostVMWare]    Script Date: 07/31/2009 13:24:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









create PROCEDURE [dbo].[pr_getHostVMWare]
	@assetid int
AS
SELECT
	cva_status.name,
	cva_hosts_vmware.*
FROM
	cva_hosts_vmware
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
		ON
			cva_hosts_vmware.assetid = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_hosts_vmware.assetid = @assetid
	AND cva_hosts_vmware.deleted = 0














GO
/****** Object:  StoredProcedure [dbo].[pr_getIPs]    Script Date: 07/31/2009 13:24:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







create PROCEDURE [dbo].[pr_getIPs]
	@assetid int
AS
SELECT
	*
FROM
	cva_ips
WHERE
	assetid = @assetid
	AND deleted = 0













GO
/****** Object:  StoredProcedure [dbo].[pr_getNetwork]    Script Date: 07/31/2009 13:24:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[pr_getNetwork]
	@assetid int
AS
SELECT
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.status,
	cva_status.datestamp AS datestamp,
	cv_users.fname + ' ' + cv_users.lname AS statusby,
	cva_status_list.name AS statusname,
	cva_network.assetid,
	cv_models.make,
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_network.depotid,
	cv_depot.name AS depot,
	cv_depot_rooms.name AS depotroom,
	cv_shelfs.name AS shelf,
	cva_network.available_ports AS ports,
	cva_network.classid,
	cv_classs.name AS class,
	cva_network.environmentid,
	cv_environment.name AS environment,
	cva_network.addressid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cva_network.roomid,
	cv_rooms.name AS room,
	cva_network.rackid,
	cv_racks.name AS rack,
	cva_network.rackposition,
	cv_ip_addresses.add1,
	cv_ip_addresses.add2,
	cv_ip_addresses.add3,
	cv_ip_addresses.add4
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_network
				LEFT OUTER JOIN
					clearview.dbo.cv_depot
				ON
					cva_network.depotid = cv_depot.id
					AND cv_depot.enabled = 1
					AND cv_depot.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_depot_rooms
				ON
					cva_network.depotroomid = cv_depot_rooms.id
					AND cv_depot_rooms.enabled = 1
					AND cv_depot_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_shelfs
				ON
					cva_network.shelfid = cv_shelfs.id
					AND cv_shelfs.enabled = 1
					AND cv_shelfs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_network.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_network.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_network.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_rooms
				ON
					cva_network.roomid = cv_rooms.id
					AND cv_rooms.enabled = 1
					AND cv_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_racks
				ON
					cva_network.rackid = cv_racks.id
					AND cv_racks.enabled = 1
					AND cv_racks.deleted = 0
				LEFT OUTER JOIN
					cva_ips
						INNER JOIN
							clearviewip.dbo.cv_ip_addresses
						ON
							cva_ips.ipaddressid = cv_ip_addresses.id
							AND cv_ip_addresses.deleted = 0
				ON
					cva_network.assetid = cva_ips.assetid
					AND cva_ips.deleted = 0
		ON
			cva_assets.id = cva_network.assetid
			AND cva_network.deleted = 0
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.id = @assetid
	AND cva_assets.deleted = 0






GO
/****** Object:  StoredProcedure [dbo].[pr_getOrder]    Script Date: 07/31/2009 13:24:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[pr_getOrder]
	@id int
AS
SELECT
	*
FROM
	cva_orders
WHERE
	id = @id








GO
/****** Object:  StoredProcedure [dbo].[pr_getOrders]    Script Date: 07/31/2009 13:24:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_getOrders]
	@platformid int
AS
SELECT
	CASE
		WHEN cv_models_property.name = '' OR cv_models_property.name is null THEN cv_models.name
		ELSE cv_models_property.name
	END AS modelname,
	cv_environment.name AS environment,
	cv_classs.name AS class,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cva_orders.*
FROM
	cva_orders
		INNER JOIN
			clearview.dbo.cv_models_property
				INNER JOIN
					clearview.dbo.cv_models
						INNER JOIN
							clearview.dbo.cv_types
						ON
							cv_models.typeid = cv_types.id
							AND cv_types.platformid = @platformid
							AND cv_types.enabled = 1
				ON
					cv_models_property.modelid = cv_models.id
					AND cv_models.enabled = 1
					AND cv_models.deleted = 0
		ON
			cva_orders.modelid = cv_models_property.id
			AND cv_models_property.enabled = 1
		INNER JOIN
			clearview.dbo.cv_location_address
				INNER JOIN
					clearview.dbo.cv_location_city
						INNER JOIN
							clearview.dbo.cv_location_state
						ON
							cv_location_city.stateid = cv_location_state.id
							AND cv_location_state.enabled = 1
							AND cv_location_state.deleted = 0
				ON
					cv_location_address.cityid = cv_location_city.id
					AND cv_location_city.enabled = 1
					AND cv_location_city.deleted = 0
		ON
			cva_orders.addressid = cv_location_address.id
			AND cv_location_address.enabled = 1
			AND cv_location_address.deleted = 0
		INNER JOIN
			clearview.dbo.cv_environment
		ON
			cva_orders.environmentid = cv_environment.id
			AND cv_environment.enabled = 1
			AND cv_environment.deleted = 0
		INNER JOIN
			clearview.dbo.cv_classs
		ON
			cva_orders.classid = cv_classs.id
			AND cv_classs.enabled = 1
			AND cv_classs.deleted = 0
WHERE
	cva_orders.deleted = 0
	and cva_orders.show = 1
ORDER BY
	cva_orders.ordered DESC



















GO
/****** Object:  StoredProcedure [dbo].[pr_getOrderTracking]    Script Date: 07/31/2009 13:24:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




create PROCEDURE [dbo].[pr_getOrderTracking]
	@tracking varchar(50)
AS
SELECT
	*
FROM
	cva_orders
WHERE
	tracking = @tracking









GO
/****** Object:  StoredProcedure [dbo].[pr_getReservations]    Script Date: 07/31/2009 13:24:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







create PROCEDURE [dbo].[pr_getReservations]
	@buildid int
AS
SELECT
	*
FROM
	cva_reservations
WHERE
	buildid = @buildid
	AND deleted = 0













GO
/****** Object:  StoredProcedure [dbo].[pr_getSearch]    Script Date: 07/31/2009 13:24:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[pr_getSearch]
	@id int
AS
SELECT
	*
FROM
	cva_search
WHERE
	id = @id









GO
/****** Object:  StoredProcedure [dbo].[pr_getSearchClass]    Script Date: 07/31/2009 13:24:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE PROCEDURE [dbo].[pr_getSearchClass]
	@platformid int,
	@classid int,
	@environmentid int
AS
-- NETWORK
SELECT
	cva_assets.id,
	cva_assets.serial,
	cva_assets.asset,
	cv_models_property.name AS model, 
	cv_models.make AS make, 
	cv_types.name AS type, 
	cv_platforms.name AS platform, 
	cva_status.name,
	cva_status.status,
	cva_status_list.name AS statusname
FROM 
	cva_assets 
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
									AND cv_platforms.platformid = @platformid
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
		INNER JOIN
			cva_network
		ON
			cva_assets.id = cva_network.assetid
			AND cva_network.deleted = 0
WHERE
	cva_assets.deleted = 0
	AND cva_network.classid = @classid
	AND @environmentid = 0
	OR cva_assets.deleted = 0
	AND cva_network.classid = @classid
	AND cva_network.environmentid = @environmentid






GO
/****** Object:  StoredProcedure [dbo].[pr_getSearchDepot]    Script Date: 07/31/2009 13:24:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE PROCEDURE [dbo].[pr_getSearchDepot]
	@platformid int,
	@depotid int
AS

SELECT
	cva_assets.id,
	cva_assets.serial,
	cva_assets.asset,
	cv_models_property.name AS model, 
	cv_models.make AS make, 
	cv_types.name AS type, 
	cv_platforms.name AS platform, 
	cva_status.name,
	cva_status.status,
	cva_status_list.name AS statusname
FROM 
	cva_assets 
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
									AND cv_platforms.platformid = @platformid
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
		INNER JOIN
			cva_network
		ON
			cva_assets.id = cva_network.assetid
			AND cva_network.deleted = 0
			AND cva_network.depotid = @depotid
WHERE
	cva_assets.deleted = 0









GO
/****** Object:  StoredProcedure [dbo].[pr_getSearchName]    Script Date: 07/31/2009 13:24:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE PROCEDURE [dbo].[pr_getSearchName]
	@name varchar(100),
	@serial varchar(50),
	@asset varchar(20)
AS

SELECT
	cva_assets.id,
	cva_assets.serial,
	cva_assets.asset,
	cv_models_property.name AS model, 
	cv_models.make AS make, 
	cv_types.name AS type, 
	cv_platforms.name AS platform, 
	cva_status.name,
	cva_status.status,
	cva_status_list.name AS statusname
FROM 
	cva_assets 
		LEFT OUTER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.deleted = 0
	AND cva_status.name LIKE @name + '%'
	AND @serial = ''
	AND @asset = ''
	OR cva_assets.deleted = 0
	AND @name = ''
	AND cva_assets.serial LIKE @serial + '%'
	AND @asset = ''
	OR cva_assets.deleted = 0
	AND @name = ''
	AND @serial = ''
	AND cva_assets.asset LIKE @asset + '%'
	OR cva_assets.deleted = 0
	AND cva_status.name LIKE @name + '%'
	AND cva_assets.serial LIKE @serial + '%'
	AND @asset = ''
	OR cva_assets.deleted = 0
	AND cva_status.name LIKE @name + '%'
	AND @serial = ''
	AND cva_assets.asset LIKE @asset + '%'
	OR cva_assets.deleted = 0
	AND @name = ''
	AND cva_assets.serial LIKE @serial + '%'
	AND cva_assets.asset LIKE @asset + '%'
	OR cva_assets.deleted = 0
	AND cva_status.name LIKE @name + '%'
	AND cva_assets.serial LIKE @serial + '%'
	AND cva_assets.asset LIKE @asset + '%'



GO
/****** Object:  StoredProcedure [dbo].[pr_getSearchPlatform]    Script Date: 07/31/2009 13:24:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[pr_getSearchPlatform]
	@platformid int,
	@typeid int,
	@modelid int
AS

SELECT
	cva_assets.id,
	cva_assets.serial,
	cva_assets.asset,
	cv_models_property.name AS model, 
	cv_models.make AS make, 
	cv_types.name AS type, 
	cv_platforms.name AS platform, 
	cva_status.name,
	cva_status.status,
	cva_status_list.name AS statusname
FROM 
	cva_assets 
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
									AND cv_platforms.platformid = @platformid
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.deleted = 0
	AND @typeid = 0
	AND @modelid = 0
	OR cva_assets.deleted = 0
	AND cv_types.id = @typeid
	AND @modelid = 0
	OR cva_assets.deleted = 0
	AND @typeid = 0
	AND cv_models.id = @modelid
	OR cva_assets.deleted = 0
	AND cv_types.id = @typeid
	AND cv_models.id = @modelid



GO
/****** Object:  StoredProcedure [dbo].[pr_getServerOrBlade]    Script Date: 07/31/2009 13:24:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[pr_getServerOrBlade]
	@assetid int
AS
SELECT
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.status,
	cva_status.datestamp AS datestamp,
	cv_users.fname + ' ' + cv_users.lname AS statusby,
	cva_status_list.name AS statusname,
	cva_server.assetid,
	cva_server.ilo,
	cva_server.dummy_name,
	cva_server.macaddress,
	cva_server.vlan,
	cv_models.make,
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_server.classid,
	cv_classs.name AS class,
	cva_server.environmentid,
	cv_environment.name AS environment,
	cva_server.addressid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cva_server.roomid,
	cv_rooms.name AS room,
	cva_server.rackid,
	cv_racks.name AS rack,
	cva_server.rackposition,
	0 AS enclosureid,
	0 AS slot,
	0 AS spare
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_server
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_server.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_server.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_server.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_rooms
				ON
					cva_server.roomid = cv_rooms.id
					AND cv_rooms.enabled = 1
					AND cv_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_racks
				ON
					cva_server.rackid = cv_racks.id
					AND cv_racks.enabled = 1
					AND cv_racks.deleted = 0
		ON
			cva_assets.id = cva_server.assetid
			AND cva_server.deleted = 0
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.id = @assetid
	AND cva_assets.deleted = 0
UNION ALL
SELECT
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.status,
	cva_status.datestamp AS datestamp,
	cv_users.fname + ' ' + cv_users.lname AS statusby,
	cva_status_list.name AS statusname,
	cva_blades.assetid,
	cva_blades.ilo,
	cva_blades.dummy_name,
	cva_blades.macaddress,
	cva_blades.vlan,
	cv_models.make,
	cv_models.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cva_enclosures.classid,
	cv_classs.name AS class,
	cva_enclosures.environmentid,
	cv_environment.name AS environment,
	cva_enclosures.addressid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cva_enclosures.roomid,
	cv_rooms.name AS room,
	cva_enclosures.rackid,
	cv_racks.name AS rack,
	cva_enclosures.rackposition,
	cva_enclosures.assetid AS enclosureid,
	cva_blades.slot,
	cva_blades.spare
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_blades
				INNER JOIN
					cva_enclosures
						LEFT OUTER JOIN
							clearview.dbo.cv_classs
						ON
							cva_enclosures.classid = cv_classs.id
							AND cv_classs.enabled = 1
							AND cv_classs.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_environment
						ON
							cva_enclosures.environmentid = cv_environment.id
							AND cv_environment.enabled = 1
							AND cv_environment.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_location_address
								INNER JOIN
									clearview.dbo.cv_location_city
										INNER JOIN
											clearview.dbo.cv_location_state
										ON
											cv_location_city.stateid = cv_location_state.id
											AND cv_location_state.enabled = 1
											AND cv_location_state.deleted = 0
								ON
									cv_location_address.cityid = cv_location_city.id
									AND cv_location_city.enabled = 1
									AND cv_location_city.deleted = 0
						ON
							cva_enclosures.addressid = cv_location_address.id
							AND cv_location_address.enabled = 1
							AND cv_location_address.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_rooms
						ON
							cva_enclosures.roomid = cv_rooms.id
							AND cv_rooms.enabled = 1
							AND cv_rooms.deleted = 0
						LEFT OUTER JOIN
							clearview.dbo.cv_racks
						ON
							cva_enclosures.rackid = cv_racks.id
							AND cv_racks.enabled = 1
							AND cv_racks.deleted = 0
				ON
					cva_enclosures.assetid = cva_blades.enclosureid
					AND cva_enclosures.deleted = 0
		ON
			cva_assets.id = cva_blades.assetid
			AND cva_blades.deleted = 0
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
WHERE
	cva_assets.id = @assetid
	AND cva_assets.deleted = 0

























GO
/****** Object:  StoredProcedure [dbo].[pr_getServerOrBladeAvailable]    Script Date: 07/31/2009 13:24:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_getServerOrBladeAvailable]
	@classid int,
	@environmentid int,
	@addressid int,
	@modelid int
AS
SELECT
	cva_assets.id,
	0 AS enclosureid,
	0 AS total
FROM
	cva_assets
		INNER JOIN
			cva_server
		ON
			cva_assets.id = cva_server.assetid
			AND cva_server.classid IN 
			(
				SELECT class1 AS classid FROM ClearView.dbo.cv_class_joins WHERE class2 = @classid AND deleted = 0
				UNION ALL
				SELECT class2 AS classid FROM ClearView.dbo.cv_class_joins WHERE class1 = @classid AND deleted = 0
			)
			AND cva_server.environmentid = @environmentid
			AND cva_server.addressid = @addressid
			AND cva_server.deleted = 0
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = 1
WHERE
	cva_assets.deleted = 0
	AND cva_assets.bad = 0
	AND cva_assets.modelid = @modelid
UNION ALL
SELECT
	cva_assets.id,
	cva_blades.enclosureid,
	cva_blades_total.total
FROM
	cva_assets
		INNER JOIN
			cva_blades
				INNER JOIN
					cva_enclosures
						INNER JOIN
							cva_assets AS cva_assets_enclosure
						ON
							cva_enclosures.assetid = cva_assets_enclosure.id
							AND cva_assets_enclosure.deleted = 0
							AND cva_assets_enclosure.bad = 0
				ON
					cva_blades.enclosureid = cva_enclosures.assetid
					AND cva_enclosures.classid IN 
					(
						SELECT class1 AS classid FROM ClearView.dbo.cv_class_joins WHERE class2 = @classid AND deleted = 0
						UNION ALL
						SELECT class2 AS classid FROM ClearView.dbo.cv_class_joins WHERE class1 = @classid AND deleted = 0
					)
					AND cva_enclosures.environmentid = @environmentid
					AND cva_enclosures.addressid = @addressid
					AND cva_enclosures.deleted = 0
				OUTER APPLY
					(SELECT COUNT(tblBlade.assetid) AS total FROM cva_blades AS tblBlade INNER JOIN cva_assets AS tblAsset INNER JOIN cva_status AS tblStatus ON tblAsset.id = tblStatus.assetid AND tblStatus.deleted = 0 AND tblStatus.status = 1 ON tblBlade.assetid = tblAsset.id AND tblAsset.deleted = 0 WHERE tblBlade.deleted = 0 AND tblBlade.enclosureid = cva_blades.enclosureid) AS cva_blades_total
		ON
			cva_assets.id = cva_blades.assetid
			AND cva_blades.deleted = 0
			AND cva_blades.spare = 0
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = 1
WHERE
	cva_assets.deleted = 0
	AND cva_assets.bad = 0
	AND cva_assets.modelid = @modelid
ORDER BY
	total DESC
























GO
/****** Object:  StoredProcedure [dbo].[pr_getServerOrBladeAvailableDR]    Script Date: 07/31/2009 13:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[pr_getServerOrBladeAvailableDR]
	@environmentid int,
	@modelid int,
	@enclosureid int,
	@slot int
AS
SELECT
	cva_assets.id
FROM
	cva_assets
		INNER JOIN
			cva_server
				INNER JOIN
					clearview.dbo.cv_location_address
				ON
					cva_server.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
					AND cv_location_address.dr = 1
				INNER JOIN
					clearview.dbo.cv_classs
				ON
					cva_server.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
					AND cv_classs.dr = 1
		ON
			cva_assets.id = cva_server.assetid
			AND cva_server.environmentid = @environmentid
			AND cva_server.deleted = 0
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = 1
WHERE
	cva_assets.deleted = 0
	AND cva_assets.bad = 0
	AND cva_assets.modelid = @modelid
UNION ALL
SELECT
	cva_assets.id
FROM
	cva_assets
		INNER JOIN
			cva_blades
				INNER JOIN
					cva_enclosures
						INNER JOIN
							cva_assets AS cva_assets_enclosure
						ON
							cva_enclosures.assetid = cva_assets_enclosure.id
							AND cva_assets_enclosure.deleted = 0
							AND cva_assets_enclosure.bad = 0
						INNER JOIN
							clearview.dbo.cv_location_address
						ON
							cva_enclosures.addressid = cv_location_address.id
							AND cv_location_address.enabled = 1
							AND cv_location_address.deleted = 0
							AND cv_location_address.dr = 1
						INNER JOIN
							clearview.dbo.cv_classs
						ON
							cva_enclosures.classid = cv_classs.id
							AND cv_classs.enabled = 1
							AND cv_classs.deleted = 0
							AND cv_classs.dr = 1
				ON
					cva_blades.enclosureid = cva_enclosures.assetid
					AND cva_enclosures.environmentid = @environmentid
					AND cva_enclosures.deleted = 0
					AND cva_enclosures.assetid = @enclosureid
		ON
			cva_assets.id = cva_blades.assetid
			AND cva_blades.deleted = 0
			AND cva_blades.spare = 0
			AND cva_blades.slot = @slot
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = 1
WHERE
	cva_assets.deleted = 0
	AND cva_assets.bad = 0
	AND cva_assets.modelid = @modelid
























GO
/****** Object:  StoredProcedure [dbo].[pr_getServerOrBlades]    Script Date: 07/31/2009 13:24:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE PROCEDURE [dbo].[pr_getServerOrBlades]
	@modelid int,
	@status int
AS
SELECT
	cva_server.addressid,
	cva_server.classid,
	cva_server.environmentid,
	cva_assets.*,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN
			cva_server
		ON
			cva_assets.id = cva_server.assetid
			AND cva_server.deleted = 0
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = @status
WHERE
	cva_assets.modelid = @modelid
	AND cva_assets.deleted = 0
UNION ALL
SELECT
	cva_enclosures.addressid,
	cva_enclosures.classid,
	cva_enclosures.environmentid,
	cva_assets.*,
	cva_status.name,
	cva_status.status,
	cva_status.userid,
	cva_status.datestamp
FROM
	cva_assets
		INNER JOIN
			cva_blades
				INNER JOIN
					cva_enclosures
				ON
					cva_enclosures.id = cva_blades.enclosureid
					AND cva_enclosures.deleted = 0
		ON
			cva_assets.id = cva_blades.assetid
			AND cva_blades.deleted = 0
		INNER JOIN
			cva_status
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
			AND cva_status.status = @status
WHERE
	cva_assets.modelid = @modelid
	AND cva_assets.deleted = 0

ORDER BY
	addressid,
	classid,
	environmentid










GO
/****** Object:  StoredProcedure [dbo].[pr_getStatus]    Script Date: 07/31/2009 13:24:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















create PROCEDURE [dbo].[pr_getStatus]
	@assetid int
AS
SELECT
	*
FROM
	cva_status
WHERE
	assetid = @assetid
	AND deleted = 0













GO
/****** Object:  StoredProcedure [dbo].[pr_getSupplyNetwork]    Script Date: 07/31/2009 13:24:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[pr_getSupplyNetwork]
AS
SELECT
	cva_assets.modelid,
	cva_assets.serial,
	cva_assets.asset,
	cva_assets.bad,
	cva_assets.validated,
	cva_status.name,
	cva_status.datestamp AS datestamp,
	cv_users.fname + ' ' + cv_users.lname AS statusby,
	cva_status_list.name AS statusname,
	cva_network.*,
	cv_models.make,
	cv_models_property.name AS model,
	cv_types.name AS type,
	cv_platforms.name AS platform,
	cv_depot.name AS stock,
	cv_depot_rooms.name AS stockroom,
	cv_shelfs.name AS shelf,
	cv_users.fname + ' ' + cv_users.lname AS technician,
	cast(cv_ip_addresses.add1 as varchar(3)) + '.' + cast(cv_ip_addresses.add2 as varchar(3)) + '.' + cast(cv_ip_addresses.add3 as varchar(3)) + '.' + cast(cv_ip_addresses.add4 as varchar(3)) AS ipaddress,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_racks.name AS rack,
	cv_environment.name AS environment,
	cv_classs.name AS class,
	cv_rooms.name AS room
FROM
	cva_assets
		INNER JOIN 
			clearview.dbo.cv_models_property 
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property.id 
			AND cv_models_property.deleted = 0 
		INNER JOIN
			cva_network
				LEFT OUTER JOIN
					clearview.dbo.cv_depot_rooms
				ON
					cva_network.depotroomid = cv_depot_rooms.id
					AND cv_depot_rooms.enabled = 1
					AND cv_depot_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_network.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_racks
				ON
					cva_network.rackid = cv_racks.id
					AND cv_racks.enabled = 1
					AND cv_racks.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_network.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_network.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_rooms
				ON
					cva_network.roomid = cv_rooms.id
					AND cv_rooms.enabled = 1
					AND cv_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_depot
				ON
					cva_network.depotid = cv_depot.id
					AND cv_depot.enabled = 1
					AND cv_depot.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_shelfs
				ON
					cva_network.shelfid = cv_shelfs.id
					AND cv_shelfs.enabled = 1
					AND cv_shelfs.deleted = 0
		ON
			cva_assets.id = cva_network.assetid
			AND cva_network.deleted = 0
		INNER JOIN
			cva_status
				INNER JOIN
					cva_status_list
				ON
					cva_status.status = cva_status_list.id
					AND cva_status_list.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users
				ON
					cva_status.userid = cv_users.userid
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0
		LEFT OUTER JOIN
			cva_ips
				INNER JOIN
					clearviewip.dbo.cv_ip_addresses
				ON
					cva_ips.ipaddressid = cv_ip_addresses.id
					AND cv_ip_addresses.deleted = 0
		ON
			cva_assets.id = cva_ips.assetid
			AND cva_ips.deleted = 0
WHERE
	cva_assets.deleted = 0
















GO
/****** Object:  StoredProcedure [dbo].[pr_getVSG]    Script Date: 07/31/2009 13:24:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[pr_getVSG]
	@type varchar(20)
AS
SELECT TOP 1
	*
FROM
	cva_vsg
WHERE
	assignedon is null
	and type = @type





GO
/****** Object:  StoredProcedure [dbo].[pr_getVSGName]    Script Date: 07/31/2009 13:24:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create procedure [dbo].[pr_getVSGName]
	@name varchar(50),
	@type varchar(20)
AS
SELECT
	*
FROM
	cva_vsg
WHERE
	assignedto = @name
	and type = @type
	and assignedon is not null






GO
/****** Object:  StoredProcedure [dbo].[pr_getVSGs]    Script Date: 07/31/2009 13:24:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[pr_getVSGs]
	@type varchar(20)
AS
SELECT
	*
FROM
	cva_vsg
WHERE
	assignedon is null
	and type = @type
ORDER BY
	name



GO
/****** Object:  StoredProcedure [dbo].[pr_updateAsset]    Script Date: 07/31/2009 13:24:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







create PROCEDURE [dbo].[pr_updateAsset]
	@assetid int,
	@modelid int,
	@serial varchar(50),
	@asset varchar(20),
	@bad int
AS
UPDATE
	cva_assets
SET
	modelid = @modelid,
	serial = @serial,
	asset = @asset,
	bad = @bad,
	modified = getdate()
WHERE
	id = @assetid









GO
/****** Object:  StoredProcedure [dbo].[pr_updateAssetAsset]    Script Date: 07/31/2009 13:24:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





create PROCEDURE [dbo].[pr_updateAssetAsset]
	@assetid int,
	@asset varchar(20)
AS
UPDATE
	cva_assets
SET
	asset = @asset,
	modified = getdate()
WHERE
	id = @assetid







GO
/****** Object:  StoredProcedure [dbo].[pr_updateAssetBad]    Script Date: 07/31/2009 13:24:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[pr_updateAssetBad]
	@assetid int,
	@bad int
AS
UPDATE
	cva_assets
SET
	bad = @bad,
	modified = getdate()
WHERE
	id = @assetid






GO
/****** Object:  StoredProcedure [dbo].[pr_updateAssetModel]    Script Date: 07/31/2009 13:24:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





create PROCEDURE [dbo].[pr_updateAssetModel]
	@assetid int,
	@modelid int
AS
UPDATE
	cva_assets
SET
	modelid = @modelid,
	modified = getdate()
WHERE
	id = @assetid







GO
/****** Object:  StoredProcedure [dbo].[pr_updateAssetSerial]    Script Date: 07/31/2009 13:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






create PROCEDURE [dbo].[pr_updateAssetSerial]
	@assetid int,
	@serial varchar(50),
	@asset varchar(20)
AS
UPDATE
	cva_assets
SET
	serial = @serial,
	asset = @asset,
	modified = getdate()
WHERE
	id = @assetid








GO
/****** Object:  StoredProcedure [dbo].[pr_updateAssetValidated]    Script Date: 07/31/2009 13:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[pr_updateAssetValidated]
	@assetid int,
	@validated int
AS
UPDATE
	cva_assets
SET
	validated = @validated
WHERE
	id = @assetid




GO
/****** Object:  StoredProcedure [dbo].[pr_updateBlade]    Script Date: 07/31/2009 13:24:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















CREATE PROCEDURE [dbo].[pr_updateBlade]
	@assetid int,
	@enclosureid int,
	@ilo varchar(15),
	@dummy_name varchar(50),
	@macaddress varchar(50),
	@vlan int,
	@slot int,
	@spare int
AS
UPDATE
	cva_blades
SET
	enclosureid = @enclosureid,
	ilo = @ilo,
	dummy_name = @dummy_name,
	macaddress = @macaddress,
	vlan = @vlan,
	slot = @slot,
	spare = @spare,
	modified = getdate()
WHERE
	assetid = @assetid



















GO
/****** Object:  StoredProcedure [dbo].[pr_updateDecommission]    Script Date: 07/31/2009 13:24:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[pr_updateDecommission]
	@assetid int,
	@destroy datetime,
	@vmware int,
	@name varchar(50)
AS
UPDATE
	cva_decommissions
SET
	turnedoff = getdate(),
	destroy = @destroy,
	vmware = @vmware,
	name = @name,
	modified = getdate()
WHERE
	assetid = @assetid
	AND deleted = 0

GO
/****** Object:  StoredProcedure [dbo].[pr_updateDecommissionActive]    Script Date: 07/31/2009 13:24:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[pr_updateDecommissionActive]
	@requestid int,
	@itemid int,
	@number int
AS
UPDATE
	cva_decommissions
SET
	active = 1
WHERE
	requestid = @requestid
	AND itemid = @itemid
	AND number = @number
	AND deleted = 0


GO
/****** Object:  StoredProcedure [dbo].[pr_updateDecommissionDestroy]    Script Date: 07/31/2009 13:24:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[pr_updateDecommissionDestroy]
	@assetid int
AS
UPDATE
	cva_decommissions
SET
	destroyed = getdate(),
	modified = getdate()
WHERE
	assetid = @assetid
	AND deleted = 0

GO
/****** Object:  StoredProcedure [dbo].[pr_updateEnclosure]    Script Date: 07/31/2009 13:24:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















CREATE PROCEDURE [dbo].[pr_updateEnclosure]
	@assetid int,
	@classid int,
	@environmentid int,
	@addressid int,
	@roomid int,
	@rackid int,
	@rackposition varchar(10),
	@vlan int,
	@oa_ip varchar(15)
AS
UPDATE
	cva_enclosures
SET
	classid = @classid,
	environmentid = @environmentid,
	addressid = @addressid,
	roomid = @roomid,
	rackid = @rackid,
	rackposition = @rackposition,
	vlan = @vlan,
	oa_ip = @oa_ip,
	modified = getdate()
WHERE
	assetid = @assetid



















GO
/****** Object:  StoredProcedure [dbo].[pr_updateEnclosureVC]    Script Date: 07/31/2009 13:24:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_updateEnclosureVC]
	@id int,
	@virtual_connect varchar(15),
	@enabled int
AS
UPDATE
	cva_enclosures_vc
SET
	virtual_connect = @virtual_connect,
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[pr_updateEnclosureVCEnabled]    Script Date: 07/31/2009 13:24:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_updateEnclosureVCEnabled]
	@id int,
	@enabled int
AS
UPDATE
	cva_enclosures_vc
SET
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[pr_updateEnclosureVCOrder]    Script Date: 07/31/2009 13:24:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_updateEnclosureVCOrder]
	@id int,
	@display int
AS
UPDATE
	cva_enclosures_vc
SET
	display = @display,
	modified = getdate()
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[pr_updateGuest]    Script Date: 07/31/2009 13:24:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO













create PROCEDURE [dbo].[pr_updateGuest]
	@assetid int,
	@classid int,
	@environmentid int
AS
UPDATE
	cva_guests
SET
	classid = @classid,
	environmentid = @environmentid,
	modified = getdate()
WHERE
	assetid = @assetid













GO
/****** Object:  StoredProcedure [dbo].[pr_updateNetwork]    Script Date: 07/31/2009 13:24:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE PROCEDURE [dbo].[pr_updateNetwork]
	@assetid int,
	@depotid int,
	@depotroomid int,
	@shelfid int,
	@available_ports int,
	@classid int,
	@environmentid int,
	@addressid int,
	@roomid int,
	@rackid int,
	@rackposition varchar(10)
AS
UPDATE
	cva_network
SET
	depotid = @depotid,
	depotroomid = @depotroomid,
	shelfid = @shelfid,
	available_ports = @available_ports,
	classid = @classid,
	environmentid = @environmentid,
	addressid = @addressid,
	roomid = @roomid,
	rackid = @rackid,
	rackposition = @rackposition,
	modified = getdate()
WHERE
	assetid = @assetid

















GO
/****** Object:  StoredProcedure [dbo].[pr_updateOrder]    Script Date: 07/31/2009 13:24:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE procedure [dbo].[pr_updateOrder]
	@id int,
	@tracking varchar(50),
	@name varchar(50),
	@quantity int,
	@modelid int,
	@classid int,
	@environmentid int,
	@addressid int,
	@confidenceid int,
	@ordered datetime,
	@status int,
	@comments varchar(max)
AS
UPDATE
	cva_orders
SET
	tracking = @tracking,
	name = @name,
	quantity = @quantity,
	modelid = @modelid,
	classid = @classid,
	environmentid = @environmentid,
	addressid = @addressid,
	confidenceid = @confidenceid,
	ordered = @ordered,
	status = @status,
	comments = @comments,
	modified = getdate()
WHERE
	id = @id










GO
/****** Object:  StoredProcedure [dbo].[pr_updateOrderReceived]    Script Date: 07/31/2009 13:24:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




create procedure [dbo].[pr_updateOrderReceived]
	@id int,
	@received int
AS
UPDATE
	cva_orders
SET
	received = @received
WHERE
	id = @id








GO
/****** Object:  StoredProcedure [dbo].[pr_updateOrderShow]    Script Date: 07/31/2009 13:24:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[pr_updateOrderShow]
	@id int
AS
UPDATE
	cva_orders
SET
	show = 0
WHERE
	id = @id







GO
/****** Object:  StoredProcedure [dbo].[pr_updateServer]    Script Date: 07/31/2009 13:24:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE PROCEDURE [dbo].[pr_updateServer]
	@assetid int,
	@classid int,
	@environmentid int,
	@addressid int,
	@roomid int,
	@rackid int,
	@rackposition varchar(10),
	@ilo varchar(15),
	@dummy_name varchar(50),
	@macaddress varchar(50),
	@vlan int
AS
UPDATE
	cva_server
SET
	classid = @classid,
	environmentid = @environmentid,
	addressid = @addressid,
	roomid = @roomid,
	rackid = @rackid,
	rackposition = @rackposition,
	ilo = @ilo,
	dummy_name = @dummy_name,
	macaddress = @macaddress,
	vlan = @vlan,
	modified = getdate()
WHERE
	assetid = @assetid

















GO
/****** Object:  StoredProcedure [dbo].[pr_updateStatus]    Script Date: 07/31/2009 13:24:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









create PROCEDURE [dbo].[pr_updateStatus]
	@assetid int,
	@name varchar(100),
	@status int,
	@userid int,
	@datestamp datetime
AS
UPDATE
	cva_status
SET
	deleted = 1
WHERE
	assetid = @assetid
INSERT INTO
	cva_status
VALUES
(
	@assetid,
	@name,
	@status,
	@userid,
	@datestamp,
	0
)















GO
/****** Object:  StoredProcedure [dbo].[pr_updateStatusName]    Script Date: 07/31/2009 13:24:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










create PROCEDURE [dbo].[pr_updateStatusName]
	@assetid int,
	@name varchar(100),
	@userid int,
	@datestamp datetime
AS

DECLARE @status INT
SET @status = (SELECT TOP 1 status FROM cva_status WHERE assetid = @assetid AND deleted = 0 ORDER BY datestamp DESC)

UPDATE
	cva_status
SET
	deleted = 1
WHERE
	assetid = @assetid
INSERT INTO
	cva_status
VALUES
(
	@assetid,
	@name,
	@status,
	@userid,
	@datestamp,
	0
)
















GO
/****** Object:  StoredProcedure [dbo].[pr_updateVSG]    Script Date: 07/31/2009 13:24:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[pr_updateVSG]
	@name varchar(50),
	@assignedto varchar(50)
AS
UPDATE
	cva_vsg
SET
	assignedon = getdate(),
	assignedto = @assignedto
WHERE
	name = @name
	and assignedon is null





GO
/****** Object:  StoredProcedure [dbo].[rpt_EnclosureInventoryDetail]    Script Date: 07/31/2009 13:24:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:	Kevin Frazier
-- Create date: 09/05/2008
-- Description:	Returns data for the Enclosure Inventory Detail report.
-- =============================================
CREATE PROCEDURE          [dbo].[rpt_EnclosureInventoryDetail] 
	@Enclosure varchar(4000),
	@Class varchar(4000),
	@Status varchar(4000),
	@Location int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE
		@mySQL As nvarchar(4000),
		@WhereClause As varchar(4000),
		@OrderByClause As varchar(4000)
			
		SET @WhereClause = '
		WHERE
			ve.EnclosureID IN ( ''' + Replace(@Enclosure, ',', ''',''') + ''' )
			And ve.ClassID IN ( ''' + Replace(@Class, ',', ''',''') + ''' )
			And ve.StatusText IN ( ''' + Replace(@Status, ',', ''',''') + ''' )			
			And ve.AddressID IN ( ''' + Replace(@Location, ',', ''',''') + ''' ) '
		
		SET @OrderByClause = '
		ORDER BY
			ve.EnclosureID ASC,
			ve.BladeSlotNumber ASC'
			
		SET @mySQL = N'
		SELECT
			*
		FROM
			ClearViewAsset.dbo.vw_EnclosureInventoryRevised ve'
		
			
--		PRINT ( @mySQL + @WhereClause + @OrderByClause )
		
		SET @mySQL = ( @mySQL + @WhereClause + @OrderByClause )
		EXECUTE sp_executesql @mySQL
END


GO
/****** Object:  StoredProcedure [dbo].[rpt_OnDemandServersInStock]    Script Date: 07/31/2009 13:24:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:	Kevin Frazier
-- Create date: 2008-09-30
-- Description:	Produces dataset for the On-Demand Servers In Stock report.
-- =============================================
CREATE PROCEDURE  [dbo].[rpt_OnDemandServersInStock] 
	@StatusID varchar(1000),
	@ServerType varchar(1000),
	@Location varchar(4000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE
		@mySQL As nvarchar(4000)

	SET @mySQL = N'
	SELECT
		vod.ModelName As Model,
		vss.StockLocation,
		vss.SerialNumber,
		vss.AssetTag,
		vod.RoomName,
		vod.RackName,
		vod.AcquisitionCost As Value,
		vss.ComputerType,
		vod.StatusID,
		sl.name As Assignment,
		Space(50) As TestLoaner,
		vss.ProjectID,
		vss.ProjectNumber,
		vss.ServerID,
		vss.AnswerID,
		vod.AssetID,
		CASE
			WHEN vss.ComputerType = ''Server'' THEN ''Distributed''
			ELSE vss.ComputerType
		END AS ''Platform''
	FROM
		ClearViewAsset.dbo.vw_OnDemandServers vod
			INNER JOIN ClearView.dbo.vw_ServerStatus vss
				ON vod.AssetID = vss.AssetID
			LEFT JOIN ClearViewAsset.dbo.cva_status_list sl
				ON vod.StatusID = sl.id
				And sl.deleted = 0
	WHERE
		sl.id IN ( ''' + Replace(@StatusID, ',', ''',''') + ''' )
		And vss.ComputerType IN ( ''' + Replace(@ServerType, ',', ''',''') + ''')
		And vss.StockLocation IN ( ''' + Replace(@Location, ',', ''',''') + ''')'
		
--	PRINT @mySQL
	EXECUTE sp_executesql @mySQL
END





GO
/****** Object:  StoredProcedure [dbo].[xxx_vw_Assets]    Script Date: 07/31/2009 13:25:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[xxx_vw_Assets]
AS
select
	cv_models_property.name AS model,
	cv_types.name AS type,
	case 
		when cva_status.status = 0 then 'Arrived'
		when cva_status.status = 10 then 'In Use'
		when cva_status.status = 1 then 'In Stock'
		when cva_status.status = 100 then 'Reserved'
		when cva_status.status = -10 then 'Disposed'
	 end as status,
	cva_status.name AS name,
	cva_status.assetid AS assetid,
	cva_assets.bad,
	cva_assets.serial AS serial,
	cva_assets.asset AS asset,
	cva_blades.dummy_name,
	cva_blades.ilo,
	cv_rooms.name AS room,
	cv_racks.name AS rack,
	cva_enclosures.rackposition,
	cv_classs.name AS class,
	cv_environment.name AS environment,
	cv_location_address.id AS locationid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_operating_systems.name AS os,
	CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress,
	cv_ip_vlans.vlan,
	cv_domains.name AS domain,
	cv_domains_test.name AS domainTEST,
	cv_forecast_answers.appcode,
	cv_forecast_answers.appname,
	cv_forecast_answers.[backup] AS tsm,
	cv_forecast_answers_backup.start_date AS tsm_start_date,
	cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time,
	cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner,
	cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary,
	cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary,
	CASE
		WHEN cv_models_property.fabric = 0 THEN 'Cisco'
		WHEN cv_models_property.fabric = 1 THEN 'Brocade'
		ELSE 'Unknown'
	END AS fabric
FROM
	cva_blades
		INNER JOIN
			cva_status
				LEFT OUTER JOIN
					clearview.dbo.cv_servers_assets
						INNER JOIN
							clearview.dbo.cv_servers
								INNER JOIN
									clearview.dbo.cv_operating_systems
								ON
									cv_servers.osid = cv_operating_systems.id
									AND cv_operating_systems.deleted = 0
								INNER JOIN
									clearview.dbo.cv_domains
								ON
									cv_servers.domainid = cv_domains.id
									AND cv_domains.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_domains AS cv_domains_test
								ON
									cv_servers.test_domainid = cv_domains_test.id
									AND cv_domains_test.deleted = 0
								INNER JOIN
									clearview.dbo.cv_forecast_answers
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_owner
										ON
											cv_forecast_answers.appcontact = cv_users_owner.userid
											AND cv_users_owner.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_primary
										ON
											cv_forecast_answers.admin1 = cv_users_primary.userid
											AND cv_users_primary.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_secondary
										ON
											cv_forecast_answers.admin2 = cv_users_secondary.userid
											AND cv_users_secondary.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_forecast_answers_backup
										ON
											cv_forecast_answers.id = cv_forecast_answers_backup.answerid
											AND cv_forecast_answers_backup.deleted = 0
								ON
									cv_servers.answerid = cv_forecast_answers.id
									AND cv_forecast_answers.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_servers_ips
										INNER JOIN
											clearviewip.dbo.cv_ip_addresses
												INNER JOIN
													clearviewip.dbo.cv_ip_networks
														INNER JOIN
															clearviewip.dbo.cv_ip_vlans
														ON
															cv_ip_networks.vlanid = cv_ip_vlans.id
															AND cv_ip_vlans.deleted = 0
												ON
													cv_ip_addresses.networkid = cv_ip_networks.id
													AND cv_ip_networks.deleted = 0
										ON
											cv_servers_ips.ipaddressid = cv_ip_addresses.id
											AND cv_ip_addresses.deleted = 0
								ON
									cv_servers.id = cv_servers_ips.serverid
									AND cv_servers_ips.deleted = 0
						ON
							cv_servers_assets.serverid = cv_servers.id
							AND cv_servers.deleted = 0
				ON
					cva_status.assetid = cv_servers_assets.assetid
					AND cv_servers_assets.latest = 1
					AND cv_servers_assets.deleted = 0
		ON
			cva_blades.assetid = cva_status.assetid
			AND cva_status.deleted = 0
		inner join
			cva_enclosures
				LEFT OUTER JOIN
					clearview.dbo.cv_classs
				ON
					cva_enclosures.classid = cv_classs.id
					AND cv_classs.enabled = 1
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_environment
				ON
					cva_enclosures.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_location_address
						INNER JOIN
							clearview.dbo.cv_location_city
								INNER JOIN
									clearview.dbo.cv_location_state
								ON
									cv_location_city.stateid = cv_location_state.id
									AND cv_location_state.enabled = 1
									AND cv_location_state.deleted = 0
						ON
							cv_location_address.cityid = cv_location_city.id
							AND cv_location_city.enabled = 1
							AND cv_location_city.deleted = 0
				ON
					cva_enclosures.addressid = cv_location_address.id
					AND cv_location_address.enabled = 1
					AND cv_location_address.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_rooms
				ON
					cva_enclosures.roomid = cv_rooms.id
					AND cv_rooms.enabled = 1
					AND cv_rooms.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_racks
				ON
					cva_enclosures.rackid = cv_racks.id
					AND cv_racks.enabled = 1
					AND cv_racks.deleted = 0
		on
			cva_blades.enclosureid = cva_enclosures.assetid
			and cva_enclosures.deleted = 0
		INNER JOIN
			cva_assets
				INNER JOIN 
					clearview.dbo.cv_models_property 
						INNER JOIN 
							clearview.dbo.cv_models 
								INNER JOIN 
									clearview.dbo.cv_types 
										INNER JOIN 
											clearview.dbo.cv_platforms 
										ON 
											cv_types.platformid = cv_platforms.platformid 
											AND cv_platforms.enabled = 1 
											AND cv_platforms.deleted = 0 
								ON 
									cv_models.typeid = cv_types.id 
									AND cv_types.enabled = 1 
									AND cv_types.deleted = 0 
						ON 
							cv_models_property.modelid = cv_models.id 
							AND cv_models.enabled = 1 
							AND cv_models.deleted = 0 
				ON 
					cva_assets.modelid = cv_models_property.id 
					AND cv_models_property.deleted = 0 
		ON
			cva_blades.assetid = cva_assets.id
			AND cva_assets.deleted = 0
where
	cva_blades.deleted = 0
UNION ALL
select
	cv_models_property_1.name AS model,
	cv_types.name AS type,
	case 
		when cva_status_1.status = 0 then 'Arrived'
		when cva_status_1.status = 10 then 'In Use'
		when cva_status_1.status = 1 then 'In Stock'
		when cva_status_1.status = 100 then 'Reserved'
		when cva_status_1.status = -10 then 'Disposed'
	 end as status,
	cva_status_1.name AS name,
	cva_status_1.assetid AS assetid,
	cva_assets.bad,
	cva_assets.serial AS serial,
	cva_assets.asset AS asset,
	cva_server.dummy_name,
	cva_server.ilo,
	cv_rooms.name AS room,
	cv_racks.name AS rack,
	cva_server.rackposition,
	cv_classs.name AS class,
	cv_environment.name AS environment,
	cv_location_address.id AS locationid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_operating_systems.name AS os,
	CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress,
	cv_ip_vlans.vlan,
	cv_domains.name AS domain,
	cv_domains_test.name AS domainTEST,
	cv_forecast_answers.appcode,
	cv_forecast_answers.appname,
	cv_forecast_answers.[backup] AS tsm,
	cv_forecast_answers_backup.start_date AS tsm_start_date,
	cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time,
	cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner,
	cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary,
	cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary,
	CASE
		WHEN cv_models_property_1.fabric = 0 THEN 'Cisco'
		WHEN cv_models_property_1.fabric = 1 THEN 'Brocade'
		ELSE 'Unknown'
	END AS fabric
FROM
	cva_server
		INNER JOIN
			cva_status AS cva_status_1
				LEFT OUTER JOIN
					clearview.dbo.cv_servers_assets
						INNER JOIN
							clearview.dbo.cv_servers
								INNER JOIN
									clearview.dbo.cv_operating_systems
								ON
									cv_servers.osid = cv_operating_systems.id
									AND cv_operating_systems.deleted = 0
								INNER JOIN
									clearview.dbo.cv_domains
								ON
									cv_servers.domainid = cv_domains.id
									AND cv_domains.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_domains AS cv_domains_test
								ON
									cv_servers.test_domainid = cv_domains_test.id
									AND cv_domains_test.deleted = 0
								INNER JOIN
									clearview.dbo.cv_forecast_answers
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_owner
										ON
											cv_forecast_answers.appcontact = cv_users_owner.userid
											AND cv_users_owner.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_primary
										ON
											cv_forecast_answers.admin1 = cv_users_primary.userid
											AND cv_users_primary.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_secondary
										ON
											cv_forecast_answers.admin2 = cv_users_secondary.userid
											AND cv_users_secondary.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_forecast_answers_backup
										ON
											cv_forecast_answers.id = cv_forecast_answers_backup.answerid
											AND cv_forecast_answers_backup.deleted = 0
								ON
									cv_servers.answerid = cv_forecast_answers.id
									AND cv_forecast_answers.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_servers_ips
										INNER JOIN
											clearviewip.dbo.cv_ip_addresses
												INNER JOIN
													clearviewip.dbo.cv_ip_networks
														INNER JOIN
															clearviewip.dbo.cv_ip_vlans
														ON
															cv_ip_networks.vlanid = cv_ip_vlans.id
															AND cv_ip_vlans.deleted = 0
												ON
													cv_ip_addresses.networkid = cv_ip_networks.id
													AND cv_ip_networks.deleted = 0
										ON
											cv_servers_ips.ipaddressid = cv_ip_addresses.id
											AND cv_ip_addresses.deleted = 0
								ON
									cv_servers.id = cv_servers_ips.serverid
									AND cv_servers_ips.deleted = 0
						ON
							cv_servers_assets.serverid = cv_servers.id
							AND cv_servers.deleted = 0
				ON
					cva_status_1.assetid = cv_servers_assets.assetid
					AND cv_servers_assets.latest = 1
					AND cv_servers_assets.deleted = 0
		ON
			cva_server.assetid = cva_status_1.assetid
			AND cva_status_1.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_classs
		ON
			cva_server.classid = cv_classs.id
			AND cv_classs.enabled = 1
			AND cv_classs.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_environment
		ON
			cva_server.environmentid = cv_environment.id
			AND cv_environment.enabled = 1
			AND cv_environment.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_location_address
				INNER JOIN
					clearview.dbo.cv_location_city
						INNER JOIN
							clearview.dbo.cv_location_state
						ON
							cv_location_city.stateid = cv_location_state.id
							AND cv_location_state.enabled = 1
							AND cv_location_state.deleted = 0
				ON
					cv_location_address.cityid = cv_location_city.id
					AND cv_location_city.enabled = 1
					AND cv_location_city.deleted = 0
		ON
			cva_server.addressid = cv_location_address.id
			AND cv_location_address.enabled = 1
			AND cv_location_address.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_rooms
		ON
			cva_server.roomid = cv_rooms.id
			AND cv_rooms.enabled = 1
			AND cv_rooms.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_racks
		ON
			cva_server.rackid = cv_racks.id
			AND cv_racks.enabled = 1
			AND cv_racks.deleted = 0
		INNER JOIN
			cva_assets
				INNER JOIN 
					clearview.dbo.cv_models_property AS cv_models_property_1
						INNER JOIN 
							clearview.dbo.cv_models 
								INNER JOIN 
									clearview.dbo.cv_types 
										INNER JOIN 
											clearview.dbo.cv_platforms 
										ON 
											cv_types.platformid = cv_platforms.platformid 
											AND cv_platforms.enabled = 1 
											AND cv_platforms.deleted = 0 
								ON 
									cv_models.typeid = cv_types.id 
									AND cv_types.enabled = 1 
									AND cv_types.deleted = 0 
						ON 
							cv_models_property_1.modelid = cv_models.id 
							AND cv_models.enabled = 1 
							AND cv_models.deleted = 0 
				ON 
					cva_assets.modelid = cv_models_property_1.id 
					AND cv_models_property_1.deleted = 0 
		ON
			cva_server.assetid = cva_assets.id
			AND cva_assets.deleted = 0
where
	cva_server.deleted = 0
UNION ALL
select
	cv_models_property_2.name AS model,
	cv_types.name AS type,
	case 
		when cva_status_2.status = 0 then 'Arrived'
		when cva_status_2.status = 10 then 'In Use'
		when cva_status_2.status = 1 then 'In Stock'
		when cva_status_2.status = 100 then 'Reserved'
		when cva_status_2.status = -10 then 'Disposed'
	 end as status,
	cva_status_2.name AS name,
	cva_status_2.assetid AS assetid,
	cva_assets.bad,
	cva_assets.serial AS serial,
	cva_assets.asset AS asset,
	'' AS dummy_name,
	'' AS ilo,
	'' AS room,
	'' AS rack,
	'' AS rackposition,
	'' AS class,
	'' AS environment,
	0 AS locationid,
	'' AS location,
	'' AS os,
	'' AS ipaddress,
	0 AS vlan,
	'' AS domain,
	'' AS domainTEST,
	'' AS appcode,
	'' AS appname,
	0 AS tsm,
	null AS tsm_start_date,
	null AS tsm_start_time,
	'' AS app_owner,
	'' AS app_primary,
	'' AS app_secondary,
	CASE
		WHEN cv_models_property_2.fabric = 0 THEN 'Cisco'
		WHEN cv_models_property_2.fabric = 1 THEN 'Brocade'
		ELSE 'Unknown'
	END AS fabric
FROM
	cva_assets
		INNER JOIN
			cva_status AS cva_status_2
		ON
			cva_assets.id = cva_status_2.assetid
			AND cva_status_2.deleted = 0
			AND cva_status_2.status = 0
		INNER JOIN 
			clearview.dbo.cv_models_property AS cv_models_property_2
				INNER JOIN 
					clearview.dbo.cv_models 
						INNER JOIN 
							clearview.dbo.cv_types 
								INNER JOIN 
									clearview.dbo.cv_platforms 
								ON 
									cv_types.platformid = cv_platforms.platformid 
									AND cv_platforms.enabled = 1 
									AND cv_platforms.deleted = 0 
						ON 
							cv_models.typeid = cv_types.id 
							AND cv_types.enabled = 1 
							AND cv_types.deleted = 0 
				ON 
					cv_models_property_2.modelid = cv_models.id 
					AND cv_models.enabled = 1 
					AND cv_models.deleted = 0 
		ON 
			cva_assets.modelid = cv_models_property_2.id 
			AND cv_models_property_2.deleted = 0 
where
	cva_assets.deleted = 0





GO
/****** Object:  StoredProcedure [dbo].[xxx_vw_AssetsDR]    Script Date: 07/31/2009 13:25:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE procedure [dbo].[xxx_vw_AssetsDR]
AS
select
	cv_models_property.name AS model,
	cv_types.name AS type,
	case 
		when cva_status.status = 10 then 'In Use'
		when cva_status.status = 1 then 'In Stock'
		when cva_status.status = 100 then 'Reserved'
		when cva_status.status = -10 then 'Disposed'
	 end as status,
	cva_status.name AS name,
	cva_status.assetid AS assetid,
	cva_assets.bad,
	cva_assets.serial AS serial,
	cva_assets.asset AS asset,
	cva_blades.dummy_name,
	cva_blades.ilo,
	cv_rooms.name AS room,
	cv_racks.name AS rack,
	cva_enclosures.rackposition,
	cv_classs.name AS class,
	cv_environment.name AS environment,
	cv_location_address.id AS locationid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_operating_systems.name AS os,
	CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress,
	cv_ip_vlans.vlan,
	cv_domains.name AS domain,
	cv_domains_test.name AS domainTEST,
	cv_forecast_answers.appcode,
	cv_forecast_answers.appname,
	cv_forecast_answers.[backup] AS tsm,
	cv_forecast_answers_backup.start_date AS tsm_start_date,
	cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time,
	cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner,
	cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary,
	cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary,
	CASE
		WHEN cv_models_property.fabric = 0 THEN 'Cisco'
		WHEN cv_models_property.fabric = 1 THEN 'Brocade'
		ELSE 'Unknown'
	END AS fabric,
	cva_hba_1.name AS WWWN1,
	cva_hba_2.name AS WWWN2,
	cv_servers.fdrive AS FdriveOnSAN,
	cv_users_lead.fname + ' ' + cv_users_lead.lname AS manager,
	cv_users_engineer.fname + ' ' + cv_users_engineer.lname AS engineer,
	cv_luns.luns + cv_mount_points.luns AS luns,
	cv_lun_drives.drives AS drives,
	cv_lun_drive_letters.drives AS driveLetters
FROM
	clearview.dbo.cv_servers
		INNER JOIN
			clearview.dbo.cv_servers_assets
				INNER JOIN
					cva_status
						INNER JOIN
							cva_blades
								INNER JOIN
									cva_enclosures
										LEFT OUTER JOIN
											clearview.dbo.cv_classs
										ON
											cva_enclosures.classid = cv_classs.id
											AND cv_classs.enabled = 1
											AND cv_classs.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_environment
										ON
											cva_enclosures.environmentid = cv_environment.id
											AND cv_environment.enabled = 1
											AND cv_environment.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_location_address
												INNER JOIN
													clearview.dbo.cv_location_city
														INNER JOIN
															clearview.dbo.cv_location_state
														ON
															cv_location_city.stateid = cv_location_state.id
															AND cv_location_state.enabled = 1
															AND cv_location_state.deleted = 0
												ON
													cv_location_address.cityid = cv_location_city.id
													AND cv_location_city.enabled = 1
													AND cv_location_city.deleted = 0
										ON
											cva_enclosures.addressid = cv_location_address.id
											AND cv_location_address.enabled = 1
											AND cv_location_address.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_rooms
										ON
											cva_enclosures.roomid = cv_rooms.id
											AND cv_rooms.enabled = 1
											AND cv_rooms.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_racks
										ON
											cva_enclosures.rackid = cv_racks.id
											AND cv_racks.enabled = 1
											AND cv_racks.deleted = 0
								ON
									cva_blades.enclosureid = cva_enclosures.assetid
									and cva_enclosures.deleted = 0
								INNER JOIN
									cva_assets
										INNER JOIN 
											clearview.dbo.cv_models_property 
												INNER JOIN 
													clearview.dbo.cv_models 
														INNER JOIN 
															clearview.dbo.cv_types 
																INNER JOIN 
																	clearview.dbo.cv_platforms 
																ON 
																	cv_types.platformid = cv_platforms.platformid 
																	AND cv_platforms.enabled = 1 
																	AND cv_platforms.deleted = 0 
														ON 
															cv_models.typeid = cv_types.id 
															AND cv_types.enabled = 1 
															AND cv_types.deleted = 0 
												ON 
													cv_models_property.modelid = cv_models.id 
													AND cv_models.enabled = 1 
													AND cv_models.deleted = 0 
										ON 
											cva_assets.modelid = cv_models_property.id 
											AND cv_models_property.deleted = 0 
										OUTER APPLY 
											(SELECT TOP 1 * FROM cva_hba WHERE cva_hba.assetid = cva_assets.id ORDER BY modified ASC) AS cva_hba_1
										OUTER APPLY 
											(SELECT TOP 1 * FROM cva_hba WHERE cva_hba.assetid = cva_assets.id ORDER BY modified DESC) AS cva_hba_2
								ON
									cva_blades.assetid = cva_assets.id
									AND cva_assets.deleted = 0
						ON
							cva_blades.assetid = cva_status.assetid
							AND cva_status.deleted = 0
				ON
					cva_status.assetid = cv_servers_assets.assetid
					AND cva_status.status = 10
					AND cva_status.deleted = 0
		ON
			cv_servers_assets.serverid = cv_servers.id
			AND cv_servers_assets.dr = 1
			AND cv_servers_assets.deleted = 0
		INNER JOIN
			clearview.dbo.cv_operating_systems
		ON
			cv_servers.osid = cv_operating_systems.id
			AND cv_operating_systems.deleted = 0
		INNER JOIN
			clearview.dbo.cv_domains
		ON
			cv_servers.domainid = cv_domains.id
			AND cv_domains.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_domains AS cv_domains_test
		ON
			cv_servers.test_domainid = cv_domains_test.id
			AND cv_domains_test.deleted = 0
		INNER JOIN
			clearview.dbo.cv_forecast_answers
				INNER JOIN
					clearview.dbo.cv_forecast
						INNER JOIN
							clearview.dbo.cv_requests
								INNER JOIN
									clearview.dbo.cv_projects
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_engineer
										ON
											cv_projects.engineer = cv_users_engineer.userid
											AND cv_users_engineer.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_lead
										ON
											cv_projects.lead = cv_users_lead.userid
											AND cv_users_lead.deleted = 0
								ON
									cv_requests.projectid = cv_projects.projectid
									AND cv_projects.deleted = 0
						ON
							cv_forecast.requestid = cv_requests.requestid
							AND cv_requests.deleted = 0
				ON
					cv_forecast_answers.forecastid = cv_forecast.id
					AND cv_forecast.deleted = 0
				OUTER APPLY
					(
						SELECT clearview.dbo.getDriveLetters(cv_forecast_answers.id) AS drives
					) AS cv_lun_drive_letters
				OUTER APPLY 
					(
						SELECT DISTINCT
							COUNT(cv_storage_luns.driveid) AS drives 
						FROM 
							clearview.dbo.cv_storage_luns 
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_lun_drives
				OUTER APPLY 
					(
						SELECT 
							COUNT(cv_storage_luns.id) AS luns 
						FROM 
							clearview.dbo.cv_storage_luns 
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_luns
				OUTER APPLY 
					(
						SELECT 
							COUNT(cv_storage_mount_points.id) AS luns 
						FROM 
							clearview.dbo.cv_storage_luns 
								INNER JOIN
									clearview.dbo.cv_storage_mount_points
								ON
									cv_storage_luns.id = cv_storage_mount_points.lunid
									AND cv_storage_mount_points.deleted = 0
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_mount_points
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_owner
				ON
					cv_forecast_answers.appcontact = cv_users_owner.userid
					AND cv_users_owner.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_primary
				ON
					cv_forecast_answers.admin1 = cv_users_primary.userid
					AND cv_users_primary.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_secondary
				ON
					cv_forecast_answers.admin2 = cv_users_secondary.userid
					AND cv_users_secondary.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_forecast_answers_backup
				ON
					cv_forecast_answers.id = cv_forecast_answers_backup.answerid
					AND cv_forecast_answers_backup.deleted = 0
		ON
			cv_servers.answerid = cv_forecast_answers.id
			AND cv_forecast_answers.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_servers_ips
				INNER JOIN
					clearviewip.dbo.cv_ip_addresses
						INNER JOIN
							clearviewip.dbo.cv_ip_networks
								INNER JOIN
									clearviewip.dbo.cv_ip_vlans
								ON
									cv_ip_networks.vlanid = cv_ip_vlans.id
									AND cv_ip_vlans.deleted = 0
						ON
							cv_ip_addresses.networkid = cv_ip_networks.id
							AND cv_ip_networks.deleted = 0
				ON
					cv_servers_ips.ipaddressid = cv_ip_addresses.id
					AND cv_ip_addresses.deleted = 0
		ON
			cv_servers.id = cv_servers_ips.serverid
			AND cv_servers_ips.deleted = 0
where
	cv_servers.deleted = 0
UNION ALL
select
	cv_models_property.name AS model,
	cv_types.name AS type,
	case 
		when cva_status.status = 10 then 'In Use'
		when cva_status.status = 1 then 'In Stock'
		when cva_status.status = 100 then 'Reserved'
		when cva_status.status = -10 then 'Disposed'
	 end as status,
	cva_status.name AS name,
	cva_status.assetid AS assetid,
	cva_assets.bad,
	cva_assets.serial AS serial,
	cva_assets.asset AS asset,
	cva_server.dummy_name,
	cva_server.ilo,
	cv_rooms.name AS room,
	cv_racks.name AS rack,
	cva_server.rackposition,
	cv_classs.name AS class,
	cv_environment.name AS environment,
	cv_location_address.id AS locationid,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location,
	cv_operating_systems.name AS os,
	CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress,
	cv_ip_vlans.vlan,
	cv_domains.name AS domain,
	cv_domains_test.name AS domainTEST,
	cv_forecast_answers.appcode,
	cv_forecast_answers.appname,
	cv_forecast_answers.[backup] AS tsm,
	cv_forecast_answers_backup.start_date AS tsm_start_date,
	cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time,
	cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner,
	cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary,
	cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary,
	CASE
		WHEN cv_models_property.fabric = 0 THEN 'Cisco'
		WHEN cv_models_property.fabric = 1 THEN 'Brocade'
		ELSE 'Unknown'
	END AS fabric,
	cva_hba_1.name AS WWWN1,
	cva_hba_2.name AS WWWN2,
	cv_servers.fdrive AS FdriveOnSAN,
	cv_users_lead.fname + ' ' + cv_users_lead.lname AS manager,
	cv_users_engineer.fname + ' ' + cv_users_engineer.lname AS engineer,
	cv_luns.luns + cv_mount_points.luns AS luns,
	cv_lun_drives.drives AS drives,
	cv_lun_drive_letters.drives AS driveLetters
FROM
	clearview.dbo.cv_servers
		INNER JOIN
			clearview.dbo.cv_servers_assets
				INNER JOIN
					cva_status
						INNER JOIN
							cva_server
								LEFT OUTER JOIN
									clearview.dbo.cv_classs
								ON
									cva_server.classid = cv_classs.id
									AND cv_classs.enabled = 1
									AND cv_classs.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_environment
								ON
									cva_server.environmentid = cv_environment.id
									AND cv_environment.enabled = 1
									AND cv_environment.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_location_address
										INNER JOIN
											clearview.dbo.cv_location_city
												INNER JOIN
													clearview.dbo.cv_location_state
												ON
													cv_location_city.stateid = cv_location_state.id
													AND cv_location_state.enabled = 1
													AND cv_location_state.deleted = 0
										ON
											cv_location_address.cityid = cv_location_city.id
											AND cv_location_city.enabled = 1
											AND cv_location_city.deleted = 0
								ON
									cva_server.addressid = cv_location_address.id
									AND cv_location_address.enabled = 1
									AND cv_location_address.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_rooms
								ON
									cva_server.roomid = cv_rooms.id
									AND cv_rooms.enabled = 1
									AND cv_rooms.deleted = 0
								LEFT OUTER JOIN
									clearview.dbo.cv_racks
								ON
									cva_server.rackid = cv_racks.id
									AND cv_racks.enabled = 1
									AND cv_racks.deleted = 0
								INNER JOIN
									cva_assets
										INNER JOIN 
											clearview.dbo.cv_models_property 
												INNER JOIN 
													clearview.dbo.cv_models 
														INNER JOIN 
															clearview.dbo.cv_types 
																INNER JOIN 
																	clearview.dbo.cv_platforms 
																ON 
																	cv_types.platformid = cv_platforms.platformid 
																	AND cv_platforms.enabled = 1 
																	AND cv_platforms.deleted = 0 
														ON 
															cv_models.typeid = cv_types.id 
															AND cv_types.enabled = 1 
															AND cv_types.deleted = 0 
												ON 
													cv_models_property.modelid = cv_models.id 
													AND cv_models.enabled = 1 
													AND cv_models.deleted = 0 
										ON 
											cva_assets.modelid = cv_models_property.id 
											AND cv_models_property.deleted = 0 
								ON
									cva_server.assetid = cva_assets.id
									AND cva_assets.deleted = 0
								OUTER APPLY 
									(SELECT TOP 1 * FROM cva_hba WHERE cva_hba.assetid = cva_assets.id ORDER BY modified ASC) AS cva_hba_1
								OUTER APPLY 
									(SELECT TOP 1 * FROM cva_hba WHERE cva_hba.assetid = cva_assets.id ORDER BY modified DESC) AS cva_hba_2
						ON
							cva_server.assetid = cva_status.assetid
							AND cva_server.deleted = 0
				ON
					cva_status.assetid = cv_servers_assets.assetid
					AND cva_status.status = 10
					AND cva_status.deleted = 0
		ON
			cv_servers_assets.serverid = cv_servers.id
			AND cv_servers_assets.dr = 1
			AND cv_servers_assets.deleted = 0
		INNER JOIN
			clearview.dbo.cv_operating_systems
		ON
			cv_servers.osid = cv_operating_systems.id
			AND cv_operating_systems.deleted = 0
		INNER JOIN
			clearview.dbo.cv_domains
		ON
			cv_servers.domainid = cv_domains.id
			AND cv_domains.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_domains AS cv_domains_test
		ON
			cv_servers.test_domainid = cv_domains_test.id
			AND cv_domains_test.deleted = 0
		INNER JOIN
			clearview.dbo.cv_forecast_answers
				INNER JOIN
					clearview.dbo.cv_forecast
						INNER JOIN
							clearview.dbo.cv_requests
								INNER JOIN
									clearview.dbo.cv_projects
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_engineer
										ON
											cv_projects.engineer = cv_users_engineer.userid
											AND cv_users_engineer.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_lead
										ON
											cv_projects.lead = cv_users_lead.userid
											AND cv_users_lead.deleted = 0
								ON
									cv_requests.projectid = cv_projects.projectid
									AND cv_projects.deleted = 0
						ON
							cv_forecast.requestid = cv_requests.requestid
							AND cv_requests.deleted = 0
				ON
					cv_forecast_answers.forecastid = cv_forecast.id
					AND cv_forecast.deleted = 0
				OUTER APPLY
					(
						SELECT clearview.dbo.getDriveLetters(cv_forecast_answers.id) AS drives
					) AS cv_lun_drive_letters
				OUTER APPLY 
					(
						SELECT DISTINCT
							COUNT(cv_storage_luns.driveid) AS drives 
						FROM 
							clearview.dbo.cv_storage_luns 
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_lun_drives
				OUTER APPLY 
					(
						SELECT 
							COUNT(cv_storage_luns.id) AS luns 
						FROM 
							clearview.dbo.cv_storage_luns 
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_luns
				OUTER APPLY 
					(
						SELECT 
							COUNT(cv_storage_mount_points.id) AS luns 
						FROM 
							clearview.dbo.cv_storage_luns 
								INNER JOIN
									clearview.dbo.cv_storage_mount_points
								ON
									cv_storage_luns.id = cv_storage_mount_points.lunid
									AND cv_storage_mount_points.deleted = 0
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_mount_points
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_owner
				ON
					cv_forecast_answers.appcontact = cv_users_owner.userid
					AND cv_users_owner.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_primary
				ON
					cv_forecast_answers.admin1 = cv_users_primary.userid
					AND cv_users_primary.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_secondary
				ON
					cv_forecast_answers.admin2 = cv_users_secondary.userid
					AND cv_users_secondary.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_forecast_answers_backup
				ON
					cv_forecast_answers.id = cv_forecast_answers_backup.answerid
					AND cv_forecast_answers_backup.deleted = 0
		ON
			cv_servers.answerid = cv_forecast_answers.id
			AND cv_forecast_answers.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_servers_ips
				INNER JOIN
					clearviewip.dbo.cv_ip_addresses
						INNER JOIN
							clearviewip.dbo.cv_ip_networks
								INNER JOIN
									clearviewip.dbo.cv_ip_vlans
								ON
									cv_ip_networks.vlanid = cv_ip_vlans.id
									AND cv_ip_vlans.deleted = 0
						ON
							cv_ip_addresses.networkid = cv_ip_networks.id
							AND cv_ip_networks.deleted = 0
				ON
					cv_servers_ips.ipaddressid = cv_ip_addresses.id
					AND cv_ip_addresses.deleted = 0
		ON
			cv_servers.id = cv_servers_ips.serverid
			AND cv_servers_ips.deleted = 0
where
	cv_servers.deleted = 0
UNION ALL
select
	cv_models_property.name AS model,
	cv_types.name AS type,
	case 
		when cva_status.status = 10 then 'In Use'
		when cva_status.status = 1 then 'In Stock'
		when cva_status.status = 100 then 'Reserved'
		when cva_status.status = -10 then 'Disposed'
	 end as status,
	cva_status.name AS name,
	cva_status.assetid AS assetid,
	cva_assets.bad,
	cva_assets.serial AS serial,
	cva_assets.asset AS asset,
	'VMWARE' AS dummy_name,
	'VMWARE' AS ilo,
	'VMWARE' AS room,
	'VMWARE' AS rack,
	'VMWARE' AS rackposition,
	'Core' AS class,
	'Disaster Recovery' AS environment,
	0 AS locationid,
	'925 Dalton St (Cincinnati, OH)' AS location,
	cv_operating_systems.name AS os,
	CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress,
	cv_ip_vlans.vlan,
	cv_domains.name AS domain,
	cv_domains_test.name AS domainTEST,
	cv_forecast_answers.appcode,
	cv_forecast_answers.appname,
	cv_forecast_answers.[backup] AS tsm,
	cv_forecast_answers_backup.start_date AS tsm_start_date,
	cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time,
	cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner,
	cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary,
	cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary,
	CASE
		WHEN cv_models_property.fabric = 0 THEN 'Cisco'
		WHEN cv_models_property.fabric = 1 THEN 'Brocade'
		ELSE 'Unknown'
	END AS fabric,
	cva_hba_1.name AS WWWN1,
	cva_hba_2.name AS WWWN2,
	cv_servers.fdrive AS FdriveOnSAN,
	cv_users_lead.fname + ' ' + cv_users_lead.lname AS manager,
	cv_users_engineer.fname + ' ' + cv_users_engineer.lname AS engineer,
	cv_luns.luns + cv_mount_points.luns AS luns,
	cv_lun_drives.drives AS drives,
	cv_lun_drive_letters.drives AS driveLetters
FROM
	clearview.dbo.cv_servers
		INNER JOIN
			clearview.dbo.cv_servers_assets
				INNER JOIN
					cva_status
						INNER JOIN
							cva_assets
								INNER JOIN 
									clearview.dbo.cv_models_property 
										INNER JOIN 
											clearview.dbo.cv_models 
												INNER JOIN 
													clearview.dbo.cv_types 
														INNER JOIN 
															clearview.dbo.cv_platforms 
														ON 
															cv_types.platformid = cv_platforms.platformid 
															AND cv_platforms.enabled = 1 
															AND cv_platforms.deleted = 0 
												ON 
													cv_models.typeid = cv_types.id 
													AND cv_types.enabled = 1 
													AND cv_types.deleted = 0 
										ON 
											cv_models_property.modelid = cv_models.id 
											AND cv_models.enabled = 1 
											AND cv_models.deleted = 0 
								ON 
									cva_assets.modelid = cv_models_property.id 
									AND cv_models_property.deleted = 0 
						ON
							cva_status.assetid = cva_assets.id
							AND cva_assets.deleted = 0
							AND cva_assets.asset LIKE 'VSG%'
						OUTER APPLY 
							(SELECT TOP 1 * FROM cva_hba WHERE cva_hba.assetid = cva_assets.id ORDER BY modified ASC) AS cva_hba_1
						OUTER APPLY 
							(SELECT TOP 1 * FROM cva_hba WHERE cva_hba.assetid = cva_assets.id ORDER BY modified DESC) AS cva_hba_2
				ON
					cva_status.assetid = cv_servers_assets.assetid
					AND cva_status.status = 10
					AND cva_status.deleted = 0
		ON
			cv_servers_assets.serverid = cv_servers.id
			AND cv_servers_assets.dr = 1
			AND cv_servers_assets.deleted = 0
		INNER JOIN
			clearview.dbo.cv_operating_systems
		ON
			cv_servers.osid = cv_operating_systems.id
			AND cv_operating_systems.deleted = 0
		INNER JOIN
			clearview.dbo.cv_domains
		ON
			cv_servers.domainid = cv_domains.id
			AND cv_domains.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_domains AS cv_domains_test
		ON
			cv_servers.test_domainid = cv_domains_test.id
			AND cv_domains_test.deleted = 0
		INNER JOIN
			clearview.dbo.cv_forecast_answers
				INNER JOIN
					clearview.dbo.cv_forecast
						INNER JOIN
							clearview.dbo.cv_requests
								INNER JOIN
									clearview.dbo.cv_projects
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_engineer
										ON
											cv_projects.engineer = cv_users_engineer.userid
											AND cv_users_engineer.deleted = 0
										LEFT OUTER JOIN
											clearview.dbo.cv_users AS cv_users_lead
										ON
											cv_projects.lead = cv_users_lead.userid
											AND cv_users_lead.deleted = 0
								ON
									cv_requests.projectid = cv_projects.projectid
									AND cv_projects.deleted = 0
						ON
							cv_forecast.requestid = cv_requests.requestid
							AND cv_requests.deleted = 0
				ON
					cv_forecast_answers.forecastid = cv_forecast.id
					AND cv_forecast.deleted = 0
				OUTER APPLY
					(
						SELECT clearview.dbo.getDriveLetters(cv_forecast_answers.id) AS drives
					) AS cv_lun_drive_letters
				OUTER APPLY 
					(
						SELECT DISTINCT
							COUNT(cv_storage_luns.driveid) AS drives 
						FROM 
							clearview.dbo.cv_storage_luns 
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_lun_drives
				OUTER APPLY 
					(
						SELECT 
							COUNT(cv_storage_luns.id) AS luns 
						FROM 
							clearview.dbo.cv_storage_luns 
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_luns
				OUTER APPLY 
					(
						SELECT 
							COUNT(cv_storage_mount_points.id) AS luns 
						FROM 
							clearview.dbo.cv_storage_luns 
								INNER JOIN
									clearview.dbo.cv_storage_mount_points
								ON
									cv_storage_luns.id = cv_storage_mount_points.lunid
									AND cv_storage_mount_points.deleted = 0
						WHERE 
							cv_storage_luns.answerid = cv_forecast_answers.id 
							AND cv_storage_luns.deleted = 0
					) AS cv_mount_points
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_owner
				ON
					cv_forecast_answers.appcontact = cv_users_owner.userid
					AND cv_users_owner.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_primary
				ON
					cv_forecast_answers.admin1 = cv_users_primary.userid
					AND cv_users_primary.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_users AS cv_users_secondary
				ON
					cv_forecast_answers.admin2 = cv_users_secondary.userid
					AND cv_users_secondary.deleted = 0
				LEFT OUTER JOIN
					clearview.dbo.cv_forecast_answers_backup
				ON
					cv_forecast_answers.id = cv_forecast_answers_backup.answerid
					AND cv_forecast_answers_backup.deleted = 0
		ON
			cv_servers.answerid = cv_forecast_answers.id
			AND cv_forecast_answers.deleted = 0
		LEFT OUTER JOIN
			clearview.dbo.cv_servers_ips
				INNER JOIN
					clearviewip.dbo.cv_ip_addresses
						INNER JOIN
							clearviewip.dbo.cv_ip_networks
								INNER JOIN
									clearviewip.dbo.cv_ip_vlans
								ON
									cv_ip_networks.vlanid = cv_ip_vlans.id
									AND cv_ip_vlans.deleted = 0
						ON
							cv_ip_addresses.networkid = cv_ip_networks.id
							AND cv_ip_networks.deleted = 0
				ON
					cv_servers_ips.ipaddressid = cv_ip_addresses.id
					AND cv_ip_addresses.deleted = 0
		ON
			cv_servers.id = cv_servers_ips.serverid
			AND cv_servers_ips.deleted = 0
where
	cv_servers.deleted = 0




GO
/****** Object:  StoredProcedure [dbo].[xxx_vw_ClearView_Asset]    Script Date: 07/31/2009 13:25:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[xxx_vw_ClearView_Asset]
AS
SELECT
	dbo.cva_assets.id, 
	dbo.cva_assets.modelid, 
	dbo.cva_assets.serial, 
	dbo.cva_assets.asset, 
	dbo.cva_assets.bad, 
	dbo.cva_assets.validated, 
	dbo.cva_assets.created, 
	dbo.cva_assets.deleted, 
	ClearView.dbo.cv_models.make, 
	ClearView.dbo.cv_models.name AS model, 
	ClearView.dbo.cv_types.name AS type, 
	ClearView.dbo.cv_platforms.name AS platform, 
	ClearView.dbo.cv_depot.name AS stock, 
	ClearView.dbo.cv_depot_rooms.name AS stockroom, 
	ClearView.dbo.cv_shelfs.name AS shelf, 
	ClearView.dbo.cv_users.fname + ' ' + ClearView.dbo.cv_users.lname AS technician, 
	CAST(ClearViewIP.dbo.cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add3 AS varchar(3)) + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add4 AS varchar(3)) AS ipaddress, 
	ClearView.dbo.cv_location_address.name + ' (' + ClearView.dbo.cv_location_city.name + ', ' + ClearView.dbo.cv_location_state.name + ')' AS location, 
	ClearView.dbo.cv_racks.name AS rack, 
	ClearView.dbo.cv_environment.name AS environment, 
	ClearView.dbo.cv_classs.name AS class, 
	ClearView.dbo.cv_rooms.name AS room, 
	dbo.cva_status.datestamp AS commissionedon,
	cv_users.fname + ' ' + cv_users.lname AS commissionedby
FROM
	dbo.cva_assets 
		LEFT JOIN
			cva_network
				LEFT OUTER JOIN
					ClearView.dbo.cv_depot_rooms 
				ON 
					dbo.cva_network.depotroomid = ClearView.dbo.cv_depot_rooms.id 
					AND ClearView.dbo.cv_depot_rooms.enabled = 1 
					AND ClearView.dbo.cv_depot_rooms.deleted = 0 
				LEFT OUTER JOIN
					ClearView.dbo.cv_location_address 
						INNER JOIN
							ClearView.dbo.cv_location_city 
								INNER JOIN
									ClearView.dbo.cv_location_state 
								ON 
									ClearView.dbo.cv_location_city.stateid = ClearView.dbo.cv_location_state.id 
									AND ClearView.dbo.cv_location_state.enabled = 1 
									AND ClearView.dbo.cv_location_state.deleted = 0 
						ON 
							ClearView.dbo.cv_location_address.cityid = ClearView.dbo.cv_location_city.id 
							AND ClearView.dbo.cv_location_city.enabled = 1 
							AND ClearView.dbo.cv_location_city.deleted = 0 
				ON 
					dbo.cva_network.addressid = ClearView.dbo.cv_location_address.id 
					AND ClearView.dbo.cv_location_address.enabled = 1 
					AND ClearView.dbo.cv_location_address.deleted = 0 
				LEFT OUTER JOIN
					ClearView.dbo.cv_racks 
				ON 
					dbo.cva_network.rackid = ClearView.dbo.cv_racks.id 
					AND ClearView.dbo.cv_racks.enabled = 1 
					AND ClearView.dbo.cv_racks.deleted = 0 
				LEFT OUTER JOIN
					ClearView.dbo.cv_environment 
				ON 
					dbo.cva_network.environmentid = ClearView.dbo.cv_environment.id 
					AND ClearView.dbo.cv_environment.enabled = 1 
					AND ClearView.dbo.cv_environment.deleted = 0 
				LEFT OUTER JOIN
					ClearView.dbo.cv_classs 
				ON 
					dbo.cva_network.classid = ClearView.dbo.cv_classs.id 
					AND ClearView.dbo.cv_classs.enabled = 1 
					AND ClearView.dbo.cv_classs.deleted = 0 
				LEFT OUTER JOIN
					ClearView.dbo.cv_rooms 
				ON 
					dbo.cva_network.roomid = ClearView.dbo.cv_rooms.id 
					AND ClearView.dbo.cv_rooms.enabled = 1 
					AND ClearView.dbo.cv_rooms.deleted = 0 
				LEFT OUTER JOIN
					ClearView.dbo.cv_depot 
				ON 
					dbo.cva_network.depotid = ClearView.dbo.cv_depot.id 
					AND ClearView.dbo.cv_depot.enabled = 1 
					AND ClearView.dbo.cv_depot.deleted = 0 
				LEFT OUTER JOIN
					ClearView.dbo.cv_shelfs 
				ON 
					dbo.cva_network.shelfid = ClearView.dbo.cv_shelfs.id 
					AND ClearView.dbo.cv_shelfs.enabled = 1 
					AND ClearView.dbo.cv_shelfs.deleted = 0 
				LEFT OUTER JOIN
					cva_ips
						INNER JOIN
							ClearViewIP.dbo.cv_ip_addresses 
						ON 
							cva_ips.ipaddressid = ClearViewIP.dbo.cv_ip_addresses.id 
							AND ClearViewIP.dbo.cv_ip_addresses.deleted = 0 
				ON
					cva_ips.assetid = cva_network.assetid
					AND cva_ips.deleted = 0
		ON
			cva_assets.id = cva_network.assetid
			AND cva_network.deleted = 0
		INNER JOIN
			ClearView.dbo.cv_models 
				INNER JOIN
					ClearView.dbo.cv_types 
						INNER JOIN
							ClearView.dbo.cv_platforms 
						ON 
							ClearView.dbo.cv_types.platformid = ClearView.dbo.cv_platforms.platformid 
							AND ClearView.dbo.cv_platforms.enabled = 1 
							AND ClearView.dbo.cv_platforms.deleted = 0 
				ON 
					ClearView.dbo.cv_models.typeid = ClearView.dbo.cv_types.id 
					AND ClearView.dbo.cv_types.enabled = 1 
					AND ClearView.dbo.cv_types.deleted = 0 
		ON 
			dbo.cva_assets.modelid = ClearView.dbo.cv_models.id 
			AND ClearView.dbo.cv_models.enabled = 1 
			AND ClearView.dbo.cv_models.deleted = 0 
		INNER JOIN
			cva_status
				LEFT OUTER JOIN
					ClearView.dbo.cv_users 
				ON 
					cva_status.userid = ClearView.dbo.cv_users.userid 
					AND ClearView.dbo.cv_users.enabled = 1 
					AND ClearView.dbo.cv_users.deleted = 0
		ON
			cva_assets.id = cva_status.assetid
			AND cva_status.deleted = 0



