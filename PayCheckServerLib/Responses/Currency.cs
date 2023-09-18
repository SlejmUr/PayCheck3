using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Currency
    {
        [HTTP("GET", "/platform/public/namespaces/pd3/users/{userId}/wallets/{currency}")]
        public static bool GetUserCurrency(HttpRequest request, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            var currencyType = session.HttpParam["currency"];
            var currencySymbol = currencyType == "CRED" ? "CREDITS" : currencyType;
            // return fake data for now
            Debugger.PrintDebug(String.Format("pd3_{0}_{1}", userID, currencyType));
            ResponseCreator response = new ResponseCreator();
            CurrencyJson currencyReponse = new()
            {
                Balance = 10000000,
                CurrencyCode = currencyType,
                CurrencySymbol = currencySymbol,
                Id = String.Format("pd3_{0}_{1}", userID, currencyType),
                Namespace = "pd3",
                Status = "ACTIVE",
                UserId = userID,
                WalletInfos = new()
                {
                    new()
                    {
                        Balance = 100000,
                        BalanceOrigin = "System",
                        CreatedAt = "2023-08-05T03:23:16.598Z",
                        CurrencyCode = currencyType,
                        CurrencySymbol = currencySymbol,
                        Id = "8ab9cfab89c2807f0189c3b882f659c6",
                        Namespace = "pd3",
                        Status = "ACTIVE",
                        TimeLimitedBalances = new(),
                        TotalPermanentBalance = 100000,
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
