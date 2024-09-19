ALTER PROCEDURE [dbo].[sproc_UTS_NextActionsForTalent]
	@HiringRequestId	BIGINT = NULL

AS
BEGIN
	
	Select 
	top 1		IMasterID =max(IntM.ID) ,IntM.HiringRequest_ID ,IntM.Talent_ID ,IntM.InterviewRound_Str  AS InterviewRound_Str
				,max(Intm.InterviewRound_Count)   AS InterviewRound_Count
	into		#TempInterviewDetails
    from		gen_InterviewSlotsMaster IntM WITH(NOLOCK) 	
	Where		IntM.HiringRequest_ID = @HiringRequestId
	group by	IntM.HiringRequest_ID ,IntM.Talent_ID, IntM.InterviewRound_Str
	order by	IMasterID desc
	
	SELECt NextActionText = t.NextActionText from (select distinct  NextActionText = isnull(case WHEN CTP.TalentStatusID_BasedOnHR = 1 THEN 'Next Action is for '+case when Isnull(H.IsAdHocHR,0) = 0 AND Isnull(H.IsPoolHR,0) = 1 then 'Pool'
								when isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND Isnull(H.IsAdHocHR,0) = 1  AND Isnull(H.IsPoolHR,0) = 0 then 'ODR'    
								when isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND Isnull(H.IsAdHocHR,0) = 1 AND Isnull(H.IsPoolHR,0) = 1 then 'ODR + Pool' ELSE '' end +' team to get Acceptance from '+ T.Name 
								WHEN isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND CTP.TalentStatusID_BasedOnHR = 2 AND isnull((Select Max(InterviewRound_Count) from gen_InterviewSlotsMaster WITH(NOLOCK) WHERE HiringRequest_ID=CTP.HiringRequestID and Talent_ID=CTP.TalentID),0) =0 then  'Next Action is for the demand team to  schedule the interview  for '+ T.Name
								WHEN isnull(INS.InterviewStatus,'')='Slot Given' AND isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND CTP.TalentStatusID_BasedOnHR in (1,2) AND (isnull( (select top 1 SI.ID from gen_TalentSelected_InterviewDetails SI with(nolock)
													  inner join gen_InterviewSlotsMaster M WITH(NOLOCK) ON M.ID = SI.InterviewMaster_ID
													  where  M.InterviewStatus_ID in(1,4,5,6,7) AND isnull(IsConfirmed,0) =1 
													  AND ISnull(Shortlisted_InterviewID,0) <> 0  AND SI.Talent_ID = T.ID and SI.HiringRequest_ID = @HiringRequestId ),0)) = 0 THEN 'Next Action is for '+ T.Name +' to Confirm a Slot for an Interview'+ ' '+Tmp.InterviewRound_Str
					            WHEN isnull(INS.InterviewStatus,'')='Interview Completed' AND isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='On Hold' AND isnull(TSACS.TalentStatus,'')!='Rejected' then  'next action is for  client to submit feedback for '+ T.Name + ' For Interview '+ Tmp.InterviewRound_Str
							    
								WHEN isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND ISNULL(O.ID,0)>0 AND isnull(O.Kickoff_StatusID,0)!=5 THEN 'Next Action is for '+ T.Name +' to On Board ' +O.EngagemenID
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)!=2 AND isnull(INS.InterviewStatus,'')='Feedback Submitted' AND isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND isnull(TSACS.TalentStatus,'')!='On Hold' THEN 'Next Action is for '+ T.Name +' to Update Client OnBoard Status '
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND isnull(O.TalentOnBoarding_StatusID,0)!=2 AND isnull(O.TalentOnBoarding_StatusID,0)!=7 AND isnull(INS.InterviewStatus,'')='Feedback Submitted' AND isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND isnull(TSACS.TalentStatus,'')!='On Hold' THEN 'Next Action is for '+ T.Name +' to Update Talent OnBoard Status '					   
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND (isnull(O.TalentOnBoarding_StatusID,0)=2 OR isnull(O.TalentOnBoarding_StatusID,0)=7) AND isnull(O.ClientLegal_StatusID,0)!=2 AND isnull(INS.InterviewStatus,'')='Feedback Submitted' AND isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND isnull(TSACS.TalentStatus,'')!='On Hold' THEN 'Next Action is for '+ T.Name +' to Update Legal Client OnBoard Status '					   
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND (isnull(O.TalentOnBoarding_StatusID,0)=2 OR isnull(O.TalentOnBoarding_StatusID,0)=7) AND isnull(O.ClientLegal_StatusID,0)=2 AND isnull(O.TalentLegal_StatusID,0)!=2 AND isnull(O.Kickoff_StatusID,0)!=5 AND isnull(INS.InterviewStatus,'')='Feedback Submitted' AND isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND isnull(TSACS.TalentStatus,'')!='On Hold' THEN 'Next Action is for '+ T.Name +' to Update Legal Talent OnBoard Status '
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND (isnull(O.TalentOnBoarding_StatusID,0)=2 OR isnull(O.TalentOnBoarding_StatusID,0)=7) AND isnull(O.ClientLegal_StatusID,0)=2 AND isnull(O.TalentLegal_StatusID,0)=2 AND isnull(O.Kickoff_StatusID,0)!=5 AND isnull(INS.InterviewStatus,'')='Feedback Submitted' AND isnull(TSACS.TalentStatus,'')!='Cancelled' AND isnull(TSACS.TalentStatus,'')!='In Replacement' AND isnull(TSACS.TalentStatus,'')!='Rejected' AND isnull(TSACS.TalentStatus,'')!='On Hold' THEN 'Next Action is for '+ T.Name +' to Update Kickoff OnBoard Status '
								ELSE '' END,''),
								T.ID
	
	from gen_Talent T WITH(NOLOCK)
	inner join [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID=T.ID
	inner join [dbo].[gen_SalesHiringRequest] H WITH(NOLOCK) ON H.ID=CTP.HiringRequestID
	inner join [dbo].[gen_SalesHiringRequest_Details] HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID
	inner join prg_TalentStatus_AfterClientSelection TSACS WITH(NOLOCK)  ON  TSACS.ID = CTP.TalentStatusID_BasedOnHR
	
	LEft JOIN #TempInterviewDetails Tmp ON Tmp.HiringRequest_ID = H.ID AND Tmp.Talent_ID = T.ID
	LEft join gen_InterviewSlotsMaster IntM WITH(NOLOCK) ON IntM.ID = Tmp.IMasterID and Tmp.InterviewRound_Count = IntM.InterviewRound_Count
	Left JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.InterviewRound = IntM.InterviewRound_Count and TI.InterviewMaster_ID=IntM.ID AND TI.HiringRequest_ID = IntM.HiringRequest_ID AND TI.HiringRequest_Detail_ID = IntM.HiringRequest_Detail_ID AND TI.Talent_ID = IntM.Talent_ID
	LEft join prg_InterviewStatus INS WITH(NOLOCK) ON INS.ID = IntM.InterviewStatus_ID		
	left join gen_OnBoardTalents O WITH(NOLOCK) ON O.Talent_ID = T.ID AND O.HiringRequest_ID = CTP.HiringRequestID
	left join gen_OnBoardClientContractDetails OC WITH(NOLOCK) ON OC.OnBoardID = O.ID
	left join prg_PreOnBoardStatus POB_Clientonboard WITH(NOLOCK) ON POB_Clientonboard.ID = O.ClientOnBoarding_StatusID
	left join prg_PreOnBoardStatus POB_Talentonboard WITH(NOLOCK) ON POB_Talentonboard.ID = O.TalentOnBoarding_StatusID AND O.Talent_ID = T.ID
	left join prg_PreOnBoardStatus POB_TalentLegal WITH(NOLOCK) ON POB_TalentLegal.ID = O.TalentLegal_StatusID
	left join prg_PreOnBoardStatus POB_ClientLegal WITH(NOLOCK) ON POB_ClientLegal.ID = O.ClientLegal_StatusID
    left join prg_TalentJoinning TJ WITH(NOLOCK)  ON TJ.ID = T.Joining_ID	
	left join gen_ContactInterviewFeedback CF with(nolock) ON  CF.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND CF.HiringRequest_ID =  Tmp.HiringRequest_ID
	left join gen_TalentSelected_NextRound_InterviewDetails N WITH(NOLOCK) ON N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND N.Talent_ID = Tmp.Talent_ID and N.InterviewRound = IntM.InterviewRound_Count
	
	WHERE H.ID=@HiringRequestId ) t
		where t.NextActionText <> '' order by t.ID

		 drop table #TempInterviewDetails

END