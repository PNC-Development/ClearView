/*==============================================================
The purpose of this script is to create procedures, functions and synonyms
==============================================================*/

ALTER view [dbo].[vwServers]
/*********************************************************************************
Procedure Name		:vwServers
Description			:VIEW to get Server Info for Servers, Blades, VMWARE 
Input Parameters	:	
Output Parameters	: 
Date Created:       : 05-Aug-2009
Author:             : Shyam Pathade
----------------------------------------------------------------------------------
Modification History
Modified By		Date		Description

Select * from vwServers where projectid =
*********************************************************************************/ 
AS
/* Server */
SELECT 
cvServers.*
,CASE WHEN vwForecastAnswers.FAIsOverrided=1 OR cvDesigns.is_exception = 1 THEN 1
		ELSE 0
END AS IsOverride
,cvDesigns.comments AS OverrideComments
,cvDesigns.created AS DesignCreated
,cvMnemonics.ATLName
,cvMnemonics.PMName
,cvMnemonics.FMName
,cvMnemonics.DMName
,cvMnemonics.CIO
,cvMnemonics.AppOwner
,cvMnemonics.RiskManager
,cvMnemonics.BRContact
,cvMnemonics.Status
,cvMnemonics.ResRating
,vwForecastAnswers.FAImplementationDate as CommitmentDate
,vwForecastAnswers.FAExecutionDate as ScheduledExecutionDate
,vwForecastAnswers.FAExecutedDate as ExecutedDate
,vwForecastAnswers.FAMnemonicCode as MnemonicCode
,vwForecastAnswers.FAProductionDate as ScheduledProductionDate
,CASE WHEN cvClasss.Prod=1 THEN vwForecastAnswers.FAFinishedDate 
	  ELSE vwForecastAnswers.FACompletedDate 
 END As CommissionDate

,vwForecastAnswers.FAAddressId
,vwForecastAnswers.Address
,vwForecastAnswers.CityName
,vwForecastAnswers.Statecode
,cvLocationsDR.LocationId AS DRAddressId
,cvLocationsDR.Name AS DRAddress
,cvLocationsDR.CityName AS DRCityName
,cvLocationsDR.StateCode AS DRStateCode
,vwForecastAnswers.FAProjectId
,vwForecastAnswers.ProjectName,vwForecastAnswers.ProjectNumber
,vwForecastAnswers.ProjectBaseDiscretionary
,vwForecastAnswers.OrganizationName
,vwForecastAnswers.LeadId,vwForecastAnswers.ProjectLeadName
,vwForecastAnswers.EngineerId,vwForecastAnswers.ProjectEngineerName
,vwForecastAnswers.TechLeadId,vwForecastAnswers.ProjectTechLeadName
,vwForecastAnswers.FAClassId,vwForecastAnswers.Class,vwForecastAnswers.IsPNCClass
,vwForecastAnswers.FAEnvironmentId,vwForecastAnswers.Environment
,vwForecastAnswers.FAApplicationId
,cvApplications.Name as ApplicationName
,vwForecastAnswers.FASubApplicationId
,vwForecastAnswers.FAMnemonicName,vwForecastAnswers.FAMnemonicCode
,vwForecastAnswers.FAExecutedBy, vwForecastAnswers.FAExecutedByUserName
,vwForecastAnswers.FAUserName 
,vwForecastAnswers.FAUserAppContact,vwForecastAnswers.FAUserPrimaryContact
,vwForecastAnswers.FAUserSecondaryContact,vwForecastAnswers.FAUserAppOwner

,ISNULL(cvModels.MpName,'Unknown Server') As ModelName
,cvModels.ModelMake
,cvModels.MpStorageType  AS ModelStorageType
,cvModels.ModelTypeId ,cvModels.ModelTypeName
,cvModels.MPAssetCategoryId as ModelAssetCategoryId,MPAssetCategory as ModelAssetCategory
,cvModels.MpReplicatedTimes 
,cvModels.MpAMP ,cvModels.MpNetWorkPorts,cvModels.MpStoragePorts
,cvModels.MpRAM,cvModels.MpCPUCount,cvModels.MpCPUSpeed
,case when (cvModels.ModelDestroy=0) then 'Reuseable'
	 when (cvModels.ModelDestroy=1) then 'Non-Reusable'
	 else 'Other'
End as ModelDispositionStatus
,cvModels.ModelGrouping

,CASE 
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastAcquisitionNonProdCost + cvModels.ModelForecastAcquisitionProdCost
	ELSE cvModels.ModelForecastAcquisitionNonProdCost
END ModelForecastAcquisitionCost
,CASE  
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastOperationsNonProdCost + cvModels.ModelForecastOperationsProdCost
	ELSE cvModels.ModelForecastOperationsNonProdCost
END  ModelForecastOperationsCost

,CASE  
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastStreetValNonProdCost + cvModels.ModelForecastStreetValProdCost
	ELSE cvModels.ModelForecastStreetValNonProdCost
END ModelForecastStreetValCost

,cvAssets.Serial as AssetSerialNo
,cvAssetsDR.Serial AS DRAssetSerialNo
,cvAssets.AssetTag as AssetTag
,cvAssetsDR.AssetTag AS DRAssetTag
,cvAssets.AssetStatusId
,AssetStatus.StatusDescription as AssetStatus
,cvAssetOrders.PurchaseOrderNumber
,cvAssetOrders.PurchaseOrderDate
,cvAssetOrders.WarrantyDate
,vwForecastAnswers.FAPlatformId,vwForecastAnswers.Platform 
,cvDomains.name as Domain
,cvAssetsServers.RoomId
,cvRooms.name as Room
,cvAssetsServers.RackId
,cvRacks.name as Rack
,cvRacks.Zone as Zone
,cvAssetsServers.RackPosition
,NULL AS Slot
,cvAssetsServersDR.RoomId AS DRRoomId
,cvRoomsDR.name as DRRoom
,cvAssetsServersDR.RackId AS DRRackId
,cvRacksDR.name as DRRack
,cvRacksDR.Zone as DRZone
,cvAssetsServersDR.RackPosition AS DRRackPosition
,NULL AS DRSlot
,cvOperatingSystems.Name as OperatingSystem,cvServicePacks.Name as ServicePack
,cvAssetsServers.DummyName 
,cvAssetsServersDR.DummyName AS DRDummyName
,cvAssetsServers.MACAddress
,cvAssetsServersDR.MACAddress AS DRMACAddress
,cvAssetsServers.ILO
,cvAssetsServersDR.ILO AS DRILO
,cvOperatingSystemGroups.Name AS OperatingSystemGroup
,NULL as EnclosureName
,NULL as DREnclosureName
,CASE WHEN  Exists(Select * from cvForecastAnswersPlatform 
	WHERE FAPlatformQuestionId =6 and FAPlatformResponseId=15 AND 
		  ForecastAnswerId=cvServers.AnswerId) THEN 1
ELSE 0 
END As SpecialHardware
,CASE WHEN  Exists(Select * from cvForecastAnswersPlatform 
	WHERE FAResponseHACSM =1 AND ForecastAnswerId=cvServers.AnswerId) THEN 1
ELSE 0 
END As LoadBalancing

,CASE WHEN cvServers.ReclaimedStorageAmt IS NOT NULL AND cast(cvServers.ReclaimedStorageAmt  AS float) > 0.00 THEN
		 cast(cvServers.ReclaimedStorageAmt  AS float) 
	 WHEN _cvExactStorageAmts.StorageAmt IS NOT NULL  AND cast(_cvExactStorageAmts.StorageAmt  AS float) > 0.00 THEN
		 cast(_cvExactStorageAmts.StorageAmt  AS float) 
	ELSE  cast(cvServers.ReclaimedStorage  AS float) 
END as SANStorageAmt
,CASE WHEN cvServers.ReclaimedStorageTier IS NULL OR cvServers.ReclaimedStorageTier=0 THEN
		 _cvExactStorageAmts.Tier
	ELSE  cast(cvServers.ReclaimedStorageTier as varchar(50) )
END as SANStorageTier
,CASE WHEN cvServers.ReclaimedStorageEnvironment IS NULL OR cvServers.ReclaimedStorageEnvironment='' THEN
		 _cvExactStorageAmts.Environment 
	ELSE  cvServers.ReclaimedStorageEnvironment  
END as SANStorageEnvironment

,CAST((SELECT TOP 1 Switch FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id ASC) AS VARCHAR(100)) AS SwitchA
,CAST((
	CASE
		WHEN (SELECT TOP 1 nexus FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id ASC) = 0
			THEN (SELECT TOP 1 interface FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id ASC) 
		ELSE (SELECT TOP 1 CAST(blade AS VARCHAR(20)) + '/' + CAST(port AS VARCHAR(20)) FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id ASC) 
	END
) AS VARCHAR(50)) AS SwitchPortA
,CAST((SELECT TOP 1 Switch FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id DESC) AS VARCHAR(100)) AS SwitchB
,CAST((
	CASE
		WHEN (SELECT TOP 1 nexus FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id DESC) = 0
			THEN (SELECT TOP 1 interface FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id DESC) 
		ELSE (SELECT TOP 1 CAST(blade AS VARCHAR(20)) + '/' + CAST(port AS VARCHAR(20)) FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.AssetId ORDER BY Id DESC) 
	END
) AS VARCHAR(50)) AS SwitchPortB
,cvProjects.ProjectNumber AS AssetOrderProjectNumber
,CAST((SELECT TOP 1 Switch FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id ASC) AS VARCHAR(100)) AS DRSwitchA
,CAST((
	CASE
		WHEN (SELECT TOP 1 nexus FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id ASC) = 0
			THEN (SELECT TOP 1 interface FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id ASC) 
		ELSE (SELECT TOP 1 CAST(blade AS VARCHAR(20)) + '/' + CAST(port AS VARCHAR(20)) FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id ASC) 
	END
) AS VARCHAR(50)) AS DRSwitchPortA
,CAST((SELECT TOP 1 Switch FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id DESC) AS VARCHAR(100)) AS DRSwitchB
,CAST((
	CASE
		WHEN (SELECT TOP 1 nexus FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id DESC) = 0
			THEN (SELECT TOP 1 interface FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id DESC) 
		ELSE (SELECT TOP 1 CAST(blade AS VARCHAR(20)) + '/' + CAST(port AS VARCHAR(20)) FROM cvAssetSwitchports WHERE cvAssetSwitchports.[type] = 1 AND cvAssetSwitchports.AssetId = cvServers.DRServerAssetId ORDER BY Id DESC) 
	END
) AS VARCHAR(50)) AS DRSwitchPortB

FROM cvServers 
INNER JOIN vwForecastAnswers
	INNER JOIN cvClasss
		ON vwForecastAnswers.FAClassId =cvClasss.ClassId
	ON cvServers.AnswerId = vwForecastAnswers.ForecastAnswerId
	AND vwForecastAnswers.Environment IS NOT NULL AND vwForecastAnswers.Platform IS NOT NULL	
	AND vwForecastAnswers.FAAddressId >0 
	LEFT OUTER JOIN cvDesigns
		ON vwForecastAnswers.ForecastAnswerId =cvDesigns.AnswerId
	LEFT OUTER JOIN ClearView.dbo.cv_mnemonics cvMnemonics
		ON vwForecastAnswers.FAMnemonicId=cvMnemonics.Id 
		AND cvMnemonics.deleted=0
INNER JOIN cvModels
	ON  cvServers.ModelId= cvModels.ModelPropertyId
INNER JOIN cvDomains
	ON cvServers.domainid =cvDomains.DomainId
INNER JOIN cvAssets
	LEFT OUTER JOIN cvAssetOrders
		LEFT OUTER JOIN cvProjects 
			ON	cvAssetOrders.ProjectId = cvProjects.ProjectId
		ON cvAssets.OrderID= cvAssetOrders.OrderID 
	INNER JOIN (Select StatusValue, StatusDescription FROM StatusList where StatusKey='ASSETSTATUS') as AssetStatus
		ON cvAssets.AssetStatusId=AssetStatus.StatusValue
	ON cvServers.AssetID =cvAssets.AssetId  
INNER JOIN cvAssetsServers
	LEFT OUTER JOIN cvOperatingSystemGroups
		ON cvAssetsServers.OperatingSystemGroupID = cvOperatingSystemGroups.OperatingSystemGroupID 
	ON cvServers.AssetID= cvAssetsServers.AssetId 
LEFT OUTER JOIN cvRooms
	ON cvAssetsServers.RoomId = cvRooms.RoomId
LEFT OUTER JOIN  cvRacks
	ON cvAssetsServers.RackId =cvRacks.RackId
LEFT OUTER JOIN cvAssets AS cvAssetsDR
	ON cvServers.DRServerAssetId =cvAssetsDR.AssetId  
LEFT OUTER JOIN cvAssetsServers AS cvAssetsServersDR
	ON cvServers.DRServerAssetId= cvAssetsServersDR.AssetId 
INNER JOIN cvLocations AS cvLocationsDR
	ON cvAssetsServersDR.AddressId= cvLocationsDR.LocationId 
LEFT OUTER JOIN cvRooms AS cvRoomsDR
	ON cvAssetsServersDR.RoomId = cvRoomsDR.RoomId
LEFT OUTER JOIN  cvRacks AS cvRacksDR
	ON cvAssetsServersDR.RackId =cvRacksDR.RackId
INNER JOIN cvOperatingSystems
	ON cvServers.OSId =cvOperatingSystems.OperatingSystemId
LEFT OUTER JOIN  cvServicePacks
	ON cvServers.SPID = cvServicePacks.ServicePackId
LEFT OUTER JOIN cvApplications 
	ON vwForecastAnswers.FAApplicationId=cvApplications.ApplicationId
OUTER APPLY
		(Select TOP 1  * from _cvExactStorageAmts ExtractStorageAmt
		WHERE ExtractStorageAmt.Host =
		cvServers.ServerName) AS _cvExactStorageAmts  


WHERE cvServers.NameId Is Not Null AND cvServers.NameId>0
/* End of Server */
UNION ALL
/* Blades */
select 
cvServers.*
,CASE WHEN vwForecastAnswers.FAIsOverrided=1 OR cvDesigns.is_exception = 1 THEN 1
		ELSE 0
END AS IsOverride
,cvDesigns.comments AS OverrideComments
,cvDesigns.created AS DesignCreated
,cvMnemonics.ATLName
,cvMnemonics.PMName
,cvMnemonics.FMName
,cvMnemonics.DMName
,cvMnemonics.CIO
,cvMnemonics.AppOwner
,cvMnemonics.RiskManager
,cvMnemonics.BRContact
,cvMnemonics.Status
,cvMnemonics.ResRating
,vwForecastAnswers.FAImplementationDate as CommitmentDate
,vwForecastAnswers.FAExecutionDate as ScheduledExecutionDate
,vwForecastAnswers.FAExecutedDate as ExecutedDate
,vwForecastAnswers.FAMnemonicCode as MnemonicCode
,vwForecastAnswers.FAProductionDate as ScheduledProductionDate
,CASE WHEN cvClasss.Prod=1 THEN vwForecastAnswers.FAFinishedDate 
	  ELSE vwForecastAnswers.FACompletedDate 
 END As CommissionDate

,vwForecastAnswers.FAAddressId
,vwForecastAnswers.Address
,vwForecastAnswers.CityName
,vwForecastAnswers.Statecode
,vwEnclosureDetailsDR.AddressId AS DRAddressId
,vwEnclosureDetailsDR.Address AS DRAddress
,vwEnclosureDetailsDR.CityName AS DRCityName
,vwEnclosureDetailsDR.StateCode AS DRStateCode
,vwForecastAnswers.FAProjectId
,vwForecastAnswers.ProjectName,vwForecastAnswers.ProjectNumber
,vwForecastAnswers.ProjectBaseDiscretionary
,vwForecastAnswers.OrganizationName
,vwForecastAnswers.LeadId,vwForecastAnswers.ProjectLeadName
,vwForecastAnswers.EngineerId,vwForecastAnswers.ProjectEngineerName
,vwForecastAnswers.TechLeadId,vwForecastAnswers.ProjectTechLeadName
,vwForecastAnswers.FAClassId,vwForecastAnswers.Class,vwForecastAnswers.IsPNCClass
,vwForecastAnswers.FAEnvironmentId,vwForecastAnswers.Environment
,vwForecastAnswers.FAApplicationId
,cvApplications.Name as ApplicationName
,vwForecastAnswers.FASubApplicationId
,vwForecastAnswers.FAMnemonicName,vwForecastAnswers.FAMnemonicCode
,vwForecastAnswers.FAExecutedBy, vwForecastAnswers.FAExecutedByUserName
,vwForecastAnswers.FAUserName 
,vwForecastAnswers.FAUserAppContact,vwForecastAnswers.FAUserPrimaryContact
,vwForecastAnswers.FAUserSecondaryContact,vwForecastAnswers.FAUserAppOwner

,ISNULL(cvModels.MpName,'Unknown Server') As ModelName
,cvModels.ModelMake
,cvModels.MpStorageType  AS ModelStorageType
,cvModels.ModelTypeId ,cvModels.ModelTypeName 
,cvModels.MPAssetCategoryId as ModelAssetCategoryId,MPAssetCategory as ModelAssetCategory
,cvModels.MpReplicatedTimes 
,cvModels.MpAMP ,cvModels.MpNetWorkPorts,cvModels.MpStoragePorts
,cvModels.MpRAM,cvModels.MpCPUCount,cvModels.MpCPUSpeed
,case when (cvModels.ModelDestroy=0) then 'Reuseable'
	 when (cvModels.ModelDestroy=1) then 'Non-Reusable'
	 else 'Other'
End as ModelDispositionStatus
,cvModels.ModelGrouping
,CASE 
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastAcquisitionNonProdCost + cvModels.ModelForecastAcquisitionProdCost
	ELSE cvModels.ModelForecastAcquisitionNonProdCost
END ModelForecastAcquisitionCost
,CASE  
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastOperationsNonProdCost + cvModels.ModelForecastOperationsProdCost
	ELSE cvModels.ModelForecastOperationsNonProdCost
END  ModelForecastOperationsCost

,CASE  
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastStreetValNonProdCost + cvModels.ModelForecastStreetValProdCost
	ELSE cvModels.ModelForecastStreetValNonProdCost
END ModelForecastStreetValCost

,cvAssets.Serial as AssetSerialNo
,cvAssetsDR.Serial AS DRAssetSerialNo
,cvAssets.AssetTag as AssetTag
,cvAssetsDR.AssetTag AS DRAssetTag
,cvAssets.AssetStatusId
,AssetStatus.StatusDescription as AssetStatus
,cvAssetOrders.PurchaseOrderNumber
,cvAssetOrders.PurchaseOrderDate
,cvAssetOrders.WarrantyDate
,vwForecastAnswers.FAPlatformId,vwForecastAnswers.Platform 
,cvDomains.name as Domain
,vwEnclosureDetails.RoomId
,vwEnclosureDetails.Room as Room
,vwEnclosureDetails.RackId
,vwEnclosureDetails.Rack as Rack
,vwEnclosureDetails.Zone as Zone
,vwEnclosureDetails.RackPosition
,cvAssetsBlades.Slot
,vwEnclosureDetailsDR.RoomId AS DRRoomId
,vwEnclosureDetailsDR.Room as DRRoom
,vwEnclosureDetailsDR.RackId AS DRRackId
,vwEnclosureDetailsDR.Rack as DRRack
,vwEnclosureDetailsDR.Zone as DRZone
,vwEnclosureDetailsDR.RackPosition AS DRRackPosition
,cvAssetsBladesDR.Slot AS DRSlot
,cvOperatingSystems.Name as OperatingSystem
,cvServicePacks.Name as ServicePack
,cvAssetsBlades.DummyName 
,cvAssetsBladesDR.DummyName AS DRDummyName
,cvAssetsBlades.MACAddress
,cvAssetsBladesDR.MACAddress AS DRMACAddress
,cvAssetsBlades.ILO
,cvAssetsBladesDR.ILO AS DRILO
,cvOperatingSystemGroups.Name AS OperatingSystemGroup
,vwEnclosureDetails.AssetName as EnclosureName
,vwEnclosureDetails.DRAssetName as DREnclosureName
,CASE WHEN  Exists(Select * from cvForecastAnswersPlatform 
	WHERE FAPlatformQuestionId =6 and FAPlatformResponseId=15 AND 
		  ForecastAnswerId=cvServers.AnswerId) THEN 1
ELSE 0 
END As SpecialHardware
,CASE WHEN  Exists(Select * from cvForecastAnswersPlatform 
	WHERE FAResponseHACSM =1 AND ForecastAnswerId=cvServers.AnswerId) THEN 1
ELSE 0 
END As LoadBalancing

,CASE WHEN cvServers.ReclaimedStorageAmt IS NOT NULL AND cast(cvServers.ReclaimedStorageAmt  AS float) > 0.00 THEN
		 cast(cvServers.ReclaimedStorageAmt  AS float) 
	 WHEN _cvExactStorageAmts.StorageAmt IS NOT NULL  AND cast(_cvExactStorageAmts.StorageAmt  AS float) > 0.00 THEN
		 cast(_cvExactStorageAmts.StorageAmt  AS float) 
	ELSE  cast(cvServers.ReclaimedStorage  AS float) 
END as SANStorageAmt
,CASE WHEN cvServers.ReclaimedStorageTier IS NULL OR cvServers.ReclaimedStorageTier=0 THEN
		 _cvExactStorageAmts.Tier
	ELSE  cast(cvServers.ReclaimedStorageTier as varchar(50) )
END as SANStorageTier
,CASE WHEN cvServers.ReclaimedStorageEnvironment IS NULL OR cvServers.ReclaimedStorageEnvironment='' THEN
		 _cvExactStorageAmts.Environment 
	ELSE  cvServers.ReclaimedStorageEnvironment  
END as SANStorageEnvironment

,CAST((SELECT TOP 1 switchA FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetails.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBlades.Slot) AS VARCHAR(100)) AS SwitchA
,CAST((SELECT TOP 1 interfaceA FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetails.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBlades.Slot) AS VARCHAR(50)) AS SwitchPortA
,CAST((SELECT TOP 1 switchB FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetails.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBlades.Slot) AS VARCHAR(100)) AS SwitchB
,CAST((SELECT TOP 1 interfaceB FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetails.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBlades.Slot) AS VARCHAR(50)) AS SwitchPortB
,cvProjects.ProjectNumber AS AssetOrderProjectNumber
,CAST((SELECT TOP 1 switchA FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetailsDR.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBladesDR.Slot) AS VARCHAR(100)) AS DRSwitchA
,CAST((SELECT TOP 1 interfaceA FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetailsDR.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBladesDR.Slot) AS VARCHAR(50)) AS DRSwitchPortA
,CAST((SELECT TOP 1 switchB FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetailsDR.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBladesDR.Slot) AS VARCHAR(100)) AS DRSwitchB
,CAST((SELECT TOP 1 interfaceB FROM cvAssetDellSwitchports WHERE cvAssetDellSwitchports.enclosure = vwEnclosureDetailsDR.AssetName AND cvAssetDellSwitchports.slot = cvAssetsBladesDR.Slot) AS VARCHAR(50)) AS DRSwitchPortB

FROM cvServers 
INNER JOIN vwForecastAnswers
	INNER JOIN cvClasss
		ON vwForecastAnswers.FAClassId =cvClasss.ClassId
	ON cvServers.AnswerId = vwForecastAnswers.ForecastAnswerId
	AND vwForecastAnswers.Environment IS NOT NULL AND vwForecastAnswers.Platform IS NOT NULL	
	AND vwForecastAnswers.FAAddressId >0 
	LEFT OUTER JOIN cvDesigns
		ON vwForecastAnswers.ForecastAnswerId =cvDesigns.AnswerId
	LEFT OUTER JOIN ClearView.dbo.cv_mnemonics cvMnemonics
		ON vwForecastAnswers.FAMnemonicId=cvMnemonics.Id 
		AND cvMnemonics.deleted=0
INNER JOIN cvModels
	ON  cvServers.ModelId= cvModels.ModelPropertyId
INNER JOIN cvDomains
	ON cvServers.domainid =cvDomains.DomainId
INNER JOIN cvAssets
	LEFT OUTER JOIN cvAssetOrders
		LEFT OUTER JOIN cvProjects 
			ON	cvAssetOrders.ProjectId = cvProjects.ProjectId
		ON cvAssets.OrderID= cvAssetOrders.OrderID 
	INNER JOIN (Select StatusValue, StatusDescription FROM StatusList where StatusKey='ASSETSTATUS') as AssetStatus
		ON cvAssets.AssetStatusId=AssetStatus.StatusValue
	ON cvServers.AssetID =cvAssets.AssetId  
INNER JOIN cvAssetsBlades
	LEFT OUTER JOIN cvOperatingSystemGroups
		ON cvAssetsBlades.OperatingSystemGroupID = cvOperatingSystemGroups.OperatingSystemGroupID 
	ON cvServers.AssetID= cvAssetsBlades.AssetId 
LEFT OUTER JOIN cvAssets AS cvAssetsDR
	ON cvServers.DRServerAssetId =cvAssetsDR.AssetId  
LEFT OUTER JOIN cvAssetsBlades AS cvAssetsBladesDR
	ON cvServers.DRServerAssetId= cvAssetsBladesDR.AssetId 
LEFT OUTER JOIN vwEnclosureDetails
		ON cvAssetsBlades.EnclosureId = vwEnclosureDetails.assetid
LEFT OUTER JOIN vwEnclosureDetails AS vwEnclosureDetailsDR
		ON cvAssetsBladesDR.EnclosureId = vwEnclosureDetailsDR.assetid
INNER JOIN cvOperatingSystems
	ON cvServers.OSId =cvOperatingSystems.OperatingSystemId
LEFT OUTER JOIN  cvServicePacks
	ON cvServers.SPID = cvServicePacks.ServicePackId
LEFT OUTER JOIN cvApplications 
	ON vwForecastAnswers.FAApplicationId=cvApplications.ApplicationId
OUTER APPLY
		(Select TOP 1  * from _cvExactStorageAmts ExtractStorageAmt
		WHERE ExtractStorageAmt.Host =
		cvServers.ServerName) AS _cvExactStorageAmts  

WHERE cvServers.NameId Is Not Null AND cvServers.NameId>0
/* End of Blades */
UNION ALL
/* VWMARE */
select 
cvServers.*
,CASE WHEN vwForecastAnswers.FAIsOverrided=1 OR cvDesigns.is_exception = 1 THEN 1
		ELSE 0
END AS IsOverride
,cvDesigns.comments AS OverrideComments
,cvDesigns.created AS DesignCreated
,cvMnemonics.ATLName
,cvMnemonics.PMName
,cvMnemonics.FMName
,cvMnemonics.DMName
,cvMnemonics.CIO
,cvMnemonics.AppOwner
,cvMnemonics.RiskManager
,cvMnemonics.BRContact
,cvMnemonics.Status
,cvMnemonics.ResRating
,vwForecastAnswers.FAImplementationDate as CommitmentDate
,vwForecastAnswers.FAExecutionDate as ScheduledExecutionDate
,vwForecastAnswers.FAExecutedDate as ExecutedDate
,vwForecastAnswers.FAMnemonicCode as MnemonicCode
,vwForecastAnswers.FAProductionDate as ScheduledProductionDate
,CASE WHEN cvClasss.Prod=1 THEN vwForecastAnswers.FAFinishedDate 
	  ELSE vwForecastAnswers.FACompletedDate 
 END As CommissionDate

,vwForecastAnswers.FAAddressId
,vwForecastAnswers.Address
,vwForecastAnswers.CityName
,vwForecastAnswers.Statecode
,0 AS DRAddressId
,'N/A' AS DRAddress
,'N/A' AS DRCityName
,'N/A' AS DRStateCode
,vwForecastAnswers.FAProjectId
,vwForecastAnswers.ProjectName,vwForecastAnswers.ProjectNumber
,vwForecastAnswers.ProjectBaseDiscretionary
,vwForecastAnswers.OrganizationName
,vwForecastAnswers.LeadId,vwForecastAnswers.ProjectLeadName
,vwForecastAnswers.EngineerId,vwForecastAnswers.ProjectEngineerName
,vwForecastAnswers.TechLeadId,vwForecastAnswers.ProjectTechLeadName
,vwForecastAnswers.FAClassId,vwForecastAnswers.Class,vwForecastAnswers.IsPNCClass
,vwForecastAnswers.FAEnvironmentId,vwForecastAnswers.Environment
,vwForecastAnswers.FAApplicationId
,cvApplications.Name as ApplicationName
,vwForecastAnswers.FASubApplicationId
,vwForecastAnswers.FAMnemonicName,vwForecastAnswers.FAMnemonicCode
,vwForecastAnswers.FAExecutedBy, vwForecastAnswers.FAExecutedByUserName
,vwForecastAnswers.FAUserName 
,vwForecastAnswers.FAUserAppContact,vwForecastAnswers.FAUserPrimaryContact
,vwForecastAnswers.FAUserSecondaryContact,vwForecastAnswers.FAUserAppOwner

,ISNULL(cvModels.MpName,'Unknown Server') As ModelName
,cvModels.ModelMake
,cvModels.MpStorageType  AS ModelStorageType
,cvModels.ModelTypeId ,cvModels.ModelTypeName 
,cvModels.MPAssetCategoryId as ModelAssetCategoryId,MPAssetCategory as ModelAssetCategory
,cvModels.MpReplicatedTimes 
,cvModels.MpAMP ,cvModels.MpNetWorkPorts,cvModels.MpStoragePorts
,vwForecastAnswers.FARAM AS MpRAM
,vwForecastAnswers.FACores AS MpCPUCount
,cvModels.MpCPUSpeed
,case when (cvModels.ModelDestroy=0) then 'Reuseable'
	 when (cvModels.ModelDestroy=1) then 'Non-Reusable'
	 else 'Other'
End as ModelDispositionStatus
,cvModels.ModelGrouping

,CASE 
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastAcquisitionNonProdCost + cvModels.ModelForecastAcquisitionProdCost
	ELSE cvModels.ModelForecastAcquisitionNonProdCost
END ModelForecastAcquisitionCost
,CASE  
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastOperationsNonProdCost + cvModels.ModelForecastOperationsProdCost
	ELSE cvModels.ModelForecastOperationsNonProdCost
END  ModelForecastOperationsCost

,CASE  
	WHEN cvClasss.Prod=1 THEN
	cvModels.ModelForecastStreetValNonProdCost + cvModels.ModelForecastStreetValProdCost
	ELSE cvModels.ModelForecastStreetValNonProdCost
END ModelForecastStreetValCost

,cvAssets.Serial as AssetSerialNo
,cvAssetsDR.Serial AS DRAssetSerialNo
,cvAssets.AssetTag as AssetTag
,cvAssetsDR.AssetTag AS DRAssetTag
,cvAssets.AssetStatusId
,AssetStatus.StatusDescription as AssetStatus
,cvAssetOrders.PurchaseOrderNumber
,cvAssetOrders.PurchaseOrderDate
,cvAssetOrders.WarrantyDate
,vwForecastAnswers.FAPlatformId,vwForecastAnswers.Platform 
,cvDomains.name as Domain
,0 as RoomId
,'N/A' as Room
,0 as RackId
,'N/A' as Rack
,'N/A' as Zone
,NULL as Rackposition
,NULL AS Slot
,0 AS DRRoomId
,'N/A' as DRRoom
,0 AS DRRackId
,'N/A' as DRRack
,'N/A' as DRZone
,NULL AS DRRackPosition
,NULL AS DRSlot
,cvOperatingSystems.Name as OperatingSystem
,cvServicePacks.Name as ServicePack
,'N/A' as DummyName 
,'N/A' as DRDummyName 
,'N/A' as MACAddress
,'N/A' as DRMACAddress 
,'N/A' as ILO
,'N/A' as DRILO 
,'N/A' AS OperatingSystemGroup
,NULL as EnclosureName
,NULL as DREnclosureName
,CASE WHEN  Exists(Select * from cvForecastAnswersPlatform 
	WHERE FAPlatformQuestionId =6 and FAPlatformResponseId=15 AND 
		  ForecastAnswerId=cvServers.AnswerId) THEN 1
ELSE 0 
END As SpecialHardware
,CASE WHEN  Exists(Select * from cvForecastAnswersPlatform 
	WHERE FAResponseHACSM =1 AND ForecastAnswerId=cvServers.AnswerId) THEN 1
ELSE 0 
END As LoadBalancing

,CASE WHEN cvServers.ReclaimedStorageAmt IS NOT NULL AND cast(cvServers.ReclaimedStorageAmt  AS float) > 0.00 THEN
		 cast(cvServers.ReclaimedStorageAmt  AS float) 
	 WHEN _cvExactStorageAmts.StorageAmt IS NOT NULL  AND cast(_cvExactStorageAmts.StorageAmt  AS float) > 0.00 THEN
		 cast(_cvExactStorageAmts.StorageAmt  AS float) 
	ELSE  cast(cvServers.ReclaimedStorage  AS float) 
END as SANStorageAmt
,CASE WHEN cvServers.ReclaimedStorageTier IS NULL OR cvServers.ReclaimedStorageTier=0 THEN
		 _cvExactStorageAmts.Tier
	ELSE  cast(cvServers.ReclaimedStorageTier as varchar(50) )
END as SANStorageTier
,CASE WHEN cvServers.ReclaimedStorageEnvironment IS NULL OR cvServers.ReclaimedStorageEnvironment='' THEN
		 _cvExactStorageAmts.Environment 
	ELSE  cvServers.ReclaimedStorageEnvironment  
END as SANStorageEnvironment
,'N/A' AS SwitchA
,'N/A' AS SwitchPortA
,'N/A' AS SwitchB
,'N/A' AS SwitchPortB
,cvProjects.ProjectNumber AS AssetOrderProjectNumber
,'N/A' AS DRSwitchA
,'N/A' AS DRSwitchPortA
,'N/A' AS DRSwitchB
,'N/A' AS DRSwitchPortB

FROM cvServers 
INNER JOIN vwForecastAnswers
	INNER JOIN cvClasss
		ON vwForecastAnswers.FAClassId =cvClasss.ClassId
	ON cvServers.AnswerId = vwForecastAnswers.ForecastAnswerId
	AND vwForecastAnswers.Environment IS NOT NULL AND vwForecastAnswers.Platform IS NOT NULL	
	AND vwForecastAnswers.FAAddressId >0 
	LEFT OUTER JOIN cvDesigns
		ON vwForecastAnswers.ForecastAnswerId =cvDesigns.AnswerId
	LEFT OUTER JOIN ClearView.dbo.cv_mnemonics cvMnemonics
		ON vwForecastAnswers.FAMnemonicId=cvMnemonics.Id 
		AND cvMnemonics.deleted=0
INNER JOIN cvModels
	ON  cvServers.ModelId= cvModels.ModelPropertyId
INNER JOIN cvDomains
	ON cvServers.domainid =cvDomains.DomainId
INNER JOIN cvAssets
	LEFT OUTER JOIN cvAssetOrders
		LEFT OUTER JOIN cvProjects 
			ON	cvAssetOrders.ProjectId = cvProjects.ProjectId
		ON cvAssets.OrderID= cvAssetOrders.OrderID 
	INNER JOIN (Select StatusValue, StatusDescription FROM StatusList where StatusKey='ASSETSTATUS') as AssetStatus
		ON cvAssets.AssetStatusId=AssetStatus.StatusValue
	ON cvServers.AssetID =cvAssets.AssetId  
LEFT OUTER JOIN cvAssets AS cvAssetsDR
	ON cvServers.DRServerAssetId =cvAssetsDR.AssetId  
INNER JOIN cvAssetsGuests
	ON cvServers.AssetID= cvAssetsGuests.AssetId 
INNER JOIN cvOperatingSystems
	ON cvServers.OSId =cvOperatingSystems.OperatingSystemId
LEFT OUTER JOIN  cvServicePacks
	ON cvServers.SPID = cvServicePacks.ServicePackId
LEFT OUTER JOIN cvApplications 
	ON vwForecastAnswers.FAApplicationId=cvApplications.ApplicationId
OUTER APPLY
		(Select TOP 1  * from _cvExactStorageAmts ExtractStorageAmt
		WHERE ExtractStorageAmt.Host =
		cvServers.ServerName) AS _cvExactStorageAmts  

WHERE cvServers.NameId Is Not Null AND cvServers.NameId>0
/* End of VWMARE */


GO


ALTER view [dbo].[vwFactoryFeed]
/*********************************************************************************
Procedure Name		:vwFactoryFeed
Description			:VIEW to get Factory Feed Server Details 
Input Parameters	:	
Output Parameters	: 
Date Created:       : 12-Nov-2009
Author:             : Shyam Pathade
----------------------------------------------------------------------------------
Modification History
Modified By		Date		Description

Select * from vwFactoryFeed
*********************************************************************************/ 
AS
SELECT
'No Information' AS ProjectName,
'No Information' AS ProjectNumber,
'No Information' AS ProjectBaseDiscretion,
'No Information' AS ProjectPoftfolio,
'No Information' AS ProjectManagerName,
'No Information' AS ProjectManagerLANID,
null AS ProjectRequesterName,
'No Information' AS ProjectRequesterLANID,
'No Information' AS ProjectIntegrationEngineerName,
'No Information' AS ProjectIntegrationEngineerLANID,
'No Information' AS ProjectTechnicalLeadName,
'No Information' AS ProjectTechnicalLeadLANDID,
'No Information' AS ProjectIIResourceName,
'No Information' AS ProjectIIResourceLANID,
null AS ServerAcquisitionsCost,
null AS ServerOperationsCost,
null AS AMPs,
null AS Memory,
null AS Processors,
'No Information' AS ApplicationName,
'No Information' AS AppCode,
'No Information' AS ServerDeptManager,
'No Information' AS ServerAppTechLead,
'No Information' AS ServerAdminContact,
'No Information' AS RequestedBy,
null AS SentOn,
'No Information' AS OS,
'No Information' AS HighAvailabilityMethod,
'No Information' AS RTO,
'No Information' AS TypeOfRecovery,
cvAssetStatus.AssetName AS ServerName,
'No Information' AS ServerDummyName,
'No Information' AS ServerDRName,
cvModels.ModelMake + ' ' + cvModels.MpName as ServerModel,
cvModels.ModelMake as Manufacturer,
cvModels.ModelName as Model,
cvModels.ModelTypeName AS ModelType,
'No Information' AS ServerCategory,
'No Information' AS ServerWWPortNames,
'No Information' AS BackupIPAddresss,
'No Information' AS ServerIPAddress,
'No Information' AS ServerGateway,
'No Information' AS ServerSubnetMask,
'No Information' AS ServerVLAN,
'No Information' AS ServerFabric,
cvAssets.Serial AS ServerSerialNumber,
cvAssets.AssetTag AS ServerAssetTag,
'No Information' AS ServerRemoteManagementIP,
null AS BuildDate,
'No Information' AS ServerRemoteManagementName,
'No Information' AS ServerDNSName,
'No Information' AS ServerDNSAlias,
'No Information' AS PortName,
'No Information' AS LoadBalancing,
'No Information' AS ServerClass,
'No Information' AS ServerEnvironment,
'No Information' AS ServerOperatingSystem,
'No Information' AS ServerRoom,
'No Information' AS ServerZone,
'No Information' AS ZRC,
null AS RMU,
'No Information' AS SpeedDuplex,
'No Information' AS ServerRack,
'No Information' AS ServerLocation,
'No Information' AS ServerLocationCity,
'No Information' AS ServerLocationState,
GETDATE() AS ServerCommissionDate,
GETDATE() AS ServerDecommissionedDate,
'No Information' AS PurchaseOrderNumber,
null AS PurchaseOrderDate,
null AS WarrantyDate,
'No Information' AS SwitchA,
'No Information' AS SwitchPortA,
'No Information' AS SwitchB,
'No Information' AS SwitchPortB,
'No Information' AS MACAddress,
'No Information' AS ServerDesignNickName,
'No Information' AS OutageWindow,
'No Information' AS CostCenter,
'No Information' AS BuildingCode,
'No Information' AS ServerClusterName,
'No Information' AS ServerRecoveryLocation,
'No Information' AS ServerBackupFrequency,
'No Information' AS ServerBackupStartTime,
'No Information' AS ServerBackupStartDate,
'No Information' AS ServerCurrentCombinedDiskUtilized,
null AS ServerAvgSizeOfOneTypicalDataFile,
'No Information' AS ServerProductTurnOverDocumentation,
null AS ServerTotalCombinedDiskCapacity,
'No Information' AS Domain,
null AS ServerForeCastAnswerId,
null AS ServerID,
'No Information' AS BackupWaiverRequestNumber,
'No Information' AS BackupWaiverRequestTitle,
null AS BackupWaiverRequestDateStart,
null AS BackupWaiverRequestStatus,
null AS BackupWaiverRequestDateComplete,
'No Information' AS BackupWaiverRequestedWork,
'No Information' AS BackupWaiverRequestType,
'No Information' AS BackupWaiverRequestForm,
null AS BackupWaiverRequestDesignID,
'No Information' AS BackupWaiverRequestRequestor

--into #temp
FROM
	cvAssetStatus
		INNER JOIN
			cvAssets
				INNER JOIN
					cvModels
				ON
					cvAssets.ModelId = cvModels.ModelPropertyId
					AND cvModels.ModelDeleted = 0
		ON
			cvAssetStatus.AssetId = cvAssets.AssetId
			AND cvAssets.Deleted = 0
WHERE
cvAssetStatus.AssetName IN (
	'OHBREIIS1000',	-- Kurt Sedei (9/15/14 9:06 AM)
	'OHBREAPP1000',	-- Kurt Sedei (9/7/14 12:00 AM)
	'OHCLEUTL100D',
	'OHCLEIIS103H',
	'OHCLEIIS104H',
	'OHCLEIIS102M',	-- Kurt Sedei (1/12/15)
	'OHCLECTX100K',	-- Kurt (6/11/2014	CHG0405236)
	'OHCLECTX100G',	-- Kurt (6/11/2014	CHG0405236)
	'OHCLEAPP104M',	-- Kurt (7/23/2014	CHG0415478)
	'OHCLEAPP104N'	-- Kurt (6/11/2014	CHG0405142)
)  
AND cvAssetStatus.AssetStatusDeleted = 0
--AND cvAssetStatus.AssetName NOT IN (SELECT ServerName FROM cvFactoryFeed)

UNION 
SELECT
'No Information' AS ProjectName,
'No Information' AS ProjectNumber,
'No Information' AS ProjectBaseDiscretion,
'No Information' AS ProjectPoftfolio,
'No Information' AS ProjectManagerName,
'No Information' AS ProjectManagerLANID,
null AS ProjectRequesterName,
'No Information' AS ProjectRequesterLANID,
'No Information' AS ProjectIntegrationEngineerName,
'No Information' AS ProjectIntegrationEngineerLANID,
'No Information' AS ProjectTechnicalLeadName,
'No Information' AS ProjectTechnicalLeadLANDID,
'No Information' AS ProjectIIResourceName,
'No Information' AS ProjectIIResourceLANID,
null AS ServerAcquisitionsCost,
null AS ServerOperationsCost,
null AS AMPs,
null AS Memory,
null AS Processors,
'No Information' AS ApplicationName,
'No Information' AS AppCode,
'No Information' AS ServerDeptManager,
'No Information' AS ServerAppTechLead,
'No Information' AS ServerAdminContact,
'No Information' AS RequestedBy,
null AS SentOn,
'No Information' AS OS,
'No Information' AS HighAvailabilityMethod,
'No Information' AS RTO,
'No Information' AS TypeOfRecovery,
cvAssetStatus.AssetName AS ServerName,
'No Information' AS ServerDummyName,
'No Information' AS ServerDRName,
cvModels.ModelMake + ' ' + cvModels.MpName as ServerModel,
cvModels.ModelMake as Manufacturer,
cvModels.ModelName as Model,
cvModels.ModelTypeName AS ModelType,
'No Information' AS ServerCategory,
'No Information' AS ServerWWPortNames,
'No Information' AS BackupIPAddresss,
'No Information' AS ServerIPAddress,
'No Information' AS ServerGateway,
'No Information' AS ServerSubnetMask,
'No Information' AS ServerVLAN,
'No Information' AS ServerFabric,
cvAssets.Serial AS ServerSerialNumber,
cvAssets.AssetTag AS ServerAssetTag,
'No Information' AS ServerRemoteManagementIP,
null AS BuildDate,
'No Information' AS ServerRemoteManagementName,
'No Information' AS ServerDNSName,
'No Information' AS ServerDNSAlias,
'No Information' AS PortName,
'No Information' AS LoadBalancing,
'No Information' AS ServerClass,
'No Information' AS ServerEnvironment,
'No Information' AS ServerOperatingSystem,
'No Information' AS ServerRoom,
'No Information' AS ServerZone,
'No Information' AS ZRC,
null AS RMU,
'No Information' AS SpeedDuplex,
'No Information' AS ServerRack,
'No Information' AS ServerLocation,
'No Information' AS ServerLocationCity,
'No Information' AS ServerLocationState,
null AS ServerCommissionDate,
null AS ServerDecommissionedDate,
'No Information' AS PurchaseOrderNumber,
null AS PurchaseOrderDate,
null AS WarrantyDate,
'No Information' AS SwitchA,
'No Information' AS SwitchPortA,
'No Information' AS SwitchB,
'No Information' AS SwitchPortB,
'No Information' AS MACAddress,
'No Information' AS ServerDesignNickName,
'No Information' AS OutageWindow,
'No Information' AS CostCenter,
'No Information' AS BuildingCode,
'No Information' AS ServerClusterName,
'No Information' AS ServerRecoveryLocation,
'No Information' AS ServerBackupFrequency,
'No Information' AS ServerBackupStartTime,
'No Information' AS ServerBackupStartDate,
'No Information' AS ServerCurrentCombinedDiskUtilized,
null AS ServerAvgSizeOfOneTypicalDataFile,
'No Information' AS ServerProductTurnOverDocumentation,
null AS ServerTotalCombinedDiskCapacity,
'No Information' AS Domain,
null AS ServerForeCastAnswerId,
null AS ServerID,
'No Information' AS BackupWaiverRequestNumber,
'No Information' AS BackupWaiverRequestTitle,
null AS BackupWaiverRequestDateStart,
null AS BackupWaiverRequestStatus,
null AS BackupWaiverRequestDateComplete,
'No Information' AS BackupWaiverRequestedWork,
'No Information' AS BackupWaiverRequestType,
'No Information' AS BackupWaiverRequestForm,
null AS BackupWaiverRequestDesignID,
'No Information' AS BackupWaiverRequestRequestor

--into #temp
FROM
	cvAssetStatus
		INNER JOIN
			cvAssets
				INNER JOIN
					cvModels
				ON
					cvAssets.ModelId = cvModels.ModelPropertyId
					AND cvModels.ModelDeleted = 0
		ON
			cvAssetStatus.AssetId = cvAssets.AssetId
			AND cvAssets.Deleted = 0
WHERE
cvAssetStatus.AssetName IN (
	'WDAPP103W',
	'OHCLENSQ100A',
	'OHCLENSQ100B',
	'OHCLEUTL100B',
	'OHCLEUTL100C',
	'OHCLEAPP104P'
)  
AND cvAssetStatus.AssetStatusDeleted = 0
--AND cvAssetStatus.AssetName NOT IN (SELECT ServerName FROM cvFactoryFeed)

UNION 
--select cvFactoryFeed.* from cvFactoryFeed
SELECT cvFF.* FROM (

Select distinct
vwCVProjects.ProjectName,
vwCVProjects.ProjectNumber,
vwCVProjects.BaseDiscretionary as ProjectBaseDiscretion,
vwCVProjects.OrganizationName as ProjectPoftfolio,
vwCVProjects.LeadName as ProjectManagerName,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.TechLeadId) as ProjectManagerLANID,
vwCVProjects.UserName as ProjectRequesterName ,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.UserId) as ProjectRequesterLANID,
vwCVProjects.EngineerName as ProjectIntegrationEngineerName,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.EngineerId) as ProjectIntegrationEngineerLANID,
vwCVProjects.TechLeadName as ProjectTechnicalLeadName  ,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.TechLeadId) as  ProjectTechnicalLeadLANDID ,
UserIISResource.UserName as  ProjectIIResourceName , 
UserIISResource.XID as  ProjectIIResourceLANID, 

/********** Design config. ***************/

vwServers.ModelForecastAcquisitionCost as ServerAcquisitionsCost,
vwServers.ModelForecastOperationsCost as ServerOperationsCost,
vwServers.MpAMP as AMPs,
vwServers.MpRAM as Memory,
vwServers.MpCPUCount as Processors,
--vwServers.ApplicationName as ServerApplicationName,
vwServers.FAMnemonicName as ApplicationName,
vwServers.FAMnemonicCode as  AppCode,
--vwServers.FAUserAppContact as ServerClientContact,
--'' as ServerClientContactLANID,
--vwServers.FAUserPrimaryContact as ServerPrimaryContact,
--'' as ServerPrimaryContactLANID,
--vwServers.FAUserSecondaryContact as ServerAdminContact,
--'' as ServerAdminContactLANID,
--vwServers.FAUserAppOwner as ServerAppOwner,
--'' as ServerAppOwnerLANID,
vwServers.FAUserAppContact as ServerDeptManager,
vwServers.FAUserPrimaryContact as ServerAppTechLead,
ISNULL(vwServers.FAUserAppOwner,vwServers.FAUserSecondaryContact) AS ServerAdminContact,
vwServers.FAExecutedByUserName AS RequestedBy,
vwServers.CommissionDate AS SentOn,


/*Question Operating System */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=8 ) AS varchar(1200)) AS OS,

/*High availability method */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=2 ) AS varchar(1200)) AS HighAvailabilityMethod,

/*Disaster recovery requirements */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=4 ) AS varchar(1200)) AS RTO,

/*Type Of Recovery */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=5 ) AS varchar(1200)) AS TypeOfRecovery,

/********** End of Design config. ***************/
CASE
	-- 8/6 : Adam Hess, Stephen Wargo, Ryan Bennett forced a rename of their infrastructure servers
	WHEN vwServers.ServerName = 'LDMRG110A' THEN 'LDMRG310A'
	WHEN vwServers.ServerName = 'LDMRG111A' THEN 'LDMRG311A'
	WHEN vwServers.ServerName = 'LDMRG112A' THEN 'LDMRG312A'
	WHEN vwServers.ServerName = 'LDMRG114A' THEN 'LDMRG214A'
	WHEN vwServers.ServerName = 'LDMRG115A' THEN 'LDMRG215A'
	WHEN vwServers.ServerName = 'LDMRG116A' THEN 'LDMRG216A'
	WHEN vwServers.ServerName = 'LSMRG213A' THEN 'LSMRG113A'
	WHEN vwServers.ServerName = 'LSMRG214A' THEN 'LSMRG114A'
	WHEN vwServers.ServerName = 'LCMRG219A' THEN 'LCMRG119A'
	WHEN vwServers.ServerName = 'LSMRG215A' THEN 'LSMRG115A'
	WHEN vwServers.ServerName = 'LSMRG216A' THEN 'LSMRG116A'
	WHEN vwServers.ServerName = 'LCMRG220A' THEN 'LCMRG120A'
	WHEN vwServers.ServerName = 'LSMRG217A' THEN 'LSMRG117A'
	WHEN vwServers.ServerName = 'LSMRG218A' THEN 'LSMRG118A'
	ELSE vwServers.ServerName
END AS ServerName,
vwServers.DummyName As ServerDummyName,
vwServers.DRName As ServerDRName,
vwServers.ModelMake + ' ' + vwServers.ModelName as ServerModel,
vwServers.ModelMake as Manufacturer,
(select TOP 1 cvModels.ModelName from cvModels where ModelPropertyId=vwServers.ModelId) as Model,
vwServers.ModelTypeName as ModelType,
vwServers.ModelAssetCategory as ServerCategory,
CAST(vwServers.ServerWWPortNames AS varchar(1200)) AS ServerWWPortNames,
CAST(vwServers.BackupIPAddresss AS varchar(1200)) AS BackupIPAddresss,
CAST(vwServers.ServerIPAddresss AS varchar(1200)) AS ServerIPAddress,
--vwServers.ServerIPAddresss as  ServerIPAddress,
CAST(vwServers.ServerGateway AS varchar(1200)) AS ServerGateway,
CAST(vwServers.ServerSubnetMask AS varchar(1200)) AS ServerSubnetMask,
CAST(vwServers.ServervLAN AS varchar(1200)) AS ServerVLAN,
vwServers.ModelStorageType as ServerFabric,
vwServers.AssetSerialNo AS ServerSerialNumber,
vwServers.AssetTag AS ServerAssetTag,
vwServers.ILO AS ServerRemoteManagementIP,
vwServers.CommissionDate AS BuildDate,
vwServers.ServerName + '-RM' AS ServerRemoteManagementName,
vwServers.ServerName + '.' + vwServers.Domain AS ServerDNSName,
vwServers.ServerName AS ServerDNSAlias,
CAST(
	CASE
		WHEN CHARINDEX('.', ServerIPAddresss) > 0 AND CHARINDEX('.', ServerIPAddresss, CHARINDEX('.', ServerIPAddresss)+1) > 0 THEN SUBSTRING(ServerIPAddresss,CHARINDEX('.', ServerIPAddresss, CHARINDEX('.', ServerIPAddresss)+1),100) + '_' + vwServers.ServerName + '-pri'
		ELSE ''
	END AS varchar(1200)) AS PortName,
CASE WHEN vwServers.LoadBalancing = 1 THEN 'Yes' ELSE 'No' END AS LoadBalancing,

--vwServers.ServerComponents as ServerFunctions,
vwServers.Class AS ServerClass,
vwServers.Environment AS ServerEnvironment,
vwServers.OperatingSystem AS ServerOperatingSystem,
vwServers.Room AS ServerRoom,
CASE
	WHEN charindex('unknown',vwServers.Zone) > 0 THEN '0'
	--WHEN charindex('default zone #',vwServers.Zone) > 0 THEN substring(vwServers.Zone,len('default zone #')+2,100)
	WHEN charindex('default zone #',vwServers.Zone) > 0 THEN '0'
	ELSE vwServers.Zone
END AS ServerZone,

CASE
	WHEN charindex('unknown',vwServers.Zone) > 0 THEN '0'
	WHEN charindex('default zone #',vwServers.Zone) > 0 THEN '0'
	WHEN charindex('N/A',vwServers.Zone) > 0 THEN '0'
	ELSE vwServers.Zone
END 
+ '.' + '1' + '.' +
CASE
	WHEN charindex('unknown',vwServers.Rack) > 0 THEN '0'
	WHEN charindex('N/A',vwServers.Zone) > 0 THEN '0'
	ELSE vwServers.Rack
END 
AS ZRC,
0 AS RMU,
'Auto/Auto' AS SpeedDuplex,
vwServers.Rack AS ServerRack,
vwServers.Address AS ServerLocation,
vwServers.CityName AS ServerLocationCity,
vwServers.Statecode AS ServerLocationState,
vwServers.CommissionDate AS ServerCommissionDate,
vwServers.DecommissionedDate AS ServerDecommissionedDate,
vwServers.PurchaseOrderNumber,
vwServers.PurchaseOrderDate,
vwServers.WarrantyDate,
vwServers.SwitchA,
vwServers.SwitchPortA,
vwServers.SwitchB,
vwServers.SwitchPortB,
vwServers.MACAddress,

/********** birth cert. ***************/
vwForecastAnswers.FAName  As ServerDesignNickName,
vwForecastAnswers.FAMaintenanceWindow AS OutageWindow,
vwForecastAnswers.FACostCenter AS CostCenter,
vwForecastAnswers.BuildingCode,

--CASE 
--WHEN RIGHT(vwServers.ServerName, 1)='Z' then
--	substring(vwServers.ServerName,1,len(vwServers.ServerName)-2)+
--	Replicate('0',2-len(cast(substring(right(vwServers.ServerName,4),1,2) as int)+1))+
--	convert(varchar(2),cast(substring(right(vwServers.ServerName,4),1,2) as int)+1)+'A'
--ELSE ''
--END as ServerClusterName,
CASE
	WHEN RIGHT(vwServers.ServerName, 1)='Z' AND vwServers.ServerClusterName IS NOT NULL AND vwServers.ServerClusterName <> '' THEN UPPER(vwServers.ServerClusterName)
	WHEN RIGHT(vwServers.ServerName, 1)='Z' THEN
		substring(vwServers.ServerName,1,len(vwServers.ServerName)-2) +
		CASE
			WHEN substring(right(vwServers.ServerName,3),1,1) = '9' THEN CHAR(ASCII(substring(right(vwServers.ServerName,4),1,1))+1)
			WHEN substring(right(vwServers.ServerName,3),1,1) = 'Z' THEN CHAR(ASCII(substring(right(vwServers.ServerName,4),1,1))+1)
			ELSE substring(right(vwServers.ServerName,4),1,1)
		END +
		CASE
			WHEN substring(right(vwServers.ServerName,3),1,1) = '9' THEN '0'	-- LCETH309AZ -> 0[9]AZ = 9 THEN 091[0]AZ
			WHEN substring(right(vwServers.ServerName,3),1,1) = 'Z' THEN 'A'	-- LCETH309AZ -> 0[Z]AZ = Z THEN 091[A]AZ
			ELSE CHAR(ASCII(substring(right(vwServers.ServerName,3),1,1))+1)	-- LCETH309AZ -> 0[9 + 1]
		END +
		CASE WHEN substring(right(vwServers.ServerName,2),1,1) = 'D' THEN 'D' ELSE 'A' END
	ELSE ''
END AS ServerClusterName,
/*'' as ServerConsistencyGroup,*/
/********** end of birth cert. ***************/
/********** Backup config. ***************/
vwForecastAnswers.FABackupRecoveryLocation as ServerRecoveryLocation,
CASE 
	WHEN vwForecastAnswers.FABackupIsDaily=1 then 'Daily'
	WHEN vwForecastAnswers.FABackupIsWeekly=1 then 'Weekly'
	WHEN vwForecastAnswers.FABackupIsMonthly=1 then 'Monthly'
END ServerBackupFrequency,
(isnull(vwForecastAnswers.FABackupTimeHour,'') + isnull(vwForecastAnswers.FABackupTimeSwitch,'') )as ServerBackupStartTime,
vwForecastAnswers.FABackupStartDate as ServerBackupStartDate,

'5 GB' as ServerCurrentCombinedDiskUtilized,
vwForecastAnswers.FABackupAverageOne  as ServerAvgSizeOfOneTypicalDataFile,
vwForecastAnswers.FABackupDocumentation  as ServerProductTurnOverDocumentation,

vwServers.ServerStorageProd as ServerTotalCombinedDiskCapacity,
/********** End of Backup config. ***************/

/* Other Info */
vwServers.Domain,
--vwCVProjects.ProjectId,
--vwServers.DomainId,
--vwServers.RackId,
--vwServers.RoomId,
--vwServers.OSId,
vwServers.FAAddressId as ServerForeCastAnswerId,
vwServers.ServerID


-- WAIVER
,'CVT' + CAST([set_GEN_1038].requestid AS varchar(20)) AS BackupWaiverRequestNumber
,cvServices.Name AS BackupWaiverRequestTitle
,cvRequests.RRCreated AS BackupWaiverRequestDateStart
,CASE WHEN cvRequests.RRCompleted IS NOT NULL THEN 'Completed' ELSE NULL END AS BackupWaiverRequestStatus
,cvRequests.RRCompleted AS BackupWaiverRequestDateComplete
,'New Server Backup Request' AS BackupWaiverRequestedWork
,vwServers.Class AS BackupWaiverRequestType
,'ClearView' AS BackupWaiverRequestForm
,cvDesigns.id AS BackupWaiverRequestDesignID
,vwForecastAnswers.FAExecutedByUserName AS BackupWaiverRequestRequestor


/*End of other info */
 FROM vwServers
 LEFT OUTER JOIN
	ClearViewServiceEditor.dbo.[set_GEN_1038]
		 INNER JOIN
			cvServices
		ON
			[set_GEN_1038].serviceid = cvServices.ServiceId
		 INNER JOIN
			cvRequests
		ON
			[set_GEN_1038].requestid = cvRequests.RequestId
			AND [set_GEN_1038].serviceid = cvRequests.RRServiceId
			AND [set_GEN_1038].number = cvRequests.RRNumber
ON
	CAST([set_GEN_1038].[1002829] AS VARCHAR(MAX)) = vwServers.ServerName
LEFT OUTER JOIN
	cvDesigns
ON
	cvDesigns.answerid = vwServers.AnswerId
INNER JOIN vwForecastAnswers 
		ON vwServers.AnswerId=vwForecastAnswers.ForecastAnswerId
LEFT OUTER JOIN vwCVProjects
	ON vwCVProjects.ProjectId =vwServers.FAProjectId
	/* For IIS ResourceName */
	LEFT OUTER JOIN cvOnDemandTasksPending  
		ON  vwForecastAnswers.ForecastAnswerId= cvOnDemandTasksPending.answerid
		OUTER APPLY (SELECT DISTINCT RequestId,RRWFId,RRWFUserId FROM cvRequests 
				WHERE cvOnDemandTasksPending.ResourceId =cvRequests.RRWFId) AS cvRRWF 
		LEFT OUTER JOIN cvUsers as UserIISResource
				ON cvRRWF.RRWFUserId = UserIISResource.UserId
		INNER JOIN cvDomains
			ON vwServers.DomainId=cvDomains.DomainId and cvDomains.Enabled=1
--WHERE vwServers.FAClassId in (6,7,8)
WHERE  vwServers.FAClassId in (1,2,3,4,5,6,7,8,10,11,12)
-- Added on 9/12/2011 (Healy)
--AND vwServers.FAAddressId not in (1674,1675)

/** Fix to Compare **/

--AND vwServers.Domain is not null 
--AND vwServers.Rack  is not null
--AND vwServers.Room is not null
AND ServerAssetLatest=1

/** End of  Fix to Compare **/

--AND vwServers.OperatingSystem  is not null
--AND vwServers.FAAddressId is not null

-- ********************************************************************
-- 10/15/2010 (Healy) : Per Kelly Doyle, Ron Pepin and Mary Majoros - Removed two server records...
-- "The anomaly of these two servers being built here and shipped to PGH is becoming a critical issue as we are trying to true-up SIW server data.  Please let me know if you and when this can be completed."
AND vwServers.ServerName NOT IN ('WSRDP302A','LSRDP300A')
AND vwServers.serverid not in 
(select serverid from dbo._tmpcvFactoryFeedReleaseServers
where Relased is null)
--AND vwServers.DecommissionedDate IS NULL
-- Added on 9/21/12 (PTM: 297378)
AND vwServers.CommissionDate IS NOT NULL
--and (RIGHT(vwServers.ServerName, 1)<>'Z' OR isnumeric(substring(right(vwServers.ServerName,4),1,2)) = 1)
-- ********************************************************************

-- 3/12/13 (Healy): Per Jeff DiFiore, Bill Ross and Jim White, removed TIV servers since they renamed them
-- to EBR.  TIV is 96 hour recovery, EBR is 6 hour recovery.  Since SIW was renamed, these were reappearing.
AND vwServers.ServerName NOT IN ('LSTIV300A','LSTIV301A','LSTIV302A')

-- 5/13/13 (Healy): Per Jeff DiFiore, removed the following servers due to renames...
AND vwServers.ServerName NOT IN ('WSISD300C')	-- renamed to WWDS317
AND vwServers.ServerName NOT IN ('WSISD301C')	-- renamed to WWDS318
) AS cvFF

UNION 

SELECT cvDR.* FROM (

Select distinct
vwCVProjects.ProjectName,
vwCVProjects.ProjectNumber,
vwCVProjects.BaseDiscretionary as ProjectBaseDiscretion,
vwCVProjects.OrganizationName as ProjectPoftfolio,
vwCVProjects.LeadName as ProjectManagerName,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.TechLeadId) as ProjectManagerLANID,
vwCVProjects.UserName as ProjectRequesterName ,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.UserId) as ProjectRequesterLANID,
vwCVProjects.EngineerName as ProjectIntegrationEngineerName,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.EngineerId) as ProjectIntegrationEngineerLANID,
vwCVProjects.TechLeadName as ProjectTechnicalLeadName  ,
(Select TOP 1 XID from dbo.cvUsers WHERE UserId= vwCVProjects.TechLeadId) as  ProjectTechnicalLeadLANDID ,
UserIISResource.UserName as  ProjectIIResourceName , 
UserIISResource.XID as  ProjectIIResourceLANID, 

/********** Design config. ***************/

vwServers.ModelForecastAcquisitionCost as ServerAcquisitionsCost,
vwServers.ModelForecastOperationsCost as ServerOperationsCost,
vwServers.MpAMP as AMPs,
vwServers.MpRAM as Memory,
vwServers.MpCPUCount as Processors,
--vwServers.ApplicationName as ServerApplicationName,
vwServers.FAMnemonicName as ApplicationName,
vwServers.FAMnemonicCode as  AppCode,
--vwServers.FAUserAppContact as ServerClientContact,
--'' as ServerClientContactLANID,
--vwServers.FAUserPrimaryContact as ServerPrimaryContact,
--'' as ServerPrimaryContactLANID,
--vwServers.FAUserSecondaryContact as ServerAdminContact,
--'' as ServerAdminContactLANID,
--vwServers.FAUserAppOwner as ServerAppOwner,
--'' as ServerAppOwnerLANID,
vwServers.FAUserAppContact as ServerDeptManager,
vwServers.FAUserPrimaryContact as ServerAppTechLead,
ISNULL(vwServers.FAUserAppOwner,vwServers.FAUserSecondaryContact) AS ServerAdminContact,
vwServers.FAExecutedByUserName AS RequestedBy,
vwServers.CommissionDate AS SentOn,


/*Question Operating System */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=8 ) AS varchar(1200)) AS OS,

/*High availability method */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=2 ) AS varchar(1200)) AS HighAvailabilityMethod,

/*Disaster recovery requirements */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=4 ) AS varchar(1200)) AS RTO,

/*Type Of Recovery */
CAST((Select TOP 1 cast(FAResponse as varchar(max)) from cvForecastAnswersPlatform 
WHERE cvForecastAnswersPlatform.ForecastAnswerId=vwServers.AnswerId 
AND cvForecastAnswersPlatform.FAResponseCategoryId=5 ) AS varchar(1200)) AS TypeOfRecovery,

/********** End of Design config. ***************/
CASE
	-- 8/6 : Adam Hess, Stephen Wargo, Ryan Bennett forced a rename of their infrastructure servers
	WHEN vwServers.ServerName = 'LDMRG110A' THEN 'LDMRG310A-DR'
	WHEN vwServers.ServerName = 'LDMRG111A' THEN 'LDMRG311A-DR'
	WHEN vwServers.ServerName = 'LDMRG112A' THEN 'LDMRG312A-DR'
	WHEN vwServers.ServerName = 'LDMRG114A' THEN 'LDMRG214A-DR'
	WHEN vwServers.ServerName = 'LDMRG115A' THEN 'LDMRG215A-DR'
	WHEN vwServers.ServerName = 'LDMRG116A' THEN 'LDMRG216A-DR'
	WHEN vwServers.ServerName = 'LSMRG213A' THEN 'LSMRG113A-DR'
	WHEN vwServers.ServerName = 'LSMRG214A' THEN 'LSMRG114A-DR'
	WHEN vwServers.ServerName = 'LCMRG219A' THEN 'LCMRG119A-DR'
	WHEN vwServers.ServerName = 'LSMRG215A' THEN 'LSMRG115A-DR'
	WHEN vwServers.ServerName = 'LSMRG216A' THEN 'LSMRG116A-DR'
	WHEN vwServers.ServerName = 'LCMRG220A' THEN 'LCMRG120A-DR'
	WHEN vwServers.ServerName = 'LSMRG217A' THEN 'LSMRG117A-DR'
	WHEN vwServers.ServerName = 'LSMRG218A' THEN 'LSMRG118A-DR'
	ELSE vwServers.ServerName + '-DR'
END AS ServerName,
vwServers.DRDummyName As ServerDummyName,
'N/A' As ServerDRName,
vwServers.ModelMake + ' ' + vwServers.ModelName as ServerModel,
vwServers.ModelMake as Manufacturer,
(select TOP 1 cvModels.ModelName from cvModels where ModelPropertyId=vwServers.ModelId) as Model,
vwServers.ModelTypeName as ModelType,
vwServers.ModelAssetCategory as ServerCategory,
CAST(vwServers.DRServerWWPortNames AS varchar(1200)) AS ServerWWPortNames,
'N/A' AS BackupIPAddresss,
'N/A' AS ServerIPAddress,
CAST(vwServers.ServerGateway AS varchar(1200)) AS ServerGateway,
CAST(vwServers.ServerSubnetMask AS varchar(1200)) AS ServerSubnetMask,
CAST(vwServers.ServervLAN AS varchar(1200)) AS ServerVLAN,
vwServers.ModelStorageType as ServerFabric,
vwServers.DRAssetSerialNo AS ServerSerialNumber,
vwServers.DRAssetTag AS ServerAssetTag,
vwServers.DRILO AS ServerRemoteManagementIP,
vwServers.CommissionDate AS BuildDate,
'N/A' AS ServerRemoteManagementName,
'N/A' AS ServerDNSName,
'N/A' AS ServerDNSAlias,
'N/A' AS PortName,
CASE WHEN vwServers.LoadBalancing = 1 THEN 'Yes' ELSE 'No' END AS LoadBalancing,

'DR' AS ServerClass,
vwServers.Environment AS ServerEnvironment,
'N/A' AS ServerOperatingSystem,
vwServers.DRRoom AS ServerRoom,
CASE
	WHEN charindex('unknown',vwServers.DRZone) > 0 THEN '0'
	--WHEN charindex('default zone #',vwServers.Zone) > 0 THEN substring(vwServers.Zone,len('default zone #')+2,100)
	WHEN charindex('default zone #',vwServers.DRZone) > 0 THEN '0'
	ELSE vwServers.DRZone
END AS ServerZone,

CASE
	WHEN charindex('unknown',vwServers.DRZone) > 0 THEN '0'
	WHEN charindex('default zone #',vwServers.DRZone) > 0 THEN '0'
	WHEN charindex('N/A',vwServers.DRZone) > 0 THEN '0'
	ELSE vwServers.DRZone
END 
+ '.' + '1' + '.' +
CASE
	WHEN charindex('unknown',vwServers.Rack) > 0 THEN '0'
	WHEN charindex('N/A',vwServers.Zone) > 0 THEN '0'
	ELSE vwServers.Rack
END 
AS ZRC,
0 AS RMU,
'Auto/Auto' AS SpeedDuplex,
vwServers.DRRack AS ServerRack,
vwServers.DRAddress AS ServerLocation,
vwServers.DRCityName AS ServerLocationCity,
vwServers.DRStatecode AS ServerLocationState,
vwServers.CommissionDate AS ServerCommissionDate,
vwServers.DecommissionedDate AS ServerDecommissionedDate,
vwServers.PurchaseOrderNumber,
vwServers.PurchaseOrderDate,
vwServers.WarrantyDate,
vwServers.DRSwitchA AS SwitchA,
vwServers.DRSwitchPortA AS SwitchPortA,
vwServers.DRSwitchB AS SwitchB,
vwServers.DRSwitchPortB AS SwitchPortB,
vwServers.DRMACAddress AS MACAddress,

/********** birth cert. ***************/
vwForecastAnswers.FAName  As ServerDesignNickName,
vwForecastAnswers.FAMaintenanceWindow AS OutageWindow,
vwForecastAnswers.FACostCenter AS CostCenter,
vwForecastAnswers.BuildingCode,
'' AS ServerClusterName,
/********** end of birth cert. ***************/
/********** Backup config. ***************/
vwForecastAnswers.FABackupRecoveryLocation as ServerRecoveryLocation,
CASE 
	WHEN vwForecastAnswers.FABackupIsDaily=1 then 'Daily'
	WHEN vwForecastAnswers.FABackupIsWeekly=1 then 'Weekly'
	WHEN vwForecastAnswers.FABackupIsMonthly=1 then 'Monthly'
END ServerBackupFrequency,
(isnull(vwForecastAnswers.FABackupTimeHour,'') + isnull(vwForecastAnswers.FABackupTimeSwitch,'') )as ServerBackupStartTime,
vwForecastAnswers.FABackupStartDate as ServerBackupStartDate,

'5 GB' as ServerCurrentCombinedDiskUtilized,
vwForecastAnswers.FABackupAverageOne  as ServerAvgSizeOfOneTypicalDataFile,
vwForecastAnswers.FABackupDocumentation  as ServerProductTurnOverDocumentation,

vwServers.ServerStorageProd as ServerTotalCombinedDiskCapacity,
/********** End of Backup config. ***************/

/* Other Info */
vwServers.Domain,
vwServers.FAAddressId as ServerForeCastAnswerId,
vwServers.ServerID


-- WAIVER
,'CVT' + CAST([set_GEN_1038].requestid AS varchar(20)) AS BackupWaiverRequestNumber
,cvServices.Name AS BackupWaiverRequestTitle
,cvRequests.RRCreated AS BackupWaiverRequestDateStart
,CASE WHEN cvRequests.RRCompleted IS NOT NULL THEN 'Completed' ELSE NULL END AS BackupWaiverRequestStatus
,cvRequests.RRCompleted AS BackupWaiverRequestDateComplete
,'New Server Backup Request' AS BackupWaiverRequestedWork
,vwServers.Class AS BackupWaiverRequestType
,'ClearView' AS BackupWaiverRequestForm
,cvDesigns.id AS BackupWaiverRequestDesignID
,vwForecastAnswers.FAExecutedByUserName AS BackupWaiverRequestRequestor


/*End of other info */
 FROM vwServers
 LEFT OUTER JOIN
	ClearViewServiceEditor.dbo.[set_GEN_1038]
		 INNER JOIN
			cvServices
		ON
			[set_GEN_1038].serviceid = cvServices.ServiceId
		 INNER JOIN
			cvRequests
		ON
			[set_GEN_1038].requestid = cvRequests.RequestId
			AND [set_GEN_1038].serviceid = cvRequests.RRServiceId
			AND [set_GEN_1038].number = cvRequests.RRNumber
ON
	CAST([set_GEN_1038].[1002829] AS VARCHAR(MAX)) = vwServers.ServerName
LEFT OUTER JOIN
	cvDesigns
ON
	cvDesigns.answerid = vwServers.AnswerId
INNER JOIN vwForecastAnswers 
		ON vwServers.AnswerId=vwForecastAnswers.ForecastAnswerId
LEFT OUTER JOIN vwCVProjects
	ON vwCVProjects.ProjectId =vwServers.FAProjectId
	/* For IIS ResourceName */
	LEFT OUTER JOIN cvOnDemandTasksPending  
		ON  vwForecastAnswers.ForecastAnswerId= cvOnDemandTasksPending.answerid
		OUTER APPLY (SELECT DISTINCT RequestId,RRWFId,RRWFUserId FROM cvRequests 
				WHERE cvOnDemandTasksPending.ResourceId =cvRequests.RRWFId) AS cvRRWF 
		LEFT OUTER JOIN cvUsers as UserIISResource
				ON cvRRWF.RRWFUserId = UserIISResource.UserId
		INNER JOIN cvDomains
			ON vwServers.DomainId=cvDomains.DomainId and cvDomains.Enabled=1
--WHERE vwServers.FAClassId in (6,7,8)
WHERE  vwServers.FAClassId in (1,2,3,4,5,6,7,8,10,11,12)
-- Added on 9/12/2011 (Healy)
--AND vwServers.FAAddressId not in (1674,1675)

/** Fix to Compare **/

--AND vwServers.Domain is not null 
--AND vwServers.Rack  is not null
--AND vwServers.Room is not null
AND vwServers.DRServerAssetId > 0
AND vwServers.ModelAssetCategory <> 'Virtual'

/** End of  Fix to Compare **/

--AND vwServers.OperatingSystem  is not null
--AND vwServers.FAAddressId is not null

-- ********************************************************************
-- 10/15/2010 (Healy) : Per Kelly Doyle, Ron Pepin and Mary Majoros - Removed two server records...
-- "The anomaly of these two servers being built here and shipped to PGH is becoming a critical issue as we are trying to true-up SIW server data.  Please let me know if you and when this can be completed."
AND vwServers.ServerName NOT IN ('WSRDP302A','LSRDP300A')
AND vwServers.serverid not in 
(select serverid from dbo._tmpcvFactoryFeedReleaseServers
where Relased is null)
--AND vwServers.DecommissionedDate IS NULL
-- Added on 9/21/12 (PTM: 297378)
AND vwServers.CommissionDate IS NOT NULL
--and (RIGHT(vwServers.ServerName, 1)<>'Z' OR isnumeric(substring(right(vwServers.ServerName,4),1,2)) = 1)
-- ********************************************************************

-- 3/12/13 (Healy): Per Jeff DiFiore, Bill Ross and Jim White, removed TIV servers since they renamed them
-- to EBR.  TIV is 96 hour recovery, EBR is 6 hour recovery.  Since SIW was renamed, these were reappearing.
AND vwServers.ServerName NOT IN ('LSTIV300A','LSTIV301A','LSTIV302A')

-- 5/13/13 (Healy): Per Jeff DiFiore, removed the following servers due to renames...
AND vwServers.ServerName NOT IN ('WSISD300C')	-- renamed to WWDS317
AND vwServers.ServerName NOT IN ('WSISD301C')	-- renamed to WWDS318
) AS cvDR



GO

