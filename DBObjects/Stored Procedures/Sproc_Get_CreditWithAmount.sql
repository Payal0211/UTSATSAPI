create procedure Sproc_Get_CreditWithAmount
as
begin
SELECT ID,CreditAmount,CurrenyCode,INRAmount FROM dbo.prg_CreditWithAmountUtilization_ClientPortal
--Post a job -> option 1 
--Profile view -> option 2
end
