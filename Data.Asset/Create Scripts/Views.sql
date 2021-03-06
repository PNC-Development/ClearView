USE [ClearViewAsset]
GO
/****** Object:  View [dbo].[vw_AssetBlades]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_AssetBlades]
AS
SELECT     TOP (100) 
                      PERCENT CASE WHEN status_blade.status = 10 THEN 'In Use' WHEN status_blade.status = 1 THEN 'In Stock' WHEN status_blade.status = 100 THEN 'Reserved'
                       WHEN status_blade.status = - 10 THEN 'Disposed' END AS status, status_blade.name AS bladename, status_blade.assetid AS bladeasset, 
                      dbo.cva_assets.bad, dbo.cva_assets.serial, enc_blade.name AS enclosure, enc_blade.assetid AS enclosureaset, dbo.cva_blades.slot, 
                      dbo.cva_blades.dummy_name, Clearview.dbo.cv_classs.name AS class, Clearview.dbo.cv_environment.name AS environment, 
                      Clearview.dbo.cv_location_address.name + ' (' + Clearview.dbo.cv_location_city.name + ', ' + Clearview.dbo.cv_location_state.name + ')' AS location
FROM         dbo.cva_blades INNER JOIN
                      dbo.cva_status AS status_blade ON dbo.cva_blades.assetid = status_blade.assetid AND status_blade.deleted = 0 INNER JOIN
                      dbo.cva_enclosures INNER JOIN
                      dbo.cva_status AS enc_blade ON dbo.cva_enclosures.assetid = enc_blade.assetid AND enc_blade.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_classs ON dbo.cva_enclosures.classid = Clearview.dbo.cv_classs.id AND Clearview.dbo.cv_classs.enabled = 1 AND 
                      Clearview.dbo.cv_classs.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_environment ON dbo.cva_enclosures.environmentid = Clearview.dbo.cv_environment.id AND 
                      Clearview.dbo.cv_environment.enabled = 1 AND Clearview.dbo.cv_environment.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_location_address INNER JOIN
                      Clearview.dbo.cv_location_city INNER JOIN
                      Clearview.dbo.cv_location_state ON Clearview.dbo.cv_location_city.stateid = Clearview.dbo.cv_location_state.id AND 
                      Clearview.dbo.cv_location_state.enabled = 1 AND Clearview.dbo.cv_location_state.deleted = 0 ON 
                      Clearview.dbo.cv_location_address.cityid = Clearview.dbo.cv_location_city.id AND Clearview.dbo.cv_location_city.enabled = 1 AND 
                      Clearview.dbo.cv_location_city.deleted = 0 ON dbo.cva_enclosures.addressid = Clearview.dbo.cv_location_address.id AND 
                      Clearview.dbo.cv_location_address.enabled = 1 AND Clearview.dbo.cv_location_address.deleted = 0 ON 
                      dbo.cva_blades.enclosureid = dbo.cva_enclosures.assetid AND dbo.cva_enclosures.deleted = 0 INNER JOIN
                      dbo.cva_assets ON dbo.cva_blades.assetid = dbo.cva_assets.id AND dbo.cva_assets.deleted = 0
WHERE     (dbo.cva_blades.deleted = 0)
ORDER BY enclosure, dbo.cva_blades.slot

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cva_blades"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "status_blade"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cva_enclosures"
            Begin Extent = 
               Top = 6
               Left = 416
               Bottom = 114
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "enc_blade"
            Begin Extent = 
               Top = 6
               Left = 605
               Bottom = 114
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_classs (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_environment (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 227
               Bottom = 222
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_address (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 416
               Bottom ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetBlades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 222
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_city (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 605
               Bottom = 222
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_state (Clearview.dbo)"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cva_assets"
            Begin Extent = 
               Top = 222
               Left = 227
               Bottom = 330
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetBlades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetBlades'
GO
/****** Object:  View [dbo].[vw_Assets]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Assets]
AS
SELECT    ClearView.dbo.cv_models_property.ID AS modelid, ClearView.dbo.cv_models_property.name AS model, ClearView.dbo.cv_types.name AS type, 
	cva_status.status as StatusID,
                      CASE WHEN cva_status.status = 0 THEN 'Arrived' WHEN cva_status.status = 10 THEN 'In Use' WHEN cva_status.status = 1 THEN 'In Stock' WHEN cva_status.status
                       = 100 THEN 'Reserved' WHEN cva_status.status = - 10 THEN 'Disposed' END AS status, dbo.cva_status.name, dbo.cva_status.assetid, 
                      dbo.cva_assets.bad, dbo.cva_assets.serial, dbo.cva_assets.asset, dbo.cva_blades.dummy_name, dbo.cva_blades.ilo, 
                      ClearView.dbo.cv_rooms.name AS room, ClearView.dbo.cv_racks.name AS rack, dbo.cva_enclosures.rackposition, 
                      cva_enclosures.classid,ClearView.dbo.cv_classs.name AS class, ClearView.dbo.cv_environment.name AS environment, ClearView.dbo.cv_location_address.id AS locationid, 
                      ClearView.dbo.cv_location_address.name + ' (' + ClearView.dbo.cv_location_city.name + ', ' + ClearView.dbo.cv_location_state.name + ')' AS location, 
                      ClearView.dbo.cv_operating_systems.name AS os, CAST(ClearViewIP.dbo.cv_ip_addresses.add1 AS varchar(3)) 
                      + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add3 AS varchar(3)) 
                      + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add4 AS varchar(3)) AS ipaddress, ClearViewIP.dbo.cv_ip_vlans.vlan, 
                      ClearView.dbo.cv_domains.name AS domain, cv_domains_test.name AS domainTEST, ClearView.dbo.cv_forecast_answers.appcode, 
                      ClearView.dbo.cv_forecast_answers.appname, ClearView.dbo.cv_forecast_answers.[backup] AS tsm, 
                      ClearView.dbo.cv_forecast_answers_backup.start_date AS tsm_start_date, 
                      ClearView.dbo.cv_forecast_answers_backup.time_hour + ' ' + ClearView.dbo.cv_forecast_answers_backup.time_switch AS tsm_start_time, 
                      cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner, cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary, 
                      cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary, 
                      CASE WHEN cv_models_property.fabric = 0 THEN 'Cisco' WHEN cv_models_property.fabric = 1 THEN 'Brocade' ELSE 'Unknown' END AS fabric,

					  isnull(ClearView.dbo.cv_forecast_answers.quantity,0) as quantity,
					  ClearView.dbo.cv_forecast_answers.executed,
					  ClearView.dbo.cv_forecast_answers.completed,
					  ClearView.dbo.cv_forecast_answers.confidenceid,
					  (Select Name from ClearView.dbo.cv_confidence
					   WHERE ClearView.dbo.cv_confidence.id =ClearView.dbo.cv_forecast_answers.confidenceid) as ConfidenceName
					  ,CASE WHEN cv_models_property.type_blade =1 then 'Blade'
					    WHEN cv_models_property.type_physical =1 then 'Rack'
						WHEN cv_models_property.type_vmware =1 then 'Virtual'
					   END as TypeCatagory
						
FROM         dbo.cva_blades INNER JOIN
                      dbo.cva_status LEFT OUTER JOIN
                      ClearView.dbo.cv_servers_assets INNER JOIN
                      ClearView.dbo.cv_servers INNER JOIN
                      ClearView.dbo.cv_operating_systems ON ClearView.dbo.cv_servers.osid = ClearView.dbo.cv_operating_systems.id AND 
                      ClearView.dbo.cv_operating_systems.deleted = 0 INNER JOIN
                      ClearView.dbo.cv_domains ON ClearView.dbo.cv_servers.domainid = ClearView.dbo.cv_domains.id AND 
                      ClearView.dbo.cv_domains.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_domains AS cv_domains_test ON ClearView.dbo.cv_servers.test_domainid = cv_domains_test.id AND 
                      cv_domains_test.deleted = 0 INNER JOIN
                      ClearView.dbo.cv_forecast_answers LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_owner ON ClearView.dbo.cv_forecast_answers.appcontact = cv_users_owner.userid AND 
                      cv_users_owner.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_primary ON ClearView.dbo.cv_forecast_answers.admin1 = cv_users_primary.userid AND 
                      cv_users_primary.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_secondary ON ClearView.dbo.cv_forecast_answers.admin2 = cv_users_secondary.userid AND 
                      cv_users_secondary.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_forecast_answers_backup ON ClearView.dbo.cv_forecast_answers.id = ClearView.dbo.cv_forecast_answers_backup.answerid AND 
                      ClearView.dbo.cv_forecast_answers_backup.deleted = 0 ON ClearView.dbo.cv_servers.answerid = ClearView.dbo.cv_forecast_answers.id AND 
                      ClearView.dbo.cv_forecast_answers.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_servers_ips INNER JOIN
                      ClearViewIP.dbo.cv_ip_addresses INNER JOIN
                      ClearViewIP.dbo.cv_ip_networks INNER JOIN
                      ClearViewIP.dbo.cv_ip_vlans ON ClearViewIP.dbo.cv_ip_networks.vlanid = ClearViewIP.dbo.cv_ip_vlans.id AND 
                      ClearViewIP.dbo.cv_ip_vlans.deleted = 0 ON ClearViewIP.dbo.cv_ip_addresses.networkid = ClearViewIP.dbo.cv_ip_networks.id AND 
                      ClearViewIP.dbo.cv_ip_networks.deleted = 0 ON ClearView.dbo.cv_servers_ips.ipaddressid = ClearViewIP.dbo.cv_ip_addresses.id AND 
                      ClearViewIP.dbo.cv_ip_addresses.deleted = 0 ON ClearView.dbo.cv_servers.id = ClearView.dbo.cv_servers_ips.serverid AND 
                      ClearView.dbo.cv_servers_ips.deleted = 0 ON ClearView.dbo.cv_servers_assets.serverid = ClearView.dbo.cv_servers.id AND 
                      ClearView.dbo.cv_servers.deleted = 0 ON dbo.cva_status.assetid = ClearView.dbo.cv_servers_assets.assetid AND 
                      ClearView.dbo.cv_servers_assets.latest = 1 AND ClearView.dbo.cv_servers_assets.deleted = 0 ON 
                      dbo.cva_blades.assetid = dbo.cva_status.assetid AND dbo.cva_status.deleted = 0 INNER JOIN
                      dbo.cva_enclosures LEFT OUTER JOIN
                      ClearView.dbo.cv_classs ON dbo.cva_enclosures.classid = ClearView.dbo.cv_classs.id AND ClearView.dbo.cv_classs.enabled = 1 AND 
                      ClearView.dbo.cv_classs.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_environment ON dbo.cva_enclosures.environmentid = ClearView.dbo.cv_environment.id AND 
                      ClearView.dbo.cv_environment.enabled = 1 AND ClearView.dbo.cv_environment.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_location_address INNER JOIN
                      ClearView.dbo.cv_location_city INNER JOIN
                      ClearView.dbo.cv_location_state ON ClearView.dbo.cv_location_city.stateid = ClearView.dbo.cv_location_state.id AND 
                      ClearView.dbo.cv_location_state.enabled = 1 AND ClearView.dbo.cv_location_state.deleted = 0 ON 
                      ClearView.dbo.cv_location_address.cityid = ClearView.dbo.cv_location_city.id AND ClearView.dbo.cv_location_city.enabled = 1 AND 
                      ClearView.dbo.cv_location_city.deleted = 0 ON dbo.cva_enclosures.addressid = ClearView.dbo.cv_location_address.id AND 
                      ClearView.dbo.cv_location_address.enabled = 1 AND ClearView.dbo.cv_location_address.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_rooms ON dbo.cva_enclosures.roomid = ClearView.dbo.cv_rooms.id AND ClearView.dbo.cv_rooms.enabled = 1 AND 
                      ClearView.dbo.cv_rooms.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_racks ON dbo.cva_enclosures.rackid = ClearView.dbo.cv_racks.id AND ClearView.dbo.cv_racks.enabled = 1 AND 
                      ClearView.dbo.cv_racks.deleted = 0 ON dbo.cva_blades.enclosureid = dbo.cva_enclosures.assetid AND dbo.cva_enclosures.deleted = 0 INNER JOIN
                      dbo.cva_assets INNER JOIN
                      ClearView.dbo.cv_models_property INNER JOIN
                      ClearView.dbo.cv_models INNER JOIN
                      ClearView.dbo.cv_types INNER JOIN
                      ClearView.dbo.cv_platforms ON ClearView.dbo.cv_types.platformid = ClearView.dbo.cv_platforms.platformid AND 
                      ClearView.dbo.cv_platforms.enabled = 1 AND ClearView.dbo.cv_platforms.deleted = 0 ON 
                      ClearView.dbo.cv_models.typeid = ClearView.dbo.cv_types.id AND ClearView.dbo.cv_types.enabled = 1 AND ClearView.dbo.cv_types.deleted = 0 ON 
                      ClearView.dbo.cv_models_property.modelid = ClearView.dbo.cv_models.id AND ClearView.dbo.cv_models.enabled = 1 AND 
                      ClearView.dbo.cv_models.deleted = 0 ON dbo.cva_assets.modelid = ClearView.dbo.cv_models_property.id AND 
                      ClearView.dbo.cv_models_property.deleted = 0 ON dbo.cva_blades.assetid = dbo.cva_assets.id AND dbo.cva_assets.deleted = 0
WHERE     (dbo.cva_blades.deleted = 0)
UNION ALL
SELECT     cv_models_property_1.ID AS modelid,cv_models_property_1.name AS model, cv_types_2.name AS type, 
cva_status_1.status as StatusID,
                      CASE WHEN cva_status_1.status = 0 THEN 'Arrived' WHEN cva_status_1.status = 10 THEN 'In Use' WHEN cva_status_1.status = 1 THEN 'In Stock' WHEN
                       cva_status_1.status = 100 THEN 'Reserved' WHEN cva_status_1.status = - 10 THEN 'Disposed' END AS status, cva_status_1.name, 
                      cva_status_1.assetid, cva_assets_2.bad, cva_assets_2.serial, cva_assets_2.asset, dbo.cva_server.dummy_name, dbo.cva_server.ilo, 
                      cv_rooms_2.name AS room, cv_racks_2.name AS rack, dbo.cva_server.rackposition,cva_server.classid, cv_classs_2.name AS class, 
                      cv_environment_2.name AS environment, cv_location_address_2.id AS locationid, 
                      cv_location_address_2.name + ' (' + cv_location_city_2.name + ', ' + cv_location_state_2.name + ')' AS location, cv_operating_systems_1.name AS os, 
                      CAST(cv_ip_addresses_1.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses_1.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses_1.add3 AS varchar(3)) 
                      + '.' + CAST(cv_ip_addresses_1.add4 AS varchar(3)) AS ipaddress, cv_ip_vlans_1.vlan, cv_domains_1.name AS domain, 
                      cv_domains_test.name AS domainTEST, cv_forecast_answers_1.appcode, cv_forecast_answers_1.appname, cv_forecast_answers_1.[backup] AS tsm, 
                      cv_forecast_answers_backup_1.start_date AS tsm_start_date, 
                      cv_forecast_answers_backup_1.time_hour + ' ' + cv_forecast_answers_backup_1.time_switch AS tsm_start_time, 
                      cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner, cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary, 
                      cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary, 
                      CASE WHEN cv_models_property_1.fabric = 0 THEN 'Cisco' WHEN cv_models_property_1.fabric = 1 THEN 'Brocade' ELSE 'Unknown' END AS fabric,
					  isnull(cv_forecast_answers_1.quantity,0) as quantity,
					  cv_forecast_answers_1.executed,
					  cv_forecast_answers_1.completed,
					  cv_forecast_answers_1.confidenceid,
					  (Select Name from ClearView.dbo.cv_confidence
					   WHERE ClearView.dbo.cv_confidence.id =cv_forecast_answers_1.confidenceid) as ConfidenceName
					  ,CASE WHEN cv_models_property_1.type_blade =1 then 'Blade'
					    WHEN cv_models_property_1.type_physical =1 then 'Rack'
						WHEN cv_models_property_1.type_vmware =1 then 'Virtual'
					   END as TypeCatagory
FROM         dbo.cva_server INNER JOIN
                      dbo.cva_status AS cva_status_1 LEFT OUTER JOIN
                      ClearView.dbo.cv_servers_assets AS cv_servers_assets_1 INNER JOIN
                      ClearView.dbo.cv_servers AS cv_servers_1 INNER JOIN
                      ClearView.dbo.cv_operating_systems AS cv_operating_systems_1 ON cv_servers_1.osid = cv_operating_systems_1.id AND 
                      cv_operating_systems_1.deleted = 0 INNER JOIN
                      ClearView.dbo.cv_domains AS cv_domains_1 ON cv_servers_1.domainid = cv_domains_1.id AND cv_domains_1.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_domains AS cv_domains_test ON cv_servers_1.test_domainid = cv_domains_test.id AND cv_domains_test.deleted = 0 INNER JOIN
                      ClearView.dbo.cv_forecast_answers AS cv_forecast_answers_1 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_owner ON cv_forecast_answers_1.appcontact = cv_users_owner.userid AND 
                      cv_users_owner.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_primary ON cv_forecast_answers_1.admin1 = cv_users_primary.userid AND 
                      cv_users_primary.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_secondary ON cv_forecast_answers_1.admin2 = cv_users_secondary.userid AND 
                      cv_users_secondary.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_forecast_answers_backup AS cv_forecast_answers_backup_1 ON 
                      cv_forecast_answers_1.id = cv_forecast_answers_backup_1.answerid AND cv_forecast_answers_backup_1.deleted = 0 ON 
                      cv_servers_1.answerid = cv_forecast_answers_1.id AND cv_forecast_answers_1.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_servers_ips AS cv_servers_ips_1 INNER JOIN
                      ClearViewIP.dbo.cv_ip_addresses AS cv_ip_addresses_1 INNER JOIN
                      ClearViewIP.dbo.cv_ip_networks AS cv_ip_networks_1 INNER JOIN
                      ClearViewIP.dbo.cv_ip_vlans AS cv_ip_vlans_1 ON cv_ip_networks_1.vlanid = cv_ip_vlans_1.id AND cv_ip_vlans_1.deleted = 0 ON 
                      cv_ip_addresses_1.networkid = cv_ip_networks_1.id AND cv_ip_networks_1.deleted = 0 ON 
                      cv_servers_ips_1.ipaddressid = cv_ip_addresses_1.id AND cv_ip_addresses_1.deleted = 0 ON cv_servers_1.id = cv_servers_ips_1.serverid AND 
                      cv_servers_ips_1.deleted = 0 ON cv_servers_assets_1.serverid = cv_servers_1.id AND cv_servers_1.deleted = 0 ON 
                      cva_status_1.assetid = cv_servers_assets_1.assetid AND cv_servers_assets_1.latest = 1 AND cv_servers_assets_1.deleted = 0 ON 
                      dbo.cva_server.assetid = cva_status_1.assetid AND cva_status_1.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_classs AS cv_classs_2 ON dbo.cva_server.classid = cv_classs_2.id AND cv_classs_2.enabled = 1 AND 
                      cv_classs_2.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_environment AS cv_environment_2 ON dbo.cva_server.environmentid = cv_environment_2.id AND 
                      cv_environment_2.enabled = 1 AND cv_environment_2.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_location_address AS cv_location_address_2 INNER JOIN
                      ClearView.dbo.cv_location_city AS cv_location_city_2 INNER JOIN
                      ClearView.dbo.cv_location_state AS cv_location_state_2 ON cv_location_city_2.stateid = cv_location_state_2.id AND 
                      cv_location_state_2.enabled = 1 AND cv_location_state_2.deleted = 0 ON cv_location_address_2.cityid = cv_location_city_2.id AND 
                      cv_location_city_2.enabled = 1 AND cv_location_city_2.deleted = 0 ON dbo.cva_server.addressid = cv_location_address_2.id AND 
                      cv_location_address_2.enabled = 1 AND cv_location_address_2.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_rooms AS cv_rooms_2 ON dbo.cva_server.roomid = cv_rooms_2.id AND cv_rooms_2.enabled = 1 AND 
                      cv_rooms_2.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_racks AS cv_racks_2 ON dbo.cva_server.rackid = cv_racks_2.id AND cv_racks_2.enabled = 1 AND 
                      cv_racks_2.deleted = 0 INNER JOIN
                      dbo.cva_assets AS cva_assets_2 INNER JOIN
                      ClearView.dbo.cv_models_property AS cv_models_property_1 INNER JOIN
                      ClearView.dbo.cv_models AS cv_models_2 INNER JOIN
                      ClearView.dbo.cv_types AS cv_types_2 INNER JOIN
                      ClearView.dbo.cv_platforms AS cv_platforms_2 ON cv_types_2.platformid = cv_platforms_2.platformid AND cv_platforms_2.enabled = 1 AND 
                      cv_platforms_2.deleted = 0 ON cv_models_2.typeid = cv_types_2.id AND cv_types_2.enabled = 1 AND cv_types_2.deleted = 0 ON 
                      cv_models_property_1.modelid = cv_models_2.id AND cv_models_2.enabled = 1 AND cv_models_2.deleted = 0 ON 
                      cva_assets_2.modelid = cv_models_property_1.id AND cv_models_property_1.deleted = 0 ON dbo.cva_server.assetid = cva_assets_2.id AND 
                      cva_assets_2.deleted = 0
WHERE     (dbo.cva_server.deleted = 0)
UNION ALL
SELECT     cv_models_property_2.ID AS modelid,cv_models_property_2.name AS model, cv_types_3.name AS type, 
cva_status_2.status as StatusID,
                      CASE WHEN cva_status_2.status = 0 THEN 'Arrived' WHEN cva_status_2.status = 10 THEN 'In Use' WHEN cva_status_2.status = 1 THEN 'In Stock' WHEN
                       cva_status_2.status = 100 THEN 'Reserved' WHEN cva_status_2.status = - 10 THEN 'Disposed' END AS status, cva_status_2.name, 
                      cva_status_2.assetid, cva_assets_3.bad, cva_assets_3.serial, cva_assets_3.asset, '' AS dummy_name, '' AS ilo, 
                      '' AS room, '' AS rack, '' AS rackposition, cva_guests.classid ,cv_classs_3.name AS class, 
                      cv_environment_3.name AS environment, cv_location_address_3.id AS locationid, 
                      cv_location_address_3.name + ' (' + cv_location_city_3.name + ', ' + cv_location_state_3.name + ')' AS location, cv_operating_systems_2.name AS os, 
                      CAST(cv_ip_addresses_2.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses_2.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses_2.add3 AS varchar(3)) 
                      + '.' + CAST(cv_ip_addresses_2.add4 AS varchar(3)) AS ipaddress, cv_ip_vlans_2.vlan, cv_domains_2.name AS domain, 
                      cv_domains_test.name AS domainTEST, cv_forecast_answers_2.appcode, cv_forecast_answers_2.appname, cv_forecast_answers_2.[backup] AS tsm, 
                      cv_forecast_answers_backup_2.start_date AS tsm_start_date, 
                      cv_forecast_answers_backup_2.time_hour + ' ' + cv_forecast_answers_backup_2.time_switch AS tsm_start_time, 
                      cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner, cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary, 
                      cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary, 
                      CASE WHEN cv_models_property_2.fabric = 0 THEN 'Cisco' WHEN cv_models_property_2.fabric = 1 THEN 'Brocade' ELSE 'Unknown' END AS fabric,
					  isnull(cv_forecast_answers_2.quantity,0) as quantity,
					  cv_forecast_answers_2.executed,
					  cv_forecast_answers_2.completed,
					  cv_forecast_answers_2.confidenceid,
					  (Select Name from ClearView.dbo.cv_confidence
					   WHERE ClearView.dbo.cv_confidence.id =cv_forecast_answers_2.confidenceid) as ConfidenceName
					  ,CASE WHEN cv_models_property_2.type_blade =1 then 'Blade'
					    WHEN cv_models_property_2.type_physical =1 then 'Rack'
						WHEN cv_models_property_2.type_vmware =1 then 'Virtual'
					   END as TypeCatagory
FROM         dbo.cva_guests INNER JOIN
                      dbo.cva_status AS cva_status_2 
LEFT OUTER JOIN
                      ClearView.dbo.cv_servers_assets AS cv_servers_assets_2 INNER JOIN
                      ClearView.dbo.cv_servers AS cv_servers_2 INNER JOIN
                      ClearView.dbo.cv_operating_systems AS cv_operating_systems_2 ON cv_servers_2.osid = cv_operating_systems_2.id AND 
                      cv_operating_systems_2.deleted = 0 INNER JOIN
                      ClearView.dbo.cv_domains AS cv_domains_2 ON cv_servers_2.domainid = cv_domains_2.id AND cv_domains_2.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_domains AS cv_domains_test ON cv_servers_2.test_domainid = cv_domains_test.id AND cv_domains_test.deleted = 0 INNER JOIN
                      ClearView.dbo.cv_forecast_answers AS cv_forecast_answers_2 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_owner ON cv_forecast_answers_2.appcontact = cv_users_owner.userid AND 
                      cv_users_owner.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_primary ON cv_forecast_answers_2.admin1 = cv_users_primary.userid AND 
                      cv_users_primary.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_users AS cv_users_secondary ON cv_forecast_answers_2.admin2 = cv_users_secondary.userid AND 
                      cv_users_secondary.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_forecast_answers_backup AS cv_forecast_answers_backup_2 ON 
                      cv_forecast_answers_2.id = cv_forecast_answers_backup_2.answerid AND cv_forecast_answers_backup_2.deleted = 0 ON 
                      cv_servers_2.answerid = cv_forecast_answers_2.id AND cv_forecast_answers_2.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_servers_ips AS cv_servers_ips_2 INNER JOIN
                      ClearViewIP.dbo.cv_ip_addresses AS cv_ip_addresses_2 INNER JOIN
                      ClearViewIP.dbo.cv_ip_networks AS cv_ip_networks_2 INNER JOIN
                      ClearViewIP.dbo.cv_ip_vlans AS cv_ip_vlans_2 ON cv_ip_networks_2.vlanid = cv_ip_vlans_2.id AND cv_ip_vlans_2.deleted = 0 ON 
                      cv_ip_addresses_2.networkid = cv_ip_networks_2.id AND cv_ip_networks_2.deleted = 0 ON 
                      cv_servers_ips_2.ipaddressid = cv_ip_addresses_2.id AND cv_ip_addresses_2.deleted = 0 ON cv_servers_2.id = cv_servers_ips_2.serverid AND 
                      cv_servers_ips_2.deleted = 0 ON cv_servers_assets_2.serverid = cv_servers_2.id AND cv_servers_2.deleted = 0 
ON 
                      cva_status_2.assetid = cv_servers_assets_2.assetid AND cv_servers_assets_2.latest = 1 AND cv_servers_assets_2.deleted = 0 ON 
                      dbo.cva_guests.assetid = cva_status_2.assetid AND cva_status_2.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_classs AS cv_classs_3 ON dbo.cva_guests.classid = cv_classs_3.id AND cv_classs_3.enabled = 1 AND 
                      cv_classs_3.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_environment AS cv_environment_3 ON dbo.cva_guests.environmentid = cv_environment_3.id AND 
                      cv_environment_3.enabled = 1 AND cv_environment_3.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_location_address AS cv_location_address_3 INNER JOIN
                      ClearView.dbo.cv_location_city AS cv_location_city_3 INNER JOIN
                      ClearView.dbo.cv_location_state AS cv_location_state_3 ON cv_location_city_3.stateid = cv_location_state_3.id AND 
                      cv_location_state_3.enabled = 1 AND cv_location_state_3.deleted = 0 ON cv_location_address_3.cityid = cv_location_city_3.id AND 
                      cv_location_city_3.enabled = 1 AND cv_location_city_3.deleted = 0 ON dbo.cva_guests.addressid = cv_location_address_3.id AND 
                      cv_location_address_3.enabled = 1 AND cv_location_address_3.deleted = 0 INNER JOIN
                      dbo.cva_assets AS cva_assets_3 INNER JOIN
                      ClearView.dbo.cv_models_property AS cv_models_property_2 INNER JOIN
                      ClearView.dbo.cv_models AS cv_models_3 INNER JOIN
                      ClearView.dbo.cv_types AS cv_types_3 INNER JOIN
                      ClearView.dbo.cv_platforms AS cv_platforms_3 ON cv_types_3.platformid = cv_platforms_3.platformid AND cv_platforms_3.enabled = 1 AND 
                      cv_platforms_3.deleted = 0 ON cv_models_3.typeid = cv_types_3.id AND cv_types_3.enabled = 1 AND cv_types_3.deleted = 0 ON 
                      cv_models_property_2.modelid = cv_models_3.id AND cv_models_3.enabled = 1 AND cv_models_3.deleted = 0 ON 
                      cva_assets_3.modelid = cv_models_property_2.id AND cv_models_property_2.deleted = 0 ON dbo.cva_guests.assetid = cva_assets_3.id AND 
                      cva_assets_3.deleted = 0
WHERE     (dbo.cva_guests.deleted = 0)

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[31] 2[2] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 3
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 32
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 5
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Assets'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Assets'
GO
/****** Object:  View [dbo].[vw_AssetsDR]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_AssetsDR]
AS
SELECT     cv_models_property.name AS model, cv_types.name AS type, 
                      CASE WHEN cva_status.status = 10 THEN 'In Use' WHEN cva_status.status = 1 THEN 'In Stock' WHEN cva_status.status = 100 THEN 'Reserved' WHEN
                       cva_status.status = - 10 THEN 'Disposed' END AS status, cva_status.name AS name, cva_status.assetid AS assetid, cva_assets.bad, 
                      cva_assets.serial AS serial, cva_assets.asset AS asset, cva_blades.dummy_name, cva_blades.ilo, cv_rooms.name AS room, cv_racks.name AS rack, 
                      cva_enclosures.rackposition, cv_classs.name AS class, cv_environment.name AS environment, cv_location_address.id AS locationid, 
                      cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location, cv_operating_systems.name AS os, 
                      CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) 
                      + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress, cv_ip_vlans.vlan, cv_domains.name AS domain, 
                      cv_domains_test.name AS domainTEST, cv_forecast_answers.appcode, cv_forecast_answers.appname, cv_forecast_answers.[backup] AS tsm, 
                      cv_forecast_answers_backup.start_date AS tsm_start_date, 
                      cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time, 
                      cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner, cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary, 
                      cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary, 
                      CASE WHEN cv_models_property.fabric = 0 THEN 'Cisco' WHEN cv_models_property.fabric = 1 THEN 'Brocade' ELSE 'Unknown' END AS fabric, 
                      cva_hba_1.name AS WWWN1, cva_hba_2.name AS WWWN2, cv_servers.fdrive AS FdriveOnSAN, 
                      cv_users_lead.fname + ' ' + cv_users_lead.lname AS manager, cv_users_engineer.fname + ' ' + cv_users_engineer.lname AS engineer, 
                      cv_luns.luns + cv_mount_points.luns AS luns, cv_lun_drives.drives AS drives, cv_lun_drive_letters.drives AS driveLetters
FROM         clearview.dbo.cv_servers INNER JOIN
                      clearview.dbo.cv_servers_assets INNER JOIN
                      cva_status INNER JOIN
                      cva_blades INNER JOIN
                      cva_enclosures LEFT OUTER JOIN
                      clearview.dbo.cv_classs ON cva_enclosures.classid = cv_classs.id AND cv_classs.enabled = 1 AND cv_classs.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_environment ON cva_enclosures.environmentid = cv_environment.id AND cv_environment.enabled = 1 AND 
                      cv_environment.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_location_address INNER JOIN
                      clearview.dbo.cv_location_city INNER JOIN
                      clearview.dbo.cv_location_state ON cv_location_city.stateid = cv_location_state.id AND cv_location_state.enabled = 1 AND 
                      cv_location_state.deleted = 0 ON cv_location_address.cityid = cv_location_city.id AND cv_location_city.enabled = 1 AND 
                      cv_location_city.deleted = 0 ON cva_enclosures.addressid = cv_location_address.id AND cv_location_address.enabled = 1 AND 
                      cv_location_address.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_rooms ON cva_enclosures.roomid = cv_rooms.id AND cv_rooms.enabled = 1 AND cv_rooms.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_racks ON cva_enclosures.rackid = cv_racks.id AND cv_racks.enabled = 1 AND cv_racks.deleted = 0 ON 
                      cva_blades.enclosureid = cva_enclosures.assetid AND cva_enclosures.deleted = 0 INNER JOIN
                      cva_assets INNER JOIN
                      clearview.dbo.cv_models_property INNER JOIN
                      clearview.dbo.cv_models INNER JOIN
                      clearview.dbo.cv_types INNER JOIN
                      clearview.dbo.cv_platforms ON cv_types.platformid = cv_platforms.platformid AND cv_platforms.enabled = 1 AND cv_platforms.deleted = 0 ON 
                      cv_models.typeid = cv_types.id AND cv_types.enabled = 1 AND cv_types.deleted = 0 ON cv_models_property.modelid = cv_models.id AND 
                      cv_models.enabled = 1 AND cv_models.deleted = 0 ON cva_assets.modelid = cv_models_property.id AND cv_models_property.deleted = 0 OUTER 
                      APPLY
                          (SELECT     TOP 1 *
                            FROM          cva_hba
                            WHERE      cva_hba.assetid = cva_assets.id
                            ORDER BY modified ASC) AS cva_hba_1 OUTER APPLY
                          (SELECT     TOP 1 *
                            FROM          cva_hba
                            WHERE      cva_hba.assetid = cva_assets.id
                            ORDER BY modified DESC) AS cva_hba_2 ON cva_blades.assetid = cva_assets.id AND cva_assets.deleted = 0 ON 
                      cva_blades.assetid = cva_status.assetid AND cva_status.deleted = 0 ON cva_status.assetid = cv_servers_assets.assetid AND 
                      cva_status.status = 10 AND cva_status.deleted = 0 ON cv_servers_assets.serverid = cv_servers.id AND cv_servers_assets.dr = 1 AND 
                      cv_servers_assets.deleted = 0 INNER JOIN
                      clearview.dbo.cv_operating_systems ON cv_servers.osid = cv_operating_systems.id AND cv_operating_systems.deleted = 0 INNER JOIN
                      clearview.dbo.cv_domains ON cv_servers.domainid = cv_domains.id AND cv_domains.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_domains AS cv_domains_test ON cv_servers.test_domainid = cv_domains_test.id AND cv_domains_test.deleted = 0 INNER JOIN
                      clearview.dbo.cv_forecast_answers INNER JOIN
                      clearview.dbo.cv_forecast INNER JOIN
                      clearview.dbo.cv_requests INNER JOIN
                      clearview.dbo.cv_projects LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_engineer ON cv_projects.engineer = cv_users_engineer.userid AND 
                      cv_users_engineer.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_lead ON cv_projects.lead = cv_users_lead.userid AND cv_users_lead.deleted = 0 ON 
                      cv_requests.projectid = cv_projects.projectid AND cv_projects.deleted = 0 ON cv_forecast.requestid = cv_requests.requestid AND 
                      cv_requests.deleted = 0 ON cv_forecast_answers.forecastid = cv_forecast.id AND cv_forecast.deleted = 0 OUTER APPLY
                          (SELECT     clearview.dbo.getDriveLetters(cv_forecast_answers.id) AS drives) AS cv_lun_drive_letters OUTER APPLY
                          (SELECT DISTINCT COUNT(cv_storage_luns.driveid) AS drives
                            FROM          clearview.dbo.cv_storage_luns
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_lun_drives OUTER APPLY
                          (SELECT     COUNT(cv_storage_luns.id) AS luns
                            FROM          clearview.dbo.cv_storage_luns
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_luns OUTER APPLY
                          (SELECT     COUNT(cv_storage_mount_points.id) AS luns
                            FROM          clearview.dbo.cv_storage_luns INNER JOIN
                                                   clearview.dbo.cv_storage_mount_points ON cv_storage_luns.id = cv_storage_mount_points.lunid AND 
                                                   cv_storage_mount_points.deleted = 0
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_mount_points LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_owner ON cv_forecast_answers.appcontact = cv_users_owner.userid AND 
                      cv_users_owner.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_primary ON cv_forecast_answers.admin1 = cv_users_primary.userid AND 
                      cv_users_primary.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_secondary ON cv_forecast_answers.admin2 = cv_users_secondary.userid AND 
                      cv_users_secondary.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_forecast_answers_backup ON cv_forecast_answers.id = cv_forecast_answers_backup.answerid AND 
                      cv_forecast_answers_backup.deleted = 0 ON cv_servers.answerid = cv_forecast_answers.id AND cv_forecast_answers.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_servers_ips INNER JOIN
                      clearviewip.dbo.cv_ip_addresses INNER JOIN
                      clearviewip.dbo.cv_ip_networks INNER JOIN
                      clearviewip.dbo.cv_ip_vlans ON cv_ip_networks.vlanid = cv_ip_vlans.id AND cv_ip_vlans.deleted = 0 ON 
                      cv_ip_addresses.networkid = cv_ip_networks.id AND cv_ip_networks.deleted = 0 ON cv_servers_ips.ipaddressid = cv_ip_addresses.id AND 
                      cv_ip_addresses.deleted = 0 ON cv_servers.id = cv_servers_ips.serverid AND cv_servers_ips.deleted = 0
WHERE     cv_servers.deleted = 0
UNION ALL
SELECT     cv_models_property.name AS model, cv_types.name AS type, 
                      CASE WHEN cva_status.status = 10 THEN 'In Use' WHEN cva_status.status = 1 THEN 'In Stock' WHEN cva_status.status = 100 THEN 'Reserved' WHEN
                       cva_status.status = - 10 THEN 'Disposed' END AS status, cva_status.name AS name, cva_status.assetid AS assetid, cva_assets.bad, 
                      cva_assets.serial AS serial, cva_assets.asset AS asset, cva_server.dummy_name, cva_server.ilo, cv_rooms.name AS room, cv_racks.name AS rack, 
                      cva_server.rackposition, cv_classs.name AS class, cv_environment.name AS environment, cv_location_address.id AS locationid, 
                      cv_location_address.name + ' (' + cv_location_city.name + ', ' + cv_location_state.name + ')' AS location, cv_operating_systems.name AS os, 
                      CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) 
                      + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress, cv_ip_vlans.vlan, cv_domains.name AS domain, 
                      cv_domains_test.name AS domainTEST, cv_forecast_answers.appcode, cv_forecast_answers.appname, cv_forecast_answers.[backup] AS tsm, 
                      cv_forecast_answers_backup.start_date AS tsm_start_date, 
                      cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time, 
                      cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner, cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary, 
                      cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary, 
                      CASE WHEN cv_models_property.fabric = 0 THEN 'Cisco' WHEN cv_models_property.fabric = 1 THEN 'Brocade' ELSE 'Unknown' END AS fabric, 
                      cva_hba_1.name AS WWWN1, cva_hba_2.name AS WWWN2, cv_servers.fdrive AS FdriveOnSAN, 
                      cv_users_lead.fname + ' ' + cv_users_lead.lname AS manager, cv_users_engineer.fname + ' ' + cv_users_engineer.lname AS engineer, 
                      cv_luns.luns + cv_mount_points.luns AS luns, cv_lun_drives.drives AS drives, cv_lun_drive_letters.drives AS driveLetters
FROM         clearview.dbo.cv_servers INNER JOIN
                      clearview.dbo.cv_servers_assets INNER JOIN
                      cva_status INNER JOIN
                      cva_server LEFT OUTER JOIN
                      clearview.dbo.cv_classs ON cva_server.classid = cv_classs.id AND cv_classs.enabled = 1 AND cv_classs.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_environment ON cva_server.environmentid = cv_environment.id AND cv_environment.enabled = 1 AND 
                      cv_environment.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_location_address INNER JOIN
                      clearview.dbo.cv_location_city INNER JOIN
                      clearview.dbo.cv_location_state ON cv_location_city.stateid = cv_location_state.id AND cv_location_state.enabled = 1 AND 
                      cv_location_state.deleted = 0 ON cv_location_address.cityid = cv_location_city.id AND cv_location_city.enabled = 1 AND 
                      cv_location_city.deleted = 0 ON cva_server.addressid = cv_location_address.id AND cv_location_address.enabled = 1 AND 
                      cv_location_address.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_rooms ON cva_server.roomid = cv_rooms.id AND cv_rooms.enabled = 1 AND cv_rooms.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_racks ON cva_server.rackid = cv_racks.id AND cv_racks.enabled = 1 AND cv_racks.deleted = 0 INNER JOIN
                      cva_assets INNER JOIN
                      clearview.dbo.cv_models_property INNER JOIN
                      clearview.dbo.cv_models INNER JOIN
                      clearview.dbo.cv_types INNER JOIN
                      clearview.dbo.cv_platforms ON cv_types.platformid = cv_platforms.platformid AND cv_platforms.enabled = 1 AND cv_platforms.deleted = 0 ON 
                      cv_models.typeid = cv_types.id AND cv_types.enabled = 1 AND cv_types.deleted = 0 ON cv_models_property.modelid = cv_models.id AND 
                      cv_models.enabled = 1 AND cv_models.deleted = 0 ON cva_assets.modelid = cv_models_property.id AND cv_models_property.deleted = 0 ON 
                      cva_server.assetid = cva_assets.id AND cva_assets.deleted = 0 OUTER APPLY
                          (SELECT     TOP 1 *
                            FROM          cva_hba
                            WHERE      cva_hba.assetid = cva_assets.id
                            ORDER BY modified ASC) AS cva_hba_1 OUTER APPLY
                          (SELECT     TOP 1 *
                            FROM          cva_hba
                            WHERE      cva_hba.assetid = cva_assets.id
                            ORDER BY modified DESC) AS cva_hba_2 ON cva_server.assetid = cva_status.assetid AND cva_server.deleted = 0 ON 
                      cva_status.assetid = cv_servers_assets.assetid AND cva_status.status = 10 AND cva_status.deleted = 0 ON 
                      cv_servers_assets.serverid = cv_servers.id AND cv_servers_assets.dr = 1 AND cv_servers_assets.deleted = 0 INNER JOIN
                      clearview.dbo.cv_operating_systems ON cv_servers.osid = cv_operating_systems.id AND cv_operating_systems.deleted = 0 INNER JOIN
                      clearview.dbo.cv_domains ON cv_servers.domainid = cv_domains.id AND cv_domains.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_domains AS cv_domains_test ON cv_servers.test_domainid = cv_domains_test.id AND cv_domains_test.deleted = 0 INNER JOIN
                      clearview.dbo.cv_forecast_answers INNER JOIN
                      clearview.dbo.cv_forecast INNER JOIN
                      clearview.dbo.cv_requests INNER JOIN
                      clearview.dbo.cv_projects LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_engineer ON cv_projects.engineer = cv_users_engineer.userid AND 
                      cv_users_engineer.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_lead ON cv_projects.lead = cv_users_lead.userid AND cv_users_lead.deleted = 0 ON 
                      cv_requests.projectid = cv_projects.projectid AND cv_projects.deleted = 0 ON cv_forecast.requestid = cv_requests.requestid AND 
                      cv_requests.deleted = 0 ON cv_forecast_answers.forecastid = cv_forecast.id AND cv_forecast.deleted = 0 OUTER APPLY
                          (SELECT     clearview.dbo.getDriveLetters(cv_forecast_answers.id) AS drives) AS cv_lun_drive_letters OUTER APPLY
                          (SELECT DISTINCT COUNT(cv_storage_luns.driveid) AS drives
                            FROM          clearview.dbo.cv_storage_luns
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_lun_drives OUTER APPLY
                          (SELECT     COUNT(cv_storage_luns.id) AS luns
                            FROM          clearview.dbo.cv_storage_luns
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_luns OUTER APPLY
                          (SELECT     COUNT(cv_storage_mount_points.id) AS luns
                            FROM          clearview.dbo.cv_storage_luns INNER JOIN
                                                   clearview.dbo.cv_storage_mount_points ON cv_storage_luns.id = cv_storage_mount_points.lunid AND 
                                                   cv_storage_mount_points.deleted = 0
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_mount_points LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_owner ON cv_forecast_answers.appcontact = cv_users_owner.userid AND 
                      cv_users_owner.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_primary ON cv_forecast_answers.admin1 = cv_users_primary.userid AND 
                      cv_users_primary.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_secondary ON cv_forecast_answers.admin2 = cv_users_secondary.userid AND 
                      cv_users_secondary.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_forecast_answers_backup ON cv_forecast_answers.id = cv_forecast_answers_backup.answerid AND 
                      cv_forecast_answers_backup.deleted = 0 ON cv_servers.answerid = cv_forecast_answers.id AND cv_forecast_answers.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_servers_ips INNER JOIN
                      clearviewip.dbo.cv_ip_addresses INNER JOIN
                      clearviewip.dbo.cv_ip_networks INNER JOIN
                      clearviewip.dbo.cv_ip_vlans ON cv_ip_networks.vlanid = cv_ip_vlans.id AND cv_ip_vlans.deleted = 0 ON 
                      cv_ip_addresses.networkid = cv_ip_networks.id AND cv_ip_networks.deleted = 0 ON cv_servers_ips.ipaddressid = cv_ip_addresses.id AND 
                      cv_ip_addresses.deleted = 0 ON cv_servers.id = cv_servers_ips.serverid AND cv_servers_ips.deleted = 0
WHERE     cv_servers.deleted = 0
UNION ALL
SELECT     cv_models_property.name AS model, cv_types.name AS type, 
                      CASE WHEN cva_status.status = 10 THEN 'In Use' WHEN cva_status.status = 1 THEN 'In Stock' WHEN cva_status.status = 100 THEN 'Reserved' WHEN
                       cva_status.status = - 10 THEN 'Disposed' END AS status, cva_status.name AS name, cva_status.assetid AS assetid, cva_assets.bad, 
                      cva_assets.serial AS serial, cva_assets.asset AS asset, 'VMWARE' AS dummy_name, 'VMWARE' AS ilo, 'VMWARE' AS room, 'VMWARE' AS rack, 
                      'VMWARE' AS rackposition, 'Core' AS class, 'Disaster Recovery' AS environment, 0 AS locationid, '925 Dalton St (Cincinnati, OH)' AS location, 
                      cv_operating_systems.name AS os, CAST(cv_ip_addresses.add1 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add2 AS varchar(3)) 
                      + '.' + CAST(cv_ip_addresses.add3 AS varchar(3)) + '.' + CAST(cv_ip_addresses.add4 AS varchar(3)) AS ipaddress, cv_ip_vlans.vlan, 
                      cv_domains.name AS domain, cv_domains_test.name AS domainTEST, cv_forecast_answers.appcode, cv_forecast_answers.appname, 
                      cv_forecast_answers.[backup] AS tsm, cv_forecast_answers_backup.start_date AS tsm_start_date, 
                      cv_forecast_answers_backup.time_hour + ' ' + cv_forecast_answers_backup.time_switch AS tsm_start_time, 
                      cv_users_owner.fname + ' ' + cv_users_owner.lname AS app_owner, cv_users_primary.fname + ' ' + cv_users_primary.lname AS app_primary, 
                      cv_users_secondary.fname + ' ' + cv_users_secondary.lname AS app_secondary, 
                      CASE WHEN cv_models_property.fabric = 0 THEN 'Cisco' WHEN cv_models_property.fabric = 1 THEN 'Brocade' ELSE 'Unknown' END AS fabric, 
                      cva_hba_1.name AS WWWN1, cva_hba_2.name AS WWWN2, cv_servers.fdrive AS FdriveOnSAN, 
                      cv_users_lead.fname + ' ' + cv_users_lead.lname AS manager, cv_users_engineer.fname + ' ' + cv_users_engineer.lname AS engineer, 
                      cv_luns.luns + cv_mount_points.luns AS luns, cv_lun_drives.drives AS drives, cv_lun_drive_letters.drives AS driveLetters
FROM         clearview.dbo.cv_servers INNER JOIN
                      clearview.dbo.cv_servers_assets INNER JOIN
                      cva_status INNER JOIN
                      cva_assets INNER JOIN
                      clearview.dbo.cv_models_property INNER JOIN
                      clearview.dbo.cv_models INNER JOIN
                      clearview.dbo.cv_types INNER JOIN
                      clearview.dbo.cv_platforms ON cv_types.platformid = cv_platforms.platformid AND cv_platforms.enabled = 1 AND cv_platforms.deleted = 0 ON 
                      cv_models.typeid = cv_types.id AND cv_types.enabled = 1 AND cv_types.deleted = 0 ON cv_models_property.modelid = cv_models.id AND 
                      cv_models.enabled = 1 AND cv_models.deleted = 0 ON cva_assets.modelid = cv_models_property.id AND cv_models_property.deleted = 0 ON 
                      cva_status.assetid = cva_assets.id AND cva_assets.deleted = 0 AND cva_assets.asset LIKE 'VSG%' OUTER APPLY
                          (SELECT     TOP 1 *
                            FROM          cva_hba
                            WHERE      cva_hba.assetid = cva_assets.id
                            ORDER BY modified ASC) AS cva_hba_1 OUTER APPLY
                          (SELECT     TOP 1 *
                            FROM          cva_hba
                            WHERE      cva_hba.assetid = cva_assets.id
                            ORDER BY modified DESC) AS cva_hba_2 ON cva_status.assetid = cv_servers_assets.assetid AND cva_status.status = 10 AND 
                      cva_status.deleted = 0 ON cv_servers_assets.serverid = cv_servers.id AND cv_servers_assets.dr = 1 AND cv_servers_assets.deleted = 0 INNER JOIN
                      clearview.dbo.cv_operating_systems ON cv_servers.osid = cv_operating_systems.id AND cv_operating_systems.deleted = 0 INNER JOIN
                      clearview.dbo.cv_domains ON cv_servers.domainid = cv_domains.id AND cv_domains.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_domains AS cv_domains_test ON cv_servers.test_domainid = cv_domains_test.id AND cv_domains_test.deleted = 0 INNER JOIN
                      clearview.dbo.cv_forecast_answers INNER JOIN
                      clearview.dbo.cv_forecast INNER JOIN
                      clearview.dbo.cv_requests INNER JOIN
                      clearview.dbo.cv_projects LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_engineer ON cv_projects.engineer = cv_users_engineer.userid AND 
                      cv_users_engineer.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_lead ON cv_projects.lead = cv_users_lead.userid AND cv_users_lead.deleted = 0 ON 
                      cv_requests.projectid = cv_projects.projectid AND cv_projects.deleted = 0 ON cv_forecast.requestid = cv_requests.requestid AND 
                      cv_requests.deleted = 0 ON cv_forecast_answers.forecastid = cv_forecast.id AND cv_forecast.deleted = 0 OUTER APPLY
                          (SELECT     clearview.dbo.getDriveLetters(cv_forecast_answers.id) AS drives) AS cv_lun_drive_letters OUTER APPLY
                          (SELECT DISTINCT COUNT(cv_storage_luns.driveid) AS drives
                            FROM          clearview.dbo.cv_storage_luns
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_lun_drives OUTER APPLY
                          (SELECT     COUNT(cv_storage_luns.id) AS luns
                            FROM          clearview.dbo.cv_storage_luns
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_luns OUTER APPLY
                          (SELECT     COUNT(cv_storage_mount_points.id) AS luns
                            FROM          clearview.dbo.cv_storage_luns INNER JOIN
                                                   clearview.dbo.cv_storage_mount_points ON cv_storage_luns.id = cv_storage_mount_points.lunid AND 
                                                   cv_storage_mount_points.deleted = 0
                            WHERE      cv_storage_luns.answerid = cv_forecast_answers.id AND cv_storage_luns.deleted = 0) AS cv_mount_points LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_owner ON cv_forecast_answers.appcontact = cv_users_owner.userid AND 
                      cv_users_owner.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_primary ON cv_forecast_answers.admin1 = cv_users_primary.userid AND 
                      cv_users_primary.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_users AS cv_users_secondary ON cv_forecast_answers.admin2 = cv_users_secondary.userid AND 
                      cv_users_secondary.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_forecast_answers_backup ON cv_forecast_answers.id = cv_forecast_answers_backup.answerid AND 
                      cv_forecast_answers_backup.deleted = 0 ON cv_servers.answerid = cv_forecast_answers.id AND cv_forecast_answers.deleted = 0 LEFT OUTER JOIN
                      clearview.dbo.cv_servers_ips INNER JOIN
                      clearviewip.dbo.cv_ip_addresses INNER JOIN
                      clearviewip.dbo.cv_ip_networks INNER JOIN
                      clearviewip.dbo.cv_ip_vlans ON cv_ip_networks.vlanid = cv_ip_vlans.id AND cv_ip_vlans.deleted = 0 ON 
                      cv_ip_addresses.networkid = cv_ip_networks.id AND cv_ip_networks.deleted = 0 ON cv_servers_ips.ipaddressid = cv_ip_addresses.id AND 
                      cv_ip_addresses.deleted = 0 ON cv_servers.id = cv_servers_ips.serverid AND cv_servers_ips.deleted = 0
WHERE     cv_servers.deleted = 0

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetsDR'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetsDR'
GO
/****** Object:  View [dbo].[vw_AssetServers]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_AssetServers]
AS
SELECT     TOP (100) 
                      PERCENT CASE WHEN cva_status.status = 10 THEN 'In Use' WHEN cva_status.status = 1 THEN 'In Stock' WHEN cva_status.status = 100 THEN 'Reserved'
                       WHEN cva_status.status = - 10 THEN 'Disposed' END AS status, dbo.cva_status.name, dbo.cva_status.assetid, dbo.cva_assets.bad, 
                      dbo.cva_assets.serial, dbo.cva_assets.asset, dbo.cva_server.dummy_name, dbo.cva_server.vlan, dbo.cva_server.ilo, 
                      Clearview.dbo.cv_rooms.name AS room, Clearview.dbo.cv_racks.name AS rack, dbo.cva_server.rackposition, Clearview.dbo.cv_classs.name AS class, 
                      Clearview.dbo.cv_environment.name AS environment, 
                      Clearview.dbo.cv_location_address.name + ' (' + Clearview.dbo.cv_location_city.name + ', ' + Clearview.dbo.cv_location_state.name + ')' AS location
FROM         dbo.cva_server INNER JOIN
                      dbo.cva_status ON dbo.cva_server.assetid = dbo.cva_status.assetid AND dbo.cva_status.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_classs ON dbo.cva_server.classid = Clearview.dbo.cv_classs.id AND Clearview.dbo.cv_classs.enabled = 1 AND 
                      Clearview.dbo.cv_classs.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_environment ON dbo.cva_server.environmentid = Clearview.dbo.cv_environment.id AND 
                      Clearview.dbo.cv_environment.enabled = 1 AND Clearview.dbo.cv_environment.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_location_address INNER JOIN
                      Clearview.dbo.cv_location_city INNER JOIN
                      Clearview.dbo.cv_location_state ON Clearview.dbo.cv_location_city.stateid = Clearview.dbo.cv_location_state.id AND 
                      Clearview.dbo.cv_location_state.enabled = 1 AND Clearview.dbo.cv_location_state.deleted = 0 ON 
                      Clearview.dbo.cv_location_address.cityid = Clearview.dbo.cv_location_city.id AND Clearview.dbo.cv_location_city.enabled = 1 AND 
                      Clearview.dbo.cv_location_city.deleted = 0 ON dbo.cva_server.addressid = Clearview.dbo.cv_location_address.id AND 
                      Clearview.dbo.cv_location_address.enabled = 1 AND Clearview.dbo.cv_location_address.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_rooms ON dbo.cva_server.roomid = Clearview.dbo.cv_rooms.id AND Clearview.dbo.cv_rooms.enabled = 1 AND 
                      Clearview.dbo.cv_rooms.deleted = 0 LEFT OUTER JOIN
                      Clearview.dbo.cv_racks ON dbo.cva_server.rackid = Clearview.dbo.cv_racks.id AND Clearview.dbo.cv_racks.enabled = 1 AND 
                      Clearview.dbo.cv_racks.deleted = 0 INNER JOIN
                      dbo.cva_assets ON dbo.cva_server.assetid = dbo.cva_assets.id AND dbo.cva_assets.deleted = 0
WHERE     (dbo.cva_server.deleted = 0)
ORDER BY dbo.cva_assets.serial

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cva_server"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cva_status"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_classs (Clearview.dbo)"
            Begin Extent = 
               Top = 6
               Left = 416
               Bottom = 114
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_environment (Clearview.dbo)"
            Begin Extent = 
               Top = 6
               Left = 605
               Bottom = 114
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_address (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_city (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 227
               Bottom = 222
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_state (Clearview.dbo)"
            Begin Extent = 
               Top = 114
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetServers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'      Left = 416
               Bottom = 222
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_rooms (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 605
               Bottom = 222
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_racks (Clearview.dbo)"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cva_assets"
            Begin Extent = 
               Top = 222
               Left = 227
               Bottom = 330
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetServers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_AssetServers'
GO
/****** Object:  View [dbo].[vw_ClearView_Asset]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_ClearView_Asset]
AS
SELECT     dbo.cva_assets.id, dbo.cva_assets.modelid, dbo.cva_assets.serial, dbo.cva_assets.asset, dbo.cva_assets.bad, dbo.cva_assets.validated, 
                      dbo.cva_assets.created, dbo.cva_assets.deleted, ClearView.dbo.cv_models.make, ClearView.dbo.cv_models.name AS model, 
                      ClearView.dbo.cv_types.name AS type, ClearView.dbo.cv_platforms.name AS platform, ClearView.dbo.cv_depot.name AS stock, 
                      ClearView.dbo.cv_depot_rooms.name AS stockroom, ClearView.dbo.cv_shelfs.name AS shelf, 
                      ClearView.dbo.cv_users.fname + ' ' + ClearView.dbo.cv_users.lname AS technician, CAST(ClearViewIP.dbo.cv_ip_addresses.add1 AS varchar(3)) 
                      + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add2 AS varchar(3)) + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add3 AS varchar(3)) 
                      + '.' + CAST(ClearViewIP.dbo.cv_ip_addresses.add4 AS varchar(3)) AS ipaddress, 
                      ClearView.dbo.cv_location_address.name + ' (' + ClearView.dbo.cv_location_city.name + ', ' + ClearView.dbo.cv_location_state.name + ')' AS location, 
                      ClearView.dbo.cv_racks.name AS rack, ClearView.dbo.cv_environment.name AS environment, ClearView.dbo.cv_classs.name AS class, 
                      ClearView.dbo.cv_rooms.name AS room, dbo.cva_status.datestamp AS commissionedon, 
                      ClearView.dbo.cv_users.fname + ' ' + ClearView.dbo.cv_users.lname AS commissionedby, dbo.cva_status.name, dbo.cva_status.userid, 
                      dbo.cva_status.status,
Case WHEN  dbo.cva_status.status=-1 THEN 
	dbo.cva_status.datestamp 
ELSE NULL END AS decommissionedon,
Case WHEN  dbo.cva_status.status=-1 THEN 
	 ClearView.dbo.cv_users.fname + ' ' + ClearView.dbo.cv_users.lname 
ELSE NULL END AS decommissionedby

FROM         dbo.cva_assets LEFT OUTER JOIN
                      dbo.cva_network LEFT OUTER JOIN
                      ClearView.dbo.cv_depot_rooms ON dbo.cva_network.depotroomid = ClearView.dbo.cv_depot_rooms.id AND 
                      ClearView.dbo.cv_depot_rooms.enabled = 1 AND ClearView.dbo.cv_depot_rooms.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_location_address INNER JOIN
                      ClearView.dbo.cv_location_city INNER JOIN
                      ClearView.dbo.cv_location_state ON ClearView.dbo.cv_location_city.stateid = ClearView.dbo.cv_location_state.id AND 
                      ClearView.dbo.cv_location_state.enabled = 1 AND ClearView.dbo.cv_location_state.deleted = 0 ON 
                      ClearView.dbo.cv_location_address.cityid = ClearView.dbo.cv_location_city.id AND ClearView.dbo.cv_location_city.enabled = 1 AND 
                      ClearView.dbo.cv_location_city.deleted = 0 ON dbo.cva_network.addressid = ClearView.dbo.cv_location_address.id AND 
                      ClearView.dbo.cv_location_address.enabled = 1 AND ClearView.dbo.cv_location_address.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_racks ON dbo.cva_network.rackid = ClearView.dbo.cv_racks.id AND ClearView.dbo.cv_racks.enabled = 1 AND 
                      ClearView.dbo.cv_racks.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_environment ON dbo.cva_network.environmentid = ClearView.dbo.cv_environment.id AND 
                      ClearView.dbo.cv_environment.enabled = 1 AND ClearView.dbo.cv_environment.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_classs ON dbo.cva_network.classid = ClearView.dbo.cv_classs.id AND ClearView.dbo.cv_classs.enabled = 1 AND 
                      ClearView.dbo.cv_classs.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_rooms ON dbo.cva_network.roomid = ClearView.dbo.cv_rooms.id AND ClearView.dbo.cv_rooms.enabled = 1 AND 
                      ClearView.dbo.cv_rooms.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_depot ON dbo.cva_network.depotid = ClearView.dbo.cv_depot.id AND ClearView.dbo.cv_depot.enabled = 1 AND 
                      ClearView.dbo.cv_depot.deleted = 0 LEFT OUTER JOIN
                      ClearView.dbo.cv_shelfs ON dbo.cva_network.shelfid = ClearView.dbo.cv_shelfs.id AND ClearView.dbo.cv_shelfs.enabled = 1 AND 
                      ClearView.dbo.cv_shelfs.deleted = 0 LEFT OUTER JOIN
                      dbo.cva_ips INNER JOIN
                      ClearViewIP.dbo.cv_ip_addresses ON dbo.cva_ips.ipaddressid = ClearViewIP.dbo.cv_ip_addresses.id AND 
                      ClearViewIP.dbo.cv_ip_addresses.deleted = 0 ON dbo.cva_ips.assetid = dbo.cva_network.assetid AND dbo.cva_ips.deleted = 0 ON 
                      dbo.cva_assets.id = dbo.cva_network.assetid AND dbo.cva_network.deleted = 0 INNER JOIN
                      ClearView.dbo.cv_models INNER JOIN
                      ClearView.dbo.cv_types INNER JOIN
                      ClearView.dbo.cv_platforms ON ClearView.dbo.cv_types.platformid = ClearView.dbo.cv_platforms.platformid AND 
                      ClearView.dbo.cv_platforms.enabled = 1 AND ClearView.dbo.cv_platforms.deleted = 0 ON 
                      ClearView.dbo.cv_models.typeid = ClearView.dbo.cv_types.id AND ClearView.dbo.cv_types.enabled = 1 AND ClearView.dbo.cv_types.deleted = 0 ON 
                      dbo.cva_assets.modelid = ClearView.dbo.cv_models.id AND ClearView.dbo.cv_models.enabled = 1 AND 
                      ClearView.dbo.cv_models.deleted = 0 INNER JOIN
                      dbo.cva_status LEFT OUTER JOIN
                      ClearView.dbo.cv_users ON dbo.cva_status.userid = ClearView.dbo.cv_users.userid AND ClearView.dbo.cv_users.enabled = 1 AND 
                      ClearView.dbo.cv_users.deleted = 0 ON dbo.cva_assets.id = dbo.cva_status.assetid AND dbo.cva_status.deleted = 0

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[24] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -288
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cva_assets"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cva_network"
            Begin Extent = 
               Top = 438
               Left = 38
               Bottom = 546
               Right = 191
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_depot_rooms (Clearview.dbo)"
            Begin Extent = 
               Top = 6
               Left = 233
               Bottom = 114
               Right = 384
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_address (Clearview.dbo)"
            Begin Extent = 
               Top = 6
               Left = 422
               Bottom = 114
               Right = 573
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_city (Clearview.dbo)"
            Begin Extent = 
               Top = 6
               Left = 611
               Bottom = 114
               Right = 762
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_location_state (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_racks (Clearview.dbo)"
            Begin Extent = 
               Top = 114
       ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ClearView_Asset'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'        Left = 227
               Bottom = 222
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_environment (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 416
               Bottom = 222
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_classs (Clearview.dbo)"
            Begin Extent = 
               Top = 114
               Left = 605
               Bottom = 222
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_rooms (Clearview.dbo)"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_depot (Clearview.dbo)"
            Begin Extent = 
               Top = 330
               Left = 281
               Bottom = 438
               Right = 432
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_shelfs (Clearview.dbo)"
            Begin Extent = 
               Top = 330
               Left = 470
               Bottom = 438
               Right = 621
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cva_ips"
            Begin Extent = 
               Top = 222
               Left = 227
               Bottom = 330
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_ip_addresses (ClearViewIP.dbo)"
            Begin Extent = 
               Top = 438
               Left = 416
               Bottom = 546
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_models (Clearview.dbo)"
            Begin Extent = 
               Top = 222
               Left = 416
               Bottom = 330
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_types (Clearview.dbo)"
            Begin Extent = 
               Top = 330
               Left = 38
               Bottom = 438
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cv_platforms (Clearview.dbo)"
            Begin Extent = 
               Top = 222
               Left = 605
               Bottom = 330
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cva_status"
            Begin Extent = 
               Top = 330
               Left = 659
               Bottom = 438
               Right = 810
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "cv_users (Clearview.dbo)"
            Begin Extent = 
               Top = 438
               Left = 227
               Bottom = 546
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 26
         Width = 284
         Width = 1500
         Width = 1500
         Width = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ClearView_Asset'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane3', @value=N'1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4050
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ClearView_Asset'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=3 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_ClearView_Asset'
GO
/****** Object:  View [dbo].[vw_EnclosureInventory]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_EnclosureInventory] AS
SELECT
 	DISTINCT
	e.id AS EnclosureID,
	st.name AS EnclosureName,
	b.slot As BladeSlotNumber,
	b.dummy_name As DummyName,
	b.ilo As ilo,
	cva.serial As SerialNumber,
	cva.asset As AssetTag,
	env.name As Environment,
	cl.name As Class,
	sa.serverid As ServerID,
	bn.name As ServerName,
	b.spare AS IsSpare,
	qln.Performance,
	qln.Size,
	qln.SizeTest,
	e.classid As ClassID,
	st.status Status,
	loc.id As AddressID,	
	loc.name As Address
FROM
	cva_enclosures e
		INNER JOIN cva_status st
			ON e.assetid = st.assetid
			And st.deleted = 0
		INNER JOIN cva_blades b
			ON e.assetid = b.enclosureid
			And b.deleted = 0 
		INNER JOIN cva_assets cva
			ON b.assetid = cva.id
			And cva.deleted = 0
		LEFT JOIN ClearView.dbo.cv_environment env
			ON e.environmentid = env.id
			And env.enabled = 1
			And env.deleted = 0
		LEFT JOIN ClearView.dbo.cv_classs cl
			ON e.classid = cl.id
			And cl.enabled = 1
			And cl.deleted = 0
		LEFT JOIN ClearView.dbo.cv_servers_assets sa
			ON b.assetid = sa.assetid
			And sa.deleted = 0
		LEFT JOIN Clearview.dbo.cv_servers svr
			ON sa.serverid = svr.id
			And svr.deleted = 0
		LEFT JOIN cva_status bn
			ON b.assetid = bn.assetid
			And bn.deleted = 0
		LEFT JOIN 
			(
    				SELECT
					Max(lun.id) As LunID,
					Max(lun.[number]) As LunNumber,
					Max(lun.answerid) As AnswerID,
					Max(lun.performance) As Performance,
					IsNull( ( Sum(lun.size) + Sum(mp.size) ), 0 )As Size,
					IsNull( ( Sum(lun.size_test) + Sum(mp.size_test) ), 0 ) As SizeTest
				FROM
					ClearView.dbo.cv_storage_luns lun
						LEFT OUTER JOIN ClearView.dbo.cv_storage_mount_points mp		
							ON lun.id = mp.lunid
							And mp.deleted = 0
				WHERE	
					lun.deleted = 0
				GROUP BY
					lun.answerid
    			)qln
			ON svr.answerid = qln.AnswerID	
		INNER JOIN ClearView.dbo.cv_forecast_answers fa
			ON svr.answerid = fa.id
			And fa.deleted = 0
		LEFT JOIN ClearView.dbo.cv_location_address loc
			ON fa.addressid = loc.id
			And loc.deleted = 0		
WHERE 
 	e.deleted = 0


GO
/****** Object:  View [dbo].[vw_EnclosureInventoryRevised]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_EnclosureInventoryRevised] AS
SELECT
 	DISTINCT
	CASE
		WHEN status_blade.status = 1 THEN 'Available'	
		WHEN status_blade.status = 10 THEN 'UnAvailable'
		WHEN status_blade.status = 100 THEN 'UnAvailable'
	END AS StatusText,
	status_blade.status As Status,
	status_blade.assetid AS BladeAssetID,
	cva_assets.bad As Bad,
	cva_assets.serial AS SerialNumber,
	cva_assets.asset AS AssetTag,
	enc_blade.name AS EnclosureName,
	enc_blade.assetid AS EnclosureAssetID,
	cva_blades.slot As BladeSlotNumber,
	cva_blades.dummy_name As DummyName,
	cls.name As Class,
	cva_enclosures.classid As ClassID,
	env.name As Environment,
	cva_enclosures.id As EnclosureID,
	cva_enclosures.addressid As AddressID,
	( la.name + ' ' + lc.name + ' ' + ls.name ) As Address
FROM
	ClearViewAsset.dbo.cva_blades
		INNER JOIN ClearViewAsset.dbo.cva_status status_blade
			ON cva_blades.assetid = status_blade.assetid
			And status_blade.deleted = 0
		INNER JOIN ClearViewAsset.dbo.cva_enclosures
		INNER JOIN ClearViewAsset.dbo.cva_status enc_blade
			ON cva_enclosures.assetid = enc_blade.assetid
			And enc_blade.deleted = 0
			ON cva_blades.enclosureid = cva_enclosures.assetid
			And cva_enclosures.deleted = 0
		INNER JOIN ClearViewAsset.dbo.cva_Assets
			ON cva_blades.assetid = cva_Assets.id
			And cva_Assets.deleted = 0
		LEFT JOIN ClearView.dbo.cv_classs cls
			ON cva_enclosures.classid = cls.id
			And cls.enabled = 1
			And cls.deleted = 0
		LEFT JOIN ClearView.dbo.cv_environment env
			ON cva_enclosures.environmentid = env.id
			And env.enabled = 1
			And env.deleted = 0
		LEFT JOIN ClearView.dbo.cv_location_address la
			ON cva_enclosures.addressid = la.id
			And la.enabled = 1
			And la.deleted = 0
		LEFT JOIN ClearView.dbo.cv_location_city lc
			ON la.cityid = lc.id
			And lc.enabled = 1
			And lc.deleted = 0
		LEFT JOIN ClearView.dbo.cv_location_state ls
			ON lc.stateid = ls.id
			And ls.enabled = 1
			And ls.deleted = 0
WHERE
 	cva_blades.deleted = 0


GO
/****** Object:  View [dbo].[vw_OnDemandServers]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_OnDemandServers] AS
SELECT 
 	b.assetid As AssetID,
	b.enclosureid As EnclosureID,
	b.dummy_name,
	r.name As RoomName,
	ra.name As RackName,
	s.status As StatusID,
	Cast(1 as bit) As IsBlade,
	cva.modelid As ModelID,
	ClearView.dbo.fn_AcquisitionCost(mp.id) As AcquisitionCost,
	mp.name As ModelName
FROM ClearViewAsset.dbo.cva_blades b
	INNER JOIN ClearViewAsset.dbo.cva_enclosures e
		ON b.enclosureid = e.assetid
		And e.deleted = 0
	LEFT JOIN ClearView.dbo.cv_rooms r
		ON e.roomid = r.id
		And r.enabled = 1
		And r.deleted = 0
	LEFT JOIN ClearView.dbo.cv_racks ra
		ON e.rackid = ra.id
		And ra.enabled = 1
		And ra.deleted = 0
	LEFT JOIN ClearViewAsset.dbo.cva_status s
		ON b.assetid = s.assetid
		And s.deleted = 0
	LEFT JOIN ClearViewAsset.dbo.cva_assets cva
		ON b.assetid = cva.id
		And cva.deleted = 0
	LEFT JOIN ClearView.dbo.cv_models_property mp
		ON cva.modelid = mp.id
		And mp.enabled = 1
		And mp.deleted = 0
WHERE
 	b.deleted = 0

UNION ALL

SELECT
  	svr.assetid As AssetID,
	Space(50) As EnclosureID,
	svr.dummy_name,
	r.name As RoomName,
	ra.name As RackName,
	s.status As StatusID,
	Cast(0 As bit) As IsBlade,
	cva.modelid As ModelID,
	ClearView.dbo.fn_AcquisitionCost(mp.id) As AcquisitionCost,
	mp.name As ModelName
FROM
	ClearViewAsset.dbo.cva_server svr
		LEFT JOIN ClearView.dbo.cv_rooms r
			ON svr.roomid = r.id
			And r.enabled = 1
			And r.deleted = 0
		LEFT JOIN ClearView.dbo.cv_racks ra
			ON svr.rackid = ra.id
			And ra.enabled = 1
			And ra.deleted = 0
		LEFT JOIN ClearViewAsset.dbo.cva_status s
			ON svr.assetid = s.assetid
			And s.deleted = 0
		LEFT JOIN ClearViewAsset.dbo.cva_assets cva
			ON svr.assetid = cva.id
			And cva.deleted = 0
		LEFT JOIN ClearView.dbo.cv_models_property mp
			ON cva.modelid = mp.id
			And mp.enabled = 1
			And mp.deleted = 0			
WHERE
 	svr.deleted = 0


GO
/****** Object:  View [dbo].[vwAssetStockAndForecast]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE View [dbo].[vwAssetStockAndForecast]
AS
SELECT 
vwModelDetails.ModelID, 
vwModelDetails.ModelName, 
vwModelDetails.Platform, 
vwForeCastDetailsIM.AddressID as LocationId, 
vwForeCastDetailsIM.Address + ' (' + vwForeCastDetailsIM.City + ', ' + vwForeCastDetailsIM.State + ')' AS location,
dbo.vwForeCastDetailsIM.Confidenceid,
dbo.vwForeCastDetailsIM.ClassID,
dbo.vwForeCastDetailsIM.ClassName as Class,
0 as Arrived,
0 as Instock,
Isnull(sum(vwForeCastDetailsIM.ServerQuantity),0) as TotalForecast,
Isnull(sum(vwForeCastDetailsIM.Recovery_Number),0) as TotalRecoveryNumber,
isnull((Select Sum(cvForecastStreetValues.cost) from ClearView.dbo.cv_forecast_street_values as cvForecastStreetValues 
where cvForecastStreetValues.modelid=vwModelDetails.ModelID  and cvForecastStreetValues.enabled=1 and cvForecastStreetValues.deleted=0
),0) as StreetValues
FROM  clearviewasset.dbo.vwForeCastDetailsIM 
	LEFT OUTER JOIN clearviewasset.dbo.vwModelDetails  
	ON vwForeCastDetailsIM.ModelID =vwModelDetails.ModelID
group by vwModelDetails.ModelID,vwModelDetails.ModelName, vwModelDetails.Platform, 
vwForeCastDetailsIM.AddressID,vwForeCastDetailsIM.Address,
vwForeCastDetailsIM.City, vwForeCastDetailsIM.State ,dbo.vwForeCastDetailsIM.Confidenceid,
dbo.vwForeCastDetailsIM.ClassID,
dbo.vwForeCastDetailsIM.ClassName


UNION ALL
SELECT     dbo.vwModelDetails.ModelID, 
dbo.vwModelDetails.ModelName, 
dbo.vwModelDetails.Platform, 
dbo.vwAssetStocks.locationid as LocationId, 
ClearView.dbo.cv_location_address.name + ' (' + ClearView.dbo.cv_location_city.name + ', ' + ClearView.dbo.cv_location_state.name + ')' AS location,
-1 as Confidenceid,
dbo.vwAssetStocks.ClassID, 
dbo.vwAssetStocks.Class , 
isnull(dbo.vwAssetStocks.Arrived,0) as Arrived,
isnull(dbo.vwAssetStocks.InStock,0) as InStock,
0 as TotalForecast,
0 as TotalRecoveryNumber,
isnull((Select Sum(cvForecastStreetValues.cost) from ClearView.dbo.cv_forecast_street_values as cvForecastStreetValues 
where cvForecastStreetValues.modelid=vwModelDetails.ModelID  and cvForecastStreetValues.enabled=1 and cvForecastStreetValues.deleted=0
),0) as StreetValues

FROM         dbo.vwModelDetails LEFT OUTER JOIN
dbo.vwAssetStocks ON dbo.vwModelDetails.ModelID = dbo.vwAssetStocks.ModelID 
LEFT OUTER JOIN ClearView.dbo.cv_location_address 
ON dbo.vwAssetStocks.locationid =cv_location_address.id
LEFT OUTER JOIN ClearView.dbo.cv_location_city 
LEFT OUTER JOIN ClearView.dbo.cv_location_state 
ON ClearView.dbo.cv_location_city.stateid = ClearView.dbo.cv_location_state.id 
AND ClearView.dbo.cv_location_state.enabled = 1 AND ClearView.dbo.cv_location_state.deleted = 0 
ON ClearView.dbo.cv_location_address.cityid = ClearView.dbo.cv_location_city.id AND ClearView.dbo.cv_location_city.enabled = 1 AND 
ClearView.dbo.cv_location_city.deleted = 0

GO
/****** Object:  View [dbo].[vwAssetStocks]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwAssetStocks]
As
Select a.ModelID,A.Model, A.Type,A.classid,A.class,A.locationid,A.location ,Sum(a.InStock) as InStock, sum(a.Arrived) as Arrived
From
(Select 
ModelID,model,
type,classid,class,
locationid,location,
CASE status WHEN 'In Stock' THEN  COUNT(status) END  as InStock,

CASE status WHEN 'Arrived' THEN  COUNT(status) END  as Arrived
FROM dbo.vw_Assets
WHERE status in ('In Stock', 'Arrived')
group by modelid,model,type,classid,class,status,locationid,location
) as a
group by
a.ModelID,A.Model, A.Type,A.classid,A.class,A.locationid,A.location

GO
/****** Object:  View [dbo].[vwForeCastAndExecutionSummary]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwForeCastAndExecutionSummary]
AS
Select 
ProjectNumber,
ProjectName,
ProjectBaseDiscretion, 
ProjectPoftfolio,
ProjectManagerName,
ServerClassId ,
ServerClass ,
ServerTypeID ,
ServerTypeName ,
ServerModelID,
ServerModel,
sum(ForecastServerQuantity) as ForecastServerQuantity,
sum(ForecastStorage) as ForecastStorage,
sum(ForecastStoragePorts) as ForecastStoragePorts,
sum(ForecastNetworkPorts) as ForecastNetworkPorts,
sum(ForecastAmps) as ForecastAmps,

sum(ExecutedServerCount) as ExecutedServerCount,
sum(ExecutedStorage) as ExecutedStorage,
sum(ExecutedStoragePorts ) as ExecutedStoragePorts,
sum(ExecutedNetworkPorts) as  ExecutedNetworkPorts,
sum(ExecutedAmps) as ExecutedAmps
FROM 

(
/* ForeCast Information */
SELECT
ProjectNumber,
ProjectName,
ProjectBaseDiscretion, 
ProjectPoftfolio,
ProjectManagerName,
classid as ServerClassId,
className as ServerClass,
TypeID as ServerTypeID,
TypeName  as ServerTypeName,
MODELID as ServerModelID,
ModelName as ServerModel,
sum(ServerQuantity+Recovery_Number) as ForecastServerQuantity,
sum(
high_total+high_test+high_QA+HighReplicatedFinal
+standard_total + standard_test +Standard_QA+StandardReplicatedFinal
+low_total+low_test +LOW_QA+LowReplicatedFinal
   ) as ForecastStorage,
(StoragePorts*sum(ServerQuantity+Recovery_Number)) as ForecastStoragePorts,
(NetworkPorts*sum(ServerQuantity+Recovery_Number)) as ForecastNetworkPorts,
(AMPs*sum(ServerQuantity+Recovery_Number)) as ForecastAmps,
0 as ExecutedServerCount,
0 as ExecutedStorage,
0 as ExecutedStoragePorts,
0 as ExecutedNetworkPorts,
0 as ExecutedAmps

FROM dbo.vwForeCastDetailsIM
group by 
ProjectNumber, 
ProjectName, 
ProjectBaseDiscretion,
ProjectPoftfolio,ProjectManagerName,ClassID, ClassName,TypeID,TypeName, MODELID,
ModelName,StoragePorts, NetworkPorts, AMPs

/* Executed Asset Information */
UNION ALL
SELECT 
ProjectNumber, 
ProjectName, 
ProjectBaseDiscretion,
ProjectPoftfolio,
ProjectManagerName,
ServerClassID, 
ServerClass,
ServerTypeID,
ServerTypeName,
ServerModelID ,
ServerModel,
0 as ForecastServerQuantity,
0 as ForecastStorage,
0 as ForecastStoragePorts,
0 as ForecastNetworkPorts,
0 as ForecastAmps,
sum(ServerQuantity+ServerRecoveryNumber) as ExecutedServerCount,
sum(ServerCapacityProd)+Sum(ServerCapacityQA)+Sum(ServerCapacityTest)+Sum(ServerCapacityReplicated) as ExecutedStorage,
(StoragePorts* sum(ServerQuantity+ServerRecoveryNumber)) as ExecutedStoragePorts,
(NetworkPorts* sum(ServerQuantity+ServerRecoveryNumber)) as ExecutedNetworkPorts,
(AMPs *sum(ServerQuantity+ServerRecoveryNumber))as ExecutedAmps

from ClearView.dbo.vwCVServerDesignDetails 
where ServerCompletionDate is not null
group by 
ProjectNumber, ProjectName, ProjectBaseDiscretion,
ProjectPoftfolio,ProjectManagerName,ServerClassID, ServerClass,ServerTypeID,ServerTypeName,
ServerModelID ,ServerModel,StoragePorts,NetworkPorts,AMPs

) a
group by 
ProjectNumber, ProjectName, ProjectBaseDiscretion,
ProjectPoftfolio,ProjectManagerName,ServerClassId ,ServerClass ,ServerTypeID ,ServerTypeName,
ServerModelID,ServerModel

GO
/****** Object:  View [dbo].[vwForeCastAvailableExecutionSummary]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwForeCastAvailableExecutionSummary] as
SELECT 
MODELID, ModelName, 
TypeID, TypeName, 
TypeCatagory collate SQL_Latin1_General_CP1_CI_AS as  TypeCatagory,
TypeGrouping  collate SQL_Latin1_General_CP1_CI_AS as TypeGrouping,
AddressID as LocationId, Address as Location,City, 
StoragePorts,
NetworkPorts,
AMPs,
ServerQuantity, Recovery_Number, 
high_total, high_QA, high_test, high_replicated, 
standard_total, standard_QA, standard_test, standard_replicated, 
low_total, low_QA, low_test, low_replicated, 
HighReplicatedFinal, StandardReplicatedFinal, LowReplicatedFinal,
ReplicateTimes, NoOfDays,
0 as AvailableServerCount,
0 as AvailableServerCapacityProd,
0 as AvailableServerCapacityQA,
0 as AvailableServerCapacityTest,
0 as AvailableServerCapacityReplicated,
0 as AvailableAMPS,
0 as AvailableStoragePorts,
0 as AvailableNetworkPorts,

0 as ExecutedServerCount,
0 as ExecutedServerCapacityProd,
0 as ExecutedServerCapacityQA,
0 as ExecutedServerCapacityTest,
0 as ExecutedServerCapacityReplicated,
0 as ExecutedAMPS,
0 as ExecutedStoragePorts,
0 as ExecutedNetworkPorts
FROM         
dbo.vwForeCastDetails

UNION ALL
/* Excuted */
SELECT 

ServerModelID as MODELID,ServerModel as ModelName,
ServerTypeID as TypeID,ServerTypeName as TypeName,
ServerTypeCatagory  collate SQL_Latin1_General_CP1_CI_AS as TypeCategory,
TypeGrouping  collate SQL_Latin1_General_CP1_CI_AS as TypeGrouping,

ServerLocationID as LocationId,ServerLocation as Location,ServerLocationCity as City,
StoragePorts,NetworkPorts,AMPs,
0 AS ServerQuantity, 0 AS Recovery_Number, 
0 AS high_total, 0 AS high_QA,0 AS  high_test,0 AS  high_replicated, 
0 AS standard_total, 0 AS standard_QA, 0 AS standard_test,0 AS  standard_replicated, 
0 AS low_total,0 AS  low_QA,0 AS  low_test,0 AS  low_replicated, 
0 AS HighReplicatedFinal, 0 AS StandardReplicatedFinal, 0 AS LowReplicatedFinal,
0 AS ReplicateTimes, NULL AS NoOfDays,

0 as AvailableServerCount,
0 as AvailableServerCapacityProd,
0 as AvailableServerCapacityQA,
0 as AvailableServerCapacityTest,
0 as AvailableServerCapacityReplicated,
0 as AvailableAMPS,
0 as AvailableStoragePorts,
0 as AvailableNetworkPorts,

sum(ServerQuantity+ServerRecoveryNumber) as ExecutedServerCount,
sum(ServerCapacityProd) as ExecutedServerCapacityProd,
Sum(ServerCapacityQA)as ExecutedServerCapacityQA,
Sum(ServerCapacityTest) as ExecutedServerCapacityTest,
Sum(ServerCapacityReplicated) as ExecutedServerCapacityReplicated,
sum(AMPs*(ServerQuantity+ServerRecoveryNumber)) as ExecutedAMPS,
sum(StoragePorts*(ServerQuantity+ServerRecoveryNumber)) as ExecutedStoragePorts,
sum(NetworkPorts*(ServerQuantity+ServerRecoveryNumber)) as ExecutedNetworkPorts
from ClearView.dbo.vwCVServerDesignDetails 
where  ServerCompletionDate >=DATEADD(Year, -1, getdate())
group by 
ProjectNumber, ProjectName, 
ProjectBaseDiscretion,ProjectPoftfolio,
ProjectManagerName,ServerModelID, 
ServerModel ,ServerClassID, ServerClass ,
ServerTypeID ,ServerTypeName, ServerTypeCatagory ,TypeGrouping,
ServerLocationID,ServerLocation,ServerLocationCity,StoragePorts,NetworkPorts,AMPs
/* END Of Excuted */

UNION ALL 

/* Available Assests */
SELECT
A.ModelID as MODELID, A.ModelName as ModelName,
A.TypeID ,A.TypeName,
A.TypeCatagory  collate SQL_Latin1_General_CP1_CI_AS as TypeCategory,
A.TypeGrouping  collate SQL_Latin1_General_CP1_CI_AS as TypeGrouping,
A.LocationId,A.Location,
A.City as City,
StoragePorts,NetworkPorts,AMPs,

0 AS ServerQuantity, 0 AS Recovery_Number, 
0 AS high_total, 0 AS high_QA,0 AS  high_test,0 AS  high_replicated, 
0 AS standard_total, 0 AS standard_QA, 0 AS standard_test,0 AS  standard_replicated, 
0 AS low_total,0 AS  low_QA,0 AS  low_test,0 AS  low_replicated, 
0 AS HighReplicatedFinal, 0 AS StandardReplicatedFinal, 0 AS LowReplicatedFinal,
0 AS ReplicateTimes, NULL AS NoOfDays,

1 as AvailableServerCount,
0 as AvailableServerCapacityProd,
0 as AvailableServerCapacityQA,
0 as AvailableServerCapacityTest,
0 as AvailableServerCapacityReplicated,
AMPs as AvailableAMPS,
StoragePorts as AvailableStoragePorts,
NetworkPorts as AvailableNetworkPorts,

0 as ExecutedServerCount,
0 as ExecutedServerCapacityProd,
0 as ExecutedServerCapacityQA,
0 as ExecutedServerCapacityTest,
0 as ExecutedServerCapacityReplicated,
0 as ExecutedAMPS,
0 as ExecutedStoragePorts,
0 as ExecutedNetworkPorts
FROM
(
select 
vw_Assets.modelid as ModelID,
vw_Assets.model as ModelName,
cvModelsProperty.storage_ports as StoragePorts,
cvModelsProperty.network_ports as NetworkPorts,
cvModelsProperty.AMP as AMPs,
cvModelsProperty.replicate_times  as ReplicateTimes,
cvTypes.ID as TypeID,
cvTypes.Name as TypeName,
vw_Assets.TypeCatagory,
CASE cvModels.grouping
WHEN 1 then 'Distributed'
WHEN 2 then 'Midrange'
ELSE ''
END as TypeGrouping,
vw_Assets.statusID as AssetStatusID,
vw_Assets.locationid as LocationID,
vw_Assets.Location as Location,
cvLocationCity.Name as City
FROM vw_Assets 
INNER JOIN clearview.dbo.cv_models_property cvModelsProperty
	on vw_Assets.modelid =cvModelsProperty.id AND cvModelsProperty.DELETED=0
INNER JOIN clearview.dbo.cv_models cvModels
	ON cvModelsProperty.modelid=cvModels.id AND  cvModels.enabled = 1 	AND cvModels.DELETED=0
INNER  JOIN ClearView.dbo.cv_types as cvTypes ON
			cvModels.typeid = cvTypes.id AND cvTypes.enabled = 1 AND cvTypes.DELETED=0

			LEFT OUTER JOIN ClearView.dbo.cv_location_address cvLocationAddress
				ON vw_Assets.locationid = cvLocationAddress.id
				And cvLocationAddress.enabled = 1 And cvLocationAddress.deleted = 0
			LEFT OUTER JOIN ClearView.dbo.cv_location_city cvLocationCity
				ON cvLocationAddress.cityid = cvLocationCity.id
				And cvLocationCity.enabled = 1 	And cvLocationCity.deleted = 0
			LEFT OUTER JOIN ClearView.dbo.cv_location_state cvLocationState
				ON cvLocationCity.stateid = cvLocationState.id
				And cvLocationState.enabled = 1 And cvLocationState.deleted = 0

WHERE dbo.vw_Assets.statusid in(0,1)
) as a




/* End of Available Assests */

GO
/****** Object:  View [dbo].[vwForeCastDetails]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW 
[dbo].[vwForeCastDetails] as
SELECT A.*,
CVModels.Name as ModelName,
cvModelsProperty.storage_ports as StoragePorts,
cvModelsProperty.network_ports as NetworkPorts,
cvModelsProperty.AMP as AMPs,
cvModelsProperty.replicate_times  as ReplicateTimes,
Datediff(day,ImplementationDate,getdate()) as NoOfDays,
cvTypes.ID as TypeID,
cvTypes.Name as TypeName,
CASE WHEN cvModelsProperty.type_blade =1 then 'Blade'
	 WHEN cvModelsProperty.type_physical =1 then 'Rack'
	 WHEN cvModelsProperty.type_vmware =1 then 'Virtual'
END  as TypeCatagory,
CASE cvModels.grouping
WHEN 1 then 'Distributed'
WHEN 2 then 'Midrange'
ELSE ''
END as TypeGrouping,
isnull(cvModelsProperty.replicate_times * isnull(a.high_replicated,0) * (a.Recovery_Number/a.ServerQuantity),0)  as HighReplicatedFinal,
isnull(cvModelsProperty.replicate_times * isnull(a.standard_replicated,0) * (a.Recovery_Number/a.ServerQuantity),0) as StandardReplicatedFinal,
isnull(cvModelsProperty.replicate_times * isnull(a.low_replicated,0) * (a.Recovery_Number/a.ServerQuantity),0) as LowReplicatedFinal

FROM
(SELECT
DISTINCT
cv_projects.ProjectID as ProjectID, 
cv_projects.number as ProjectNumber, 
cv_projects.name as ProjectName, 
cv_projects.BD as ProjectBaseDiscretion, 
cv_organizations.Name as ProjectPoftfolio,
RTRIM(isnull(cv_users_lead.lname,'')) + ', ' + RTRIM(isnull(cv_users_lead.fname,'')) AS ProjectManagerName, 
cv_forecast_answers.classid ,
cv_classs.name AS className ,
cv_forecast_answers.id as ForeCastAnswerId, 
/*cv_forecast_answers.modelid as Model,  */
CASE cv_forecast_answers.modelid 
	WHEN 0 THEN (SELECT Top 1 modelid from ClearView.dbo.fnGetForecastModel( cv_forecast_answers.id))
	 ELSE 
		cv_forecast_answers.MODELID
	 END MODELID,
cv_forecast_answers.Confidenceid,
cv_confidence.Name as ConfidenceName,
cv_confidence.display as ConfidenceDisplayOrder, 
isnull(cv_forecast_answers.Quantity,0) as ServerQuantity,
isnull(cv_forecast_answers.recovery_number,0) as Recovery_Number,
cv_forecast_answers.implementation as ImplementationDate,
cv_forecast_answers.environmentid as EnvironmentID,
/***********Storage Information**************/
isnull(cv_forecast_answers_storage.high,0) as high,
isnull(cv_forecast_answers_storage.high_total,0) as high_total,
isnull(cv_forecast_answers_storage.high_qa,0) as high_QA,
isnull(cv_forecast_answers_storage.high_test,0)as high_test,
isnull(cv_forecast_answers_storage.high_replicated,0)as high_replicated,

isnull(cv_forecast_answers_storage.standard,0) as standard,
isnull(cv_forecast_answers_storage.standard_total,0) as standard_total,
isnull(cv_forecast_answers_storage.standard_qa,0) as standard_QA,
isnull(cv_forecast_answers_storage.standard_test,0) as standard_test,
isnull(cv_forecast_answers_storage.standard_replicated,0) as standard_replicated,

isnull(cv_forecast_answers_storage.low,0) as low,
isnull(cv_forecast_answers_storage.low_total,0) as low_total,
isnull(cv_forecast_answers_storage.low_qa,0) as low_QA,
isnull(cv_forecast_answers_storage.low_test,0) as low_test,
isnull(cv_forecast_answers_storage.low_replicated,0) as low_replicated,

/***********END Of Storage Information**************/
cv_environment.name as EnvironmentName,
cv_forecast_answers.AddressID,
cv_forecast_answers.created as ForeCastRequestDate,
cv_location_address.Name as Address,
cv_location_city.Name as City,
cv_location_state.Name as State
FROM
ClearView.dbo.cv_forecast_answers
			INNER JOIN 
				ClearView.dbo.cv_forecast
					INNER JOIN 
						ClearView.dbo.cv_requests
							INNER JOIN 
								ClearView.dbo.cv_projects
									INNER JOIN 
										ClearView.dbo.cv_organizations
									ON 
										cv_projects.organization = cv_organizations.organizationid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_segments
									ON 
										cv_projects.segmentid = cv_segments.id
									LEFT OUTER JOIN 
										ClearView.dbo.cv_users AS cv_users_lead
									ON 
										cv_projects.lead = cv_users_lead.userid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_users AS cv_users_engineer
									ON 
										cv_projects.engineer = cv_users_engineer.userid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_users AS cv_users_technical
									ON 
										cv_projects.technical = cv_users_technical.userid
							ON 
								cv_requests.projectid = cv_projects.projectid
								and cv_projects.deleted = 0
					ON 
						cv_forecast.requestid = cv_requests.requestid
						And cv_requests.deleted = 0
					INNER JOIN 
						ClearView.dbo.cv_users
					ON 
						cv_forecast.userid = cv_users.userid
			ON 
				cv_forecast_answers.forecastid = cv_forecast.id
				And cv_forecast.deleted = 0
			LEFT OUTER JOIN 
				ClearView.dbo.cv_confidence
			ON 
				cv_forecast_answers.confidenceid = cv_confidence.id
			INNER JOIN 
				ClearView.dbo.cv_platforms
			ON 
				cv_forecast_answers.platformid = cv_platforms.platformid
			INNER JOIN 
				ClearView.dbo.cv_classs
			ON 
				cv_forecast_answers.classid = cv_classs.id
			INNER JOIN 
				ClearView.dbo.cv_environment
			ON 
				cv_forecast_answers.environmentid = cv_environment.id
			INNER JOIN 
				ClearView.dbo.cv_location_address
					INNER JOIN 
						ClearView.dbo.cv_location_city
							INNER JOIN 
								ClearView.dbo.cv_location_state
							ON 
								cv_location_city.stateid = cv_location_state.id
								AND cv_location_state.enabled = 1
								AND cv_location_state.deleted = 0
					ON 
						cv_location_address.cityid = cv_location_city.id
						AND cv_location_city.enabled = 1
						AND cv_location_city.deleted = 0
			ON 
				cv_forecast_answers.addressid = cv_location_address.id
				AND cv_location_address.enabled = 1
				AND cv_location_address.deleted = 0
				AND cv_location_address.storage=1
			LEFT OUTER JOIN 
				ClearView.dbo.cv_maintenance_windows
			ON 
				cv_forecast_answers.maintenanceid = cv_maintenance_windows.id
			INNER JOIN 
				ClearView.dbo.cv_forecast_answers_storage
			ON 
				cv_forecast_answers.id = cv_forecast_answers_storage.answerid
				And cv_forecast_answers_storage.deleted = 0
			LEFT OUTER JOIN	
				ClearView.dbo.cv_forecast_answers_backup
			ON 
				cv_forecast_answers.id = cv_forecast_answers_backup.answerid
				And cv_forecast_answers_backup.deleted = 0
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_1
			ON 
				cv_forecast_answers.appcontact = cv_users_1.userid
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_2
			ON 
				cv_forecast_answers.admin1 = cv_users_2.userid
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_3
			ON 
				cv_forecast_answers.admin2 = cv_users_3.userid
			LEFT JOIN 
				ClearView.dbo.cv_servers svr
			ON 
				ClearView.dbo.cv_forecast_answers.id = svr.answerid
				And svr.deleted = 0
			LEFT JOIN 
				ClearView.dbo.cv_servernames sn
			ON 
				svr.nameid = sn.id
				And sn.deleted = 0
				And sn.created IS NULL
	WHERE
		cv_forecast_answers.deleted = 0
		AND cv_forecast_answers.completed is null
--		AND cv_forecast_answers.executed is null
		--AND cv_forecast_answers.Confidenceid in (3,5)
		AND YEAR(cv_forecast_answers.implementation)>= (Year(GETDATE()) - 1)
		--AND cv_projects.number not in ('999999','1234510','CV1222682')
		
		AND cv_forecast_answers.forecastid 


/*--- */
IN (	SELECT 0
		UNION ALL
		SELECT id  FROM
			ClearView.dbo.cv_forecast cv_forecast
				INNER JOIN
					ClearView.dbo.cv_requests cv_requests
						INNER JOIN
							ClearView.dbo.cv_projects cv_projects
						ON
							cv_requests.projectid = cv_projects.projectid
							AND cv_projects.deleted = 0
				ON
					cv_forecast.requestid = cv_requests.requestid
					AND cv_requests.deleted = 0
		WHERE
			cv_forecast.deleted = 0
	)
) AS A
LEFT OUTER JOIN
clearview.dbo.cv_models_property cvModelsProperty
on A.modelid =cvModelsProperty.id

LEFT OUTER JOIN clearview.dbo.cv_models cvModels
ON cvModelsProperty.modelid=cvModels.id 
AND  cvModels.enabled = 1 	AND cvModels.deleted = 0

LEFT OUTER  JOIN ClearView.dbo.cv_types as cvTypes ON
			cvModels.typeid = cvTypes.id AND cvTypes.enabled = 1 
			AND cvTypes.deleted = 0
/*
WHERE (cvModels.typeid <> 58 or cvModels.typeid is null) */

GO
/****** Object:  View [dbo].[vwForeCastDetailsIM]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW 
[dbo].[vwForeCastDetailsIM] as
SELECT A.*,
CVModels.Name as ModelName,
cvModelsProperty.storage_ports as StoragePorts,
cvModelsProperty.network_ports as NetworkPorts,
cvModelsProperty.AMP as AMPs,
cvModelsProperty.replicate_times  as ReplicateTimes,
Datediff(day,ImplementationDate,getdate()) as NoOfDays,
cvTypes.ID as TypeID,
cvTypes.Name as TypeName,
CASE WHEN cvModelsProperty.type_blade =1 then 'Blade'
	 WHEN cvModelsProperty.type_physical =1 then 'Rack'
	 WHEN cvModelsProperty.type_vmware =1 then 'Virtual'
END  as TypeCatagory,
CASE cvModels.grouping
WHEN 1 then 'Distributed'
WHEN 2 then 'Midrange'
ELSE ''
END as TypeGrouping,
cvModelsProperty.type_blade as Blade,
(SELECT TOP 1 hostid FROM clearview.dbo.cv_models WHERE id = (SELECT TOP 1 modelid FROM clearview.dbo.cv_models_property WHERE id = a.MODELID)) as Host,
(SELECT TOP 1 storage FROM clearview.dbo.cv_hosts WHERE id = (SELECT TOP 1 hostid FROM clearview.dbo.cv_models WHERE id = (SELECT TOP 1 modelid FROM clearview.dbo.cv_models_property WHERE id = a.MODELID))) as host_storage,
/*isnull(cvModelsProperty.replicate_times * isnull(a.high_replicated+a.high_replicated_os,0) * (a.Recovery_Number/a.ServerQuantity),0)  as HighReplicatedFinal,*/
isnull(cvModelsProperty.replicate_times * isnull(a.high_replicated+a.high_replicated_os,0) ,0)  as HighReplicatedFinal,
isnull(cvModelsProperty.replicate_times * isnull(a.standard_replicated+a.standard_replicated_os,0) ,0) as StandardReplicatedFinal,
isnull(cvModelsProperty.replicate_times * isnull(a.low_replicated+a.low_replicated_os,0)  ,0) as LowReplicatedFinal

FROM
(SELECT
DISTINCT
cv_projects.ProjectID as ProjectID, 
cv_projects.number as ProjectNumber, 
cv_projects.name as ProjectName, 
cv_projects.BD as ProjectBaseDiscretion, 
cv_organizations.Name as ProjectPoftfolio,
RTRIM(isnull(cv_users_lead.lname,'')) + ', ' + RTRIM(isnull(cv_users_lead.fname,'')) AS ProjectManagerName, 
RTRIM(isnull(cv_users_engineer.lname,'')) + ', ' + RTRIM(isnull(cv_users_engineer.fname,'')) AS ProjectIntegrationEngineer, 
RTRIM(isnull(cv_users_ForeCastRequestedBy.lname,'')) + ', ' + RTRIM(isnull(cv_users_ForeCastRequestedBy.fname,'')) AS ForeCastRequestedBy, 

cv_forecast_answers.classid ,
cv_classs.name AS className ,
cv_forecast_answers.id as ForeCastAnswerId, 
/*cv_forecast_answers.modelid as Model,  */
CASE cv_forecast_answers.modelid 
	WHEN 0 THEN (SELECT Top 1 modelid from ClearView.dbo.fnGetForecastModel( cv_forecast_answers.id))
	 ELSE 
		cv_forecast_answers.MODELID
	 END MODELID,
cv_forecast_answers.Confidenceid,
cv_confidence.Name as ConfidenceName,
cv_confidence.display as ConfidenceDisplayOrder, 
isnull(cv_forecast_answers.Quantity,0) as ServerQuantity,
isnull(cv_forecast_answers.recovery_number,0) as Recovery_Number,
cv_forecast_answers.implementation as ImplementationDate,
cv_forecast_answers.environmentid as EnvironmentID,
/***********Storage Information**************/
isnull(cv_forecast_answers_storage.high,0) as high,
isnull(cv_forecast_answers_storage.high_total,0) as high_total,
isnull(cv_forecast_answers_storage.high_qa,0) as high_QA,
isnull(cv_forecast_answers_storage.high_test,0)as high_test,
isnull(cv_forecast_answers_storage.high_replicated,0)as high_replicated,

isnull(cv_forecast_answers_storage.standard,0) as standard,
isnull(cv_forecast_answers_storage.standard_total,0) as standard_total,
isnull(cv_forecast_answers_storage.standard_qa,0) as standard_QA,
isnull(cv_forecast_answers_storage.standard_test,0) as standard_test,
isnull(cv_forecast_answers_storage.standard_replicated,0) as standard_replicated,

isnull(cv_forecast_answers_storage.low,0) as low,
isnull(cv_forecast_answers_storage.low_total,0) as low_total,
isnull(cv_forecast_answers_storage.low_qa,0) as low_QA,
isnull(cv_forecast_answers_storage.low_test,0) as low_test,
isnull(cv_forecast_answers_storage.low_replicated,0) as low_replicated,


isnull(cv_forecast_answers_storage_os.high_total,0) AS high_total_os,
isnull(cv_forecast_answers_storage_os.high_qa,0) AS high_qa_os,
isnull(cv_forecast_answers_storage_os.high_test,0) AS high_test_os,
isnull(cv_forecast_answers_storage_os.high_replicated,0) AS high_replicated_os,
isnull(cv_forecast_answers_storage_os.high_level,0) AS high_level_os,
isnull(cv_forecast_answers_storage_os.standard_total,0) AS standard_total_os,
isnull(cv_forecast_answers_storage_os.standard_qa,0) AS standard_qa_os,
isnull(cv_forecast_answers_storage_os.standard_test,0) AS standard_test_os,
isnull(cv_forecast_answers_storage_os.standard_replicated ,0)AS standard_replicated_os,
isnull(cv_forecast_answers_storage_os.standard_level,0) AS standard_level_os,
isnull(cv_forecast_answers_storage_os.low_total,0) AS low_total_os,
isnull(cv_forecast_answers_storage_os.low_qa,0) AS low_qa_os,
isnull(cv_forecast_answers_storage_os.low_test,0) AS low_test_os,
isnull(cv_forecast_answers_storage_os.low_replicated,0) AS low_replicated_os,
isnull(cv_forecast_answers_storage_os.low_level,0) AS low_level_os,
/***********END Of Storage Information**************/
cv_environment.name as EnvironmentName,
cv_forecast_answers.AddressID,
cv_forecast_answers.created as ForeCastRequestDate,
cv_location_address.Name as Address,
cv_location_city.Name as City,
cv_location_state.Name as State
FROM
ClearView.dbo.cv_forecast_answers
			INNER JOIN 
				ClearView.dbo.cv_forecast
					INNER JOIN 
						ClearView.dbo.cv_requests
							INNER JOIN 
								ClearView.dbo.cv_projects
									INNER JOIN 
										ClearView.dbo.cv_organizations
									ON 
										cv_projects.organization = cv_organizations.organizationid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_segments
									ON 
										cv_projects.segmentid = cv_segments.id
									LEFT OUTER JOIN 
										ClearView.dbo.cv_users AS cv_users_lead
									ON 
										cv_projects.lead = cv_users_lead.userid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_users AS cv_users_engineer
									ON 
										cv_projects.engineer = cv_users_engineer.userid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_users AS cv_users_technical
									ON 
										cv_projects.technical = cv_users_technical.userid
							ON 
								cv_requests.projectid = cv_projects.projectid
								and cv_projects.deleted = 0
					ON 
						cv_forecast.requestid = cv_requests.requestid
						And cv_requests.deleted = 0
					INNER JOIN 
						ClearView.dbo.cv_users
					ON 
						cv_forecast.userid = cv_users.userid
			ON 
				cv_forecast_answers.forecastid = cv_forecast.id
				And cv_forecast.deleted = 0
			LEFT OUTER JOIN 
				ClearView.dbo.cv_confidence
			ON 
				cv_forecast_answers.confidenceid = cv_confidence.id
			INNER JOIN 
				ClearView.dbo.cv_platforms
			ON 
				cv_forecast_answers.platformid = cv_platforms.platformid
			INNER JOIN 
				ClearView.dbo.cv_classs
			ON 
				cv_forecast_answers.classid = cv_classs.id
			INNER JOIN 
				ClearView.dbo.cv_environment
			ON 
				cv_forecast_answers.environmentid = cv_environment.id
			INNER JOIN 
				ClearView.dbo.cv_location_address
					INNER JOIN 
						ClearView.dbo.cv_location_city
							INNER JOIN 
								ClearView.dbo.cv_location_state
							ON 
								cv_location_city.stateid = cv_location_state.id
								AND cv_location_state.enabled = 1
								AND cv_location_state.deleted = 0
					ON 
						cv_location_address.cityid = cv_location_city.id
						AND cv_location_city.enabled = 1
						AND cv_location_city.deleted = 0
			ON 
				cv_forecast_answers.addressid = cv_location_address.id
				AND cv_location_address.enabled = 1
				AND cv_location_address.deleted = 0
				AND cv_location_address.storage=1
			LEFT OUTER JOIN 
				ClearView.dbo.cv_maintenance_windows
			ON 
				cv_forecast_answers.maintenanceid = cv_maintenance_windows.id
			INNER JOIN 
				ClearView.dbo.cv_forecast_answers_storage
			ON 
				cv_forecast_answers.id = cv_forecast_answers_storage.answerid
				And cv_forecast_answers_storage.deleted = 0
			LEFT OUTER JOIN
				ClearView.dbo.cv_forecast_answers_storage_os
			ON
				cv_forecast_answers.id = cv_forecast_answers_storage_os.answerid
				AND cv_forecast_answers_storage_os.deleted = 0

			LEFT OUTER JOIN	
				ClearView.dbo.cv_forecast_answers_backup
			ON 
				cv_forecast_answers.id = cv_forecast_answers_backup.answerid
				And cv_forecast_answers_backup.deleted = 0

			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_ForeCastRequestedBy
			ON 
				cv_forecast_answers.userid = cv_users_ForeCastRequestedBy.userid
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_1
			ON 
				cv_forecast_answers.appcontact = cv_users_1.userid
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_2
			ON 
				cv_forecast_answers.admin1 = cv_users_2.userid
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_3
			ON 
				cv_forecast_answers.admin2 = cv_users_3.userid
			LEFT JOIN 
				ClearView.dbo.cv_servers svr
			ON 
				ClearView.dbo.cv_forecast_answers.id = svr.answerid
				And svr.deleted = 0
			LEFT JOIN 
				ClearView.dbo.cv_servernames sn
			ON 
				svr.nameid = sn.id
				And sn.deleted = 0
				And sn.created IS NULL
	WHERE
		cv_forecast_answers.deleted = 0
		AND cv_forecast_answers.completed is null
--		AND cv_forecast_answers.executed is null
		--AND cv_forecast_answers.Confidenceid in (3,5)
		AND YEAR(cv_forecast_answers.implementation)>= (Year(GETDATE()) - 1)
		AND cv_projects.number not in ('999999','1234510','CV1222682')
		
		AND cv_forecast_answers.forecastid 


/*--- */
IN (	SELECT 0
		UNION ALL
		SELECT id  FROM
			ClearView.dbo.cv_forecast cv_forecast
				INNER JOIN
					ClearView.dbo.cv_requests cv_requests
						INNER JOIN
							ClearView.dbo.cv_projects cv_projects
						ON
							cv_requests.projectid = cv_projects.projectid
							AND cv_projects.deleted = 0
				ON
					cv_forecast.requestid = cv_requests.requestid
					AND cv_requests.deleted = 0
		WHERE
			cv_forecast.deleted = 0
	)
) AS A
LEFT OUTER JOIN
clearview.dbo.cv_models_property cvModelsProperty
on A.modelid =cvModelsProperty.id

LEFT OUTER JOIN clearview.dbo.cv_models cvModels
ON cvModelsProperty.modelid=cvModels.id 
AND  cvModels.enabled = 1 	AND cvModels.deleted = 0

LEFT OUTER  JOIN ClearView.dbo.cv_types as cvTypes ON
			cvModels.typeid = cvTypes.id AND cvTypes.enabled = 1 
			AND cvTypes.deleted = 0
/*
WHERE (cvModels.typeid <> 58 or cvModels.typeid is null) */
WHERE 
(SELECT TOP 1 hostid FROM clearview.dbo.cv_models WHERE id = (SELECT TOP 1 modelid FROM clearview.dbo.cv_models_property WHERE id = a.MODELID)) =0
OR
((SELECT TOP 1 storage FROM clearview.dbo.cv_hosts WHERE id = (SELECT TOP 1 hostid FROM clearview.dbo.cv_models WHERE id = (SELECT TOP 1 modelid FROM clearview.dbo.cv_models_property WHERE id = a.MODELID))) is null OR
(SELECT TOP 1 storage FROM clearview.dbo.cv_hosts WHERE id = (SELECT TOP 1 hostid FROM clearview.dbo.cv_models WHERE id = (SELECT TOP 1 modelid FROM clearview.dbo.cv_models_property WHERE id = a.MODELID))) =0)

GO
/****** Object:  View [dbo].[vwForeCastModel]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwForeCastModel] 
as
Select a.ModelID,sum(a.Quantity) as TotalForecast,sum(a.Recovery_Number) as RecoveryNumber, A.AddressID,a.Confidenceid,
a.classid,a.class
FROM
(
select Distinct
cv_forecast_answers.id, 
cv_forecast_answers.modelid as Model, 
CASE cv_forecast_answers.modelid 
	WHEN 0 THEN (SELECT Top 1 modelid from ClearView.dbo.fnGetForecastModel( cv_forecast_answers.id))
	 ELSE 
		cv_forecast_answers.MODELID
	 END MODELID,
isnull(cv_forecast_answers.Quantity,0) as Quantity,
isnull(cv_forecast_answers.recovery_number,0) as Recovery_Number,
cv_forecast_answers.AddressID,
cv_location_address.name as Address,
cv_forecast_answers.Confidenceid,
cv_forecast_answers.classid,
ClearView.dbo.cv_classs.name as Class
from 
ClearView.dbo.cv_forecast_answers
			INNER JOIN 
				ClearView.dbo.cv_forecast
					INNER JOIN 
						ClearView.dbo.cv_requests
							INNER JOIN 
								ClearView.dbo.cv_projects
									INNER JOIN 
										ClearView.dbo.cv_organizations
									ON 
										cv_projects.organization = cv_organizations.organizationid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_segments
									ON 
										cv_projects.segmentid = cv_segments.id
									LEFT OUTER JOIN 
										ClearView.dbo.cv_users AS cv_users_lead
									ON 
										cv_projects.lead = cv_users_lead.userid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_users AS cv_users_engineer
									ON 
										cv_projects.engineer = cv_users_engineer.userid
									LEFT OUTER JOIN	
										ClearView.dbo.cv_users AS cv_users_technical
									ON 
										cv_projects.technical = cv_users_technical.userid
							ON 
								cv_requests.projectid = cv_projects.projectid
								and cv_projects.deleted = 0
					ON 
						cv_forecast.requestid = cv_requests.requestid
						And cv_requests.deleted = 0
					INNER JOIN 
						ClearView.dbo.cv_users
					ON 
						cv_forecast.userid = cv_users.userid
			ON 
				cv_forecast_answers.forecastid = cv_forecast.id
				And cv_forecast.deleted = 0
			LEFT OUTER JOIN 
				ClearView.dbo.cv_confidence
			ON 
				cv_forecast_answers.confidenceid = cv_confidence.id
			INNER JOIN 
				ClearView.dbo.cv_platforms
			ON 
				cv_forecast_answers.platformid = cv_platforms.platformid
			INNER JOIN 
				ClearView.dbo.cv_classs
			ON 
				cv_forecast_answers.classid = cv_classs.id
			INNER JOIN 
				ClearView.dbo.cv_environment
			ON 
				cv_forecast_answers.environmentid = cv_environment.id
			INNER JOIN 
				ClearView.dbo.cv_location_address
					INNER JOIN 
						ClearView.dbo.cv_location_city
							INNER JOIN 
								ClearView.dbo.cv_location_state
							ON 
								cv_location_city.stateid = cv_location_state.id
								AND cv_location_state.enabled = 1
								AND cv_location_state.deleted = 0
					ON 
						cv_location_address.cityid = cv_location_city.id
						AND cv_location_city.enabled = 1
						AND cv_location_city.deleted = 0
			ON 
				cv_forecast_answers.addressid = cv_location_address.id
				AND cv_location_address.enabled = 1
				AND cv_location_address.deleted = 0
			LEFT OUTER JOIN 
				ClearView.dbo.cv_maintenance_windows
			ON 
				cv_forecast_answers.maintenanceid = cv_maintenance_windows.id
			LEFT OUTER JOIN 
				ClearView.dbo.cv_forecast_answers_storage
			ON 
				cv_forecast_answers.id = cv_forecast_answers_storage.answerid
				And cv_forecast_answers_storage.deleted = 0
			LEFT OUTER JOIN	
				ClearView.dbo.cv_forecast_answers_backup
			ON 
				cv_forecast_answers.id = cv_forecast_answers_backup.answerid
				And cv_forecast_answers_backup.deleted = 0
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_1
			ON 
				cv_forecast_answers.appcontact = cv_users_1.userid
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_2
			ON 
				cv_forecast_answers.admin1 = cv_users_2.userid
			LEFT OUTER JOIN	
				ClearView.dbo.cv_users AS cv_users_3
			ON 
				cv_forecast_answers.admin2 = cv_users_3.userid
			LEFT JOIN 
				ClearView.dbo.cv_servers svr
			ON 
				ClearView.dbo.cv_forecast_answers.id = svr.answerid
				And svr.deleted = 0
			LEFT JOIN 
				ClearView.dbo.cv_servernames sn
			ON 
				svr.nameid = sn.id
				And sn.deleted = 0
				And sn.created IS NULL
	WHERE
		cv_forecast_answers.deleted = 0
		AND cv_forecast_answers.completed is null
		AND cv_forecast_answers.executed is null
		/*AND cv_forecast_answers.Confidenceid in (3,5)
		AND YEAR(cv_forecast_answers.implementation)>= (Year(GETDATE()) - 1)*/
		AND cv_projects.number not in ('999999','1234510','CV1222682')
		AND cv_forecast_answers.forecastid 


/*--- */
IN (	SELECT 0
		UNION ALL
		SELECT id  FROM
			ClearView.dbo.cv_forecast cv_forecast
				INNER JOIN
					ClearView.dbo.cv_requests cv_requests
						INNER JOIN
							ClearView.dbo.cv_projects cv_projects
						ON
							cv_requests.projectid = cv_projects.projectid
							AND cv_projects.deleted = 0
				ON
					cv_forecast.requestid = cv_requests.requestid
					AND cv_requests.deleted = 0
		WHERE
			cv_forecast.deleted = 0
	)
) as  a
Group by a.ModelID, a.AddressID,a.Confidenceid,a.classid,a.class

GO
/****** Object:  View [dbo].[vwModelDetails]    Script Date: 07/31/2009 13:23:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[vwModelDetails]
as
SELECT  
cvModelsProperty.id as ModelID,
cvModelsProperty.Name as ModelName ,
cvTypes.Name as Platform
FROM
ClearView.dbo.cv_models_property  as cvModelsProperty
INNER JOIN  ClearView.dbo.cv_models as cvModels ON
			cvModelsProperty.modelid=cvModels.ID AND  cvModels.enabled = 1 
			AND cvModels.deleted = 0
INNER JOIN  ClearView.dbo.cv_types as cvTypes ON
			cvModels.typeid = cvTypes.id AND cvTypes.enabled = 1 
			AND cvTypes.deleted = 0 

