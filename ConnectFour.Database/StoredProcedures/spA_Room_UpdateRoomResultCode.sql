CREATE PROCEDURE [dbo].[spA_Room_UpdateRoomResultCode]
           @RoomId int,
           @RoomResultCode int

AS

    UPDATE dbo.[Room]
    SET dbo.[Room].ResultCode = @RoomResultCode
    WHERE dbo.[Room].Id = @RoomId