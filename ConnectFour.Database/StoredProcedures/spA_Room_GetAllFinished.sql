CREATE PROCEDURE [dbo].[spA_Room_GetAllFinished]
AS
	SELECT	Room.Id				AS RoomId,
			Room.CreationTime	AS RoomCreationTime,
			Room.ResultCode		AS RoomResultCode,
			Player.Id			AS PlayerId,
			Player.[Name]		AS PlayerName,
			Player.RoomId		AS PlayerRoomId,
			Player.Num			AS PlayerNum,
			Turn.Id				AS TurnId,
			Turn.[Time]			AS TurnTime,
			Turn.RowNum			AS TurnRowNum,
			Turn.ColNum			AS TurnColNum,
			Turn.RoomId			AS TurnRoomId,
			Turn.Num			AS TurnNum
	FROM	Room
	INNER JOIN Player ON Player.RoomId = Room.Id
	INNER JOIN Turn ON Turn.RoomId = Room.Id AND Turn.Num = Room.CurrentTurnNum
	WHERE	Room.ResultCode > 0