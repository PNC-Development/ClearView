USE [master]
GO
/****** Object:  Database [ClearViewServiceEditor]    Script Date: 08/04/2009 14:37:23 ******/
CREATE DATABASE [ClearViewServiceEditor] ON  PRIMARY 
( NAME = N'ClearViewServiceEditor', FILENAME = N'F:\Production\Database\ClearViewServiceEditor.mdf' , SIZE = 15360KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ClearViewServiceEditor_log', FILENAME = N'G:\Production\Logs\ClearViewServiceEditor_log.ldf' , SIZE = 416384KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'ClearViewServiceEditor', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ClearViewServiceEditor].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [ClearViewServiceEditor] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET ARITHABORT OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ClearViewServiceEditor] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ClearViewServiceEditor] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ClearViewServiceEditor] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ClearViewServiceEditor] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ClearViewServiceEditor] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ClearViewServiceEditor] SET  READ_WRITE 
GO
ALTER DATABASE [ClearViewServiceEditor] SET RECOVERY FULL 
GO
ALTER DATABASE [ClearViewServiceEditor] SET  MULTI_USER 
GO
ALTER DATABASE [ClearViewServiceEditor] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ClearViewServiceEditor] SET DB_CHAINING OFF  