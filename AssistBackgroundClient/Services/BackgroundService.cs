using System.Threading.Tasks;
using ValNet;

namespace AssistBackgroundClient.Services;

public class BackgroundService
{
    private DiscordPresenceService _discordPresence;
    private ValorantGameController _valorantGame = new ();
    private ValorantSocketService _socketService = new ();
    private RiotClientController _clientController = new();
    

    public async Task StartService()
    {
        await FindValorantProcess();
        
        // Connect to Riot Websocket
        await ConnectToWebsocket();
        
        AssistLog.Normal("Done!");
    }

    public async Task StartGame()
    {
        AssistLog.Normal("Start Game");
        await _clientController.StartClient();
        AssistLog.Normal("Started Game");
    }
    private async Task ConnectToWebsocket()
    {
        await _socketService.ConnectValorantUser();

        if (_socketService.ValorantUser.AuthType != AuthType.Socket)
            await ConnectToWebsocket();
    }

    private async Task FindValorantProcess()
    {
        // Find Valorant Process
        await _valorantGame.ConnectToValorantGame();

        if (_valorantGame.ValorantGameProcess == null)
            await FindValorantProcess();
    }
}