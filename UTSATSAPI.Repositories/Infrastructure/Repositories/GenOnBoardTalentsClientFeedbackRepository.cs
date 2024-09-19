using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardTalentsClientFeedbackRepository : GenericRepository<GenOnBoardTalentsClientFeedback>, IGenOnBoardTalentsClientFeedbackRepository
{
public GenOnBoardTalentsClientFeedbackRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
