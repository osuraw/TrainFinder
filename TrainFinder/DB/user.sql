CREATE TABLE [dbo].[user]
(
	[UID] TINYINT NOT NULL PRIMARY KEY IDENTITY , 
    [Name] NCHAR(30) NULL, 
    [Uname] NCHAR(20) NULL unique, 
    [Password] NCHAR(20) NULL
)
