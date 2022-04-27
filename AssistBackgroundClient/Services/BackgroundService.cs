using System;
using System.Threading.Tasks;
using ValNet;
using WebSocketSharp;

namespace AssistBackgroundClient.Services;

public class BackgroundService
{
    private DiscordPresenceService _discordPresence;
    private ValorantGameController _valorantGame = new ();
    private ValorantSocketService _socketService = new ();
    private RiotClientController _clientController = new();
    
    private const int PROCESSCOOLDOWN = 5000;
    public async Task StartService()
    {
        AssistLog.Debug("Starting Trying to find Valorant Process");
        await FindValorantProcess();
        
        // Connect to Riot Websocket
        await ConnectToWebsocket();
        
        AssistLog.Normal("Done!");
    }

    public async Task StartGame()
    {
        AssistLog.Normal("Start Game");
        await _clientController.StartClient();
        
    }
    private async Task ConnectToWebsocket()
    {
        try
        {
            await _socketService.ConnectValorantUser();
        }
        catch (Exception e)
        {
            AssistLog.Error(e.Message);
        }
        
        if (_socketService.ValorantUser == null || _socketService.ValorantUser.AuthType != AuthType.Socket)
        {
            await Task.Delay(PROCESSCOOLDOWN);
            await ConnectToWebsocket();
        }
        
        // Connection is successful
        _socketService.ValorantUser.UserWebsocket.OnMessage += delegate(object? sender, MessageEventArgs args) { AssistLog.Debug(args.Data); };
    }

    private async Task FindValorantProcess()
    {
        // Find Valorant Process
        try
        {
            await _valorantGame.ConnectToValorantGame();
        }
        catch (Exception e)
        {
            AssistLog.Error(e.Message);
            throw;
        }
        

        if (_valorantGame.ValorantGameProcess == null)
            await FindValorantProcess();
    }


    public async Task LogService()
    {
        if(_valorantGame.ValorantGameProcess != null)
            AssistLog.Debug($"Valorant Game Process Id: {_valorantGame.ValorantGameProcess.Id}");
        AssistLog.Debug("--------------");
        if(_clientController.RiotClientProcess != null)
            AssistLog.Debug($"Riot Client Process Id: {_clientController.RiotClientProcess.Id}");
        if(_valorantGame.ValorantGameProcess.HasExited)
            AssistLog.Debug($"Valorant Game Process is Exited");
    }
}