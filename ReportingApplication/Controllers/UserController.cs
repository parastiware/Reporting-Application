using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using ReportingApplication.Data.Entities.ApllicationDbContext;
using ReportingApplicationShared.Models;
namespace ReportingApplication.Controllers
{
  
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class UserController : ControllerBase
    {
         
       private readonly  ApplicationDbContext _context;


        public UserController( ApplicationDbContext dbContext)
        {
            _context = dbContext;
    
        }

        [HttpGet(Name = "GetUser")]
        public IEnumerable<User> Get()
        {
            return _context.Users.Select(user => new User
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed=user.UserName
            })
            .ToArray();
        }

        [HttpPost(Name = "CreateUser")]
        public IEnumerable<User> Post()
        {
            return _context.Users.Select(user => new User
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.UserName
            })
            .ToArray();
        }
    }
}
