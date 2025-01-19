using ModdableWebServer;
using ModdableWebServer.Attributes;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using PayCheckServerLib.ModdableWebServerExtensions;

namespace PayCheckServerLib.Responses;

public class Orders
{
    public static string GenOrderNumber()
    {
        var rand = new Random();
        string ret = "";
        for (int i = 0; i <= 19; i++)
        {
            ret += rand.Next(0, 9);
        }
        return "O" + ret;
    }

    [HTTP("POST", "/platform/public/namespaces/{namespace}/users/{userid}/orders")]
	[AuthenticationRequired("NAMESPACE:{namespace}:USER:{userId}:ORDER", AuthenticationRequiredAttribute.Access.CREATE)]
    public static bool CreateOrder(HttpRequest request, ServerStruct serverStruct)
    {
        var body = JsonConvert.DeserializeObject<OrderPostBody>(request.Body);
        //if (!Directory.Exists("Orders")) { Directory.CreateDirectory("Orders"); }
        //File.WriteAllText($"Orders/{serverStruct.Parameters["namespace"]}_{serverStruct.Parameters["userid"]}_{body.ItemId}", request.Body);

        ResponseCreator response = new();

		if(body == null)
		{
			return serverStruct.ReturnErrorHelper(ErrorHelper.Errors.ValidationError);
		}

        var ordernumber = GenOrderNumber();
		ItemDefinitionJson? item = UserEntitlementHelper.GetItemDefinitions().Find(item => item.ItemId == body.ItemId);

		if(item == null)
		{
			return serverStruct.ReturnErrorHelper(ErrorHelper.Errors.ItemDoesNotExistInNamespace);
		}

        Order order = new()
        {
            OrderNo = ordernumber,
            Namespace = serverStruct.Parameters["namespace"],
            UserId = serverStruct.Parameters["userid"],
            ItemId = item.ItemId,
            Sandbox = false,
            Quantity = body.Quantity,
            Price = body.Price,
            DiscountedPrice = body.DiscountedPrice,
            Tax = 0,
            Vat = 0,
            SalesTax = 0,
            PaymentProviderFee = 0,
            PaymentMethodFee = 0,
            Currency = new()
            {
                CurrencyCode = body.CurrencyCode,
                CurrencySymbol = body.CurrencyCode,
                // CASH = Cash, GOLD = CStacks
                CurrencyType = (body.CurrencyCode == "CASH" || body.CurrencyCode == "GOLD") ? "VIRTUAL" : "REAL",
                Namespace = serverStruct.Parameters["namespace"],
                Decimals = 0
            },
            // will be like this until i can confirm they are the same type ~HW12Dev
			// ItemSnapshot is not a required parameter ~HW12Dev 18/01/2025
            ItemSnapshot = JsonConvert.DeserializeObject<Order.ItemSnapshotJson>(JsonConvert.SerializeObject(item))!,
            Region = body.Region,
            Language = body.Language,
            Status = "FULFILLED",
            CreatedTime = DateTime.UtcNow.ToString("o"),
            ChargedTime = DateTime.UtcNow.AddMilliseconds(100).ToString("o"),
            FulfilledTime = DateTime.UtcNow.ToString("o"),
            //  this should fix buying attachments
            ExpireTime = DateTime.UtcNow.AddDays(1).ToString("o"),
            PaymentRemainSeconds = 0,
            TotalTax = 0,
            TotalPrice = body.Price,
            SubtotalPrice = body.Price,
            CreatedAt = DateTime.UtcNow.ToString("o"),
            UpdatedAt = DateTime.UtcNow.ToString("o"),
            PaymentOrderNo = "P" + ordernumber,
            PaymentProvider = "WALLET"
        };
        response.SetBody(JsonConvert.SerializeObject(order));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }
}
