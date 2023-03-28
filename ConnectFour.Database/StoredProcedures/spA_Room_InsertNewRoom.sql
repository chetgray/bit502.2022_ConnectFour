CREATE PROCEDURE [dbo].[spA_Room_InsertNewRoom]
AS
	INSERT INTO Room(CreationTime, CurrentTurnNum)
	VALUES(GETDATE(), 1)
	SELECT SCOPE_IDENTITY()