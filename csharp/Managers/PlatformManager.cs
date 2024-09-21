using EIV_Common.Platform;
using ExtractIntoVoid.Modding.Platform;
using System.Collections.Generic;

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
#if MODDABLE
        CreatePlatformEvent createPlatformEvent = new()
        {
            Platform = null
        };
        ModAPI.V2.V2Manager.TriggerEvent(createPlatformEvent);
        Platform = createPlatformEvent.Platform;
#else
        string thisLocation = typeof(DRMManager).Assembly.Location;

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
#endif
        if (Platform == null)
        {
            Platform = new UnknownPlatform();
        }
    }
}
