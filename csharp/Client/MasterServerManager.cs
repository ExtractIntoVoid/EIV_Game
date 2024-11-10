using EIV_Common;
using EIV_Common.InfoJson;
using EIV_Common.Platform;
using ExtractIntoVoid.Managers;
using Newtonsoft.Json;
using System.Net.Http;

namespace ExtractIntoVoid.Client;

public class MasterServerManager
{
    public static string JWTToken => Authenticate();

    public static bool CanConnect => ConfigINI.Read<bool>(BuildDefined.INI, "MasterServer", "ConnectToMasterServer");

    public static string MasterServerIP => ConfigINI.Read(BuildDefined.INI, "MasterServer", "MasterServerURL");

    const string AuthURL = "/EIV_Master/Users/Authenticate";

    public static string Authenticate()
    {
        if (!CanConnect)
            return string.Empty;

        IPlatform platform = GameManager.Instance.Platform_Manager.Platform;
        IUser user = platform.GetUser();
        if (user == null)
            return string.Empty;

        HttpClient httpClient = new();
        UserInfoJson userInfoJSON = new()
        {
            Platform = platform.PlatformType,
            UserId = user.GetUserId(),
            Name = user.GetUserName(),
            Version = BuildDefined.Version.ToString(),
        };
        var rsp = httpClient.PostAsync(MasterServerIP + AuthURL, new StringContent(JsonConvert.SerializeObject(userInfoJSON))).Result;
        if (rsp.StatusCode != System.Net.HttpStatusCode.OK)
            return string.Empty;
        return rsp.Content.ReadAsStringAsync().Result;
    }
}
