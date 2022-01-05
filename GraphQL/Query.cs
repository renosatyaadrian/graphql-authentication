using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using ProductQL.Models;

namespace ProductQL.GraphQL
{
    public class Query
    {
        [Authorize]
        public IQueryable<User> GetUsers([Service] productsContext context){
            return context.Users.Select(user=> new User{
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.Username
            });   
            
        }
         public IQueryable<Product> GetProducts([Service] productsContext context) => context.Products; 
        
    }
}