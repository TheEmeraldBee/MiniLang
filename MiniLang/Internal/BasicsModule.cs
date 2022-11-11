using MiniLang.Core;

namespace MiniLang.Internal;

public class BasicsModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            // Changing
            case '>':
                engine.Idx += engine.GetNumAfter() ?? 1;
                break;
            case '<':
                engine.Idx -= engine.GetNumAfter() ?? 1;
                break;
            case '+':
                engine.Program[engine.Idx] += engine.GetNumAfter() ?? 1;
                break;
            case '-':
                engine.Program[engine.Idx] -= engine.GetNumAfter() ?? 1;
                break;
            
            // Comment
            case '{':
                while (engine.CurrentCommand != '}')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected comment end, but did not find it.");
                    }
                }
                break;
        }

        return new Result(true);
    }
}