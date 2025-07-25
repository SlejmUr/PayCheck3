﻿using ModdableWebServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons.PartyStuff;

namespace PayCheckServerLib.Helpers
{
    public class PartyController
    {

        public class SavedStuff
        {
            public Dictionary<string, object> Attributes;   //WHY I NEED TO SAVE THIS AAAAAAAAAAAAAAA
            public string Code;
            public string CreatedBy;
            public string Id;
            public string LeaderId;
            public long version;
            public List<PartyPost.Member> Members;
            public bool IsActive;
            public bool IsFull;
            public string SessionType;
            public string CreatedAt;
            public string UpdatedAt;
            public string NameSpace;
        }

        public static Dictionary<string, SavedStuff> PartySaves = new();

        public static PartyPost.Response ParsePartyToRSP(SavedStuff stuff)
        {
            PartyPost.Response rsp = new();
            rsp.Code = stuff.Code;
            rsp.Attributes = stuff.Attributes;
            rsp.Configuration = CloudSaveDataHelper.GetStaticData<PartyPost.Configuration>($"PartyConfigurations/{stuff.SessionType}Configuration.json");
            rsp.Id = stuff.Id;
            rsp.IsActive = stuff.IsActive;
            rsp.IsFull = stuff.IsFull;
            rsp.LeaderId = stuff.LeaderId;
            rsp.CreatedBy = stuff.CreatedBy;
            rsp.Namespace = stuff.NameSpace;
            rsp.Version = stuff.version;
            rsp.CreatedAt = stuff.CreatedAt;
            rsp.UpdatedAt = stuff.UpdatedAt;
            rsp.Members = stuff.Members;
            return rsp;
        }


        public static PartyPost.Response CreateParty(PartyPostReq partyPost, string nameSpace)
        {
            var code = UserIdHelper.CreateCode();

            PartyPost.Response rsp = new();
            rsp.Code = code;
            rsp.Attributes = partyPost.Attributes;
            rsp.Configuration = CloudSaveDataHelper.GetStaticData<PartyPost.Configuration>($"PartyConfigurations/{partyPost.ConfigurationName}Configuration.json");
            rsp.Id = UserIdHelper.CreateNewID();
            rsp.IsActive = true;
            rsp.IsFull = false;
            rsp.LeaderId = partyPost.Members[0].ID;
            rsp.CreatedBy = partyPost.Members[0].ID;
            rsp.Namespace = nameSpace;
            rsp.Version = 1;    //init version 1
            rsp.CreatedAt = DateTime.UtcNow.ToString("o");
            rsp.UpdatedAt = DateTime.UtcNow.ToString("o");
            rsp.Members = new();
            foreach (var item in partyPost.Members)
            {
                rsp.Members.Add(new()
                {
                    Id = item.ID,
                    PlatformId = item.PlatformId.ToUpper(),
                    PlatformUserId = item.PlatformUserId,
                    Status = "JOINED",
                    StatusV2 = "JOINED",
                    UpdatedAt = DateTime.UtcNow.ToString("o")
                });
            }

            SavedStuff saved = new()
            {
                Code = code,
                SessionType = partyPost.ConfigurationName,
                CreatedAt = rsp.CreatedAt,
                CreatedBy = rsp.CreatedBy,
                Id = rsp.Id,
                IsActive = rsp.IsActive,
                IsFull = rsp.IsFull,
                LeaderId = rsp.LeaderId,
                Members = rsp.Members,
                UpdatedAt = rsp.UpdatedAt,
                version = rsp.Version,
                NameSpace = nameSpace,
                Attributes = rsp.Attributes
            };
            PartySaves.Add(code, saved);
            Debugger.PrintInfo("New Party made!");
            return rsp;
        }


        public static PartyPost.Response JoinParty(string Code, TokenHelper.Token token)
        {
            if (!PartySaves.ContainsKey(Code))
            {
				return null;
            }
            var stuff = PartySaves[Code];
            stuff.UpdatedAt = DateTime.UtcNow.ToString("o");
            var user = stuff.Members.Where(x => x.Id == token.UserId).First();
            //shinenigans too edit user in members
            stuff.Members.Remove(user);
            user.Status = "JOINED";
            user.StatusV2 = "JOINED";
            user.UpdatedAt = DateTime.UtcNow.ToString("o");
            stuff.Members.Add(user);
            //stuff.version += 1;   //does this needed?
            return ParsePartyToRSP(stuff);
        }



        public static PartyPost.Response UpdateParty(string PartyId, PartyPatch body)
        {
            var party = PartySaves.Where(x => x.Value.Id == PartyId).FirstOrDefault().Value;
            if (party == null)
            {
				return null;
            }

            party.UpdatedAt = DateTime.UtcNow.ToString("o");
            party.Attributes = body.Attributes;
            party.version = (body.Version + 1);
            PartySaves[party.Code] = party;
            return ParsePartyToRSP(party);
        }

        public static void LeftParty(string PartyId, string UserId, ServerStruct serverStruct)
        {
            var party = PartySaves.Where(x => x.Value.Id == PartyId).FirstOrDefault().Value;
            if (party == null)
            {
				return;
            }

            party.UpdatedAt = DateTime.UtcNow.ToString("o");
            party.version += 1;
            foreach (var m in party.Members)
            {
                if (m.Id == UserId)
                {
                    m.Status = "LEFT";
                    m.StatusV2 = "LEFT";
                }
            }

			bool allMembersLeft = true;
			foreach(var m in party.Members)
			{
				if (m.StatusV2 == "LEFT")
					continue;
				allMembersLeft = false;
				break;
			}
			if(allMembersLeft)
			{
				PartySaves.Remove(party.Code);
			}
            //todo send other users update that we left!
            //or if we are the leader send this one dead and sent to leave everybody
        }

        public static string GetPartyWhereUserID(string UserId, string NameSpace)
        {
            var resp = PartySaves.Where(x => x.Value.NameSpace == NameSpace && x.Value.Members.Any(y => y.Id == UserId)).FirstOrDefault();
            return resp.Key;
        }
    }
}
