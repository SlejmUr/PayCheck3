using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Currency
    {
        [HTTP("GET", "/platform/public/namespaces/{namespace}/users/{userId}/wallets/{currency}")]
        public static bool GetUserCurrency(HttpRequest request, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            var currencyType = session.HttpParam["currency"];
            var nspace = session.HttpParam["namespace"];
            var currencySymbol = currencyType == "CRED" ? "CREDITS" : currencyType;
            // return fake data for now
            Debugger.PrintDebug(String.Format("{0}_{1}_{2}", nspace, userID, currencyType));
            ResponseCreator response = new ResponseCreator();
            int balance = 0;
            if (ConfigHelper.ServerConfig.InDevFeatures.GiveMeMoney > 0)
                balance = ConfigHelper.ServerConfig.InDevFeatures.GiveMeMoney;
            CurrencyJson currencyReponse = new()
            {
                Balance = balance,
                CurrencyCode = currencyType,
                CurrencySymbol = currencySymbol,
                Id = String.Format("{0}_{1}_{2}", nspace, userID, currencyType),
                Namespace = nspace,
                Status = "ACTIVE",
                UserId = userID,
                WalletInfos = new()
                {
                    new()
                    {
                        Balance = balance,
                        BalanceOrigin = "System",
                        CreatedAt = "2023-08-05T03:23:16.598Z",
                        CurrencyCode = currencyType,
                        CurrencySymbol = currencySymbol,
                        Id = "8ab9cfab89c2807f0189c3b882f659c6",
                        Namespace = nspace,
                        Status = "ACTIVE",
                        TimeLimitedBalances = new(),
                        TotalPermanentBalance = balance,
                        TotalTimeLimitedBalance = 0,
                        UpdatedAt = "2023-08-05T03:23:16.612Z",
                        UserId = userID
                    }
                },
                WalletStatus = "ACTIVE"
            };
            response.SetBody(JsonConvert.SerializeObject(currencyReponse));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}
