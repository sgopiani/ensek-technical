CREATE TABLE [dbo].[Accounts](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL, 
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([AccountId])
) ON [PRIMARY]
GO

