using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductQL.Inputs
{
    public record AddProductInput
    (
        int? id,
        string Name,
        int Stock,
        double Price,
        DateTime? Created
    );
}