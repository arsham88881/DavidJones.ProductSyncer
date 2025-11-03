using DavidJones.Core.Models.DapperDriver;
using DavidJones.Core.Models.ProductPusher;
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

public class ProductItemQohPullerService
{
    public async Task PullFromBillApi(IDapperDriver dbService, HttpClient httpClient)
    {
        var itemInqResult = await dbService.QueryAsync<object, ItemqohDto>(new { }, new SpOptions("[dbo].[S_itemqoh_ListInputcode]"));


        foreach (var item in itemInqResult.Value ?? [])
        {
            string urlGetItemBycode = $"itemqoh?inputcode={item.ProductVariantCode}";
            var response = await httpClient.GetAsync(urlGetItemBycode);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node?["Value"];
            content = '[' + value?.ToString() + ']';
            var table = value?["Table"]?.Deserialize<object>();

            var dbResult = await dbService.ExecuteAsync<object>(new
            {
                ResponseContent = content,
                Table = JsonSerializer.Serialize(table),
            }, new SpOptions("[dbo].[S_itemqoh_InsertItem]"));
            Console.WriteLine($"product Variant added successfully with id : {item.ProductVariantCode}");
        }

    }

    public async Task PullImagesBillApi(IDapperDriver dbservice, HttpClient httpClient)
    {
        var itemInqResult = await dbservice.QueryAsync<object, ItemimageDto>(new { }, new SpOptions("[dbo].[S_itemimage_ListParentId]"));


        foreach (var item in itemInqResult.Value ?? [])
        {
            string urlGetItemBycode = $"itemimage?parentid={item.ItemParentID}&count=100";
            var response = await httpClient.GetAsync(urlGetItemBycode);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node?["Value"];
            //var table = value?.Deserialize<object>();

            var dbResult = await dbservice.ExecuteAsync<object>(new
            {
                JsonListItemImages = value?.ToString() //JsonSerializer.Serialize(table),

            }, new SpOptions("[dbo].[S_itemqoh_InsertItemImage]"));

            Console.WriteLine($"product Variant images successfully with id : {item.ItemParentID}");
        }
    }
    public async Task PullItemQuantityOnHand(IDapperDriver dbservice, HttpClient httpClient)
    {
        var itemInqResult = await dbservice.QueryAsync<object, ItemQuantityOnHandDto>(new { }, new SpOptions("[dbo].[S_itemqoh_ItemQuantityOnHand]"));


        foreach (var item in itemInqResult.Value ?? [])
        {
            //string urlGetItemBycode = $"barcodeqoh?count=100&itembarcode={item.ItemBarcode}";
            string urlGetItemBycode = $"itemlist?itembarcode={item.ItemBarcode}&count=100";
            var response = await httpClient.GetAsync(urlGetItemBycode);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node?["Value"];
            //var table = value?.Deserialize<object>();

            var dbResult = await dbservice.ExecuteAsync<object>(new
            {
                JsonItemQuantityOnHands = value?.ToString() //JsonSerializer.Serialize(table),

            }, new SpOptions("[dbo].[S_itemqoh_ItemQuantityOnHand_Insert]"));

            Console.WriteLine($"product Variant images successfully with id : {item.ItemBarcode}");
        }
        Console.WriteLine("completed process ");
    }
    public async Task PullUpdateQuantityOnHand(IDapperDriver dbservice, HttpClient httpClient)
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
}
