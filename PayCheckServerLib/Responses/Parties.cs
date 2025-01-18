using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons.Basic;
using PayCheckServerLib.Jsons.PartyStuff;
using PayCheckServerLib.Jsons.WSS;
using PayCheckServerLib.WSController;
using ModdableWebServer;
using ModdableWebServer.Attributes;

namespace PayCheckServerLib.Responses;

public class Parties
{
    [HTTP("POST", "/session/v1/public/namespaces/{namespace}/party")]
    public static bool Party(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
        var token = TokenHelper.ReadToken(auth);
        var body = JsonConvert.DeserializeObject<PartyPostReq>(request.Body);
        var rsp = PartyController.CreateParty(body, token.Namespace);
        ResponseCreator response = new();
        response.SetBody(JsonConvert.SerializeObject(rsp));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();

        //send notif to user to party created
        var wss_sess = LobbyControl.GetLobbyUser(token.UserId, token.Namespace);
        OnPartyCreated pld = new()
        {
            Code = rsp.Code,
            CreatedBy = rsp.CreatedBy,
            PartyId = rsp.Id,
            TextChat = body.TextChat
        };
        Dictionary<string, string> resp = new()
        {
            { "type", "messageSessionNotif" },
            { "topic", "OnPartyCreated" },
            { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(pld)) },
            { "sentAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }
        };

        LobbyControl.SendToLobby(resp, wss_sess);

        Chats.eventAddedToTopic topic = new()
        {
            Jsonrpc = "2.0",
            Method = "eventAddedToTopic",
            Params = new()
            {
                Name = "Party-" + rsp.Id,
                SenderId = token.UserId,
                TopicId = "p." + rsp.Id,
                Type = "GROUP",
                UserId = token.UserId
            }
        };

        ChatControl.SendToChat(JsonConvert.SerializeObject(topic), ChatControl.GetChatUser(token.UserId, token.Namespace));

        return true;
    }

    [HTTP("PATCH", "/session/v1/public/namespaces/{namespace}/parties/{partyid}")]
    public static bool PATCH_Parties(HttpRequest request, ServerStruct serverStruct)
    {
        var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
        var token = TokenHelper.ReadToken(auth);
        var body = JsonConvert.DeserializeObject<PartyPatch>(request.Body);
        PartyPost.Response rsp = PartyController.UpdateParty(serverStruct.Parameters["partyid"], body);
        ResponseCreator response = new();
        response.SetBody(JsonConvert.SerializeObject(rsp, Formatting.Indented));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        OnPartyUpdated pld = new()
        {
            Code = rsp.Code,
            CreatedBy = rsp.CreatedBy,
            Attributes = rsp.Attributes,
            Configuration = new()
            {
                ClientVersion = rsp.Configuration.ClientVersion,
                Joinability = rsp.Configuration.Joinability,
                InactiveTimeout = rsp.Configuration.InactiveTimeout,
                InviteTimeout = rsp.Configuration.InviteTimeout,
                MaxPlayers = rsp.Configuration.MaxPlayers,
                MinPlayers = rsp.Configuration.MinPlayers,
                Name = rsp.Configuration.Name,
                Type = rsp.Configuration.Type,
                RequestedRegions = rsp.Configuration.RequestedRegions
            },
            ConfigurationName = rsp.Configuration.Name,
            CreatedAt = rsp.CreatedAt,
            Id = rsp.Id,
            IsFull = rsp.IsFull,
            LeaderId = rsp.LeaderId,
            Members = new(),
            Namespace = serverStruct.Parameters["namespace"],
            UpdatedAt = rsp.UpdatedAt,
            Version = rsp.Version
        };
        foreach (var member in rsp.Members)
        {
            pld.Members.Add(new()
            {
                ID = member.Id,
                Status = member.Status,
                StatusV2 = member.StatusV2,
                PlatformID = member.PlatformId,
                PlatformUserID = member.PlatformUserId,
                UpdatedAt = member.UpdatedAt
            });
        }
        Dictionary<string, string> resp = new()
        {
            { "type", "messageSessionNotif" },
            { "topic", "OnPartyUpdated" },
            { "payload", LobbyControl.Base64Encode(JsonConvert.SerializeObject(pld)) },
            { "sentAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }
        };


        List<string> ids = new();
        rsp.Members.ForEach(m => ids.Add(m.Id));

        foreach (var id in LobbyControl.LobbyUsers.Keys)
        {
            var splitted = id.Split("_");

            if (ids.Contains(splitted[1]))
            {
                Debugger.PrintDebug(splitted[1]);
                LobbyControl.SendToLobby(resp, LobbyControl.GetLobbyUser(splitted[1], token.Namespace));
            }
        }

        return true;
    }


    [HTTP("DELETE", "/session/v1/public/namespaces/{namespace}/parties/{partyid}/users/me/leave")]
    public static bool LeaveParties(HttpRequest request, ServerStruct serverStruct)
    {
        //This response sadly KILLING THE GAME (Even without emu)
        //  SBZ PLEASE FIX!
        var auth = serverStruct.Headers["authorization"].Replace("Bearer ", "");
        var token = TokenHelper.ReadToken(auth);
        PartyController.LeftParty(serverStruct.Parameters["partyid"], token.UserId, serverStruct);
        var party = PartyController.PartySaves.Where(x => x.Value.Id == serverStruct.Parameters["partyid"]).FirstOrDefault().Value;
        if (party == null)
        {
            Debugger.PrintError("NO Code???? WHAT THE FUCK");
            throw new Exception("Code is not exist in saved parties????");
        }
        Chats.eventAddedToTopic topic = new()
        {
            Jsonrpc = "2.0",
            Method = "eventRemovedFromTopic",
            Params = new()
            {
                Name = "Party-" + party.Id,
                SenderId = token.UserId,
                TopicId = "p." + party.Id,
                Type = "GROUP",
                UserId = token.UserId
            }
        };
        ChatControl.SendToChat(JsonConvert.SerializeObject(topic), ChatControl.GetChatUser(token.UserId, token.Namespace));
        ResponseCreator response = new(204);
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }


    [HTTP("POST", "/session/v1/public/namespaces/{namespace}/parties/{partyid}/users/me/join")]
    public static bool JoinParties(HttpRequest request, ServerStruct serverStruct)
    {
        var party = PartyController.PartySaves.Where(x => x.Value.Id == serverStruct.Parameters["partyid"]).FirstOrDefault().Value;
        if (party == null)
        {
            Debugger.PrintError("NO Code???? WHAT THE FUCK");
            throw new Exception("Code is not exist in saved parties????");
        }
        var rsp = PartyController.ParsePartyToRSP(party);
        ResponseCreator response = new();
        response.SetBody(JsonConvert.SerializeObject(rsp));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("POST", "/session/v1/public/namespaces/{namespace}/parties/{partyid}/users/me/reject")]
    public static bool RejectParties(HttpRequest request, ServerStruct serverStruct)
    {
        /*
        var body = JsonConvert.DeserializeObject<object>(request.Body);
        ResponseCreator response = new();
        response.SetBody(JsonConvert.SerializeObject(""));
        session.SendResponse(response.GetResponse());
        */
        return false;
    }

    public class UsersMeJoinCode
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    [HTTP("POST", "/session/v1/public/namespaces/{namespace}/parties/users/me/join/code")]
    public static bool JoinPartyByCode(HttpRequest request, ServerStruct serverStruct)
    {

        var body = JsonConvert.DeserializeObject<UsersMeJoinCode>(request.Body);
        if (PartyController.PartySaves.TryGetValue(body.Code, out var saved))
        {
            var rsp = PartyController.ParsePartyToRSP(saved);
            ResponseCreator response = new();
            response.SetBody(JsonConvert.SerializeObject(rsp));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
        }
        else
        {
            AttribError error = new()
            {
                Attributes = new Dictionary<string, string>()
                {
                    { "id", body.Code },
                    { "namespace", serverStruct.Parameters["namespace"] }
                },
                ErrorCode = 20041,
                ErrorMessage = $"No party with ID [{body.Code}] exists in namespace [{serverStruct.Parameters["namespace"]}]",
                Message = $"No party with ID [{body.Code}] exists in namespace [{serverStruct.Parameters["namespace"]}]",
                Name = "PartyNotFound"
            };
            ResponseCreator response = new(404);
            response.SetBody(JsonConvert.SerializeObject(error));
            serverStruct.Response = response.GetResponse();
            serverStruct.SendResponse();
        }

        return true;
    }

    [HTTP("POST", "/session/v1/public/namespaces/{namespace}/parties/{partyid}/invite")]
    public static bool InviteOtherPlayer(HttpRequest request, ServerStruct serverStruct)
    {
        /*
        var body = JsonConvert.DeserializeObject<object>(request.Body);
        ResponseCreator response = new();
        response.SetBody(JsonConvert.SerializeObject(""));
        serverStruct.Response = response.GetResponse();
        serverStruct.SendResponse();
        */
        return false;
    }
}
