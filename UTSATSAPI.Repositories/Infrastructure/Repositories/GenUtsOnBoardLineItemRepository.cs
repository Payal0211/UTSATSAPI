using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenUtsOnBoardLineItemRepository : GenericRepository<GenUtsOnBoardLineItem>, IGenUtsOnBoardLineItemRepository
{
public GenUtsOnBoardLineItemRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
