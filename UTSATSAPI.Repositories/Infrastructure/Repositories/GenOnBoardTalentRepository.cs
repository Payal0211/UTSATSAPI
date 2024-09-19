using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardTalentRepository : GenericRepository<GenOnBoardTalent>, IGenOnBoardTalentRepository
{
public GenOnBoardTalentRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
