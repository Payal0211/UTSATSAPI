using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncPlacementFeesTalentUserRoleDetailRepository : GenericRepository<GenIncPlacementFeesTalentUserRoleDetail>, IGenIncPlacementFeesTalentUserRoleDetailRepository
{
public GenIncPlacementFeesTalentUserRoleDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
