using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardClientLeavePolicyRepository : GenericRepository<GenOnBoardClientLeavePolicy>, IGenOnBoardClientLeavePolicyRepository
{
public GenOnBoardClientLeavePolicyRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
