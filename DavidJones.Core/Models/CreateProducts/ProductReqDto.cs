using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.CreateProducts;

public class ProductReqDto
{
    [JsonIgnore]
    public string? ItemCode { get; set; }
    public CreateProductDto? product { get; set; }
    public string[] tags { get; set; } = [];
    public CreateProductAttributeItemDto[]? attributes { get; set; }
    public CreateProdcutVariantItemDto[]? product_variants { get; set; }

}
public class CreateProductDto
{
    public required string name { get; set; }
    public bool enabled { get; set; } = true;
    public string url { get; set; } = string.Empty;
    public string product_type { get; set; } = "simple";
    public short? form_id { get; set; } = -1;
}
public class CreateProductAttributeItemDto
{
    public string type { get; set; } = "string";
    public string name { get; set; } = "color";
    public string? attribute_type { get; set; }
}

public class CreateProdcutVariantItemDto
{
    public long? price { get; set; }
    public string? sku { get; set; }
    public bool? enabled { get; set; } = true;
    public int min_order_count { get; set; } = 1;
    public bool? is_stock_managed { get; set; } = false;
    public int? stock_number { get; set; }
    public int? weight { get; set; } = 1;
    public int? sort_index { get; set; }
    public CreateProductVariantAttributeItemDto[]? attributes { get; set; }


}


public class CreateProductVariantAttributeItemDto
{
    public required string name { get; set; }
    public CreateVariantColorDto? value { get; set; }

}

public class CreateVariantColorDto
{
    public string? extra { get; set; }
    public string? fieldType { get; set; } = "color";
    public string? value { get; set; }
}
