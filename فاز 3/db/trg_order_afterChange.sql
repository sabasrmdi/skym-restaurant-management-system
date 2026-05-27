USE [SkymResturant]
GO

/****** Object:  Trigger [dbo].[trg_Order_AfterChange]    Script Date: 6/18/2024 6:10:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[trg_Order_AfterChange]
ON [dbo].[Order]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @username VARCHAR(200) = SUSER_SNAME();

    -- Insert log entry for DELETE action
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- This is to handle DELETEs only, ensure this is not a part of an UPDATE
        IF NOT EXISTS (SELECT * FROM inserted WHERE [id] IN (SELECT [id] FROM deleted))
        BEGIN
            INSERT INTO [dbo].[OrderLog] ([order_id], [username], [type], [happeningDateTime], [description])
            SELECT [id], @username, 2, GETDATE(), null
            FROM deleted;
        END
    END

    -- Insert log entry for INSERT action
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- This is to handle INSERTs only, ensure this is not a part of an UPDATE
        IF NOT EXISTS (SELECT * FROM deleted WHERE [id] IN (SELECT [id] FROM inserted))
        BEGIN
            INSERT INTO [dbo].[OrderLog] ([order_id], [username], [type], [happeningDateTime], [description])
            SELECT [id], @username, 1, GETDATE(), null
            FROM inserted;
        END
    END

    -- Insert log entry for UPDATE action
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO [dbo].[OrderLog] ([order_id], [username], [type], [happeningDateTime], [description])
        SELECT i.[id], @username, 3, GETDATE(),null
        FROM inserted i
        INNER JOIN deleted d ON i.[id] = d.[id];
    END
END

GO

ALTER TABLE [dbo].[Order] ENABLE TRIGGER [trg_Order_AfterChange]
GO


