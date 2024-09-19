using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardClientDevicePolicyDetailRepository : GenericRepository<GenOnBoardClientDevicePolicyDetail>, IGenOnBoardClientDevicePolicyDetailRepository
{
public GenOnBoardClientDevicePolicyDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
