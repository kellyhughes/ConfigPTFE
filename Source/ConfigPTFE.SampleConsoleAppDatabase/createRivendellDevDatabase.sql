USE [master]
GO
/****** Object:  Database [RivendellDev]    Script Date: 07/27/2012 00:38:20 ******/
CREATE DATABASE [RivendellDev] ON  PRIMARY 
( NAME = N'RivendellDev', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellDev.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RivendellDev_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellDev_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [RivendellDev] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RivendellDev].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RivendellDev] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [RivendellDev] SET ANSI_NULLS OFF
GO
ALTER DATABASE [RivendellDev] SET ANSI_PADDING OFF
GO
ALTER DATABASE [RivendellDev] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [RivendellDev] SET ARITHABORT OFF
GO
ALTER DATABASE [RivendellDev] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [RivendellDev] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [RivendellDev] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [RivendellDev] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [RivendellDev] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [RivendellDev] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [RivendellDev] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [RivendellDev] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [RivendellDev] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [RivendellDev] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [RivendellDev] SET  DISABLE_BROKER
GO
ALTER DATABASE [RivendellDev] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [RivendellDev] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [RivendellDev] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [RivendellDev] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [RivendellDev] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [RivendellDev] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [RivendellDev] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [RivendellDev] SET  READ_WRITE
GO
ALTER DATABASE [RivendellDev] SET RECOVERY FULL
GO
ALTER DATABASE [RivendellDev] SET  MULTI_USER
GO
ALTER DATABASE [RivendellDev] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [RivendellDev] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'RivendellDev', N'ON'
GO
USE [RivendellDev]
GO
/****** Object:  Schema [Configuration]    Script Date: 07/27/2012 00:38:20 ******/
CREATE SCHEMA [Configuration] AUTHORIZATION [dbo]
GO
/****** Object:  Table [Configuration].[ApplicationSetting]    Script Date: 07/27/2012 00:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Configuration].[ApplicationSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](256) NOT NULL,
	[value] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_ApplicationSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ApplicationSetting] ON [Configuration].[ApplicationSetting] 
(
	[name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET IDENTITY_INSERT [Configuration].[ApplicationSetting] ON
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (1, N'Elrond', N'Vault of Stars')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (3, N'Celebrian', N'Silver Crown Gift')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (4, N'Elladan', N'Wise Man')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (5, N'Elrohir', N'Knight')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (6, N'Arwen', N'Noble Maiden')
SET IDENTITY_INSERT [Configuration].[ApplicationSetting] OFF
