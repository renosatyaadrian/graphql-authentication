using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductQL.Models;

namespace ProductQL.Payloads
{
    public class AddUserPayload
    {
        public AddUserPayload(User user)
        {
            this.user = user;
        }

        public User user { get; set; }
    }
}