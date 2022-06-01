using System;
using System.Threading.Tasks;
using ValNet;

namespace AssistBackgroundClient.Services;

public class ValorantSocketService
{
    public RiotUser ValorantUser;


    public async Task ConnectValorantUser()
    {
        try
        {
            var u = new RiotUser();
            var w = await u.Authentication.AuthenticateWithSocketCurl();
            if (w.bIsAuthComplete)
                ValorantUser = u;
        }
        catch (Exception e)
        {
            AssistLog.Error(e.Message);
        }
    }
}