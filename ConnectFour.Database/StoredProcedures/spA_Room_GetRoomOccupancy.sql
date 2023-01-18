CREATE PROCEDURE [dbo].[spA_Room_GetRoomOccupancy]
	@RoomId int
AS
	SELECT	Room.Id					AS RoomId,
			Room.CreationTime		AS RoomCreationTime,
			Room.CurrentTurnNum		AS RoomCurrentTurnNum,
			Room.ResultCode			AS RoomResultCode,
			Player.Id				AS PlayerId,
			Player.[Name]			AS PlayerName,
			Player.RoomId			AS PlayerRoomId,
			Player.Num				AS PlayerNum
	FROM	Room
	INNER JOIN Player ON Player.RoomId = @RoomId
	WHERE	Room.ResultCode IS NULL AND Room.Id = @RoomId
