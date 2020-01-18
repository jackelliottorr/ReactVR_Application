using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Models
{
    [Serializable]
    public class UserAccount
    {
        public Guid UserAccountId;
        public string Name;
        public string EmailAddress;
        public string Password;
        public DateTime CreatedDate;
        public bool IsDeleted;
    }
}
