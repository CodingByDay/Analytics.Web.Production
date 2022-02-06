USE [master]
GO
/****** Object:  Database [Development]    Script Date: 2/6/2022 9:06:03 AM ******/
CREATE DATABASE [Development]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'petpak', FILENAME = N'D:\MSSQL15.SPLAHOST\MSSQL\DATA\Development.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'petpak_log', FILENAME = N'L:\MSSQL15.SPLAHOST\MSSQL\Log\Development.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Development] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Development].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Development] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Development] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Development] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Development] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Development] SET ARITHABORT OFF 
GO
ALTER DATABASE [Development] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Development] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Development] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Development] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Development] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Development] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Development] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Development] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Development] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Development] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Development] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Development] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Development] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Development] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Development] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Development] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Development] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Development] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Development] SET  MULTI_USER 
GO
ALTER DATABASE [Development] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Development] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Development] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Development] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Development] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Development] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Development] SET QUERY_STORE = OFF
GO
USE [Development]
GO
/****** Object:  User [petpakn]    Script Date: 2/6/2022 9:06:04 AM ******/
CREATE USER [petpakn] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [petpak]    Script Date: 2/6/2022 9:06:04 AM ******/
CREATE USER [petpak] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [jankoj]    Script Date: 2/6/2022 9:06:04 AM ******/
CREATE USER [jankoj] FOR LOGIN [jankoj] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [dashboards]    Script Date: 2/6/2022 9:06:04 AM ******/
CREATE USER [dashboards] FOR LOGIN [dashboards] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [jankoj]
GO
ALTER ROLE [db_owner] ADD MEMBER [dashboards]
GO
/****** Object:  Table [dbo].[companies]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[companies](
	[id_company] [int] NOT NULL,
	[company_name] [char](200) NULL,
	[company_number] [bigint] NULL,
	[website] [char](200) NULL,
	[admin_id] [varchar](15) NULL,
	[databaseName] [varchar](120) NULL,
	[ID] [int] NULL,
	[Designer] [int] NOT NULL,
	[scripted] [int] NOT NULL,
 CONSTRAINT [PK_companies] PRIMARY KEY CLUSTERED 
(
	[id_company] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[company_string]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[company_string](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[string] [char](100) NULL,
 CONSTRAINT [PK_company_string] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dashboards]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dashboards](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Dashboard] [varbinary](max) NULL,
	[Caption] [nvarchar](255) NULL,
	[isViewerOnly] [int] NULL,
	[belongsTo] [varchar](max) NULL,
 CONSTRAINT [PK_Dashboards] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[memberships]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[memberships](
	[id_membership] [int] IDENTITY(1,1) NOT NULL,
	[id_company] [int] NULL,
	[startDate] [datetime] NOT NULL,
	[endDate] [datetime] NOT NULL,
	[id_types_of_memberships] [int] NULL,
	[stripe] [varchar](500) NULL,
 CONSTRAINT [PK_memberships] PRIMARY KEY CLUSTERED 
(
	[id_membership] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[permisions]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[permisions](
	[id_permisions] [int] NOT NULL,
	[NewDashboard] [bit] NOT NULL,
	[Proizvodnjavlanskemletu] [bit] NOT NULL,
	[Naročila] [bit] NOT NULL,
	[Naročila2] [bit] NOT NULL,
	[test1] [bit] NOT NULL,
	[New] [bit] NOT NULL,
	[ModulDENAR] [bit] NOT NULL,
	[Prodaja] [bit] NOT NULL,
	[testquerrybuilder] [bit] NOT NULL,
	[PSProdaja1] [bit] NOT NULL,
	[ProdajaTEST] [bit] NOT NULL,
	[PSProdaja2] [bit] NOT NULL,
	[PSHR1] [bit] NOT NULL,
	[PSHR2] [bit] NOT NULL,
 CONSTRAINT [PK_permisions] PRIMARY KEY CLUSTERED 
(
	[id_permisions] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[permisions_user]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[permisions_user](
	[id_permisions_user] [int] NOT NULL,
	[PSProdaja1] [int] NOT NULL,
	[PSProdaja2] [int] NOT NULL,
	[PSHR1] [int] NOT NULL,
	[PSHR2] [int] NOT NULL,
	[DBMNGMNreport1] [int] NOT NULL,
	[DBMNGMNreport2] [int] NOT NULL,
	[PSReklamacije] [int] NOT NULL,
	[PSReklamacije1] [int] NOT NULL,
	[PSReklamacije2] [int] NOT NULL,
	[TESTProdaja] [int] NOT NULL,
	[NewDashboard] [int] NOT NULL,
	[DBProdaja1] [int] NOT NULL,
	[FIS] [int] NOT NULL,
	[DBForcast1] [int] NOT NULL,
	[DBHR1] [int] NOT NULL,
	[DBHR2] [int] NOT NULL,
	[DBIzdobavaNABAVA] [int] NOT NULL,
	[DBIzdobavaPRODAJA] [int] NOT NULL,
	[DBMNGMNreport3] [int] NOT NULL,
	[DBOdmikicenNABAVA1] [int] NOT NULL,
	[DBOdmikicenPRODAJA2] [int] NOT NULL,
	[DBProdaja2] [int] NOT NULL,
	[DBProdaja3] [int] NOT NULL,
	[DBProizvodnjaDN] [int] NOT NULL,
	[DBZaloga1] [int] NOT NULL,
	[DBZaloga2] [int] NOT NULL,
	[aaa] [int] NOT NULL,
	[testdynamic] [int] NOT NULL,
	[NewDashboardProdaja] [int] NOT NULL,
	[DBProdajaTCMotoShop1] [int] NOT NULL,
	[DBProdaja1YTD] [int] NOT NULL,
	[DBProdaja2YTD] [int] NOT NULL,
	[DBProdaja3YTD] [int] NOT NULL,
	[DBProdajaTCMotoShop1Champion] [int] NOT NULL,
	[DBProdajaTCMotoShop1MotoShop] [int] NOT NULL,
	[DBProdaja1OdDo] [int] NOT NULL,
	[DBProdaja2OdDo] [int] NOT NULL,
	[DBProdaja3OdDo] [int] NOT NULL,
	[DBProdaja4OdDo] [int] NOT NULL,
	[DBNabava1] [int] NOT NULL,
	[DBProdaja1OdDoTEST] [int] NOT NULL,
 CONSTRAINT [PK_permisions_user] PRIMARY KEY CLUSTERED 
(
	[id_permisions_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblResetPasswordRequests]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblResetPasswordRequests](
	[id] [uniqueidentifier] NOT NULL,
	[username] [varchar](15) NULL,
	[ResetRequestDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[types_of_memberships]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[types_of_memberships](
	[id_types_of_memberships] [int] IDENTITY(1,1) NOT NULL,
	[name] [char](200) NULL,
	[price] [bigint] NULL,
	[oneTimePurchase] [int] NULL,
	[duration] [int] NULL,
 CONSTRAINT [PK_types_of_memberships] PRIMARY KEY CLUSTERED 
(
	[id_types_of_memberships] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[uname] [varchar](15) NOT NULL,
	[Pwd] [varchar](500) NULL,
	[userRole] [varchar](25) NOT NULL,
	[id_permisions] [int] NULL,
	[id_company] [int] NULL,
	[ViewState] [varchar](120) NULL,
	[FullName] [varchar](120) NULL,
	[email] [varchar](150) NULL,
	[id_permision_user] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY NONCLUSTERED 
(
	[uname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship3]    Script Date: 2/6/2022 9:06:04 AM ******/
CREATE NONCLUSTERED INDEX [IX_Relationship3] ON [dbo].[memberships]
(
	[id_company] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Relationship6]    Script Date: 2/6/2022 9:06:04 AM ******/
CREATE NONCLUSTERED INDEX [IX_Relationship6] ON [dbo].[memberships]
(
	[id_types_of_memberships] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[companies] ADD  DEFAULT ((1)) FOR [Designer]
GO
ALTER TABLE [dbo].[companies] ADD  DEFAULT ((0)) FOR [scripted]
GO
ALTER TABLE [dbo].[Dashboards] ADD  DEFAULT ('0') FOR [isViewerOnly]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [NewDashboard]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [Proizvodnjavlanskemletu]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [Naročila]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [Naročila2]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [test1]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [New]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [ModulDENAR]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [Prodaja]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [testquerrybuilder]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [PSProdaja1]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [ProdajaTEST]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [PSProdaja2]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [PSHR1]
GO
ALTER TABLE [dbo].[permisions] ADD  DEFAULT ((0)) FOR [PSHR2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  CONSTRAINT [DF_permisions_user_PSProdaja1]  DEFAULT ((0)) FOR [PSProdaja1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  CONSTRAINT [DF_permisions_user_PSProdaja2]  DEFAULT ((0)) FOR [PSProdaja2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  CONSTRAINT [DF_permisions_user_PSHR1]  DEFAULT ((0)) FOR [PSHR1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  CONSTRAINT [DF_permisions_user_PSHR2]  DEFAULT ((0)) FOR [PSHR2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBMNGMNreport1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBMNGMNreport2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [PSReklamacije]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [PSReklamacije1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [PSReklamacije2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [TESTProdaja]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [NewDashboard]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [FIS]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBForcast1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBHR1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBHR2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBIzdobavaNABAVA]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBIzdobavaPRODAJA]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBMNGMNreport3]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBOdmikicenNABAVA1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBOdmikicenPRODAJA2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja3]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProizvodnjaDN]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBZaloga1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBZaloga2]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [aaa]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [testdynamic]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [NewDashboardProdaja]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdajaTCMotoShop1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja1YTD]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja2YTD]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja3YTD]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdajaTCMotoShop1Champion]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdajaTCMotoShop1MotoShop]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja1OdDo]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja2OdDo]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja3OdDo]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja4OdDo]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBNabava1]
GO
ALTER TABLE [dbo].[permisions_user] ADD  DEFAULT ((0)) FOR [DBProdaja1OdDoTEST]
GO
ALTER TABLE [dbo].[types_of_memberships] ADD  DEFAULT ((0)) FOR [oneTimePurchase]
GO
ALTER TABLE [dbo].[companies]  WITH CHECK ADD FOREIGN KEY([admin_id])
REFERENCES [dbo].[Users] ([uname])
GO
ALTER TABLE [dbo].[companies]  WITH CHECK ADD  CONSTRAINT [String relationship] FOREIGN KEY([ID])
REFERENCES [dbo].[company_string] ([ID])
GO
ALTER TABLE [dbo].[companies] CHECK CONSTRAINT [String relationship]
GO
ALTER TABLE [dbo].[memberships]  WITH CHECK ADD  CONSTRAINT [Relationship3] FOREIGN KEY([id_company])
REFERENCES [dbo].[companies] ([id_company])
GO
ALTER TABLE [dbo].[memberships] CHECK CONSTRAINT [Relationship3]
GO
ALTER TABLE [dbo].[memberships]  WITH CHECK ADD  CONSTRAINT [Relationship6] FOREIGN KEY([id_types_of_memberships])
REFERENCES [dbo].[types_of_memberships] ([id_types_of_memberships])
GO
ALTER TABLE [dbo].[memberships] CHECK CONSTRAINT [Relationship6]
GO
ALTER TABLE [dbo].[tblResetPasswordRequests]  WITH CHECK ADD FOREIGN KEY([username])
REFERENCES [dbo].[Users] ([uname])
GO
/****** Object:  StoredProcedure [dbo].[spChangePassword]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[spChangePassword]
@GUID uniqueidentifier,
@Password nvarchar(100)
as
Begin
 Declare @UserId varchar(15)
 
 Select @UserId = username 
 from tblResetPasswordRequests
 where Id= @GUID
 
 if(@UserId is null)
 Begin
  -- If UserId does not exist
  Select 0 as IsPasswordChanged
 End
 Else
 Begin
  -- If UserId exists, Update with new password
  Update Users set
  [Pwd] = @Password
  where uname = @UserId
  
  -- Delete the password reset request row 
  Delete from tblResetPasswordRequests
  where Id = @GUID
  
  Select 1 as IsPasswordChanged
 End
End
GO
/****** Object:  StoredProcedure [dbo].[spIsPasswordResetLinkValid]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[spIsPasswordResetLinkValid] 
@GUID uniqueidentifier
as
Begin
 Declare @UserId int
 
 If(Exists(Select username from tblResetPasswordRequests where Id = @GUID))
 Begin
  Select 1 as IsValidPasswordResetLink
 End
 Else
 Begin
  Select 0 as IsValidPasswordResetLink
 End
End
GO
/****** Object:  StoredProcedure [dbo].[spResetPassword]    Script Date: 2/6/2022 9:06:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[spResetPassword]
@UserName nvarchar(100)
as
Begin
 Declare @user varchar(15)
 Declare @Email nvarchar(100)
 
 Select @user = uname, @Email = Email 
 from Users
 where uname = @UserName
 
 if(@user IS NOT NULL)
 Begin
  --If username exists
  Declare @GUID UniqueIdentifier
  Set @GUID = NEWID()
  
  Insert into tblResetPasswordRequests
  (Id, username, ResetRequestDateTime)
  Values(@GUID, @user, GETDATE())
  
  Select 1 as ReturnCode, @GUID as UniqueId, @Email as Email
 End
 Else
 Begin
  --If username does not exist
  SELECT 0 as ReturnCode, NULL as UniqueId, NULL as Email
 End
End
GO
USE [master]
GO
ALTER DATABASE [Development] SET  READ_WRITE 
GO
