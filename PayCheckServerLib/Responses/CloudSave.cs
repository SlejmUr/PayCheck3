using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class CloudSave
    {
        [HTTP("GET", "/cloudsave/v1/namespaces/pd3beta/records/title-data")]
        public static bool TitleData(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            TitleData title = new()
            {
                SetBy = "SERVER",
                CreatedAt = "2023-06-23T07:21:11.604Z",
                UpdatedAt = "2023-06-23T07:21:11.604Z",
                Key = "title-data",
                Namespace = "pd3beta",
                Value = new()
                {
                    { "TitleData", "My Fancy Title Data" }
                }
            };
            response.SetBody(JsonConvert.SerializeObject(title));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3beta/records/news-feed")]
        public static bool NewsFeed(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetBody(File.ReadAllText("./Files/NewsFeed.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3beta/records/infamy-translation-table")]
        public static bool infamytranslationtable(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            InfamyTranslationTable.Basic table = new()
            {
                Value = new()
                {
                    InfamyTranslationTable = new()
                    {
                    }
                }
            };
            var infamylist = JsonConvert.DeserializeObject<List<InfamyTranslationTable.CInfamyTranslationTable>>(File.ReadAllText("Files/BasicInfamyTable.json"));
            table.Value.InfamyTranslationTable = infamylist;
            response.SetBody(JsonConvert.SerializeObject(table));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("POST", "/cloudsave/v1/namespaces/pd3beta/records/bulk")]
        public static bool RecordsBulk(HttpRequest request, PC3Server.PC3Session session)
        {
            var req = JsonConvert.DeserializeObject<WeaponsTableREQ>(request.Body);
            ResponseCreator response = new ResponseCreator();

            WeaponsTable weaponsTable = new()
            {
                Data = new()
            };
            var weaponTranslationTables = JsonConvert.DeserializeObject<List<WeaponsTable.WeaponTranslationTable>>(File.ReadAllText("Files/BasicWeaponsTable.json"));
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

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3beta/records/meta-events")]
        public static bool MetaEvents(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetBody(File.ReadAllText("Files/MetaEvents.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3beta/records/security-firm-rotation")]
        public static bool securityfirmrotation(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetBody(File.ReadAllText("Files/FirmRotation.json"));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/cloudsave/v1/namespaces/pd3beta/users/{userId}/records/PlatformBlockedPlayerData")]
        public static bool PlatformBlockedPlayerData(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator(404);
            session.SendResponse(response.GetResponse());
            return true;
        }


        [HTTP("GET", "/cloudsave/v1/namespaces/pd3beta/users/{userId}/records/progressionsavegame")]
        public static bool progressionsavegameGET(HttpRequest request, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            var response = new ResponseCreator();
            if (SaveHandler.IsUserExist(userID))
            {
                Console.WriteLine("SAVEFILE EXIST!");
                var now = DateTime.UtcNow.ToString("o");
                var save = JsonConvert.DeserializeObject<ProgressionSave>(SaveHandler.ReadUserSTR(userID));
                ProgressionSaveRSP saveRSP = new()
                {
                    CreatedAt = now,
                    UpdatedAt = now,
                    UserId = userID,
                    Value = save
                };
                response.SetBody(JsonConvert.SerializeObject(saveRSP));
                session.SendResponse(response.GetResponse());
                Console.WriteLine("sent rsp with success!");
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

        [HTTP("POST", "/cloudsave/v1/namespaces/pd3beta/users/{userId}/records/progressionsavegame")]
        public static bool progressionsavegamePOST(HttpRequest request, PC3Server.PC3Session session)
        {
            var userID = session.HttpParam["userId"];
            SaveHandler.SaveUser(userID, request.BodyBytes);
            if (ConfigHelper.ServerConfig.Saves.SaveRequest)
                SaveHandler.SaveUser_Request(userID, request.Body);
            var now = DateTime.UtcNow.ToString("o");
            var save = JsonConvert.DeserializeObject<ProgressionSave>(request.Body);
            ProgressionSaveRSP saveRSP = new()
            {
                CreatedAt = now,
                UpdatedAt = now,
                UserId = userID,
                Value = save
            };
            ResponseCreator response = new ResponseCreator();
            response.SetBody(JsonConvert.SerializeObject(saveRSP));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}
