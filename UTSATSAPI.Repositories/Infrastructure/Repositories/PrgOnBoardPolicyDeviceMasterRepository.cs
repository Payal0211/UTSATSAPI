using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgOnBoardPolicyDeviceMasterRepository : GenericRepository<PrgOnBoardPolicyDeviceMaster>, IPrgOnBoardPolicyDeviceMasterRepository
{
public PrgOnBoardPolicyDeviceMasterRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
