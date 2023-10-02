using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Helpers
{
    public class UserStatController
    {
        public static List<UserStatItemsData> GetStatItems()
        { 
            return JsonConvert.DeserializeObject<List<UserStatItemsData>>(File.ReadAllText($"Files/StatItems.json"));
        }


        public static bool IsStatCodeExist(string StatCode)
        {
            var statItems = GetStatItems();
            foreach (var stat in statItems)
            {
                if (stat.StatCode == StatCode)
                    return true;
            }
            Debugger.PrintWarn("Stat Code not exist! Please report this issue!");
            return false;
        }

        public static UserStatItemsData? GetStatBycode(string StatCode)
        {
            foreach (var item in GetStatItems())
            {
                if (item.StatCode == StatCode)
                    return item;
            }
            return null;
        }


        public static List<StatItemsBulkRsp> AddStat(List<StatItemsBulkReq> req, TokenHelper.Token token)
        {
            var stat = GetStat(token.UserId, token.Namespace);
            if (stat == null)
            {
                stat = new List<UserStatItemsData>();
            }
            List<StatItemsBulkRsp> resp = new();
            foreach (var item in req)
            {
                if (!IsStatCodeExist(item.StatCode))
                {
                    Debugger.PrintWarn($"Code NOT exist! ({item.StatCode}) We skip it.");
                    continue;
                }
                var itemstat = GetStatBycode(item.StatCode);
                if (itemstat == null)
                    continue;

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
            SaveStat(stat, token.UserId, token.Namespace);
            return resp;
        }



        public static void SaveStat(List<UserStatItemsData> stat, string UserId, string NameSpace)
        {
            if (!Directory.Exists("UsersStat")) { Directory.CreateDirectory("UsersStat"); }
            File.WriteAllText($"UsersStat/{NameSpace}_{UserId}.json", JsonConvert.SerializeObject(stat));
        }


        public static List<UserStatItemsData>? GetStat(string UserId, string NameSpace)
        {
            if (File.Exists($"UsersStat/{NameSpace}_{UserId}.json"))
            {
                return JsonConvert.DeserializeObject<List<UserStatItemsData>>(File.ReadAllText($"UsersStat/{UserId}.json"));
            }
            return null;
        }
    }
}
