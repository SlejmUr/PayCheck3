using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace JsonRequester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("rsp");
            Console.WriteLine("Hello, World!");
            Auth.Init();
            //_  = JsonConvert.SerializeObject(Auth.LoginToken);
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/feature-toggle", "FeatureToggle");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/matchmaking", "Matchmaking");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/edgegap_beacons", "EdgeGapBeacons");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/title-data", "TitleData");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/infamy-translation-table", "InfamyTranslationTable");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/meta-events", "MetaEvents");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/time-based-player-content", "TimeBasedPlayerContent");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/dlc-entitlements", "DLC_Entitlements");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/client-configuration", "ClientConfiguration", false);
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/challenge-recommendations", "ChallengeRecommendations", false);
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/challenge-dailies", "ChallengeDailies");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/security-firm-rotation", "SecurityFirmRotation");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/reward-reduction", "rewardreduction");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/level-reward-diamonddistrict", "levelreward_diamonddistrict");
            SaveToFileResponse("/cloudsave/v1/namespaces/pd3/records/level-reward-branchbank", "levelreward_branchbank");
            /*
            foreach (var item in bulk_items)
            {
                SaveToFileResponse(Requests.GetBulkRecords(item), $"Bulk_{item}");
            }*/

            SaveToFileResponse("/platform/public/namespaces/pd3/items/byCriteria?limit=2147483647&includeSubCategoryItem=false", "Items_NoSub");
            SaveToFileResponse("/platform/public/namespaces/pd3/items/byCriteria?limit=2147483647&includeSubCategoryItem=true", "Items_WithSub");
            SaveToFileResponse($"/platform/public/namespaces/pd3/users/{Auth.LoginToken.UserId}/entitlements?limit=2147483647", "Entitlements", false);
            SaveToFileResponse($"/social/v1/public/namespaces/pd3/users/{Auth.LoginToken.UserId}/statitems?limit=100000", "Statitems");
            SaveToFileResponse($"/cloudsave/v1/namespaces/pd3/records/challenges?limit=2147483647", "ChallengeRecords");
            SaveToFileResponse($"/cloudsave/v1/namespaces/pd3/users/{Auth.LoginToken.UserId}/records/challenges", "CloudSave_Challenges", false);

            Console.ReadLine();
            List<UserStatItemsData> mainstat = JsonConvert.DeserializeObject<List<UserStatItemsData>>(File.ReadAllText("StatItems_pd3.json"));


            List<UserStatItemsData> secondary = JsonConvert.DeserializeObject<List<UserStatItemsData>>(File.ReadAllText("Statitems.json"));

            var missing = secondary.Where(x=> !mainstat.Any(y=> y.StatCode == x.StatCode)).ToList();
            File.WriteAllText("missing_stats.json", JsonConvert.SerializeObject(missing));
            var time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            foreach (var item in missing)
            {
                item.Value = 0;
                item.UpdatedAt = time;
                item.UserId = "UserId";
                item.Tags = null;
                mainstat.Add(item);
            }
            File.WriteAllText("StatItems_pd3.json", JsonConvert.SerializeObject(mainstat, Formatting.Indented));
            Console.WriteLine("Done");
            Console.ReadLine();
        }


        static void SaveToFileResponse(string url_end, string filename, bool clean = true)
        {
            var rsp = Requests.GetResponse(url_end);
            if (rsp == null)
            {
                Console.WriteLine("Response doesnt seem to be right: " + url_end);
                return;
            }
            if (clean)
                rsp = RemoveUserIdAndTimeClean(rsp);
            File.WriteAllText("rsp/" + filename + ".json", JsonConvert.SerializeObject(rsp, Formatting.Indented));
        }

        static void SaveToFileResponse(JObject? jobject, string filename)
        {
            if (jobject == null)
            {
                Console.WriteLine("Response doesnt seem to be right: " + filename);
                return;
            }
            jobject = RemoveUserIdAndTimeClean(jobject);
            File.WriteAllText("rsp/" + filename + ".json", JsonConvert.SerializeObject(jobject, Formatting.Indented));
        }

        static JObject RemoveUserIdAndTimeClean(JObject obj)
        {
            if (obj.ContainsKey("userId"))
            {
                obj["userId"] = "REPLACEME";
            }
            if (obj.ContainsKey("updatedAt"))
            {
                obj["updatedAt"] = "updatedattomakegitdiffseasiertoread";
            }
            if (obj.ContainsKey("updated_at"))
            {
                obj["updated_at"] = "updatedattomakegitdiffseasiertoread";
            }
            foreach (var item in obj)
            {
                if (item.Value != null)
                    checktoken(item.Value);

            }
            return obj;
        }

        static JToken checktoken(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                token = RemoveUserIdAndTimeClean((JObject)token);
            }
            if (token.Type == JTokenType.Array)
            {
                var array = ((JArray)token).ToArray();
                for (int j = 0; j < array.Length; j++)
                {
                    array[j] = checktoken(array[j]);
                }
            }
            return token;
        }

        static List<string> bulk_items = [
            "weapon-translation-table-assaultrifle-car4",
            "weapon-translation-table-shotgun-r880",
            "weapon-translation-table-marksman-r900s",
            "weapon-translation-table-smg-compact7",
            "weapon-translation-table-assaultrifle-ku59",
            "weapon-translation-table-smg-commando",
            "weapon-translation-table-marksman-a114",
            "weapon-translation-table-smg-pc9",
            "weapon-translation-table-assaultrifle-nwb9",
            "weapon-translation-table-shotgun-mosconi12c",
            "weapon-translation-table-assaultrifle-vf7s",
            "weapon-translation-table-shotgun-fsa12",
            "weapon-translation-table-smg-war45",
            "weapon-translation-table-marksman-fik22",
            "weapon-translation-table-assaultrifle-rg5",
            "weapon-translation-table-smg-atk7",
            "weapon-translation-table-lmg-mx63",
            "weapon-translation-table-assaultrifle-chs3",
            "weapon-translation-table-shotgun-m7p",
            "weapon-translation-table-pistol-s40",
            "weapon-translation-table-revolver-castigo44",
            "weapon-translation-table-pistol-stryk7",
            "weapon-translation-table-pistol-spm11",
            "weapon-translation-table-pistol-s403",
            "weapon-translation-table-revolver-bison",
            "weapon-translation-table-revolver-bullkick500",
            "weapon-translation-table-pistol-t32",
            "weapon-translation-table-pistol-pd5"
            ];
    }
}
