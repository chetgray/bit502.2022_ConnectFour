﻿CREATE TABLE [dbo].[Room]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CreationTime] DATETIME2 NULL, 
    [CurrentTurnNum] INT NULL, 
    [ResultCode] INT NULL
)
