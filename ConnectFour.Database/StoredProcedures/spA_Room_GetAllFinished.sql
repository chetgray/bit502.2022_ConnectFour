CREATE PROCEDURE [dbo].[spA_Room_GetAllFinished]
AS
	SELECT	Room.Id				AS RoomId,
			Room.CreationTime,
			Room.ResultCode,
			Player.Id			AS PlayerId,
			Player.[Name],
			Player.RoomId		AS PlayerRoomId,
			Player.Num			AS PlayerNum,
			Turn.Id				AS TurnId,
			Turn.[Time],
			Turn.RowNum,
			Turn.ColNum,
			Turn.RoomId			AS TurnRoomId,
			Turn.Num			AS TurnNum
	FROM	Room
	INNER JOIN Player ON Player.RoomId = Room.Id
	INNER JOIN Turn ON Turn.RoomId = Room.Id AND Turn.Num = Room.CurrentTurnNum
	WHERE	Room.ResultCode > 0