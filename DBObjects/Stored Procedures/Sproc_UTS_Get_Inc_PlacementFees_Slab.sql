USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_Get_Inc_PlacementFees_Slab]    Script Date: 20-02-2023 14:15:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sproc_UTS_Get_Inc_PlacementFees_Slab]
@PageIndex				INT = 1,
@PageSize				INT = 50,
@SortExpression			nvarchar(100) = 'ID',
@SortDirection			nvarchar(100) = 'asc'
AS
BEGIN

	Declare @WhereClauseSQL nvarchar(max)=''
	Declare @MainSQL nvarchar(max)=''

	
	IF OBJECT_ID('tempdb..#PlacementFees_Slab') is not null
	DROP TABLE #PlacementFees_Slab

	create table #PlacementFees_Slab
	(
		ID							int primary key identity(1,1),
		PlacementFeesSlab			nvarchar(100),
		SalesConsultant				decimal(9,2) default 0,
		PODManagers					decimal(9,2) default 0,
		BDR							decimal(9,2) default 0,
		BDR_Lead					decimal(9,2) default 0,
		BDRManager_Head				decimal(9,2) default 0,
		MarketingTeam				decimal(9,2) default 0,
		MarketingLead				decimal(9,2) default 0,
		MarketingHead				decimal(9,2) default 0,
		AM							decimal(9,2) default 0,
		AMHead						decimal(9,2) default 0,
		TotalRecords				int default 0
	)


	INSERT INTO #PlacementFees_Slab(PlacementFeesSlab)
	select PS.PlacementFees_Slab from prg_Inc_PlacementFees_Slab PS WITH(NOLOCK)

    Update S
	SET    SalesConsultant = Q.Inc_Percentage 
	from   #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage ,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.id = 1
		)Q On Q.Id = S.ID


	Update S
	SET    PODManagers = Q.Inc_Percentage 
	from   #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage ,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.id = 2
		)Q On Q.Id = S.ID

		 
	Update S
	SET BDR = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 3
	)Q on Q.ID = S.ID

	Update S
	SET BDR_Lead = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 4
	)Q on Q.ID = S.ID

	Update S
	SET BDRManager_Head = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 5
	)Q on Q.ID = S.ID

	Update S
	SET MarketingTeam = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 6
	)Q on Q.ID = S.ID

	Update S
	SET MarketingLead = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 7
	)Q on Q.ID = S.ID

	Update S
	SET MarketingHead = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 8
	)Q on Q.ID = S.ID

	Update S
	SET AM = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 9
	)Q on Q.ID = S.ID

	Update S
	SET AMHead = Q.Inc_Percentage
	from #PlacementFees_Slab S
	inner join(
			select PF.Inc_Percentage,PS.ID
			from usr_UserRole UR WITH(NOLOCK)
			inner join gen_Inc_PlacementFees_Talent_UserRole_Details PF WITH(NOLOCK) on PF.User_Role_ID = UR.ID
			inner join prg_Inc_PlacementFees_Slab PS WITH(NOLOCK) on PS.ID = PF.PlacementFees_Slab_Id
			where UR.ID = 10
	)Q on Q.ID = S.ID


		Declare @Total_Records as nvarchar(max)
		SET @Total_Records ='update #PlacementFees_Slab set TotalRecords = (select count(1) from #PlacementFees_Slab WHERE 1= 1'+@WhereClauseSQL+')'

		Execute sp_executesql @Total_Records

		SET  @MainSQL = 'select * from #PlacementFees_Slab WHERE 1= 1  ' + @WhereClauseSQL + ''

		If	@SortExpression = 'PlacementFeesSlab'
			SET @MainSQL += ' ORDER BY ID ' + @SortDirection
		ELSe 
			SET @MainSQL += ' ORDER BY ' + @SortExpression + ' ' + @SortDirection


		IF(@PageSize>0)
			BEGIN
				SET @MainSQL= 	@MainSQL+ '	OFFSET ' + CONVERT(nvarchar, ((@PageIndex - 1)  * @PageSize)) + ' ROWS 
								FETCH NEXT ' + CONVERT(nvarchar, @PageSize) + ' ROWS ONLY';
			END
		
		PRINT(@MainSQL)
		EXECUTE sp_executesql @MainSQL	

END


	