/*==============================================================
The purpose of this script is to perform any dml statements 
(insert, update, delete)
==============================================================*/

--UPDATE cv_ondemand_steps_done_server set step = 25 where step = 24
--GO
--UPDATE cv_ondemand_steps set step = 25 where step = 24
--GO
--UPDATE cv_servers_errors set step = 25 where step = 24
--GO
--update cv_servers set step = 25 where step = 24
--GO
--INSERT INTO cv_ondemand_steps VALUES (58, 'Configure Cluster', 'Configure Cluster', '', '', '', '', 0, 0, 0, 0, 0, 1, 0, 0, 24, 1, GETDATE(), GETDATE(), 0)
--GO

UPDATE cv_servers SET storage_configured = GETDATE()
GO

