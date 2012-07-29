USE [master]
GO
/****** Object:  Database [RivendellRegression]    Script Date: 07/27/2012 00:38:20 ******/
CREATE DATABASE [RivendellRegression] ON  PRIMARY 
( NAME = N'RivendellRegression', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellRegression.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RivendellRegression_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RivendellRegression_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [RivendellRegression] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RivendellRegression].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RivendellRegression] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [RivendellRegression] SET ANSI_NULLS OFF
GO
ALTER DATABASE [RivendellRegression] SET ANSI_PADDING OFF
GO
ALTER DATABASE [RivendellRegression] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [RivendellRegression] SET ARITHABORT OFF
GO
ALTER DATABASE [RivendellRegression] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [RivendellRegression] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [RivendellRegression] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [RivendellRegression] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [RivendellRegression] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [RivendellRegression] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [RivendellRegression] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [RivendellRegression] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [RivendellRegression] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [RivendellRegression] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [RivendellRegression] SET  DISABLE_BROKER
GO
ALTER DATABASE [RivendellRegression] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [RivendellRegression] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [RivendellRegression] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [RivendellRegression] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [RivendellRegression] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [RivendellRegression] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [RivendellRegression] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [RivendellRegression] SET  READ_WRITE
GO
ALTER DATABASE [RivendellRegression] SET RECOVERY FULL
GO
ALTER DATABASE [RivendellRegression] SET  MULTI_USER
GO
ALTER DATABASE [RivendellRegression] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [RivendellRegression] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'RivendellRegression', N'ON'
GO
USE [RivendellRegression]
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
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (1, N'Elrond', N'Hugo Weaving')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (3, N'Celebrian', N'unannounced')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (4, N'Elladan', N'James Phelps')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (5, N'Elrohir', N'Oliver Phelps')
INSERT [Configuration].[ApplicationSetting] ([Id], [name], [value]) VALUES (6, N'Arwen', N'Liv Tyler')
SET IDENTITY_INSERT [Configuration].[ApplicationSetting] OFF
