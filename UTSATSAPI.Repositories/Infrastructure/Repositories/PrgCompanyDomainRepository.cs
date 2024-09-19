using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgCompanyDomainRepository : GenericRepository<PrgCompanyDomain>, IPrgCompanyDomainRepository
{
public PrgCompanyDomainRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
