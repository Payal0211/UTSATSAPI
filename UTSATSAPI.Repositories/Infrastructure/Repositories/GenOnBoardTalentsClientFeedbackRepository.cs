using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardTalentsClientFeedbackRepository : GenericRepository<GenOnBoardTalentsClientFeedback>, IGenOnBoardTalentsClientFeedbackRepository
{
public GenOnBoardTalentsClientFeedbackRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
