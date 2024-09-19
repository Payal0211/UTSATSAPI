ALTER PROCEDURE sproc_UTS_NextActionsForManagedHR
	@HiringRequestId BIGINT = NULL
AS
BEGIN

		select NextActionText = t.NextActionText from (select  NextActionText = isnull(case  WHEN ISNULL(O.ID,0)>0 AND isnull(O.Kickoff_StatusID,0)!=5   THEN 'Next Action is for '+ Isnull(T.ManagedTalentFirstName,'') + ' ' + IsNULL(T.ManagedTalentLastName,'')  +' to On Board ' +O.EngagemenID
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)!=2  THEN 'Next Action is for '+ Isnull(T.ManagedTalentFirstName,'') + ' ' + IsNULL(T.ManagedTalentLastName,'')  +' to Update Client OnBoard Status '
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND isnull(O.TalentOnBoarding_StatusID,0)!=2 AND isnull(O.TalentOnBoarding_StatusID,0)!=7  THEN 'Next Action is for '+ Isnull(T.ManagedTalentFirstName,'') + ' ' + IsNULL(T.ManagedTalentLastName,'')  +' to Update Talent OnBoard Status '					   
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND (isnull(O.TalentOnBoarding_StatusID,0)=2 OR isnull(O.TalentOnBoarding_StatusID,0)=7) AND isnull(O.ClientLegal_StatusID,0)!=2 THEN 'Next Action is for '+ Isnull(T.ManagedTalentFirstName,'') + ' ' + IsNULL(T.ManagedTalentLastName,'')  +' to Update Legal Client OnBoard Status '					   
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND (isnull(O.TalentOnBoarding_StatusID,0)=2 OR isnull(O.TalentOnBoarding_StatusID,0)=7) AND isnull(O.ClientLegal_StatusID,0)=2 AND isnull(O.TalentLegal_StatusID,0)!=2 AND isnull(O.Kickoff_StatusID,0)!=5  THEN 'Next Action is for '+ Isnull(T.ManagedTalentFirstName,'') + ' ' + IsNULL(T.ManagedTalentLastName,'')  +' to Update Legal Talent OnBoard Status '
								WHEN ISNULL(O.ID,0)>0 AND isnull(O.ClientOnBoarding_StatusID,0)=2 AND (isnull(O.TalentOnBoarding_StatusID,0)=2 OR isnull(O.TalentOnBoarding_StatusID,0)=7) AND isnull(O.ClientLegal_StatusID,0)=2 AND isnull(O.TalentLegal_StatusID,0)=2 AND isnull(O.Kickoff_StatusID,0)!=5   THEN 'Next Action is for '+ Isnull(T.ManagedTalentFirstName,'') + ' ' + IsNULL(T.ManagedTalentLastName,'') +' to Update Kickoff OnBoard Status '
								ELSE '' END,'') ,
								T.ID
						 from		gen_ManagedTalent T WITH(NOLOCK)					
						inner join gen_SalesHiringRequest H WITH(NOLOCK) ON H.ID = T.ManagedHRId
						inner join gen_SalesHiringRequest_Details HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID		
						inner join prg_CurrencyExchangeRate CC WITH(NOLOCK) ON  CC.CurrencyCode = HD.Currency
						left join gen_OnBoardTalents O WITH(NOLOCK) ON O.ManagedTalentID = T.ID AND O.HiringRequest_ID = T.ManagedHRId
						left join gen_OnBoardClientContractDetails OC WITH(NOLOCK) ON OC.OnBoardID = O.ID
						left join prg_PreOnBoardStatus POB_Clientonboard WITH(NOLOCK) ON POB_Clientonboard.ID = O.ClientOnBoarding_StatusID
						left join prg_PreOnBoardStatus POB_Talentonboard WITH(NOLOCK) ON POB_Talentonboard.ID = O.TalentOnBoarding_StatusID AND O.Talent_ID = T.ID
						left join prg_PreOnBoardStatus POB_TalentLegal WITH(NOLOCK) ON POB_TalentLegal.ID = O.TalentLegal_StatusID
						left join prg_PreOnBoardStatus POB_ClientLegal WITH(NOLOCK) ON POB_ClientLegal.ID = O.ClientLegal_StatusID
			WHERE		H.ID=@HiringRequestId	)t
			where t.NextActionText <> ''
			order by T.ID

END