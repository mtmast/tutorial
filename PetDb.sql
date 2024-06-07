USE [DbPet]
GO
/****** Object:  Table [dbo].[cats]    Script Date: 6/7/2024 1:10:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cats](
	[id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_cats] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dogs]    Script Date: 6/7/2024 1:10:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dogs](
	[id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_dogs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[cats] ([id], [name]) VALUES (1, N'Hla Hla')
INSERT [dbo].[cats] ([id], [name]) VALUES (2, N'Shwe War')
INSERT [dbo].[cats] ([id], [name]) VALUES (3, N'Aung Aung')
INSERT [dbo].[cats] ([id], [name]) VALUES (4, N'Chit Thu')
INSERT [dbo].[cats] ([id], [name]) VALUES (5, N'Ma Fae War')
INSERT [dbo].[cats] ([id], [name]) VALUES (6, N'R Zar Ni')
INSERT [dbo].[cats] ([id], [name]) VALUES (7, N'Chit Thu')
GO
INSERT [dbo].[dogs] ([id], [name]) VALUES (1, N'Aung Aung')
INSERT [dbo].[dogs] ([id], [name]) VALUES (2, N'Bo Phyu')
INSERT [dbo].[dogs] ([id], [name]) VALUES (3, N'Bo Ni')
INSERT [dbo].[dogs] ([id], [name]) VALUES (4, N'Si Si')
INSERT [dbo].[dogs] ([id], [name]) VALUES (5, N'Min Min')
INSERT [dbo].[dogs] ([id], [name]) VALUES (6, N'Hlaing Gyi')
INSERT [dbo].[dogs] ([id], [name]) VALUES (7, N'Hla Hla')
GO
