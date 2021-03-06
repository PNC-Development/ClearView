USE [ClearViewService]
GO
/****** Object:  StoredProcedure [dbo].[pr_addChangeDetails]    Script Date: 07/31/2009 13:37:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_addChangeDetails] 
	 @change_number varchar(25),
	 @implementor_name varchar(50),
	 @addtl_implementor_name varchar(50),
	 @severity int,
	 @category varchar(50),
	 @change_what varchar(50),
	 @change_how varchar(50), 
	 @priority varchar(15),
	 @portfolio_ncc varchar(50),
	 @business_area_ncc varchar(50),
	 @change_startdatetime datetime,
	 @description varchar(100),
	 @change_enddatetime datetime,
	 @close_datetime datetime,
	 @completion_code varchar(25),
	 @num_incidents int,
	 @open_time datetime,
	 @approval_groups varchar(50)	 
AS
BEGIN



INSERT INTO 
     cvs_change
VALUES
(
     @change_number,
	 @implementor_name,
	 @addtl_implementor_name,
	 @severity,
	 @category,
	 @change_what,
	 @change_how, 
	 @priority,
	 @portfolio_ncc,
	 @business_area_ncc,
	 @change_startdatetime,
	 @description,
	 @change_enddatetime,
	 @close_datetime,
	 @completion_code,
	 @num_incidents,
	 @open_time,
	 @approval_groups,
     getdate() 
) ;

SET NOCOUNT ON;
  
END



GO
/****** Object:  StoredProcedure [dbo].[pr_addIncidentDetails]    Script Date: 07/31/2009 13:37:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[pr_addIncidentDetails] 
	 @incident_number varchar(25),
	 @open_datetime datetime,
     @close_datetime datetime,
	 @assigned_group varchar(50),
	 @severity int,
	 @assignee_name varchar(50),
	 @contact_name varchar(50),
	 @category varchar(25),
     @subcategory varchar(25),
     @component varchar(25),
     @change_control  varchar(25),
     @cause_code varchar(50),
     @ack varchar(30),
     @isack int
     
AS
BEGIN



INSERT INTO 
     cvs_incident
VALUES
(
    @incident_number,
	 @open_datetime,
     @close_datetime,
	 @assigned_group,
	 @severity,
	 @assignee_name,
	 @contact_name,
	 @category,
     @subcategory,
     @component,
     @change_control,
     @cause_code,
     @ack,
     @isack,
     getdate() 
) ;

SET NOCOUNT ON;
  
END




