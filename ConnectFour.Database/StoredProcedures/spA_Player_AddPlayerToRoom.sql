CREATE PROCEDURE [dbo].[spA_Player_AddPlayerToRoom]
	@RoomId int,
	@PlayerNum int,
	@PlayerName nvarchar(4000)
AS
	DECLARE @PlayerId int

	INSERT INTO Player ([Name], Num, RoomId)
	VALUES (@PlayerName, @PlayerNum, @RoomId)
	SET @PlayerId = SCOPE_IDENTITY()

	SELECT	Player.Id		AS PlayerId,
			Player.[Name]   AS PlayerName,
			Player.Num		AS PlayerNum,
			Player.RoomId	AS PlayerRoomId

	FROM Player WHERE Player.Id = @PlayerId
