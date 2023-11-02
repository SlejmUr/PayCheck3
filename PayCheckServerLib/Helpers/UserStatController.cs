using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Helpers
{
    public class UserStatController
    {
        public static List<UserStatItemsData> GetStatItems(string NameSpace)
        {
            return JsonConvert.DeserializeObject<List<UserStatItemsData>>(File.ReadAllText($"Files/StatItems_{NameSpace.ToLower()}.json"));
        }


        public static bool IsStatCodeExist(string StatCode, string NameSpace)
        {
            var statItems = GetStatItems(NameSpace);
            foreach (var stat in statItems)
            {
                if (stat.StatCode == StatCode)
                    return true;
            }
            Debugger.PrintWarn("Stat Code not exist! Please report this issue!");
            return false;
        }

        public static UserStatItemsData? GetStatBycode(string StatCode, string NameSpace)
        {
            foreach (var item in GetStatItems(NameSpace))
            {
                if (item.StatCode == StatCode)
                    return item;
            }
            return null;
        }


        public static List<StatItemsBulkRsp> AddStat(List<StatItemsBulkReq> req, TokenHelper.Token token)
        {
            var stat_file = SaveFileHandler.ReadUserSTR(token.UserId, token.Namespace, SaveFileHandler.SaveType.statitems);
            var stat = new List<UserStatItemsData>();
            if (stat_file == string.Empty)
            {
                stat = new List<UserStatItemsData>();
            }
            else
                stat = JsonConvert.DeserializeObject<List<UserStatItemsData>>(stat_file);

            List<StatItemsBulkRsp> resp = new();
            foreach (var item in req)
            {
                if (!IsStatCodeExist(item.StatCode, token.Namespace))
                {
                    Debugger.PrintWarn($"Code NOT exist! ({item.StatCode}) We skip it.");
                    continue;
                }
                var itemstat = GetStatBycode(item.StatCode, token.Namespace);
                if (itemstat == null)
                {
                    Debugger.PrintWarn($"STAT NOT exist! ({item.StatCode}) We skip it.");
                    continue;
                }

                var statItem = stat.Find(x => x.StatCode == item.StatCode);
                if (statItem == null)
                {
                    stat.Add(new()
                    {
                        StatCode = item.StatCode,
                        CreatedAt = TimeHelper.GetOTime(),
                        Namespace = token.Namespace,
                        StatName = itemstat.StatName,
                        Tags = itemstat.Tags,
                        UpdatedAt = TimeHelper.GetOTime(),
                        UserId = token.UserId,
                        Value = item.Inc
                    });
                }
                else
                {
                    statItem.Value += item.Inc;
                }
                statItem = stat.Find(x => x.StatCode == item.StatCode);
                if (statItem == null)
                {
                    Debugger.PrintWarn($"what the fuck.");
                }
                resp.Add(new()
                {
                    Details = new()
                    {
                        CurrentValue = statItem.Value
                    },
                    StatCode = item.StatCode,
                    Success = true
                });
            }
            SaveFileHandler.SaveUser(token.UserId, token.Namespace, JsonConvert.SerializeObject(stat), SaveFileHandler.SaveType.statitems);
            return resp;
        }
    }
}
