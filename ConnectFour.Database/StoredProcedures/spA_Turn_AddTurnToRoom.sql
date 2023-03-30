CREATE PROCEDURE [dbo].[spA_Turn_AddTurnToRoom]
           @TurnRoomId int,
           @TurnTime datetime2(0),
           @TurnRowNum int,
           @TurnColNum int,
           @TurnNum int
AS
	INSERT INTO dbo.Turn (RoomId, Time, RowNum, ColNum, Num)
    VALUES (@TurnRoomId, @TurnTime, @TurnRowNum, @TurnColNum,@TurnNum)

    UPDATE dbo.[Room]
    SET dbo.[Room].CurrentTurnNum = @TurnNum
    WHERE dbo.[Room].Id = @TurnRoomId
    SELECT SCOPE_IDENTITY()