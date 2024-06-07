USE [db_mv_rent_shop]
GO
/****** Object:  Table [dbo].[tbl_customer]    Script Date: 6/7/2024 11:19:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_customer](
	[CusId] [int] NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[Salutation] [varchar](4) NOT NULL,
	[Address] [varchar](150) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_tbl_customer] PRIMARY KEY CLUSTERED 
(
	[CusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_movie]    Script Date: 6/7/2024 11:19:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_movie](
	[MvId] [int] NOT NULL,
	[Title] [varchar](125) NOT NULL,
	[ReleaseDate] [date] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_tbl_movie] PRIMARY KEY CLUSTERED 
(
	[MvId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_rent]    Script Date: 6/7/2024 11:19:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_rent](
	[RentId] [int] NOT NULL,
	[CusId] [int] NOT NULL,
	[MvId] [int] NOT NULL,
	[RentAt] [datetime] NOT NULL,
	[ReturnAt] [datetime] NULL,
 CONSTRAINT [PK_tbl_rent] PRIMARY KEY CLUSTERED 
(
	[RentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (1, N'Sandy', N'Ms', N'First Street Plot No 4', CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (2, N'John', N'Mr', N'Second Street Plot No 5', CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (3, N'Jonet Jones', N'Mr', N'Second Street Plot No 7', CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[tbl_movie] ([MvId], [Title], [ReleaseDate], [CreatedAt]) VALUES (1, N'Daddy''s Little Girls', CAST(N'2020-02-02' AS Date), CAST(N'2020-03-02T00:00:00.000' AS DateTime))
INSERT [dbo].[tbl_movie] ([MvId], [Title], [ReleaseDate], [CreatedAt]) VALUES (2, N'Clash of the Titans', CAST(N'2019-01-01' AS Date), CAST(N'2019-04-05T00:00:00.000' AS DateTime))
INSERT [dbo].[tbl_movie] ([MvId], [Title], [ReleaseDate], [CreatedAt]) VALUES (3, N'Forgetting Sarah Marshi', CAST(N'2013-01-01' AS Date), CAST(N'2021-08-23T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (1, 1, 1, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (2, 1, 2, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (3, 2, 3, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (4, 2, 2, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (5, 3, 1, CAST(N'2024-06-06T00:00:00.000' AS DateTime), NULL)
GO
ALTER TABLE [dbo].[tbl_rent]  WITH CHECK ADD  CONSTRAINT [FK_tbl_customer_tbl_rent] FOREIGN KEY([CusId])
REFERENCES [dbo].[tbl_customer] ([CusId])
GO
ALTER TABLE [dbo].[tbl_rent] CHECK CONSTRAINT [FK_tbl_customer_tbl_rent]
GO
ALTER TABLE [dbo].[tbl_rent]  WITH CHECK ADD  CONSTRAINT [FK_tbl_movie_tbl_rent] FOREIGN KEY([MvId])
REFERENCES [dbo].[tbl_movie] ([MvId])
GO
ALTER TABLE [dbo].[tbl_rent] CHECK CONSTRAINT [FK_tbl_movie_tbl_rent]
GO
