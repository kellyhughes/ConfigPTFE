USE [master]
GO
/****** Object:  Database [RivendellIntegration]    Script Date: 07/27/2012 00:38:20 ******/
CREATE DATABASE [RivendellIntegration] ON  PRIMARY 
( NAME = N'RivendellIntegration', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellIntegration.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RivendellIntegration_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellIntegration_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [RivendellIntegration] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RivendellIntegration].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RivendellIntegration] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [RivendellIntegration] SET ANSI_NULLS OFF
GO
ALTER DATABASE [RivendellIntegration] SET ANSI_PADDING OFF
GO
ALTER DATABASE [RivendellIntegration] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [RivendellIntegration] SET ARITHABORT OFF
GO
ALTER DATABASE [RivendellIntegration] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [RivendellIntegration] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [RivendellIntegration] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [RivendellIntegration] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [RivendellIntegration] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [RivendellIntegration] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [RivendellIntegration] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [RivendellIntegration] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [RivendellIntegration] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [RivendellIntegration] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [RivendellIntegration] SET  DISABLE_BROKER
GO
ALTER DATABASE [RivendellIntegration] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [RivendellIntegration] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [RivendellIntegration] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [RivendellIntegration] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [RivendellIntegration] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [RivendellIntegration] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [RivendellIntegration] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [RivendellIntegration] SET  READ_WRITE
GO
ALTER DATABASE [RivendellIntegration] SET RECOVERY FULL
GO
ALTER DATABASE [RivendellIntegration] SET  MULTI_USER
GO
ALTER DATABASE [RivendellIntegration] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [RivendellIntegration] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'RivendellIntegration', N'ON'
GO
USE [RivendellIntegration]
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
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (1, N'Elrond', N'Father')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (3, N'Celebrian', N'Mother')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (4, N'Elladan', N'Son')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (5, N'Elrohir', N'Son')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (6, N'Arwen', N'Daughter')
SET IDENTITY_INSERT [Configuration].[ApplicationSetting] OFF
