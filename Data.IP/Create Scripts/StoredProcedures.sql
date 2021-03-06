USE [ClearViewIP]
GO
/****** Object:  StoredProcedure [dbo].[pr_addAddress]    Script Date: 07/31/2009 13:32:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE PROCEDURE [dbo].[pr_addAddress]
	@networkid int,
	@add1 int,
	@add2 int,
	@add3 int,
	@add4 int,
	@dhcp int,
	@userid int,
	@id int output
AS
INSERT INTO
	cv_ip_addresses
VALUES
(
	@networkid,
	@add1,
	@add2,
	@add3,
	@add4,
	@dhcp,
	@userid,
	0,
	getdate(),
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()













GO
/****** Object:  StoredProcedure [dbo].[pr_addAddressDetail]    Script Date: 07/31/2009 13:32:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[pr_addAddressDetail]
	@ipaddressid int,
	@detailid int
AS
UPDATE
	cv_ip_addresses_detail
SET
	deleted = 1
WHERE
	ipaddressid = @ipaddressid
INSERT INTO
	cv_ip_addresses_detail
VALUES
(
	@ipaddressid,
	@detailid,
	getdate(),
	0
)










GO
/****** Object:  StoredProcedure [dbo].[pr_addAddressDetails]    Script Date: 07/31/2009 13:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[pr_addAddressDetails]
	@url varchar(100),
	@projectid int,
	@instance varchar(50),
	@vlan int,
	@serial varchar(50),
	@server_name varchar(100),
	@classid int,
	@environmentid int,
	@addressid int,
	@csm int,
	@type int,
	@id int output
AS
INSERT INTO
	cv_ip_addresses_details
VALUES
(
	@url,
	@projectid,
	@instance,
	@vlan,
	@serial,
	@server_name,
	@classid,
	@environmentid,
	@addressid,
	@csm,
	@type,
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()










GO
/****** Object:  StoredProcedure [dbo].[pr_addDhcp]    Script Date: 07/31/2009 13:32:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[pr_addDhcp]
	@networkid int,
	@min4 int,
	@max4 int,
	@ips_notify varchar(max),
	@ips_left int,
	@enabled int
AS
INSERT INTO
	cv_ip_dhcp
VALUES
(
	@networkid,
	@min4,
	@max4,
	@ips_notify,
	@ips_left,
	@enabled,
	getdate(),
	getdate(),
	0
)






GO
/****** Object:  StoredProcedure [dbo].[pr_addNetwork]    Script Date: 07/31/2009 13:32:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_addNetwork]
	@vlanid int,
	@add1 int,
	@add2 int,
	@add3 int,
	@min4 int,
	@max4 int,
	@mask varchar(15),
	@gateway varchar(15),
	@starting int,
	@maximum int,
	@reverse int,
	@routable int,
	@notify varchar(100),
	@enabled int
AS
INSERT INTO
	cv_ip_networks
VALUES
(
	@vlanid,
	@add1,
	@add2,
	@add3,
	@min4,
	@max4,
	@mask,
	@gateway,
	@starting,
	@maximum,
	@reverse,
	@routable,
	@notify,
	0,
	@enabled,
	getdate(),
	getdate(),
	0
)








GO
/****** Object:  StoredProcedure [dbo].[pr_addNetworkRelation]    Script Date: 07/31/2009 13:32:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






create PROCEDURE [dbo].[pr_addNetworkRelation]
	@networkid int,
	@related int
AS
INSERT INTO
	cv_ip_networks_relations
VALUES
(
	@networkid,
	@related,
	getdate(),
	0
)










GO
/****** Object:  StoredProcedure [dbo].[pr_addVlan]    Script Date: 07/31/2009 13:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[pr_addVlan]
	@vlan int,
	@physical_windows int,
	@physical_unix int,
	@ecom_production int,
	@ecom_service int,
	@ipx int,
	@virtual_workstation int,
	@vmware_workstation int,
	@vmware_host int,
	@vmware_vmotion int,
	@vmware_windows int,
	@vmware_linux int,
	@blades int,
	@apv int,
	@mainframe int,
	@csm int,
	@csm_soa int,
	@replicates int,
	@pxe int,
	@ilo int,
	@csm_vip int,
	@ltm_web int,
	@ltm_app int,
	@ltm_middle int,
	@ltm_vip int,
	@windows_cluster int,
	@unix_cluster int,
	@accenture int,
	@ha int,
	@sun_cluster int,
	@storage int,
	@switchname varchar(30),
	@vtpdomain varchar(30),
	@classid int,
	@environmentid int,
	@addressid int,
	@enabled int
AS
INSERT INTO
	cv_ip_vlans
VALUES
(
	@vlan,
	@physical_windows,
	@physical_unix,
	@ecom_production,
	@ecom_service,
	@ipx,
	@virtual_workstation,
	@vmware_workstation,
	@vmware_host,
	@vmware_vmotion,
	@vmware_windows,
	@vmware_linux,
	@blades,
	@apv,
	@mainframe,
	@csm,
	@csm_soa,
	@replicates,
	@pxe,
	@ilo,
	@csm_vip,
	@ltm_web,
	@ltm_app,
	@ltm_middle,
	@ltm_vip,
	@windows_cluster,
	@unix_cluster,
	@accenture,
	@ha,
	@sun_cluster,
	@storage,
	@switchname,
	@vtpdomain,
	@classid,
	@environmentid,
	@addressid,
	@enabled,
	getdate(),
	getdate(),
	0
)













GO
/****** Object:  StoredProcedure [dbo].[pr_addVlanHA]    Script Date: 07/31/2009 13:33:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











create PROCEDURE [dbo].[pr_addVlanHA]
	@original_vlan int,
	@ha_vlan int
AS
INSERT INTO
	cv_ip_vlans_ha
VALUES
(
	@original_vlan,
	@ha_vlan,
	getdate(),
	getdate(),
	0
)














GO
/****** Object:  StoredProcedure [dbo].[pr_deleteAddress]    Script Date: 07/31/2009 13:33:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_deleteAddress]
	@id int
AS
UPDATE
	cv_ip_addresses
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_deleteDhcp]    Script Date: 07/31/2009 13:33:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_deleteDhcp]
	@id int
AS
UPDATE
	cv_ip_dhcp
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_deleteNetwork]    Script Date: 07/31/2009 13:33:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_deleteNetwork]
	@id int
AS
UPDATE
	cv_ip_networks
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_deleteNetworkRelation]    Script Date: 07/31/2009 13:33:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









create PROCEDURE [dbo].[pr_deleteNetworkRelation]
	@networkid int
AS
UPDATE
	cv_ip_networks_relations
SET
	deleted = 1,
	modified = getdate()
WHERE
	networkid = @networkid
















GO
/****** Object:  StoredProcedure [dbo].[pr_deleteVlan]    Script Date: 07/31/2009 13:33:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_deleteVlan]
	@id int
AS
UPDATE
	cv_ip_Vlans
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_deleteVlanHA]    Script Date: 07/31/2009 13:33:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











create PROCEDURE [dbo].[pr_deleteVlanHA]
	@id int
AS
UPDATE
	cv_ip_vlans_ha
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id













GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress]    Script Date: 07/31/2009 13:33:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_getAddress]
	@id int
AS
SELECT
	*
FROM
	cv_ip_addresses
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_]    Script Date: 07/31/2009 13:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[pr_getAddress_]
	@classid int,
	@environmentid int,
	@addressid int,
	@physical_windows int,
	@physical_unix int,
	@ecom_production int,
	@ecom_service int,
	@ipx int,
	@virtual_workstation int,
	@vmware_workstation int,
	@vmware_host int,
	@vmware_vmotion int,
	@vmware_windows int,
	@vmware_linux int,
	@apv int,
	@csm int,
	@csm_soa int,
	@ilo int,
	@csm_vip int,
	@ltm_web int,
	@ltm_app int,
	@ltm_middle int,
	@ltm_vip int,
	@accenture int,
	@ha int,
	@sun_cluster int,
	@storage int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.physical_windows >= @physical_windows
			AND cv_ip_vlans.physical_unix >= @physical_unix
			AND cv_ip_vlans.ipx >= @ipx
			AND cv_ip_vlans.virtual_workstation >= @virtual_workstation
			AND cv_ip_vlans.vmware_workstation >= @vmware_workstation
			AND cv_ip_vlans.vmware_host >= @vmware_host
			AND cv_ip_vlans.vmware_vmotion >= @vmware_vmotion
			AND cv_ip_vlans.vmware_windows >= @vmware_windows
			AND cv_ip_vlans.vmware_linux >= @vmware_linux
			AND cv_ip_vlans.apv >= @apv
			AND cv_ip_vlans.csm >= @csm
			AND cv_ip_vlans.csm_soa >= @csm_soa
			AND cv_ip_vlans.ilo >= @ilo
			AND cv_ip_vlans.csm_vip >= @csm_vip
			AND cv_ip_vlans.ltm_web >= @ltm_web
			AND cv_ip_vlans.ltm_app >= @ltm_app
			AND cv_ip_vlans.ltm_middle >= @ltm_middle
			AND cv_ip_vlans.ltm_vip >= @ltm_vip
			AND cv_ip_vlans.accenture >= @accenture
			AND cv_ip_vlans.ha >= @ha
			AND cv_ip_vlans.sun_cluster >= @sun_cluster
			AND cv_ip_vlans.storage >= @storage
			AND cv_ip_vlans.windows_cluster >= 0
			AND cv_ip_vlans.unix_cluster >= 0
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
			AND cv_ip_vlans.ecom_production = @ecom_production
			AND cv_ip_vlans.ecom_service = @ecom_service
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
ORDER BY
	vlan













GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_Blade]    Script Date: 07/31/2009 13:33:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















CREATE PROCEDURE [dbo].[pr_getAddress_Blade]
	@classid int,
	@environmentid int,
	@addressid int,
	@ecom_production int,
	@ecom_service int,
	@vlan int,
	@networkid int,
	@ha int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.vlan = @vlan
			AND cv_ip_vlans.blades = 1
			AND cv_ip_vlans.windows_cluster >= 0
			AND cv_ip_vlans.unix_cluster >= 0
			AND cv_ip_vlans.ha = @ha
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
			AND cv_ip_vlans.ecom_production = @ecom_production
			AND cv_ip_vlans.ecom_service = @ecom_service
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
	AND @networkid = 0
	OR cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
	AND cv_ip_networks.id = @networkid
ORDER BY
	vlan













GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_Cluster]    Script Date: 07/31/2009 13:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_getAddress_Cluster]
	@classid int,
	@environmentid int,
	@addressid int,
	@ecom_production int,
	@ecom_service int,
	@windows_cluster int,
	@unix_cluster int,
	@networkid int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.windows_cluster >= @windows_cluster
			AND cv_ip_vlans.unix_cluster >= @unix_cluster
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
			AND cv_ip_vlans.ecom_production = @ecom_production
			AND cv_ip_vlans.ecom_service = @ecom_service
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 0
	AND cv_ip_networks.id = @networkid
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
ORDER BY
	vlan














GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_ClusterNetwork]    Script Date: 07/31/2009 13:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[pr_getAddress_ClusterNetwork]
	@classid int,
	@environmentid int,
	@addressid int,
	@ecom_production int,
	@ecom_service int,
	@windows_cluster int,
	@unix_cluster int,
	@not_vlan int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.windows_cluster >= @windows_cluster
			AND cv_ip_vlans.unix_cluster >= @unix_cluster
			AND cv_ip_vlans.vlan <> @not_vlan
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
			AND cv_ip_vlans.ecom_production = @ecom_production
			AND cv_ip_vlans.ecom_service = @ecom_service
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 0
--	AND cv_ip_networks.cluster_inuse = 0--
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
ORDER BY
	vlan


















GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_Network]    Script Date: 07/31/2009 13:33:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE PROCEDURE [dbo].[pr_getAddress_Network]
	@classid int,
	@environmentid int,
	@addressid int,
	@ecom_production int,
	@ecom_service int,
	@networkid int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
			AND cv_ip_vlans.ecom_production = @ecom_production
			AND cv_ip_vlans.ecom_service = @ecom_service
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id = @networkid
ORDER BY
	vlan






GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_Related]    Script Date: 07/31/2009 13:33:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE PROCEDURE [dbo].[pr_getAddress_Related]
	@classid int,
	@environmentid int,
	@addressid int,
	@related int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
	AND cv_ip_networks.id = @related
ORDER BY
	vlan







GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_VLAN]    Script Date: 07/31/2009 13:33:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















CREATE PROCEDURE [dbo].[pr_getAddress_VLAN]
	@classid int,
	@environmentid int,
	@addressid int,
	@ecom_production int,
	@ecom_service int,
	@vlan int,
	@networkid int,
	@csm int,
	@csm_soa int,
	@ltm_web int,
	@ltm_app int,
	@ltm_middle int,
	@ha int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.vlan = @vlan
			AND cv_ip_vlans.csm >= @csm
			AND cv_ip_vlans.csm_soa >= @csm_soa
			AND cv_ip_vlans.ltm_web >= @ltm_web
			AND cv_ip_vlans.ltm_app >= @ltm_app
			AND cv_ip_vlans.ltm_middle >= @ltm_middle
			AND cv_ip_vlans.ha = @ha
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
			AND cv_ip_vlans.ecom_production = @ecom_production
			AND cv_ip_vlans.ecom_service = @ecom_service
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
	AND @networkid = 0
	OR cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
	AND cv_ip_networks.id = @networkid
ORDER BY
	vlan













GO
/****** Object:  StoredProcedure [dbo].[pr_getAddress_VLANExclude]    Script Date: 07/31/2009 13:33:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















CREATE PROCEDURE [dbo].[pr_getAddress_VLANExclude]
	@classid int,
	@environmentid int,
	@addressid int,
	@ecom_production int,
	@ecom_service int,
	@vlan int,
	@networkid int,
	@csm int,
	@csm_soa int,
	@ltm_web int,
	@ltm_app int,
	@ltm_middle int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_networks.*
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		on
			cv_ip_vlans.id = cv_ip_networks.vlanid
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
			AND cv_ip_vlans.vlan = @vlan
			AND cv_ip_vlans.virtual_workstation = 0
			AND cv_ip_vlans.csm >= @csm
			AND cv_ip_vlans.csm_soa >= @csm_soa
			AND cv_ip_vlans.csm_vip = 0
			AND cv_ip_vlans.ltm_web >= @ltm_web
			AND cv_ip_vlans.ltm_app >= @ltm_app
			AND cv_ip_vlans.ltm_middle >= @ltm_middle
			AND cv_ip_vlans.ltm_vip = 0
			AND cv_ip_vlans.windows_cluster >= 0
			AND cv_ip_vlans.unix_cluster >= 0
			AND cv_ip_vlans.classid = @classid
			AND cv_ip_vlans.environmentid = @environmentid
			AND cv_ip_vlans.addressid = @addressid
			AND cv_ip_vlans.ecom_production = @ecom_production
			AND cv_ip_vlans.ecom_service = @ecom_service
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
	AND @networkid = 0
	OR cv_ip_networks.deleted = 0
	AND cv_ip_networks.enabled = 1
	AND cv_ip_networks.routable = 1
	AND cv_ip_networks.id NOT IN (SELECT related FROM cv_ip_networks_relations WHERE deleted = 0)
	AND cv_ip_networks.id = @networkid
ORDER BY
	vlan













GO
/****** Object:  StoredProcedure [dbo].[pr_getAddressDetail]    Script Date: 07/31/2009 13:33:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create PROCEDURE [dbo].[pr_getAddressDetail]
	@ipaddressid int
AS
SELECT
	cv_ip_addresses_details.*
FROM
	cv_ip_addresses
		INNER JOIN
			cv_ip_addresses_detail
				INNER JOIN
					cv_ip_addresses_details
						LEFT OUTER JOIN
							clearview.dbo.cv_projects
						ON
							cv_projects.projectid = cv_ip_addresses_details.projectid
							AND cv_projects.deleted = 0
				ON
					cv_ip_addresses_detail.detailid = cv_ip_addresses_details.id
					AND cv_ip_addresses_details.deleted = 0
		ON
			cv_ip_addresses.id = cv_ip_addresses_detail.ipaddressid
			AND cv_ip_addresses_detail.deleted = 0
WHERE
	cv_ip_addresses.id = @ipaddressid




GO
/****** Object:  StoredProcedure [dbo].[pr_getAddresses]    Script Date: 07/31/2009 13:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[pr_getAddresses]
	@networkid int,
	@available int
AS
SELECT
	*
FROM
	cv_ip_addresses
WHERE
	deleted = 0
	and networkid = @networkid
	and available >= @available






GO
/****** Object:  StoredProcedure [dbo].[pr_getAddressesMine]    Script Date: 07/31/2009 13:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[pr_getAddressesMine]
	@userid int
AS
SELECT
	cv_ip_addresses.id,
	cv_ip_vlans.vlan,
	cast(cv_ip_addresses.add1 AS varchar(3)) + '.' + cast(cv_ip_addresses.add2 AS varchar(3)) + '.' + cast(cv_ip_addresses.add3 AS varchar(3)) + '.' + cast(cv_ip_addresses.add4 AS varchar(3)) AS name,
	CASE
		WHEN cv_ip_addresses_details.url <> '' THEN 'CSM'
		WHEN cv_ip_addresses_details.instance <> '' THEN 'Cluster Instance'
		WHEN cv_ip_addresses_details.serial <> '' THEN 'ILO'
		WHEN cv_ip_addresses_details.server_name <> '' THEN 'Server Name'
	END AS type,
	CASE
		WHEN cv_ip_addresses_details.url <> '' THEN cv_ip_addresses_details.url
		WHEN cv_ip_addresses_details.instance <> '' THEN cv_ip_addresses_details.instance
		WHEN cv_ip_addresses_details.serial <> '' THEN cv_ip_addresses_details.serial
		WHEN cv_ip_addresses_details.server_name <> '' THEN cv_ip_addresses_details.server_name
	END AS custom,
	cv_ip_addresses.modified
FROM
	cv_ip_addresses
		INNER JOIN
			cv_ip_addresses_detail
				INNER JOIN
					cv_ip_addresses_details
						LEFT OUTER JOIN
							clearview.dbo.cv_projects
						ON
							cv_projects.projectid = cv_ip_addresses_details.projectid
							AND cv_projects.deleted = 0
				ON
					cv_ip_addresses_detail.detailid = cv_ip_addresses_details.id
					AND cv_ip_addresses_details.deleted = 0
		ON
			cv_ip_addresses.id = cv_ip_addresses_detail.ipaddressid
			AND cv_ip_addresses_detail.deleted = 0
		LEFT OUTER JOIN
			cv_ip_networks
				INNER JOIN
					cv_ip_vlans
				ON
					cv_ip_networks.vlanid = cv_ip_vlans.id
					AND cv_ip_vlans.deleted = 0
		ON
			cv_ip_addresses.networkid = cv_ip_networks.id
			AND cv_ip_networks.deleted = 0
WHERE
	cv_ip_addresses.deleted = 0
	and cv_ip_addresses.userid = @userid
	and cv_ip_addresses.available = 0
ORDER BY
	cv_ip_addresses.modified DESC









GO
/****** Object:  StoredProcedure [dbo].[pr_getAddressFull]    Script Date: 07/31/2009 13:33:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[pr_getAddressFull]
	@add1 int,
	@add2 int,
	@add3 int,
	@add4 int
AS
SELECT
	*
FROM
	cv_ip_addresses
WHERE
	deleted = 0
	AND add1 = @add1
	AND add2 = @add2
	AND add3 = @add3
	AND add4 = @add4






GO
/****** Object:  StoredProcedure [dbo].[pr_getAddressFullAvailable]    Script Date: 07/31/2009 13:33:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_getAddressFullAvailable]
	@add1 int,
	@add2 int,
	@add3 int,
	@add4 int,
	@available int
AS
SELECT
	*
FROM
	cv_ip_addresses
WHERE
	deleted = 0
	AND add1 = @add1
	AND add2 = @add2
	AND add3 = @add3
	AND add4 = @add4
	AND available >= @available







GO
/****** Object:  StoredProcedure [dbo].[pr_getDhcp]    Script Date: 07/31/2009 13:33:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[pr_getDhcp]
	@id int
AS
SELECT
	cv_ip_networks.add1,
	cv_ip_networks.add2,
	cv_ip_networks.add3,
	cv_ip_dhcp.*
FROM
	cv_ip_dhcp
		LEFT OUTER JOIN
			cv_ip_networks
		ON
			cv_ip_dhcp.networkid = cv_ip_networks.id
WHERE
	cv_ip_dhcp.id = @id




GO
/****** Object:  StoredProcedure [dbo].[pr_getDhcps]    Script Date: 07/31/2009 13:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[pr_getDhcps]
	@networkid int,
	@enabled int
AS
SELECT
	cv_ip_networks.add1,
	cv_ip_networks.add2,
	cv_ip_networks.add3,
	cv_ip_dhcp.*
FROM
	cv_ip_dhcp
		LEFT OUTER JOIN
			cv_ip_networks
		ON
			cv_ip_dhcp.networkid = cv_ip_networks.id
WHERE
	cv_ip_dhcp.deleted = 0
	and cv_ip_dhcp.networkid = @networkid
	and cv_ip_dhcp.enabled >= @enabled





GO
/****** Object:  StoredProcedure [dbo].[pr_getNetwork]    Script Date: 07/31/2009 13:33:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_getNetwork]
	@id int
AS
SELECT
	*
FROM
	cv_ip_networks
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_getNetwork_]    Script Date: 07/31/2009 13:33:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[pr_getNetwork_]
	@classid int,
	@environmentid int,
	@addressid int,
	@physical_windows int,
	@physical_unix int,
	@ecom_production int,
	@ecom_service int,
	@ipx int,
	@virtual_workstation int,
	@vmware_workstation int,
	@vmware_host int,
	@vmware_vmotion int,
	@vmware_windows int,
	@vmware_linux int,
	@apv int,
	@csm int,
	@csm_soa int,
	@ilo int,
	@csm_vip int,
	@ltm_web int,
	@ltm_app int,
	@ltm_middle int,
	@ltm_vip int,
	@accenture int,
	@ha int,
	@sun_cluster int,
	@storage int
AS
SELECT
	cv_ip_vlans.vlan,
	cv_ip_dhcp.networkid,
	cv_ip_dhcp.min4,
	cv_ip_dhcp.max4,
	cv_ip_dhcp.ips_notify,
	cv_ip_dhcp.ips_left,
	cv_ip_networks_used.total,
	cv_ip_networks.*
FROM
	cv_ip_dhcp
		INNER JOIN
			cv_ip_networks
				INNER JOIN
					cv_ip_vlans
				ON
					cv_ip_vlans.id = cv_ip_networks.vlanid
					AND cv_ip_vlans.enabled = 1
					AND cv_ip_vlans.deleted = 0
					AND cv_ip_vlans.physical_windows >= @physical_windows
					AND cv_ip_vlans.physical_unix >= @physical_unix
					AND cv_ip_vlans.ipx >= @ipx
					AND cv_ip_vlans.virtual_workstation >= @virtual_workstation
					AND cv_ip_vlans.vmware_workstation >= @vmware_workstation
					AND cv_ip_vlans.vmware_host >= @vmware_host
					AND cv_ip_vlans.vmware_vmotion >= @vmware_vmotion
					AND cv_ip_vlans.vmware_windows >= @vmware_windows
					AND cv_ip_vlans.vmware_linux >= @vmware_linux
					AND cv_ip_vlans.apv >= @apv
					AND cv_ip_vlans.csm >= @csm
					AND cv_ip_vlans.csm_soa >= @csm_soa
					AND cv_ip_vlans.ilo >= @ilo
					AND cv_ip_vlans.csm_vip >= @csm_vip
					AND cv_ip_vlans.ltm_web >= @ltm_web
					AND cv_ip_vlans.ltm_app >= @ltm_app
					AND cv_ip_vlans.ltm_middle >= @ltm_middle
					AND cv_ip_vlans.ltm_vip >= @ltm_vip
					AND cv_ip_vlans.accenture >= @accenture
					AND cv_ip_vlans.ha >= @ha
					AND cv_ip_vlans.sun_cluster >= @sun_cluster
					AND cv_ip_vlans.storage >= @storage
					AND cv_ip_vlans.windows_cluster = 0
					AND cv_ip_vlans.unix_cluster = 0
					AND cv_ip_vlans.classid = @classid
					AND cv_ip_vlans.environmentid = @environmentid
					AND cv_ip_vlans.addressid = @addressid
					AND cv_ip_vlans.ecom_production = @ecom_production
					AND cv_ip_vlans.ecom_service = @ecom_service
				OUTER APPLY
					(
						SELECT
							COUNT(tblWorkstation.networkid) AS total
						FROM
							clearview.dbo.cv_workstation_virtual AS tblWorkstation
								INNER JOIN
									cv_ip_networks AS tblNetwork
								ON
									tblWorkstation.networkid = tblNetwork.id
									AND tblNetwork.enabled = 1
									AND tblNetwork.deleted = 0
								INNER JOIN
									ClearViewAsset.dbo.cva_assets AS tblAsset
										INNER JOIN
											ClearViewAsset.dbo.cva_status AS tblStatus
										ON
											tblAsset.id = tblStatus.assetid
											AND tblStatus.deleted = 0
											AND tblStatus.status = 10
								ON
									tblWorkstation.assetid = tblAsset.id
									AND tblAsset.deleted = 0
						WHERE
							tblWorkstation.deleted = 0
							AND tblWorkstation.networkid = cv_ip_networks.id
					) AS cv_ip_networks_used
		ON
			cv_ip_dhcp.networkid = cv_ip_networks.id
			AND cv_ip_networks.enabled = 1
			AND cv_ip_networks.deleted = 0
WHERE
	cv_ip_dhcp.deleted = 0
	AND cv_ip_dhcp.enabled = 1
ORDER BY
	total DESC













GO
/****** Object:  StoredProcedure [dbo].[pr_getNetworkRelations]    Script Date: 07/31/2009 13:33:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










create PROCEDURE [dbo].[pr_getNetworkRelations]
	@networkid int
AS
SELECT
	cv_ip_networks.*
FROM
	cv_ip_networks_relations
		INNER JOIN
			cv_ip_networks
		ON
			cv_ip_networks_relations.related = cv_ip_networks.id
			AND cv_ip_networks.enabled = 1
			AND cv_ip_networks.deleted = 0
WHERE
	cv_ip_networks_relations.deleted = 0
	and cv_ip_networks_relations.networkid = @networkid











GO
/****** Object:  StoredProcedure [dbo].[pr_getNetworks]    Script Date: 07/31/2009 13:33:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[pr_getNetworks]
	@vlanid int,
	@enabled int
AS
SELECT
	*
FROM
	cv_ip_networks
WHERE
	deleted = 0
	and vlanid = @vlanid
	and enabled >= @enabled




GO
/****** Object:  StoredProcedure [dbo].[pr_getNetworksAddressList]    Script Date: 07/31/2009 13:33:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[pr_getNetworksAddressList]
	@vlanid int
AS
SELECT DISTINCT
	cv_ip_networks.id,
	cast(cv_ip_networks.add1 AS varchar(3)) + '.' + cast(cv_ip_networks.add2 AS varchar(3)) + '.' + cast(cv_ip_networks.add3 AS varchar(3)) + '.' + cast(cv_ip_networks.min4 AS varchar(3)) + ' - ' + cast(cv_ip_networks.add1 AS varchar(3)) + '.' + cast(cv_ip_networks.add2 AS varchar(3)) + '.' + cast(cv_ip_networks.add3 AS varchar(3)) + '.' + cast(cv_ip_networks.max4 AS varchar(3)) AS name
FROM
	cv_ip_networks
		INNER JOIN
			cv_ip_vlans
		ON
			cv_ip_networks.vlanid = cv_ip_vlans.id
			AND cv_ip_vlans.enabled = 1
			AND cv_ip_vlans.deleted = 0
WHERE
	cv_ip_networks.deleted = 0
	AND cv_ip_networks.vlanid = @vlanid
	and cv_ip_networks.enabled = 1
ORDER BY
	name







GO
/****** Object:  StoredProcedure [dbo].[pr_getVlan]    Script Date: 07/31/2009 13:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create PROCEDURE [dbo].[pr_getVlan]
	@vlan int,
	@classid int,
	@environmentid int,
	@addressid int
AS
SELECT
	*
FROM
	cv_ip_vlans
WHERE
	vlan = @vlan
	AND classid = @classid
	AND environmentid = @environmentid
	AND addressid = @addressid
	AND enabled = 1
	AND deleted = 0




GO
/****** Object:  StoredProcedure [dbo].[pr_getVlanHA]    Script Date: 07/31/2009 13:33:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











create PROCEDURE [dbo].[pr_getVlanHA]
	@id int
AS
SELECT
	*
FROM
	cv_ip_vlans_ha
WHERE
	id = @id
	and deleted = 0













GO
/****** Object:  StoredProcedure [dbo].[pr_getVlanHAs]    Script Date: 07/31/2009 13:33:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











create PROCEDURE [dbo].[pr_getVlanHAs]
	@original_vlan int
AS
SELECT
	*
FROM
	cv_ip_vlans_ha
WHERE
	original_vlan = @original_vlan
	and deleted = 0







GO
/****** Object:  StoredProcedure [dbo].[pr_getVlanHAsAll]    Script Date: 07/31/2009 13:33:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






create PROCEDURE [dbo].[pr_getVlanHAsAll]
AS
SELECT
	*
FROM
	cv_ip_vlans_ha
WHERE
	deleted = 0

GO
/****** Object:  StoredProcedure [dbo].[pr_getVlanId]    Script Date: 07/31/2009 13:33:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_getVlanId]
	@id int
AS
SELECT
	*
FROM
	cv_ip_Vlans
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_getVlans]    Script Date: 07/31/2009 13:33:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[pr_getVlans]
	@enabled int
AS
SELECT
	*
FROM
	cv_ip_vlans
WHERE
	deleted = 0
	and enabled >= @enabled




GO
/****** Object:  StoredProcedure [dbo].[pr_getVlansAddress]    Script Date: 07/31/2009 13:33:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create PROCEDURE [dbo].[pr_getVlansAddress]
	@classid int,
	@environmentid int,
	@addressid int,
	@enabled int
AS
SELECT DISTINCT
	*
FROM
	cv_ip_vlans
WHERE
	deleted = 0
	and classid = @classid
	and environmentid = @environmentid
	and addressid = @addressid
	and enabled >= @enabled




GO
/****** Object:  StoredProcedure [dbo].[pr_getVlansAddresses]    Script Date: 07/31/2009 13:33:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[pr_getVlansAddresses]
	@classid int,
	@environmentid int,
	@enabled int
AS
SELECT DISTINCT
	cv_location_address.id,
	cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS name
FROM
	cv_ip_vlans
		left outer join
			clearview.dbo.cv_location_address
				inner join
					clearview.dbo.cv_location_city
						inner join
							clearview.dbo.cv_location_state
						on
							cv_location_city.stateid = cv_location_state.id
							and cv_location_state.enabled = 1
							and cv_location_state.deleted = 0
				on
					cv_location_address.cityid = cv_location_city.id
					and cv_location_city.enabled = 1
					and cv_location_city.deleted = 0
		on
			cv_ip_vlans.addressid = cv_location_address.id
			and cv_location_address.enabled = 1
			and cv_location_address.deleted = 0
WHERE
	cv_ip_vlans.deleted = 0
	and cv_ip_vlans.classid = @classid
	and cv_ip_vlans.environmentid = @environmentid
	and cv_ip_vlans.enabled >= @enabled






GO
/****** Object:  StoredProcedure [dbo].[pr_getVlansAddressList]    Script Date: 07/31/2009 13:33:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[pr_getVlansAddressList]
	@classid int,
	@environmentid int,
	@addressid int,
	@blade int,
	@csm int,
	@ltm_web int,
	@ltm_app int,
	@ltm_middle int,
	@enabled int
AS
SELECT DISTINCT
	id, vlan
FROM
	cv_ip_vlans
WHERE
	deleted = 0
	and classid = @classid
	and environmentid = @environmentid
	and addressid = @addressid
	and blades >= @blade
	and csm >= @csm
	and ltm_web >= @ltm_web
	and ltm_app >= @ltm_app
	and ltm_middle >= @ltm_middle
	and enabled >= @enabled
ORDER BY
	vlan





GO
/****** Object:  StoredProcedure [dbo].[pr_getVlansClass]    Script Date: 07/31/2009 13:33:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_getVlansClass]
	@addressid int
AS
SELECT DISTINCT
	cv_classs.id,
	cv_classs.name
FROM
	cv_ip_vlans
		INNER JOIN
			clearview.dbo.cv_classs
		ON
			cv_ip_vlans.classid = cv_classs.id
			and cv_classs.enabled = 1
			and cv_classs.deleted = 0
WHERE
	cv_ip_vlans.deleted = 0
	and cv_ip_vlans.addressid = @addressid
	and cv_ip_vlans.enabled = 1



GO
/****** Object:  StoredProcedure [dbo].[pr_getVlansEnvironment]    Script Date: 07/31/2009 13:33:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[pr_getVlansEnvironment]
	@addressid int,
	@classid int
AS
SELECT DISTINCT
	cv_environment.id,
	cv_environment.name
FROM
	cv_ip_vlans
		INNER JOIN
			clearview.dbo.cv_class_environments
				INNER JOIN
					clearview.dbo.cv_environment
				ON
					cv_class_environments.environmentid = cv_environment.id
					AND cv_environment.enabled = 1
					AND cv_environment.deleted = 0
		ON
			cv_ip_vlans.classid = cv_class_environments.classid
			and cv_ip_vlans.environmentid = cv_class_environments.environmentid
			and cv_class_environments.deleted = 0
WHERE
	cv_ip_vlans.deleted = 0
	and cv_ip_vlans.addressid = @addressid
	and cv_ip_vlans.classid = @classid
	and cv_ip_vlans.enabled = 1


GO
/****** Object:  StoredProcedure [dbo].[pr_updateAddress]    Script Date: 07/31/2009 13:33:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[pr_updateAddress]
	@id int,
	@userid int
AS
UPDATE
	cv_ip_addresses
SET
	userid = @userid,
	modified = getdate()
WHERE
	id = @id








GO
/****** Object:  StoredProcedure [dbo].[pr_updateAddressAvailable]    Script Date: 07/31/2009 13:33:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[pr_updateAddressAvailable]
	@id int,
	@available int
AS
UPDATE
	cv_ip_addresses
SET
	available = @available,
	modified = getdate()
WHERE
	id = @id




GO
/****** Object:  StoredProcedure [dbo].[pr_updateAddressDHCP]    Script Date: 07/31/2009 13:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create PROCEDURE [dbo].[pr_updateAddressDHCP]
	@id int,
	@dhcp int
AS
UPDATE
	cv_ip_addresses
SET
	dhcp = @dhcp,
	modified = getdate()
WHERE
	id = @id





GO
/****** Object:  StoredProcedure [dbo].[pr_updateDhcp]    Script Date: 07/31/2009 13:33:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[pr_updateDhcp]
	@id int,
	@networkid int,
	@min4 int,
	@max4 int,
	@ips_notify varchar(max),
	@ips_left int,
	@enabled int
AS
UPDATE
	cv_ip_dhcp
SET
	networkid = @networkid,
	min4 = @min4,
	max4 = @max4,
	ips_notify = @ips_notify,
	ips_left = @ips_left,
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id






GO
/****** Object:  StoredProcedure [dbo].[pr_updateDhcpEnabled]    Script Date: 07/31/2009 13:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_updateDhcpEnabled]
	@id int,
	@enabled int
AS
UPDATE
	cv_ip_dhcp
SET
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_updateNetwork]    Script Date: 07/31/2009 13:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[pr_updateNetwork]
	@id int,
	@vlanid int,
	@add1 int,
	@add2 int,
	@add3 int,
	@min4 int,
	@max4 int,
	@mask varchar(15),
	@gateway varchar(15),
	@starting int,
	@maximum int,
	@reverse int,
	@routable int,
	@notify varchar(100),
	@enabled int
AS
UPDATE
	cv_ip_networks
SET
	vlanid = @vlanid,
	add1 = @add1,
	add2 = @add2,
	add3 = @add3,
	min4 = @min4,
	max4 = @max4,
	mask = @mask,
	gateway = @gateway,
	starting = @starting,
	maximum = @maximum,
	reverse = @reverse,
	routable = @routable,
	notify = @notify,
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id







GO
/****** Object:  StoredProcedure [dbo].[pr_updateNetworkCluster]    Script Date: 07/31/2009 13:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





create PROCEDURE [dbo].[pr_updateNetworkCluster]
	@id int,
	@cluster_inuse int
AS
UPDATE
	cv_ip_networks
SET
	cluster_inuse = @cluster_inuse,
	modified = getdate()
WHERE
	id = @id







GO
/****** Object:  StoredProcedure [dbo].[pr_updateNetworkEnabled]    Script Date: 07/31/2009 13:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_updateNetworkEnabled]
	@id int,
	@enabled int
AS
UPDATE
	cv_ip_networks
SET
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_updateVlan]    Script Date: 07/31/2009 13:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE PROCEDURE [dbo].[pr_updateVlan]
	@id int,
	@vlan int,
	@physical_windows int,
	@physical_unix int,
	@ecom_production int,
	@ecom_service int,
	@ipx int,
	@virtual_workstation int,
	@vmware_workstation int,
	@vmware_host int,
	@vmware_vmotion int,
	@vmware_windows int,
	@vmware_linux int,
	@blades int,
	@apv int,
	@mainframe int,
	@csm int,
	@csm_soa int,
	@replicates int,
	@pxe int,
	@ilo int,
	@csm_vip int,
	@ltm_web int,
	@ltm_app int,
	@ltm_middle int,
	@ltm_vip int,
	@windows_cluster int,
	@unix_cluster int,
	@accenture int,
	@ha int,
	@sun_cluster int,
	@storage int,
	@switchname varchar(30),
	@vtpdomain varchar(30),
	@classid int,
	@environmentid int,
	@addressid int,
	@enabled int
AS
UPDATE
	cv_ip_vlans
SET
	vlan = @vlan,
	physical_windows = @physical_windows,
	physical_unix = @physical_unix,
	ecom_production = @ecom_production,
	ecom_service = @ecom_service,
	ipx = @ipx,
	virtual_workstation = @virtual_workstation,
	vmware_workstation = @vmware_workstation,
	vmware_host = @vmware_host,
	vmware_vmotion = @vmware_vmotion,
	vmware_windows = @vmware_windows,
	vmware_linux = @vmware_linux,
	blades = @blades,
	apv = @apv,
	mainframe = @mainframe,
	csm = @csm,
	csm_soa = @csm_soa,
	replicates = @replicates,
	pxe = @pxe,
	ilo = @ilo,
	csm_vip = @csm_vip,
	ltm_web = @ltm_web,
	ltm_app = @ltm_app,
	ltm_middle = @ltm_middle,
	ltm_vip = @ltm_vip,
	windows_cluster = @windows_cluster,
	unix_cluster = @unix_cluster,
	accenture = @accenture,
	ha = @ha,
	sun_cluster = @sun_cluster,
	storage = @storage,
	switchname = @switchname,
	vtpdomain = @vtpdomain,
	classid = @classid,
	environmentid = @environmentid,
	addressid = @addressid,
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id













GO
/****** Object:  StoredProcedure [dbo].[pr_updateVlanEnabled]    Script Date: 07/31/2009 13:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_updateVlanEnabled]
	@id int,
	@enabled int
AS
UPDATE
	cv_ip_Vlans
SET
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[pr_updateVlanHA]    Script Date: 07/31/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











create PROCEDURE [dbo].[pr_updateVlanHA]
	@id int,
	@original_vlan int,
	@ha_vlan int
AS
UPDATE
	cv_ip_vlans_ha
SET
	original_vlan = @original_vlan,
	ha_vlan = @ha_vlan,
	modified = getdate()
WHERE
	id = @id












