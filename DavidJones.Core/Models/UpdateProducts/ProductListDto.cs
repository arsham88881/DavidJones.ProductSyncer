using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.UpdateProducts;

public class ProductListDto
{
    public int ProductId { get; set; }
    public string? Name { get; set; }

}

public class CreateProductListDto
{
    public string? ItemCode { get; set; }
    public string? ItemDisplayName { get; set; }

}

