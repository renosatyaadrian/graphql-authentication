using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQL.Inputs
{
    public record LoginUserInput
    (
        string username,
        string password
    );
}