using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AssistBackgroundClient.Services;

public class ValorantGameController
{
    public Process ValorantGameProcess;
    private const int PROCESSCOOLDOWN = 5000;
    public async Task ConnectToValorantGame()
    {
        while (ValorantGameProcess == null)
        {
            AssistLog.Debug("Trying to find client.");
            await FindValorantProcess();
            
            if(ValorantGameProcess != null)
                break;
            
            await Task.Delay(PROCESSCOOLDOWN);
        }
    }
    public async Task FindValorantProcess()
    {
        var ps = await RiotClientController.GetCurrentRiotProcesses();
        var vProc = ps.Where(_p => _p.ProcessName.Contains("VALORANT-Win64-Shipping")).FirstOrDefault();
        
        if(vProc == null)
            return;
        

        ValorantGameProcess = vProc;
        ValorantGameProcess.EnableRaisingEvents = true;
        
        AssistLog.Debug("Found Valorant Process");
        AssistLog.Debug($"Process ID: {vProc.Id}");
        AssistLog.Debug($"Process Name: {vProc.ProcessName}");
        
        AssistLog.Normal("Found Valorant Game Process: " + ValorantGameProcess.Id);
    }
}