using DavidJones.Core.Models.DapperDriver;
using DavidJones.Core.Models.ProductPusher;
using DavidJones.Core.Models.UpdateQohPrice;
using DavidJones.ProductSyncer.Helpers.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DavidJones.Core.Services.Business;

public class UpdateQohPriceService
{
    #region update prices pull and push 
    public async Task PullUpdatePrice(IDapperDriver dbservice, HttpClient httpClient)
    {
        var itemInqResult = await dbservice.QueryAsync<object, ItemQuantityOnHandDto>(new { }, new SpOptions("[dbo].[S_ProductVariant_QuantityOnHand]"));


        foreach (var item in itemInqResult.Value ?? [])
        {
            string urlGetItemBycode = $"itemlist?itembarcode={item.ItemBarcode}";
            var response = await httpClient.GetAsync(urlGetItemBycode);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node?["Value"];

            var dbResult = await dbservice.ExecuteAsync<object>(new
            {
                JsonItemPrices = value?.ToString()

            }, new SpOptions("[dbo].[S_ProductVariant_Price_Insert]"));

            Console.WriteLine($"product Variant images successfully with id : {item.ItemBarcode}");
        }
        Console.WriteLine("completed process ");

    }
    public async Task PushUpdatePrice(IDapperDriver dbservice, HttpClient httpClient)
    {
        int pageIndex = 0;
        int pageEndIndex = -1;
        int pageSize = 50;
        long totalCount;
        bool firstSetTotal = false;

        try
        {
            do
            {

                var resultListQuantity = await dbservice.QueryAsync<object, ItemPriceDto>(
                    new
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                    }, new SpOptions("[dbo].[S_Push_Price_tosite]", hasTotalCount: true));

                if (!firstSetTotal)
                {
                    totalCount = resultListQuantity.TotalCount ?? 0;
                    firstSetTotal = true;
                    pageEndIndex = (int)Math.Ceiling((double)totalCount / pageSize);
                }

                var reqDto = new SiteBulkUpdateReqDto<ItemPriceDto>();
                reqDto.variants = resultListQuantity.Value?.ToList() ?? [];

                if (reqDto.variants.Count == 0)
                    break;

                //sending to david joes site 
                string urlpushtosite = $"accounting/bulk-update-price";
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(reqDto), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(urlpushtosite, httpContent);
                var content = await response.Content.ReadAsStringAsync();
                var node = JsonNode.Parse(content);
                var value = node.Deserialize<SiteBulkUpdateResDto>();

                if (value is null)
                    throw new Exception("response from website is null");

                if (value.failed_variant_ids is not null)
                {
                    var logResult = await dbservice.ExecuteAsync<object>(new
                    {
                        JsonFailedVariantIds = JsonSerializer.Serialize(value.failed_variant_ids),
                        FlagDescription = "push-update-price"
                    }, new SpOptions("[dbo].[S_Log_FailedVariantIds_Insert]"));

                }
                pageIndex++;
            } while (pageIndex <= pageEndIndex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error occurred during pushing quantity on hand to website : {ex.Message}");
        }
    }
    #endregion

    #region quantity on hand pull and push 
    public async Task PullUpdateQuantityOnHand(IDapperDriver dbservice, HttpClient httpClient) //update price and quantity On hand 
    {
        var itemInqResult = await dbservice.QueryAsync<object, ItemQuantityOnHandDto>(new { }, new SpOptions("[dbo].[S_ProductVariant_QuantityOnHand]"));


        foreach (var item in itemInqResult.Value ?? [])
        {
            string urlGetItemBycode = $"barcodeqoh?count=100&itembarcode={item.ItemBarcode}";
            var response = await httpClient.GetAsync(urlGetItemBycode);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node?["Value"];
            //var table = value?.Deserialize<object>();

            var dbResult = await dbservice.ExecuteAsync<object>(new
            {
                JsonItemQuantityOnHands = value?.ToString() //JsonSerializer.Serialize(table),

            }, new SpOptions("[dbo].[S_ProductVariant_QuantityOnHand_Insert]"));

            Console.WriteLine($"product Variant images successfully with id : {item.ItemBarcode}");
        }
        Console.WriteLine("completed process ");
    }
    public async Task PushUpdateQuantityOnHand(IDapperDriver dbservice, HttpClient httpClient)
    {
        int pageIndex = 0;
        int pageEndIndex = -1;
        int pageSize = 50;
        long totalCount;
        bool firstSetTotal = false;

        try
        {
            do
            {

                var resultListQuantity = await dbservice.QueryAsync<object, ItemQuantityDto>(
                    new
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                    }, new SpOptions("[dbo].[S_Push_QuantityOnHand_tosite]", hasTotalCount: true));

                if (!firstSetTotal)
                {
                    totalCount = resultListQuantity.TotalCount ?? 0;
                    firstSetTotal = true;
                    pageEndIndex = (int)Math.Ceiling((double)totalCount / pageSize);
                }

                var reqDto = new SiteBulkUpdateReqDto<ItemQuantityDto>();
                reqDto.variants = resultListQuantity.Value?.ToList() ?? [];

                if (reqDto.variants.Count == 0)
                    break;

                //sending to david joes site 
                string urlpushtosite = $"accounting/bulk-update-stock";
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize(reqDto), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(urlpushtosite, httpContent);
                var content = await response.Content.ReadAsStringAsync();
                var node = JsonNode.Parse(content);
                var value = node.Deserialize<SiteBulkUpdateResDto>();

                if (value is null)
                    throw new Exception("response from website is null");

                if (value.failed_variant_ids is not null)
                {
                    var logResult = await dbservice.ExecuteAsync<object>(new
                    {
                        JsonFailedVariantIds = JsonSerializer.Serialize(value.failed_variant_ids),
                        FlagDescription = "push-update-stock"
                    }, new SpOptions("[dbo].[S_Log_FailedVariantIds_Insert]"));

                }
                pageIndex++;
            } while (pageIndex <= pageEndIndex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error occurred during pushing quantity on hand to website : {ex.Message}");
        }
    }
    #endregion
}

