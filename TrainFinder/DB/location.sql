CREATE TABLE [dbo].[location]
(
	[DID] TINYINT NOT NULL , 
    [Datetime] DATETIME NOT NULL , 
    [Locationdata] XML NULL, 
    CONSTRAINT [PK_location] PRIMARY KEY ([DID], [Datetime]), 
    CONSTRAINT [FK_location_ToTable] FOREIGN KEY ([DID]) REFERENCES [device]([DID])
)
