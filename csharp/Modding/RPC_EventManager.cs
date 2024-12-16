using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EIV_JsonLib;
using ExtractIntoVoid.Managers;
using Godot;
using ModAPI;

namespace ExtractIntoVoid.Modding;

public class RPC_EventManager
{
    public static List<MethodInfo> RPC_Methods = new();

    public static void Load()
    {
        foreach (var item in MainLoader.Mods)
        {
            LoadAssembly(item);
        }
    }

    public static void LoadAssembly(Assembly assembly)
    {
        BuildType build = BuildDefined.Build;
        foreach (var item in assembly.GetTypes())
        {
            var custom_rpc_methods = item.GetMethods().Where(method => method.GetCustomAttribute<CustomRPCAttribute>() != null && method.IsStatic);
            //  filtering only this build call.
            custom_rpc_methods = custom_rpc_methods.Where(method => method.GetCustomAttribute<CustomRPCAttribute>().CallOnBuild == build);
            RPC_Methods.AddRange(custom_rpc_methods);
        }
    }

    public static void CallMethod(string MethodToCall, Godot.Collections.Array<Variant> variants)
    {
        var methods = RPC_Methods.Where(x=>x.Name == MethodToCall).ToList();
        foreach (var item in methods)
        {
            var @delegate = Delegate.CreateDelegate(typeof(Action<List<Variant>>), item);
            var action = (Action<List<Variant>>)@delegate;
            action(variants.ToList());
        }
    }
}
