using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgCompanySizeRepository : GenericRepository<PrgCompanySize>, IPrgCompanySizeRepository
{
public PrgCompanySizeRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
