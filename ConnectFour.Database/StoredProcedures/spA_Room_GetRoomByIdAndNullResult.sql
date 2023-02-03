CREATE PROCEDURE [dbo].[spA_Room_GetRoomByIdAndNullResult]
	@RoomId int
AS
	SELECT	Room.Id					AS RoomId,
			Room.CreationTime		AS RoomCreationTime,
			Room.CurrentTurnNum		AS RoomCurrentTurnNum,
			Room.ResultCode			AS RoomResultCode
	FROM    Room
	WHERE	Room.ResultCode IS NULL AND Room.Id = @RoomId
