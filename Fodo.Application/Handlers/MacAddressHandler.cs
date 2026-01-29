using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Handlers
{
    public static class MacAddressHelper
    {
        public static string Normalize(string mac)
        {
            if (string.IsNullOrWhiteSpace(mac)) return null;

            return mac.Trim()
                      .ToUpperInvariant()
                      .Replace(":", "")
                      .Replace("-", "")
                      .Replace(" ", "");
        }
    }
}
