using DavidJones.Core.Models.CreateProducts;
using DavidJones.Core.Models.DapperDriver;
using DavidJones.Core.Models.ProductPusher;
using DavidJones.Core.Models.UpdateProducts;
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

public class SiteCreateProductService
{
    public async Task FetchProductsFromFileAsBillToSite(string txtFilePath, IDapperDriver dbService, HttpClient httpClientBill, HttpClient httpClientSite)
    {
        //if (!Directory.Exists(txtFilePath))
        //    throw new Exception("file is not exist check file address");

        List<string> productIds = new List<string>(File.ReadAllLines(txtFilePath));

        await PropareContextForInsert(dbService);

        foreach (string id in productIds)
        {
            string urlGetItemBycode = $"itemqoh?inputcode={id}";
            var response = await httpClientBill.GetAsync(urlGetItemBycode);
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
            Console.WriteLine($"product Variant added successfully with id : {id}");

        }

        //convert itemqoh to item and heading and title 
        await ItemQohToProdcutItem(dbService);

        ////asign categury to productVariantItem 
        await ItemInitCateguryCodes(dbService, httpClientBill);

        ////pulling images
        await PullImagesBillApi(dbService, httpClientBill);

        //push to site
        await PushCreateProductsToSite(dbService, httpClientSite);
        //pull site product and insert into tmp table 
        //
    }

    private async Task ItemInitCateguryCodes(IDapperDriver dbservice, HttpClient httpClient)
    {
        var itemInqResult = await dbservice.QueryAsync<object, ItemQuantityOnHandDto>(new { }, new SpOptions("[dbo].[S_itemqoh_ItemBarcode_list]"));


        foreach (var item in itemInqResult.Value ?? [])
        {
            string urlGetItemBycode = $"itemlist?itembarcode={item.ItemBarcode}&count=100";
            var response = await httpClient.GetAsync(urlGetItemBycode);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node?["Value"];

            var dbResult = await dbservice.ExecuteAsync<object>(new
            {
                JsonItemQuantityOnHands = value?.ToString()

            }, new SpOptions("[dbo].[S_itemqoh_ItemCategury_Insert]"));
        }
        Console.WriteLine("completed Item Init Categury Code process ");
    }
    private async Task PropareContextForInsert(IDapperDriver dbService)
    {
        var result = await dbService.ExecuteAsync<object>(new { }, new SpOptions("[dbo].[S_itemqoh_PropereContext_insertNext]"));
        Console.WriteLine("completed PropereContext process ");
    }
    private async Task ItemQohToProdcutItem(IDapperDriver dbService)
    {
        //convert itemqoh to
        //- BillItemqohVariantItem
        //- BillItemqohTableTitle
        //- BillItemqohTableHead
        var result = await dbService.ExecuteAsync<object>(new { }, new SpOptions("[dbo].[S_itemqoh_converter_toItem]"));
        Console.WriteLine("completed itemqoh converter toItem process ");
    }
    private async Task PullImagesBillApi(IDapperDriver dbservice, HttpClient httpClient)
    {
        var itemInqResult = await dbservice.QueryAsync<object, ItemimageDto>(new { }, new SpOptions("[dbo].[S_itemimage_ListParentId]"));

        foreach (var item in itemInqResult.Value ?? [])
        {
            string urlGetItemBycode = $"itemimage?parentid={item.ItemParentID}&count=100";
            var response = await httpClient.GetAsync(urlGetItemBycode);
            var content = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(content);
            var value = node?["Value"];

            var dbResult = await dbservice.ExecuteAsync<object>(new
            {
                JsonListItemImages = value?.ToString()
            }, new SpOptions("[dbo].[S_itemqoh_InsertItemImage]"));

            Console.WriteLine($"product Variant images successfully with id : {item.ItemParentID}");
        }
        Console.WriteLine("completed image pulling process ");
    }

    private async Task PushCreateProductsToSite(IDapperDriver dbService, HttpClient httpClient)
    {
        var ProductListResult = await dbService.QueryAsync<object, CreateProductListDto>(new { }, new SpOptions("[dbo].[S_Site_CreateProduct_List]"));


        List<ProductReqDto> PropereProducts = new List<ProductReqDto>();

        foreach (var product in ProductListResult.Value ?? [])
        {
            ProductReqDto prdItem = new();

            var variantListResult = await dbService.QueryAsync<object, CreateProductVariantListDto>(new
            {
                product.ItemCode,
            }, new SpOptions("[dbo].[S_Site_CreateProductVariant_List]"));

            var prd = new CreateProductDto() { name = product.ItemDisplayName ?? string.Empty, enabled = true };

            var variantList = variantListResult.Value ?? [];
            var variants = variantList.Select(x =>
            {

                var attributeColor = new CreateProductVariantAttributeItemDto
                {
                    name = "color",
                    value = new CreateVariantColorDto { value = x.ColorNamePersian, extra = x.ColorExtraCode, fieldType = "رنگ" }
                };
                return new CreateProdcutVariantItemDto
                {
                    price = x.Price
                    ,
                    enabled = true
                    ,
                    sort_index = x.SortIndex
                    ,
                    stock_number = x.StockNumber
                    ,
                    attributes = [attributeColor]
                };
            });
            prdItem.product = prd;
            prdItem.product_variants = variants.ToArray();
            prdItem.attributes = [new CreateProductAttributeItemDto { name = "رنگ", type = "string", attribute_type = "differentiator" }];

            PropereProducts.Add(prdItem);
        }
        Console.WriteLine("generation of list propereProduct is completed ");
        foreach (var prpProduct in PropereProducts ?? [])
        {
            string urlpushtosite = $"products";
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(prpProduct), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(urlpushtosite, httpContent);
            var content = await response.Content.ReadAsStringAsync();
            //var node = JsonNode.Parse(content);
            //var value = node.Deserialize<SiteUpdateProductResDto>();

            //if (value is null)
            //    throw new Exception("response from website is null");

            Console.WriteLine($"updating product with id : {prpProduct.product?.name ?? "empty unsuccess"} completed success");

            //if ((value.status is not null && value.status != 200)
            //    || (value.error_code != 0))
            //{
            //    var logResult = await dbService.ExecuteAsync<object>(new
            //    {
            //        JsonFailedVariantIds = JsonSerializer.Serialize(value.result?.product ?? new object { }),
            //        FlagDescription = "push-update-productColor"
            //    }, new SpOptions("[dbo].[S_Log_FailedVariantIds_Insert]"));

            //}
        }
    }

}
