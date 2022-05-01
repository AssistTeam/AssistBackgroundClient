using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AssistBackgroundClient.Models;
using DiscordRPC;

namespace AssistBackgroundClient.Services;

public class ValorantRpcController
{
    private static PrivateData userPrivateData;
    private BackgroundService _bgService => ApplicationService.BackgroundService;
    public static async Task ParsePresenseMessage(PresencesV4DataObj presence)
    {
        string non64 = Encoding.UTF8.GetString(Convert.FromBase64String(presence.data.presences[0].@private));

        userPrivateData = JsonSerializer.Deserialize<PrivateData>(non64);
        
        switch (userPrivateData.sessionLoopState)
        {
            case "MENUS":
                await CreateMenuStatus();
                break;
            case "PREGAME":
                await CreatePregameStatus();
                break;
            case "INGAME":
                await CreateIngameStatus();
                break;

        }
        
        
        
    }

    private static async Task CreateMenuStatus()
    {
        
    }
    
    private static async Task CreatePregameStatus()
    {
        
    }
    
    private static async Task CreateIngameStatus()
    {
        
    }
}