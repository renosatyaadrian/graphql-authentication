using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using ProductQL.Exeptions;

namespace ProductQL.Filters
{
    public class LoginErrorExceptionFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is LoginErrorException ex)
                return error.WithMessage($"Username or password not match");
                
            return error;
        }
        
    }
}