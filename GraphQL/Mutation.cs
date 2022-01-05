using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
// using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductQL.Exeptions;
using ProductQL.Helpers;
using ProductQL.Inputs;
using ProductQL.Models;
using ProductQL.Payloads;

namespace ProductQL.GraphQL
{
    public class Mutation
    {
        private readonly productsContext _context;
        private readonly AppSettings _appSettings;
        public Mutation([Service] productsContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<AddUserPayload> RegisterUserAsync(AddUserInput input, ClaimsPrincipal claims)
        {
            var userFind = _context.Users.Any(us=>us.Username.ToLower() == input.Username.ToLower());
            if(userFind) return new AddUserPayload(new User());
            // byte[] salt = new byte[128 / 8];
            // string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            //     password: input.Password,
            //     salt: salt,
            //     prf: KeyDerivationPrf.HMACSHA256,
            //     iterationCount: 100000,
            //     numBytesRequested: 256 / 8));

            var hashedPass = BCrypt.Net.BCrypt.HashPassword(input.Password);
            var user = new User
            {
                FullName = input.FullName,
                Username = input.Username,
                Email = input.Email,
                Password = hashedPass
            };
           _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new AddUserPayload(user);
        }

        public async Task<LoginPayload> LoginUserAsync(LoginUserInput input)
        {   
            // byte[] salt = new byte[128 / 8];
            // string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            //     password: input.password,
            //     salt: salt,
            //     prf: KeyDerivationPrf.HMACSHA256,
            //     iterationCount: 100000,
            //     numBytesRequested: 256 / 8));
            
            // Console.WriteLine(hashed);

            var userFind = _context.Users.Where(user=>user.Username==input.username).SingleOrDefault();

            if(userFind==null) throw new LoginErrorException();
            var valid = BCrypt.Net.BCrypt.Verify(input.password, userFind.Password);
            if(valid){

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, userFind.Username));
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return new LoginPayload(tokenString);
            }
            
            else throw new LoginErrorException();
        }

        [Authorize]
        public async Task<AddProductPayload> AddProductAsync(AddProductInput input)
        {
            var product = new Product
            {
                Name = input.Name,
                Stock = input.Stock,
                Price = input.Price,
                Created = DateTime.Now
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new AddProductPayload(product);
        }

        [Authorize]
        public async Task<AddProductPayload> DeleteProductsByIdAsync(IdProductInput input)
        {  

            var todo = _context.Products.Where(a=>a.Id==input.Id).SingleOrDefault();
            if(todo==null) throw new ArgumentNullException(nameof(todo));
            _context.Products.Remove(todo);  
            await _context.SaveChangesAsync();  
            return new AddProductPayload(todo);  
        }  
            
        [Authorize]
        public async Task<AddProductPayload> UpdateProductsAsync(AddProductInput input)  
        {  
            var todo = _context.Products.Where(a=>a.Id==input.id).SingleOrDefault();
            todo.Name = input.Name;
            todo.Stock = input.Stock;
            todo.Price = input.Price;

            var updatedEvent = (_context.Products.Update(todo)).Entity;  
            await _context.SaveChangesAsync();  
            return new AddProductPayload(updatedEvent);  
        }
    }
}