USE [Clearview]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_AcquisitionCost]    Script Date: 07/31/2009 12:07:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:	Kevin Frazier
-- Create date: 2008-09-30
-- Description:	Returns the Acquisition cost for a modelid.
-- =============================================
CREATE FUNCTION [dbo].[fn_AcquisitionCost] 
(
	-- Add the parameters for the function here
	@ModelID int
)
RETURNS money
AS
BEGIN
	RETURN
		(
			SELECT 
				IsNull(Sum(acq.cost), 0)
			FROM ClearView.dbo.cv_forecast_acquisitions acq
				INNER JOIN ClearView.dbo.cv_forecast_line_items fli
					ON acq.lineitemid = fli.id
					And fli.enabled = 1
					And fli.deleted = 0
			WHERE
				acq.enabled = 1
				And acq.deleted = 0
				And acq.modelid = @ModelID
		)

END


GO
/****** Object:  UserDefinedFunction [dbo].[fn_ProjectStatus]    Script Date: 07/31/2009 12:07:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author: 	Kevin Frazier
-- Create date: 2008/01/01
-- Description:	Returns the text version of the project status code from a numeric value.
-- =============================================
CREATE FUNCTION [dbo].[fn_ProjectStatus] 
(
	-- Add the parameters for the function here
	@StatusCode int
)
RETURNS varchar(25)
AS
BEGIN
	-- Add the T-SQL statements to compute the return value here
	RETURN
		(
			SELECT CASE @StatusCode 
				WHEN -2 THEN 'Cancelled' 
				WHEN -1 THEN 'Denied' 
				WHEN  0 THEN 'Pending' 
				WHEN  1 THEN 'Approved' 
				WHEN  2 THEN 'Active' 
				WHEN  3 THEN 'Completed' 
				WHEN  5 THEN 'Hold' 
				WHEN 10 THEN 'Future'
			END
		)
END


GO
/****** Object:  UserDefinedFunction [dbo].[fnGetServiceFolderPath]    Script Date: 07/31/2009 12:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnGetServiceFolderPath](@folderID int)
RETURNS varchar(4000)
AS
BEGIN

	DECLARE @ServiceFolderPath varchar(4000)
	DECLARE @folderPath varchar(4000)
	DECLARE @Parent as int
	SET @folderPath=''
	SET @ServiceFolderPath=''

	
	SELECT @ServiceFolderPath = Name from cv_services_folder cvServicesFolder
	WHERE id=@folderID

	SELECT @Parent =parent from cv_services_folder cvServicesFolder
	WHERE id=@folderID

	WHILE (@Parent<>0) 
	BEGIN
		
		SELECT @folderPath= Name from cv_services_folder cvServicesFolder
		WHERE id=@Parent
		
		SET @ServiceFolderPath=@folderPath+' >> ' +@ServiceFolderPath
		
		SELECT @Parent =parent from cv_services_folder cvServicesFolder
		WHERE id=@Parent
	END

	RETURN @ServiceFolderPath
	
END

GO
/****** Object:  UserDefinedFunction [dbo].[getDriveLetters]    Script Date: 07/31/2009 12:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[getDriveLetters](@answerid int)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @drives varchar(1000)
	SELECT @drives = COALESCE(@drives + ', ', '') + tbl1.letter
		FROM 
			(
				SELECT DISTINCT
					tbl2.letter
				FROM 
					(
						SELECT id, letter FROM cv_storage_drive_letters
						UNION ALL
						SELECT -100, 'F'
					) tbl2
						INNER JOIN
							cv_storage_luns
						ON
							cv_storage_luns.driveid = tbl2.id
							AND cv_storage_luns.deleted = 0
				WHERE 
					cv_storage_luns.answerid = @answerid
			) tbl1
	RETURN @drives
end



