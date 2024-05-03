using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using ReportingApplication.Data.Entities.ApllicationDbContext;
using ReportingApplicationShared.Models;
namespace ReportingApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class RolesController : ControllerBase
    {
         
       private readonly  ApplicationDbContext _context;


        public RolesController( ApplicationDbContext dbContext)
        {
            _context = dbContext;
    
        }

        [HttpGet(Name = "GetRoles")]
        public IEnumerable<Role> Get()
        {
            return _context.Roles.Select(role => new Role
            {
                Id = role.Id,
                Name = role.Name
            })
            .ToArray();
        }

    }
}
