using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.SiteGetProduct;

public class ResultGetProductSiteDto
{
    public ResultGetProductSiteResultDto? result { get; set; }
    public string? error { get; set; }
    public int? error_code { get; set; }
    public int? status { get; set; }

}
public class ResultGetProductSiteResultDto
{
    public int? max_price { get; set; }
    public int? min_price { get; set; }
    public int? page_number { get; set; }
    public int? page_size { get; set; }
    public List<ResultGetProductItemDto>? products { get; set; }
    public int? stock_alert_limit { get; set; }
    public object? sub_categories { get; set; }
    public int? total_count { get; set; }
    public int? total_count_raw { get; set; }
}


public class ResultGetProductItemDto
{
    public int? id { get; set; }
    public string? name { get; set; }
    public string? url { get; set; }
    public List<ResultGetProductVariantItemDto>? product_variants { get; set; }
}

public class ResultGetProductVariantItemDto
{
    public int? id { get; set; }
    public string? title { get; set; }
    public long? price { get; set; }
    public List<ResultGetVariantAttributeDto>? product_attributes { get; set; }
    public int? image_id { get; set; }
    public int? sold_count { get; set; }

}
public class ResultGetVariantAttributeDto
{
    public string? name { get; set; }
    public string? type { get; set; }
    public ResultAttributeColor? value { get; set; }
}

//public class ResultGetVariantAttributeColorDto : ResultGetVariantAttributeDto
//{
//    public new ResultAttributeColor? value { get; set; }

//    //public static explicit operator ResultGetVariantAttributeColorDto(ResultGetVariantAttributeDto v)
//    //{
//    //    throw new NotImplementedException();
//    //}
//}
public class ResultGetVariantAttributeSizeDto //: ResultGetVariantAttributeDto
{
    public string? value { get; set; }
}

public class ResultAttributeColor
{
    public string? extra { get; set; }
    public string? fieldType { get; set; } = "color";
    public string? value { get; set; }
}