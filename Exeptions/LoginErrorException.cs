using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQL.Exeptions
{
    public class LoginErrorException : Exception
    {
        public int LoginUsername { get; internal set; }
    }
}