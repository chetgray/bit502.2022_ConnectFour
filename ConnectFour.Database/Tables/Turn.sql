CREATE TABLE [dbo].[Turn]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RoomId] INT NULL, 
    [Time] DATETIME2(0) NULL, 
    [Num] INT NULL, 
    [RowNum] INT NULL, 
    [ColNum] INT NULL
    CONSTRAINT UC_OneTurnNumAndRoomId UNIQUE (RoomId, Num)
    CONSTRAINT UC_RoomAndVectors UNIQUE (RoomId, RowNum, ColNum)
    FOREIGN KEY (RoomId) REFERENCES Room(Id)
)
