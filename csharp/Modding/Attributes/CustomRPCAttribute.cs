using System;

namespace ExtractIntoVoid.Modding;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class CustomRPCAttribute : Attribute
{
    public BuildType CallOnBuild { get; } = BuildType.None;

    public CustomRPCAttribute()
    {

    }

    public CustomRPCAttribute(BuildType callOnBuild)
    {
        CallOnBuild = callOnBuild;
    }
}
