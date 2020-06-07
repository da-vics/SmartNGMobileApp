using System;
using System.Collections.Generic;
using System.Text;

namespace SmartNG.DataProfiles
{

    public class RegistrationProfile
    {
        public string Email { get; set; }
        public string PassWordHash { get; set; }
        public string ApiKeyId { get; set; }
        public string Fullname { get; set; }
        public string HomeAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
