using EIV_Common.Platform;
using ExtractIntoVoid.Modding.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ExtractIntoVoid.Managers;

public class PlatformManager
{
    protected List<string> PlatformNames = ["EIV_Platform.Free.dll", "EIV_Platform.Steam.dll", "EIV_Platform.Epic.dll"];

    public IPlatform Platform;

    public void Init()
    {
        if (Platform == null)
            GetDRM();
        Platform.Init();
    }

    public void GetDRM()
    {
        CreatePlatformEvent createPlatformEvent = new()
        {
            Platform = null
        };
        ModAPI.V2.V2Manager.TriggerEvent(createPlatformEvent);
        Platform = createPlatformEvent.Platform;
        if (Platform == null)
        {
            string thisLocation = typeof(PlatformManager).Assembly.Location;
            foreach (string item in PlatformNames)
            {
                string Platform_Location = Path.Combine(thisLocation, item);
                GameManager.Instance.logger.Verbose(Platform_Location);
                if (File.Exists(Platform_Location))
                {
                    AssemblyLoadContext assemblyLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()) ?? AssemblyLoadContext.Default;
                    Assembly asm = assemblyLoadContext.LoadFromAssemblyPath(Platform_Location);
                    var iType = asm.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IPlatform))).FirstOrDefault();
                    if (iType == null)
                        continue;
                    IPlatform platform = (IPlatform)Activator.CreateInstance(iType);
                    if (platform == null)
                        continue;
                    Platform = platform;
                }
            }
        }
        if (Platform == null)
        {
            Platform = new UnknownPlatform();
        }
    }
}
