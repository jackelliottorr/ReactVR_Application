using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Models
{
    [Serializable]
    public class UserAccount
    {
        public Guid UserAccountId { get; set; } 
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; } 
    }
}
