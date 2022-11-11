namespace MiniLang.Core;

public interface IModule
{
    // Returns True if command was handled.
    public Result HandleCommand(Engine engine);
}