USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[sproc_UTS_GetFilterTypeForDeals]    Script Date: 15-03-2023 13:10:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[sproc_UTS_GetFilterTypeForDeals]
@types as varchar(50)
as
IF @types like '%,%'
Begin
select urs.FullName,urs.ID from usr_UserRoleDetails urd
inner join usr_UserRole ur WITH(NOLOCK) on ur.ID = urd.UserRole_ID
inner join usr_User urs WITH(NOLOCK) on urs.ID = urd.User_ID
where Charindex(','+cast(urd.UserRole_ID as varchar(8000))+',', @types) > 0
end 
else 
begin
 select urs.FullName,urs.ID from usr_UserRoleDetails urd
inner join usr_UserRole ur WITH(NOLOCK) on ur.ID = urd.UserRole_ID
inner join usr_User urs WITH(NOLOCK) on urs.ID = urd.User_ID
where urd.UserRole_ID = CAST(@types as int)
end 

