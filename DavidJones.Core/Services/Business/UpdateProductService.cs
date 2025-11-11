using DavidJones.Core.Models.DapperDriver;
using DavidJones.Core.Models.ProductPusher;
using DavidJones.Core.Models.UpdateProducts;
using DavidJones.Core.Models.UpdateQohPrice;
using DavidJones.ProductSyncer.Helpers.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DavidJones.Core.Services.Business;

public class UpdateProductService
{
    #region updateColor from site color
    public async Task PullThenPushColor(IDapperDriver dbService, HttpClient httpClient)
    {
        var ProductListResult = await dbService.QueryAsync<object, ProductListDto>(new { }, new SpOptions("[dbo].[S_Site_Product_List]"));


        List<SiteUpdateProductDto> PropereProducts = new List<SiteUpdateProductDto>();

        foreach (var product in ProductListResult.Value ?? [])
        {
            SiteUpdateProductDto prdItem = new();

            var variantListResult = await dbService.QueryAsync<object, ProductVariantListDto>(new
            {
                product.ProductId,
            }, new SpOptions("[dbo].[S_Site_ProductVariant_List]"));

            var prd = new ProductDto() { name = product.Name ?? string.Empty };

            var variantList = variantListResult.Value ?? [];
            var variants = variantList.Select(x =>
            {

                var attributeColor = new ProductVariantAttributeItemDto
                {
                    name = "رنگ",
                    value = new VariantColorDto { value = x.ColorNamePersian, extra = x.ColorExtraCode, fieldType = "color" }
                };
                return new ProdcutVariantItemDto { id = x.SiteVariantId, attributes = [attributeColor] };
            });
            prdItem.ProductId = product.ProductId;
            prdItem.product = prd;
            prdItem.product_variants = variants.ToArray();
            prdItem.attributes = [new ProductAttributeItemDto { name = "رنگ", type = "string", value = "", attribute_type = "differentiator" }];

            PropereProducts.Add(prdItem);
        }
        Console.WriteLine("generation of list propereProduct is completed ");
        foreach (var prpProduct in PropereProducts ?? [])
        {
            string urlpushtosite = $"products/{prpProduct.ProductId}";
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(prpProduct), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(urlpushtosite, httpContent);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node.Deserialize<SiteUpdateProductResDto>();

            if (value is null)
                throw new Exception("response from website is null");

            Console.WriteLine($"updating product with id : {value.result?.product?.id} completed success");

            if ((value.status is not null && value.status != 200)
                || (value.error_code != 0))
            {
                var logResult = await dbService.ExecuteAsync<object>(new
                {
                    JsonFailedVariantIds = JsonSerializer.Serialize(value.result?.product ?? new object { }),
                    FlagDescription = "push-update-productColor"
                }, new SpOptions("[dbo].[S_Log_FailedVariantIds_Insert]"));

            }
        }
    }
    #endregion
}



