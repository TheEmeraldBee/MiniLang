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
                engine.Set(-engine.Get());
                break;
            case '$':
                engine.SetIdx(engine.GetNumAfter() ?? 0);
                break;
            case '*':
                engine.Set(engine.GetNumAfter() ?? 0);
                break;
        }

        return new Result(true);
    }
}