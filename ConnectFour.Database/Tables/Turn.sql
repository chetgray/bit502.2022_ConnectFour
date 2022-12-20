CREATE TABLE [dbo].[Turn]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RoomId] INT NULL, 
    [Time] DATETIME2(0) NULL, 
    [Num] INT NULL, 
    [RowNum] INT NULL, 
    [ColNum] INT NULL
    CONSTRAINT AK_Turn_RoomId_Num UNIQUE (RoomId, Num)
    CONSTRAINT AK_Turn_RoomId_RowNum_ColNum UNIQUE (RoomId, RowNum, ColNum)
    CONSTRAINT FK_Turn_RoomId FOREIGN KEY (RoomId) REFERENCES Room(Id)
)
