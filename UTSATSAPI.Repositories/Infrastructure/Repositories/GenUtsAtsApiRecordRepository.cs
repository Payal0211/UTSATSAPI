using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenUtsAtsApiRecordRepository : GenericRepository<GenUtsAtsApiRecord>, IGenUtsAtsApiRecordRepository
{
public GenUtsAtsApiRecordRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
