﻿CREATE TABLE [dbo].[device]
(
	[DID] TINYINT NOT NULL PRIMARY KEY IDENTITY, 
    [TID] SMALLINT NOT NULL, 
    [Description] NCHAR(100) NULL, 
    CONSTRAINT [FK_device_ToTable] FOREIGN KEY ([TID]) REFERENCES [train]([TID])
)