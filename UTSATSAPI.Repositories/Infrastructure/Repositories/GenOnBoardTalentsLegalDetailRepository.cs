using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardTalentsLegalDetailRepository : GenericRepository<GenOnBoardTalentsLegalDetail>, IGenOnBoardTalentsLegalDetailRepository
{
public GenOnBoardTalentsLegalDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
