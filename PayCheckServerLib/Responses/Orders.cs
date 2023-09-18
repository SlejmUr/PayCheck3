using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Orders
    {
        public static string GenOrderNumber()
        {
            var rand = new Random();
            string ret = "";
            for (int i = 0; i < 19; i++)
            {
                ret += rand.Next(0, 9);
            }
            return "O" + ret;
        }

        public static ItemDefinitionJson? GetItemFromId(string id)
        {
            var items = JsonConvert.DeserializeObject<DataPaging<ItemDefinitionJson>>(File.ReadAllText("Files/Items.json"))!.Data;
            foreach (var item in items)
            {
                if (item.ItemId == id)
                {
                    return item;
                }
            }
            return null;
        }

        [HTTP("POST", "/platform/public/namespaces/pd3/users/{userid}/orders")]
        public static bool UserOrders(HttpRequest request, PC3Server.PC3Session session)
        {
            //ResponseCreator response = new ResponseCreator();
            //OrdersJsonPayload payload = new() {
            //	Data = { },
            //	Paging = { }
            //};
            //response.SetBody(JsonConvert.SerializeObject(payload));
            //session.SendResponse(response.GetResponse());

            var body = JsonConvert.DeserializeObject<OrderPostBody>(request.Body) ?? throw new Exception("UserOrders -> body is null!");
            if (!Directory.Exists("Orders")) { Directory.CreateDirectory("Orders"); }
            File.WriteAllText($"Orders/{session.HttpParam["userid"]}_{body.ItemId}", request.Body);

            ResponseCreator response = new();

            ItemDefinitionJson? item = GetItemFromId(body.ItemId) ?? throw new Exception("GetItemFromId -> Item is null!");
            Order order = new()
            {
                OrderNo = GenOrderNumber(),
                Namespace = "pd3",
                UserId = session.HttpParam["userid"],
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
                    Namespace = "pd3",
                    Decimals = 0
                },
                // will be like this until i can confirm they are the same type ~HW12Dev
                ItemSnapshot = JsonConvert.DeserializeObject<Order.ItemSnapshotJson>(JsonConvert.SerializeObject(item))!,
                Region = body.Region,
                Language = body.Language,
                Status = "FULFILLED",
                CreatedTime = DateTime.UtcNow.ToString("o"),
                ChargedTime = DateTime.UtcNow.ToString("o"),
                FulfilledTime = DateTime.UtcNow.ToString("o"),
                ExpireTime = DateTime.UtcNow.ToString("o"),
                PaymentRemainSeconds = 0,
                TotalTax = 0,
                TotalPrice = body.Price,
                SubtotalPrice = body.Price,
                CreatedAt = DateTime.UtcNow.ToString("o"),
                UpdatedAt = DateTime.UtcNow.ToString("o")
            };
            response.SetBody(JsonConvert.SerializeObject(order));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}
