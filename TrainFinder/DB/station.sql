CREATE TABLE [dbo].[station]
(
	[SID] SMALLINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(20) NOT NULL, 
    [Distance] FLOAT NOT NULL, 
    [Llongitude] FLOAT NOT NULL, 
    [Llatitude] FLOAT NOT NULL, 
    [Address] NCHAR(100) NULL, 
    [Telephone] NCHAR(15) NULL, 
    [RID] SMALLINT NOT NULL, 
    CONSTRAINT [FK_station_ToTable] FOREIGN KEY (RID) REFERENCES [route]([RID])
)

GO

CREATE INDEX [IX_station_Column] ON [dbo].[station] (RID)
