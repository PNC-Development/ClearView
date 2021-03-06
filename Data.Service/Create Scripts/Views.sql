USE [ClearViewService]
GO
/****** Object:  View [dbo].[vw_change]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_change]
AS
SELECT     
	TOP (100) PERCENT 
 	c.id,
	c.change_number,
	c.implementor_name,
	c.implementor_dept,
	c.addtl_implementor_name,
	c.severity,
	c.category,
	c.change_what,
	c.change_how,
	c.priority,
	c.portfolio_ncc,
	c.business_area_ncc,
	c.change_startdatetime,
	c.description,
	c.change_enddatetime,
	c.close_datetime,
	c.completion_code,
	c.num_incidents,
	c.open_time,
	c.approval_groups,
	c.modified
FROM ClearViewService.dbo.cvs_change c


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
         Begin Table = "cvs_change"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 238
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_change'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_change'
GO
/****** Object:  View [dbo].[vw_ChangeMgmtSummaryByMonth]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:	Kevin Frazier (Vertex Computer Systems)
-- Create date: 06/26/2008
-- Description:	Total infrastructure changes grouped by month by category.
-- =============================================

CREATE VIEW [dbo].[vw_ChangeMgmtSummaryByMonth] AS
SELECT 
 	Max ( sq.Category ) As Category,
	Max ( sq.MonthNumber ) As MonthNumber,
	Max ( sq.MonthName ) As MonthName,
	Sum ( sq.TotalChanges ) As TotalChanges
FROM 
	(
		SELECT
			CASE Max ( Upper(c.category) )
				WHEN 'MIDRANGE' THEN 'MIDRANGE'
				WHEN 'NETWORK' THEN 'NETWORK'
				ELSE Max ( 'DISTRIBUTED' )
			END As Category,
			Max ( DatePart(Month, c.change_enddatetime) ) As MonthNumber,
			Max ( DateName(Month, c.change_enddatetime) ) As MonthName,
			Count(c.id) As TotalChanges
		FROM
			vw_change c
		WHERE 
--			Upper( LTrim( c.category) ) IN ('NETWORK', 'DISTRIBUTED SERVER', 'MIDRANGE')
			Substring(Upper(c.implementor_dept),1,29) = 'INFRASTRUCTURE IMPLEMENTATION'
			Or c.implementor_dept = 'IS INT ENTRY INFRASTRUCTURE IMPLEMENTATION'
		GROUP BY 
			DatePart(m, c.change_enddatetime),
			c.category,
			c.completion_code
	)sq
GROUP BY
 	sq.Category,
	sq.MonthNumber


GO
/****** Object:  View [dbo].[vw_ChangeMgmtSummaryByMonthPivot]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:	Kevin Frazier (Vertex Computer Systems)
-- Create date: 06/26/2008
-- Description:	Total infrastructure changes totaled by month by category and pivoted by month.
-- =============================================
CREATE VIEW [dbo].[vw_ChangeMgmtSummaryByMonthPivot] AS
SELECT
	Category,
	[1] AS January,
	[2] AS February,
	[3] AS March,
	[4] AS April,
	[5] AS May,
	[6] AS June,
	[7] AS July,
	[8] AS August,
	[9] AS September,
	[10] AS October,
	[11] AS November,
	[12] AS December
FROM 
(
	SELECT
  		TOP (100) Percent
		v.Category,
		v.MonthNumber,
		v.MonthName,
		v.TotalChanges
	FROM
		vw_ChangeMgmtSummaryByMonth v
	ORDER BY
		v.Category Asc,
		v.MonthNumber Asc
) sq
PIVOT
	(
		Sum ( sq.TotalChanges )
		FOR sq.MonthNumber In ( [1] , [2] , [3] , [4] , [5] , [6] , [7] , [8] , [9] , [10] , [11] , [12] )
	) AS pvt


GO
/****** Object:  View [dbo].[vw_incident]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_incident]
AS
SELECT
	i.id,
	i.incident_number,
	i.open_datetime,
	i.close_datetime,
	i.assigned_group,
	i.severity,
	i.assignee_name,
	i.contact_name,
	i.category,
	i.subcategory,
	i.component,
	i.change_control,
	i.cause_code,
	i.ack,
	i.isack,
	i.modified
FROM
	ClearViewService.dbo.cvs_incident i


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
         Begin Table = "cvs_incident"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 197
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_incident'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_incident'
GO
/****** Object:  View [dbo].[vw_incident_AckMinutes]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:	Kevin Frazier (Vertex Computer Systems)
-- Create date: 06/30/2008
-- Description:	View of incidents with Acknowledgement converted in to numeric minutes.
-- =============================================

CREATE VIEW [dbo].[vw_incident_AckMinutes] AS
SELECT
  	sq.Id,
	sq.IncidentNumber,
	sq.AssignedGroup,
	sq.SubCategory,
	sq.Severity,
	sq.AssigneeName,
	sq.ContactName,
	sq.Category,
	sq.Component,
	sq.ChangeControl,
	sq.CauseCode,
	sq.IsAck,
	sq.OpenDate,
	sq.CloseDate,
	sq.ModifiedDate,
	sq.Ack,
	CASE
		WHEN sq.AckFlag > 0 THEN RTrim(Substring(sq.Ack,1,SubstringEnd))
		ELSE 0
	END As AckMinutes
FROM
(
	SELECT 
  		i.id As Id,
		i.incident_number As IncidentNumber,
		i.assigned_group As AssignedGroup,
		i.subcategory As SubCategory,
		i.severity As Severity,
		i.assignee_name As AssigneeName,
		i.contact_name As ContactName,
		i.category As Category,
		i.component As Component,
		i.change_control As ChangeControl,
		i.cause_code As CauseCode,
		i.ack As Ack,
		i.isack As IsAck,
		i.open_datetime As OpenDate,
		i.close_datetime As CloseDate,
		i.modified As ModifiedDate,
		(PATINDEX ( '%minute%' , i.ack )-1) As SubstringEnd,
		CASE
			WHEN Len(i.ack) < 1 THEN 0
			ELSE 1
		END As AckFlag
	FROM
		vw_incident i
)sq


GO
/****** Object:  View [dbo].[vw_IncidentSummaryByMonth]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	Kevin Frazier (Vertex Computer Systems)
-- Create date: 06/27/2008
-- Description:	Total incidents totaled by month by category.
-- =============================================
CREATE VIEW [dbo].[vw_IncidentSummaryByMonth] AS
SELECT
 	Count(sq.Id) As TotalIncidents,
 	Max(sq.AssignedGroup) As AssignedGroup,
	Max(sq.SubCategory) As SubCategory,
	Max(sq.MonthNumber) As MonthNumber,
	Max(sq.MonthName) As MonthName
--	CASE
--		WHEN sq.AckFlag > 0 THEN RTrim(Substring(sq.Ack,1,SubstringEnd))
--		ELSE 0
--	END As AckMinutes
FROM
(
	SELECT 
  		i.id As Id,
		i.assigned_group As AssignedGroup,
		i.subcategory As SubCategory,
		i.ack As Ack,
		i.open_datetime As OpenDate,
		i.close_datetime As CloseDate,
		(PATINDEX ( '%minute%' , i.ack )-1) As SubstringEnd,
		CASE
			WHEN Len(i.ack) < 1 THEN 0
			ELSE 1
		END As AckFlag,
		CASE
			WHEN i.close_datetime IS NOT NULL THEN	DatePart(Month, i.close_datetime)
			ELSE 0
		END As MonthNumber,
		CASE
			WHEN i.close_datetime IS NOT NULL THEN DateName(Month, i.close_datetime) 
			ELSE 'Other'
		END As MonthName
	FROM
		vw_incident i
)sq
GROUP BY
 	sq.AssignedGroup,
 	sq.MonthNumber

GO
/****** Object:  View [dbo].[vw_IncidentSummaryByMonthPivot]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:	Kevin Frazier (Vertex Computer Systems)
-- Create date: 06/30/2008
-- Description:	Total incidents totaled by month by assigned group and pivoted by month.
-- =============================================
CREATE VIEW [dbo].[vw_IncidentSummaryByMonthPivot] AS
SELECT
	AssignedGroup,
	[0] As Other,
	[1] AS January,
	[2] AS February,
	[3] AS March,
	[4] AS April,
	[5] AS May,
	[6] AS June,
	[7] AS July,
	[8] AS August,
	[9] AS September,
	[10] AS October,
	[11] AS November,
	[12] AS December
FROM 
(
	SELECT
  		TOP (100) Percent
		v.AssignedGroup,
		v.MonthNumber,
		v.MonthName,
		v.TotalIncidents
	FROM
		vw_IncidentSummaryByMonth v
	ORDER BY
		v.AssignedGroup Asc,
		v.MonthNumber Asc
) sq
PIVOT
	(
		Sum ( sq.TotalIncidents )
		FOR sq.MonthNumber In ( [0], [1] , [2] , [3] , [4] , [5] , [6] , [7] , [8] , [9] , [10] , [11] , [12] )
	) AS pvt


GO
/****** Object:  View [dbo].[vw_ServiceSuccessStatusSummary]    Script Date: 07/31/2009 13:37:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	Kevin Frazier (Vertex Computer Systems)
-- Create date: 06/26/2008
-- Description:	Total infrastructure changes summarized by completion status.
-- =============================================
CREATE VIEW [dbo].[vw_ServiceSuccessStatusSummary] AS
SELECT
 	CASE Len(sq.Status)
		WHEN 0 THEN 'Successful'
		ELSE sq.Status
	END As Status,
	sq.TotalChanges
FROM
(
	SELECT
		Max 
			(
				Upper(Substring(q.completion_code,1,1)) + 
				Lower(Substring(q.completion_code,2,Len(q.completion_code)+1))
			) As Status,
		Count(q.completion_code) As TotalChanges
	FROM
		(
			SELECT
				c.id,
				CASE
					WHEN Len(LTrim(c.completion_code)) < 1 THEN 'SUCCESSFUL'
					ELSE c.completion_code
				END AS completion_code
			FROM
				vw_change c
		)q
	GROUP BY
		q.completion_code
)sq
