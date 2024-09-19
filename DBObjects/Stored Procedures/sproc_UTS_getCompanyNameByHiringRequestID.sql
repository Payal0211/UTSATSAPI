create proc sproc_UTS_getCompanyNameByHiringRequestID
@HiringRequestID bigint=0
as
select Company from gen_company gco
inner join gen_contact gc WITH(NOLOCK) on gco.ID = gc.CompanyID
inner join gen_salesHiringrequest gs WITH(NOLOCK) on gs.ContactID = gc.ID
where gs.ID = @HiringRequestID   
