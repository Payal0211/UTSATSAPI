using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncUserTargetRepository : GenericRepository<GenIncUserTarget>, IGenIncUserTargetRepository
{
public GenIncUserTargetRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
