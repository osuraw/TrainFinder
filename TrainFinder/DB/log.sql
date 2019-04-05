CREATE TABLE [dbo].[log]
(
	[TID] SMALLINT NOT NULL , 
    [Datetime] DATETIME NOT NULL , 
    [Stime] TIME NULL, 
    [Etime] TIME NULL, 
    [Maxspeed] FLOAT NULL, 
    [Avgspeed] FLOAT NULL, 
    [Delay] TIME NULL, 
	CONSTRAINT [PK_log] PRIMARY KEY ([TID], [Datetime]),
    CONSTRAINT [FK_log_ToTable] FOREIGN KEY ([TID]) REFERENCES [train]([TID])
)
