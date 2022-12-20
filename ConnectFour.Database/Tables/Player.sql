CREATE TABLE [dbo].[Player]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(4000) NULL, 
    [Num] INT NULL, 
    [RoomId] INT NULL
    CONSTRAINT UC_OnePlayerRoomIdAndNum UNIQUE (RoomId, Num)
    FOREIGN KEY (RoomId) REFERENCES Room(Id)
)
