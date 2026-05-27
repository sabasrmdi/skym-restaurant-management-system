USE [SkymResturant]
GO

/****** Object:  Trigger [dbo].[update_menu_count]    Script Date: 6/18/2024 6:11:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create trigger [dbo].[update_menu_count]
on [dbo].[OrderMenu]
AFTER INSERT 
AS 
BEGIN 
	declare @menu_id int;
	declare @count int;
	select @menu_id=menu_id,@count=[count] from inserted;
	UPDATE Menu
	set Menu.[count]=Menu.[count]-@count
	where Menu.id=@menu_id
END;
GO

ALTER TABLE [dbo].[OrderMenu] ENABLE TRIGGER [update_menu_count]
GO


