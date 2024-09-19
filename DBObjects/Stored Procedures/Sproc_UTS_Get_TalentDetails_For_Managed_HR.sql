ALTER PROCEDURE [dbo].[Sproc_UTS_Get_TalentDetails_For_Managed_HR]
	@HR_Id				bigint = null
AS
BEGIN
			Select		Isnull(T.ManagedTalentFirstName,'') + ' ' + IsNULL(T.ManagedTalentLastName,'') as TalentName,
						TalentStatus=CASE WHEN H.Status_ID not in (3,4) then 'Shortlisted' when H.Status_ID = 3 then 'Hired' when H.Status_ID = 4 then 'Cancelled' else '' end , 
						Cast(T.HRCost as Nvarchar(10)) + ' ' + CC.CurrencyCode + ' / Month' AS BillRate,
						Cast(T.ManagedTalentCost as Nvarchar(10)) + ' ' + 'USD / Month'  As PayRate,				   
						Convert(varchar,T.NRPercentage) + ' %' AS NR,					
						isnull(T.ID,0) as TalentID,
						isnull(HD.ID,0) as HiringDetailID,
						Isnull(H.ContactID,0) as ContactId,
						ISNULL(O.ClientOnBoarding_StatusID,0) ClientOnBoarding_StatusID,
						Isnull(O.ID,0) AS OnBoardId,
						Isnull(POB_Clientonboard.PreOnBoardStatus,'') ClientOnBoardStatus,
						ISNULL(O.TalentOnBoarding_StatusID,0) TalentOnBoarding_StatusID,
						ISNULL(O.ClientLegal_StatusID,0) LegalClientOnBoarding_StatusID,
						ISNULL(O.TalentLegal_StatusID,0) LegalTalentOnBoarding_StatusID,
						ISNULL(O.Kickoff_StatusID,0) Kickoff_StatusID,
						Isnull(POB_Talentonboard.PreOnBoardStatus,'') TalentOnBoardStatus,
						ISNULL(T.ManagedTalent_Level,'') ManagedTalent_Level,
						ISNULL(T.POC_FullName,'') POC_FullName,
						ISNULL(T.ScopeOfWork,'') ScopeOfWork,
						TalentOnBoardDate =  ISNULL((CONVERT(varchar, ISNULL(OC.TalentOnBoardDate, ''), 103) + ' '  + convert(VARCHAR(8),OC.TalentOnBoardDate, 14)),'') ,
						ISNULL(TT.Talent_Type,'') as TalentSource,
						ISNULL(TR.TalentRole,'') as TalentRole,
						ProfileStatusCode = CASE	WHEN H.Status_ID not in (3,4) THEN 302
													WHEN H.Status_ID = 3 THEN 304
													WHEN H.Status_ID = 4 THEN 305
											END

			from		gen_ManagedTalent T WITH(NOLOCK)					
						inner join gen_SalesHiringRequest H WITH(NOLOCK) ON H.ID = T.ManagedHRId
						inner join gen_SalesHiringRequest_Details HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID		
						inner join prg_CurrencyExchangeRate CC WITH(NOLOCK) ON  CC.CurrencyCode = HD.Currency
						inner join prg_TalentRoles TR with(nolock) on T.ManagedTalentRoleID = TR.ID
						left join gen_OnBoardTalents O WITH(NOLOCK) ON O.ManagedTalentID = T.ID AND O.HiringRequest_ID = T.ManagedHRId
						left join gen_OnBoardClientContractDetails OC WITH(NOLOCK) ON OC.OnBoardID = O.ID
						left join prg_PreOnBoardStatus POB_Clientonboard WITH(NOLOCK) ON POB_Clientonboard.ID = O.ClientOnBoarding_StatusID
						left join prg_PreOnBoardStatus POB_Talentonboard WITH(NOLOCK) ON POB_Talentonboard.ID = O.TalentOnBoarding_StatusID AND O.Talent_ID = T.ID
						left join prg_PreOnBoardStatus POB_TalentLegal WITH(NOLOCK) ON POB_TalentLegal.ID = O.TalentLegal_StatusID
						left join prg_PreOnBoardStatus POB_ClientLegal WITH(NOLOCK) ON POB_ClientLegal.ID = O.ClientLegal_StatusID
						left join prg_TalentType TT with(nolock) on T.ManagedTalent_TypeID = TT.ID 
			WHERE		H.ID=@HR_ID	
			order by	T.CreatedByDatetime desc
			

END