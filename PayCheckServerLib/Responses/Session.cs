using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Session
    {
        [HTTP("GET", "/session/v1/public/namespaces/pd3beta/users/me/attributes")]
        public static bool GETSessionAttributes(HttpRequest _, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            ResponseCreator response = new();
            AttribSuccess success = new()
            {
                CrossplayEnabled = true,
                CurrentPlatform = token.PlatformType.ToString().ToUpper(),
                Namespace = "pd3beta",
                Platforms = new()
                {
                    new()
                    {
                        Name = token.PlatformType.ToString().ToUpper(),
                        UserId = token.PlatformId
                    }
                },
                UserId = token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("POST", "/session/v1/public/namespaces/pd3beta/users/me/attributes")]
        public static bool POSTSessionAttributes(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var req = JsonConvert.DeserializeObject<AttribRequest>(request.Body) ?? throw new Exception("POSTSessionAttributes -> req is null!");
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            AttribSuccess success = new()
            {
                CrossplayEnabled = req.CrossplayEnabled,
                CurrentPlatform = req.CurrentPlatform,
                Namespace = "pd3beta",
                Platforms = req.Platforms,
                UserId = token.UserId
            };
            response.SetBody(JsonConvert.SerializeObject(success));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/pd3beta/users/me/parties")]
        public static bool SessionsParties(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            Challenges challenges = new()
            {
                Paging = new()
                {
                    First = "",
                    Last = "",
                    Previous = "",
                    Next = ""
                },
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(challenges));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/session/v1/public/namespaces/pd3beta/users/me/gamesessions")]
        public static bool Sessionsgamesessions(HttpRequest _, PC3Server.PC3Session session)
        {
            ResponseCreator response = new();
            response.SetHeader("Content-Type", "application/json");
            Challenges challenges = new()
            {
                Paging = new()
                {
                    First = "",
                    Last = "",
                    Previous = "",
                    Next = ""
                },
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(challenges));
            session.SendResponse(response.GetResponse());
            return true;
        }

        /*
        [HTTP("POST", "/session/v1/public/namespaces/pd3beta/party")]
        public static bool SessionsPartyPOST(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            var post = JsonConvert.DeserializeObject<PartyPostReq.Basic>(request.Body);

            PartyPost.Basic party = new()
            {
                Attributes = new()
                {
                    { "SESSIONTEMPLATENAME", post.Attributes["SESSIONTEMPLATENAME"] },
                    { "TEXTCHAT", post.Attributes["TEXTCHAT"] },
                    { "preference", new Dictionary<string, object>()
                        {
                            { "crossplayEnabled", true },
                            { "currentPlatform", "STEAM" }
                        }                     
                    }
                },
                Code = "YEEET", //  Make random code generation
                CreatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'FFFZ"),
                CreatedBy = "29475976933497845197035744456968",
                Id = UserIdHelper.CreateNewID(),
                IsActive = true,
                IsFull = false,
                LeaderId = "29475976933497845197035744456968",
                Namespace = "pd3beta",
                UpdatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'FFFZ"),
                Version = 1,
                Members = new()
                {
                },
                Configuration = new()
                {
                    AutoJoin = false,
                    DsSource = "",
                    Deployment = "",
                    ClientVersion = "0",
                    FallbackClaimKeys = new(),
                    TieTeamsSessionLifetime = false,
                    InactiveTimeout = 60,
                    InviteTimeout = 60,
                    Joinability = "INVITE_ONLY",
                    MaxPlayers = 4,
                    MinPlayers = 1,
                    Name = "PartySession",
                    Persistent = false,
                    PreferredClaimKeys = new(),
                    PsnBaseUrl = "https://s2s.sp-int.playstation.net",
                    TextChat = true,
                    Type = "NONE",
                    RequestedRegions = new()
                    {
                        "eu-north-1",
                        "eu-west-1",
                        "eu-central-1"
                    },
                    NativeSessionSetting = new()
                    {
                        PsnServiceLabel = 0,
                        PsnSupportedPlatforms = new(),
                        SessionTitle = "Payday3 Party",
                        ShouldSync = true,
                        XboxServiceConfigId = "00000000-0000-0000-0000-00006cbe5b43",
                        LocalizedSessionName = new()
                        {
                            DefaultLanguage = "en-US",
                            LocalizedText = new()
                            {
                                { "en-US", "Payday3 Party" }
                            }
                        },
                        XboxSessionTemplateName = "Payday3"
                    }
                }
            };

            foreach (var member in post.Members)
            {
                party.Members.Add(new PartyPost.Member()
                {
                    Id = member.ID,
                    PlatformId = member.PlatformId,
                    Status = "JOINED",
                    StatusV2 = "JOINED",
                    PlatformUserId = member.PlatformUserId,
                    UpdatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'FFFZ")
                });
            }

            response.SetBody(JsonConvert.SerializeObject(party));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("PATCH", "/session/v1/public/namespaces/pd3beta/parties/{partyId}")]
        public static bool SessionsPartiesPATCH(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            var post = JsonConvert.DeserializeObject<PartyPostReq.Basic>(request.Body);

            PartyPost.Basic party = new()
            {
                Attributes = new()
                {
                },
                Code = "YEEET", //  Make random code generation
                CreatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'FFFZ"),
                CreatedBy = "29475976933497845197035744456968",
                Id = UserIdHelper.CreateNewID(),
                IsActive = true,
                IsFull = false,
                LeaderId = "29475976933497845197035744456968",
                Namespace = "pd3beta",
                UpdatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'FFFZ"),
                Version = 1,
                Members = new()
                {
                },
                Configuration = new()
                {
                    AutoJoin = false,
                    DsSource = "",
                    Deployment = "",
                    ClientVersion = "0",
                    FallbackClaimKeys = new(),
                    TieTeamsSessionLifetime = false,
                    InactiveTimeout = 60,
                    InviteTimeout = 60,
                    Joinability = "INVITE_ONLY",
                    MaxPlayers = 4,
                    MinPlayers = 1,
                    Name = "PartySession",
                    Persistent = false,
                    PreferredClaimKeys = new(),
                    PsnBaseUrl = "https://s2s.sp-int.playstation.net",
                    TextChat = true,
                    Type = "NONE",
                    RequestedRegions = new()
                    {
                        "eu-north-1",
                        "eu-west-1",
                        "eu-central-1"
                    },
                    NativeSessionSetting = new()
                    {
                        PsnServiceLabel = 0,
                        PsnSupportedPlatforms = new(),
                        SessionTitle = "Payday3 Party",
                        ShouldSync = true,
                        XboxServiceConfigId = "00000000-0000-0000-0000-00006cbe5b43",
                        LocalizedSessionName = new()
                        {
                            DefaultLanguage = "en-US",
                            LocalizedText = new()
                            {
                                { "en-US", "Payday3 Party" }
                            }
                        },
                        XboxSessionTemplateName = "Payday3"
                    }
                }
            };

            foreach (var member in post.Members)
            {
                party.Members.Add(new PartyPost.Member()
                {
                    Id = member.ID,
                    PlatformId = member.PlatformId,
                    Status = "JOINED",
                    StatusV2 = "JOINED",
                    PlatformUserId = member.PlatformUserId,
                    UpdatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'FFFZ")
                });
            }

            response.SetBody(JsonConvert.SerializeObject(party));
            session.SendResponse(response.GetResponse());
            return true;
        }

        */
    }
}
