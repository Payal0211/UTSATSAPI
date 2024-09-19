
ALTER PROCEDURE [dbo].[sproc_UTS_GetTalentProfileLog]
@TalentID	BIGINT = NULL,
@fromDate   NVARCHAR(100) = NULL,
@toDate	    NVARCHAR(100) = NULL
AS
BEGIN

           DECLARE @WhereClauseForDateTime nvarchar(max) = '';

		   IF @fromDate is not null and @toDate is not null and ISNULL(@fromDate,'') <> '' and ISNULL(@toDate,'') <> '' 
		   BEGIN
		   if @fromDate = @toDate
		   begin 
		   		SET @WhereClauseForDateTime += ' AND cast(His.CreatedByDatetime as date) = '+ '''' + @fromDate +'''';
		   end
		   else
		   begin
		   		SET @WhereClauseForDateTime += ' AND cast(His.CreatedByDatetime as date) >= '+ '''' + @fromDate +'''' +' AND cast(His.CreatedByDatetime as date) <= '+ ''''+ @toDate +'''';
		   end
		   END

	IF OBJECT_ID('temp_db..#TalentProfileLog') IS NOT NULL
		DROP TABLE #TalentProfileLog;

	CREATE TABLE #TalentProfileLog
	(
		TalentID			BIGINT,
		TalentName			NVARCHAR(250),
		TalentRole			NVARCHAR(200),
		ProfileSharedCount	INT,
		FeedbackCount		INT,
		RejectedCount		INT,
		SelectedForCount	INT
	)

	INSERT INTO #TalentProfileLog(TalentID, TalentRole, TalentName)
	SELECT	@TalentID,
			ISNULL(TR.TalentRole,'') as TalentRole,
			ISNULL(T.Name,'') as TalentName
	FROM	gen_Talent T with(nolock)
			INNER JOIN prg_TalentRoles TR with(nolock) on T.RoleID = TR.ID
	WHERE	T.ID = @TalentID;

	begin	--Profile Shared Count
		--Action ID = 6
		DECLARE @MainSQL1 nvarchar(max) = 'UPDATE	X
		SET		X.ProfileSharedCount = Y.TotalCount
		FROM	#TalentProfileLog X
				INNER JOIN 
				(
					SELECT	ISNULL(SUM(T.TotalCount),0) as TotalCount FROM
					(
						
						SELECT		COUNT(His.ID) TotalCount,His.HiringRequest_ID,His.Talent_ID 
						FROM		gen_History His with(nolock) 
									INNER JOIN gen_SalesHiringRequest H with(nolock) on His.HiringRequest_ID = H.ID
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
						WHERE		H.IsActive = 1 and H.IsAccepted = 1 and His.Action_ID = 6 and His.Talent_ID = ' + cast(@TalentID as varchar) + @WhereClauseForDateTime + ' 
						GROUP BY	His.HiringRequest_ID,His.Talent_ID
					) T
		) Y on 1=1';
		EXEC (@MainSQL1)
	end

	begin	--FeedbackCount
		--Action ID = 51 and History Table IsActive = 1
		DECLARE @MainSQL2 nvarchar(max) = 'UPDATE	X
		SET		X.FeedbackCount = Y.TotalCount
		FROM	#TalentProfileLog X
				INNER JOIN 
				(
					SELECT	ISNULL(SUM(T.TotalCount),0) as TotalCount FROM
					(
						SELECT		COUNT(His.ID) TotalCount,His.HiringRequest_ID,His.Talent_ID 
						FROM		gen_History His with(nolock) 
									INNER JOIN gen_SalesHiringRequest H with(nolock) on His.HiringRequest_ID = H.ID
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
						WHERE		H.IsActive = 1 and H.IsAccepted = 1 and His.Action_ID = 51 and His.Talent_ID = ' + cast(@TalentID as varchar) + ' and His.IsActive = 1 ' + @WhereClauseForDateTime +  ' 
						GROUP BY	His.HiringRequest_ID,His.Talent_ID
					) T
				) Y on 1=1';
				
				EXEC (@MainSQL2)
	end

	begin	--RejectedCount
		--Action ID = 22
		DECLARE @MainSQL3 nvarchar(max) = 'UPDATE	X
		SET		X.RejectedCount = Y.TotalCount
		FROM	#TalentProfileLog X
				INNER JOIN 
				(
					SELECT	ISNULL(SUM(T.TotalCount),0) as TotalCount FROM
					(
						SELECT		COUNT(His.ID) TotalCount,His.HiringRequest_ID,His.Talent_ID 
						FROM		gen_History His with(nolock) 
									INNER JOIN gen_SalesHiringRequest H with(nolock) on His.HiringRequest_ID = H.ID
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
						WHERE		H.IsActive = 1 and H.IsAccepted = 1 and His.Action_ID = 22 and His.Talent_ID =  ' + cast(@TalentID as varchar) + @WhereClauseForDateTime +  '
						GROUP BY	His.HiringRequest_ID,His.Talent_ID 
					) T
				) Y on 1=1';
				
				EXEC (@MainSQL3)
	end

	begin	--SelectedForCount
		--Action ID = 12
		DECLARE @MainSQL4 nvarchar(max) ='UPDATE	X
		SET		X.SelectedForCount = Y.TotalCount
		FROM	#TalentProfileLog X
				INNER JOIN 
				(
					SELECT	ISNULL(SUM(T.TotalCount),0) as TotalCount FROM
					(
						SELECT		COUNT(His.ID) TotalCount,His.HiringRequest_ID,His.Talent_ID 
						FROM		gen_History His with(nolock) 
									INNER JOIN gen_SalesHiringRequest H with(nolock) on His.HiringRequest_ID = H.ID
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
						WHERE		H.IsActive = 1 and H.IsAccepted = 1 and His.Action_ID = 12 and His.Talent_ID = ' + cast(@TalentID as varchar)  + @WhereClauseForDateTime +  '
						GROUP BY	His.HiringRequest_ID,His.Talent_ID
					) T
				) Y on 1=1';
				EXEC (@MainSQL4)
	end


	select 
	
	ISNULL(TalentID,0) as TalentID,
	ISNULL(TalentName,'') as TalentName,
	ISNULL(TalentRole,'') as TalentRole,
	ISNULL(ProfileSharedCount,0) as ProfileSharedCount,
	ISNULL(FeedbackCount,0) as FeedbackCount,
	ISNULL(RejectedCount,0) as RejectedCount,
	ISNULL(SelectedForCount,0) as SelectedForCount

	 from #TalentProfileLog;
END
