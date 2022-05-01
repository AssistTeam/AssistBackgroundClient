using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AssistBackgroundClient.Settings;

namespace AssistBackgroundClient.Services;

public class RiotClientController
{
    private string _riotClientPath;
    public Process RiotClientProcess;
    
    public async Task StartClient()
    {
        AssistLog.Debug("Finding Client");
        try
        {
            _riotClientPath = await FindRiotClient();
            if (_riotClientPath == null)
                throw new Exception("Riot Client was not found on computer.");
        }
        catch (Exception e)
        {
            AssistLog.Error(e.Message);
            return;
        }
        
        
        
        var clientLaunchArgs = new ProcessStartInfo
        {
            FileName = _riotClientPath,
            Arguments = CreateLaunchArgs(),
            UseShellExecute = true
        };
        
        AssistLog.Debug("Launching path: " + clientLaunchArgs.FileName);
        AssistLog.Debug("Launching Args: " + clientLaunchArgs.Arguments);
        
        RiotClientProcess = Process.Start(clientLaunchArgs);
    }
    async Task<string> FindRiotClient()
    {
        List<string> clients = new();

        string riotInstallPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Riot Games/RiotClientInstalls.json"); ;

        if (!File.Exists(riotInstallPath)) return null;

        JsonDocument config;
        try
        {
            config = JsonDocument.Parse(File.ReadAllText(riotInstallPath));
        }
        catch (Exception e)
        {
            AssistLog.Error("Riot Client Check: Could not properly parse json file");
            return null;
        }
        
        if (config.RootElement.TryGetProperty("rc_default", out JsonElement rcDefault)) { clients.Add(rcDefault.GetString()); }
        if (config.RootElement.TryGetProperty("rc_live", out JsonElement rcLive)) { clients.Add(rcLive.GetString()); }
        if (config.RootElement.TryGetProperty("rc_beta", out JsonElement rcBeta)) { clients.Add(rcBeta.GetString()); }
        
        foreach (var clientPath in clients)
        {
            AssistLog.Debug("Found Client: " + clientPath);
            if (File.Exists(clientPath))
                return clientPath;
        }
        AssistLog.Debug("Did not find client");
        return null;
    }
    string CreateLaunchArgs()
    {
        // Check if Hidden is Running
        return $"--launch-product=valorant --launch-patchline={ApplicationSettings.ValPatchline}";
    }
    static internal async Task<IEnumerable<Process>> GetCurrentRiotProcesses() {
        var processlist = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Where(process => process.Id != Process.GetCurrentProcess().Id).ToList();
        processlist.AddRange(Process.GetProcessesByName("VALORANT-Win64-Shipping"));
        processlist.AddRange(Process.GetProcessesByName("RiotClientServices"));
        return processlist;
    }
}