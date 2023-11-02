using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;
using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;

namespace PayCheckServerLib.Responses
{
    public class CloudSave
    {
        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/title-data")]
        public static bool TitleData(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            TitleData title = new()
            {
                SetBy = "SERVER",
                CreatedAt = "2023-06-23T07:21:11.604Z",
                UpdatedAt = "2023-06-23T07:21:11.604Z",
                Key = "title-data",
                Namespace = serverStruct.Parameters["namespace"],
                Value = new()
                {
                    { "TitleData", "My Fancy Title Data" }
                }
            };
            response.SetBody(JsonConvert.SerializeObject(title));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/news-feed")]
        public static bool NewsFeed(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            //todo:
            //  make into json C# and replace namespace
            response.SetBody(File.ReadAllText("./Files/NewsFeed.json"));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/infamy-translation-table")]
        public static bool InfamyTranslationTable(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            //InfamyTranslationTable.Basic;
            TopLevel<InfamyTranslationTable.Value> table = new()
            {
                CreatedAt = "2023-06-27T12:18:00.00Z",
                UpdatedAt = "2023-06-27T12:18:00.00Z",
                Key = "infamy-translation-table",
                Namespace = serverStruct.Parameters["namespace"],
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/challenge-recommendations")]
        public static bool challengerecommendations(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            //InfamyTranslationTable.Basic;
            TopLevel<ChallengeRecommendations> table = new()
            {
                CreatedAt = "2023-06-27T12:18:00.00Z",
                UpdatedAt = "2023-06-27T12:18:00.00Z",
                Key = "challenge-recommendations",
                Namespace = serverStruct.Parameters["namespace"],
                SetBy = "SERVER",
                Value = new()
                {
                    BlockArray = new()
                    {
                    }
                }
            };
            var blockarry = JsonConvert.DeserializeObject<List<ChallengeRecommendations.CBlockArray>>(File.ReadAllText("Files/ChallengeRecommendations.json")) ?? throw new Exception("ChallengeRecommendations is null!");
            table.Value.BlockArray = blockarry;
            response.SetBody(JsonConvert.SerializeObject(table));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/cloudsave/v1/namespaces/{namespace}/records/bulk")]
        public static bool RecordsBulk(HttpRequest request, ServerStruct serverStruct)
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
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/meta-events")]
        public static bool MetaEvents(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            //todo:
            //  make into json C# and replace namespace
            response.SetBody(File.ReadAllText("Files/MetaEvents.json"));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/security-firm-rotation")]
        public static bool SecurityFirmRotation(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator response = new();
            //todo:
            //  make into json C# and replace namespace
            response.SetBody(File.ReadAllText("Files/FirmRotation.json"));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/PlatformBlockedPlayerData")]
        public static bool PlatformBlockedPlayerData(HttpRequest _, ServerStruct serverStruct)
        {
            var userID = serverStruct.Parameters["userId"];
            ResponseCreator response = new(404);
            ErrorMSG errorMSG = new()
            {
                ErrorCode = 18022,
                ErrorMessage = $"unable to get_player_record: player record not found, user ID: {userID}, key: PlatformBlockedPlayerData"
            };
            response.SetBody(JsonConvert.SerializeObject(errorMSG));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/PlatformBackendSettingsData")]
        public static bool RecordsPlatformBackendSettingsData(HttpRequest _, ServerStruct serverStruct)
        {
            var userID = serverStruct.Parameters["userId"];
            var response = new ResponseCreator();
            if (SaveFileHandler.IsUserExist(userID, serverStruct.Parameters["namespace"], SaveFileHandler.SaveType.PlatformBackendSettingsData))
            {
                PlatformBackendSettingsData.PlatformBackendSettingsDataValue? save = null;
                try
                {
                    save = JsonConvert.DeserializeObject<PlatformBackendSettingsData.PlatformBackendSettingsDataValue>(SaveFileHandler.ReadUserSTR(userID, serverStruct.Parameters["namespace"], SaveFileHandler.SaveType.PlatformBackendSettingsData));
                }
                catch
                {
                    Debugger.PrintError("JSON cannot be serialized!");
                }
                var now = DateTime.UtcNow.ToString("o");
                PlatformBackendSettingsData saveRSP = new()
                {
                    CreatedAt = DateTime.UtcNow.AddDays(-1).ToString("o"),
                    UpdatedAt = now,
                    UserId = userID,
                    Value = save,
                    IsPublic = false,
                    Key = "PlatformBackendSettingsData",
                    Namespace = serverStruct.Parameters["namespace"],
                    SetBy = "CLIENT"
                };
                response.SetBody(JsonConvert.SerializeObject(saveRSP, Formatting.Indented));
                serverStruct.Response = response.GetResponse();
                serverStruct.SendResponse();
                return true;
            }
            response = new ResponseCreator(404);
            ErrorMSG error = new()
            {
                ErrorCode = 18022,
                ErrorMessage = $"unable to get_player_record: player record not found, user ID: {userID}, key: PlatformBackendSettingsData"
            };
            response.SetBody(JsonConvert.SerializeObject(error));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("PUT", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/PlatformBackendSettingsData")]
        public static bool PlatformBackendSettingsDataPUT(HttpRequest request, ServerStruct serverStruct)
        {
            var userID = serverStruct.Parameters["userId"];
            SaveFileHandler.SaveUser(userID, serverStruct.Parameters["namespace"], request.BodyBytes, SaveFileHandler.SaveType.PlatformBackendSettingsData);
            if (ConfigHelper.ServerConfig.Saves.SaveRequest)
                SaveFileHandler.SaveUser_Request(userID, serverStruct.Parameters["namespace"], request.Body, SaveFileHandler.SaveType.PlatformBackendSettingsData);

            PlatformBackendSettingsData.PlatformBackendSettingsDataValue? save = null;
            try
            {
                save = JsonConvert.DeserializeObject<PlatformBackendSettingsData.PlatformBackendSettingsDataValue>(SaveFileHandler.ReadUserSTR(userID, serverStruct.Parameters["namespace"], SaveFileHandler.SaveType.PlatformBackendSettingsData));
            }
            catch
            {
                Debugger.PrintError("JSON cannot be serialized!");
            }
            var now = DateTime.UtcNow.ToString("o");
            PlatformBackendSettingsData saveRSP = new()
            {
                CreatedAt = DateTime.UtcNow.AddDays(-1).ToString("o"),
                UpdatedAt = now,
                UserId = userID,
                Value = save,
                IsPublic = false,
                Key = "PlatformBackendSettingsData",
                Namespace = serverStruct.Parameters["namespace"],
                SetBy = "CLIENT"
            };
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(saveRSP, Formatting.Indented));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/progressionsavegame")]
        public static bool ProgressionsavegameGET(HttpRequest _, ServerStruct serverStruct)
        {
            var userID = serverStruct.Parameters["userId"];
            var response = new ResponseCreator();
            if (SaveFileHandler.IsUserExist(userID, serverStruct.Parameters["namespace"], SaveFileHandler.SaveType.progressionsavegame))
            {
                Progression.Basic? save = null;
                try
                {
                    save = Progression.Basic.FromJson(SaveFileHandler.ReadUserSTR(userID, serverStruct.Parameters["namespace"], SaveFileHandler.SaveType.progressionsavegame));
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
                    Namespace = serverStruct.Parameters["namespace"],
                    SetBy = "CLIENT"
                };
                response.SetBody(JsonConvert.SerializeObject(saveRSP, Progression.Converter.Settings));
                serverStruct.Response = response.GetResponse();
                serverStruct.SendResponse();
                return true;
            }
            response = new ResponseCreator(404);
            ErrorMSG error = new()
            {
                ErrorCode = 18022,
                ErrorMessage = $"unable to get_player_record: player record not found, user ID: {userID}, key: progressionsavegame"
            };
            response.SetBody(JsonConvert.SerializeObject(error));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }


        [HTTP("POST", "/cloudsave/v1/namespaces/{namespace}/users/{userId}/records/progressionsavegame")]
        public static bool ProgressionsavegamePOST(HttpRequest request, ServerStruct serverStruct)
        {
            var userID = serverStruct.Parameters["userId"];
            SaveFileHandler.SaveUser(userID, serverStruct.Parameters["namespace"], request.BodyBytes, SaveFileHandler.SaveType.progressionsavegame);
            if (ConfigHelper.ServerConfig.Saves.SaveRequest)
                SaveFileHandler.SaveUser_Request(userID, serverStruct.Parameters["namespace"], request.Body, SaveFileHandler.SaveType.progressionsavegame);

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
                Namespace = serverStruct.Parameters["namespace"],
                SetBy = "CLIENT"
            };
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(saveRSP, Progression.Converter.Settings));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
            return true;
        }
    }
}
