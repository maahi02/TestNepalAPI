using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Dtos
{
    public class UserRegistration
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? TenantId { get; set; }
        public UserAddress UserAddress { get; set; }
    }
}
