CREATE PROCEDURE [dbo].[spA_Turn_GetLastTurnInRoom]
	@RoomId int
AS
	DECLARE @LastTurnInRoomNum int
	SET @LastTurnInRoomNum = (SELECT Room.CurrentTurnNum FROM Room WHERE Room.Id = @RoomId)

	SELECT  t.Id			AS TurnId,
			t.RoomId		AS TurnRoomId,
			t.Time			AS TurnTime,
			t.Num			AS TurnNum,
			t.RowNum		AS TurnRowNum,
			t.ColNum		AS TurnColNum
	FROM	dbo.[Turn] t
	WHERE	t.Num = @LastTurnInRoomNum AND RoomId = @RoomId