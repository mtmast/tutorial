USE [db_mv_rent_shop]
GO
/****** Object:  Table [dbo].[tbl_customer]    Script Date: 6/6/2024 2:58:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_customer](
	[cus_id] [int] NOT NULL,
	[full_name] [varchar](50) NOT NULL,
	[salutation] [varchar](4) NOT NULL,
	[address] [varchar](150) NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NULL,
 CONSTRAINT [PK_tbl_customer] PRIMARY KEY CLUSTERED 
(
	[cus_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_movie]    Script Date: 6/6/2024 2:58:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_movie](
	[mv_id] [int] NOT NULL,
	[title] [varchar](125) NOT NULL,
	[release_date] [date] NOT NULL,
	[created_at] [datetime] NOT NULL,
 CONSTRAINT [PK_tbl_movie] PRIMARY KEY CLUSTERED 
(
	[mv_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_rent]    Script Date: 6/6/2024 2:58:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_rent](
	[rent_id] [int] NOT NULL,
	[cus_id] [int] NOT NULL,
	[mv_id] [int] NOT NULL,
	[rent_at] [datetime] NOT NULL,
	[return_at] [datetime] NULL,
 CONSTRAINT [PK_tbl_rent] PRIMARY KEY CLUSTERED 
(
	[rent_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbl_customer] ([cus_id], [full_name], [salutation], [address], [created_at], [updated_at]) VALUES (1, N'Sandy', N'Ms', N'First Street Plot No 4', CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([cus_id], [full_name], [salutation], [address], [created_at], [updated_at]) VALUES (2, N'John', N'Mr', N'Second Street Plot No 5', CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([cus_id], [full_name], [salutation], [address], [created_at], [updated_at]) VALUES (3, N'Jonet Jones', N'Mr', N'Second Street Plot No 7', CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[tbl_movie] ([mv_id], [title], [release_date], [created_at]) VALUES (1, N'Daddy''s Little Girls', CAST(N'2020-02-02' AS Date), CAST(N'2020-03-02T00:00:00.000' AS DateTime))
INSERT [dbo].[tbl_movie] ([mv_id], [title], [release_date], [created_at]) VALUES (2, N'Clash of the Titans', CAST(N'2019-01-01' AS Date), CAST(N'2019-04-05T00:00:00.000' AS DateTime))
INSERT [dbo].[tbl_movie] ([mv_id], [title], [release_date], [created_at]) VALUES (3, N'Forgetting Sarah Marshi', CAST(N'2013-01-01' AS Date), CAST(N'2021-08-23T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tbl_rent] ([rent_id], [cus_id], [mv_id], [rent_at], [return_at]) VALUES (1, 1, 1, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([rent_id], [cus_id], [mv_id], [rent_at], [return_at]) VALUES (2, 1, 2, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([rent_id], [cus_id], [mv_id], [rent_at], [return_at]) VALUES (3, 2, 3, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([rent_id], [cus_id], [mv_id], [rent_at], [return_at]) VALUES (4, 2, 2, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([rent_id], [cus_id], [mv_id], [rent_at], [return_at]) VALUES (5, 3, 1, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
GO
ALTER TABLE [dbo].[tbl_rent]  WITH CHECK ADD  CONSTRAINT [FK_tbl_customer_tbl_rent] FOREIGN KEY([cus_id])
REFERENCES [dbo].[tbl_customer] ([cus_id])
GO
ALTER TABLE [dbo].[tbl_rent] CHECK CONSTRAINT [FK_tbl_customer_tbl_rent]
GO
ALTER TABLE [dbo].[tbl_rent]  WITH CHECK ADD  CONSTRAINT [FK_tbl_movie_tbl_rent] FOREIGN KEY([mv_id])
REFERENCES [dbo].[tbl_movie] ([mv_id])
GO
ALTER TABLE [dbo].[tbl_rent] CHECK CONSTRAINT [FK_tbl_movie_tbl_rent]
GO
ALTER TABLE [dbo].[tbl_customer]  WITH CHECK ADD CHECK  (([salutation]='Ms' OR [salutation]='Mr'))
GO
ALTER TABLE [dbo].[tbl_customer]  WITH CHECK ADD CHECK  (([salutation]='Ms' OR [salutation]='Mr'))
GO
