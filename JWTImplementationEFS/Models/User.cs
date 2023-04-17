using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JWTImplementationEFS.Models
{
    public class User
    {
        public string employeeId { get; set; }
        public string USerName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}