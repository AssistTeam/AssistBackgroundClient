using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AssistBackgroundClient.Models;
using DiscordRPC;
using ValNet;
using WebSocketSharp;

namespace AssistBackgroundClient.Services;

public class BackgroundService
{
    private DiscordPresenceService _discordPresence = new ();
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
        
        // Start Discord RP
        if (Settings.ApplicationSettings.RPEnabled)
            await StartDiscordPresence();
        
        AssistLog.Normal("Done!");
    }
    public async Task StartGame()
    {
        AssistLog.Normal("Starting Game");
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
        _socketService.ValorantUser.UserWebsocket.OnMessage += HandleMessage;
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

    public async Task StartDiscordPresence()
    {
        if (_socketService.ValorantUser == null || _socketService.ValorantUser.AuthType != AuthType.Socket)
        {
            await ConnectToWebsocket();
        }
        
        if(_discordPresence.BDiscordPresenceActive == false)
            await _discordPresence.Initalize();
    }

        
    public async Task StopDiscordPresence()
    {
        if(_discordPresence.BDiscordPresenceActive)
            await _discordPresence.Shutdown();
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
    private async void HandleMessage(object? sender, MessageEventArgs e)
    {
        AssistLog.Debug("Message Recieved");
        if (!e.Data.Contains("/chat/v4/presences") || !e.Data.Contains(_socketService.ValorantUser.UserData.sub))
            return;

        if (_discordPresence.BDiscordPresenceActive)
        {
            AssistLog.Debug("Discord Message Recieved");
            var stuff = JsonSerializer.Deserialize<List<object>>(e.Data);
            var dataObj = stuff[stuff.Count - 1].ToString();
            var obj = JsonSerializer.Deserialize<PresencesV4DataObj>(dataObj);

            var p = await ValorantRpcController.ParsePresenseMessage(obj);

            AssistLog.Debug("Updating Pres");
            await UpdateCurrentPresence(p);
        }


    }
    public async Task UpdateCurrentPresence(RichPresence pres) => await _discordPresence.UpdatePresence(pres);

    public bool bHasValorantExited => _valorantGame.ValorantGameProcess.HasExited;
}