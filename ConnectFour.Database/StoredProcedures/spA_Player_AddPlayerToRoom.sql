CREATE PROCEDURE [dbo].[spA_Player_AddPlayerToRoom]
	@RoomId int,
	@PlayerNum int,
	@Name nvarchar(4000)
AS
	DECLARE @PlayerId int

	INSERT INTO Player ([Name], Num, RoomId)
	VALUES (@Name, @PlayerNum, @RoomId)
	SET @PlayerId = SCOPE_IDENTITY()

	SELECT	Player.Id		AS PlayerId,
			Player.[Name],
			Player.Num		AS PlayerNum,
			Player.RoomID	AS PlayerRoomId

	FROM Player WHERE Player.Id = @PlayerId