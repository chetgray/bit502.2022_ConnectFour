CREATE PROCEDURE [dbo].[spA_Player_GetPlayersInRoom]
	@RoomId int
AS
	SELECT  Player.Id				AS PlayerId,
			Player.[Name]			AS PlayerName,
			Player.RoomId			AS PlayerRoomId,
			Player.Num				AS PlayerNum
	FROM	Player 
	WHERE	Player.RoomId = @RoomId
	ORDER BY Player.Num ASC
