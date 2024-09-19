using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardTalentsReplacementDetailRepository : GenericRepository<GenOnBoardTalentsReplacementDetail>, IGenOnBoardTalentsReplacementDetailRepository
{
public GenOnBoardTalentsReplacementDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
