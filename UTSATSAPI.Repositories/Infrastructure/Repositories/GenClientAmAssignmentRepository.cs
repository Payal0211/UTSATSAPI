using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenClientAmAssignmentRepository : GenericRepository<GenClientAmAssignment>, IGenClientAmAssignmentRepository
{
public GenClientAmAssignmentRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
