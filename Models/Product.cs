using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

#nullable disable

namespace ProductQL.Models
{
    [Authorize]
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public DateTime Created { get; set; }
    }
}
