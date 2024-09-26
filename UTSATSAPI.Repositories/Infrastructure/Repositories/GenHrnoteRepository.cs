using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenHrnoteRepository : GenericRepository<GenHrnote>, IGenHrnoteRepository
{
public GenHrnoteRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
