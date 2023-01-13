CREATE PROCEDURE [dbo].[spA_Room_GetRoomOccupancy]
	@RoomId int
AS
	SELECT	Room.Id				AS RoomId,
			Room.CreationTime,
			Room.CurrentTurnNum,
			Room.ResultCode,
			Player.Id			AS PlayerId,
			Player.[Name],
			Player.RoomId		AS PlayerRoomId,
			Player.Num			AS PlayerNum
	FROM	Room
	INNER JOIN Player ON Player.RoomId = @RoomId
	WHERE	Room.ResultCode IS NULL AND Room.Id = @RoomId
