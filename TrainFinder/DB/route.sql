CREATE TABLE [dbo].[route] (
    [RID]      SMALLINT       NOT NULL IDENTITY,
    [Sstation] SMALLINT       NULL,
    [Estation] SMALLINT       NULL,
    [Distance] FLOAT (53)     NULL,
    [Name]     NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([RID] ASC) 
);


