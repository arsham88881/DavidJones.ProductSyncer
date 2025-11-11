using DavidJones.Core.Models.DapperDriver;
using DavidJones.Core.Models.SiteGetProduct;
using DavidJones.Core.Models.UpdateQohPrice;
using DavidJones.ProductSyncer.Helpers.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DavidJones.Core.Services.Business;

public class SiteGetAllProductsService
{
    public async Task GetAllForSyncWithPushed(IDapperDriver dbService, HttpClient httpClientSite)
    {
        int pageIndex = 0;
        int pageEndIndex = -1;
        int pageSize = 10;
        long totalCount;
        bool firstSetTotal = false;
        try
        {
            do
            {
                string urlpushtosite = $"products?page_number={pageIndex}&page_size={pageSize}";
                var response = await httpClientSite.GetAsync(urlpushtosite);
                var content = await response.Content.ReadAsStringAsync();


                var node = JsonNode.Parse(content);
                var value = node.Deserialize<ResultGetProductSiteDto>();

                if (value is null)
                    throw new Exception("response from website is null");

                if (!firstSetTotal)
                {
                    totalCount = value.result?.total_count ?? 0;
                    firstSetTotal = true;
                    pageEndIndex = (int)Math.Ceiling((double)totalCount / pageSize);
                    Console.WriteLine($"pageEndIndex is {pageEndIndex}");
                }
                List<GetProductSiteListDto> productVList = new List<GetProductSiteListDto>();

                foreach (var item in value.result?.products ?? [])
                {
                    var p = item.product_variants?.Select(xvariant =>
                    {
                        var xx = xvariant.product_attributes;
                        var coloratt = xx?.FirstOrDefault()?.value;

                        var ite = new GetProductSiteListDto
                        {
                            SiteProductId = item.id,
                            SiteVariantId = xvariant.id,
                            SiteTitle = item.name,
                            SellPrice = xvariant.price,
                            ColorNamePersian = coloratt?.value,
                            ColorExtraCode = coloratt?.extra,
                        };
                        return ite;
                    });
                    productVList.AddRange(p ?? []);

                }
                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true // Optional: for pretty formatting
                };
                var r = JsonSerializer.Serialize(productVList, options);
                var logResult = await dbService.ExecuteAsync<object>(new
                {
                    JsonProductList = r
                }, new SpOptions("[dbo].[S_Site_SyncAllProduct_Insert]"));

                Console.WriteLine($"page is index {pageIndex}");
                pageIndex++;
            } while (pageIndex <= pageEndIndex);
            Console.WriteLine("process is completed ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error occurred during getting product from website : {ex.Message}");
        }

    }
}
