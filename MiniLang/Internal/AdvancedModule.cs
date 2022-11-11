using MiniLang.Core;

namespace MiniLang.Internal;

public class AdvancedModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            // Setting
            case '!':
                engine.Program[engine.Idx] = -engine.Program[engine.Idx];
                break;
            case '$':
                engine.Idx = engine.GetNumAfter() ?? 0;
                break;
            case '*':
                engine.Program[engine.Idx] = engine.GetNumAfter() ?? 0;
                break;
        }

        return new Result(true);
    }
}