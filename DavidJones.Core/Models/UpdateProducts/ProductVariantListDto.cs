using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.UpdateProducts;

public class ProductVariantListDto
{
    public int SiteProductId { get; set; }
    public int SiteVariantId { get; set; }
    public string? SiteTitle { get; set; }
    public required string ColorName { get; set; }
    public required string ColorNamePersian { get; set; }
    public required string ColorExtraCode { get; set; }
}


public class CreateProductVariantListDto
{
    public int? SortIndex { get; set; }
    public int? StockNumber { get; set; }
    public long? Price { get; set; }
    public required string ColorName { get; set; }
    public required string ColorNamePersian { get; set; }
    public required string ColorExtraCode { get; set; }
}
