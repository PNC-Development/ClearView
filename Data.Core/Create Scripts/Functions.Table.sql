USE [Clearview]
GO
/****** Object:  UserDefinedFunction [dbo].[fnGetForecastModel]    Script Date: 07/31/2009 12:07:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnGetForecastModel]
(
@answerid int
)

RETURNS
@ModelIDList table
(
	modelid varchar(Max)
)
AS
BEGIN

IF Exists(select cv_solution_codes.modelid FROM dbo.cv_solution_codes
WHERE id =(select SolutionCode FROM cv_servername_subapplications 
WHERE id=(Select subapplicationid  FROM cv_forecast_answers  WHERE id =@answerid)))
BEGIN
	--Insert into model list and return
	INSERT  INTO @ModelIDList(modelid)  select cv_solution_codes.modelid FROM dbo.cv_solution_codes
	WHERE id =(select SolutionCode FROM cv_servername_subapplications 
	WHERE id=(Select subapplicationid  FROM cv_forecast_answers  WHERE id =@answerid))

	RETURN
END


IF Exists(select cv_solution_codes.modelid FROM dbo.cv_solution_codes
WHERE id =(select SolutionCode FROM cv_servername_applications 
WHERE id=(Select applicationid  FROM cv_forecast_answers  WHERE id =@answerid)))
BEGIN
	--Insert into model list and return
	INSERT  INTO @ModelIDList(modelid) select cv_solution_codes.modelid FROM dbo.cv_solution_codes
	WHERE id =(select SolutionCode FROM cv_servername_applications 
	WHERE id=(Select applicationid  FROM cv_forecast_answers  WHERE id =@answerid))
	RETURN
END



DECLARE @high int
DECLARE @low int
DECLARE @high_overall float
DECLARE @standard_overall float
DECLARE @low_overall float
DECLARE @overall float
DECLARE @quantity float
DECLARE @TempTable table
(  modelid int,
   mod int,
   questionid int,
   priority int
)

SET @quantity = (SELECT quantity FROM cv_forecast_answers WHERE id = @answerid AND deleted = 0)

SET @high_overall = (SELECT ISNULL(SUM(high_total),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(high_total),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
IF (@high_overall = 0.00)
	SET @high_overall = (SELECT ISNULL(SUM(high_qa),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(high_qa),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
IF (@high_overall = 0.00)
	SET @high_overall = (SELECT ISNULL(SUM(high_test),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(high_test),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)

SET @standard_overall = (SELECT ISNULL(SUM(standard_total),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(standard_total),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
IF (@standard_overall = 0.00)
	SET @standard_overall = (SELECT ISNULL(SUM(standard_qa),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(standard_qa),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
IF (@standard_overall = 0.00)
	SET @standard_overall = (SELECT ISNULL(SUM(standard_test),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(standard_test),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)

SET @low_overall = (SELECT ISNULL(SUM(low_total),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(low_total),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
IF (@low_overall = 0.00)
	SET @low_overall = (SELECT ISNULL(SUM(low_qa),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(low_qa),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
IF (@high_overall = 0.00)
	SET @low_overall = (SELECT ISNULL(SUM(low_test),0) FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0) + (SELECT ISNULL(SUM(low_test),0) FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)

SET @overall = @high_overall + @standard_overall + @low_overall
SET @overall = @overall / @quantity



SET @high = (SELECT high FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0)
SET @low = (SELECT low FROM cv_forecast_answers_storage WHERE answerid = @answerid AND deleted = 0)
IF (@high is null)
	SET @high = 0
IF (@low is null)
	SET @low = 0
DECLARE @high_os int
DECLARE @low_os int
SET @high_os = (SELECT high FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
SET @low_os = (SELECT low FROM cv_forecast_answers_storage_os WHERE answerid = @answerid AND deleted = 0)
IF (@high_os is null)
	SET @high_os = 0
IF (@low_os is null)
	SET @low_os = 0

INSERT INTO @TempTable (modelid,mod,questionid,priority) 
SELECT
	cv_solution_codes.modelid,
	cv_models_property.modelid as mod,
	cv_forecast_answers_platform.questionid,
	cv_solution_codes.priority
FROM
	cv_solution_codes
		INNER JOIN
			cv_solution_selection
				INNER JOIN
					cv_forecast_answers_platform
				ON
					cv_solution_selection.responseid = cv_forecast_answers_platform.responseid
					AND cv_forecast_answers_platform.deleted = 0
					AND cv_forecast_answers_platform.answerid = @answerid
		ON
			cv_solution_codes.id = cv_solution_selection.codeid
			AND cv_solution_selection.deleted = 0
		INNER JOIN
			cv_solution_codes_locations
				INNER JOIN
					cv_forecast_answers
				ON
					cv_forecast_answers.id = @answerid
					AND cv_forecast_answers.deleted = 0
					AND cv_forecast_answers.classid = cv_solution_codes_locations.classid
					AND cv_forecast_answers.environmentid = cv_solution_codes_locations.environmentid
					AND cv_forecast_answers.addressid = cv_solution_codes_locations.addressid
		ON
			cv_solution_codes.id = cv_solution_codes_locations.codeid
			AND cv_solution_codes_locations.deleted = 0
		INNER JOIN
			cv_models_property
				INNER JOIN
					cv_models
				ON
					cv_models_property.modelid = cv_models.id
					AND cv_models.enabled = 1
					AND cv_models.deleted = 0
		ON
			cv_models_property.id = cv_solution_codes.modelid
			AND cv_models_property.high_performance >= @high
			AND cv_models_property.low_performance >= @low
			AND cv_models_property.high_performance >= @high_os
			AND cv_models_property.low_performance >= @low_os
			AND cv_models_property.deleted = 0
			AND cv_models_property.StorageThresholdMin <= @overall
			AND cv_models_property.StorageThresholdMax >= @overall
WHERE
	cv_solution_codes.enabled = 1
	AND cv_solution_codes.deleted = 0
ORDER BY
	cv_solution_codes.priority

DECLARE @max int
DECLARE @questionid int
DECLARE @stop int
DECLARE @stop2 int
SET @stop2 = 0
SET @max = (SELECT max(priority) FROM @TempTable)
WHILE @max is not null AND @max > 0 AND @stop2 = 0
BEGIN
	SET @stop = 0
	DECLARE c1 CURSOR FOR SELECT DISTINCT questionid FROM cv_forecast_answers_platform WHERE answerid = @answerid AND deleted = 0
	OPEN c1
	FETCH NEXT FROM c1 INTO @questionid
	WHILE @@FETCH_STATUS = 0 AND @stop = 0
	BEGIN
		DECLARE @return int
		SET @return = (SELECT DISTINCT priority FROM @TempTable WHERE questionid = @questionid AND priority = @max)
		if (@return is null)
		BEGIN
			SET @stop = 1
			--print 'MAX ' + convert(varchar, @max) + ' not found for questionid ' + convert(varchar, @questionid)
		END
		FETCH NEXT FROM c1 INTO @questionid
	END
	CLOSE c1
	DEALLOCATE c1
	IF (@stop = 0)
		SET @stop2 = 1
	ELSE
		SET @max = @max - 1
END
--print 'MAX ' + convert(varchar, @max)
/** SELECT DISTINCT modelid FROM #temp WHERE priority = @max **/
/* SELECT * FROM #temp ORDER BY priority DESC, questionid */

INSERT  INTO @ModelIDList(modelid)  SELECT DISTINCT modelid FROM @TempTable WHERE priority = @max 

RETURN

END

GO
/****** Object:  UserDefinedFunction [dbo].[fnParseCommaSeparatedLists]    Script Date: 07/31/2009 12:07:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*********************************************************************************
FUNCTION Name		: fnParseCommaSeparatedLists
Description			:	The following is a general purpose UDF to split comma separated lists into individual items.
Input Parameters	:	
Output Parameters	: 
Date Created:       : 30-Mar-2008
Author:             : Shyam Pathade
----------------------------------------------------------------------------------
Modification History
Modified By		Date		Description

*********************************************************************************/ 
CREATE FUNCTION [dbo].[fnParseCommaSeparatedLists] 
(
	@List varchar(Max)
)
RETURNS 
@ParsedList table
(
	ListValue varchar(Max)
)
AS
BEGIN
	DECLARE @ListValue varchar(Max), @Pos int

	SET @List = LTRIM(RTRIM(@List))+ ','
	SET @Pos = CHARINDEX(',', @List, 1)

	IF REPLACE(@List, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @ListValue = LTRIM(RTRIM(LEFT(@List, @Pos - 1)))
			IF @ListValue <> ''
			BEGIN
				INSERT INTO @ParsedList (ListValue) 
				VALUES (@ListValue) --Use Appropriate conversion
			END
			SET @List = RIGHT(@List, LEN(@List) - @Pos)
			SET @Pos = CHARINDEX(',', @List, 1)

		END
	END	
	RETURN
END
