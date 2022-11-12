using MiniLang.Core;

namespace MiniLang.Internal;

public class TempVariableModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case 'T':
                var num = engine.GetNumAfter();
                if (num == null) { return new Result(false, "ERROR: Expected number after 'T' but was not found."); }
                engine.Set((int) num, false);
                break;
        }

        return new Result(true);
    }

    public Result HandleSkip(Engine engine)
    {
        return new Result(true);
    }
}