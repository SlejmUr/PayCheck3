using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PayCheckServerLib.Responses
{
    public class CloudSave
    {
        [HTTP("GET", "/cloudsave/v1/namespaces/pd3/records/title-data")]
        public static bool TitleData(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            TitleData title = new()
            {
                SetBy = "SERVER",
                CreatedAt = "2023-06-23T07:21:11.604Z",
                UpdatedAt = "2023-06-23T07:21:11.604Z",
                Key = "title-data",
                Namespace = "pd3",
                Value = new()
                {
                    { "TitleData", "My Fancy Title Data" }
                }
            };
            response.SetBody(JsonConvert.SerializeObject(title));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3/records/news-feed")]
        public static bool NewsFeed(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetBody(File.ReadAllText("./Files/NewsFeed.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3/records/infamy-translation-table")]
        public static bool InfamyTranslationTable(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            InfamyTranslationTable.Basic table = new()
            {
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

        [HTTP("POST", "/cloudsave/v1/namespaces/pd3/records/bulk")]
        public static bool RecordsBulk(HttpRequest request, PC3Server.PC3Session session)
        {
            var req = JsonConvert.DeserializeObject<WeaponsTableREQ>(request.Body) ?? throw new Exception("WeaponsTableREQ is null!");
            ResponseCreator response = new();

            WeaponsTable weaponsTable = new()
            {
                Data = new()
            };
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
            response.SetBody(JsonConvert.SerializeObject(weaponsTable));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3/records/meta-events")]
        public static bool MetaEvents(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetBody(File.ReadAllText("Files/MetaEvents.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3/records/security-firm-rotation")]
        public static bool SecurityFirmRotation(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetBody(File.ReadAllText("Files/FirmRotation.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3/users/{userId}/records/PlatformBlockedPlayerData")]
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


        [HTTP("GET", "/cloudsave/v1/namespaces/pd3/users/{userId}/records/progressionsavegame")]
        public static bool ProgressionsavegameGET(HttpRequest _, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            var response = new ResponseCreator();
            if (SaveHandler.IsUserExist(userID))
            {
                var now = DateTime.UtcNow.ToString("o");
                var save = JsonConvert.DeserializeObject<object>(SaveHandler.ReadUserSTR(userID)) ?? throw new Exception("save is null!");
                ProgressionSaveRSP saveRSP = new()
                {
                    CreatedAt = now,
                    UpdatedAt = now,
                    UserId = userID,
                    Value = save
                };
                response.SetBody(JsonConvert.SerializeObject(saveRSP));
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

        [HTTP("POST", "/cloudsave/v1/namespaces/pd3/users/{userId}/records/progressionsavegame")]
        public static bool ProgressionsavegamePOST(HttpRequest request, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            SaveHandler.SaveUser(userID, request.BodyBytes);
            if (ConfigHelper.ServerConfig.Saves.SaveRequest)
                SaveHandler.SaveUser_Request(userID, request.Body);
            var now = DateTime.UtcNow.ToString("o");
            var save = JsonConvert.DeserializeObject<object>(request.Body);
            if (save == null) throw new Exception("save is null!");
            ProgressionSaveRSP saveRSP = new()
            {
                CreatedAt = now,
                UpdatedAt = now,
                UserId = userID,
                Value = save
            };
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(saveRSP));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}
