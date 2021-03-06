USE [TelegramDB]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 7/7/2021 8:35:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FName] [nvarchar](500) COLLATE Persian_100_CI_AS NULL,
	[LName] [nvarchar](500) COLLATE Persian_100_CI_AS NULL,
	[Username] [nvarchar](500) COLLATE Persian_100_CI_AS NULL,
	[Phone] [nvarchar](500) COLLATE Persian_100_CI_AS NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[AddNew]    Script Date: 7/7/2021 8:35:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNew]
       @fname nvarchar(500),
	   @lname nvarchar(500),
       @phone nvarchar(500),
	   @username nvarchar(500)
AS
BEGIN
       SET NOCOUNT ON;
       INSERT INTO Person
              (FName, LName, Username, Phone)
       VALUES
              (@fname, @lname, @username, @phone)
END
GO
