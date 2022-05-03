using System;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Message;

namespace AssistBackgroundClient.Services;

public class DiscordPresenceService
{
    private const string APPID = "925134832453943336";
    private DiscordRpcClient _client;
    private RichPresence _currentPresence;
    public bool BDiscordPresenceActive;
    public DateTime timeStart;
    
    public static Button[] clientButtons = {
        new() {Label = "Download Assist", Url = "https://github.com/HeyM1ke/Assist/"}
    };
    public DiscordPresenceService()
    {
        _client = new DiscordRpcClient(APPID);
    }
    public async Task Initalize()
    {
        _client.OnReady += delegate(object sender, ReadyMessage args) { AssistLog.Normal("Discord Presence Client Ready, User: " + args.User.Username); timeStart = DateTime.Now; };
        _currentPresence = new RichPresence {
            Buttons = clientButtons,
            Assets = new Assets()
            {
                LargeImageKey = "default",
                LargeImageText = "Powered By Assist"
            },
            Details = "Chilling",
            Party = new Party()
            {
                Max = 5,
                Size = 1
            },
            Secrets = null,
            State = "VALORANT",
        };
        _client.SetPresence(_currentPresence); 
        _client.Initialize();
        BDiscordPresenceActive = true;
    }
    public async Task UpdatePresence(RichPresence data)
    {
        _currentPresence = data;
        // Send Data to Discord
        _client.SetPresence(_currentPresence);
        _client.Invoke();
    }
    public async Task Deinitalize()
    {
        _client.Deinitialize();
        BDiscordPresenceActive = false;
        
    }

    public async Task Shutdown()
    {
        if (!_client.IsDisposed)
            _client.Dispose();

        if (_client.IsInitialized)
            _client.Deinitialize();

        _client = new(APPID);
        BDiscordPresenceActive = false;
    }
}