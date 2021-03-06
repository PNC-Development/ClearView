USE [ClearViewServiceEditor]
GO
/****** Object:  StoredProcedure [dbo].[sep_addConfig]    Script Date: 07/31/2009 13:42:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE procedure [dbo].[sep_addConfig]
	@serviceid int,
	@fieldid int,
	@question text,
	@dbfield int,
	@length int,
	@width int,
	@height int,
	@checked int,
	@direction int,
	@multiple int,
	@tip text,
	@required int,
	@required_text text,
	@display int,
	@enabled int,
	@id int output
AS
INSERT INTO
	set_config
VALUES
(
	@serviceid,
	@fieldid,
	@question,
	@dbfield,
	@length,
	@width,
	@height,
	@checked,
	@direction,
	@multiple,
	@tip,
	@required,
	@required_text,
	@display,
	@enabled,
	getdate(),
	getdate(),
	0
)
SET @id = SCOPE_IDENTITY()




GO
/****** Object:  StoredProcedure [dbo].[sep_addConfigValue]    Script Date: 07/31/2009 13:42:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sep_addConfigValue]
	@configid int,
	@value text,
	@display int
AS
INSERT INTO
	set_config_values
VALUES
(
	@configid,
	@value,
	@display,
	getdate(),
	getdate(),
	0
)

GO
/****** Object:  StoredProcedure [dbo].[sep_addField]    Script Date: 07/31/2009 13:42:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sep_addField]
	@name varchar(100),
	@type varchar(10),
	@code varchar(10),
	@description text,
	@image varchar(100),
	@length int,
	@width int,
	@display int,
	@enabled int
AS
INSERT INTO
	set_fields
VALUES
(
	@name,
	@type,
	@code,
	@description,
	@image,
	@length,
	@width,
	@display,
	@enabled,
	getdate(),
	getdate(),
	0
)


GO
/****** Object:  StoredProcedure [dbo].[sep_addFieldDB]    Script Date: 07/31/2009 13:42:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[sep_addFieldDB]
	@id int output
AS
INSERT INTO
	set_fields_db
VALUES
(
	getdate()
)
SET @id = SCOPE_IDENTITY()










GO
/****** Object:  StoredProcedure [dbo].[sep_addRequest]    Script Date: 07/31/2009 13:42:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[sep_addRequest]
	@requestid int,
	@serviceid int,
	@number int,
	@title varchar(100),
	@priority int,
	@statement text,
	@start_date datetime,
	@end_date datetime,
	@expedite int
AS
DELETE FROM
	set_requests
WHERE
	requestid = @requestid
	AND serviceid = @serviceid
	AND number = @number
INSERT INTO
	set_requests
VALUES
(
	@requestid,
	@serviceid,
	@number,
	@title,
	@priority,
	@statement,
	@start_date,
	@end_date,
	@expedite,
	getdate(),
	0
)














GO
/****** Object:  StoredProcedure [dbo].[sep_deleteConfig]    Script Date: 07/31/2009 13:42:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sep_deleteConfig]
	@id int
AS
UPDATE
	set_config
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id

GO
/****** Object:  StoredProcedure [dbo].[sep_deleteConfigValues]    Script Date: 07/31/2009 13:42:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sep_deleteConfigValues]
	@configid int
AS
UPDATE
	set_config_values
SET
	deleted = 1,
	modified = getdate()
WHERE
	configid = @configid
	AND deleted = 0


GO
/****** Object:  StoredProcedure [dbo].[sep_deleteField]    Script Date: 07/31/2009 13:42:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sep_deleteField]
	@id int
AS
UPDATE
	set_fields
SET
	deleted = 1,
	modified = getdate()
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[sep_getConfig]    Script Date: 07/31/2009 13:42:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create PROCEDURE [dbo].[sep_getConfig]
	@id int
AS
SELECT
	*
FROM
	set_config
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[sep_getConfigs]    Script Date: 07/31/2009 13:42:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [dbo].[sep_getConfigs]
	@serviceid int,
	@enabled int
AS
SELECT
	set_fields.name,
	set_fields.type,
	set_fields.code,
	CASE 
		WHEN set_fields.width > 0 THEN set_fields.width
		ELSE set_config.width
	END AS width,
	set_config.*
FROM
	set_config
		INNER JOIN
			set_fields
		ON
			set_config.fieldid = set_fields.id
			AND set_fields.enabled = 1
			AND set_fields.deleted = 0
WHERE
	set_config.deleted = 0
	AND set_config.serviceid = @serviceid
	and set_config.enabled >= @enabled
ORDER BY
	set_config.display







GO
/****** Object:  StoredProcedure [dbo].[sep_getConfigValues]    Script Date: 07/31/2009 13:42:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sep_getConfigValues]
	@configid int
AS
SELECT
	*
FROM
	set_config_values
WHERE
	configid = @configid
	AND deleted = 0


GO
/****** Object:  StoredProcedure [dbo].[sep_getField]    Script Date: 07/31/2009 13:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sep_getField]
	@id int
AS
SELECT
	*
FROM
	set_fields
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[sep_getFields]    Script Date: 07/31/2009 13:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sep_getFields]
	@enabled int
AS
SELECT
	*
FROM
	set_fields
WHERE
	deleted = 0
	and enabled >= @enabled
ORDER BY
	display


GO
/****** Object:  StoredProcedure [dbo].[sep_updateConfig]    Script Date: 07/31/2009 13:42:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[sep_updateConfig]
	@id int,
	@question text,
	@length int,
	@width int,
	@height int,
	@checked int,
	@direction int,
	@multiple int,
	@tip text,
	@required int,
	@required_text text,
	@enabled int
AS
UPDATE
	set_config
SET
	question = @question,
	length = @length,
	width = @width,
	height = @height,
	checked = @checked,
	direction = @direction,
	multiple = @multiple,
	tip = @tip,
	required = @required,
	required_text = @required_text,
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[sep_updateConfigOrder]    Script Date: 07/31/2009 13:42:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create PROCEDURE [dbo].[sep_updateConfigOrder]
	@id int,
	@display int
AS
UPDATE
	set_config
SET
	display = @display,
	modified = getdate()
WHERE
	id = @id



GO
/****** Object:  StoredProcedure [dbo].[sep_updateField]    Script Date: 07/31/2009 13:42:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sep_updateField]
	@id int,
	@name varchar(100),
	@type varchar(10),
	@code varchar(10),
	@description text,
	@image varchar(100),
	@length int,
	@width int,
	@enabled int
AS
UPDATE
	set_fields
SET
	name = @name,
	type = @type,
	code = @code,
	description = @description,
	image = @image,
	length = @length,
	width = @width,
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[sep_updateFieldEnabled]    Script Date: 07/31/2009 13:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sep_updateFieldEnabled]
	@id int,
	@enabled int
AS
UPDATE
	set_fields
SET
	enabled = @enabled,
	modified = getdate()
WHERE
	id = @id


GO
/****** Object:  StoredProcedure [dbo].[sep_updateFieldOrder]    Script Date: 07/31/2009 13:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sep_updateFieldOrder]
	@id int,
	@display int
AS
UPDATE
	set_fields
SET
	display = @display,
	modified = getdate()
WHERE
	id = @id

