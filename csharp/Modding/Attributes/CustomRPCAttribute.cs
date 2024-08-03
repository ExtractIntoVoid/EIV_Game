using ExtractIntoVoid.Managers;
using System;

namespace ExtractIntoVoid.Modding;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class CustomRPCAttribute : Attribute
{
    public BuildType CallOnBuild { get; } = BuildDefined.Build;

    public CustomRPCAttribute()
    {

    }

    public CustomRPCAttribute(BuildType callOnBuild)
    {
        CallOnBuild = callOnBuild;
    }
}
