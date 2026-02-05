using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.Requests
{
    public class LoginByPasswordRequest
    {
        public string? Username { get; set; }
        public string Password { get; set; }
    }
}
