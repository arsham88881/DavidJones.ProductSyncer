using DavidJones.Core.Implementations;
using DavidJones.Core.Models.DapperDriver;
using DavidJones.Core.Models.ProductPusher;
using DavidJones.Core.Services.Business;
using DavidJones.ProductSyncer.Helpers.services;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Xml.Linq;

IDapperDriver dbService = new DapperDriverServiceSqlServer();

var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
                (HttpRequestMessage req, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors) => false,
};


var httpClientBill = new HttpClient(handler);
httpClientBill.BaseAddress = new Uri("http://192.168.1.102:8201/api/");
httpClientBill.DefaultRequestHeaders.Add("WEB_TOKEN", "77E6D74E-CC50-4801-9C17-CBDC9DE795E7");

var httpClientWebSite = new HttpClient(); ///api/v1/accounting/bulk-update-price
httpClientWebSite.BaseAddress = new Uri("https://davidjonesparis.ir/api/v1/");
httpClientWebSite.DefaultRequestHeaders.Add("X-API-KEY", "h%q#e6m8s1");

var services = new UpdateQohPriceService();
//var services = new UpdateProductService();
//var services = new SiteCreateProductService();
//var services = new SiteGetAllProductsService();

//await services.GetAllForSyncWithPushed(dbService, httpClientWebSite);

//await services.FetchProductsFromFileAsBillToSite("E:\\productSyncerDavidJons\\Docs\\productCodes\\importProduct2.txt" //"E:/productSyncerDavidJons/Docs/productCodes/import-products-from-1-to-50-and-perfumes.txt" ///"E:\\productSyncerDavidJons\\Docs\\productCodes\\davidJonsProductCode_1.txt"
//    , dbService
//    , httpClientBill
//    , httpClientWebSite);

//await services.PullThenPushColor(dbService, httpClientWebSite);



//Console.WriteLine("start updating product colors to web site");
//await services.PullThenPushColor(dbService, httpClientWebSite);
//Console.WriteLine("updating products colors to web site completed");


////update quantity on hand from bill api
await services.PullUpdateQuantityOnHand(dbService, httpClientBill);
Console.WriteLine("process update from bill api completed ");
Console.WriteLine("pushing to quantityOnhand started");
await services.PushUpdateQuantityOnHand(dbService, httpClientWebSite);
Console.WriteLine("pushing to quantityOnhand completed");
//////////////////////////////////////////////


// update price from bill api
//await services.PullUpdatePrice(dbService, httpClientBill);
//Console.WriteLine("process update price from bill api completed ");

//await services.PushUpdatePrice(dbService, httpClientWebSite);
//Console.WriteLine("pushing to price completed");








Console.ReadKey();







