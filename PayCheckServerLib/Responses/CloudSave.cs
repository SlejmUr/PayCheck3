using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses
{
    public class CloudSave
    {
        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/title-data")]
        public static bool TitleData(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            TitleData title = new()
            {
                SetBy = "SERVER",
                CreatedAt = "2023-06-23T07:21:11.604Z",
                UpdatedAt = "2023-06-23T07:21:11.604Z",
                Key = "title-data",
                Namespace = session.HttpParam["namespace"],
                Value = new()
                {
                    { "TitleData", "My Fancy Title Data" }
                }
            };
            response.SetBody(JsonConvert.SerializeObject(title));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/news-feed")]
        public static bool NewsFeed(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            //todo:
            //  make into json C# and replace namespace
            response.SetBody(File.ReadAllText("./Files/NewsFeed.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/infamy-translation-table")]
        public static bool InfamyTranslationTable(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            //InfamyTranslationTable.Basic;
            TopLevel<InfamyTranslationTable.Value> table = new()
            {
                CreatedAt = "2023-06-27T12:18:00.00Z",
                UpdatedAt = "2023-06-27T12:18:00.00Z",
                Key = "infamy-translation-table",
                Namespace = session.HttpParam["namespace"],
                SetBy = "SERVER",
                Value = new()
                {
                    InfamyTranslationTable = new()
                    {
                    }
                }
            };
            var infamylist = JsonConvert.DeserializeObject<List<InfamyTranslationTable.CInfamyTranslationTable>>(File.ReadAllText("Files/BasicInfamyTable.json")) ?? throw new Exception("BasicInfamyTable is null!");
            table.Value.InfamyTranslationTable = infamylist;
            response.SetBody(JsonConvert.SerializeObject(table));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("POST", "/cloudsave/v1/namespaces/{namespace}/records/bulk")]
        public static bool RecordsBulk(HttpRequest request, PC3Server.PC3Session session)
        {
            var req = JsonConvert.DeserializeObject<WeaponsTableREQ>(request.Body) ?? throw new Exception("WeaponsTableREQ is null!");
            ResponseCreator response = new();
            WeaponsTable weaponsTable = new()
            {
                Data = new()
            };
            if (ConfigHelper.ServerConfig.InDevFeatures.UseBasicWeaponTable)
            {
                var weaponTranslationTables = JsonConvert.DeserializeObject<List<WeaponsTable.WeaponTranslationTable>>(File.ReadAllText("Files/BasicWeaponsTable.json")) ?? throw new Exception("BasicWeaponsTable is null!");
                foreach (var item in req.Keys)
                {
                    weaponsTable.Data.Add(new WeaponsTable.CData()
                    {
                        Key = item,
                        Value = new()
                        {
                            WeaponTranslationTable = weaponTranslationTables
                        }
                    });
                }
            }
            else
            {
                var WeaponTables = JsonConvert.DeserializeObject<List<WeaponsTable.CData>>(File.ReadAllText("Files/WeaponTables.json"));
                if (WeaponTables == null)
                {
                    throw new Exception("WeaponTables null!");
                }
                foreach (var item in WeaponTables)
                {
                    if (req.Keys.Contains(item.Key))
                    {
                        weaponsTable.Data.Add(item);
                    }
                }
            }

            response.SetBody(JsonConvert.SerializeObject(weaponsTable));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/meta-events")]
        public static bool MetaEvents(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            //todo:
            //  make into json C# and replace namespace
            response.SetBody(File.ReadAllText("Files/MetaEvents.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/security-firm-rotation")]
        public static bool SecurityFirmRotation(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            //todo:
            //  make into json C# and replace namespace
            response.SetBody(File.ReadAllText("Files/FirmRotation.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/PlatformBlockedPlayerData")]
        public static bool PlatformBlockedPlayerData(HttpRequest _, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            ResponseCreator response = new(404);
            ErrorMSG errorMSG = new()
            {
                ErrorCode = 18022,
                ErrorMessage = $"unable to get_player_record: player record not found, user ID: {userID}, key: PlatformBlockedPlayerData"
            };
            response.SetBody(JsonConvert.SerializeObject(errorMSG));
            session.SendResponse(response.GetResponse());
            return true;
        }


        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/progressionsavegame")]
        public static bool ProgressionsavegameGET(HttpRequest _, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            var response = new ResponseCreator();
            if (SaveHandler.IsUserExist(userID, session.HttpParam["namespace"]))
            {
                Progression.Basic? save = null;
                try
                {
                    save = Progression.Basic.FromJson(SaveHandler.ReadUserSTR(userID, session.HttpParam["namespace"]));
                    save.ProgressionSaveGame.LastTimeEventCheck = TimeHelper.GetEpochTime();
                }
                catch
                {
                    Debugger.PrintError("JSON cannot be serialized!");
                }
                var now = DateTime.UtcNow.ToString("o");
                ProgressionSaveRSP saveRSP = new()
                {
                    CreatedAt = DateTime.UtcNow.AddDays(-1).ToString("o"),
                    UpdatedAt = now,
                    UserId = userID,
                    Value = save,
                    IsPublic = false,
                    Key = "progressionsavegame",
                    Namespace = session.HttpParam["namespace"],
                    SetBy = "CLIENT"
                };
                response.SetBody(JsonConvert.SerializeObject(saveRSP, Progression.Converter.Settings));
                session.SendResponse(response.GetResponse());
                return true;
            }
            response = new ResponseCreator(404);
            ErrorMSG error = new()
            {
                ErrorCode = 18022,
                ErrorMessage = $"unable to get_player_record: player record not found, user ID: {userID}, key: progressionsavegame"
            };
            response.SetBody(JsonConvert.SerializeObject(error));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("POST", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/progressionsavegame")]
        public static bool ProgressionsavegamePOST(HttpRequest request, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            SaveHandler.SaveUser(userID, session.HttpParam["namespace"], request.BodyBytes);
            if (ConfigHelper.ServerConfig.Saves.SaveRequest)
                SaveHandler.SaveUser_Request(userID, session.HttpParam["namespace"], request.Body);

            Progression.Basic? save = null;
            try
            {
                save = Progression.Basic.FromJson(request.Body);
                save.ProgressionSaveGame.LastTimeEventCheck = TimeHelper.GetEpochTime();
            }
            catch
            {
                Debugger.PrintError("JSON cannot be serialized!");
            }
            var now = DateTime.UtcNow.ToString("o");
            ProgressionSaveRSP saveRSP = new()
            {
                CreatedAt = DateTime.UtcNow.AddDays(-1).ToString("o"),
                UpdatedAt = now,
                UserId = userID,
                Value = save,
                IsPublic = false,
                Key = "progressionsavegame",
                Namespace = session.HttpParam["namespace"],
                SetBy = "CLIENT"
            };
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(saveRSP, Progression.Converter.Settings));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}
