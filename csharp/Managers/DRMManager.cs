using EIV_Common.DRM;
using ExtractIntoVoid.Modding.DRM;
using System.Collections.Generic;

namespace ExtractIntoVoid.Managers;

public class DRMManager
{
    protected List<string> DRMNames = ["EIV_DRM.Free.dll", "EIV_DRM.Steam.dll", "EIV_DRM.Epic.dll"];

    public IDRM DRM;

    public void Init()
    {
        if (DRM == null)
            GetDRM();
        DRM.Init();
    }


    public void GetDRM()
    {
#if MODDABLE
        CreateDRMEvent createDRMEvent = new()
        { 
            DRM = null
        };
        ModAPI.V2.V2Manager.TriggerEvent(createDRMEvent);
        DRM = createDRMEvent.DRM;
#else
        string thisLocation = typeof(DRMManager).Assembly.Location;

        foreach (string item in DRMNames)
        {
            string DRM_Location = Path.Combine(thisLocation, item);
            GameManager.Instance.logger.Verbose(DRM_Location);
            if (File.Exists(DRM_Location))
            {
                AssemblyLoadContext assemblyLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()) ?? AssemblyLoadContext.Default;
                Assembly asm = assemblyLoadContext.LoadFromAssemblyPath(DRM_Location);
                var idrmType = asm.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IDRM))).FirstOrDefault();
                if (idrmType == null)
                    continue;
                IDRM drm = (IDRM)Activator.CreateInstance(idrmType);
                if (drm == null)
                    continue;
                DRM = drm;
            }
        }    
#endif
        if (DRM == null)
        {
            DRM = new UnknownDRM();
        }
    }
}
