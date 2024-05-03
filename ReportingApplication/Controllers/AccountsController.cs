using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using ReportingApplication.Data.Entities.ApllicationDbContext;
using ReportingApplicationShared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ReportingApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class AccountsController : ControllerBase
    {
         
       private readonly  ApplicationDbContext _context;
       private readonly  UserManager<IdentityUser> _userManager;
       private readonly  SignInManager<IdentityUser> _signInManager;
       private readonly IConfiguration _configurationManager;


        public AccountsController( ApplicationDbContext dbContext 
            ,UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager
            , IConfiguration configuration)
        {
            _context = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _configurationManager= configuration;
        }

        [HttpPost(Name = "SignIn")]
        public async Task<IResult> SignIn(User user)
        {
          

            var result = await _signInManager.PasswordSignInAsync(user.Email,
                          user.Password, user.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded) {
                var signedInUser = await _signInManager.UserManager.FindByEmailAsync(user.Email);
                var role = await _userManager.GetRolesAsync(signedInUser);
                var issuer = _configurationManager.GetSection("Jwt")["Issuer"];
                var audience = _configurationManager.GetSection("Jwt")["Audience"];
                var key = Encoding.ASCII.GetBytes(_configurationManager.GetSection("Jwt")["Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("Id", signedInUser.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim("Role",role.FirstOrDefault()??""),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);
                return Results.Ok(signedInUser);
            }
        
            return Results.Unauthorized();
        }


        //[HttpPost(Name = "SignOut")]
        //public async Task<IResult> SignOut([FromBody] User user)
        //{

        //    await _signInManager.SignOutAsync();
        //    //if (true)
        //    //{
        //    //    var signedInUser = await _signInManager.UserManager.FindByNameAsync(user.Email);
        //    //    var role = await _userManager.GetRolesAsync(signedInUser);
        //    //    var issuer = _configurationManager.GetSection("Jwt")["Issuer"];
        //    //    var audience = _configurationManager.GetSection("Jwt")["Audience"];
        //    //    var key = Encoding.ASCII.GetBytes(_configurationManager.GetSection("Jwt")["Key"]);
        //    //    var tokenDescriptor = new SecurityTokenDescriptor
        //    //    {
        //    //        Subject = new ClaimsIdentity(new[]
        //    //        {
        //    //        new Claim("Id", signedInUser.Id),
        //    //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //    //        new Claim("Role",role.FirstOrDefault()??""),
        //    //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //    //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    //        }),
        //    //        Expires = DateTime.UtcNow.AddMinutes(5),
        //    //        Issuer = issuer,
        //    //        Audience = audience,
        //    //        SigningCredentials = new SigningCredentials
        //    //        (new SymmetricSecurityKey(key),
        //    //        SecurityAlgorithms.HmacSha512Signature)
        //    //    };
        //    //    var tokenHandler = new JwtSecurityTokenHandler();
        //    //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    //    var stringToken = tokenHandler.WriteToken(token);
        //    //    return Results.Ok(stringToken);
        //    //}

        //    return Results.Unauthorized();
        //}


    }
}
