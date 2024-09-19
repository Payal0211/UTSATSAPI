USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[sproc_UTS_GetHiringInterview]    Script Date: 20-01-2023 12:42:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[sproc_UTS_GetHiringInterview]
	@PageIndex				INT = 1,
	@PageSize				INT = 50,
	@SortExpression			nvarchar(100) = 'CreatedByDatetime',
	@SortDirection			nvarchar(100) = 'desc',
	@Contact				varchar(200) = NULL,
	@Company                varchar(500) = NULL,
	@TalentName				varchaR(100) = NULL,
	@HRNumber				varchar(100) = NULL
AS
BEGIN
	
		DECLARE @WhereClauseSQL nvarchar(max) = ''
		DECLARE @MainSQL nvarchar(max) = ''
		DECLARE @TmpDBSQL nvarchar(max) = ''
		DECLARE @SelectMainSQL nvarchar(max) = ''
		DECLARE @ExcMainSQL nvarchar(max) = ''
		SET @WhereClauseSQL=' '


		IF isnull(@Contact,'') <> ''
			SET @WhereClauseSQL += ' AND C.Fullname LIKE ''%' + CONVERT(nvarchar,@Contact) + '%'''
		IF isnull(@Company,'') <> ''
			SET @WhereClauseSQL += ' AND CO.Company LIKE ''%' + CONVERT(nvarchar,@Company) + '%'''
		IF isnull(@TalentName,'') <> ''
			SET @WhereClauseSQL += ' AND T.Name LIKE ''%' + CONVERT(nvarchar,@TalentName) + '%'''
		IF isnull(@HRNumber,'') <> ''
			SET @WhereClauseSQL += ' AND H.HR_Number LIKE ''%' + CONVERT(nvarchar,@HRNumber) + '%'''

		SET @MainSQL= ';WITH CTE_Records AS (Select IntM.ID ,SelectedInterviewId = isnull(TI.ID,0),IntM.CreatedByDatetime
,InterviewDateTime = isnull((STUFF(( select char(10) + (CONVERT(varchar, ISNULL(S.ScheduledOn, '' ''), 105) + '' ''+ convert(VARCHAR(5),S.Interview_StartTime, 14)) +'' To '' +(convert(VARCHAR (5),S.Interview_EndTime, 14)) from gen_ShortlistedTalent_InterviewDetails S WITH(NOLOCK) WHERE S.InterviewMaster_ID = IntM.ID For XML Path('''')),1,1,'''')),'''')
,[ISTInterviewDateTime] = isnull((STUFF((select char(10) + (CONVERT(varchar, ISNULL(S.IST_ScheduledOn, '' ''), 105) +'' '' + convert(VARCHAR(5),S.IST_Interview_StartTime, 14)) +'' To '' +(convert(VARCHAR(5),S.IST_Interview_EndTime, 14)) from gen_ShortlistedTalent_InterviewDetails S WITH(NOLOCK) WHERE S.HiringRequest_ID = H.ID AND S.InterviewMaster_ID = IntM.ID For XML Path('''')),1,1,'''')),'''')
,Slotconfirmed = (select CONVERT(varchar, ISNULL(s.ScheduledOn, '' ''), 105) + '' ''  + convert(VARCHAR(5),s.Interview_StartTime, 14) +'' To '' + convert(VARCHAR(5),s.Interview_EndTime, 14) from gen_ShortlistedTalent_InterviewDetails S inner join gen_TalentSelected_InterviewDetails I WITH(NOLOCK) ON S.ID = I.Shortlisted_InterviewID where isnull(I.Zoom_InterviewLink,'''') != '''' AND I.ID =TI.ID)
,ISTSlotconfirmed = (select CONVERT(varchar, ISNULL(s.IST_ScheduledOn, '' ''), 105) + '' ''  + convert(VARCHAR(5),s.IST_Interview_StartTime, 14) +'' To '' + convert(VARCHAR(5),s.IST_Interview_EndTime, 14) from gen_ShortlistedTalent_InterviewDetails S WITH
(NOLOCK) inner join gen_TalentSelected_InterviewDetails I WITH(NOLOCK) ON S.ID = I.Shortlisted_InterviewID where isnull(I.Zoom_InterviewLink,'''') != '''' AND I.ID =TI.ID)
 , Shortlisted_InterviewID =isnull(TI.Shortlisted_InterviewID,0),HiringRequest_Detail_ID = isnull(IntM.HiringRequest_Detail_ID,0), ContactID = isnull(IntM.ContactID,0), HiringRequest_ID = isnull(IntM.HiringRequest_ID,0),HRID = H.HR_Number,ClientName = C.FullName,Companyname = CO.Company,TalentName = T.Name,Priority ='''',InterviewROUND =IntM.InterviewRound_Str,ClientFeedback = (select top 1 CF.FeedBack_Type from gen_ContactInterviewFeedback CF with(nolock) where  CF.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND CF.ContactID =  TI.ContactID),TalentFeedback = case when isnull(IsFeedbackSubmitted,0) =1 then ''Submitted'' else '''' end,FinalCall='''',InterviewStatus = INS.InterviewStatus,
 case
 when (INS.ID = 1) then 501
 when (INS.ID = 2) then 502
 when (INS.ID = 3) then 503
 when (INS.ID = 4) then 504
 when (INS.ID = 5) then 505
 when (INS.ID = 6) then 506
 when (INS.ID = 7) then 507
 when (INS.ID = 8) then 508 else 999 end as InterviewStatusFrontCode
 ,InterviewDuration = (select top 1 replace(cast(DurationInHours AS nvarchar(10)),''.00'','''') from gen_ShortlistedTalent_InterviewDetails S where S.ID = TI.Shortlisted_InterviewID), InterviewTimeZone = CASE WHEN (isnull(CTZ.ShortName,'''')) = '''' THEN 
(
	SELECT top 1 isnull(CTZI.ShortName,'''') as ShortName from gen_ShortlistedTalent_InterviewDetails S WITH(NOLOCK) 
	INNER join prg_ContactTimeZone CTZI WITH(NOLOCK) ON CTZI.ID = S.TimeZone_ID 
	where S.HiringRequest_ID = H.ID and S.Talent_ID = T.ID and S.ContactID = C.ID
) ELSE CTZ.ShortName END ,IsReschedule = cast(case when H.Status_ID =2 AND TI.Status_ID in(1,3,4) then 1  else 0 end as int),Talent_ID =isnull(IntM.Talent_ID,0)
,1 as TotalCountID ,ClientFeedbackId = isnull((select top 1 CF.ID from gen_ContactInterviewFeedback CF with(nolock) where  CF.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND CF.ContactID =  TI.ContactID),0),SlotGivenStatus = isnull((select top 1 SlotGiven from gen_TalentSelected_NextRound_InterviewDetails N WITH(NOLOCK) where N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID),''''),IsLaterSlotGiven = isnull((select top 1 IsLaterSlotGiven from gen_TalentSelected_NextRound_InterviewDetails N WITH(NOLOCK) where N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID),0),NextRound_InterviewDetailsID = isnull((select top 1 ID from gen_TalentSelected_NextRound_InterviewDetails N WITH(NOLOCK) where N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID),0) ,NextroundShortlistedId = isnull((select top 1 Shortlisted_InterviewID from gen_TalentSelected_NextRound_InterviewDetails N WITH(NOLOCK) where N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID),0)'
SET @SelectMainSQL =' from gen_InterviewSlotsMaster IntM WITH(NOLOCK) INNER JOIN gen_SalesHiringRequest H WITH(NOLOCK) ON H.ID = IntM.HiringRequest_ID INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID INNER JOIN gen_Talent T WITH(NOLOCK) ON T.ID = IntM.Talent_ID 
 inner join prg_InterviewStatus INS WITH(NOLOCK) ON INS.ID = IntM.InterviewStatus_ID
 LEFT JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.InterviewMaster_ID=IntM.ID AND TI.HiringRequest_ID = IntM.HiringRequest_ID AND TI.HiringRequest_Detail_ID = IntM.HiringRequest_Detail_ID AND TI.Talent_ID = IntM.Talent_ID
 LEFT join prg_ContactTimeZone CTZ WITH(NOLOCK) ON CTZ.ID = TI.Contact_TimeZone_ID WHERE 1=1   ' + @WhereClauseSQL + ' )
							,Cte_TotalCount AS(
									Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
							)
							SELECT	
							ID,
							--U.SelectedInterviewId,
							--U.InterviewDateTime,	
							--U.ISTInterviewDateTime,
							case when U.CreatedByDatetime is null then null else (CONVERT(varchar, ISNULL(U.CreatedByDatetime, ''''), 3) ) end as iDate,							
							HRID,
							--SlotConfirmed,
							ISNULL(ISTSlotconfirmed,'''') as ISTSlotconfirmed,
							Companyname,
							InterviewTimeZone,
							TalentName,
							--ClientName,	
							InterviewStatus,
							InterviewStatusFrontCode,
							ISNULL(ClientFeedback,'''') as ClientStatus,
							case 
							when ISNULL(ClientFeedback,'''') = ''AnotherRound'' then 601
							when ISNULL(ClientFeedback,'''') = ''Hire'' then 602
							when ISNULL(ClientFeedback,'''') = ''NoHire'' then 603
							when ISNULL(ClientFeedback,'''') = ''OnHold'' then 604
							else 999
							end as ClientStatusFrontCode,
							--Priority,
							--InterviewROUND,
							--TalentFeedback,
							--FinalCall,
							--InterviewDuration,
							--IsReschedule,
							--Shortlisted_InterviewID,
							--HiringRequest_Detail_ID,
							--ContactID,
							--HiringRequest_ID,
							--Talent_ID,
							T.TotalRecords
							--ClientFeedbackId,
							--SlotGivenStatus,
							--IsLaterSlotGiven,
							--NextRound_InterviewDetailsID,
							--NextroundShortlistedId				
							FROM	CTE_Records U INNER JOIN Cte_TotalCount T on T.TotalCountID =U.TotalCountID WHERE isnull(InterviewDateTime,'''') != '''' '						

		

		If @SortExpression = 'CreatedByDatetime'
				BEGIN
					SET @SelectMainSQL += ' ORDER BY ID DESC, cast(' + @SortExpression + ' as datetime) ' + @SortDirection
									
				END
		ELSe 
			SET @SelectMainSQL += ' ORDER BY cast(' + @SortExpression + ' as datetime) ' + @SortDirection

		IF(@PageSize>0)
			BEGIN
				SET @SelectMainSQL= 	@SelectMainSQL+ '	OFFSET ' + CONVERT(nvarchar, ((@PageIndex - 1)  * @PageSize)) + ' ROWS 
								FETCH NEXT ' + CONVERT(nvarchar, @PageSize) + ' ROWS ONLY ';
			END


			SET @ExcMainSQL = (@MainSQL + @SelectMainSQL )
			EXEC sp_executesql @ExcMainSQL

			--SELECT @ExcMainSQL

END

