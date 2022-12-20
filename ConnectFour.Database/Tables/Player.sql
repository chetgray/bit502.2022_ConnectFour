CREATE TABLE [dbo].[Player]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(4000) NULL, 
    [Num] INT NULL, 
    [RoomId] INT NULL
    CONSTRAINT AK_Player_RoomId_Num UNIQUE (RoomId, Num)
    CONSTRAINT FK_Player_RoomId FOREIGN KEY (RoomId) REFERENCES Room(Id)
)
