USE [db_mv_rent_shop]
GO
/****** Object:  Table [dbo].[tbl_movie]    Script Date: 6/13/2024 2:27:28 PM ******/
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
/****** Object:  Table [dbo].[tbl_rent]    Script Date: 6/13/2024 2:27:28 PM ******/
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
/****** Object:  Table [dbo].[tbl_customer]    Script Date: 6/13/2024 2:27:28 PM ******/
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
/****** Object:  View [dbo].[vw_cus_mv_rent]    Script Date: 6/13/2024 2:27:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_cus_mv_rent]
AS
SELECT dbo.tbl_customer.FullName, dbo.tbl_customer.Address, dbo.tbl_movie.Title, dbo.tbl_customer.Salutation, dbo.tbl_rent.RentId
FROM     dbo.tbl_customer INNER JOIN
                  dbo.tbl_rent ON dbo.tbl_customer.CusId = dbo.tbl_rent.CusId INNER JOIN
                  dbo.tbl_movie ON dbo.tbl_rent.MvId = dbo.tbl_movie.MvId
GO
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (14577279, N'AgAg', N'Mr', N'street 1', CAST(N'2024-06-13T14:25:01.457' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (21446386, N'Taylor', N'Ms', N'street 2', CAST(N'2024-06-13T14:25:02.143' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (22295576, N'Toe Toe Lay', N'Mr', N'street 3', CAST(N'2024-06-13T14:25:02.230' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (22357547, N'Yu ya', N'Ms', N'street 4', CAST(N'2024-06-13T14:25:02.237' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (22410366, N'Sandy', N'Ms', N'First Street Plot No 4', CAST(N'2024-06-13T14:25:02.240' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (72125004, N'Taylor', N'Ms', N'street 1', CAST(N'2024-06-13T14:21:17.213' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (95597548, N'AgAg', N'Mr', N'street 1', CAST(N'2024-06-13T11:39:39.560' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (95650092, N'Sandy', N'Ms', N'First Street Plot No 4', CAST(N'2024-06-13T11:39:39.567' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (95760079, N'John', N'Mr', N'Second Street Plot No 5', CAST(N'2024-06-13T11:39:39.577' AS DateTime), NULL)
INSERT [dbo].[tbl_customer] ([CusId], [FullName], [Salutation], [Address], [CreatedAt], [UpdatedAt]) VALUES (95815362, N'Taylor', N'Ms', N'street 1', CAST(N'2024-06-13T11:39:39.580' AS DateTime), NULL)
GO
INSERT [dbo].[tbl_movie] ([MvId], [Title], [ReleaseDate], [CreatedAt]) VALUES (1, N'Daddy''s Little Girls', CAST(N'2020-02-02' AS Date), CAST(N'2020-03-02T00:00:00.000' AS DateTime))
INSERT [dbo].[tbl_movie] ([MvId], [Title], [ReleaseDate], [CreatedAt]) VALUES (2, N'Clash of the Titans', CAST(N'2019-01-01' AS Date), CAST(N'2019-04-05T00:00:00.000' AS DateTime))
INSERT [dbo].[tbl_movie] ([MvId], [Title], [ReleaseDate], [CreatedAt]) VALUES (3, N'Forgetting Sarah Marshi', CAST(N'2013-01-01' AS Date), CAST(N'2021-08-23T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (14577279, 14577279, 1, CAST(N'2024-06-13T14:25:01.683' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (21446386, 21446386, 2, CAST(N'2024-06-13T14:25:02.227' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (22295576, 22295576, 1, CAST(N'2024-06-13T14:25:02.233' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (22357547, 22357547, 2, CAST(N'2024-06-13T14:25:02.240' AS DateTime), NULL)
INSERT [dbo].[tbl_rent] ([RentId], [CusId], [MvId], [RentAt], [ReturnAt]) VALUES (22410366, 22410366, 2, CAST(N'2024-06-13T14:25:02.243' AS DateTime), NULL)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_customer"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_movie"
            Begin Extent = 
               Top = 7
               Left = 290
               Bottom = 170
               Right = 484
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_rent"
            Begin Extent = 
               Top = 7
               Left = 532
               Bottom = 170
               Right = 726
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_cus_mv_rent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_cus_mv_rent'
GO
