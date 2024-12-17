// o7 This is all legacy code from Origin Framework

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
using System;
using System.Collections.Generic;
using Godot;
using Serilog.Core;
using Serilog.Events;

namespace ExtractIntoVoid.Managers;

public static class ConsoleManager
{
    public static string Contents = "* Extract Into Void Developer Console *\nPress TAB for autocomplete.\n'help' to see all commands.\n";
    public static Godot.Input.MouseModeEnum CachedMouse = Godot.Input.MouseModeEnum.Visible;
   
    public static Node GetConsole()
    {
        var console = SceneManager.GetPackedScene("DevConsole").Instantiate() as DevConsole;
        return console;
    }
    private static Dictionary<string, Command> _commands = new();
    /// <summary>
    /// Registers a new Command.
    /// </summary>
    /// <param name="name">The name of the command</param>
    /// <param name="CFuncOrCVar">A CVar or CFunc Command</param>
    public static void Register(string name, Command CFuncOrCVar)
    {
        _commands.TryAdd(name, CFuncOrCVar);
    }
    public static bool TryGet(string name, out Command CFuncOrCVar)
    {
        return _commands.TryGetValue(name, out CFuncOrCVar);
    }
    public static Dictionary<string, Command> GetAll()
    {
        return new Dictionary<string, Command>(_commands);
    }
}
public abstract class Command 
{
    public string Description = "";

}
public class CommandContext
{
    public readonly DevConsole Console;
    public readonly Control Scene;
    public readonly string[] Args;

    public CommandContext(DevConsole console, string[] args)
    {
        Console = console;
        Scene = console.GetParent() as Control;
        Args = args;
    }
}
public class CFunc : Command
{
    private Action<CommandContext> _function;
    public CFunc(string description, Action<CommandContext> function)
    {
        Description = description;
        _function = function;
    }
    public void Call(CommandContext ctx)
    {
        _function.Invoke(ctx);
    }

}
public class CVar<T> : Command
{
    private Func<CommandContext, T> _getter;
    private Action<CommandContext, T> _setter;
    public CVar(string description, Func<CommandContext, T> getter, Action<CommandContext, T> setter)
    {
        Description = description;
        // Check at runtime if the type is one of the allowed types
        if (typeof(T) != typeof(int) && typeof(T) != typeof(float) && 
            typeof(T) != typeof(bool) && typeof(T) != typeof(string))
        {
            throw new InvalidOperationException("Invalid type. Only int, float, bool, and string are allowed.");
        }
        _getter = getter;
        _setter = setter;
    }
    public T Get(CommandContext ctx)
    {
        return _getter.Invoke(ctx);
    }
    public void Set(CommandContext ctx, T value)
    {
        _setter.Invoke(ctx, value);
    }

}
public class ConsoleSink : ILogEventSink
{

    public void Emit(LogEvent logEvent)
    {
        ConsoleManager.Contents += logEvent.RenderMessage();
    }
}