using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenBlogRepository : GenericRepository<GenBlog>, IGenBlogRepository
{
public GenBlogRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
