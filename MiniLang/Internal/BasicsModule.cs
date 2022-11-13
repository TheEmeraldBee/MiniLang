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
                engine.MoveIdx(engine.GetNumAfter() ?? 1);
                break;
            case '<':
                engine.MoveIdx(-(engine.GetNumAfter() ?? 1));
                break;
            case '+':
                engine.Set(engine.Get() + (engine.GetNumAfter() ?? 1));
                break;
            case '-':
                engine.Set(engine.Get() - (engine.GetNumAfter() ?? 1));
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
    
    public Result HandleSkip(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            // Comment
            case '{':
                while (engine.CurrentCommand != '}')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected comment end, but did not find it.");
                    }
                }

                engine.MoveReader();
                
                break;
        }

        return new Result(true);
    }
}