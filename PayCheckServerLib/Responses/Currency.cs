using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses;

public class Currency
{
    [HTTP("GET", "/platform/public/namespaces/{namespace}/users/{userId}/wallets/{currency}")]
    public static bool GetUserCurrency(HttpRequest request, ServerStruct serverStruct)
    {
        var userID = serverStruct.Parameters["userId"];
        var currencyType = serverStruct.Parameters["currency"];
        var nspace = serverStruct.Parameters["namespace"];
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
        switch (currencyType)
        {
            case "CASH":
                currencyReponse.WalletInfos[0].Id = "8ab9a2588ab8676b018ab892a08804be";
                break;
            case "GOLD":
                currencyReponse.WalletInfos[0].Id = "8ab9b52b8abb77f5018abbafe969046d";
                break;
            default:
                break;
        }

        response.SetBody(JsonConvert.SerializeObject(currencyReponse));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }
}
