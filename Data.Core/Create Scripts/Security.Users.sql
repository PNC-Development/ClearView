USE [Clearview]
GO
/****** Object:  User [CORPDEV\SLABADM]    Script Date: 07/31/2009 12:09:35 ******/
GO
CREATE USER [CORPDEV\SLABADM] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [cvexception]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [cvexception] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [cvperformance]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [cvperformance] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [cvreader]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [cvreader] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [cvreports]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [cvreports] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [cvsiw]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [cvsiw] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [cvuser]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [cvuser] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[db_owner]
GO
/****** Object:  User [dbo]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [dbo] FOR LOGIN [sa] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [guest]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [guest] WITH DEFAULT_SCHEMA=[guest]
GO
/****** Object:  User [INFORMATION_SCHEMA]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [INFORMATION_SCHEMA]
GO
/****** Object:  User [OHCLEUTL4002\fakeuser]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [OHCLEUTL4002\fakeuser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [slabadm]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [slabadm] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [sys]    Script Date: 07/31/2009 12:09:36 ******/
GO
CREATE USER [sys]