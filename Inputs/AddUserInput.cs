using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQL.Inputs
{
    public record AddUserInput
    (
        string FullName,
        string Username,
        string Email,
        string Password
    );
}