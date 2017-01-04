/*==============================================================
The purpose of this script is to create procedures, functions and synonyms
==============================================================*/

ALTER PROCEDURE [dbo].[pr_addStorageLunDisk]
	@lunid int,
	@virtual_bus_number int,
	@virtual_unit_number int,
	@vmware_disk_uuid varchar(100)
AS
UPDATE
	cv_storage_luns_disks
SET
	deleted = 1,
	modified = GETDATE()
WHERE
	lunid = @lunid
INSERT INTO
	cv_storage_luns_disks
VALUES
(
	@lunid,
	@virtual_bus_number,
	@virtual_unit_number,
	@vmware_disk_uuid,
	getdate(),
	getdate(),
	0
)
GO

create PROCEDURE [dbo].[pr_getStorageLunsClusterSharedMapping]
	@answerid int
AS
-- EXEC pr_getStorageLunsClusterSharedMapping 26098
SELECT
	cv_storage_luns.id,
	cv_storage_luns.clusterid,
	cv_storage_luns.instanceid,
	cv_storage_luns.number,
	cv_storage_luns.driveid,
	cv_storage_luns.performance,
	cv_storage_luns.path,
	CASE
		WHEN size > 0 THEN size
		WHEN size_qa > 0 THEN size_qa
		WHEN size_test > 0 THEN size_test
	END AS size,
	cv_storage_luns_disks.vmware_disk_uuid AS uuid,
	CASE
		WHEN cv_storage_luns.driveid = -1 THEN 'Q'
		WHEN cv_storage_luns.driveid = -10 THEN 'P'
		WHEN cv_storage_luns.driveid = -100 THEN 'F'
		WHEN cv_storage_luns.driveid = -1000 THEN 'E'
		ELSE cv_storage_drive_letters.letter
	END AS letter
FROM
	cv_storage_luns
		LEFT OUTER JOIN
			cv_storage_drive_letters
		ON
			cv_storage_drive_letters.id = cv_storage_luns.driveid
			AND cv_storage_drive_letters.enabled = 1
			AND cv_storage_drive_letters.deleted = 0
		INNER JOIN
			cv_clusters
		ON
			cv_clusters.id = cv_storage_luns.clusterid
			AND cv_clusters.deleted = 0
		LEFT OUTER JOIN
			cv_storage_luns_disks
		ON
			cv_storage_luns.id = cv_storage_luns_disks.lunid
			AND cv_storage_luns_disks.deleted = 0
WHERE
	cv_storage_luns.answerid = @answerid
	AND cv_storage_luns.clusterid > 0
	AND (
		(cv_storage_luns.instanceid > 0)
		OR (cv_storage_luns.instanceid = 0 AND cv_storage_luns.driveid = -1)
		OR (cv_storage_luns.instanceid = 0 AND cv_storage_luns.driveid = -10)
	)
GO

CREATE PROCEDURE [dbo].[pr_addDesignApprovalConditional]
	@name varchar(100),
	@approve_by_field varchar(30),
	@approve_by_group int,
	@approve_by_requestor int,
	@approve_by_app_owner int,
	@approve_by_atl int,
	@approve_by_asm int,
	@approve_by_sd int,
	@approve_by_dm int,
	@approve_by_cio int,
	@display int,
	@enabled int,
	@id int output
AS
INSERT INTO
	cv_design_approval_conditional
VALUES
(
	@name,
	@approve_by_field,
	@approve_by_group,
	@approve_by_requestor,
	@approve_by_app_owner,
	@approve_by_atl,
	@approve_by_asm,
	@approve_by_sd,
	@approve_by_dm,
	@approve_by_cio,
	@display,
	@enabled,
	getdate(),
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()
GO

CREATE PROCEDURE [dbo].[pr_updateDesignApprovalConditional]
	@id int,
	@name varchar(100),
	@approve_by_field varchar(30),
	@approve_by_group int,
	@approve_by_requestor int,
	@approve_by_app_owner int,
	@approve_by_atl int,
	@approve_by_asm int,
	@approve_by_sd int,
	@approve_by_dm int,
	@approve_by_cio int,
	@enabled int
AS
UPDATE
	cv_design_approval_conditional
SET
	[name] = @name,
	[approve_by_field] = @approve_by_field,
	[approve_by_group] = @approve_by_group,
	[approve_by_requestor] = @approve_by_requestor,
	[approve_by_app_owner] = @approve_by_app_owner,
	[approve_by_atl] = @approve_by_atl,
	[approve_by_asm] = @approve_by_asm,
	[approve_by_sd] = @approve_by_sd,
	[approve_by_dm] = @approve_by_dm,
	[approve_by_cio] = @approve_by_cio,
	[enabled] = @enabled,
	[modified] = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_updateDesignApprovalConditionalOrder]
	@id int,
	@display int
AS
UPDATE
	cv_design_approval_conditional
SET
	display = @display
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_updateDesignApprovalConditionalEnabled]
	@id int,
	@enabled int
AS
UPDATE
	cv_design_approval_conditional
SET
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_deleteDesignApprovalConditional]
	@id int
AS
UPDATE
	cv_design_approval_conditional
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_getDesignApprovalConditional]
	@id int
AS
SELECT
	*
FROM
	cv_design_approval_conditional
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_getDesignApprovalConditionals]
	@enabled int
AS
SELECT
	*
FROM
	cv_design_approval_conditional
WHERE
	deleted = 0
	AND enabled >= @enabled
ORDER BY
	display
GO

CREATE PROCEDURE [dbo].[pr_addDesignApprovalConditionalSet]
	@approvalid int,
	@field varchar(30),
	@is_lt int,
	@is_lte int,
	@is_gt int,
	@is_gte int,
	@is_eq int,
	@is_neq int,
	@is_in int,
	@is_nin int,
	@is_ends int,
	@is_starts int,
	@dt_int int,
	@dt_date int,
	@value varchar(100),
	@or_group int,
	@id int output
AS
INSERT INTO
	cv_design_approval_conditional_sets
VALUES
(
	@approvalid,
	@field,
	@is_lt,
	@is_lte,
	@is_gt,
	@is_gte,
	@is_eq,
	@is_neq,
	@is_in,
	@is_nin,
	@is_ends,
	@is_starts,
	@dt_int,
	@dt_date,
	@value,
	@or_group,
	getdate(),
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()
GO

CREATE PROCEDURE [dbo].[pr_updateDesignApprovalConditionalSet]
	@id int,
	@approvalid int,
	@field varchar(30),
	@is_lt int,
	@is_lte int,
	@is_gt int,
	@is_gte int,
	@is_eq int,
	@is_neq int,
	@is_in int,
	@is_nin int,
	@is_ends int,
	@is_starts int,
	@dt_int int,
	@dt_date int,
	@value varchar(100),
	@or_group int
AS
UPDATE
	cv_design_approval_conditional_sets
SET
	[approvalid] = @approvalid,
	[field] = @field,
	[is_lt] = @is_lt,
	[is_lte] = @is_lte,
	[is_gt] = @is_gt,
	[is_gte] = @is_gte,
	[is_eq] = @is_eq,
	[is_neq] = @is_neq,
	[is_in] = @is_in,
	[is_nin] = @is_nin,
	[is_ends] = @is_ends,
	[is_starts] = @is_starts,
	[dt_int] = @dt_int,
	[dt_date] = @dt_date,
	[value] = @value,
	[or_group] = @or_group,
	[modified] = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_deleteDesignApprovalConditionalSet]
	@id int
AS
UPDATE
	cv_design_approval_conditional_sets
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_getDesignApprovalConditionalSet]
	@id int
AS
SELECT
	*
FROM
	cv_design_approval_conditional_sets
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_getDesignApprovalConditionalSets]
	@approvalid int
AS
SELECT
	*,
	CASE
		WHEN is_lt = 1 THEN 'Less Than'
		WHEN is_lte = 1 THEN 'Less Than or Equal To'
		WHEN is_gt = 1 THEN 'Greater Than'
		WHEN is_gte = 1 THEN 'Greater Than or Equal To'
		WHEN is_eq = 1 THEN 'Equal To'
		WHEN is_neq = 1 THEN 'NOT Equal To'
		WHEN is_in = 1 THEN 'Contains'
		WHEN is_nin = 1 THEN 'Does NOT Contain'
		WHEN is_ends = 1 THEN 'Ends With'
		WHEN is_starts = 1 THEN 'Starts With'
	END AS operator
FROM
	cv_design_approval_conditional_sets
WHERE
	deleted = 0
	AND approvalid = @approvalid
GO

CREATE PROCEDURE [dbo].[pr_addDesignApprovalConditionalWorkflow]
	@designid int,
	@approvalid int
AS
INSERT INTO
	cv_design_approval_conditional_workflow
VALUES
(
	@designid,
	@approvalid,
	0,
	null,
	null,
	getdate(),
	null,
	0
)
GO

CREATE PROCEDURE [dbo].[pr_updateDesignApprovalConditionalWorkflow]
	@id int,
	@userid int,
	@rejected int,
	@reason varchar(max)
AS
UPDATE
	cv_design_approval_conditional_workflow
SET
	userid = @userid,
	rejected = @rejected,
	reason = @reason,
	completed = GETDATE()
WHERE
	id = @id
GO

CREATE PROCEDURE [dbo].[pr_deleteDesignApprovalConditionalWorkflow]
	@designid int
AS
UPDATE
	cv_design_approval_conditional_workflow
SET
	deleted = 1
WHERE
	designid = @designid
	AND deleted = 0
GO

CREATE PROCEDURE [dbo].[pr_getDesignApprovalConditionalWorkflow]
	@designid int
AS
SELECT
	CASE
		WHEN cv_design_approval_conditional.approve_by_requestor = 1 THEN 'Requestor'
		WHEN cv_design_approval_conditional.approve_by_app_owner = 1 THEN 'Application Owner'
		WHEN cv_design_approval_conditional.approve_by_atl = 1 THEN 'Application Technical Lead'
		WHEN cv_design_approval_conditional.approve_by_asm = 1 THEN 'Application System Manager'
		WHEN cv_design_approval_conditional.approve_by_sd = 1 THEN 'System Director'
		WHEN cv_design_approval_conditional.approve_by_dm = 1 THEN 'Department Manager'
		WHEN cv_design_approval_conditional.approve_by_cio = 1 THEN 'CIO'
		WHEN cv_design_approval_conditional.approve_by_group > 0 THEN cv_design_approval_groups.name
		ELSE approve_by_field
	END AS approval,
	cv_design_approval_conditional.name,
	cv_design_approval_conditional_workflow.created,
	cv_design_approval_conditional_workflow.userid,
	cv_users.fname + ' ' + cv_users.lname + ' (' + cv_users.pnc_id + ')' AS username,
	cv_design_approval_conditional_workflow.rejected,
	cv_design_approval_conditional_workflow.reason,
	cv_design_approval_conditional_workflow.completed
FROM
	cv_design_approval_conditional
		LEFT OUTER JOIN
			cv_design_approval_groups
		ON
			cv_design_approval_conditional.approve_by_group = cv_design_approval_groups.id
			AND cv_design_approval_groups.enabled = 1
			AND cv_design_approval_groups.deleted = 0
		INNER JOIN
			cv_design_approval_conditional_workflow
				LEFT OUTER JOIN
					cv_users
				ON
					cv_design_approval_conditional_workflow.userid = cv_users.userid
					AND cv_users.deleted = 0
		ON
			cv_design_approval_conditional.id = cv_design_approval_conditional_workflow.approvalid
			AND cv_design_approval_conditional_workflow.designid = @designid
			AND cv_design_approval_conditional_workflow.deleted = 0
WHERE
	cv_design_approval_conditional.deleted = 0
GO

ALTER procedure [dbo].[pr_getPNCTaskStepsDesign]
	@answerid int,
	@current int,
	@serverid int
AS
-- EXEC pr_getPNCTaskStepsDesign 24048, 0, 0
-- NOTE: if the CREATED field is NULL then the service has not been inititated
-- NOTE: if the CREATED field is NOT NULL, but the completed date IS NULL, then the service has been inititated but is incomplete
-- GET ALL TASKS ASSOCIATED WITH THE DESIGN
DECLARE @demo INT
SET @demo = (SELECT
				cv_SetupMaster.SMId
					FROM
						cv_forecast_answers
							INNER JOIN
								cv_forecast
									INNER JOIN
										cv_requests
											INNER JOIN
												cv_projects
													LEFT OUTER JOIN
														cv_SetupMaster
													ON
														cv_projects.number = cv_SetupMaster.Value
														AND cv_SetupMaster.SMkey = 'DEMO_PROJECT'
														AND cv_SetupMaster.deleted = 0
											ON
												cv_requests.projectid = cv_projects.projectid
												AND cv_projects.deleted = 0
									ON
										cv_forecast.requestid = cv_requests.requestid
										AND cv_requests.deleted = 0
							ON
								cv_forecast_answers.forecastid = cv_forecast.id
								AND cv_forecast.deleted = 0
					WHERE
						cv_forecast_answers.id = @answerid)

--print @demo

SELECT DISTINCT
	cv_servers.answerid,
	cv_servername_components_details_selected.detailid
INTO
	#components
FROM
	cv_servername_components_details_selected
		INNER JOIN
			cv_servers
		ON
			cv_servername_components_details_selected.serverid = cv_servers.id
			AND cv_servers.deleted = 0
		INNER JOIN
			cv_servername_components_details
				INNER JOIN
					cv_servername_components
				ON
					cv_servername_components_details.componentid = cv_servername_components.id
					AND cv_servername_components.deleted = 0
		ON
			cv_servername_components_details_selected.detailid = cv_servername_components_details.id
			AND cv_servername_components_details.deleted = 0
WHERE
	cv_servername_components_details_selected.deleted = 0
	AND cv_servers.answerid = @answerid

--SELECT * FROM #components

SELECT DISTINCT
	cvOnDemandTasks.*,
	cv_classs.pnc,
	CASE WHEN
		cv_operating_systems.windows = 1 OR cv_operating_systems.windows2008 = 1 THEN 1
		ELSE 0
	END AS windows,
	CASE 
		WHEN cv_servers.dns_auto = 1 THEN cv_forecast_answers.completed
		ELSE NULL
	END AS dns_auto,
	CASE
		WHEN cv_forecast_responses.ha_csm = 1 THEN 1
		WHEN cv_designs.ha_load_balancing = 1 THEN 1
		ELSE 0
	END AS ha_csm,
	CASE
		WHEN cv_forecast_responses.ha_cluster = 1 THEN 1
		WHEN cv_designs.ha_clustering = 1 THEN 1
		ELSE 0
	END AS ha_cluster,
	CASE
		WHEN compCitrix.detailid IS NOT NULL OR compCitrix.detailid IS NOT NULL THEN 1
		ELSE 0
	END AS citrix,
	CASE
		WHEN cv_location_address.common = 1 THEN 0
		ELSE 1
	END AS offsite,
	cv_forecast_answers.[backup],
	cv_forecast_answers.avamar,
	cvBackupLocation.tsm,
	cv_servers.tsm_registered,
	cv_servers.tsm_output,
	cv_servers_avamar.created AS avamar_created,
	cv_servers_avamar.output AS avamar_output,
	cv_servers_avamar.started AS avamar_started,
	cv_servers_avamar.completed AS avamar_completed,
	(SELECT
		MAX(cvAvamar.error)
	FROM
	(
		SELECT MAX(cv_servers_avamar.error) AS error FROM cv_servers_avamar WHERE cv_servers_avamar.serverid IN (SELECT cv_servers.id FROM cv_servers WHERE cv_servers.answerid = cv_forecast_answers.id)
		UNION ALL
		SELECT MAX(cv_servers_avamar_activations.error) AS error FROM cv_servers_avamar_activations WHERE cv_servers_avamar_activations.serverid IN (SELECT cv_servers.id FROM cv_servers WHERE cv_servers.answerid = cv_forecast_answers.id)
		UNION ALL
		SELECT MAX(cv_servers_avamar_backups.error) AS error FROM cv_servers_avamar_backups WHERE cv_servers_avamar_backups.serverid IN (SELECT cv_servers.id FROM cv_servers WHERE cv_servers.answerid = cv_forecast_answers.id)
	) AS cvAvamar) AS avamar_error,
	--cv_servers_avamar.error AS avamar_error,
	CASE 
		WHEN cv_models_property.sun_virtual = 1 AND cv_sve_clusters.storage_allocated = 1 THEN cv_forecast_answers.completed
		ELSE NULL
	END AS sun_virtual_storage,
	cv_forecast_answers.storage,
	CASE 
		WHEN cv_models_property.type_vmware = 1 THEN cv_forecast_answers.completed
		ELSE NULL
	END AS type_vmware,
	CASE
		WHEN cv_models_property.vmware_virtual = 1 OR cv_models_property.ibm_virtual = 1 OR cv_models_property.sun_virtual = 1 THEN 1
		ELSE 0
	END AS virtual
INTO
	#cvTasksTemp
FROM
	(
		SELECT @answerid AS answerid, 0 AS serviceid, NULL AS created, NULL AS completed, 0 AS userid, '' AS comments, 0 AS status, 0 AS id
		UNION ALL
		SELECT answerid, cv_ondemand_tasks_server_other.serviceid, cv_ondemand_tasks_server_other.created, cv_ondemand_tasks_server_other.completed, ISNULL(cv_resource_requests_workflow.userid,0) AS userid, ISNULL(CAST(cv_resource_request_update.comments AS VARCHAR(MAX)),'') AS comments, ISNULL(cv_resource_request_update.status,0) AS status, ISNULL(cv_resource_requests.id,0) AS id
			FROM cv_ondemand_tasks_server_other 
				INNER JOIN
					cv_resource_requests
						LEFT OUTER JOIN
							cv_resource_requests_workflow
								LEFT OUTER JOIN
									cv_resource_request_update
								ON
									cv_resource_requests_workflow.id = cv_resource_request_update.parent
									AND cv_resource_request_update.latest = 1
									AND cv_resource_request_update.deleted = 0
						ON
							cv_resource_requests.id = cv_resource_requests_workflow.parent
							AND cv_resource_requests_workflow.deleted = 0
				ON
					cv_ondemand_tasks_server_other.requestid = cv_resource_requests.requestid
					AND cv_ondemand_tasks_server_other.serviceid = cv_resource_requests.serviceid
					AND cv_ondemand_tasks_server_other.number = cv_resource_requests.number
					AND cv_resource_requests.deleted = 0
			WHERE
				cv_ondemand_tasks_server_other.deleted = 0
				AND cv_ondemand_tasks_server_other.answerid = @answerid
		UNION ALL
		SELECT answerid, cv_resource_requests.serviceid, cv_resource_requests.created, cv_ondemand_tasks_server_storage.completed, ISNULL(cv_resource_requests_workflow.userid,0) AS userid, ISNULL(CAST(cv_resource_request_update.comments AS VARCHAR(MAX)),'') AS comments, ISNULL(cv_resource_request_update.status,0) AS status, ISNULL(cv_resource_requests.id,0) AS id
			FROM cv_ondemand_tasks_server_storage
				INNER JOIN
					cv_resource_requests
						LEFT OUTER JOIN
							cv_resource_requests_workflow
								LEFT OUTER JOIN
									cv_resource_request_update
								ON
									cv_resource_requests_workflow.id = cv_resource_request_update.parent
									AND cv_resource_request_update.latest = 1
									AND cv_resource_request_update.deleted = 0
						ON
							cv_resource_requests.id = cv_resource_requests_workflow.parent
							AND cv_resource_requests_workflow.deleted = 0
				ON
					cv_ondemand_tasks_server_storage.requestid = cv_resource_requests.requestid
					AND cv_ondemand_tasks_server_storage.itemid = cv_resource_requests.itemid
					AND cv_ondemand_tasks_server_storage.number = cv_resource_requests.number
					AND cv_resource_requests.deleted = 0
			WHERE
				cv_ondemand_tasks_server_storage.deleted = 0
				AND cv_ondemand_tasks_server_storage.answerid = @answerid
		UNION ALL
		SELECT answerid, cv_resource_requests.serviceid, cv_resource_requests.created, cv_ondemand_tasks_server_backup.completed, ISNULL(cv_resource_requests_workflow.userid,0) AS userid, ISNULL(CAST(cv_resource_request_update.comments AS VARCHAR(MAX)),'') AS comments, ISNULL(cv_resource_request_update.status,0) AS status, ISNULL(cv_resource_requests.id,0) AS id
			FROM cv_ondemand_tasks_server_backup
				INNER JOIN
					cv_resource_requests
						LEFT OUTER JOIN
							cv_resource_requests_workflow
								LEFT OUTER JOIN
									cv_resource_request_update
								ON
									cv_resource_requests_workflow.id = cv_resource_request_update.parent
									AND cv_resource_request_update.latest = 1
									AND cv_resource_request_update.deleted = 0
						ON
							cv_resource_requests.id = cv_resource_requests_workflow.parent
							AND cv_resource_requests_workflow.deleted = 0
				ON
					cv_ondemand_tasks_server_backup.requestid = cv_resource_requests.requestid
					AND cv_ondemand_tasks_server_backup.itemid = cv_resource_requests.itemid
					AND cv_ondemand_tasks_server_backup.number = cv_resource_requests.number
					AND cv_resource_requests.deleted = 0
			WHERE
				cv_ondemand_tasks_server_backup.deleted = 0
				AND cv_ondemand_tasks_server_backup.answerid = @answerid
	) AS cvOnDemandTasks
		INNER JOIN
			cv_forecast_answers
				LEFT OUTER JOIN
					cv_designs
				ON
					cv_forecast_answers.id = cv_designs.answerid
					AND cv_designs.deleted = 0
				LEFT OUTER JOIN
					cv_forecast_answers_platform
						INNER JOIN
							cv_forecast_responses
						ON
							cv_forecast_answers_platform.responseid = cv_forecast_responses.id
							AND cv_forecast_responses.deleted = 0
				ON
					cv_forecast_answers.id = cv_forecast_answers_platform.answerid
					AND cv_forecast_answers_platform.deleted = 0
				LEFT OUTER JOIN
					cv_forecast_answers_backup
				ON
					cv_forecast_answers.id = cv_forecast_answers_backup.answerid
					AND cv_forecast_answers_backup.deleted = 0
				INNER JOIN
					cv_location_address AS cvBackupLocation
				ON
					cv_forecast_answers.addressid = cvBackupLocation.id
					AND cvBackupLocation.deleted = 0
				LEFT OUTER JOIN
					#components AS compCitrix
						INNER JOIN
							cv_servername_components_details
								INNER JOIN
									cv_servername_components
								ON
									cv_servername_components_details.componentid = cv_servername_components.id
									AND cv_servername_components.code = 'CTX'
									AND cv_servername_components.deleted = 0
						ON
							compCitrix.detailid = cv_servername_components_details.id
							AND cv_servername_components_details.deleted = 0
				ON
					cv_forecast_answers.id = compCitrix.answerid
				INNER JOIN
					cv_classs
				ON
					cv_forecast_answers.classid = cv_classs.id
					AND cv_classs.deleted = 0
				INNER JOIN
					cv_location_address
				ON
					cv_forecast_answers.addressid = cv_location_address.id
					AND cv_location_address.deleted = 0
				INNER JOIN
					cv_servers
						LEFT OUTER JOIN
							cv_servers_avamar
						ON
							cv_servers.id = cv_servers_avamar.serverid
							AND cv_servers_avamar.latest = 1
							AND cv_servers_avamar.deleted = 0
						INNER JOIN
							cv_operating_systems
						ON
							cv_operating_systems.id = cv_servers.osid
							AND cv_operating_systems.deleted = 0
						INNER JOIN
							cv_models_property
						ON
							cv_models_property.id = cv_servers.modelid
							AND cv_models_property.deleted = 0
						LEFT OUTER JOIN
							cv_sve_guests
								INNER JOIN
									cv_sve_clusters
								ON
									cv_sve_guests.clusterid = cv_sve_clusters.id
									AND cv_sve_clusters.deleted = 0
						ON
							cv_servers.id = cv_sve_guests.serverid
							AND cv_sve_guests.deleted = 0
				ON
					cv_servers.answerid = cv_forecast_answers.id
					AND cv_servers.deleted = 0
		ON
			cvOnDemandTasks.answerid = cv_forecast_answers.id
			AND cv_forecast_answers.deleted = 0

--SELECT * FROM #cvTasksTemp

SELECT
	cvTasksTemp.*
INTO
	#cvTasks
FROM
	(
		SELECT
			@answerid AS answerid,
			0 AS id,
			serviceid,
			CASE
				WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid)
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid)
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid)
				WHEN (cv_pnc_tasks.tsm = 1 OR @demo IS NOT NULL) AND ((SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL) IS NOT NULL) THEN (SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL)
				WHEN (cv_pnc_tasks.legato = 1 OR @demo IS NOT NULL) AND cv_pnc_tasks.non_transparent = 0 AND ((SELECT TOP 1 avamar_output FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_output IS NOT NULL) IS NOT NULL) THEN (SELECT TOP 1 avamar_created FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_created IS NOT NULL)
				ELSE NULL
			END AS created,
			CASE
				WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid)
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid)
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid)
				WHEN (cv_pnc_tasks.tsm = 1 OR @demo IS NOT NULL) AND ((SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL) IS NOT NULL) AND ((SELECT TOP 1 tsm_output FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) AND ((SELECT TOP 1 tsm_output FROM #cvTasksTemp WHERE answerid = @answerid) <> 'PENDING') THEN (SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL)
				WHEN (cv_pnc_tasks.legato = 1 OR @demo IS NOT NULL) AND cv_pnc_tasks.non_transparent = 0 AND ((SELECT TOP 1 avamar_output FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_output IS NOT NULL) IS NOT NULL) AND ((SELECT TOP 1 avamar_started FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 avamar_completed FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_completed IS NOT NULL AND avamar_error <> 1)
				ELSE NULL
			END AS completed,
			NULL AS pnc,
			NULL AS windows,
			NULL AS dns_auto,
			NULL AS ha_csm,
			NULL AS ha_cluster,
			NULL AS citrix,
			(SELECT TOP 1 offsite FROM #cvTasksTemp WHERE answerid = @answerid) AS offsite,
			NULL AS [backup],
			CASE
				WHEN @demo IS NOT NULL THEN 0
				ELSE (SELECT TOP 1 avamar FROM #cvTasksTemp WHERE answerid = @answerid)
			END AS avamar,
			CASE
				WHEN @demo IS NOT NULL THEN 1
				ELSE (SELECT TOP 1 tsm FROM #cvTasksTemp WHERE answerid = @answerid)
			END AS tsm,
			NULL AS tsm_registered,
			NULL AS storage,
			CASE
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid)
				ELSE NULL
			END AS type_vmware,
			CASE
				WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN -999
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN -999
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN -999
				WHEN (cv_pnc_tasks.tsm = 1 OR @demo IS NOT NULL) AND ((SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL) IS NOT NULL) THEN -999
				WHEN (cv_pnc_tasks.legato = 1 OR @demo IS NOT NULL) AND cv_pnc_tasks.non_transparent = 0 AND ((SELECT TOP 1 avamar_output FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_output IS NOT NULL) IS NOT NULL) THEN -999
				ELSE 0
			END AS userid,
			CASE
				WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN 'DNS record successfully created'
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN 'Completed'
				WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN 'Completed'
				-- TSM output handled later on
				ELSE ''
			END AS comments,
			0 AS status,
			(SELECT TOP 1 virtual FROM #cvTasksTemp WHERE answerid = @answerid) AS virtual
		FROM
			cv_pnc_tasks
		WHERE
			cv_pnc_tasks.serviceid NOT IN (SELECT serviceid FROM #cvTasksTemp WHERE answerid = @answerid)
			AND cv_pnc_tasks.decom = 0
		UNION ALL
			SELECT
				answerid,
				id,
				serviceid,
				created,
				completed,
				pnc,
				windows,
				dns_auto,
				ha_csm,
				ha_cluster,
				citrix,
				offsite,
				[backup],
				CASE
					WHEN @demo IS NOT NULL THEN 0
					ELSE avamar
				END AS avamar,
				CASE
					WHEN @demo IS NOT NULL THEN 1
					ELSE tsm
				END AS tsm,
				tsm_registered,
				storage,
				type_vmware,
				userid,
				comments,
				status,
				virtual
			FROM
				#cvTasksTemp
	) AS cvTasksTemp

DROP TABLE #cvTasksTemp

--SELECT * FROM #cvTasks

-- GET ALL TASKS CURRENTLY BEING WORKED ON
SELECT DISTINCT
	cv_pnc_tasks.serviceid,
	cv_pnc_tasks.implementor,
	cv_pnc_tasks.network_engineer,
	cv_pnc_tasks.dba,
	cv_pnc_tasks.project_manager,
	cv_pnc_tasks.departmental_manager,
	cv_pnc_tasks.application_lead,
	cv_pnc_tasks.administrative_contact,
	cv_pnc_tasks.application_owner,
	cv_pnc_tasks.requestor,
	cv_pnc_tasks.storage,
	cv_pnc_tasks.dns,
	cv_pnc_tasks.tsm,
	cv_pnc_tasks.legato,
	cv_pnc_tasks.step,
	cv_pnc_tasks.substep,
	cv_pnc_tasks.non_transparent,
	cv_pnc_tasks.client,
	cv_services.name,
	#cvTasks.created,
	CASE
		WHEN cv_pnc_tasks.storage = 1 THEN ISNULL(type_vmware, #cvTasks.completed)
		ELSE #cvTasks.completed
	END AS completed,
	#cvTasks.userid,
	--#cvTasks.tsm_registered,
	#cvTasks.id,
	#cvTasks.status,
	cv_users.fname + ' ' + cv_users.lname AS username,
	CASE
		-- TSM
		WHEN cv_pnc_tasks.tsm = 1 AND #cvTasks.tsm = 1 AND (SELECT TOP 1 tsm_output FROM cv_servers WHERE id = @serverid) = 'PENDING' THEN 'Processing (this may take a minute)...'
		WHEN cv_pnc_tasks.tsm = 1 AND #cvTasks.tsm = 1 THEN ISNULL((SELECT TOP 1 tsm_output FROM cv_servers WHERE id = @serverid), '')
		-- Avamar
		WHEN cv_pnc_tasks.legato = 1 AND #cvTasks.tsm = 0 AND #cvTasks.avamar > 0 AND (SELECT TOP 1 started FROM cv_servers_avamar WHERE serverid = @serverid AND latest = 1 AND deleted = 0) IS NOT NULL AND (SELECT TOP 1 completed FROM cv_servers_avamar WHERE serverid = @serverid AND latest = 1 AND deleted = 0) IS NULL THEN 'Processing (this may take a minute)...'
		WHEN cv_pnc_tasks.legato = 1 AND #cvTasks.tsm = 0 AND #cvTasks.avamar > 0 THEN ISNULL((SELECT TOP 1 output FROM cv_servers_avamar WHERE serverid = @serverid AND latest = 1 AND deleted = 0), '')
		-- Legato
		WHEN cv_pnc_tasks.legato = 1 AND #cvTasks.tsm = 0 AND #cvTasks.avamar < 1 THEN #cvTasks.comments
		ELSE #cvTasks.comments
	END AS comments
INTO 
	#return
FROM
	cv_pnc_tasks
		INNER JOIN
			cv_services
				LEFT OUTER JOIN
					#cvTasks
						LEFT OUTER JOIN
							cv_users
						ON
							#cvTasks.userid = cv_users.userid
						OUTER APPLY
							(
								SELECT TOP 1
									cv_resource_requests.id
								FROM
									cv_resource_requests
										INNER JOIN
											cv_ondemand_tasks_server_backup
										ON
											cv_resource_requests.requestid = cv_ondemand_tasks_server_backup.requestid
											AND cv_resource_requests.itemid = cv_ondemand_tasks_server_backup.itemid
											AND cv_resource_requests.number = cv_ondemand_tasks_server_backup.number
											AND cv_resource_requests.serviceid = 523
											AND cv_resource_requests.deleted = 0
											AND cv_ondemand_tasks_server_backup.answerid = #cvTasks.answerid
								WHERE
									cv_resource_requests.deleted = 0
							) AS cvBackupTSM
				ON
					cv_services.serviceid = #cvTasks.serviceid
		ON
			cv_pnc_tasks.serviceid = cv_services.serviceid
			AND cv_services.enabled = 1
			AND cv_services.deleted = 0
WHERE
	cv_pnc_tasks.enabled = 1
	AND cv_pnc_tasks.deleted = 0
	AND cv_pnc_tasks.decom = 0
	AND (cv_pnc_tasks.if_ltm_install = 0 OR (cv_pnc_tasks.if_ltm_install = (SELECT MAX(ha_csm) FROM #cvTasks WHERE answerid = @answerid)))
	AND (cv_pnc_tasks.if_ltm_config = 0 OR (cv_pnc_tasks.if_ltm_config = (SELECT MAX(ha_csm) FROM #cvTasks WHERE answerid = @answerid)))
	AND (cv_pnc_tasks.if_cluster = 0 OR (cv_pnc_tasks.if_cluster = (SELECT MAX(ha_cluster) FROM #cvTasks WHERE answerid = @answerid)))
	AND (cv_pnc_tasks.if_citrix = 0 OR (cv_pnc_tasks.if_citrix = (SELECT MAX(citrix) FROM #cvTasks WHERE answerid = @answerid)))
	AND (((SELECT MAX(pnc) FROM #cvTasks WHERE answerid = @answerid) = cv_pnc_tasks.pnc) OR ((SELECT MAX(pnc) FROM #cvTasks WHERE answerid = @answerid) <> cv_pnc_tasks.ncb))
	AND (((SELECT MAX(windows) FROM #cvTasks WHERE answerid = @answerid) = cv_pnc_tasks.[distributed]) OR ((SELECT MAX(windows) FROM #cvTasks WHERE answerid = @answerid) <> cv_pnc_tasks.midrange))
	AND (cv_pnc_tasks.storage = 0 OR (cv_pnc_tasks.storage = 1 AND ((SELECT MAX(storage) FROM #cvTasks WHERE answerid = @answerid) = 1)))
	AND (
		(cv_pnc_tasks.tsm = 0 AND cv_pnc_tasks.legato = 0)
		--OR (#cvTasks.tsm = 1 AND cv_pnc_tasks.tsm = 1 AND (SELECT MAX([backup]) FROM #cvTasks WHERE answerid = @answerid) = 1)
		--OR (#cvTasks.tsm = 0 AND cv_pnc_tasks.legato = 1 AND (SELECT MAX([backup]) FROM #cvTasks WHERE answerid = @answerid) = 1)
		OR (cv_pnc_tasks.tsm = 1 AND (#cvTasks.tsm = 1 OR cvBackupTSM.id IS NOT NULL) AND (SELECT MAX([backup]) FROM #cvTasks WHERE answerid = @answerid) = 1)
		OR (cv_pnc_tasks.legato = 1 AND (#cvTasks.tsm = 0 AND cvBackupTSM.id IS NULL) AND (SELECT MAX([backup]) FROM #cvTasks WHERE answerid = @answerid) = 1)
	)
	AND (
		(#cvTasks.virtual = 1 AND cv_pnc_tasks.if_virtual = 1)
		OR (#cvTasks.virtual = 0 AND cv_pnc_tasks.if_physical = 1)
	)
	AND (cv_pnc_tasks.offsite = 0 OR (cv_pnc_tasks.offsite = (SELECT MAX(offsite) FROM #cvTasks WHERE answerid = @answerid)))
ORDER BY
	step, substep

IF (@current = 1)
	SELECT TOP 1 * FROM #return WHERE completed IS NULL
ELSE
	SELECT * FROM #return

DROP TABLE #return
DROP TABLE #cvTasks
DROP TABLE #components
GO

ALTER procedure [dbo].[pr_getPNCTaskStepsDesigns]
AS
-- NOTE: if the CREATED field is NULL then the service has not been inititated
-- NOTE: if the CREATED field is NOT NULL, but the completed date IS NULL, then the service has been inititated but is incomplete
-- GET ALL TASKS ASSOCIATED WITH THE DESIGN
SELECT DISTINCT
	cv_servers.answerid,
	cv_servername_components_details_selected.detailid
INTO
	#components
FROM
	cv_servername_components_details_selected
		INNER JOIN
			cv_servers
		ON
			cv_servername_components_details_selected.serverid = cv_servers.id
			AND cv_servers.deleted = 0
		INNER JOIN
			cv_servername_components_details
				INNER JOIN
					cv_servername_components
				ON
					cv_servername_components_details.componentid = cv_servername_components.id
					AND cv_servername_components.deleted = 0
		ON
			cv_servername_components_details_selected.detailid = cv_servername_components_details.id
			AND cv_servername_components_details.deleted = 0
WHERE
	cv_servername_components_details_selected.deleted = 0

SELECT DISTINCT
	cvOnDemandTasks.*,
	cv_classs.pnc,
	CASE WHEN
		cv_operating_systems.windows = 1 OR cv_operating_systems.windows2008 = 1 THEN 1
		ELSE 0
	END AS windows,
	CASE 
		WHEN cv_servers.dns_auto = 1 THEN cv_forecast_answers.completed
		ELSE NULL
	END AS dns_auto,
	CASE
		WHEN cv_forecast_responses.ha_csm = 1 THEN 1
		WHEN cv_designs.ha_load_balancing = 1 THEN 1
		ELSE 0
	END AS ha_csm,
	CASE
		WHEN cv_forecast_responses.ha_cluster = 1 THEN 1
		WHEN cv_designs.ha_clustering = 1 THEN 1
		ELSE 0
	END AS ha_cluster,
	CASE
		WHEN compCitrix.detailid IS NOT NULL OR compCitrix.detailid IS NOT NULL THEN 1
		ELSE 0
	END AS citrix,
	CASE
		WHEN cv_location_address.common = 1 THEN 0
		ELSE 1
	END AS offsite,
	cv_forecast_answers.[backup],
	cv_forecast_answers.avamar,
	cvBackupLocation.tsm,
	cv_servers.tsm_registered,
	cv_servers.tsm_output,
	cv_servers_avamar.output AS avamar_output,
	cv_servers_avamar.started AS avamar_started,
	cv_servers_avamar.completed AS avamar_completed,
	(SELECT
		MAX(cvAvamar.error)
	FROM
	(
		SELECT MAX(cv_servers_avamar.error) AS error FROM cv_servers_avamar WHERE cv_servers_avamar.serverid IN (SELECT cv_servers.id FROM cv_servers WHERE cv_servers.answerid = cv_forecast_answers.id)
		UNION ALL
		SELECT MAX(cv_servers_avamar_activations.error) AS error FROM cv_servers_avamar_activations WHERE cv_servers_avamar_activations.serverid IN (SELECT cv_servers.id FROM cv_servers WHERE cv_servers.answerid = cv_forecast_answers.id)
		UNION ALL
		SELECT MAX(cv_servers_avamar_backups.error) AS error FROM cv_servers_avamar_backups WHERE cv_servers_avamar_backups.serverid IN (SELECT cv_servers.id FROM cv_servers WHERE cv_servers.answerid = cv_forecast_answers.id)
	) AS cvAvamar) AS avamar_error,
	--cv_servers_avamar.error AS avamar_error,
	CASE 
		WHEN cv_models_property.sun_virtual = 1 AND cv_sve_clusters.storage_allocated = 1 THEN cv_forecast_answers.completed
		ELSE NULL
	END AS sun_virtual_storage,
	cv_forecast_answers.storage,
	CASE 
		WHEN cv_models_property.type_vmware = 1 THEN cv_forecast_answers.completed
		ELSE NULL
	END AS type_vmware,
	CASE
		WHEN cv_models_property.vmware_virtual = 1 OR cv_models_property.ibm_virtual = 1 OR cv_models_property.sun_virtual = 1 THEN 1
		ELSE 0
	END AS virtual
INTO
	#cvTasksTemp
FROM
	(
		SELECT cvFA.id AS answerid, 0 AS serviceid, NULL AS created, NULL AS completed, 0 AS userid, '' AS comments, 0 AS status, 0 AS id
			FROM
				cv_forecast_answers AS cvFA
			WHERE
				cvFA.deleted = 0
				AND cvFA.forecastid > 0
				AND executed_by > 0
				AND executed IS NOT NULL
				AND completed IS NOT NULL
				AND cvFA.id NOT IN 
				(
					SELECT answerid FROM cv_ondemand_tasks_server_other WHERE deleted = 0
					UNION ALL
					SELECT answerid FROM cv_ondemand_tasks_server_storage WHERE deleted = 0
					UNION ALL
					SELECT answerid FROM cv_ondemand_tasks_server_backup WHERE deleted = 0
				)
		UNION ALL
		SELECT answerid, cv_ondemand_tasks_server_other.serviceid, cv_ondemand_tasks_server_other.created, cv_ondemand_tasks_server_other.completed, ISNULL(cv_resource_requests_workflow.userid,0) AS userid, ISNULL(CAST(cv_resource_request_update.comments AS VARCHAR(MAX)),'') AS comments, ISNULL(cv_resource_request_update.status,0) AS status, cv_resource_requests_workflow.id AS id
			FROM cv_ondemand_tasks_server_other 
				INNER JOIN
					cv_resource_requests
						LEFT OUTER JOIN
							cv_resource_requests_workflow
								LEFT OUTER JOIN
									cv_resource_request_update
								ON
									cv_resource_requests_workflow.id = cv_resource_request_update.parent
									AND cv_resource_request_update.latest = 1
									AND cv_resource_request_update.deleted = 0
						ON
							cv_resource_requests.id = cv_resource_requests_workflow.parent
							AND cv_resource_requests_workflow.deleted = 0
				ON
					cv_ondemand_tasks_server_other.requestid = cv_resource_requests.requestid
					AND cv_ondemand_tasks_server_other.serviceid = cv_resource_requests.serviceid
					AND cv_ondemand_tasks_server_other.number = cv_resource_requests.number
					AND cv_resource_requests.deleted = 0
			WHERE
				cv_ondemand_tasks_server_other.deleted = 0
				--AND cv_ondemand_tasks_server_other.answerid = @answerid
		UNION ALL
		SELECT answerid, cv_resource_requests.serviceid, cv_resource_requests.created, cv_ondemand_tasks_server_storage.completed, ISNULL(cv_resource_requests_workflow.userid,0) AS userid, ISNULL(CAST(cv_resource_request_update.comments AS VARCHAR(MAX)),'') AS comments, ISNULL(cv_resource_request_update.status,0) AS status, cv_resource_requests_workflow.id AS id
			FROM cv_ondemand_tasks_server_storage
				INNER JOIN
					cv_resource_requests
						LEFT OUTER JOIN
							cv_resource_requests_workflow
								LEFT OUTER JOIN
									cv_resource_request_update
								ON
									cv_resource_requests_workflow.id = cv_resource_request_update.parent
									AND cv_resource_request_update.latest = 1
									AND cv_resource_request_update.deleted = 0
						ON
							cv_resource_requests.id = cv_resource_requests_workflow.parent
							AND cv_resource_requests_workflow.deleted = 0
				ON
					cv_ondemand_tasks_server_storage.requestid = cv_resource_requests.requestid
					AND cv_ondemand_tasks_server_storage.itemid = cv_resource_requests.itemid
					AND cv_ondemand_tasks_server_storage.number = cv_resource_requests.number
					AND cv_resource_requests.deleted = 0
			WHERE
				cv_ondemand_tasks_server_storage.deleted = 0
				--AND cv_ondemand_tasks_server_storage.answerid = @answerid
		UNION ALL
		SELECT answerid, cv_resource_requests.serviceid, cv_resource_requests.created, cv_ondemand_tasks_server_backup.completed, ISNULL(cv_resource_requests_workflow.userid,0) AS userid, ISNULL(CAST(cv_resource_request_update.comments AS VARCHAR(MAX)),'') AS comments, ISNULL(cv_resource_request_update.status,0) AS status, cv_resource_requests_workflow.id AS id
			FROM cv_ondemand_tasks_server_backup
				INNER JOIN
					cv_resource_requests
						LEFT OUTER JOIN
							cv_resource_requests_workflow
								LEFT OUTER JOIN
									cv_resource_request_update
								ON
									cv_resource_requests_workflow.id = cv_resource_request_update.parent
									AND cv_resource_request_update.latest = 1
									AND cv_resource_request_update.deleted = 0
						ON
							cv_resource_requests.id = cv_resource_requests_workflow.parent
							AND cv_resource_requests_workflow.deleted = 0
				ON
					cv_ondemand_tasks_server_backup.requestid = cv_resource_requests.requestid
					AND cv_ondemand_tasks_server_backup.itemid = cv_resource_requests.itemid
					AND cv_ondemand_tasks_server_backup.number = cv_resource_requests.number
					AND cv_resource_requests.deleted = 0
			WHERE
				cv_ondemand_tasks_server_backup.deleted = 0
				--AND cv_ondemand_tasks_server_backup.answerid = @answerid
	) AS cvOnDemandTasks
		INNER JOIN
			cv_forecast_answers
				LEFT OUTER JOIN
					cv_designs
				ON
					cv_forecast_answers.id = cv_designs.answerid
					AND cv_designs.deleted = 0
				INNER JOIN
					cv_forecast_answers_platform
						INNER JOIN
							cv_forecast_responses
						ON
							cv_forecast_answers_platform.responseid = cv_forecast_responses.id
							AND cv_forecast_responses.deleted = 0
				ON
					cv_forecast_answers.id = cv_forecast_answers_platform.answerid
					AND cv_forecast_answers_platform.deleted = 0
				LEFT OUTER JOIN
					cv_forecast_answers_backup
				ON
					cv_forecast_answers.id = cv_forecast_answers_backup.answerid
					AND cv_forecast_answers_backup.deleted = 0
				INNER JOIN
					cv_location_address AS cvBackupLocation
				ON
					cv_forecast_answers.addressid = cvBackupLocation.id
					AND cvBackupLocation.deleted = 0
				LEFT OUTER JOIN
					#components AS compCitrix
						INNER JOIN
							cv_servername_components_details
								INNER JOIN
									cv_servername_components
								ON
									cv_servername_components_details.componentid = cv_servername_components.id
									AND cv_servername_components.code = 'CTX'
									AND cv_servername_components.deleted = 0
						ON
							compCitrix.detailid = cv_servername_components_details.id
							AND cv_servername_components_details.deleted = 0
				ON
					cv_forecast_answers.id = compCitrix.answerid
				INNER JOIN
					cv_classs
				ON
					cv_forecast_answers.classid = cv_classs.id
					AND cv_classs.deleted = 0
				INNER JOIN
					cv_location_address
				ON
					cv_forecast_answers.addressid = cv_location_address.id
					AND cv_location_address.deleted = 0
				INNER JOIN
					cv_servers
						LEFT OUTER JOIN
							cv_servers_avamar
						ON
							cv_servers.id = cv_servers_avamar.serverid
							AND cv_servers_avamar.latest = 1
							AND cv_servers_avamar.deleted = 0
						INNER JOIN
							cv_operating_systems
						ON
							cv_operating_systems.id = cv_servers.osid
							AND cv_operating_systems.deleted = 0
						INNER JOIN
							cv_models_property
						ON
							cv_models_property.id = cv_servers.modelid
							AND cv_models_property.deleted = 0
						LEFT OUTER JOIN
							cv_sve_guests
								INNER JOIN
									cv_sve_clusters
								ON
									cv_sve_guests.clusterid = cv_sve_clusters.id
									AND cv_sve_clusters.deleted = 0
						ON
							cv_servers.id = cv_sve_guests.serverid
							AND cv_sve_guests.deleted = 0
				ON
					cv_servers.answerid = cv_forecast_answers.id
					AND cv_servers.deleted = 0
		ON
			cvOnDemandTasks.answerid = cv_forecast_answers.id
			AND cv_forecast_answers.deleted = 0


--SELECT * FROM #cvTasksTemp WHERE answerid = 11673

SELECT DISTINCT
	answerid
INTO
	#cvDesigns
FROM
	#cvTasksTemp
WHERE
	answerid in (SELECT answerid FROM cv_servers WHERE build_ready IS NULL AND modelid IN (select id FROM cv_models_property WHERE manual_build <> 1))
	AND answerid NOT in (SELECT answerid FROM cv_servers WHERE build_ready IS NOT NULL)
	AND answerid in (SELECT id FROM cv_forecast_answers WHERE forecastid > 0 AND executed_by > 0 AND executed IS NOT NULL AND completed IS NOT NULL)
	AND answerid NOT in (SELECT id FROM cv_forecast_answers WHERE finished IS NOT NULL)
--	AND answerid = 11673

--SELECT * FROM #cvDesigns

CREATE TABLE #OpenDesigns (answerid INT, serviceid INT, name VARCHAR(200), step INT)

DECLARE @answerid int
DECLARE c1 CURSOR FOR SELECT answerid FROM #cvDesigns
OPEN c1
FETCH NEXT FROM c1 INTO @answerid
WHILE @@FETCH_STATUS = 0
BEGIN


		SELECT
			cvTasksTemp.*
		INTO
			#cvTasks
		FROM
			(
				SELECT
					@answerid AS answerid,
					0 AS id,
					serviceid,
					step,
					CASE
						WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid)
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid)
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid)
						WHEN cv_pnc_tasks.tsm = 1 AND ((SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL) IS NOT NULL) THEN (SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL)
						WHEN cv_pnc_tasks.legato = 1 AND cv_pnc_tasks.non_transparent = 0 AND ((SELECT TOP 1 avamar_output FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_output IS NOT NULL) IS NOT NULL) THEN (SELECT TOP 1 avamar_started FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_started IS NOT NULL)
						ELSE NULL
					END AS created,
					CASE
						WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid)
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid)
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid)
						WHEN cv_pnc_tasks.tsm = 1 AND ((SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL) IS NOT NULL) AND ((SELECT TOP 1 tsm_output FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) AND ((SELECT TOP 1 tsm_output FROM #cvTasksTemp WHERE answerid = @answerid) <> 'PENDING') THEN (SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL)
						WHEN cv_pnc_tasks.legato = 1 AND cv_pnc_tasks.non_transparent = 0 AND ((SELECT TOP 1 avamar_output FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_output IS NOT NULL) IS NOT NULL) AND ((SELECT TOP 1 avamar_started FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 avamar_completed FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_completed IS NOT NULL AND avamar_error <> 1)
						ELSE NULL
					END AS completed,
					NULL AS pnc,
					NULL AS windows,
					NULL AS dns_auto,
					NULL AS ha_csm,
					NULL AS ha_cluster,
					NULL AS citrix,
					(SELECT TOP 1 offsite FROM #cvTasksTemp WHERE answerid = @answerid) AS offsite,
					NULL AS [backup],
					(SELECT TOP 1 avamar FROM #cvTasksTemp WHERE answerid = @answerid) AS avamar,
					(SELECT TOP 1 tsm FROM #cvTasksTemp WHERE answerid = @answerid) AS tsm,
					NULL AS tsm_registered,
					NULL AS storage,
					CASE
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN (SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid)
						ELSE NULL
					END AS type_vmware,
					CASE
						WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN -999
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN -999
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN -999
						WHEN cv_pnc_tasks.tsm = 1 AND ((SELECT TOP 1 tsm_registered FROM #cvTasksTemp WHERE answerid = @answerid AND tsm_registered IS NOT NULL) IS NOT NULL) THEN -999
						WHEN cv_pnc_tasks.legato = 1 AND cv_pnc_tasks.non_transparent = 0 AND ((SELECT TOP 1 avamar_output FROM #cvTasksTemp WHERE answerid = @answerid AND avamar_output IS NOT NULL) IS NOT NULL) THEN -999
						ELSE 0
					END AS userid,
					CASE
						WHEN cv_pnc_tasks.dns = 1 AND ((SELECT TOP 1 dns_auto FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN 'DNS record successfully created'
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 type_vmware FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN 'Completed'
						WHEN cv_pnc_tasks.storage = 1 AND ((SELECT TOP 1 sun_virtual_storage FROM #cvTasksTemp WHERE answerid = @answerid) IS NOT NULL) THEN 'Completed'
						-- TSM output handled later on
						ELSE ''
					END AS comments,
					0 AS status,
					(SELECT TOP 1 virtual FROM #cvTasksTemp WHERE answerid = @answerid) AS virtual
				FROM
					cv_pnc_tasks
				WHERE
					cv_pnc_tasks.serviceid NOT IN (SELECT serviceid FROM #cvTasksTemp WHERE answerid = @answerid)
					AND cv_pnc_tasks.decom = 0
				UNION ALL
					SELECT
						#cvTasksTemp.answerid,
						#cvTasksTemp.id,
						#cvTasksTemp.serviceid,
						cv_pnc_tasks.step,
						#cvTasksTemp.created,
						#cvTasksTemp.completed,
						#cvTasksTemp.pnc,
						#cvTasksTemp.windows,
						#cvTasksTemp.dns_auto,
						#cvTasksTemp.ha_csm,
						#cvTasksTemp.ha_cluster,
						#cvTasksTemp.citrix,
						#cvTasksTemp.offsite,
						#cvTasksTemp.[backup],
						#cvTasksTemp.avamar,
						#cvTasksTemp.tsm,
						#cvTasksTemp.tsm_registered,
						#cvTasksTemp.storage,
						#cvTasksTemp.type_vmware,
						#cvTasksTemp.userid,
						#cvTasksTemp.comments,
						#cvTasksTemp.status,
						#cvTasksTemp.virtual
					FROM
						#cvTasksTemp
							LEFT OUTER JOIN
								cv_pnc_tasks
							ON
								#cvTasksTemp.serviceid = cv_pnc_tasks.serviceid
								AND cv_pnc_tasks.deleted = 0
					WHERE 
						#cvTasksTemp.answerid = @answerid
			) AS cvTasksTemp

		--SELECT * FROM #cvTasks

		
		-- GET ALL TASKS CURRENTLY BEING WORKED ON
		SELECT DISTINCT
			cv_pnc_tasks.serviceid,
			cv_pnc_tasks.implementor,
			cv_pnc_tasks.network_engineer,
			cv_pnc_tasks.dba,
			cv_pnc_tasks.project_manager,
			cv_pnc_tasks.departmental_manager,
			cv_pnc_tasks.application_lead,
			cv_pnc_tasks.administrative_contact,
			cv_pnc_tasks.application_owner,
			cv_pnc_tasks.requestor,
			cv_pnc_tasks.step,
			cv_pnc_tasks.substep,
			cv_services.name,
			#cvTasks.created,
			CASE
				WHEN cv_pnc_tasks.storage = 1 THEN ISNULL(type_vmware, #cvTasks.completed)
				ELSE #cvTasks.completed
			END AS completed,
			#cvTasks.userid,
			#cvTasks.id,
			#cvTasks.status,
			cv_users.fname + ' ' + cv_users.lname AS username,
			CASE
				-- TSM
				WHEN cv_pnc_tasks.tsm = 1 AND #cvTasks.tsm = 1 AND (SELECT TOP 1 tsm_output FROM cv_servers WHERE answerid = @answerid) = 'PENDING' THEN 'Processing (this may take a minute)...'
				WHEN cv_pnc_tasks.tsm = 1 AND #cvTasks.tsm = 1 THEN ISNULL((SELECT TOP 1 tsm_output FROM cv_servers WHERE answerid = @answerid), '')
				-- Avamar
				WHEN cv_pnc_tasks.legato = 1 AND #cvTasks.tsm = 0 AND #cvTasks.avamar > 0 AND (SELECT TOP 1 started FROM cv_servers_avamar WHERE serverid in (select id from cv_servers where answerid = @answerid) AND latest = 1 AND deleted = 0) IS NOT NULL AND (SELECT TOP 1 completed FROM cv_servers_avamar WHERE serverid in (select id from cv_servers where answerid = @answerid) AND latest = 1 AND deleted = 0) IS NULL THEN 'Processing (this may take a minute)...'
				WHEN cv_pnc_tasks.legato = 1 AND #cvTasks.tsm = 0 AND #cvTasks.avamar > 0 THEN ISNULL((SELECT TOP 1 output FROM cv_servers_avamar WHERE serverid in (select id from cv_servers where answerid = @answerid) AND latest = 1 AND deleted = 0), '')
				-- Legato
				WHEN cv_pnc_tasks.legato = 1 AND #cvTasks.tsm = 0 AND #cvTasks.avamar < 1 THEN #cvTasks.comments
				ELSE #cvTasks.comments
			END AS comments
		INTO 
			#return
		FROM
			cv_pnc_tasks
				INNER JOIN
					cv_services
						LEFT OUTER JOIN
							#cvTasks
								LEFT OUTER JOIN
									cv_users
								ON
									#cvTasks.userid = cv_users.userid
						ON
							cv_services.serviceid = #cvTasks.serviceid
				ON
					cv_pnc_tasks.serviceid = cv_services.serviceid
					AND cv_services.enabled = 1
					AND cv_services.deleted = 0
		WHERE
			cv_pnc_tasks.enabled = 1
			AND cv_pnc_tasks.deleted = 0
			AND cv_pnc_tasks.decom = 0
			AND (cv_pnc_tasks.if_ltm_install = 0 OR (cv_pnc_tasks.if_ltm_install = (SELECT MAX(ha_csm) FROM #cvTasks WHERE answerid = @answerid)))
			AND (cv_pnc_tasks.if_ltm_config = 0 OR (cv_pnc_tasks.if_ltm_config = (SELECT MAX(ha_csm) FROM #cvTasks WHERE answerid = @answerid)))
			AND (cv_pnc_tasks.if_cluster = 0 OR (cv_pnc_tasks.if_cluster = (SELECT MAX(ha_cluster) FROM #cvTasks WHERE answerid = @answerid)))
			AND (cv_pnc_tasks.if_citrix = 0 OR (cv_pnc_tasks.if_citrix = (SELECT MAX(citrix) FROM #cvTasks WHERE answerid = @answerid)))
			AND (((SELECT MAX(pnc) FROM #cvTasks WHERE answerid = @answerid) = cv_pnc_tasks.pnc) OR ((SELECT MAX(pnc) FROM #cvTasks WHERE answerid = @answerid) <> cv_pnc_tasks.ncb))
			AND (((SELECT MAX(windows) FROM #cvTasks WHERE answerid = @answerid) = cv_pnc_tasks.[distributed]) OR ((SELECT MAX(windows) FROM #cvTasks WHERE answerid = @answerid) <> cv_pnc_tasks.midrange))
			AND (cv_pnc_tasks.storage = 0 OR (cv_pnc_tasks.storage = 1 AND ((SELECT MAX(storage) FROM #cvTasks WHERE answerid = @answerid) = 1)))
			AND (
				(cv_pnc_tasks.tsm = 0 AND cv_pnc_tasks.legato = 0)
				OR (#cvTasks.tsm = 1 AND cv_pnc_tasks.tsm = 1 AND (SELECT MAX([backup]) FROM #cvTasks WHERE answerid = @answerid) = 1)
				OR (#cvTasks.tsm = 0 AND cv_pnc_tasks.legato = 1 AND (SELECT MAX([backup]) FROM #cvTasks WHERE answerid = @answerid) = 1)
			)
			AND (
				(#cvTasks.virtual = 1 AND cv_pnc_tasks.if_virtual = 1)
				OR (#cvTasks.virtual = 0 AND cv_pnc_tasks.if_physical = 1)
			)
			AND (cv_pnc_tasks.offsite = 0 OR (cv_pnc_tasks.offsite = (SELECT MAX(offsite) FROM #cvTasks WHERE answerid = @answerid)))
		ORDER BY
			step, substep

		--SELECT * FROM#return

		DECLARE @step INT
		SET @step = (SELECT MIN(step) FROM #return)
		DECLARE @step_create INT
		SET @step_create = (SELECT MIN(step) FROM #return WHERE created IS NULL)
		DECLARE @step_active INT
		SET @step_active = (SELECT MIN(step) FROM #return WHERE created IS NOT NULL AND completed IS NULL)
		/*
		SELECT @step
		SELECT @step_create
		SELECT @step_active*/

		SELECT DISTINCT
			serviceid,
			implementor,
			step,
			name,
			created,
			completed,
			userid,
			id,
			status,
			username,
			comments
		INTO 
			#return2
		FROM
			#return
		WHERE
			-- Four scenarios
			-- #1 = No tasks initiated
			--    = @step IS NULL
			-- #2 = A previous task has not been initiated
			--    = step = @step_create AND step <= @step_active
			-- #3 = All tasks have been initiated up to a point, and then none on the same step have been kicked off
			--    = step = @step_create AND step <= @step_active
			-- #4 = All tasks have been initiated up to a point, and then none in the future (greater step) have been kicked off
			--    = step = @step_create AND @step_active IS NULL
			((@step IS NULL) OR (step = @step_create AND step <= @step_active) OR (step = @step_create AND @step_active IS NULL))
			AND created IS NULL


	INSERT INTO #OpenDesigns 
	SELECT @answerid, serviceid, name, step FROM #return2
	
	--SELECT * FROM #return2

	DROP TABLE #return2
	DROP TABLE #return
	DROP TABLE #cvTasks

FETCH NEXT FROM c1 INTO @answerid
END
CLOSE c1
DEALLOCATE c1

SELECT * FROM #OpenDesigns ORDER BY answerid, step
SELECT DISTINCT answerid FROM #OpenDesigns

DROP TABLE #cvTasksTemp
DROP TABLE #OpenDesigns
DROP TABLE #cvDesigns
DROP TABLE #components
GO

CREATE PROCEDURE [dbo].[pr_addClustering]
	@answerid int,
	@id int output
AS
UPDATE
	cv_clustering
SET
	latest = 0
WHERE
	answerid = @answerid
INSERT INTO
	cv_clustering
VALUES
(
	@answerid,
	getdate(),	--created
	NULL,		--started
	NULL,		--completed
	'',			--output
	1,			--latest
	NULL,		--error
	0
)
SET @id = SCOPE_IDENTITY()
GO

CREATE PROCEDURE [dbo].[pr_updateClusteringStarted]
	@answerid int,
	@started datetime
AS
UPDATE
	cv_clustering
SET
	[started] = @started,
	[completed] = NULL,
	[output] = '',
	[error] = NULL
WHERE
	answerid = @answerid
	AND [latest] = 1
GO

CREATE PROCEDURE [dbo].[pr_updateClusteringCompleted]
	@answerid int,
	@output varchar(MAX),
	@completed datetime,
	@error int
AS
UPDATE
	cv_clustering
SET
	[completed] = @completed,
	[output] = @output,
	[error] = @error
WHERE
	answerid = @answerid
	AND [latest] = 1
GO

CREATE PROCEDURE [dbo].[pr_getClustering]
AS
-- EXEC pr_getClustering
SELECT
	cv_clustering.answerid
FROM
	cv_clustering
		INNER JOIN 
			cv_forecast_answers
				INNER JOIN
					cv_designs
				ON
					cv_designs.answerid = cv_forecast_answers.id
					AND cv_designs.deleted = 0
				INNER JOIN
					cv_forecast
						INNER JOIN
							cv_requests
								INNER JOIN
									cv_projects
								ON
									cv_requests.projectid = cv_projects.projectid
									AND cv_projects.deleted = 0
						ON
							cv_forecast.requestid = cv_requests.requestid
							AND cv_requests.deleted = 0
				ON
					cv_forecast_answers.forecastid = cv_forecast.id
					AND cv_forecast.deleted = 0
		ON
			cv_clustering.answerid = cv_forecast_answers.id
			AND cv_forecast_answers.deleted = 0
WHERE
	cv_clustering.deleted = 0
	AND cv_clustering.latest = 1
	AND cv_clustering.started IS NULL
	AND cv_clustering.completed IS NULL
	AND cv_clustering.answerid NOT IN (SELECT answerid FROM cv_forecast_answer_errors WHERE deleted = 0 AND fixed IS NULL)
	AND cv_clustering.answerid NOT IN 
	(
		SELECT
			cv_servers.answerid
		FROM
			cv_servers
				LEFT OUTER JOIN
					cv_servers_avamar
				ON
					cv_servers.id = cv_servers_avamar.serverid
					AND cv_servers_avamar.deleted = 0
					AND cv_servers_avamar.latest = 1
					AND (
						cv_servers_avamar.started IS NULL
						OR cv_servers_avamar.completed IS NULL
						OR cv_servers_avamar.error = 1
					)
				LEFT OUTER JOIN
					cv_servers_avamar_activations
				ON
					cv_servers.id = cv_servers_avamar_activations.serverid
					AND cv_servers_avamar_activations.deleted = 0
					AND cv_servers_avamar_activations.latest = 1
					AND (
						cv_servers_avamar_activations.started IS NULL
						OR cv_servers_avamar_activations.completed IS NULL
						OR cv_servers_avamar_activations.error = 1
					)
				LEFT OUTER JOIN
					cv_servers_avamar_backups
				ON
					cv_servers.id = cv_servers_avamar_backups.serverid
					AND cv_servers_avamar_backups.deleted = 0
					AND cv_servers_avamar_backups.latest = 1
					AND (
						cv_servers_avamar_backups.completed IS NULL
						OR cv_servers_avamar_backups.error = 1
					)
		WHERE
			cv_servers.deleted = 0
			AND cv_servers.step = 999
			AND
			(
				cv_servers_avamar.id IS NOT NULL
				OR cv_servers_avamar_activations.id IS NOT NULL
				OR cv_servers_avamar_backups.id IS NOT NULL
			)
	)
GO

CREATE PROCEDURE [dbo].[pr_getServerStorageConfigured]
AS
-- EXEC pr_getServerStorageConfigured
SELECT
	cv_servers.id,
	cv_servers.answerid,
	cv_servers.number,
	cv_servers.clusterid,
	cv_domains.environment,
	cv_forecast_answers.id AS answerid,
	cv_forecast_answers.mnemonicid,
	cv_designs.id AS designid,
	cv_designs.dr,
	cv_location_address.common AS onsite,
	cv_designs.backup_frequency,
	cv_servernames_factory.os + cv_servernames_factory.location + cv_servernames_factory.mnemonic + cv_servernames_factory.environment + cv_servernames_factory.name1 + cv_servernames_factory.name2 + cv_servernames_factory.func + cv_servernames_factory.specific AS servername,
	(
		SELECT TOP 1
			CAST(cv_ip_addresses.add1 AS VARCHAR(3)) + '.' + CAST(cv_ip_addresses.add2 AS VARCHAR(3)) + '.' + CAST(cv_ip_addresses.add3 AS VARCHAR(3)) + '.' + CAST(cv_ip_addresses.add4 AS VARCHAR(3))
		FROM
			ClearViewIP_DEV.dbo.cv_ip_addresses
				INNER JOIN
					cv_servers_ips
				ON
					cv_ip_addresses.id = cv_servers_ips.ipaddressid
					AND cv_servers_ips.deleted = 0
					AND cv_servers_ips.serverid = cv_servers.id
		WHERE
			cv_ip_addresses.deleted = 0
	) AS ipaddress,
	cv_forecast_answers.addressid,
	cv_forecast_answers.classid,
	cv_forecast_answers.environmentid
FROM
	cv_servers
		INNER JOIN 
			cv_forecast_answers
				INNER JOIN
					cv_designs
				ON
					cv_designs.answerid = cv_forecast_answers.id
					AND cv_designs.deleted = 0
				INNER JOIN
					cv_location_address
				ON
					cv_location_address.id = cv_forecast_answers.addressid
					AND cv_location_address.deleted = 0
				INNER JOIN
					cv_forecast
						INNER JOIN
							cv_requests
								INNER JOIN
									cv_projects
								ON
									cv_requests.projectid = cv_projects.projectid
									AND cv_projects.deleted = 0
						ON
							cv_forecast.requestid = cv_requests.requestid
							AND cv_requests.deleted = 0
				ON
					cv_forecast_answers.forecastid = cv_forecast.id
					AND cv_forecast.deleted = 0
		ON
			cv_servers.answerid = cv_forecast_answers.id
			AND cv_forecast_answers.deleted = 0
		INNER JOIN
			cv_servernames_factory
		ON
			cv_servers.nameid = cv_servernames_factory.id
			AND cv_servernames_factory.deleted = 0
			AND cv_servers.pnc = 1
		INNER JOIN
			cv_domains
		ON
			cv_servers.domainid = cv_domains.id
			AND cv_domains.deleted = 0
WHERE
	cv_servers.storage_configured IS NULL
	AND cv_servers.step = 999
	AND cv_servers.deleted = 0

GO

CREATE PROCEDURE [dbo].[pr_updateServerStorageConfigured]
	@id int,
	@storage_configured datetime
AS
UPDATE
	cv_servers
SET
	storage_configured = @storage_configured
WHERE
	id = @id
GO

create procedure [dbo].[pr_addForecastAnswerError]
	@requestid int,
	@itemid int,
	@number int,
	@answerid int,
	@step int,
	@reason varchar(max),
	@id int output
AS
BEGIN
	INSERT INTO
		cv_forecast_answer_errors
	VALUES
	(
		@requestid,
		@itemid,
		@number,
		@answerid,
		@step,
		@reason,
		'',	-- incident
		0,	-- assigned
		0,	-- errorid
		0,	-- userid
		getdate(),
		getdate(),
		null,
		0
	)
	SET @id = SCOPE_IDENTITY()

END
GO

CREATE procedure [dbo].[pr_getForecastAnswerErrorLatest]
	@answerid int,
	@step int
AS
SELECT TOP 1
	*
FROM
	cv_forecast_answer_errors
WHERE
	answerid = @answerid
	AND step = @step
	AND fixed IS NOT NULL
	AND deleted = 0
ORDER BY
	fixed DESC
GO

create procedure [dbo].[pr_getForecastAnswerError]
	@answerid int,
	@step int
AS
SELECT
	*
FROM
	cv_forecast_answer_errors
WHERE
	answerid = @answerid
	AND step = @step
	AND fixed IS NULL
	AND deleted = 0
GO

create procedure [dbo].[pr_updateForecastAnswerErrorIncident]
	@id int,
	@incident varchar(20),
	@assigned int
AS
UPDATE
	cv_forecast_answer_errors
SET
	incident = @incident,
	assigned = @assigned,
	modified = getdate()
WHERE
	id = @id
	AND fixed IS NULL
	AND deleted = 0
GO

create procedure [dbo].[pr_updateForecastAnswerError]
	@answerid int,
	@step int,
	@errorid int,
	@userid int
AS
UPDATE
	cv_forecast_answer_errors
SET
	errorid = @errorid,
	userid = @userid,
	modified = getdate(),
	fixed = getdate()
WHERE
	answerid = @answerid
	AND step = @step
	AND deleted = 0
GO

create procedure [dbo].[pr_getForecastAnswerErrorsByRequest]
	@RequestId int,
	@ItemId int,
	@Number int
AS
SELECT
	cv_forecast_answer_errors.*,
	cv_projects.name AS project,
	cv_projects.number as ProjectNumber,
	CASE
		WHEN cv_users.pnc_id IS NOT NULL AND cv_users.pnc_id <> '' THEN cv_users.pnc_id
		ELSE cv_users.xid
	END AS implementor,
	RTRIM(isnull(cv_users.lname,'')) + ', ' + RTRIM(isnull(cv_users.fname,'')) AS implementorName,  
	CASE
		WHEN CVUserIE.pnc_id IS NOT NULL AND CVUserIE.pnc_id <> '' THEN CVUserIE.pnc_id
		ELSE CVUserIE.xid
	END AS IntegrationEngineer,
	RTRIM(isnull(CVUserIE.lname,'')) + ', ' + RTRIM(isnull(CVUserIE.fname,'')) AS IntegrationEnggName, 
	CASE
		WHEN CVUserExecutedBy.pnc_id IS NOT NULL AND CVUserExecutedBy.pnc_id <> '' THEN CVUserExecutedBy.pnc_id
		ELSE CVUserExecutedBy.xid
	END AS ExecutedBy,
	RTRIM(isnull(CVUserExecutedBy.lname,'')) + ', ' + RTRIM(isnull(CVUserExecutedBy.fname,'')) AS ExecutedByName, 
	cv_forecast_answers.Id as DesignID,
	ISNULL(cv_designs_models_property.name, cv_models_property.name) AS model,
	ISNULL(cv_designs_class.name, cv_classs.name) AS class,
	ISNULL(cv_designs_environment.name, cv_environment.name) AS environment,
	cv_forecast_answers.executed as executed
FROM 
	cv_forecast_answer_errors
		INNER JOIN
			cv_forecast_answers
				LEFT OUTER JOIN
					cv_models_property
				ON
					cv_forecast_answers.modelid = cv_models_property.id
					AND cv_models_property.deleted = 0
				LEFT OUTER JOIN
					cv_classs
				ON
					cv_forecast_answers.classid = cv_classs.id
					AND cv_classs.deleted = 0
				LEFT OUTER JOIN
					cv_environment
				ON
					cv_forecast_answers.environmentid = cv_environment.id
					AND cv_environment.deleted = 0
				LEFT OUTER JOIN
					cv_designs
						LEFT OUTER JOIN
							cv_models_property AS cv_designs_models_property
						ON
							cv_designs.modelid = cv_designs_models_property.id
							AND cv_designs_models_property.deleted = 0
						INNER JOIN
							cv_classs AS cv_designs_class
						ON
							cv_designs.classid = cv_designs_class.id
							AND cv_designs_class.deleted = 0
						INNER JOIN
							cv_environment AS cv_designs_environment
						ON
							cv_designs.environmentid = cv_designs_environment.id
							AND cv_designs_environment.deleted = 0
				ON
					cv_designs.answerid = cv_forecast_answers.id
					AND cv_designs.deleted = 0
				LEFT OUTER JOIN
					cv_forecast
						INNER JOIN
							cv_requests
								INNER JOIN
									cv_projects
										LEFT OUTER JOIN 
											dbo.cv_users AS CVUserIE 
										ON
											cv_projects.engineer = CVUserIE.userid
											AND CVUserIE.deleted = 0
								ON
									cv_requests.projectid = cv_projects.projectid
									AND cv_projects.deleted = 0
						ON
							cv_forecast.requestid = cv_requests.requestid
							AND cv_requests.deleted = 0
				ON
					cv_forecast_answers.forecastid = cv_forecast.id
					AND cv_forecast.deleted = 0
				LEFT OUTER JOIN
					dbo.cv_users AS CVUserExecutedBy 
				ON
					cv_forecast_answers.executed_by = CVUserExecutedBy.userid 
					AND CVUserExecutedBy.deleted = 0
		ON
			cv_forecast_answers.id = cv_forecast_answer_errors.answerid 
			AND cv_forecast_answers.deleted = 0
		LEFT OUTER JOIN
			cv_ondemand_tasks_pending
				INNER JOIN
					cv_resource_requests_workflow
						INNER JOIN
							cv_users
						ON
							cv_resource_requests_workflow.userid = cv_users.userid
							AND cv_users.deleted = 0
				ON
					cv_ondemand_tasks_pending.resourceid = cv_resource_requests_workflow.id
					AND cv_resource_requests_workflow.deleted = 0
		ON
			cv_forecast_answers.id = cv_ondemand_tasks_pending.answerid
			AND cv_ondemand_tasks_pending.deleted = 0
	
WHERE
	cv_forecast_answer_errors.requestid = @RequestId 
	AND cv_forecast_answer_errors.number=@Number 
	AND cv_forecast_answer_errors.itemid=@ItemId
GO

create procedure [dbo].[pr_getForecastAnswerErrors]
	@answerid int
AS
-- EXEC pr_getForecastAnswerErrors 26312
SELECT
	cv_forecast_answer_errors.id,
	cv_forecast_answer_errors.assigned,
	cv_forecast_answer_errors.answerid,
	cv_forecast_answer_errors.fixed,
	cv_forecast_answer_errors.step,
	'Design # ' + CAST(cv_forecast_answers.id AS varchar(50)) AS name,
	cv_forecast_answer_errors.created,
	cv_forecast_answer_errors.reason,
	'Unknown' AS title,
	cv_forecast_answer_errors.incident,
	cv_forecast_answer_errors.requestid,
	cv_forecast_answer_errors.itemid,
	cv_forecast_answer_errors.number
FROM
	cv_forecast_answer_errors
		INNER JOIN 
			cv_forecast_answers
		ON 
			cv_forecast_answer_errors.answerid = cv_forecast_answers.id 
			AND cv_forecast_answers.deleted = 0 
WHERE
	cv_forecast_answer_errors.answerid = @answerid
	AND cv_forecast_answer_errors.deleted = 0
ORDER BY
	cv_forecast_answer_errors.created DESC
GO

ALTER procedure [dbo].[pr_getServerErrors]
	@serverid int
AS
-- EXEC pr_getServerErrors 25839
SELECT
	cv_servers_errors.id,
	cv_servers_errors.assigned,
	cv_forecast_answers.id as answerid,
	cv_servers_errors.fixed,
	cv_servers_errors.step,
	CASE
		WHEN cv_servers.pnc IS NULL OR cv_servers.pnc = 0 THEN cv_servernames.prefix1 + cv_servernames.prefix2 + cv_servernames.sitecode + cv_servernames.name1 + cv_servernames.name2
		ELSE cv_servernames_factory.os + cv_servernames_factory.location + cv_servernames_factory.mnemonic + cv_servernames_factory.environment + cv_servernames_factory.name1 + cv_servernames_factory.name2 + cv_servernames_factory.func + cv_servernames_factory.specific
	END AS name,
	cv_servers_errors.created,
	cv_servers_errors.reason,
	cv_servers_errors.serverid,
	cv_servers_assets.assetid,
	CASE
		WHEN cv_SetupMaster.Description IS NOT NULL THEN cv_SetupMaster.Description
		ELSE ISNULL((SELECT TOP 1 title FROM cv_ondemand_steps WHERE typeid = cv_models.typeid AND deleted = 0 AND step = cv_servers_errors.step),'Unknown')
	END AS title,
	cv_servers_errors.incident,
	cv_servers_errors.requestid,
	cv_servers_errors.itemid,
	cv_servers_errors.number
FROM
	cv_servers_errors
		INNER JOIN
			cv_servers
				INNER JOIN 
					cv_forecast_answers
				ON 
					cv_servers.answerid = cv_forecast_answers.id 
					AND cv_forecast_answers.deleted = 0 
				LEFT OUTER JOIN
					cv_servers_assets
						INNER JOIN
							clearviewasset_DEV.dbo.cva_assets
						ON
							cv_servers_assets.assetid = cva_assets.id
							AND cva_assets.deleted = 0
				ON
					cv_servers.id = cv_servers_assets.serverid
					AND cv_servers_assets.latest = 1
					AND cv_servers_assets.deleted = 0
				LEFT OUTER JOIN
					cv_servernames
				ON
					cv_servers.nameid = cv_servernames.id
					AND cv_servernames.deleted = 0
					AND cv_servers.pnc = 0
				LEFT OUTER JOIN
					cv_servernames_factory
				ON
					cv_servers.nameid = cv_servernames_factory.id
					AND cv_servernames_factory.deleted = 0
					AND cv_servers.pnc = 1
				INNER JOIN
					cv_models_property
						INNER JOIN
							cv_models
						ON
							cv_models_property.modelid = cv_models.id
							AND cv_models.enabled = 1
							AND cv_models.deleted = 0
				ON
					cv_servers.modelid = cv_models_property.id
					AND cv_models_property.enabled = 1
					AND cv_models_property.deleted = 0
		ON
			cv_servers_errors.serverid = cv_servers.id
			AND cv_servers.deleted = 0
		LEFT OUTER JOIN
			cv_SetupMaster
		ON
			CAST(cv_servers_errors.step AS VARCHAR(10)) = cv_SetupMaster.Value
			AND cv_SetupMaster.SMkey = 'SERVER_ERROR'
			AND cv_SetupMaster.deleted = 0
WHERE
	cv_servers_errors.serverid = @serverid
	AND cv_servers_errors.deleted = 0
ORDER BY
	cv_servers_errors.created DESC
GO

ALTER procedure [dbo].[pr_getServerErrorsAll]
AS
-- exec pr_getServerErrorsAll
SELECT
	'sid' AS url,
	cv_servers_errors.serverid AS id,
	cv_servers_errors.reason,
	cv_servers_errors.incident,
	cv_servers_errors.fixed,
	CASE
		WHEN cv_servers.pnc IS NULL OR cv_servers.pnc = 0 THEN cv_servernames.prefix1 + cv_servernames.prefix2 + cv_servernames.sitecode + cv_servernames.name1 + cv_servernames.name2
		ELSE cv_servernames_factory.os + cv_servernames_factory.location + cv_servernames_factory.mnemonic + cv_servernames_factory.environment + cv_servernames_factory.name1 + cv_servernames_factory.name2 + cv_servernames_factory.func + cv_servernames_factory.specific
	END AS name,
	cv_servers_errors.created,
	cv_servers_errors.step,
	ISNULL((SELECT TOP 1 title FROM cv_ondemand_steps WHERE typeid = cv_models.typeid AND deleted = 0 AND step = cv_servers_errors.step),'Unknown') AS title
FROM
	cv_servers_errors
		INNER JOIN
			cv_servers
				INNER JOIN 
					cv_forecast_answers
				ON 
					cv_servers.answerid = cv_forecast_answers.id 
					AND cv_forecast_answers.deleted = 0 
				LEFT OUTER JOIN
					cv_servers_assets
						INNER JOIN
							clearviewasset.dbo.cva_assets
						ON
							cv_servers_assets.assetid = cva_assets.id
							AND cva_assets.deleted = 0
				ON
					cv_servers.id = cv_servers_assets.serverid
					AND cv_servers_assets.latest = 1
					AND cv_servers_assets.deleted = 0
				LEFT OUTER JOIN 
					cv_domains
				ON 
					cv_servers.domainid = cv_domains.id 
					AND cv_domains.enabled = 1
					AND cv_domains.deleted = 0 
				LEFT OUTER JOIN 
					cv_domains AS cv_domains_test
				ON 
					cv_servers.test_domainid = cv_domains_test.id 
					AND cv_domains_test.enabled = 1
					AND cv_domains_test.deleted = 0 
				LEFT OUTER JOIN
					cv_servernames
				ON
					cv_servers.nameid = cv_servernames.id
					AND cv_servernames.deleted = 0
					AND cv_servers.pnc = 0
				LEFT OUTER JOIN
					cv_servernames_factory
				ON
					cv_servers.nameid = cv_servernames_factory.id
					AND cv_servernames_factory.deleted = 0
					AND cv_servers.pnc = 1
				INNER JOIN
					cv_models_property
						INNER JOIN
							cv_models
						ON
							cv_models_property.modelid = cv_models.id
							AND cv_models.enabled = 1
							AND cv_models.deleted = 0
				ON
					cv_servers.modelid = cv_models_property.id
					AND cv_models_property.enabled = 1
					AND cv_models_property.deleted = 0
		ON
			cv_servers_errors.serverid = cv_servers.id
			AND cv_servers.deleted = 0
WHERE
	cv_servers_errors.deleted = 0
	AND cv_servers_errors.fixed IS NULL
UNION ALL
SELECT
	'aid' AS url,
	cv_forecast_answer_errors.answerid AS id,
	cv_forecast_answer_errors.reason,
	cv_forecast_answer_errors.incident,
	cv_forecast_answer_errors.fixed,
	'Design # ' + CAST(cv_forecast_answers.id AS varchar(50)) AS name,
	cv_forecast_answer_errors.created,
	cv_forecast_answer_errors.step,
	'Unknown' AS title
FROM
	cv_forecast_answer_errors
		INNER JOIN
			cv_forecast_answers
		ON
			cv_forecast_answer_errors.answerid = cv_forecast_answers.id
			AND cv_forecast_answer_errors.deleted = 0
WHERE
	cv_forecast_answer_errors.deleted = 0
	AND cv_forecast_answer_errors.fixed IS NULL
ORDER BY
	created
GO

create PROCEDURE [dbo].[pr_addServerRebuild]
	@requestid int,
	@serviceid int,
	@number int,
	@serverid int,
	@change varchar(20)
AS
INSERT INTO
	cv_server_rebuilds
VALUES
(
	@requestid,
	@serviceid,
	@number,
	@serverid,
	@change,
	getdate(),
	null,
	0
)
GO

create procedure [dbo].[pr_getServerRebuild]
	@requestid int,
	@serviceid int,
	@number int
AS
SELECT
	cv_server_rebuilds.*,
	cv_servers.nameid
FROM
	cv_server_rebuilds
		INNER JOIN
			cv_servers
		ON
			cv_server_rebuilds.serverid = cv_servers.id
			AND cv_servers.deleted = 0
WHERE
	cv_server_rebuilds.deleted = 0
	AND cv_server_rebuilds.requestid = @requestid
	AND cv_server_rebuilds.serviceid = @serviceid
	AND cv_server_rebuilds.number = @number
GO

create procedure [dbo].[pr_getServerRebuildID]
	@serverid int
AS
SELECT
	cv_requests.*,
	cv_server_rebuilds.*,
	cv_servers.*
FROM
	cv_server_rebuilds
		INNER JOIN
			cv_servers
		ON
			cv_server_rebuilds.serverid = cv_servers.id
			AND cv_servers.deleted = 0
		INNER JOIN
			cv_requests
		ON
			cv_server_rebuilds.requestid = cv_requests.requestid
			AND cv_requests.deleted = 0
WHERE
	cv_server_rebuilds.deleted = 0
	AND cv_server_rebuilds.serverid = @serverid
	AND cv_server_rebuilds.submitted IS NOT NULL
	AND cv_server_rebuilds.completed IS NULL
GO

create procedure [dbo].[pr_updateServerRebuilds]
	@requestid int,
	@serviceid int,
	@number int,
	@serverid int,
	@change varchar(20)
AS
UPDATE
	cv_server_rebuilds
SET
	serverid = @serverid,
	change = @change
WHERE
	deleted = 0
	AND requestid = @requestid
	AND serviceid = @serviceid
	AND number = @number
GO

create procedure [dbo].[pr_updateServerRebuildCompleted]
	@serverid int,
	@completed datetime
AS
UPDATE
	cv_server_rebuilds
SET
	completed = @completed
WHERE
	deleted = 0
	AND serverid = @serverid
	AND submitted IS NOT NULL
	AND completed IS NULL
GO

ALTER PROCEDURE [dbo].[pr_getStorageLunDisks]
	@answerid int
AS
SELECT
	*
FROM
	cv_storage_luns_disks
		INNER JOIN
			cv_storage_luns
		ON
			cv_storage_luns_disks.lunid = cv_storage_luns.id
			AND cv_storage_luns.deleted = 0
WHERE
	cv_storage_luns_disks.deleted = 0
	AND cv_storage_luns.answerid = @answerid
GO

create procedure [dbo].[pr_updateStorageLunInstance]
	@id int,
	@instanceid int
AS
UPDATE
	cv_storage_luns
SET
	instanceid = @instanceid,
	modified = getdate()
WHERE
	id = @id
GO

ALTER PROCEDURE [dbo].[pr_addServer]
	@requestid int,
	@answerid int,
	@modelid int,
	@csmconfigid int,
	@clusterid int,
	@number int,
	@osid int,
	@spid int,
	@templateid int,
	@domainid int,
	@test_domainid int,
	@infrastructure int,
	@ha int,
	@dr int,
	@dr_exist int,
	@dr_name varchar(30),
	@dr_consistency int,
	@dr_consistencyid int,
	@configured int,
	@local_storage int,
	@accounts int,
	@fdrive int,
	@dba int,
	@pnc int,
	@vmware_clusterid int,
	@dns_auto int,
	@id int output
AS
INSERT INTO
	cv_servers
VALUES
(
	@requestid,
	@answerid,
	@modelid,
	@csmconfigid,
	@clusterid,
	@number,
	@osid,
	@spid,
	@templateid,
	@domainid,
	@test_domainid,
	@infrastructure,
	@ha,
	@dr,
	@dr_exist,
	@dr_name,
	@dr_consistency,
	@dr_consistencyid,
	@configured,
	@local_storage,
	@accounts,
	@fdrive,
	@dba,
	@pnc,
	@vmware_clusterid,
	0, -- nameid
	'',-- dhcp
	null,	-- mhs
	0, -- zeus_error
	0, -- step
	0, -- step_skip_start
	0, -- step_skip_goto
	0, -- tsm_schedule
	0, -- tsm_cloptset
	'',
	'',
	null,
	0, -- tsm_bypass
	null, -- tsm_registered
	@dns_auto,
	null,
	null, --build_started
	null, --build_completed
	null, --build_ready
	null, --mis_audits
	null, --rebuild
	0,    --rebuilding
	null, --decommissioned
	0.00,		-- reclaimed_storage
	0.00,		-- reclaimed_backup
	null,		-- reclaimed_amt
	null,		-- reclaimed_tier
	null,		-- reclaimed_environment
	null,		-- reclaimed_storage_precooldown
	null,		-- reclaimed_storage_cooldown
	null,		-- reclaimed_storage_cr2
	null,		-- reclaimed_storage_classification
	null,		-- reclaimed_storage_vendor
	null,		-- reclaimed_storage_location
	null,		-- reclaimed_storage_array
	null,		-- reclaimed_storage_notes
	0,		-- paused
	null,		-- storage_configured
	getdate(),	-- created
	getdate(),	-- modified
	0
)
SET @id = SCOPE_IDENTITY()
GO

ALTER procedure [dbo].[pr_updateLocationAddress]
	@id int,
	@cityid int,
	@name varchar(100),
	@factory_code varchar(5),
	@common int,
	@commonname varchar(50),
	@storage int,
	@tsm int,
	@dr int,
	@offsite_build int,
	@manual_build int,
	@building_code varchar(20),
	@service_now varchar(20),
	@recovery int,
	@vmware_ipaddress int,
	@prod int,
	@qa int,
	@test int,
	@enabled int
AS
UPDATE
	cv_location_address
SET
	cityid = @cityid,
	name = @name,
	factory_code = @factory_code,
	common = @common,
	commonname = @commonname,
	storage = @storage,
	tsm = @tsm,
	dr = @dr,
	offsite_build = @offsite_build,
	manual_build = @manual_build,
	building_code = @building_code,
	service_now = @service_now,
	recovery = @recovery,
	vmware_ipaddress = @vmware_ipaddress,
	prod = @prod,
	qa = @qa,
	test = @test,
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id
GO

ALTER PROCEDURE [dbo].[pr_addLocationAddress]
	@cityid int,
	@name varchar(100),
	@factory_code varchar(5),
	@common int,
	@commonname varchar(50),
	@storage int,
	@tsm int,
	@dr int,
	@offsite_build int,
	@manual_build int,
	@building_code varchar(20),
	@service_now varchar(20),
	@recovery int,
	@vmware_ipaddress int,
	@prod int,
	@qa int,
	@test int,
	@enabled int
AS
INSERT INTO
	cv_location_address
VALUES
(
	@cityid,
	@name,
	@factory_code,
	@common,
	@commonname,
	@storage,
	@tsm,
	@dr,
	@offsite_build,
	@manual_build,
	@building_code,
	@service_now,
	@recovery,
	@vmware_ipaddress,
	@prod,
	@qa,
	@test,
	0,	-- display
	@enabled,
	getdate(),
	getdate(),
	0
)
GO

ALTER PROCEDURE [dbo].[pr_deleteServerIPNicInterface]
	@serveripid int
AS
UPDATE
	cv_servers_ips_nicinterfaces
SET
	deleted = 1,
	modified = getdate()
WHERE
	serveripid = @serveripid
GO

