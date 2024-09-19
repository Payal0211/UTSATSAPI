using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenPayoutInformationRepository : GenericRepository<GenPayoutInformation>, IGenPayoutInformationRepository
{
public GenPayoutInformationRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
