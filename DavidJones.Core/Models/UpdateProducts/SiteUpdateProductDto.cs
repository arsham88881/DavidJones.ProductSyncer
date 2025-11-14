using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.UpdateProducts;

public class SiteUpdateProductDto
{
    [JsonIgnore]
    public int ProductId { get; set; }
    public ProductDto? product { get; set; }
    public ProductAttributeItemDto[]? attributes { get; set; }
    public ProdcutVariantItemDto[]? product_variants { get; set; }

}
public class ProductDto
{
    public required string name { get; set; }
}
public class ProductAttributeItemDto
{
    public string type { get; set; } = "string";
    public string name { get; set; } = "color";
    public string value { get; set; } = string.Empty;
    public string? attribute_type { get; set; }
}

public class ProdcutVariantItemDto
{
    public int min_order_count { get; set; } = 1;
    public bool is_stock_managed { get; set; }
    public required long id { get; set; }
    public ProductVariantAttributeItemDto[]? attributes { get; set; }

}


public class ProductVariantAttributeItemDto
{
    public required string name { get; set; }
    public VariantColorDto? value { get; set; }

}

public class VariantColorDto
{
    public string? extra { get; set; }
    public string? fieldType { get; set; } = "color";
    public string? value { get; set; }
}
