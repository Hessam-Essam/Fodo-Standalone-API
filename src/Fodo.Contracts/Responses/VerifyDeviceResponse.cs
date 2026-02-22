using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.Responses
{
    public class VerifyDeviceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? DeviceId { get; set; }
    }
}
