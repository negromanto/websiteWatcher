USE [WebsitesWatcher]
GO
/****** Object:  Schema [az_func]    Script Date: 29/04/2025 18:25:11 ******/
CREATE SCHEMA [az_func]
GO
/****** Object:  Table [az_func].[GlobalState]    Script Date: 29/04/2025 18:25:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [az_func].[GlobalState](
	[UserFunctionID] [char](16) NOT NULL,
	[UserTableID] [int] NOT NULL,
	[LastSyncVersion] [bigint] NOT NULL,
	[LastAccessTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserFunctionID] ASC,
	[UserTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [az_func].[Leases_22e0d40a602ada7a_901578250]    Script Date: 29/04/2025 18:25:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [az_func].[Leases_22e0d40a602ada7a_901578250](
	[Id] [uniqueidentifier] NOT NULL,
	[_az_func_ChangeVersion] [bigint] NOT NULL,
	[_az_func_AttemptCount] [int] NOT NULL,
	[_az_func_LeaseExpirationTime] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [az_func].[Leases_fda63ba29858ee60_901578250]    Script Date: 29/04/2025 18:25:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [az_func].[Leases_fda63ba29858ee60_901578250](
	[Id] [uniqueidentifier] NOT NULL,
	[_az_func_ChangeVersion] [bigint] NOT NULL,
	[_az_func_AttemptCount] [int] NOT NULL,
	[_az_func_LeaseExpirationTime] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Snapshots]    Script Date: 29/04/2025 18:25:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Snapshots](
	[Id] [uniqueidentifier] NOT NULL,
	[Content] [varchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Snapshots] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Timestamp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Websites]    Script Date: 29/04/2025 18:25:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Websites](
	[Id] [uniqueidentifier] NOT NULL,
	[Url] [varchar](max) NOT NULL,
	[XPathExpression] [varchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Websites] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [az_func].[GlobalState] ADD  DEFAULT (getutcdate()) FOR [LastAccessTime]
GO
ALTER TABLE [dbo].[Snapshots] ADD  DEFAULT (getutcdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Websites] ADD  DEFAULT (getutcdate()) FOR [Timestamp]
GO
