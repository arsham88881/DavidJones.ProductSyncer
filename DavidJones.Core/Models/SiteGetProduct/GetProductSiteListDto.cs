using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.SiteGetProduct;

public class GetProductSiteListDto
{
    public int? SiteProductId { get; set; }
    public int? SiteVariantId { get; set; }
    public string? SiteTitle { get; set; }
    public long? SellPrice { get; set; }
    public string? ColorNamePersian { get; set; }
    public string? ColorExtraCode { get; set; }
}
