using Fodo.Contracts.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.Responses
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
