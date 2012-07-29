USE [master]
GO
/****** Object:  Database [RivendellProduction]    Script Date: 07/27/2012 00:38:20 ******/
CREATE DATABASE [RivendellProduction] ON  PRIMARY 
( NAME = N'RivendellProduction', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellProduction.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RivendellProduction_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellProduction_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [RivendellProduction] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RivendellProduction].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RivendellProduction] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [RivendellProduction] SET ANSI_NULLS OFF
GO
ALTER DATABASE [RivendellProduction] SET ANSI_PADDING OFF
GO
ALTER DATABASE [RivendellProduction] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [RivendellProduction] SET ARITHABORT OFF
GO
ALTER DATABASE [RivendellProduction] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [RivendellProduction] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [RivendellProduction] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [RivendellProduction] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [RivendellProduction] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [RivendellProduction] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [RivendellProduction] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [RivendellProduction] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [RivendellProduction] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [RivendellProduction] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [RivendellProduction] SET  DISABLE_BROKER
GO
ALTER DATABASE [RivendellProduction] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [RivendellProduction] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [RivendellProduction] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [RivendellProduction] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [RivendellProduction] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [RivendellProduction] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [RivendellProduction] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [RivendellProduction] SET  READ_WRITE
GO
ALTER DATABASE [RivendellProduction] SET RECOVERY FULL
GO
ALTER DATABASE [RivendellProduction] SET  MULTI_USER
GO
ALTER DATABASE [RivendellProduction] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [RivendellProduction] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'RivendellProduction', N'ON'
GO
USE [RivendellProduction]
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
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (1, N'Elrond', N'First Age 532')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (3, N'Celebrian', N'Second Age 1350')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (4, N'Elladan', N'Third Age 130')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (5, N'Elrohir', N'Third Age 130')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (6, N'Arwen', N'Third Age 241')
SET IDENTITY_INSERT [Configuration].[ApplicationSetting] OFF
