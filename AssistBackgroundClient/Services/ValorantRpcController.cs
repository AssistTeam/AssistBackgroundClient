using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AssistBackgroundClient.Models;
using AssistBackgroundClient.Objects;
using DiscordRPC;

namespace AssistBackgroundClient.Services;

public class ValorantRpcController
{
    private static PrivateData userPrivateData;
    private static BackgroundService _bgService => ApplicationService.BackgroundService;
    public static async Task<RichPresence> ParsePresenseMessage(PresencesV4DataObj presence)
    {
        RichPresence pres = new RichPresence();
        string non64 = Encoding.UTF8.GetString(Convert.FromBase64String(presence.data.presences[0].@private));

        userPrivateData = JsonSerializer.Deserialize<PrivateData>(non64);
        
        switch (userPrivateData.sessionLoopState)
        {
            case "MENUS":
                pres = await CreateMenuStatus();
                break;
            case "PREGAME":
                pres = await CreatePregameStatus();
                break;
            case "INGAME":
                pres = await CreateIngameStatus();
                break;
        }
        
        return pres;
    }

    private static async Task<RichPresence> CreateMenuStatus()
    {
        var pres = await GetBasePresence();
        string currQueue = await DetermineQueueKey();
        switch (userPrivateData.partyState)
        {
            case "DEFAULT":
                pres.Details = $"Lobby - {currQueue}";
                break;
            case "MATCHMAKING":
                pres.Details = $"In Queue - {currQueue}"; // magic woo, Capitalizes first letter.
                break;
            default:
                pres.Details = "Lobby";
                break;
        }
        
        return pres;
    }
    
    private static async Task<RichPresence> CreatePregameStatus()
    {
        var pres = await GetBasePresence();
        
        string state = "Agent Select";

        string mapName = await DetermineMapKey();
        string currQueue = await DetermineQueueKey();
        string details;
        if (userPrivateData.partyState.Contains("CUSTOM_GAME"))
            details = "Custom Game || Agent Select";
        else
        {
            details = $"{currQueue} || Agent Select";
        }
        
        // Set Map Image
        pres.Assets = new Assets
        {
            LargeImageKey = mapName.ToLower(),
            LargeImageText = "Powered By Assist",
            SmallImageKey = $"rank_{userPrivateData.competitiveTier}",
            SmallImageText = $"{ValorantRanks.Ranks[userPrivateData.competitiveTier]}"
        };

        pres.Details = details;
        
        return pres;
    }
    
    private static async Task<RichPresence> CreateIngameStatus()
    {
        var pres = await GetBasePresence();
        string mapName = await DetermineMapKey();
        string currQueue = await DetermineQueueKey();
        string details;
        if (userPrivateData.partyState.Contains("CUSTOM_GAME"))
            details = $"Custom Game || {userPrivateData.partyOwnerMatchScoreAllyTeam}-{userPrivateData.partyOwnerMatchScoreEnemyTeam}";
        else if (userPrivateData.matchMap.Contains("Range"))
            details = $"Tapping Heads in the Range.";
        else
            details = $"{currQueue} || {userPrivateData.partyOwnerMatchScoreAllyTeam}-{userPrivateData.partyOwnerMatchScoreEnemyTeam}";
        
        // Set Map Image
        pres.Assets = new Assets
        {
            LargeImageKey = mapName.ToLower(),
            LargeImageText = "Powered By Assist",
            SmallImageKey = $"rank_{userPrivateData.competitiveTier}",
            SmallImageText = $"{ValorantRanks.Ranks[userPrivateData.competitiveTier]}"
        };
        
        pres.Details = details;
        
        return pres;
    }

    private static async Task<RichPresence> GetBasePresence()
    {
        Party.PrivacySetting privacy;
        if (userPrivateData.partyAccessibility.Equals("CLOSED"))
            privacy = Party.PrivacySetting.Private;
        else
            privacy = Party.PrivacySetting.Public;

        string partyState;
        if (privacy == Party.PrivacySetting.Public)
            partyState = "In an Open Party";
        else
            partyState = "In a Closed Party";
        
        return new RichPresence()
        {
            Assets = new Assets
            {
                LargeImageKey = "default",
                LargeImageText = "Powered By Assist",
                SmallImageKey = $"rank_{userPrivateData.competitiveTier}",
                SmallImageText = $"{ValorantRanks.Ranks[userPrivateData.competitiveTier]}"
            },
            Buttons = DiscordPresenceService.clientButtons,
            State = partyState,
            Party = new Party()
            {
                ID = userPrivateData.partyId,
                Max = userPrivateData.maxPartySize,
                Privacy = privacy,
                Size = userPrivateData.partySize
            }

        };
    }
    private static async Task<string> DetermineMapKey()
        {
            switch (userPrivateData.matchMap)
            {
                case "/Game/Maps/Ascent/Ascent":
                    return "Ascent";
                case "/Game/Maps/Bonsai/Bonsai":
                    return "Split";
                case "/Game/Maps/Canyon/Canyon":
                    return "Fracture";
                case "/Game/Maps/Duality/Duality":
                    return "Bind";
                case "/Game/Maps/Foxtrot/Foxtrot":
                    return "Breeze";
                case "/Game/Maps/Triad/Triad":
                    return "Haven";
                case "/Game/Maps/Port/Port":
                    return "Icebox";
                case "/Game/Maps/Poveglia/Range":
                    return "Range";
                default:
                    return "Unknown";
            }

        }
    private static async Task<string> DetermineQueueKey()
        {
            switch (userPrivateData.queueId)
            {
                case "ggteam":
                    return "Escalation";
                case "deathmatch":
                    return "Deathmatch";
                case "spikerush":
                    return "Spike Rush";
                case "competitive":
                    return "Competitive";
                case "unrated":
                    return "Unrated";
                case "oneforall":
                    return "Replication";
                case "onefa":
                    return "Replication";
                case "snowball":
                    return "Snowball Fight";
                default:
                    return "VALORANT";
            }

        }
}