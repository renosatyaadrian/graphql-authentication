using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQL.Payloads
{
    public class LoginPayload
    {
        public LoginPayload(string Token)
        {
            this.Token = Token;
        }
        public string Token { get; set; }
    }
}