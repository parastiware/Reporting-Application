using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingApplicationShared.Models
{
    public class User
    {
        public string Id { get; set; }= string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
        public Boolean RememberMe { get; set; }=false;
        public string EmailConfirmed { get; set; } = string.Empty;  
        public string PasswordConfirmed { get; set; } = string.Empty;


    }
}
