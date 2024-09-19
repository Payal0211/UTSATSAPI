ALTER PROCEDURE [Dbo].[Sproc_EmailHRTypeChanged]
	@HRID			bigint = 0,
	@TalentID		bigint = 0
AS
BEGIN
		
		SELECT  HR_Number = H.HR_Number,ClientName = Isnull(C.FullName,''),
				TalentName = Isnull(T.Name,''),
				FinalCost = Case when  H.IsHRTypeDP = 1 then cast(12 * Isnull(T.FinalCost,0) as Nvarchar(20)) + ' Per Anum' else cast(Isnull(T.FinalCost,0) as Nvarchar(20))  end,
				DPPercentage = ISnull(DP_Percentage,0),
				TalentRole = Isnull(T.Designation,''),
				SalesEmailId = ISnull(U.EmailID,''),
				SalesEmailName = Isnull(U.FullName,''),
				HRSalesPersonID = ISnull(H.SalesUserID,0)
		FROM	gen_SalesHiringRequest H WITH(NOLOCK)
				inner join gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
				inner join gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.HiringRequestID = H.ID and C.ID = CTP.ContactID
				inner join gen_Talent T WITH(NOLOCK) ON T.ID = CTP.TalentID
				inner join usr_User U WITH(NOLOCK) ON U.ID = H.SalesUserID 
		where   H.ID = @HRID and T.Id = @TalentID
	
ENd	