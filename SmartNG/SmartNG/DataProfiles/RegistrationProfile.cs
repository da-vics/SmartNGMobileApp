using System;
using System.Collections.Generic;
using System.Text;

namespace SmartNG.DataProfiles
{

    public class RegistrationProfile
    {
        public string Email { get; set; } = string.Empty;
        public string PassWordHash { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
