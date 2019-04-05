CREATE TABLE [dbo].[stopat]
(
	[TID] SMALLINT NOT NULL , 
    [SID] SMALLINT NOT NULL , 
    [Direction] BIT NOT NULL , 
    [Atime] TIME NOT NULL, 
    [Dtime] TIME NOT NULL, 
	CONSTRAINT [PK_stopat] PRIMARY KEY ([TID], [SID], [Direction]),
    CONSTRAINT [FK_train] FOREIGN KEY ([TID]) REFERENCES [train]([TID]), 
    CONSTRAINT [FK_station] FOREIGN KEY ([SID]) REFERENCES [station]([SID])
)
