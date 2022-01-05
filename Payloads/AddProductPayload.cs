using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductQL.Models;

namespace ProductQL.Payloads
{
    public class AddProductPayload
    {
        public AddProductPayload(Product product)
        {
            this.product = product;
        }
        public Product product { get; set; }
    }
}